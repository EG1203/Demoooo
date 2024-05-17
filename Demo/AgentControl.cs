using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    public partial class AgentControl : UserControl
    {
        public AgentControl()
        {
            InitializeComponent();
        }

        public string Наименование
        {
            get { return labelName.Text; }
            set { labelName.Text = value; }
        }

        public string КоличествоПродаж
        {
            get { return labelSales.Text; }
            set { labelSales.Text = value; }
        }

        public string Скидка
        {
            get { return labelDiscount.Text; }
            set { labelDiscount.Text = value; }
        }

        public string Телефон
        {
            get { return labelPhone.Text; }
            set { labelPhone.Text = value; }
        }
        //public string ТипАгента
        //{
        //    get { return labelAgentType.Text; }
        //    set { labelAgentType.Text = value; }
        //}

        public Image Логотип
        {
            get { return pictureBox.Image; }
            set { pictureBox.Image = value; }
        }

        public void LoadImage(string path)
        {
            pictureBox.Image = Image.FromFile(path);
        }
    }
}
