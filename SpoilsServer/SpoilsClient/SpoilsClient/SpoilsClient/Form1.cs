using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SpoilsClient
{
    public partial class ClientForm : Form
    {
        public class Card
        {
            public string name;

            public Card(string _name)
            {
                this.name = _name;
            }
        }


        public ClientForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(ipPoint);

               


                string message = txtText.Text;
                Card card = new Card("bobr");


                byte[] data = Encoding.Default.GetBytes(JsonConvert.SerializeObject(card));
                socket.Send(data);

              
                data = new byte[256]; 
                StringBuilder builder = new StringBuilder();
                int bytes = 0; 

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Default.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                richTextBox1.Text = builder.ToString();

               
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
