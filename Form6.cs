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

namespace WindowsFormsApplication1
{
    

    public partial class Form6 : Form
    {
        private string goid;
        public Form6()
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
        }

        public static double FTest2(int k, double[] x1, double sumx1, int n1, double[] x2, double sumx2, int n2)
        {

            double meanXO = (sumx1 + sumx2 ) / (n1 + n2 );
            double meanx1 = sumx1 / n1;
            double meanx2 = sumx2 / n2;
            double sscond = (meanx1 - meanXO) * (meanx1 - meanXO) * n1 + (meanx2 - meanXO) * (meanx2 - meanXO) * n2;
            double sserr = 0;
            for (int i = 0; i < n1; i++)
            {
                sserr += (x1[i] - meanx1) * (x1[i] - meanx1);
            }
            for (int i = 0; i < n2; i++)
            {
                sserr += (x2[i] - meanx2) * (x2[i] - meanx2);
            }
           
            double mscond = sscond / (k - 1);
            double mserr = sserr / (n1 + n2 - k);
            double f = mscond / mserr;
            return f;

        }


        public static double FTest3(int k, double[] x1, double sumx1, int n1, double[] x2, double sumx2, int n2, double[] x3, double sumx3, int n3)
        {

            double meanXO = (sumx1 + sumx2 + sumx3 ) / (n1 + n2 + n3 );
            double meanx1 = sumx1 / n1;
            double meanx2 = sumx2 / n2;
            double meanx3 = sumx3 / n3;
            double sscond = (meanx1 - meanXO) * (meanx1 - meanXO) * n1 + (meanx2 - meanXO) * (meanx2 - meanXO) * n2 + (meanx3 - meanXO) * (meanx3 - meanXO) * n3 ;
            double sserr = 0;
            for (int i = 0; i < n1; i++)
            {
                sserr += (x1[i] - meanx1) * (x1[i] - meanx1);
            }
            for (int i = 0; i < n2; i++)
            {
                sserr += (x2[i] - meanx2) * (x2[i] - meanx2);
            }
            for (int i = 0; i < n3; i++)
            {
                sserr += (x3[i] - meanx3) * (x3[i] - meanx3);
            }
           
            double mscond = sscond / (k - 1);
            double mserr = sserr / (n1 + n2 + n3  - k);
            double f = mscond / mserr;
            return f;


        }


        public static double FTest4(int k, double[] x1, double sumx1, int n1, double[] x2, double sumx2, int n2, double[] x3, double sumx3,int n3, double[] x4, double sumx4, int n4)
        {
           
            double meanXO = (sumx1+ sumx2 + sumx3 + sumx4) / (n1+n2+n3+n4);
            double meanx1 = sumx1 / n1;
            double meanx2 = sumx2 / n2;
            double meanx3 = sumx3 / n3;
            double meanx4 = sumx4 / n4;


            double sscond = (meanx1 - meanXO) * (meanx1 - meanXO)*n1 + (meanx2 - meanXO) * (meanx2 - meanXO) * n2 + (meanx3 - meanXO) * (meanx3 - meanXO) * n3 + (meanx4 - meanXO) * (meanx4 - meanXO) * n4;
            double sserr = 0;
            for (int i = 0; i < n1; i++)
            {
                sserr += (x1[i] - meanx1) * (x1[i] - meanx1);
            }
            for (int i = 0; i < n2; i++)
            {
                sserr += (x2[i] - meanx2) * (x2[i] - meanx2);
            }
            for (int i = 0; i < n3; i++)
            {
                sserr += (x3[i] - meanx3) * (x3[i] - meanx3);
            }
            for (int i = 0; i < n4; i++)
            {
                sserr += (x4[i] - meanx4) * (x4[i] - meanx4);
            }

            double mscond = sscond/(k - 1);
            double mserr = sserr / (n1 + n2 + n3 + n4 - k);
            double f = mscond / mserr;
            return f;

        }

