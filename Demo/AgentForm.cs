using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Demo
{
    public partial class AgentForm : Form
    {
        private int agentID;
        private string connectionString = "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;";

        public AgentForm(int agentID = 0)
        {
            
            this.agentID = agentID;
            if (agentID != 0)
            {
                LoadAgent();
            }
        }

        //private void InitializeComponent()
        //{
        //    this.textBoxName = new System.Windows.Forms.TextBox();
        //    this.comboBoxType = new System.Windows.Forms.ComboBox();
        //    this.textBoxAddress = new System.Windows.Forms.TextBox();
        //    this.textBoxINN = new System.Windows.Forms.TextBox();
        //    this.textBoxKPP = new System.Windows.Forms.TextBox();
        //    this.textBoxDirector = new System.Windows.Forms.TextBox();
        //    this.textBoxPhone = new System.Windows.Forms.TextBox();
        //    this.textBoxEmail = new System.Windows.Forms.TextBox();
        //    this.numericUpDownPriority = new System.Windows.Forms.NumericUpDown();
        //    this.buttonSave = new System.Windows.Forms.Button();
        //    ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriority)).BeginInit();
        //    this.SuspendLayout();
        //    // 
        //    // textBoxName
        //    // 
        //    this.textBoxName.Location = new System.Drawing.Point(12, 12);
        //    this.textBoxName.Name = "textBoxName";
        //    this.textBoxName.Size = new System.Drawing.Size(260, 20);
        //    this.textBoxName.TabIndex = 0;
        //    // 
        //    // comboBoxType
        //    // 
        //    this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        //    this.comboBoxType.FormattingEnabled = true;
        //    this.comboBoxType.Location = new System.Drawing.Point(12, 38);
        //    this.comboBoxType.Name = "comboBoxType";
        //    this.comboBoxType.Size = new System.Drawing.Size(260, 21);
        //    this.comboBoxType.TabIndex = 1;
        //    // 
        //    // textBoxAddress
        //    // 
        //    this.textBoxAddress.Location = new System.Drawing.Point(12, 65);
        //    this.textBoxAddress.Name = "textBoxAddress";
        //    this.textBoxAddress.Size = new System.Drawing.Size(260, 20);
        //    this.textBoxAddress.TabIndex = 2;
        //    // 
        //    // textBoxINN
        //    // 
        //    this.textBoxINN.Location = new System.Drawing.Point(12, 91);
        //    this.textBoxINN.Name = "textBoxINN";
        //    this.textBoxINN.Size = new System.Drawing.Size(260, 20);
        //    this.textBoxINN.TabIndex = 3;
        //    // 
        //    // textBoxKPP
        //    // 
        //    this.textBoxKPP.Location = new System.Drawing.Point(12, 117);
        //    this.textBoxKPP.Name = "textBoxKPP";
        //    this.textBoxKPP.Size = new System.Drawing.Size(260, 20);
        //    this.textBoxKPP.TabIndex = 4;
        //    // 
        //    // textBoxDirector
        //    // 
        //    this.textBoxDirector.Location = new System.Drawing.Point(12, 143);
        //    this.textBoxDirector.Name = "textBoxDirector";
        //    this.textBoxDirector.Size = new System.Drawing.Size(260, 20);
        //    this.textBoxDirector.TabIndex = 5;
        //    // 
        //    // textBoxPhone
        //    // 
        //    this.textBoxPhone.Location = new System.Drawing.Point(12, 169);
        //    this.textBoxPhone.Name = "textBoxPhone";
        //    this.textBoxPhone.Size = new System.Drawing.Size(260, 20);
        //    this.textBoxPhone.TabIndex = 6;
        //    // 
        //    // textBoxEmail
        //    // 
        //    this.textBoxEmail.Location = new System.Drawing.Point(12, 195);
        //    this.textBoxEmail.Name = "textBoxEmail";
        //    this.textBoxEmail.Size = new System.Drawing.Size(260, 20);
        //    this.textBoxEmail.TabIndex = 7;
        //    // 
        //    // numericUpDownPriority
        //    // 
        //    this.numericUpDownPriority.Location = new System.Drawing.Point(12, 221);
        //    this.numericUpDownPriority.Name = "numericUpDownPriority";
        //    this.numericUpDownPriority.Size = new System.Drawing.Size(120, 20);
        //    this.numericUpDownPriority.TabIndex = 8;
        //    // 
        //    // buttonSave
        //    // 
        //    this.buttonSave.Location = new System.Drawing.Point(197, 247);
        //    this.buttonSave.Name = "buttonSave";
        //    this.buttonSave.Size = new System.Drawing.Size(75, 23);
        //    this.buttonSave.TabIndex = 9;
        //    this.buttonSave.Text = "Сохранить";
        //    this.buttonSave.UseVisualStyleBackColor = true;
        //    this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
        //    // 
        //    // AgentForm
        //    // 
        //    this.ClientSize = new System.Drawing.Size(284, 281);
        //    this.Controls.Add(this.buttonSave);
        //    this.Controls.Add(this.numericUpDownPriority);
        //    this.Controls.Add(this.textBoxEmail);
        //    this.Controls.Add(this.textBoxPhone);
        //    this.Controls.Add(this.textBoxDirector);
        //    this.Controls.Add(this.textBoxKPP);
        //    this.Controls.Add(this.textBoxINN);
        //    this.Controls.Add(this.textBoxAddress);
        //    this.Controls.Add(this.comboBoxType);
        //    this.Controls.Add(this.textBoxName);
        //    this.Name = "AgentForm";
        //    this.Text = "Добавить/Редактировать агента";
        //    ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriority)).EndInit();
        //    this.ResumeLayout(false);
        //    this.PerformLayout();
        //}

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
                    textBoxAddress.Text = reader["Address"].ToString();
                    textBoxINN.Text = reader["INN"].ToString();
                    textBoxKPP.Text = reader["KPP"].ToString();
                    textBoxDirector.Text = reader["Director"].ToString();
                    textBoxPhone.Text = reader["Phone"].ToString();
                    textBoxEmail.Text = reader["Email"].ToString();
                    numericUpDownPriority.Value = Convert.ToInt32(reader["Priority"]);
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
                    command = new SqlCommand("INSERT INTO Agents (CompanyName, AgentType, Address, INN, KPP, Director, Phone, Email, Priority) VALUES (@CompanyName, @AgentType, @Address, @INN, @KPP, @Director, @Phone, @Email, @Priority)", connection);
                }
                else
                {
                    command = new SqlCommand("UPDATE Agents SET CompanyName = @CompanyName, AgentType = @AgentType, Address = @Address, INN = @INN, KPP = @KPP, Director = @Director, Phone = @Phone, Email = @Email, Priority = @Priority WHERE AgentID = @agentID", connection);
                    command.Parameters.AddWithValue("@agentID", agentID);
                }
                command.Parameters.AddWithValue("@CompanyName", textBoxName.Text);
                command.Parameters.AddWithValue("@AgentType", comboBoxType.SelectedItem.ToString());
                command.Parameters.AddWithValue("@Address", textBoxAddress.Text);
                command.Parameters.AddWithValue("@INN", textBoxINN.Text);
                command.Parameters.AddWithValue("@KPP", textBoxKPP.Text);
                command.Parameters.AddWithValue("@Director", textBoxDirector.Text);
                command.Parameters.AddWithValue("@Phone", textBoxPhone.Text);
                command.Parameters.AddWithValue("@Email", textBoxEmail.Text);
                command.Parameters.AddWithValue("@Priority", numericUpDownPriority.Value);
                command.ExecuteNonQuery();
            }
            this.DialogResult = DialogResult.OK;
        }

        private TextBox textBoxName;
        private ComboBox comboBoxType;
        private TextBox textBoxAddress;
        private TextBox textBoxINN;
        private TextBox textBoxKPP;
        private TextBox textBoxDirector;
        private TextBox textBoxPhone;
        private TextBox textBoxEmail;
        private NumericUpDown numericUpDownPriority;
        private Button buttonSave;

        private void AgentForm_Load(object sender, EventArgs e)
        {

        }
    }
}
