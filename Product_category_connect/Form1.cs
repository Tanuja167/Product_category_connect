using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

using System.Windows.Forms;
using System.Xml.Linq;
using System.Data;

namespace Product_category_connect
{
    public partial class Form1 : Form
    {

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;


        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "update Product set pname=@name,price=@price, cid=@cid where id=@id";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtname.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToInt32(txtprice.Text));
                cmd.Parameters.AddWithValue("@cid", Convert.ToInt32(cmbcid.SelectedValue));
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record updated");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllEmps();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Category> list = new List<Category>();
                string qry = "select * from Category";
                cmd = new SqlCommand(qry, con);
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                       Category category = new Category();
                        category.CID = Convert.ToInt32(reader["cid"]);
                        category.CName= reader["cname"].ToString();
                        list.Add(category);
                    }
                }
                // display dname & on selection of dname we need did
                cmbcid.DataSource = list;
                cmbcid.DisplayMember = "cname";
                cmbcid.ValueMember = "cid";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "insert into Product values(@Name,@Price,@Cid)";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtname.Text);
                cmd.Parameters.AddWithValue("@Price", Convert.ToInt32(txtprice.Text));
                cmd.Parameters.AddWithValue("@Cid", Convert.ToInt32(cmbcid.SelectedValue));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllEmps();

        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "delete from Product where id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record deleted");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllEmps();

        }
        private void GetAllEmps()
        {
            string qry = "select p.*, c.cname from Product p inner join Category c on c.cid = p.cid";
            cmd = new SqlCommand(qry, con);
            con.Open();
            reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table; 
            con.Close();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.*, c.cname from Product p inner join Category c on c.cid = p.cid where p.id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtid.Text));
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        txtname.Text = reader["pname"].ToString();
                        txtprice.Text = reader["price"].ToString();
                        cmbcid.Text = reader["cname"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Record not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }

        }
    }
}
