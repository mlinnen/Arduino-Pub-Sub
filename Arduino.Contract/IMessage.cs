using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.Contract
{
	public interface IMessage
	{
		string MessageType { get;}
		string MessageDetail { get; set; }
	}
}
