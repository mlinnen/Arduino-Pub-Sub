using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Arduino.Contract.UnitTests
{
	[TestFixture]
	public class MessageFixture
	{
		[Test]
		public void When_MessageOnlyHasMessageType_ThenMessageDetailIsEmpty()
		{
			Message msg = new Message("HB");
			Assert.AreEqual("HB", msg.MessageType, "Incorrect MessageType");
			Assert.AreEqual("", msg.MessageDetail, "Incorrect MessageDetail");
		}
		[Test]
		public void When_CreatingAMessageWith2Parts_ThenMessageIsValid()
		{
			Message msg = new Message("HB:10");
			Assert.AreEqual("HB",msg.MessageType,"Incorrect MessageType");
			Assert.AreEqual("10", msg.MessageDetail, "Incorrect MessageDetail");
		}
		[Test]
		public void When_CreatingAMessageWith3Parts_ThenMessageIsValid()
		{
			Message msg = new Message("HB:10:BAR");
			Assert.AreEqual("HB", msg.MessageType, "Incorrect MessageType");
			Assert.AreEqual("10:BAR", msg.MessageDetail, "Incorrect MessageDetail");
		}
		[Test]
		public void When_MessageTypeAndDetailIsPassedInConstructor_ThenMessageIsValid()
		{
			Message msg = new Message("HB","10");
			Assert.AreEqual("HB", msg.MessageType, "Incorrect MessageType");
			Assert.AreEqual("10", msg.MessageDetail, "Incorrect MessageDetail");
		}
		[Test]
		public void When_MessageTypeAndDetailIsPassedInConstructorAndDetailIsNull_ThenMessageIsValid()
		{
			Message msg = new Message("HB", null);
			Assert.AreEqual("HB", msg.MessageType, "Incorrect MessageType");
			Assert.AreEqual("", msg.MessageDetail, "Incorrect MessageDetail");
		}
	}
}
