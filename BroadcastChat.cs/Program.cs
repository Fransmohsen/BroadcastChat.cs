using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace BroadcastChat.cs
{
    class Program
    {
        private const int ListenPort = 11000;
        static void Main(string[] args)
        {
            //skapar en tråd som körs parallelt 0emed huvudprogrammet
            var listenThread = new Thread(Listener);
            listenThread.Start();

            Socket socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            socket.EnableBroadcast = true;

            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, ListenPort);

            while(true)
            {
                //Använderen skriver medelande 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Skriv meddelandet:");
                string msg = Console.ReadLine();

                byte[] sendbuf = Encoding.UTF8.GetBytes(msg);
                socket.SendTo(sendbuf, ep);
                Thread.Sleep(200);
            }


        }
        static void Listener()
        {
            UdpClient listener = new UdpClient(ListenPort);
            try
            {
                while(true)
                {
                    DateTime time = DateTime.Now;
                    string format = "MMM ddd d HH:mm yyyy";
                    //skapa  objekt som lyssnar efter traik från valfri ip-adress men via port
                    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, ListenPort);
                    byte[] bytes = listener.Receive(ref groupEP);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\n Mottaget meddelande från {0} : {1}\n",
                        groupEP.ToString(), Encoding.UTF8.GetString(bytes,0,bytes.Length));
                    Console.WriteLine("Meddelandet mottogs: " + time.ToString(format));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }

        }
    }
}
