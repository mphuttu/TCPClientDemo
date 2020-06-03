using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPClientDemo
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string TimeStamp { get; set; }
        
    }
    class Program
    {
        public static string PrintTimestamp(DateTime now)
        {
            return now.ToString("yyyy-MM-dd HH:mm:ss ffff");
        }
        public static void StartClient()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000); // jos ei pelitä, ipAddress = "127.0.0.1", localhost

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    //byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");
                    byte[] msg = Encoding.Default.GetBytes("Tämä on testi<EOF>");
                    byte[] sendBuf2 = Encoding.Default.GetBytes("\n\n\nSua katson vaan\n\n" +
                                        "Sua katson vaan, sua katson vaan,\n" +
                                        "sua katselen silmät veessä.\n" +
                                        "Tää onneni on niin outo ja uus,\n" +
                                        "sen että mä vapisen eessä.\n" +
                                        "Kun sydän on auki, on kiinni suu,\n" +
                                        "mun syömeni hehkuu ja halaa.\n" +
                                        "Sua katson ja säästän ja silitän vaan\n" +
                                        "kuin kerjuri leivänpalaa.\n" +
                                        "Minä joka en nauttinut onnestain\n" +
                                        "kuin sieltä ja täältä murun!\n" +
                                        "Tää pöytä mulleko katettu ois ?\n" +
                                        "Ois tullutko loppu surun ?\n" +
                                        "\n - Eino Leino\n\n\n<EOF>");

                    Person dummy = new Person();
                    dummy.FirstName = "Teemu";
                    dummy.LastName = "Pukki";
                    dummy.TimeStamp = PrintTimestamp(DateTime.Now);
                    Person dummy2 = new Person();
                    dummy2.FirstName = "Joel";
                    dummy2.LastName = "Pohjanpalo";
                    dummy2.TimeStamp = PrintTimestamp(DateTime.Now);
                    Person dummy3 = new Person();
                    dummy3.FirstName = "Jarkko (Wiz)";
                    dummy3.LastName = "Oikarinen";
                    dummy3.TimeStamp = PrintTimestamp(DateTime.Now);
                    List<Person> jsonList = new List<Person>();
                    jsonList.Add(dummy);
                    jsonList.Add(dummy2);
                    jsonList.Add(dummy3);
                    string jsonmessage = JsonConvert.SerializeObject(jsonList);
                    byte[] msgJson = Encoding.Default.GetBytes(jsonmessage + "<EOF>");

                    //string jsonmessge = { "etunimi": "Joulu", "sukunimi": "Pukki"};

                    // Send the data through the socket.  
                    //int bytesSent = sender.Send(msg);
                    //int bytesSent = sender.Send(sendBuf2);

                    int bytesSent = sender.Send(msgJson);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Kaiutettu testi = {0}",
                        Encoding.Default.GetString(bytes, 0, bytesRec));
                    //Console.WriteLine("Echoed test = {0}",
                    // Encoding.ASCII.GetString(bytes, 0, bytesRec));
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
        } // end of StartClient
        static int  Main(string[] args)
        {
            StartClient();
            return 0;
        } // end of Main
    } // end of Progrman
} // end namespace
