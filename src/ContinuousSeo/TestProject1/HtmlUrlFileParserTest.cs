﻿using ContinuousSeo.W3cValidation.Runner.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for HtmlUrlFileParserTest and is intended
    ///to contain all HtmlUrlFileParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HtmlUrlFileParserTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ParseLine
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ContinuousSeo.W3cValidation.Runner.dll")]
        public void ParseLineTest()
        {
            HtmlUrlFileParser_Accessor target = new HtmlUrlFileParser_Accessor(); // TODO: Initialize to an appropriate value
            string line = string.Empty; // TODO: Initialize to an appropriate value
            IUrlFileLineInfo expected = null; // TODO: Initialize to an appropriate value
            IUrlFileLineInfo actual;
            actual = target.ParseLine(line);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
