using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Client app is the one sending messages to a Server/listener.   
// Both listener and client can send messages back and forth once a   
// communication is established.  

namespace Client
{
    public class SocketClient
    {
        public static int Main(String[] args)
        {
            StartClient();
            Console.WriteLine("Pressione ENTER para sair...");
            Console.Read();
            return 0;
        }


        public static void StartClient()
        {
            string caminho = "../../../../Prova Estagio DEV - Anexo 1.txt";
            List<Pessoas> lista = new List<Pessoas>();
            if (File.Exists(caminho))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(caminho))
                    {
                        string linha;
                        while ((linha = reader.ReadLine()) != null)
                        {
                            Pessoas pessoa = new Pessoas();
                            if (linha.Contains("1"))
                            {
                                pessoa.Nome = linha;
                                lista.Add(pessoa);
                            }
                            Console.WriteLine(linha);
                        }
                    }
                    ////foreach (Pessoas i in lista)
                    ////{
                    ////    Console.WriteLine(i.Nome);
                    ////}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine($" O arquivo {caminho} não foi localizado");
            }
            string finalList = string.Join("\n", lista.Select(t => t.Nome).ToArray());
            //Console.WriteLine(finalList);

            byte[] bytes = new byte[1024];

            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.    
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket conectado ao {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.    
                    byte[] msg = Encoding.ASCII.GetBytes(finalList);

                    // Send the data through the socket.    
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.    
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
