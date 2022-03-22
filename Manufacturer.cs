using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WindowsFormsApp10
{
    public partial class Manufacturer : Form
    {
        public Manufacturer()
        {
            InitializeComponent();
            ShowMan();
        }



        SqlConnection Con = new SqlConnection("Data Source=DESKTOP-UVVBQS2;Initial Catalog=Pharmacy;Integrated Security=True");

        private void ShowMan()
        {
            Con.Open();
            string Query = "Select * from Manufacturer";
            SqlDataAdapter da = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_mfname.Text == "" || txt_mfname.Text.Any(char.IsDigit))
                MetroFramework.MetroMessageBox.Show(this, "Manufacturer Name cannot be blank and cannot contain numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txt_address.Text == "")
                MetroFramework.MetroMessageBox.Show(this, "Address cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if(txt_tp.Text.Length != 10 || txt_tp.Text.Any(char.IsLetter))
                MetroFramework.MetroMessageBox.Show(this, "Phone must include 10 numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                
                    Con.Open();
                   SqlCommand cmd = new SqlCommand("Insert into Manufacturer values('" + txt_mfname.Text + "','" + txt_address.Text + "','" + txt_tp.Text + "','" + date_time.Value + "')", Con);
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                        MetroFramework.MetroMessageBox.Show(this,"Data saved successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MetroFramework.MetroMessageBox.Show(this,"Data cannot be saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Con.Close();
                ShowMan();
                
                
            }
        }    
            
        private void Manufacturer_Load(object sender, EventArgs e)
        {
           
        }

        private void btn_update_Click(object sender, EventArgs e)
        {try
            {
                Regex nonNumericRegex = new Regex(@"\D");
                if (nonNumericRegex.IsMatch(txt_id.Text))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Please enter correct Manufacturer ID to update data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update Manufacturer set ManName='" + txt_mfname.Text + "',ManAddress='" + txt_address.Text + "',ManTP='" + txt_tp.Text + "',ManJDate='" + date_time.Value + "' where ManId='" + txt_id.Text + "'", Con);
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                        MetroFramework.MetroMessageBox.Show(this, "Manufacturer updated successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MetroFramework.MetroMessageBox.Show(this, "Manufacturer cannot be updated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Con.Close();
                    ShowMan();


                }
            }
            catch(FormatException)
            {
                MetroFramework.MetroMessageBox.Show(this, "Format is not supported, please check again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "Error detected, please check again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            Regex nonNumericRegex = new Regex(@"\D");
            if (nonNumericRegex.IsMatch(txt_id.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Please enter correct Manufacturer ID to delete data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                {

                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from Manufacturer where ManId='" + txt_id.Text + "'", Con);
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                        MetroFramework.MetroMessageBox.Show(this, "Manufacturer deleted successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MetroFramework.MetroMessageBox.Show(this, "Manufacturer cannot be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Con.Close();
                    ShowMan();


                }
            
        }

        private void btn_cls_Click(object sender, EventArgs e)
        {
            txt_mfname.Clear();
            txt_id.Clear();
            txt_address.Clear();
            txt_tp.Clear();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
