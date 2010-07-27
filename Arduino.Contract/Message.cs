using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.Contract
{
	public class Message:IMessage
	{
		private string _messageType;
		private string _message;

		/// <summary>
		/// Either a message with no detail or a message with detail.
		/// </summary>
		/// <param name="data"></param>
		public Message(string data)
		{
			if (data.Contains(":"))
			{
				int first = data.IndexOf(":");
				_messageType = data.Substring(0, first);
				_message = data.Substring(first + 1);
			}
			else
			{
				_messageType = data;
				_message = "";
			}
		}

		/// <summary>
		/// Constructor that takes a message type and the message detail
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="messageDetail"></param>
		public Message(string messageType,string messageDetail)
		{
			_messageType = messageType;
			if (messageDetail == null)
				_message = "";
			else
				_message = messageDetail;
		}

		#region IMessage Members

		/// <summary>
		/// The unique identifier describing what type of message this is
		/// </summary>
		public string MessageType
		{
			get { return _messageType; }
		}

		/// <summary>
		/// The body of the message
		/// </summary>
		public string MessageDetail
		{
			get { return _message; }
			set { _message = value; }
		}

		#endregion
	}
}
