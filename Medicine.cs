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
    public partial class Medicine : Form
    {
        public Medicine()
        {
            InitializeComponent();
            ShowMed();
            GetManufacturer();
        }

        SqlConnection Con = new SqlConnection("Data Source=DESKTOP-UVVBQS2;Initial Catalog=Pharmacy;Integrated Security=True");

        private void ShowMed()
        {
            Con.Open();
            string Query = "Select * from Medicine";
            SqlDataAdapter da = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void GetManufacturer()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select ManId from Manufacturer",Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("ManId", typeof(int));
            dt.Load(rdr);
            cmb_mnf.ValueMember = "ManId";
            cmb_mnf.DataSource = dt;
            Con.Close();

        }
        private void GetManName()
        {
            Con.Open();
            string Query = "Select * from Manufacturer where ManId ='" + cmb_mnf.SelectedValue.ToString() + "'";
            SqlCommand cmd = new SqlCommand(Query,Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                txt_mfname.Text = dr["ManName"].ToString();
            }
            Con.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_mdname.Text == "" || txt_mdname.Text.Any(char.IsDigit))
                MetroFramework.MetroMessageBox.Show(this, "Medicine Name cannot be blank and cannot contain numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txt_qty.Text == "" || txt_qty.Text.Any(char.IsLetter))
                MetroFramework.MetroMessageBox.Show(this, "Quantity cannot be blank and cannot contain letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txt_price.Text == "" || txt_price.Text.Any(char.IsLetter))
                MetroFramework.MetroMessageBox.Show(this, "Price cannot be blank and cannot contain letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if(cmb_type.SelectedIndex == -1)
                MetroFramework.MetroMessageBox.Show(this, "Please select a Medicine Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if(cmb_mnf.SelectedIndex == -1)
                MetroFramework.MetroMessageBox.Show(this, "Please select a Manufacturer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {

                Con.Open();
                SqlCommand cmd = new SqlCommand("Insert into Medicine values('" + txt_mdname.Text + "','" + cmb_type.SelectedItem.ToString() + "','" + txt_qty.Text + "','" + txt_price.Text + "','"+cmb_mnf.SelectedValue.ToString()+"','"+txt_mfname.Text+"')", Con);
                int i = cmd.ExecuteNonQuery();
                if (i == 1)
                    MetroFramework.MetroMessageBox.Show(this, "Medicine saved successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MetroFramework.MetroMessageBox.Show(this, "Medicine cannot be saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Con.Close();
                ShowMed();


            }
        }

        private void btn_cls_Click(object sender, EventArgs e)
        {
            txt_mdno.Clear();
            txt_mdname.Clear();
            txt_mfname.Clear();
            txt_price.Clear();
            txt_qty.Clear();
            
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                Regex nonNumericRegex = new Regex(@"\D");
                if (nonNumericRegex.IsMatch(txt_mdno.Text))
                    MetroFramework.MetroMessageBox.Show(this, "Please enter correct Medicine No to update data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (txt_mdname.Text == "" || txt_mdname.Text.Any(char.IsDigit))
                    MetroFramework.MetroMessageBox.Show(this, "Medicine Name cannot be blank and cannot contain numbers in order to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (txt_qty.Text == "" || txt_qty.Text.Any(char.IsLetter))
                    MetroFramework.MetroMessageBox.Show(this, "Quantity cannot be blank and cannot contain letters in order to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (txt_price.Text == "" || txt_price.Text.Any(char.IsLetter))
                    MetroFramework.MetroMessageBox.Show(this, "Price cannot be blank and cannot contain letters in order to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (cmb_type.SelectedIndex == -1)
                    MetroFramework.MetroMessageBox.Show(this, "Please select a Medicine Type in order to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (cmb_mnf.SelectedIndex == -1)
                    MetroFramework.MetroMessageBox.Show(this, "Please select a Manufacturer in order to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {

                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update Medicine set MedName='" + txt_mdname.Text + "',MedType='" + cmb_type.SelectedItem.ToString()+ "',MedQty='" + txt_qty.Text + "',MedPrice='" + txt_price.Text + "',MedManId='"+cmb_mnf.SelectedValue.ToString()+"', MedManufact='"+txt_mfname.Text+"' where MedNum='" + txt_mdno.Text + "'", Con);
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                        MetroFramework.MetroMessageBox.Show(this, "Medicine updated successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MetroFramework.MetroMessageBox.Show(this, "Medicine cannot be updated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Con.Close();
                    ShowMed();


                }
            }
            catch (FormatException)
            {
                MetroFramework.MetroMessageBox.Show(this, "Format is not supported, please check again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "Error detected, please check again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Medicine_Load(object sender, EventArgs e)
        {

        }

        private void cmb_mnf_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetManName();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            Regex nonNumericRegex = new Regex(@"\D");
            if (nonNumericRegex.IsMatch(txt_mdno.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Please enter correct Medicine No to delete data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                Con.Open();
                SqlCommand cmd = new SqlCommand("Delete from Medicine where MedNum='" + txt_mdno.Text + "'", Con);
                int i = cmd.ExecuteNonQuery();
                if (i == 1)
                    MetroFramework.MetroMessageBox.Show(this, "Medicine deleted successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MetroFramework.MetroMessageBox.Show(this, "Medicine cannot be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Con.Close();
                ShowMed();


            }
        }
    }
}
