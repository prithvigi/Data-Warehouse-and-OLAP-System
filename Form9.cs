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
    public partial class Form9 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private BindingSource bindingSource2 = new BindingSource();
        private BindingSource bindingSource3 = new BindingSource();
        private BindingSource bindingSource4 = new BindingSource();
        public Form9()
        {
            InitializeComponent();
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
            bindingSource1.DataSource = glob.dataglob;
            dataGridView1.DataSource = bindingSource1;

            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#
            IExcel.Application appExl;
            appExl = new IExcel.Application();

            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            String query = "SELECT patient.p_id, ap.u_id, mn.exp FROM  mrnaexpression mn INNER JOIN arrayprobe ap ON mn.pb_id=ap.pb_id  INNER JOIN (SELECT a.s_id, b.p_id  FROM clinicalsample a, patientsample b WHERE a.s_id = b.s_id AND b.p_id  IN (SELECT p_id  FROM diagnosis      WHERE ds_id IN (SELECT ds_id FROM disease        WHERE name = '"+ glob.dis_glob+"'))) patient ON mn.s_id = patient.s_id AND ap.u_id IN(";
            query = query + glob.dataglob[0].UID;
            for (int i1 = 1; i1 < glob.length; i1++)
            {
                query = query + "," + glob.dataglob[i1].UID;
            }
            query = query + ") ORDER BY patient.p_id,ap.u_id";
            
            cmd.CommandText = query;
            OracleDataReader dr = cmd.ExecuteReader();
            OracleDataReader dr_temp = dr;
            int i = 0;
            double[] tempuid = new double[100000];
            double[] tempexp = new double[100000];
            Pid_UID_Exp[] dataexp = new Pid_UID_Exp[100000];
            int length1 = 0;
            int length2 = 0;

            while (dr.Read())
            {
                var abc = dr.GetValue(0);
                tempuid[i] = Convert.ToDouble(abc);
                var abc1 = dr.GetValue(1);
                tempexp[i] = Convert.ToDouble(abc1);
                var abc2 = dr.GetValue(2);
                tempexp[i] = Convert.ToDouble(abc2);

                dataexp[i] = new Pid_UID_Exp();
                dataexp[i].PID = Convert.ToInt32(abc);
                dataexp[i].UID = Convert.ToInt32(abc1);
                dataexp[i].EXP = Convert.ToInt32(abc2);

                i++;

            }
            length1 = i;
            bindingSource2.DataSource = dataexp;
            dataGridView2.DataSource = bindingSource2;



            int j = 0;
            double[] tempuid2 = new double[100000];
            double[] tempexp2 = new double[100000];
            Pid_UID_Exp[] dataexp2 = new Pid_UID_Exp[100000];

            OracleCommand cmd2 = conn.CreateCommand();
            String query2 = "SELECT patient.p_id, ap.u_id, mn.exp FROM  mrnaexpression mn INNER JOIN arrayprobe ap ON mn.pb_id=ap.pb_id  INNER JOIN (SELECT a.s_id, b.p_id  FROM clinicalsample a, patientsample b WHERE a.s_id = b.s_id AND b.p_id  IN (SELECT p_id  FROM diagnosis      WHERE ds_id IN (SELECT ds_id FROM disease        WHERE name != '"+ glob.dis_glob+"'))) patient ON mn.s_id = patient.s_id AND ap.u_id IN(";
            query2 = query2 + glob.dataglob[0].UID;
            for (int i1 = 1; i1 < glob.length; i1++)
            {
                query2 = query2 + "," + glob.dataglob[i1].UID;
            }
            query2 = query2 + ") ORDER BY patient.p_id,ap.u_id";

            cmd2.CommandText = query2;
            OracleDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                var abc = dr2.GetValue(0);
                tempuid2[j] = Convert.ToDouble(abc);
                var abc1 = dr2.GetValue(1);
                tempexp2[j] = Convert.ToDouble(abc1);
                var abc2 = dr2.GetValue(2);
                tempexp2[j] = Convert.ToDouble(abc2);

                dataexp2[j] = new Pid_UID_Exp();
                dataexp2[j].PID = Convert.ToInt32(abc);
                dataexp2[j].UID = Convert.ToInt32(abc1);
                dataexp2[j].EXP = Convert.ToInt32(abc2);

                j++;

            }
            length2 = j;

            bindingSource3.DataSource = dataexp2;
            dataGridView3.DataSource = bindingSource3;

            int[] test = new int[glob.length];
            int[] test1 = new int[glob.length];
            int[] test2 = new int[glob.length];
            int[] test3 = new int[glob.length];
            int[] test4 = new int[glob.length];
            int[] test5 = new int[glob.length];


            int l = 0;
            OracleCommand cmd3 = conn.CreateCommand();
            String query3 = "select * from test_samples where u_id in (";
            query3 = query3 + glob.dataglob[0].UID;
            for (int i1 = 1; i1 < glob.length; i1++)
            {
                query3 = query3 + "," + glob.dataglob[i1].UID;
            }
            query3 = query3 + ") ORDER BY u_id";

            cmd2.CommandText = query3;
            OracleDataReader dr3 = cmd2.ExecuteReader();
            while (dr3.Read())
            {
                var abc = dr3.GetValue(0);
                test[l] = Convert.ToInt32(abc);
                var abc1 = dr3.GetValue(1);
                test1[l] = Convert.ToInt32(abc1);
                var abc2 = dr3.GetValue(2);
                test2[l] = Convert.ToInt32(abc2);
                var abc3 = dr3.GetValue(3);
                test3[l] = Convert.ToInt32(abc3);
                var abc4 = dr3.GetValue(4);
                test4[l] = Convert.ToInt32(abc4);
                var abc5 = dr3.GetValue(5);
                test5[l] = Convert.ToInt32(abc5);
                l++;

            }
            int length3 = j;
            int[] f1 = new int[glob.length];
            int b = 0;
            double[] c1 = new double[50000];
            double[] c2 = new double[50000];
            double[] c3 = new double[50000];
            double[] c4 = new double[50000];
            double[] c5 = new double[50000];
            int count_in1 = 0;
            for (int a = 0; a < length1; a++)
            {
                f1[b] = dataexp[a].EXP;
                b++;
                if (b == glob.length)
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

            int[] f2 = new int[glob.length];
            b = 0;
            double[] c6 = new double[50000];
            double[] c7 = new double[50000];
            double[] c8 = new double[50000];
            double[] c9 = new double[50000];
            double[] c10 = new double[50000];
            int count_in2 = 0;
            for (int a = 0; a < length2; a++)
            {
                f2[b] = dataexp2[a].EXP;
                b++;
                if (b == glob.length)
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


        }
    }
    public class Pid_UID_Exp
    {
        public int PID { get; set; }
        public int UID { get; set; }
        public int EXP { get; set; }

    }
    public class dataf
    {
        public string PATIENT { get; set; }
        public double TVAL { get; set; }
        public double PVAL { get; set; }
        public string PREDICTION { get; set; }

    }

}
