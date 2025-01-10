using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using DapperSqlCourse.Dtos;

namespace DapperSqlCourse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection=new SqlConnection("Server=EMU2025\\MSSQLSERVER01;Initial Catalog=DapperSql;integrated security=true");
        private async void Form1_Load(object sender, EventArgs e)
        {
            string query1 = "Select Count(*) From TblProduct";
            var productTotalCount = await connection.QueryFirstOrDefaultAsync<int>(query1);
            TotalBookCount.Text = productTotalCount.ToString();

            string query2 = "Select ProductName From TblProduct Where ProductPrice=(Select Max(ProductPrice) From TblProduct)";
            var macPriceProductName= await connection.QueryFirstOrDefaultAsync<string>(query2);
            MaxBookName.Text = macPriceProductName.ToString();

            string query3 = "Select Count(Distinct(ProductCategory)) from TblProduct";
            var distincProductCount =await connection.QueryFirstOrDefaultAsync<int>(query3);
            distincCategoryCount.Text = distincProductCount.ToString();

        }
        private async void button1_Click(object sender, EventArgs e)
        {
            string query = "Select * From TblProduct";
            var values = await connection.QueryAsync<ResultProductDto>(query);
            dataGridView1.DataSource = values;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string query = "Insert into TblProduct (ProductName,ProductPrice,ProductStock,ProductCategory) values (@p1,@p2,@p3,@p4)";
            var parameters = new DynamicParameters();
            parameters.Add("@p1", textBox2.Text);
            parameters.Add("@p2", textBox3.Text);
            parameters.Add("@p3", textBox4.Text);
            parameters.Add("@p4", textBox5.Text);
            await connection.ExecuteAsync(query, parameters);//query calistir paramaetreden gelen degerlerle
            MessageBox.Show("ADDED");

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string query = "Delete From TblProduct Where ProductId=@p1";
            var parameters = new DynamicParameters();
            parameters.Add("@p1", textBox1.Text);
            await connection.ExecuteAsync(query,parameters);
            MessageBox.Show("Deleted");
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            string query = "Update TblProduct Set ProductName=@p1,ProductPrice=@p2,ProductStock=@p3,ProductCategory=@p4 where ProductId=@p5";

            var parameters = new DynamicParameters();
            parameters.Add("@p1", textBox2.Text);
            parameters.Add("@p2", textBox3.Text);
            parameters.Add("@p3", textBox4.Text);
            parameters.Add("@p4", textBox5.Text);
            parameters.Add("@p5", textBox1.Text);
            await connection.ExecuteAsync(query,parameters);
            MessageBox.Show("Updated");
        }
    }
}
