using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.IO;

namespace Demo

{
    public partial class MainForm : Form
    {
        private int currentPage = 1;
        private int totalPages = 1;

        public MainForm()
        {
            InitializeComponent();
        }
 private string connectionString = "Data Source=DESKTOP-RJC8724\\SQLEXPRESS01;Initial Catalog=Demo;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";


        
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadAgents();
            UpdatePageNavigation();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            LoadAgents();
        }

        private void comboBoxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAgents();
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAgents();
        }

        private void flowLayoutPanel_Paint(object sender, PaintEventArgs e)
        {
            // Можно оставить пустым или добавить кастомную логику рисования
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadAgents(currentPage);
                UpdatePageNavigation();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadAgents(currentPage);
                UpdatePageNavigation();
            }
        }

        private void LoadAgents(int page = 1)
        {
            string filter = textBoxSearch.Text.Trim();
            string sort = comboBoxSort.SelectedItem?.ToString() ?? "Имя по возрастанию";
            string agentType = comboBoxFilter.SelectedItem?.ToString() ?? "Все типы";

            string query = "SELECT * FROM Agents WHERE (CompanyName LIKE @filter OR ContactPhone LIKE @filter)";

            if (agentType != "Все типы")
            {
                query += " AND AgentType = @agentType";
            }

            query += " ORDER BY ";

            switch (sort)
            {
                case "Имя по возрастанию":
                    query += "CompanyName ASC";
                    break;
                case "Имя по убыванию":
                    query += "CompanyName DESC";
                    break;
                case "Скидка по возрастанию":
                    query += "Discount ASC";
                    break;
                case "Скидка по убыванию":
                    query += "Discount DESC";
                    break;
                case "Приоритет по возрастанию":
                    query += "Priority ASC";
                    break;
                case "Приоритет по убыванию":
                    query += "Priority DESC";
                    break;
                default:
                    query += "CompanyName ASC";
                    break;
            }

            query += " OFFSET @Offset ROWS FETCH NEXT 10 ROWS ONLY";

            flowLayoutPanel.Controls.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Agents WHERE (CompanyName LIKE @filter OR ContactPhone LIKE @filter)", connection);
                command.Parameters.AddWithValue("@filter", "%" + filter + "%");
                if (agentType != "Все типы")
                {
                    command.Parameters.AddWithValue("@agentType", agentType);
                }
                int totalAgents = (int)command.ExecuteScalar();
                totalPages = (int)Math.Ceiling((double)totalAgents / 10);

                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@filter", "%" + filter + "%");
                command.Parameters.AddWithValue("@Offset", (page - 1) * 10);
                if (agentType != "Все типы")
                {
                    command.Parameters.AddWithValue("@agentType", agentType);
                }

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string agentId = reader["AgentID"].ToString();
                    int salesCount = CalculateSalesCount(agentId);
                    int discount = CalculateDiscount(agentId);
                    string phone = reader["ContactPhone"].ToString();
                    string companyName = reader["CompanyName"].ToString();
                    string discountString = discount + "%";

                    // Логирование для проверки
                    Console.WriteLine($"AgentID: {agentId}, SalesCount: {salesCount}, Discount: {discount}, Phone: {phone}, CompanyName: {companyName}");

                    AgentControl agentControl = new AgentControl
                    {
                        Наименование = companyName,
                        КоличествоПродаж = salesCount.ToString(),
                        Скидка = discountString,
                        Телефон = phone,
                        Логотип = File.Exists("Resources/picture.png") ? Image.FromFile("Resources/picture.png") : null,
                        Tag = agentId
                    };

                    if (discount >= 25)
                    {
                        agentControl.BackColor = Color.LightGreen;
                    }

                    agentControl.DoubleClick += agentControl_DoubleClick;
                    flowLayoutPanel.Controls.Add(agentControl);
                }
            }
        }

        private void UpdatePageNavigation()
        {
            pagePanel.Controls.Clear();
            for (int i = 1; i <= totalPages; i++)
            {
                Button pageButton = new Button
                {
                    Text = i.ToString(),
                    Width = 30,
                    Height = 30,
                    Tag = i,
                    Margin = new Padding(2)
                };
                pageButton.Click += PageButton_Click;
                pagePanel.Controls.Add(pageButton);
            }

            if (currentPage > 1)
            {
                Button prevButton = new Button
                {
                    Text = "Предыдущая",
                    Width = 80,
                    Height = 30,
                    Margin = new Padding(2)
                };
                prevButton.Click += buttonPrevious_Click;
                pagePanel.Controls.Add(prevButton);
            }

            if (currentPage < totalPages)
            {
                Button nextButton = new Button
                {
                    Text = "Следующая",
                    Width = 80,
                    Height = 30,
                    Margin = new Padding(2)
                };
                nextButton.Click += buttonNext_Click;
                pagePanel.Controls.Add(nextButton);
            }
        }

        private void PageButton_Click(object sender, EventArgs e)
        {
            currentPage = (int)((Button)sender).Tag;
            LoadAgents(currentPage);
            UpdatePageNavigation();
        }

        private int CalculateSalesCount(string agentId)
        {
            int salesCount = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "SELECT SUM(QuantityProduction) " +
                    "FROM Products " +
                    "WHERE AgentName = @agentName AND RealisationDate >= DATEADD(YEAR, -1, GETDATE())",
                    connection);
                command.Parameters.AddWithValue("@agentName", agentId);
                object result = command.ExecuteScalar();
                salesCount = result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
            return salesCount;
        }

        private int CalculateDiscount(string agentId)
        {
            int totalSales = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "SELECT SUM(QuantityProduction * MinimumAgentPrice) " +
                    "FROM Products " +
                    "WHERE AgentName = @agentName",
                    connection);
                command.Parameters.AddWithValue("@agentName", agentId);
                object result = command.ExecuteScalar();
                totalSales = result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }

            if (totalSales < 10000)
                return 0;
            else if (totalSales < 50000)
                return 5;
            else if (totalSales < 150000)
                return 10;
            else if (totalSales < 500000)
                return 20;
            else
                return 25;
        }

        private void buttonAddAgent_Click(object sender, EventArgs e)
        {
            AddEditAgentForm addEditAgentForm = new AddEditAgentForm();
            if (addEditAgentForm.ShowDialog() == DialogResult.OK)
            {
                LoadAgents(currentPage);
            }
        }

        private void agentControl_DoubleClick(object sender, EventArgs e)
        {
            AgentControl agentControl = sender as AgentControl;
            int agentID = int.Parse(agentControl.Tag.ToString());
            AddEditAgentForm addEditAgentForm = new AddEditAgentForm(agentID);
            if (addEditAgentForm.ShowDialog() == DialogResult.OK)
            {
                LoadAgents(currentPage);
            }
        }
    }
}