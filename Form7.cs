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
    public partial class Form7 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private BindingSource bindingSource2 = new BindingSource();
        private string name1;
        private string name2;
        private string goid;
        public Form7()
        {
            InitializeComponent();
            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#

            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT distinct go_id FROM goannotation order by go_id";
            OracleDataReader dr4 = cmd.ExecuteReader();
            while (dr4.Read())
            {
                var abc = dr4.GetValue(0);
                this.comboBox1.Items.Add(abc.ToString());

            }

            this.comboBox1.Sorted = true;
            this.comboBox1.Refresh();

            OracleCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = "SELECT name FROM disease";
            OracleDataReader dr5 = cmd2.ExecuteReader();
            while (dr5.Read())
            {
                var abc = dr5.GetValue(0);
                this.comboBox2.Items.Add(abc.ToString());
            }

            this.comboBox2.Sorted = true;
            this.comboBox2.Refresh();

            OracleCommand cmd3 = conn.CreateCommand();
            cmd3.CommandText = "SELECT name FROM disease";
            OracleDataReader dr6 = cmd3.ExecuteReader();
            while (dr6.Read())
            {
                var abc = dr6.GetValue(0);
                this.comboBox3.Items.Add(abc.ToString());
            }

            this.comboBox3.Sorted = true;
            this.comboBox3.Refresh();

            conn.Close();
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#
            IExcel.Application appExl;
            appExl = new IExcel.Application();
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT d.p_id, mn.exp FROM  mrnaexpression mn,clinicalsample cs, patientsample ps, patient p, diagnosis d WHERE mn.s_id = cs.s_id AND cs.s_id = ps.s_id AND ps.p_id = p.p_id AND p.p_id = d.p_id AND d.ds_id IN(SELECT ds_id FROM disease WHERE name = '"+name1+"') AND mn.pb_id IN(SELECT ap.pb_id FROM arrayprobe ap,genesequence gs,goannotation ga WHERE ap.u_id = gs.u_id AND gs.u_id = ga.u_id AND ga.go_id ='"+goid+"')  ORDER by d.p_id, mn.pb_id";
            OracleDataReader dr = cmd.ExecuteReader();
            OracleDataReader dr_temp = dr;
            int i = 0;
            // double[] tempc1 = new double[100000];
            //Exp[] dataexp = new Exp[100000];

            double[] temppid = new double[100000];
            double[] tempexp = new double[100000];
            Exp_Pid[] dataexp = new Exp_Pid[100000];
            int length1 = 0;
            int length2 = 0;

            while (dr.Read())
            {
                var abc = dr.GetValue(0);
                temppid[i] = Convert.ToDouble(abc);
                var abc1 = dr.GetValue(1);
                tempexp[i] = Convert.ToDouble(abc1);

                dataexp[i] = new Exp_Pid();
                dataexp[i].PID = Convert.ToInt32(abc);
                dataexp[i].EXP = Convert.ToInt32(abc1);

                i++;

            }
            length1 = i;
            bindingSource1.DataSource = dataexp;
            dataGridView1.DataSource = bindingSource1;



            int j = 0;
            double[] temppid2 = new double[100000];
            double[] tempexp2 = new double[100000];
            Exp_Pid[] dataexp2 = new Exp_Pid[100000];

            OracleCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = "SELECT patient.p_id, mn.exp FROM  mrnaexpression mn INNER JOIN arrayprobe ap ON mn.pb_id=ap.pb_id  INNER JOIN (SELECT a.s_id, b.p_id  FROM clinicalsample a, patientsample b WHERE a.s_id = b.s_id AND b.p_id  IN (SELECT p_id  FROM diagnosis      WHERE ds_id IN (SELECT ds_id FROM disease        WHERE name = '"+name2+"'))) patient ON mn.s_id = patient.s_id AND ap.u_id IN(SELECT u_id FROM goannotation WHERE go_id = '"+goid+"') ORDER by patient.p_id,mn.pb_id";
            OracleDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                var abc = dr2.GetValue(0);
                temppid2[j] = Convert.ToDouble(abc);
                var abc1 = dr2.GetValue(1);
                tempexp2[j] = Convert.ToDouble(abc1);

                dataexp2[j] = new Exp_Pid();
                dataexp2[j].PID = Convert.ToInt32(abc);
                dataexp2[j].EXP = Convert.ToInt32(abc1);

                j++;

            }
            length2 = j;

            bindingSource2.DataSource = dataexp2;
            dataGridView2.DataSource = bindingSource2;

            double[] cor = new double[100000];
            double cor_sum = 0;
            int count = 0;
            int farzi = 3;
            int arlen = 0;
            int arlen_flag = 0;
            for (int n = 0; n < length1; n++)
            {
                double prev = temppid[n];
                int[] newexp = new int[350];
                int cin1 = 0;
                farzi = n + 9;
                for (int m = n; m < length1; m++)
                {

                    if (prev != temppid[n])
                    {
                        double prev1 = temppid[m];
                        int[] newexp2 = new int[350];
                        int cin = 0;
                        if (arlen_flag == 0)
                        {
                            arlen = n;
                            arlen_flag = 1;
                        } 
                        for (int o = m; o < length1; o++)
                        {

                            if (prev1 != temppid[o])
                            {
                                int[] p1 = new int[arlen];
                                int[] p2 = new int[arlen];
                                for (int p = 0; p < (arlen); p++)
                                {
                                    p1[p] = newexp[p];
                                    p2[p] = newexp2[p];
                                }
                                cor[count] = appExl.WorksheetFunction.Pearson(p1, p2);
                                cor_sum = cor_sum + cor[count];
                                count++;
                                cin = 0;
                                prev1 = temppid[o];
                                //break;
                            }
                            newexp2[cin] = Convert.ToInt32(tempexp[o]);
                            cin++;
                            if (o == length1 - 1)
                            {
                                int[] p1 = new int[arlen];
                                int[] p2 = new int[arlen];
                                for (int p = 0; p < (arlen); p++)
                                {
                                    p1[p] = newexp[p];
                                    p2[p] = newexp2[p];
                                }
                                cor[count] = appExl.WorksheetFunction.Pearson(p1, p2);
                                cor_sum = cor_sum + cor[count];
                                count++;
                                cin = 0;
                                prev1 = temppid[o];
                            }

                        }
                        prev = temppid[n];
                        cin1 = 0;
                        break;
                    }
                    newexp[cin1] = Convert.ToInt32(tempexp[n]);
                    cin1++;
                    n++;

                }
                n--;
                if (n == (length1 - 1))
                {

                }
            }

            double avgcor = cor_sum / count;
            label2.Text = label2.Text + "'" +avgcor.ToString() + "'";

            double[] cor2 = new double[50000];
            double cor_sum2 = 0;
            int count2 = 0;
            int[] arr1 = new int[50000];
            int[] arr2 = new int[50000];
            double pre1 = temppid[0];
            double pre2 = temppid2[0];
            int cn = 0;
            int cn2 = 0;
            arlen = 0;
            arlen_flag = 0;
            for (int p = 0; p < length1; p++)
            {
                if (pre1 != temppid[p])
                {
                    pre2 = temppid2[0];
                    if (arlen_flag == 0)
                    {
                        arlen = p;
                        arlen_flag = 1;
                    }
                    for (int q = 0; q < length2; q++)
                    {
                        if ((pre2 != temppid2[q]))
                        {
                            int[] p1 = new int[arlen];
                            int[] p2 = new int[arlen];
                            for (int s = 0; s < (arlen); s++)
                            {
                                p1[s] = arr1[s];
                                p2[s] = arr2[s];
                            }
                            cor2[count2] = appExl.WorksheetFunction.Pearson(p1, p2);
                            cor_sum2 = cor_sum2 + cor2[count2];
                            count2++;
                            pre2 = temppid2[q];
                            cn2 = 0;
                        }
                        arr2[cn2] = Convert.ToInt32(tempexp2[q]);
                        cn2++;

                        pre1 = temppid[p];
                        cn = 0;
                        if (q == (length2 - 1))
                        {
                            int[] p1 = new int[arlen];
                            int[] p2 = new int[arlen];
                            for (int s = 0; s < (arlen); s++)
                            {
                                p1[s] = arr1[s];
                                p2[s] = arr2[s];
                            }
                            cor2[count2] = appExl.WorksheetFunction.Pearson(p1, p2);
                            cor_sum2 = cor_sum2 + cor2[count2];
                            count2++;
                            pre2 = temppid2[q];
                            cn2 = 0;

                        }
                    }
                    

                }
                arr1[cn] = Convert.ToInt32(tempexp[p]);
                cn++;
                if (p == (length1 - 1))
                {
                    pre2 = temppid2[0];
                    if (arlen_flag == 0)
                    {
                        arlen = p;
                        arlen_flag = 1;
                    }
                    for (int q = 0; q < length2; q++)
                    {
                        if ((pre2 != temppid2[q]))
                        {
                            int[] p1 = new int[arlen];
                            int[] p2 = new int[arlen];
                            for (int s = 0; s < (arlen); s++)
                            {
                                p1[s] = arr1[s];
                                p2[s] = arr2[s];
                            }
                            cor2[count2] = appExl.WorksheetFunction.Pearson(p1, p2);
                            cor_sum2 = cor_sum2 + cor2[count2];
                            count2++;
                            pre2 = temppid2[q];
                            cn2 = 0;
                        }
                        arr2[cn2] = Convert.ToInt32(tempexp2[q]);
                        cn2++;

                        pre1 = temppid[p];
                        cn = 0;
                        if (q == (length2 - 1))
                        {
                            int[] p1 = new int[arlen];
                            int[] p2 = new int[arlen];
                            for (int s = 0; s < (arlen); s++)
                            {
                                p1[s] = arr1[s];
                                p2[s] = arr2[s];
                            }
                            cor2[count2] = appExl.WorksheetFunction.Pearson(p1, p2);
                            cor_sum2 = cor_sum2 + cor2[count2];
                            count2++;
                            pre2 = temppid2[q];
                            cn2 = 0;

                        }
                    }
                }
            }
                double avgcor2 = cor_sum2 / count2 ;
                label3.Text = label3.Text + "'" + avgcor2.ToString() + "'";





            //double t = TTest(temp, temp2, length1, length2);
            //label2.Text = "T-Test value is: " + t.ToString();

            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox1 = (ComboBox)sender;

            string selectedgoid = (string)comboBox1.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox1.FindStringExact(selectedgoid);


            goid = selectedgoid;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox2 = (ComboBox)sender;

            string selectedname = (string)comboBox2.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox2.FindStringExact(selectedname);


            name1 = selectedname;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox2 = (ComboBox)sender;

            string selectedname = (string)comboBox2.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox2.FindStringExact(selectedname);


            name2 = selectedname;
        }
    }
    public class Exp_Pid
    {
        public int PID { get; set; }
        public int EXP { get; set; }

    }
}
