using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace Arduino.PubSubService
{
	public class PublishService
	{
		private TcpListener _tcpListener;
		private Thread _listenThread;
		private Thread _clientThread;

		public PublishService()
		{
			Subscribers = new List<Subscription>();
		}

		[Import("Subscriptions")]
		public List<Subscription> Subscribers { get; set; }

		public void Publish(string message)
		{
			// Validate the message
			if (string.IsNullOrEmpty(message))
				return;
			
			// Parse the message
			string[] parts = message.Split(':');
			string messageType = parts[0];

			// Is this a subscription?
			if (messageType.Equals("sub"))
			{
				
			}
			else
			{
				foreach (Subscription subscriber in Subscribers)
				{
					if (subscriber.Equals(messageType))
					{
						SendMessage(subscriber, message);
					}
				}
			}
		}

		public void Connect()
		{
			// We simply listen on port 9999
			_tcpListener = new TcpListener(IPAddress.Any, 9999);
			_listenThread = new Thread(new ThreadStart(ListenForClients));
			_listenThread.Start();

		}

		private void ListenForClients()
		{
			_tcpListener.Start();
			while (true)
			{
				TcpClient client = _tcpListener.AcceptTcpClient();

				// create a thread to handle incomming messages
				_clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));

				_clientThread.Start(client);
			}
		}

		private void HandleClientComm(object client)
		{
			TcpClient tcpClient = (TcpClient)client;
			NetworkStream clientStream = tcpClient.GetStream();
			StreamReader reader = new StreamReader(clientStream);
			while (true)
			{
				string data = "";
				try
				{
					// Wait for an entire line to be read
					data = reader.ReadLine();
				}
				catch (Exception ex)
				{
					break;
				}
				if (data==null)
					continue;
				
				Publish(data);
			}
			tcpClient.Close();
		}

		private void SendMessage(Subscription subscription, string message)
		{
			Int32 port = 9999;
			TcpClient client = new TcpClient(subscription.Ip, port);

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
