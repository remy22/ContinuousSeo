namespace ContinuousSeo.W3cValidation.Runner.UnitTests.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Processors;

    [TestFixture]
    public class HtmlValidatorWrapperTests
    {
        private Mock<HtmlValidator> mValidator = null;
        private Mock<HtmlValidatorRunnerContext> mContext = null;
        private int mTotalValidatorValidateCalls = 0;

        #region SetUp / TearDown

        

        [SetUp]
        public void Init()
        {
            mValidator = new Mock<HtmlValidator>();
            mContext = new Mock<HtmlValidatorRunnerContext>();

            mValidator
                .Setup(x => x.IsDefaultValidatorAddress(It.IsAny<string>()))
                .Returns(false);

            mContext
                .Setup(x => x.ValidatorUrl)
                .Returns("http://www.whereever.com/validator");
        }

        [TearDown]
        public void Dispose()
        {
            mValidator = null;
            mContext = null;
            mTotalValidatorValidateCalls = 0;
        }

        private HtmlValidatorWrapper NewHtmlValidatorWrapperInstance()
        {
            return new HtmlValidatorWrapper(mValidator.Object, mContext.Object);
        }

        private void SetupValidValidatorValidateReturnStatusWith8Warnings()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()))
                .Returns(new HtmlValidatorResult("Valid", 0, 8, 1));

            //var result = new Mock<IValidatorReportItem>().SetupAllProperties().Object;

            //result.IsValid = true;
            //result.Errors = 0;
            //result.Warnings = 8;

            //mValidator
            //    .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
            //    .Returns(result);

        }

        private void SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()))
                .Returns(new HtmlValidatorResult("Invalid", 4, 11, 1));

            //var result = new Mock<IValidatorReportItem>().SetupAllProperties().Object;

            //result.IsValid = false;
            //result.Errors = 4;
            //result.Warnings = 11;

            //mValidator
            //    .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
            //    .Returns(result);
        }

        private void SetupValidatorValidateToThrowHttpException()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()))
                .Throws<System.Web.HttpException>();


            //mValidator
            //    .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
            //    .Throws<System.Web.HttpException>();
        }



        

        private void SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()))
                .Callback(() => mTotalValidatorValidateCalls++);
            mValidator
                .Setup(x => x.Validate(It.IsAny<System.IO.Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()))
                .Callback(() => mTotalValidatorValidateCalls++);

            //mValidator
            //    .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
            //    .Callback(() => mTotalValidatorValidateCalls++);
        }

        #endregion

        #region ValidateUrl Method

        [Test]
        public void ValidateUrl_ValidUrl_ReturnsDomainName()
        {
            // arrange
            var url = "http://www.google.com/";
            SetupValidValidatorValidateReturnStatusWith8Warnings();


            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }


            // assert
            var actual = result.DomainName;
            var expected = "www.google.com";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidUrlWithNonStandardPort_ReturnsDomainNameAndPort()
        {
            // arrange
            var url = "http://www.google.com:9090/test.aspx";
            SetupValidValidatorValidateReturnStatusWith8Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.DomainName;
            var expected = "www.google.com:9090";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_InvalidUrl_ReturnsValidFalse()
        {
            // arrange
            var url = "http:/www.google.com/test.aspx";

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.IsValid;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_InvalidUrl_ReturnsInvalidUrlExceptionMessage()
        {
            // arrange
            var url = "http:/www.google.com/test.aspx";

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.ErrorMessage;
            var expected = "The url is not in a valid format.";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_InvalidUrl_ReturnsUrl()
        {
            // arrange
            var url = "http:/www.google.com/test.aspx";

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Url;
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidUrl_ReturnsUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidValidatorValidateReturnStatusWith8Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Url;
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_HttpExceptionThrown_ReturnsUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToThrowHttpException();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Url;
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ContextOutputFormatHtml_CallsValidatorValidateWithHtmlFormat()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            mContext.Setup(x => x.OutputFormat).Returns("html");
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = OutputFormat.Html;
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ContextOutputFormatXml_CallsValidatorValidateWithSoap12Format()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            mContext.Setup(x => x.OutputFormat).Returns("xml");
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = OutputFormat.Soap12;
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ContextOutputFormatNull_CallsValidatorValidateWithHtmlFormat()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            // By default the mContext.Object.OutputFormat return value is null

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = OutputFormat.Html;
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ContextValidatorUrlProvided_CallsValidatorValidateWithSameValidatorUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = "http://www.whereever.com/validator";
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, expectedValue),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, expectedValue),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        //[Test]
        //public void ValidateUrl_OutputPathProviderReturnedValue_CallsValidatorValidateWithSameOutputPath()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";
        //    var path = @"C:\TestDir\TestFile.html";

        //    mOutputPathProvider.Setup(x => x.GetOutputPath(url)).Returns(path);

        //    var target = NewHtmlValidatorWrapperInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ValidateUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var expectedValue = path;
        //    mValidator
        //        .Verify(x => x.Validate(expectedValue, It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()), 
        //        Times.AtMostOnce());
        //}

        [Test]
        public void ValidateUrl_ValidUrl_CallsValidatorValidateWithSameUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = url;
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), expectedValue, InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), expectedValue, InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidatorValidateReturnsValidStatus_ReportsValidStatus()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupValidValidatorValidateReturnStatusWith8Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.IsValid;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidatorValidateReturnsInvalidStatus_ReportsInvalidStatus()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.IsValid;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidatorValidateReturns8Warnings_Reports8Warnings()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupValidValidatorValidateReturnStatusWith8Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Warnings;
            var expected = 8;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidatorValidateReturns4Errors_Reports4Errors()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Errors;
            var expected = 4;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_HttpExceptionThrown_ReportsInvalidStatus()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupValidatorValidateToThrowHttpException();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.IsValid;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
