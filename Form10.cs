using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using IExcel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApplication1
{
    public partial class Form10 : Form
    {
        private BindingSource bindingSource3 = new BindingSource();
        private BindingSource bindingSource4 = new BindingSource();
        public string disease;
        public Form10()
        {
            InitializeComponent();
            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#

            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT name FROM disease";
            OracleDataReader dr4 = cmd.ExecuteReader();
            while (dr4.Read())
            {
                var abc = dr4.GetValue(0);
                this.comboBox1.Items.Add(abc.ToString());
            }

            this.comboBox1.Sorted = true;
            this.comboBox1.Refresh();

            conn.Close();
        }
        public static double TTest(double[] x, double[] y, int n1, int n2)
        {

            double sumX = 0.0;
            double sumY = 0.0;
            for (int i = 0; i < n1; i++)
                sumX = sumX + x[i];
            for (int i = 0; i < n2; i++)
                sumY = sumY + y[i];

            double meanX = sumX / n1;
            double meanY = sumY / n2;
            double sumXminusMeanSquared = 0.0; // Calculate variances
            double sumYminusMeanSquared = 0.0;
            for (int i = 0; i < n1; i++)
                sumXminusMeanSquared = sumXminusMeanSquared + (x[i] - meanX) * (x[i] - meanX);
            for (int i = 0; i < n2; i++)
                sumYminusMeanSquared = sumYminusMeanSquared + (y[i] - meanY) * (y[i] - meanY);
            double varX = sumXminusMeanSquared / (n1 - 1);
            double varY = sumYminusMeanSquared / (n2 - 1);

            double top = (meanX - meanY);

            double sp = (((n1 - 1) * varX) + ((n2 - 1) * varY)) / (n1 + n2 - 2);

            double bot = Math.Sqrt((sp * (n1 + n2) / (n1 * n2)));
            double t = top / bot;
            return t;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#
            IExcel.Application appExl;
            appExl = new IExcel.Application();

            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ap.u_id, mn.exp FROM mrnaexpression mn INNER JOIN arrayprobe ap ON mn.pb_id=ap.pb_id  WHERE mn.s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN (SELECT s_id  FROM patientsample  WHERE p_id  IN (SELECT p_id  FROM diagnosis      WHERE ds_id IN (SELECT ds_id FROM disease        WHERE name = '" + disease + "'))))ORDER BY ap.u_id";
            OracleDataReader dr = cmd.ExecuteReader();
            OracleDataReader dr_temp = dr;
            int i = 0;
            double[] tempuid = new double[100000];
            double[] tempexp = new double[100000];
            Exp_UID[] dataexp = new Exp_UID[100000];
            int length1 = 0;
            int length2 = 0;

            while (dr.Read())
            {
                var abc = dr.GetValue(0);
                var abc1 = dr.GetValue(1);
                tempuid[i] = Convert.ToDouble(abc);
                tempexp[i] = Convert.ToDouble(abc1);

                dataexp[i] = new Exp_UID();
                dataexp[i].UID = Convert.ToInt32(abc);
                dataexp[i].EXP = Convert.ToInt32(abc1);

                i++;

            }
            length1 = i;
           



            int j = 0;
            double[] tempuid2 = new double[100000];
            double[] tempexp2 = new double[100000];
            Exp_UID[] dataexp2 = new Exp_UID[100000];

            OracleCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = "SELECT ap.u_id, mn.exp FROM mrnaexpression mn INNER JOIN arrayprobe ap ON mn.pb_id=ap.pb_id  WHERE mn.s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN (SELECT s_id  FROM patientsample  WHERE p_id  IN (SELECT p_id  FROM diagnosis      WHERE ds_id IN (SELECT ds_id FROM disease        WHERE name != '" + disease + "'))))ORDER BY ap.u_id";
            OracleDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                var abc = dr2.GetValue(0);
                tempuid2[j] = Convert.ToDouble(abc);
                var abc1 = dr2.GetValue(1);
                tempexp2[j] = Convert.ToDouble(abc1);

                dataexp2[j] = new Exp_UID();
                dataexp2[j].UID = Convert.ToInt32(abc);
                dataexp2[j].EXP = Convert.ToInt32(abc1);

                j++;

            }
            length2 = j;


            Uid[] dataexp3 = new Uid[100000];
            int[] arr = new int[500];
            int c = 0;
            double prev = tempuid[0];
            for (int k = 0; k < length1; k++)
            {
                double[] arr1 = new double[500];
                double[] arr2 = new double[500];
                int count1 = 0;
                int count2 = 0;


                for (int x = 0; x < length2; x++)
                {

                    if (tempuid[k] == tempuid2[x])
                    {
                        arr1[count1] = tempexp[k];
                        count1++;
                        arr2[count2] = tempexp2[x];
                        count2++;
                        prev = tempuid[k];
                        k++;
                        x++;
                        while (prev == tempuid[k] && k < length1)
                        {
                            arr1[count1] = tempexp[k];
                            count1++;
                            k++;
                        }
                        k--;
                        while (prev == tempuid2[x] && x < length2)
                        {
                            arr2[count2] = tempexp2[x];
                            count2++;
                            x++;
                        }
                        int q = Convert.ToInt32(prev);
                        /*if (q == 48199244)
                        {
                            int assads;
                            assads = q + 1;
                        }*/
                        double t = TTest(arr1, arr2, count1, count2);
                        double p = appExl.WorksheetFunction.TDist(Math.Abs(t), (count1 + count2 - 2), 2);
                        if (p < 0.01)
                        {
                            dataexp3[c] = new Uid();
                            dataexp3[c].UID = Convert.ToInt32(prev);
                            c++;
                        }
                        break;
                    }
                }
            }


            bindingSource3.DataSource = dataexp3;
            dataGridView1.DataSource = bindingSource3;

            OracleCommand cmd4 = conn.CreateCommand();
            String query4 = "SELECT patient.p_id, ap.u_id, mn.exp FROM  mrnaexpression mn INNER JOIN arrayprobe ap ON mn.pb_id=ap.pb_id  INNER JOIN (SELECT a.s_id, b.p_id  FROM clinicalsample a, patientsample b WHERE a.s_id = b.s_id AND b.p_id  IN (SELECT p_id  FROM diagnosis      WHERE ds_id IN (SELECT ds_id FROM disease        WHERE name = '" + disease + "'))) patient ON mn.s_id = patient.s_id AND ap.u_id IN(";
            query4 = query4 + dataexp3[0].UID;
            for (int i1 = 1; i1 < c; i1++)
            {
                query4 = query4 + "," + dataexp3[i1].UID;
            }
            query4 = query4 + ") ORDER BY patient.p_id,ap.u_id";

            cmd4.CommandText = query4;
            OracleDataReader dr4 = cmd4.ExecuteReader();
            int y = 0;
            double[] tempuid3 = new double[100000];
            double[] tempexp3 = new double[100000];
            Pid_UID_Exp[] dataexp4 = new Pid_UID_Exp[100000];
            int length3 = 0;
            int length4 = 0;

            while (dr4.Read())
            {
                var abc = dr4.GetValue(0);
                tempuid3[y] = Convert.ToDouble(abc);
                var abc1 = dr4.GetValue(1);
                tempexp3[y] = Convert.ToDouble(abc1);
                var abc2 = dr4.GetValue(2);
                tempexp3[y] = Convert.ToDouble(abc2);

                dataexp4[y] = new Pid_UID_Exp();
                dataexp4[y].PID = Convert.ToInt32(abc);
                dataexp4[y].UID = Convert.ToInt32(abc1);
                dataexp4[y].EXP = Convert.ToInt32(abc2);

                y++;

            }
            length3 = y;
           


            int z = 0;
            double[] tempuid5 = new double[100000];
            double[] tempexp5 = new double[100000];
            Pid_UID_Exp[] dataexp5 = new Pid_UID_Exp[100000];

            OracleCommand cmd5 = conn.CreateCommand();
            String query5 = "SELECT patient.p_id, ap.u_id, mn.exp FROM  mrnaexpression mn INNER JOIN arrayprobe ap ON mn.pb_id=ap.pb_id  INNER JOIN (SELECT a.s_id, b.p_id  FROM clinicalsample a, patientsample b WHERE a.s_id = b.s_id AND b.p_id  IN (SELECT p_id  FROM diagnosis      WHERE ds_id IN (SELECT ds_id FROM disease        WHERE name != '"+disease+"'))) patient ON mn.s_id = patient.s_id AND ap.u_id IN(";
            query5 = query5 + dataexp3[0].UID;
            for (int i1 = 1; i1 < c; i1++)
            {
                query5 = query5 + "," + dataexp3[i1].UID;
            }
            query5 = query5 + ") ORDER BY patient.p_id,ap.u_id";

            cmd5.CommandText = query5;
            OracleDataReader dr5 = cmd5.ExecuteReader();
            while (dr5.Read())
            {
                var abc = dr5.GetValue(0);
                tempuid5[z] = Convert.ToDouble(abc);
                var abc1 = dr5.GetValue(1);
                tempexp5[z] = Convert.ToDouble(abc1);
                var abc2 = dr5.GetValue(2);
                tempexp5[z] = Convert.ToDouble(abc2);

                dataexp5[z] = new Pid_UID_Exp();
                dataexp5[z].PID = Convert.ToInt32(abc);
                dataexp5[z].UID = Convert.ToInt32(abc1);
                dataexp5[z].EXP = Convert.ToInt32(abc2);

                z++;

            }
            length4 = z;

           

            int[] test = new int[c];
            int[] test1 = new int[c];
            int[] test2 = new int[c];
            int[] test3 = new int[c];
            int[] test4 = new int[c];
            int[] test5 = new int[c];


            int l = 0;
            OracleCommand cmd6 = conn.CreateCommand();
            String query6 = "select * from test_samples where u_id in (";
            query6 = query6 + dataexp3[0].UID;
            for (int i1 = 1; i1 < c; i1++)
            {
                query6 = query6 + "," + dataexp3[i1].UID;
            }
            query6 = query6 + ") ORDER BY u_id";

            cmd2.CommandText = query6;
            OracleDataReader dr6 = cmd2.ExecuteReader();
            while (dr6.Read())
            {
                var abc = dr6.GetValue(0);
                test[l] = Convert.ToInt32(abc);
                var abc1 = dr6.GetValue(1);
                test1[l] = Convert.ToInt32(abc1);
                var abc2 = dr6.GetValue(2);
                test2[l] = Convert.ToInt32(abc2);
                var abc3 = dr6.GetValue(3);
                test3[l] = Convert.ToInt32(abc3);
                var abc4 = dr6.GetValue(4);
                test4[l] = Convert.ToInt32(abc4);
                var abc5 = dr6.GetValue(5);
                test5[l] = Convert.ToInt32(abc5);
                l++;

            }
           
            int[] f1 = new int[c];
            int b = 0;
            double[] c1 = new double[50000];
            double[] c2 = new double[50000];
            double[] c3 = new double[50000];
            double[] c4 = new double[50000];
            double[] c5 = new double[50000];
            int count_in1 = 0;
            for (int a = 0; a < length3; a++)
            {
                f1[b] = dataexp4[a].EXP;
                b++;
                if (b == c)
                {
                    b = 0;

                    c1[count_in1] = appExl.WorksheetFunction.Pearson(f1, test1);
                    c2[count_in1] = appExl.WorksheetFunction.Pearson(f1, test2);
                    c3[count_in1] = appExl.WorksheetFunction.Pearson(f1, test3);
                    c4[count_in1] = appExl.WorksheetFunction.Pearson(f1, test4);
                    c5[count_in1] = appExl.WorksheetFunction.Pearson(f1, test5);
                    count_in1++;
                }


            }

            int[] f2 = new int[c];
            b = 0;
            double[] c6 = new double[50000];
            double[] c7 = new double[50000];
            double[] c8 = new double[50000];
            double[] c9 = new double[50000];
            double[] c10 = new double[50000];
            int count_in2 = 0;
            for (int a = 0; a < length4; a++)
            {
                f2[b] = dataexp5[a].EXP;
                b++;
                if (b == c)
                {
                    b = 0;

                    c6[count_in2] = appExl.WorksheetFunction.Pearson(f2, test1);
                    c7[count_in2] = appExl.WorksheetFunction.Pearson(f2, test2);
                    c8[count_in2] = appExl.WorksheetFunction.Pearson(f2, test3);
                    c9[count_in2] = appExl.WorksheetFunction.Pearson(f2, test4);
                    c10[count_in2] = appExl.WorksheetFunction.Pearson(f2, test5);
                    count_in2++;
                }

            }


            double t1 = TTest(c1, c6, count_in1, count_in2);
            double p1 = appExl.WorksheetFunction.TDist(Math.Abs(t1), (count_in1 + count_in2 - 2), 2);
            double t2 = TTest(c2, c7, count_in1, count_in2);
            double p2 = appExl.WorksheetFunction.TDist(Math.Abs(t2), (count_in1 + count_in2 - 2), 2);
            double t3 = TTest(c3, c8, count_in1, count_in2);
            double p3 = appExl.WorksheetFunction.TDist(Math.Abs(t3), (count_in1 + count_in2 - 2), 2);
            double t4 = TTest(c4, c9, count_in1, count_in2);
            double p4 = appExl.WorksheetFunction.TDist(Math.Abs(t4), (count_in1 + count_in2 - 2), 2);
            double t5 = TTest(c5, c10, count_in1, count_in2);
            double p5 = appExl.WorksheetFunction.TDist(Math.Abs(t5), (count_in1 + count_in2 - 2), 2);


            dataf[] data_final = new dataf[5];
            data_final[0] = new dataf();
            data_final[0].PATIENT = "PATIENT 1";
            data_final[0].TVAL = t1;
            data_final[0].PVAL = p1;
            if (p1 < 0.01)
            {
                data_final[0].PREDICTION = "Classified as '" + glob.dis_glob + " disease : Patient belongs to group A";
            }
            else
            {
                data_final[0].PREDICTION = "Classified as not " + glob.dis_glob + " disease: Patient belongs to group B";
            }

            data_final[1] = new dataf();
            data_final[1].PATIENT = "PATIENT 2";
            data_final[1].TVAL = t2;
            data_final[1].PVAL = p2;
            if (p2 < 0.01)
            {
                data_final[1].PREDICTION = "Classified as '" + glob.dis_glob + " disease : Patient belongs to group A";
            }
            else
            {
                data_final[1].PREDICTION = "Classified as not " + glob.dis_glob + " disease: Patient belongs to group B";
            }

            data_final[2] = new dataf();
            data_final[2].PATIENT = "PATIENT 3";
            data_final[2].TVAL = t3;
            data_final[2].PVAL = p3;
            if (p3 < 0.01)
            {
                data_final[2].PREDICTION = "Classified as '" + glob.dis_glob + " disease : Patient belongs to group A";
            }
            else
            {
                data_final[2].PREDICTION = "Classified as not " + glob.dis_glob + " disease: Patient belongs to group B";
            }

            data_final[3] = new dataf();
            data_final[3].PATIENT = "PATIENT 4";
            data_final[3].TVAL = t4;
            data_final[3].PVAL = p4;
            if (p4 < 0.01)
            {
                data_final[3].PREDICTION = "Classified as '" + glob.dis_glob + " disease : Patient belongs to group A";
            }
            else
            {
                data_final[3].PREDICTION = "Classified as not " + glob.dis_glob + " disease: Patient belongs to group B";
            }

            data_final[4] = new dataf();
            data_final[4].PATIENT = "PATIENT 5";
            data_final[4].TVAL = t5;
            data_final[4].PVAL = p5;
            if (p5 < 0.01)
            {
                data_final[4].PREDICTION = "Classified as '" + glob.dis_glob + " disease : Patient belongs to group A";
            }
            else
            {
                data_final[4].PREDICTION = "Classified as not " + glob.dis_glob + " disease: Patient belongs to group B";
            }

            bindingSource4.DataSource = data_final;
            dataGridView4.DataSource = bindingSource4;
            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            string selecteddisease = (string)comboBox1.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox1.FindStringExact(selecteddisease);


            disease = selecteddisease;
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
   
    
}
