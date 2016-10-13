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
    public partial class Form8 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private BindingSource bindingSource2 = new BindingSource();
        private BindingSource bindingSource3 = new BindingSource();
        private BindingSource bindingSource4 = new BindingSource();

        public Uid[] dataexp3 = new Uid[100000];
        public string disease;
        public Form8()
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
        
        public void button1_Click(object sender, EventArgs e)
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
            bindingSource1.DataSource = dataexp;
            dataGridView1.DataSource = bindingSource1;



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

            bindingSource2.DataSource = dataexp2;
            dataGridView2.DataSource = bindingSource2;

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
                
                
                for (int l = 0; l < length2; l++)
                {
                    
                    if (tempuid[k] == tempuid2[l])
                    {
                        arr1[count1] = tempexp[k];
                        count1++;
                        arr2[count2] = tempexp2[l];
                        count2++;
                        prev = tempuid[k];
                        k++;
                        l++;
                        while (prev == tempuid[k] && k<length1)
                        {
                            arr1[count1] = tempexp[k];
                            count1++;
                            k++;
                        }
                        k--;
                        while (prev == tempuid2[l] && l<length2)
                        {
                            arr2[count2] = tempexp2[l];
                            count2++;
                            l++;
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
            dataGridView3.DataSource = bindingSource3;

           

                glob.dataglob = dataexp3;
                glob.length = c;
                glob.dis_glob = disease;


            //double t = TTest(tempexp, tempexp2, length1, length2);
            //label2.Text = "T-Test value is: " + t.ToString();

            conn.Close();
    }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            ComboBox comboBox = (ComboBox)sender;

            string selecteddisease = (string)comboBox1.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox1.FindStringExact(selecteddisease);

           
            disease = selecteddisease;
          
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    public class Exp_UID
    {
        public int UID { get; set; }
        public int EXP { get; set; }

    }
    public class Uid
    {
        public int UID { get; set; }
        
    }

    public class disease
    {
        public string DISEASE { get; set; }

    }
}
