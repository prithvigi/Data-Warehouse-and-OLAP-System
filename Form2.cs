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
    public partial class Form2 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private string name;
        private string type;
        private string description;
        public Form2()
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
            cmd2.CommandText = "SELECT distinct type FROM disease";
            OracleDataReader dr5 = cmd2.ExecuteReader();
            while (dr5.Read())
            {
                var abc = dr5.GetValue(0);
                this.comboBox2.Items.Add(abc.ToString());
             }

            this.comboBox2.Sorted = true;
            this.comboBox2.Refresh();

            OracleCommand cmd3 = conn.CreateCommand();
            cmd3.CommandText = "SELECT distinct description FROM disease";
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
            cmd.CommandText = "SELECT b.description Disease,count(a.P_ID) Patients FROM DIAGNOSIS a ,DISEASE b where a.ds_id = b.ds_id AND b.DESCRIPTION = '" + description +"' GROUP BY b.description UNION SELECT b.type,count(a.P_ID) FROM DIAGNOSIS a ,DISEASE b where a.ds_id = b.ds_id AND b.type = '"+type+"' GROUP BY b.type UNION SELECT b.name,count(a.P_ID) FROM DIAGNOSIS a ,DISEASE b where a.ds_id = b.ds_id AND b.name = '"+name+"' GROUP BY b.name";
            OracleDataReader dr = cmd.ExecuteReader();

            bindingSource1.DataSource = dr;
            dataGridView1.DataSource = bindingSource1;
            conn.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
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

            string selectedtype = (string)comboBox2.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox2.FindStringExact(selectedtype);


            type = selectedtype;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox3 = (ComboBox)sender;

            string selecteddescription = (string)comboBox3.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox3.FindStringExact(selecteddescription);


            description = selecteddescription;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
