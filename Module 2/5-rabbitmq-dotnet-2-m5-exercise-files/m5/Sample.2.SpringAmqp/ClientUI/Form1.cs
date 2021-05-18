using ClientUI.Gateways;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void getWeatherButton_Click(object sender, EventArgs e)
        {
            var ctx = ContextRegistry.GetContext();
            var weatherServiceClient = (IWeatherServiceGateway)ctx.GetObject("WeatherServiceGateway");
            var response = weatherServiceClient.Send(new Contract.WeatherRequest { Location = "London" });

            MessageBox.Show(string.Format("Location: {0}, Temp: {1}, Description:{2}", response.Location, response.Temperature, response.WeatherDescription));                    
            
        }

        private void getTimeButton_Click(object sender, EventArgs e)
        {
            var ctx = ContextRegistry.GetContext();    
            var timeServiceClient = (ITimeServiceGateway)ctx.GetObject("TimeServiceGateway");
            var response = timeServiceClient.Send(new Contract.TimeRequest());

            MessageBox.Show(string.Format("Time: {0}", response.Time.ToShortTimeString()));
            
        }
    }
}
