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
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                
                host = Dns.GetHostEntry(Dns.GetHostName()); //finds host IP addresses
                ipAddress = host.AddressList[0]; //Choose IP address from list
                remoteEP = new IPEndPoint(ipAddress, 11000); 

                // Create a TCP/IP  socket.    
                
                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                /* */
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
    }
}
