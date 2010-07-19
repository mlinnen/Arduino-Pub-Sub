using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Arduino.PubSubService
{
	public class PublishSubscribeService
	{

		private TcpListener _tcpListener;
		private Thread _listenThread;
		private Thread _clientThread;

		public PublishSubscribeService()
		{
			Subscribers = new List<Subscription>();
		}

		public int Port { get; set; }

		public List<Subscription> Subscribers { get; set; }

		public void Publish(string message)
		{
			// parse the message
			string[] parts = message.Split(':');

			// check to be sure the message is valid
			if (parts.Length < 2)
				return;

			string messageType = parts[0];

			// Is this a subscription message type?
			if (messageType.Equals("sub"))
			{
				Subscribe(message);
			}
			// Is this an unsubscribe message type?
			else if (messageType.Equals("unsub"))
			{
				UnSubscribe(message);
			}
			// Must be just a plain old message so send it off to subscribers
			else
			{
				foreach (Subscription subscriber in Subscribers)
				{
					if (subscriber.MessageType.Equals(messageType))
					{
						SendMessage(subscriber, message);
					}
				}

			}


		}

		public void Connect()
		{
			_tcpListener = new TcpListener(IPAddress.Any, Port);
			_listenThread = new Thread(new ThreadStart(ListenForClients));
			_listenThread.Start();

		}

		private void Subscribe(string message)
		{
			string[] parts = message.Split(':');

			// Validate the subscription message
			if (parts.Length == 4)
			{
				// Validate the IP address
				string[] ipParts = parts[2].Split('.');
				if (ipParts.Length != 4)
				{
					return;
				}

				// Validate the message that we are subscribing to

				Subscription subscription = new Subscription(parts[1], parts[2],parts[3]);
				Subscribe(subscription);
			}
		}

		private void Subscribe(Subscription subscribe)
		{
			// TODO Do some locking
			bool found = false;
			foreach (Subscription subscriber in Subscribers)
			{
				if (subscriber.MessageType.Equals(subscribe.MessageType) && subscriber.Ip.Equals(subscribe.Ip))
				{
					found = true;
					break;
				}
			}
			if (!found)
				Subscribers.Add(subscribe);
		}

		private void UnSubscribe(string message)
		{
			string[] parts = message.Split(':');

			// Validate the subscription message
			if (parts.Length == 2)
			{
				// Validate the IP address
				string[] ipParts = parts[2].Split('.');
				if (ipParts.Length != 4)
				{
					return;
				}

				// Validate the message that we are unsubscribing from

				Subscription subscription = new Subscription(parts[1], parts[2],parts[3]);
				UnSubscribe(subscription);
			}
		}

		private void UnSubscribe(Subscription subscribe)
		{
			// TODO Do some locking
			foreach (Subscription subscriber in Subscribers)
			{
				if (subscriber.MessageType.Equals(subscribe.MessageType) && subscriber.Ip.Equals(subscribe.Ip))
				{
					Subscribers.Remove(subscriber);
					break;
				}
			}
		}

		private void SendMessage(Subscription subscription, string message)
		{
			TcpClient client = new TcpClient(subscription.Ip, subscription.Port);

			// Translate the passed message into ASCII and store it as a Byte array.
			Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

			// Get a client stream for reading and writing.
			//  Stream stream = client.GetStream();

			NetworkStream stream = client.GetStream();

			// Send the message to the connected TcpServer. 
			stream.Write(data, 0, data.Length);

			stream.Flush();

			stream.Close();

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
			StreamReader reader = new StreamReader(clientStream);

			// TODO convert this to be more efficient so that it isn't consuming cpu time 
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
				if (data == null)
					continue;

				Publish(data);
				Thread.Sleep(1000);
			}
			tcpClient.Close();
		}

	}
}
