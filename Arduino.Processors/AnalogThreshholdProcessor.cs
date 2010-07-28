using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.Contract;
using System.ComponentModel.Composition;

namespace Arduino.Processors
{

	/// <summary>
	/// The AnalogThreshholdProcessor parses the Analog Threshold messages and publishes an event when a valid message is executed.
	/// This message from the arduino is sent when a specific analog input reading has exceeded a threshold
	/// Message Format: AT:deviceNumber:sensorNumber:Threshold:ActualValue
	///   Where 
	///		deviceNumber - is an integer that identifies which sensor the reading came from.  Use this to have thresholds that come from multiple arduinos on the network.
	///		sensorNumber - is the pin that the threshold message belongs to as an integer.  Use this to differentiate between multiple threshold messages that originate from the same device
	///		Threshold - is the actual threshold value that was exceeded as an integer
	///		ActualValue - is the actual value that was read on the pin as an integer
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
			int deviceNumber = 0;
			int sensorNumber = 0;
			int threshhold=0;
			int actualValue=0;
			string[] parms = msg.MessageDetail.Split(':');
			if (parms.Length == 4)
			{
				if (int.TryParse(parms[0], out deviceNumber) &&
					int.TryParse(parms[1], out sensorNumber) &&
					int.TryParse(parms[2], out threshhold) &&
					int.TryParse(parms[3], out actualValue))
				{
					Send(deviceNumber,sensorNumber, threshhold, actualValue);
				}
			}
		}

		#endregion

		[Import("AnalogThreshholdProcessor")]
		public Action<int, int, int, int> MessageReceived { get; set; }

		private void Send(int deviceNumber, int sensorNumber, int threshhold, int actualValue)
		{
			MessageReceived(deviceNumber, sensorNumber, threshhold, actualValue);
		}

	}
}
