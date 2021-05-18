using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinClient
{
    public partial class Form1 : Form
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string HoldingQueueName = "Module3.Sample11.HoldingQueue";

        private int messageCount;

        public Form1()
        {
            InitializeComponent();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            #region Connect to RabbitMQ
            var connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            #endregion
            
            

            //Setup properties
            var properties = model.CreateBasicProperties();
            properties.SetPersistent(true);
            properties.Expiration = "5000";
            
            //Serialize
            byte[] messageBuffer = Encoding.Default.GetBytes(messageCount.ToString());            

            //Send message
            model.BasicPublish("", HoldingQueueName, properties, messageBuffer);

            MessageBox.Show(string.Format("Sending Message - Message - {0}", messageCount.ToString()), "Message sent");

            messageCount++;
        }

        private static string GetComboItem(ComboBox comboBox)
        {
            if (string.IsNullOrEmpty(comboBox.Text))
                return string.Empty;
            return comboBox.Text;
        }
    }
}
