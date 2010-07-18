using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Arduino.PubSubService
{
  public class SubscriptionProxyService
  {
    public void Subscribe(string messageType, string returnIp)
    {
      Int32 port = 9998;
      string ip = "127.0.0.1";
      TcpClient client = new TcpClient(ip, port);

      string message = string.Format("{0} {1}", messageType, returnIp);

      // Translate the passed message into ASCII and store it as a Byte array.
      Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

      // Get a client stream for reading and writing.
      //  Stream stream = client.GetStream();

      NetworkStream stream = client.GetStream();

      // Send the message to the connected TcpServer. 
      stream.Write(data, 0, data.Length);


    }
  }
}
