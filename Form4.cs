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
    public partial class Form4 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private string name;
        private string cid;
        private string mu;
        public Form4()
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

            OracleCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = "SELECT cl_id FROM clustermaster order by cl_id";
            OracleDataReader dr5 = cmd2.ExecuteReader();
            while (dr5.Read())
            {
                var abc = dr5.GetValue(0);
                this.comboBox2.Items.Add(abc.ToString());
            }

            this.comboBox2.Sorted = true;
            this.comboBox2.Refresh();

            OracleCommand cmd3 = conn.CreateCommand();
            cmd3.CommandText = "SELECT mu_id FROM measureunit";
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

        private void button1_Click(object sender, EventArgs e)
        {
            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#

            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT exp FROM mrnaexpression WHERE s_id IN (SELECT s_id  FROM clinicalsample WHERE s_id  IN(SELECT s_id  FROM patientsample  WHERE p_id  IN(SELECT p_id  FROM diagnosis WHERE ds_id IN(SELECT ds_id FROM disease WHERE name = '"+ name +"')))) AND pb_id IN (SELECT pb_id FROM arrayprobe WHERE u_id IN (SELECT u_id FROM genecluster WHERE cl_id = '"+cid+"')) AND mu_id = '"+mu+"'";
            OracleDataReader dr = cmd.ExecuteReader();

            Exp[] exp = new Exp[10000];
            int i = 0;
            while (dr.Read())
            {
                var abc = dr.GetValue(0);

                exp[i] = new Exp();
                exp[i].EXP = Convert.ToInt32(abc);

                i++;

            }
            bindingSource1.DataSource = exp;
            dataGridView1.DataSource = bindingSource1;

            conn.Close();
            label4.Text = "Expressions Found:" + i.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox1 = (ComboBox)sender;

            string selectedname = (string)comboBox1.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox1.FindStringExact(selectedname);


            name = selectedname;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox2 = (ComboBox)sender;

            string selectedcid = (string)comboBox2.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox2.FindStringExact(selectedcid);


            cid = selectedcid;

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox3 = (ComboBox)sender;

            string selectedmu = (string)comboBox3.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox3.FindStringExact(selectedmu);


            mu = selectedmu;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
