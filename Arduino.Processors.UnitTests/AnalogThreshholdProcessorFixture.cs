using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using Arduino.Contract;
using NUnit.Framework;

namespace Arduino.Processors.UnitTests
{
	[TestFixture]
	public class AnalogThreshholdProcessorFixture
	{
		CompositionContainer _container;
		private int _count = 0;

		[TestFixtureSetUp]
		public void Setup()
		{
			Compose();
		}
		[TestFixtureTearDown]
		public void TearDown()
		{
			
		}

		[Test]
		public void When_ValidAnalogThresholdMessageArrives_ThenMessagedetailsArePublished()
		{
			_count = 0;
			AnalogThreshholdProcessor processor = new AnalogThreshholdProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType,"1:10:12");
			processor.Execute(message);
			Assert.AreEqual(1,_count,"The message did not fire");

		}

		[Test]
		public void When_InValidAnalogThresholdMessageArrives_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			AnalogThreshholdProcessor processor = new AnalogThreshholdProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message("BLAH", "10");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidAnalogThresholdArrivesWitNoParams_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			AnalogThreshholdProcessor processor = new AnalogThreshholdProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidAnalogThresholdArrivesWithMissingParms_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			AnalogThreshholdProcessor processor = new AnalogThreshholdProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:2");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidAnalogThresholdArrivesWithPinParmCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			AnalogThreshholdProcessor processor = new AnalogThreshholdProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "A:2:3");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidAnalogThresholdArrivesWithThresholdParmCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			AnalogThreshholdProcessor processor = new AnalogThreshholdProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:A:3");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidAnalogThresholdArrivesWithActualValueParmCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			AnalogThreshholdProcessor processor = new AnalogThreshholdProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:2:A");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		private void Compose()
		{
			var catalog = new AggregateCatalog(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()), new DirectoryCatalog(@".\"));

			_container = new CompositionContainer(catalog);

			_container.ComposeParts(this);
		}

		[Export("AnalogThreshholdProcessor")]
		public void AnalogThreshholdReceived(int pin, int threshhold, int actualValue)
		{
			System.Console.WriteLine("AnalogThreshhold msg received pin {0} threshold {1} actualValue {2} ", pin, threshhold, actualValue);
			_count++;
		}

	}
}