        public static double FTest5(int k, double[] x1, double sumx1, int n1, double[] x2, double sumx2, int n2, double[] x3, double sumx3, int n3, double[] x4, double sumx4, int n4, double[] x5, double sumx5, int n5)
        {

            double meanXO = (sumx1 + sumx2 + sumx3 + sumx4 + sumx5) / (n1 + n2 + n3 + n4 + n5);
            double meanx1 = sumx1 / n1;
            double meanx2 = sumx2 / n2;
            double meanx3 = sumx3 / n3;
            double meanx4 = sumx4 / n4;
            double meanx5 = sumx5 / n5;

            double sscond = (meanx1 - meanXO) * (meanx1 - meanXO) * n1 + (meanx2 - meanXO) * (meanx2 - meanXO) * n2 + (meanx3 - meanXO) * (meanx3 - meanXO) * n3 + (meanx4 - meanXO) * (meanx4 - meanXO) * n4 + (meanx5 - meanXO) * (meanx5 - meanXO) * n5;
            double sserr = 0;
            for (int i = 0; i < n1; i++)
            {
                sserr += (x1[i] - meanx1) * (x1[i] - meanx1);
            }
            for (int i = 0; i < n2; i++)
            {
                sserr += (x2[i] - meanx2) * (x2[i] - meanx2);
            }
            for (int i = 0; i < n3; i++)
            {
                sserr += (x3[i] - meanx3) * (x3[i] - meanx3);
            }
            for (int i = 0; i < n4; i++)
            {
                sserr += (x4[i] - meanx4) * (x4[i] - meanx4);
            }
            for (int i = 0; i < n5; i++)
            {
                sserr += (x5[i] - meanx5) * (x5[i] - meanx5);
            }

            double mscond = sscond / (k - 1);
            double mserr = sserr / (n1 + n2 + n3 + n4 +n5 - k);
            double f = mscond / mserr;
            return f;


        }

        public static double FTest6(int k, double[] x1, double sumx1, int n1, double[] x2, double sumx2, int n2, double[] x3, double sumx3, int n3, double[] x4, double sumx4, int n4, double[] x5, double sumx5, int n5, double[] x6, double sumx6, int n6)
        {

            double meanXO = (sumx1 + sumx2 + sumx3 + sumx4 + sumx5) / (n1 + n2 + n3 + n4 + n5 + n6);
            double meanx1 = sumx1 / n1;
            double meanx2 = sumx2 / n2;
            double meanx3 = sumx3 / n3;
            double meanx4 = sumx4 / n4;
            double meanx5 = sumx5 / n5;
            double meanx6 = sumx6 / n6;

            double sscond = (meanx1 - meanXO) * (meanx1 - meanXO) * n1 + (meanx2 - meanXO) * (meanx2 - meanXO) * n2 + (meanx3 - meanXO) * (meanx3 - meanXO) * n3 + (meanx4 - meanXO) * (meanx4 - meanXO) * n4 + (meanx5 - meanXO) * (meanx5 - meanXO) * n5 + (meanx6 - meanXO) * (meanx6 - meanXO) * n6;
            double sserr = 0;
            for (int i = 0; i < n1; i++)
            {
                sserr += (x1[i] - meanx1) * (x1[i] - meanx1);
            }
            for (int i = 0; i < n2; i++)
            {
                sserr += (x2[i] - meanx2) * (x2[i] - meanx2);
            }
            for (int i = 0; i < n3; i++)
            {
                sserr += (x3[i] - meanx3) * (x3[i] - meanx3);
            }
            for (int i = 0; i < n4; i++)
            {
                sserr += (x4[i] - meanx4) * (x4[i] - meanx4);
            }
            for (int i = 0; i < n5; i++)
            {
                sserr += (x5[i] - meanx5) * (x5[i] - meanx5);
            }
            for (int i = 0; i < n5; i++)
            {
                sserr += (x6[i] - meanx6) * (x6[i] - meanx6);
            }

            double mscond = sscond / (k - 1);
            double mserr = sserr / (n1 + n2 + n3 + n4 + n5 + n6 - k);
            double f = mscond / mserr;
            return f;


        }


        private void button1_Click(object sender, EventArgs e)
        {
            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#
            int i = 0;
            int k = 0;
            int[] lst= new int[6];
            double sum1 = 0.0;
            double s1=0.0, s2 = 0.0, s3 = 0.0, s4 = 0.0, s5 = 0.0, s6= 0.0;
            int l1 = 0, l2 = 0, l3 = 0, l4 = 0, l5 = 0, l6 = 0;
            double[] d1 = new double[10000];
            double[] d2 = new double[10000];
            double[] d3= new double[10000];
            double[] d4 = new double[10000];
            double[] d5 = new double[10000];
            double[] d6 = new double[10000];
            double[] temp = new double[10000];
            Exp[] dataexp = new Exp[10000];
            int length1 = 0;
            conn.Open();
            if (checkBox1.Checked)
            {   
                
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT exp FROM mrnaexpression WHERE s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN(SELECT s_id  FROM patientsample  WHERE p_id  IN(SELECT p_id  FROM diagnosis      WHERE ds_id IN(SELECT ds_id FROM disease        WHERE name = 'ALL')))) AND pb_id IN(SELECT pb_id FROM arrayprobe     WHERE u_id IN(SELECT u_id FROM goannotation WHERE go_id = '"+goid+"'))";
                OracleDataReader dr = cmd.ExecuteReader();
                
                while (dr.Read())
                {
                    var abc = dr.GetValue(0);
                    temp[i] = Convert.ToDouble(abc);
                    sum1 = sum1 + temp[i];
                    dataexp[i] = new Exp();
                    dataexp[i].EXP = Convert.ToInt32(abc);

                    i++;

                }
                length1 = i;
                lst[k] = 1;
                k++;
                l1 = length1;
                s1 = sum1;
                d1 = temp;
            }

