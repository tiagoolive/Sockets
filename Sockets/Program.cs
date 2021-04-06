using System;
using System.IO;
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
  
// Socket Listener acts as a server and listens to the incoming   
// messages on the specified port and protocol.

namespace Sockets
{
    public class SocketListener  
    {  
        public static int Main(String[] args)  
        {  
            StartServer();  
            return 0;  
        }  
  
     
        public static void StartServer()  
        {  
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses  
            IPHostEntry host = Dns.GetHostEntry("localhost");  
            IPAddress ipAddress = host.AddressList[0];  
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            string caminho = "../../../../Nova lista.txt";



            try {   
  
                // Create a Socket that will use Tcp protocol      
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);  
                // A Socket must be associated with an endpoint using the Bind method  
                listener.Bind(localEndPoint);  
                // Specify how many requests a Socket can listen before it gives Server busy response.  
                // We will listen 10 requests at a time  
                listener.Listen(10);  
  
                Console.WriteLine("Esperando por uma conexão...");  
                Socket handler = listener.Accept();  
  
                 // Incoming data from the client.    
                 string data = null;  
                 byte[] bytes = null;  

                 bytes = new byte[1024];  
                 int bytesRec = handler.Receive(bytes);  
                 data += Encoding.ASCII.GetString(bytes, 0, bytesRec);  

                using (StreamWriter escritor = new StreamWriter(caminho))
                {
                    escritor.Write(data);
                }
                
  
                Console.WriteLine("Conteudo recebido : {0}", data);  
  
                byte[] msg = Encoding.ASCII.GetBytes("Lista criada com sucesso");  
                handler.Send(msg);  
                handler.Shutdown(SocketShutdown.Both);  
                handler.Close();  
            }  
            catch (Exception e)  
            {  
                Console.WriteLine(e.ToString());  
            }  
  
            Console.WriteLine("\n Precione ENTER para sair...");  
            Console.ReadKey();  
        }          
    }
}
  