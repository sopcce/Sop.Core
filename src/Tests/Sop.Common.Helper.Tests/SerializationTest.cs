
using NUnit.Framework;
using Sop.Common.Helper.Tests.Models;
using System;

namespace Sop.Common.Helper.Tests
{
    public class SerializationTest
    {
        public TestInfo Info;
        public string TestStr;


        [SetUp]
        public void Setup()
        {
            Info = new TestInfo()
            {
                IsDel = false,
                Body = "this_is_body",
                DateCreated = DateTime.Now,
                DecimalValue = 12.3265m,
                FloatValue = 32.235f,
                LongValue = 125689487655654655L,
                Status = false,
                Type = TestInfoType.Again
            };

            TestStr = "";
        }

        [Test]
        public void Serialization_Json()
        {
            string jsonStr = Info.ToJson();
            TestInfo newInfo = jsonStr.FromJson<TestInfo>();
            Assert.AreEqual(newInfo.Body, Info.Body);
        }


        [Test]
        public void Serialization_XML()
        {
            string jsonStr = Info.ToXml();
            TestInfo newInfo = jsonStr.FromXml<TestInfo>();
            Assert.AreEqual(newInfo.Body, Info.Body);
        }



    }
}