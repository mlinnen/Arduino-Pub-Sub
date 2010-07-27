using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Arduino.Contract;

namespace Arduino.Contract
{
	public class PublishSubscribeClient
	{
		private TcpListener _tcpListener;
		private Thread _listenThread;
		private Thread _clientThread;

		public string BrokerIp { get; set; }
		public int BrokerPort { get; set; }
		public int MessagePort { get; set; }

		/// <summary>
		/// A collection of message processors
		/// </summary>
		[ImportMany]
		public List<IMessageProcessor> MessageProcessors { get; set; }

		public PublishSubscribeClient()
		{
			BrokerIp = "127.0.0.1";
			BrokerPort = 9999;
			MessagePort = 9998;
		}

		public void Publish(string messageType, string message)
		{
			// Publish the message to the broker
			TcpClient client = new TcpClient(BrokerIp, BrokerPort);

			string msg = string.Format("{0}:{1}", messageType, message);

			// Translate the passed message into ASCII and store it as a Byte array.
			Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

			// Get a client stream for reading and writing.
			//  Stream stream = client.GetStream();

			NetworkStream stream = client.GetStream();

			// Send the message to the connected TcpServer. 
			stream.Write(data, 0, data.Length);

			stream.Flush();

			client.Close();

		}

		public void Subscribe(Subscription subscription)
		{
			// Publish the message to the broker
			TcpClient client = new TcpClient(BrokerIp, BrokerPort);

			// Translate the passed message into ASCII and store it as a Byte array.
			Byte[] data = System.Text.Encoding.ASCII.GetBytes(subscription.SubscribeMessage);

			NetworkStream stream = client.GetStream();

			// Send the message to the connected TcpServer. 
			stream.Write(data, 0, data.Length);

			stream.Flush();

			client.Close();

		}

		public void UnSubscribe(Subscription subscription)
		{
			// Publish the message to the broker
			TcpClient client = new TcpClient(BrokerIp, BrokerPort);

			// Translate the passed message into ASCII and store it as a Byte array.
			Byte[] data = System.Text.Encoding.ASCII.GetBytes(subscription.UnsubSubscribeMessage);

			NetworkStream stream = client.GetStream();

			// Send the message to the connected TcpServer. 
			stream.Write(data, 0, data.Length);

			stream.Flush();

			client.Close();

		}

		public void Connect()
		{
			_tcpListener = new TcpListener(IPAddress.Any, MessagePort);
			_listenThread = new Thread(new ThreadStart(ListenForClients));
			_listenThread.Start();

		}

		private void ListenForClients()
		{
			_tcpListener.Start();
			while (true)
			{
				TcpClient client = _tcpListener.AcceptTcpClient();

				// create a thread to handle incoming messages
				_clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));

				_clientThread.Start(client);
			}
		}

		private void HandleClientComm(object client)
		{
			TcpClient tcpClient = (TcpClient)client;
			NetworkStream clientStream = tcpClient.GetStream();
			byte[] message = new byte[4096];
			int bytesRead;
			while (true)
			{
				try
				{
					// Wait for an entire line to be read
					bytesRead = clientStream.Read(message, 0, 4096);
				}
				catch (Exception ex)
				{
					break;
				}
				if (bytesRead == 0)
					break;
				ASCIIEncoding encoder = new ASCIIEncoding();
				string data = encoder.GetString(message, 0, bytesRead);

				IMessage msg = new Message(data);

				// TODO Add an export that sends the raw data instead of dumping to the console
				System.Console.WriteLine(data);

				// Process the data that came from the Arduino
				if (MessageProcessors != null)
				{
					foreach (IMessageProcessor processor in MessageProcessors)
					{
						if (processor.ShouldProcess(msg))
						{
							processor.Execute(msg);
							break;
						}
					}
				}

			}
			tcpClient.Close();
		}
	}
}