            double[] temp2 = new double[10000];
            Exp[] dataexp2 = new Exp[10000];
            int length2 = 0;
            double sum2 = 0.0;

            if (checkBox2.Checked)
            {
                i = 0;
               
                OracleCommand cmd2 = conn.CreateCommand();
                cmd2.CommandText = "SELECT exp FROM mrnaexpression WHERE s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN(SELECT s_id  FROM patientsample  WHERE p_id  IN(SELECT p_id  FROM diagnosis      WHERE ds_id IN(SELECT ds_id FROM disease        WHERE name = 'AML')))) AND pb_id IN(SELECT pb_id FROM arrayprobe     WHERE u_id IN(SELECT u_id FROM goannotation WHERE go_id = '" + goid + "'))";
                OracleDataReader dr2 = cmd2.ExecuteReader();
                while (dr2.Read())
                {
                    var abc = dr2.GetValue(0);
                    temp2[i] = Convert.ToDouble(abc);
                    sum2 = sum2 + temp2[i];
                    dataexp2[i] = new Exp();
                    dataexp2[i].EXP = Convert.ToInt32(abc);

                    i++;

                }
                length2 = i;
                lst[k] = 2;
                k++;
                if (k == 1)
                {
                    l1 = length2;
                    s1 = sum2;
                    d1 = temp2;
                }
                else if (k == 2)
                {
                    l2 = length2;
                    s2 = sum2;
                    d2 = temp2;
                }

            }



            double[] temp3 = new double[10000];
            Exp[] dataexp3 = new Exp[10000];
            int length3 = 0;
            double sum3 = 0.0;


            if (checkBox3.Checked)
            {
                OracleCommand cmd3 = conn.CreateCommand();
                cmd3.CommandText = "SELECT exp FROM mrnaexpression WHERE s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN(SELECT s_id  FROM patientsample  WHERE p_id  IN(SELECT p_id  FROM diagnosis      WHERE ds_id IN(SELECT ds_id FROM disease        WHERE name = 'Breast tumor')))) AND pb_id IN(SELECT pb_id FROM arrayprobe     WHERE u_id IN(SELECT u_id FROM goannotation WHERE go_id = '" + goid + "'))";
                OracleDataReader dr3 = cmd3.ExecuteReader();
                i = 0;
                
                while (dr3.Read())
                {
                    var abc = dr3.GetValue(0);
                    temp3[i] = Convert.ToDouble(abc);
                    sum3 = sum3 + temp3[i];
                    dataexp3[i] = new Exp();
                    dataexp3[i].EXP = Convert.ToInt32(abc);

                    i++;

                }
                length3 = i;
                lst[k] = 3;
                k++;
                if (k == 1)
                {
                    l1 = length3;
                    s1 = sum3;
                    d1 = temp3;
                }
                else if (k == 2)
                {
                    l2 = length3;
                    s2 = sum3;
                    d2 = temp3;
                }
                 else if(k==3)
                {
                    l3 = length3;
                    s3 = sum3;
                    d3 = temp3;
                }
            }

            double[] temp4 = new double[10000];
            Exp[] dataexp4 = new Exp[10000];
            int length4 = 0;
            double sum4 = 0.0;

            if (checkBox4.Checked)
            {
                OracleCommand cmd4 = conn.CreateCommand();
                cmd4.CommandText = "SELECT exp FROM mrnaexpression WHERE s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN(SELECT s_id  FROM patientsample  WHERE p_id  IN(SELECT p_id  FROM diagnosis      WHERE ds_id IN(SELECT ds_id FROM disease        WHERE name = 'Flu')))) AND pb_id IN(SELECT pb_id FROM arrayprobe     WHERE u_id IN(SELECT u_id FROM goannotation WHERE go_id = '" + goid + "'))";
                OracleDataReader dr4 = cmd4.ExecuteReader();
                i = 0;
                

                while (dr4.Read())
                {
                    var abc = dr4.GetValue(0);
                    temp4[i] = Convert.ToDouble(abc);
                    sum4 = sum4 + temp4[i];
                    dataexp4[i] = new Exp();
                    dataexp4[i].EXP = Convert.ToInt32(abc);

                    i++;

                }
                length4 = i;
                lst[k] = 4;
                k++;
                if (k == 1)
                {
                    l1 = length4;
                    s1 = sum4;
                    d1 = temp4;
                }
                else if (k == 2)
                {
                    l2 = length4;
                    s2 = sum4;
                    d2 = temp4;
                }
                else if (k == 3)
                {
                    l3 = length4;
                    s3 = sum4;
                    d3 = temp4;
                }
                else if (k == 4)
                {
                    l4 = length4;
                    s4 = sum4;
                    d4 = temp4;
                }

            }

