using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Arduino.PubSubService.UnitTests
{
	[TestFixture]
	public class PublishSubscribeServiceFixture
	{
		[Test]
		public void When_PublishingAValidSubscriptionMessage_ThenANewSubscriberIsAddedToSubscribersList()
		{
			PublishSubscribeService service = new PublishSubscribeService();
			service.Publish("sub:mymessage");
			//Assert.IsTrue(service.);
		}
		public void When_PublishingAnInValidSubscriptionMessage_ThenNoNewSubscribersAreAddedToSubscribersList()
		{

		}
		public void When_PublishingAValidSubscriptionMessageTwice_ThenOnlyOneIsAddedToTheSubscribersList()
		{

		}
		public void When_PublishingAValidSubscriptionMessageForTwoDifferentDevices_ThenTwoIsAddedToTheSubscribersList()
		{

		}
		public void When_Unsubscribing_ThenTwoIsAddedToTheSubscribersList()
		{

		}
	}
}
