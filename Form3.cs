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
    public partial class Form3 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private string description;
        public Form3()
        {
            InitializeComponent();

            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#

            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT distinct description FROM disease";
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

        private void button1_Click(object sender, EventArgs e)
        {
            string oradb = "DATA SOURCE=aos.acsu.buffalo.edu:1521/aos.buffalo.edu;PASSWORD=cse601;USER ID=AVIJEETM";
            OracleConnection conn = new OracleConnection(oradb);  // C#

            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Select DISTINCT type FROM DRUG where DR_ID in (Select a.dr_id from DRUGUSE a where a.p_id in (select distinct b.p_id from diagnosis b, disease c where b.ds_id = c.ds_id and c.description = '"+description +"'))";
            OracleDataReader dr = cmd.ExecuteReader();
            description[] desc = new description[10000];
            int i = 0;
            while (dr.Read())
            {
                var abc = dr.GetValue(0);
                
                desc[i] = new description();
                desc[i].DESCRIPTION = abc.ToString();

                i++;

            }
            bindingSource1.DataSource = desc;
            dataGridView1.DataSource = bindingSource1;

            conn.Close();
            label2.Text = "Types of Drugs found:" + i.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox1 = (ComboBox)sender;

            string selecteddescription = (string)comboBox1.SelectedItem;

            int resultIndex = -1;


            resultIndex = comboBox1.FindStringExact(selecteddescription);


            description = selecteddescription;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
    public class description
    {
        public string DESCRIPTION { get; set; }
    }
}