            double[] temp5 = new double[10000];
            Exp[] dataexp5 = new Exp[10000];
            int length5 = 0;
            double sum5 = 0.0;

            if (checkBox5.Checked)
            {
                OracleCommand cmd5 = conn.CreateCommand();
                cmd5.CommandText = "SELECT exp FROM mrnaexpression WHERE s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN(SELECT s_id  FROM patientsample  WHERE p_id  IN(SELECT p_id  FROM diagnosis      WHERE ds_id IN(SELECT ds_id FROM disease        WHERE name = 'Colon tumor')))) AND pb_id IN(SELECT pb_id FROM arrayprobe     WHERE u_id IN(SELECT u_id FROM goannotation WHERE go_id = '" + goid + "'))";
                OracleDataReader dr5 = cmd5.ExecuteReader();
                i = 0;


                while (dr5.Read())
                {
                    var abc = dr5.GetValue(0);
                    temp5[i] = Convert.ToDouble(abc);
                    sum5 = sum5 + temp5[i];
                    dataexp5[i] = new Exp();
                    dataexp5[i].EXP = Convert.ToInt32(abc);

                    i++;

                }
                length5 = i;
                lst[k] = 5;
                k++;
                if (k == 1)
                {
                    l1 = length5;
                    s1 = sum5;
                    d1 = temp5;
                }
                else if (k == 2)
                {
                    l2 = length5;
                    s2 = sum5;
                    d2 = temp5;
                }
                else if (k == 3)
                {
                    l3 = length5;
                    s3 = sum5;
                    d3 = temp5;
                }
                else if (k == 4)
                {
                    l4 = length5;
                    s4 = sum5;
                    d4 = temp5;
                }
                else if (k == 5)
                {
                    l5 = length5;
                    s5 = sum5;
                    d5 = temp5;
                }
            }

            double[] temp6 = new double[10000];
            Exp[] dataexp6 = new Exp[10000];
            int length6 = 0;
            double sum6 = 0.0;

            if (checkBox6.Checked)
            {
                OracleCommand cmd6 = conn.CreateCommand();
                cmd6.CommandText = "SELECT exp FROM mrnaexpression WHERE s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN(SELECT s_id  FROM patientsample  WHERE p_id  IN(SELECT p_id  FROM diagnosis      WHERE ds_id IN(SELECT ds_id FROM disease        WHERE name = 'Giloblastome')))) AND pb_id IN(SELECT pb_id FROM arrayprobe     WHERE u_id IN(SELECT u_id FROM goannotation WHERE go_id = '" + goid + "'))";
                OracleDataReader dr6 = cmd6.ExecuteReader();
                i = 0;


                while (dr6.Read())
                {
                    var abc = dr6.GetValue(0);
                    temp6[i] = Convert.ToDouble(abc);
                    sum6 = sum6 + temp6[i];
                    dataexp6[i] = new Exp();
                    dataexp6[i].EXP = Convert.ToInt32(abc);

                    i++;

                }
                length6 = i;
                lst[k] = 6;
                k++;
                if (k == 1)
                {
                    l1 = length6;
                    s1 = sum6;
                    d1 = temp6;
                }
                else if (k == 2)
                {
                    l2 = length6;
                    s2 = sum6;
                    d2 = temp6;
                }
                else if (k == 3)
                {
                    l3 = length6;
                    s3 = sum6;
                    d3 = temp6;
                }
                else if (k == 4)
                {
                    l4 = length6;
                    s4 = sum6;
                    d4 = temp6;
                }
                else if (k == 5)
                {
                    l5 = length6;
                    s5 = sum6;
                    d5 = temp6;
                }
                else if (k == 6)
                {
                    l6 = length6;
                    s6 = sum6;
                    d6 = temp6;
                }
            }
            double f = 0.0;

            if (k==2)
            {
                f = FTest2(k, d1, s1, l1, d2, s2, l2); 
            }
            else if (k == 3)
            {
                 f = FTest3(k, d1, s1, l1, d2, s2, l2, d3, s3, l3);
            }
            else if (k == 4)
            {
                 f = FTest4(k, d1, s1, l1, d2, s2, l2, d3, s3, l3, d4, s4, l4);
            }
            else if (k == 5)
            {
                f = FTest5(k, d1, s1, l1, d2, s2, l2, d3, s3, l3, d4, s4, l4, d5, s5, l5);
            }
            else if (k == 6)
            {
                f = FTest6(k, d1, s1, l1, d2, s2, l2, d3, s3, l3, d4, s4, l4, d5, s5, l5, d6, s6, l6);
            }

            
            //double t = TTest(temp, temp2, length1, length2);
            label2.Text = "F-Test value is: " + f.ToString();

            conn.Close();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

      
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
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
    }
}
