using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Arduino.Contract;

namespace Arduino.Processors
{
	/// <summary>
	/// The DoorBellProcesor parses Doorbell messages and publishes an event when a valid message is executed.
	/// Message Format: DB:DoorbellNumber
	///   Where DoorbellNumber is an integer 
	/// </summary>
	[Export(typeof(IMessageProcessor))]
	public class DoorBellProcessor:IMessageProcessor
	{
		private IMessage _message;

		#region IMessageProcessor Members

		public string MessageType
		{
			get { return "DB"; }
		}

		public void Execute(IMessage msg)
		{
			if (msg != null && !string.IsNullOrEmpty(msg.MessageDetail) && msg.MessageType.Equals(MessageType))
			{
				int door = 0;
				string[] parms = msg.MessageDetail.Split(':');
				if (parms.Length == 1)
				{ 
					if (int.TryParse(parms[0], out door))
						Send(door);
				}
			}
		}

		public bool ShouldProcess(IMessage msg)
		{

			if (msg == null || string.IsNullOrEmpty(msg.MessageDetail) || string.IsNullOrEmpty(msg.MessageType) || !msg.MessageType.Equals(MessageType))
			{
				return false;
			}
			return true;
		}

		#endregion
		public void Send(int door)
		{
			MessageReceived(door);
		}

		[Import("DoorBellProcessor")]
		public Action<int> MessageReceived { get; set; }


	}
}
