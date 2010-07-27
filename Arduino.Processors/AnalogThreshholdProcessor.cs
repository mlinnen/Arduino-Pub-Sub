using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.Contract;
using System.ComponentModel.Composition;

namespace Arduino.Processors
{

	/// <summary>
	/// This message from the arduino is sent when a specific analog input reading has 
	/// exceeded a threshold
	/// </summary>
	[Export(typeof(IMessageProcessor))]
	public class AnalogThreshholdProcessor : IMessageProcessor
	{
		private IMessage _message;

		public AnalogThreshholdProcessor()
		{
		}

		#region IMessageProcessor Members

		public string MessageType
		{
			get { return "AT"; }
		}

		/// <summary>
		/// Tests to be sure that the msg is truly an Analog Threshold msg 
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool ShouldProcess(IMessage msg)
		{
			if (msg==null || string.IsNullOrEmpty(msg.MessageDetail) || string.IsNullOrEmpty(msg.MessageType) || !msg.MessageType.Equals(MessageType))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Parses the msg and raises an event notifying subscribers that the message was 
		/// received from the device
		/// </summary>
		/// <param name="msg">the data from the device to act uppon</param>
		public void Execute(IMessage msg)
		{
			int pin;
			int threshhold;
			int actualValue;
			string[] parms = msg.MessageDetail.Split(':');
			if (parms.Length > 2)
			{
				int.TryParse(parms[0], out pin);
				int.TryParse(parms[1], out threshhold);
				int.TryParse(parms[2], out actualValue);
				Send(pin, threshhold, actualValue);
			}
		}

		#endregion

		[Import("AnalogThreshholdProcessor")]
		public Action<int, int, int> MessageReceived { get; set; }

		private void Send(int pin, int threshhold, int actualValue)
		{
			MessageReceived(pin, threshhold, actualValue);
		}

	}
}
