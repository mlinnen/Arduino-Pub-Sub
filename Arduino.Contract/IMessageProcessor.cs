using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.Contract
{
	/// <summary>
	/// The interface all message processors have to implement
	/// </summary>
	public interface IMessageProcessor
	{
		/// <summary>
		/// The type of message this processor is for
		/// </summary>
		string MessageType { get; }

		/// <summary>
		/// This is called once it is determined that the message processor is a match for the data that 
		/// came from the device
		/// </summary>
		/// <param name="msg"></param>
		void Execute(IMessage msg);

		/// <summary>
		/// A simple test to see if the msg is for this message processor
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>true if this data should be processed by this message processor</returns>
		bool ShouldProcess(IMessage msg);
	}
}
