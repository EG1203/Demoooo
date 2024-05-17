using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Demo
{
    public partial class AddEditAgentForm : Form
    {
        private int agentID;
        private string connectionString = "Your Connection String Here";

        public AddEditAgentForm(int agentID = 0)
        {
            InitializeComponent();
            this.agentID = agentID;

            if (agentID != 0)
            {
                LoadAgent();
            }
        }

        private void LoadAgent()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Agents WHERE AgentID = @agentID", connection);
                command.Parameters.AddWithValue("@agentID", agentID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    textBoxName.Text = reader["CompanyName"].ToString();
                    comboBoxType.SelectedItem = reader["AgentType"].ToString();
                    textBoxAddress.Text = reader["LegalAddress"].ToString();
                    textBoxINN.Text = reader["INN"].ToString();
                    textBoxKPP.Text = reader["KPP"].ToString();
                    textBoxDirector.Text = reader["DirectorName"].ToString();
                    textBoxPhone.Text = reader["ContactPhone"].ToString();
                    textBoxEmail.Text = reader["ContactEmail"].ToString();
                    numericUpDownPriority.Value = Convert.ToInt32(reader["SupplyPriority"]);
                    pictureBoxLogo.Image = File.Exists("Resources/" + reader["Logo"].ToString()) ? Image.FromFile("Resources/" + reader["Logo"].ToString()) : null;
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command;
                if (agentID == 0)
                {
                    command = new SqlCommand(
                        "INSERT INTO Agents (CompanyName, AgentType, LegalAddress, INN, KPP, DirectorName, ContactPhone, ContactEmail, SupplyPriority, Logo) " +
                        "VALUES (@CompanyName, @AgentType, @LegalAddress, @INN, @KPP, @DirectorName, @ContactPhone, @ContactEmail, @SupplyPriority, @Logo)", connection);
                }
                else
                {
                    command = new SqlCommand(
                        "UPDATE Agents SET CompanyName = @CompanyName, AgentType = @AgentType, LegalAddress = @LegalAddress, INN = @INN, KPP = @KPP, " +
                        "DirectorName = @DirectorName, ContactPhone = @ContactPhone, ContactEmail = @ContactEmail, SupplyPriority = @SupplyPriority, Logo = @Logo " +
                        "WHERE AgentID = @agentID", connection);
                    command.Parameters.AddWithValue("@agentID", agentID);
                }

                command.Parameters.AddWithValue("@CompanyName", textBoxName.Text);
                command.Parameters.AddWithValue("@AgentType", comboBoxType.SelectedItem.ToString());
                command.Parameters.AddWithValue("@LegalAddress", textBoxAddress.Text);
                command.Parameters.AddWithValue("@INN", textBoxINN.Text);
                command.Parameters.AddWithValue("@KPP", textBoxKPP.Text);
                command.Parameters.AddWithValue("@DirectorName", textBoxDirector.Text);
                command.Parameters.AddWithValue("@ContactPhone", textBoxPhone.Text);
                command.Parameters.AddWithValue("@ContactEmail", textBoxEmail.Text);
                command.Parameters.AddWithValue("@SupplyPriority", numericUpDownPriority.Value);
                command.Parameters.AddWithValue("@Logo", "picture.png"); // Здесь вы можете указать путь к вашему логотипу

                command.ExecuteNonQuery();
            }
            DialogResult = DialogResult.OK;
        }

        private void buttonLoadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxLogo.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (agentID != 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Agents WHERE AgentID = @agentID", connection);
                    command.Parameters.AddWithValue("@agentID", agentID);
                    command.ExecuteNonQuery();
                }
            }
            DialogResult = DialogResult.OK;
        }
    }
}
