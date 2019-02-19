using IqOptionApi;
using IqOptionApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trader_AI
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        public async Task RunSample()
        {

            var api = new IqOptionApi.IqOptionApi(textBox1.Text, textBox2.Text);


            if (await api.ConnectAsync())
            {

                //get profile
                var profile = await api.GetProfileAsync();


                // get candles data
                var candles = await api.GetCandlesAsync(ActivePair.AUDJPY, TimeFrame.Min1, 100, DateTimeOffset.Now);

                


                // subscribe to pair to get real-time data for tf1min and tf5min
                var streamMin1 = await api.SubscribeRealtimeDataAsync(ActivePair.AUDJPY, TimeFrame.Min1);

                


                streamMin1
                      .Subscribe(candleInfo => {
                          
                          double a = candleInfo.Bid;
                          double b = candleInfo.Ask;
                          double c = (a + b) / 2;

                          SetText(c);
                          SetText2(candleInfo.Bid.ToString());
                          SetText3(candleInfo.Ask.ToString());
                          Console.WriteLine(candleInfo.Bid.ToString());
                      });





                // after this line no-more realtime data for min5 print on console
                await api.UnSubscribeRealtimeData(ActivePair.EURUSD, TimeFrame.Min5);


                //when price down with 

            }


        }

        delegate void SetTextCallback(double valor);
        private void SetText(double valor)
        {
            
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label1.InvokeRequired)
            {
                SetTextCallback t = new SetTextCallback(SetText);
                try
                { this.Invoke(t, new object[] { valor }); }

                catch (Exception)
                {

                }
            }
            else
            {
                this.label1.Text = valor.ToString();
            }



            if (this.Grafica.InvokeRequired)
            {
                SetTextCallback f = new SetTextCallback(SetText);
                this.Invoke(f, new object[] { valor });
            }
            else
            {

                this.Grafica.Series[0].Points.AddXY(0, valor);
                
                if (Grafica.Series[0].Points.Count > 300)
                {
                    // Borra desde X = 0.
                    Grafica.Series[0].Points.RemoveAt(0);
            
                }
            }
        }

        delegate void SetTextCallback2(string valor);
        private void SetText2(string valor)
        {

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label2.InvokeRequired)
            {
                SetTextCallback2 t = new SetTextCallback2(SetText2);
                try
                { this.Invoke(t, new object[] { valor }); }

                catch (Exception)
                {

                }
            }
            else
            {
                this.label2.Text = valor;
            }
        }
        private void SetText3(string valor)
        {

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label3.InvokeRequired)
            {
                SetTextCallback2 t = new SetTextCallback2(SetText3);
                try
                { this.Invoke(t, new object[] { valor }); }

                catch (Exception)
                {

                }
            }
            else
            {
                this.label3.Text = valor;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Grafica.ChartAreas[0].AxisX.Maximum = 300;
            Grafica.ChartAreas[0].AxisY.IsStartedFromZero = false;
            Grafica.ChartAreas[0].AxisY.Interval = 0.01;
            Grafica.ChartAreas[0].RecalculateAxesScale();
            RunSample();
        }
        




    }
}
