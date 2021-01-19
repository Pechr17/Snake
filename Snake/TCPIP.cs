using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Snake
{
    /// <summary>
    /// Class used to establish and send message. 
    /// </summary>
    public class SocketClient
    {
        Socket sender;
        IPHostEntry host;
        IPAddress ipAddress;
        IPEndPoint remoteEP;

        /// <summary>
        /// Starts a TCP client with localhosts' IP address. Thus as written right now it
        /// is only possible to transmit messages to a client on the same host.
        /// To be able to transmit a message a client listening on the socket is to be started.
        /// </summary>
        public void StartClient()
        {
            byte[] bytes = new byte[1024]; //a variable used to store the message to be transmitted in

            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  

                // If a host has multiple addresses, you will get a list of addresses  

                host = Dns.GetHostEntry(Dns.GetHostName()); //finds host IP addresses
                ipAddress = host.AddressList[0]; //Choose IP address from list
                remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.    

                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.    
                    byte[] msg = Encoding.ASCII.GetBytes("9");

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

        //sendMsg()
        //shutDown()
        //createclient()
    }
}
    /*
     * The next section must be executed from another program
     */
    /*
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;

    namespace test_tcp
    {
        public class SocketListener
        {
            static void Main(string[] args)
            {
                SocketListener L1 = new SocketListener();
                L1.StartServer();
            }
            Socket handler;
            public void StartServer()
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method  
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.  
                // We will listen 10 requests at a time  
                listener.Listen(10);

                try
                {
                    // Create a Socket that will use Tcp protocol      
                    // Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    // A Socket must be associated with an endpoint using the Bind method  
                    //listener.Bind(localEndPoint);
                    // Specify how many requests a Socket can listen before it gives Server busy response.  
                    // We will listen 10 requests at a time  
                    //listener.Listen(10);

                    Console.WriteLine("Waiting for a connection...");
                    handler = listener.Accept();

                    // Incoming data from the client.    
                    string data = null;
                    byte[] bytes = null;



                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.Length > 0)
                        {
                            break;
                        }

                    }
                    int t1 = Convert.ToInt32(data[0]);
                    Console.WriteLine(data);

                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Console.WriteLine("\n Press any key to continue...");
                Console.ReadKey();
            }
        }

        // ['c1','c2','t']
        /* c1,c2 = coordinate;
        * t = tegn. 'X', '-', ' ', 'B';
        */
//}
