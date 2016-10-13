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
using System.Web.UI.DataVisualization.Charting;
using CenterSpace.NMath.Core;
using System.Collections.ObjectModel;
using CenterSpace.NMath.Analysis;
using CenterSpace.NMath.Charting;
using CenterSpace.NMath.Matrix;



namespace WindowsFormsApplication1
{
    
    public partial class Form5 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private BindingSource bindingSource2 = new BindingSource();
        private string name;
        private string goid;
        public Form5()
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

            //int n1 = x.Length;
            //int n2 = y.Length;
            double meanX = sumX / n1;
            double meanY = sumY / n2;
            double sumXminusMeanSquared = 0.0; // Calculate variances
            double sumYminusMeanSquared = 0.0;
            for (int i = 0; i < n1; i++)
                sumXminusMeanSquared = sumXminusMeanSquared + (x[i] - meanX) * (x[i] - meanX);
            for (int i = 0; i < n2; i++)
                sumYminusMeanSquared = sumYminusMeanSquared + (y[i] - meanY) * (y[i] - meanY);
            double varX = sumXminusMeanSquared / (n1-1 );
            double varY = sumYminusMeanSquared / (n2-1);

            double top = (meanX - meanY);

            double sp = (((n1 - 1) * varX) + ((n2 - 1) * varY)) / (n1 + n2 - 2);

            double bot = Math.Sqrt((sp*(n1+n2)/ (n1*n2)));
            double t = top / bot;
            double num = ((varX / n1) + (varY / n2)) *
  ((varX / n1) + (varY / n2));
            double denomLeft = ((varX / n1) * (varX / n1)) / (n1 );
            double denomRight = ((varY / n2) * (varY / n2)) / (n2);
            double denom = denomLeft + denomRight;
            double df = num / denom;

            double p = Student(t, df); // Cumulative two-tail density
            return t;
            /*Console.WriteLine("mean of x = " + meanX.ToString("F2"));
            Console.WriteLine("mean of y = " + meanY.ToString("F2"));
            Console.WriteLine("t = " + t.ToString("F4"));
            Console.WriteLine("df = " + df.ToString("F3"));
            Console.WriteLine("p-value = " + p.ToString("F5"));
            Console.WriteLine("n1 = " + n1.ToString("F5"));
            Console.WriteLine("n2 = " + n2.ToString("F5"));
            */

        }
        public static double Gauss(double z)
        {
            // input = z-value (-inf to +inf)
            // output = p under Standard Normal curve from -inf to z
            // e.g., if z = 0.0, function returns 0.5000
            // ACM Algorithm #209
            double y; // 209 scratch variable
            double p; // result. called 'z' in 209
            double w; // 209 scratch variable
            if (z == 0.0)
                p = 0.0;
            else
            {
                y = Math.Abs(z) / 2;
                if (y >= 3.0)
                {
                    p = 1.0;
                }
                else if (y < 1.0)
                {
                    w = y * y;
                    p = ((((((((0.000124818987 * w
                      - 0.001075204047) * w + 0.005198775019) * w
                      - 0.019198292004) * w + 0.059054035642) * w
                      - 0.151968751364) * w + 0.319152932694) * w
                      - 0.531923007300) * w + 0.797884560593) * y * 2.0;
                }
                else
                {
                    y = y - 2.0;
                    p = (((((((((((((-0.000045255659 * y
                      + 0.000152529290) * y - 0.000019538132) * y
                      - 0.000676904986) * y + 0.001390604284) * y
                      - 0.000794620820) * y - 0.002034254874) * y
                      + 0.006549791214) * y - 0.010557625006) * y
                      + 0.011630447319) * y - 0.009279453341) * y
                      + 0.005353579108) * y - 0.002141268741) * y
                      + 0.000535310849) * y + 0.999936657524;
                }
            }
            if (z > 0.0)
                return (p + 1.0) / 2;
            else
                return (1.0 - p) / 2;
        }
        public static double Student(double t, double df)
        {
            // for large integer df or double df
            // adapted from ACM algorithm 395
            // returns 2-tail p-value
            double n = df; // to sync with ACM parameter name
            double a, b, y;
            t = t * t;
            y = t / n;
            b = y + 1.0;
            if (y > 1.0E-6) y = Math.Log(b);
            a = n - 0.5;
            b = 48.0 * a * a;
            y = a * y;
            y = (((((-0.4 * y - 3.3) * y - 24.0) * y - 85.5) /
              (0.8 * y * y + 100.0 + b) + y + 3.0) / b + 1.0) *
              Math.Sqrt(y);
            return 2.0 * Gauss(-y); // ACM algorithm 209
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#

            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT exp FROM mrnaexpression WHERE s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN(SELECT s_id  FROM patientsample  WHERE p_id  IN(SELECT p_id  FROM diagnosis      WHERE ds_id IN(SELECT ds_id FROM disease        WHERE name = '"+name+"')))) AND pb_id IN(SELECT pb_id FROM arrayprobe     WHERE u_id IN(SELECT u_id FROM goannotation WHERE go_id = '"+goid+"'))";
            OracleDataReader dr = cmd.ExecuteReader();
            OracleDataReader dr_temp = dr;
            int i = 0;
            double[] temp = new double[10000];
            Exp[] dataexp = new Exp[10000];
            int length1 = 0;
            int length2 = 0;

            while (dr.Read())
            {
                var abc = dr.GetValue(0);
                temp[i] = Convert.ToDouble(abc);
                 
                dataexp[i] = new Exp();
                dataexp[i].EXP = Convert.ToInt32(abc);

                i++;
                
            }
            length1 = i;
            bindingSource1.DataSource = dataexp;
            dataGridView1.DataSource = bindingSource1;



            int j = 0;
            double[] temp2 = new double[10000];
            Exp[] dataexp2 = new Exp[10000];
            
            OracleCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = "SELECT exp FROM mrnaexpression WHERE s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN(SELECT s_id  FROM patientsample  WHERE p_id  IN(SELECT p_id  FROM diagnosis      WHERE ds_id IN(SELECT ds_id FROM disease        WHERE name != '"+name+"')))) AND pb_id IN(SELECT pb_id FROM arrayprobe     WHERE u_id IN(SELECT u_id FROM goannotation WHERE go_id = '"+goid+"'))";
            OracleDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                var abc = dr2.GetValue(0);
                temp2[j] = Convert.ToDouble(abc);
                
                dataexp2[j] = new Exp();
                dataexp2[j].EXP = Convert.ToInt32(abc);

                j++;

            }
            length2 = j;

            bindingSource2.DataSource = dataexp2;
            dataGridView2.DataSource = bindingSource2;


            double t = TTest(temp, temp2, length1, length2);
            label2.Text = "T-Test value is: " + t.ToString();
           
            conn.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
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


            name = selectedname;
        }
    }
    public class Exp
    {
        public int EXP { get; set; }
        
    }
}
