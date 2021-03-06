#region Copyright
// -----------------------------------------------------------------------
//
// Copyright (c) 2012, Shad Storhaug <shad@shadstorhaug.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// -----------------------------------------------------------------------
#endregion

namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Xml;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.Core.Extensions;
    using ContinuousSeo.Core.Net;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.W3cValidation.Runner.Parsers;

    [TestFixture]
    public class SitemapsParserTests
    {
        #region SetUp / TearDown

        [SetUp]
        public void Init()
        { }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region ProcessSitemapsFile Method

        private static void WriteSitemapsStream(Stream stream, IEnumerable<SitemapInfo> entries)
        {
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            writer.WriteStartElement("url");

            foreach (SitemapInfo entry in entries)
            {
                writer.WriteStartElement("loc");
                writer.WriteRaw(entry.Location);
                writer.WriteEndElement();

                writer.WriteStartElement("lastmod");
                writer.WriteRaw(entry.LastModification.ToString("yyyy-MM-dd"));
                writer.WriteEndElement();

                writer.WriteStartElement("changefreq");
                writer.WriteRaw(entry.ChangeFrequency.ToString().ToLowerInvariant());
                writer.WriteEndElement();

                writer.WriteStartElement("priority");
                writer.WriteRaw(entry.Priority.GetDescription());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();

            // Reset to beginning of stream
            writer.Flush();
            stream.Position = 0;

            // leave writer open so the stream isn't closed
        }

        private void WriteValidSitemapsStreamWith4Urls(Stream stream)
        {
            List<SitemapInfo> entries = new List<SitemapInfo>();

            entries.Add(new SitemapInfo("http://www.google.com/", DateTime.MinValue, SitemapChangeFrequency.monthly, SitemapPriority.Priority5));
            entries.Add(new SitemapInfo("http://www.google.com/test.aspx", DateTime.MinValue, SitemapChangeFrequency.monthly, SitemapPriority.Priority5));
            entries.Add(new SitemapInfo("http://www.google.com/test1.aspx", DateTime.MinValue, SitemapChangeFrequency.monthly, SitemapPriority.Priority5));
            entries.Add(new SitemapInfo("http://www.google.com/test2.aspx", DateTime.MinValue, SitemapChangeFrequency.monthly, SitemapPriority.Priority5));

            WriteSitemapsStream(stream, entries);
        }

        [Test]
        public void ParseUrlsFromFile_ValidStreamWith4Urls_ShouldReturn4Urls()
        {
            // arrange
            var httpClient = new Mock<IHttpClient>();
            var streamFactory = new Mock<IStreamFactory>();
            SitemapsParser target = new SitemapsParser(httpClient.Object, streamFactory.Object);
            IEnumerable<string> result;

            using (Stream file = new MemoryStream())
            {
                WriteValidSitemapsStreamWith4Urls(file);

                // act
                result = target.ParseUrlsFromFile(file);
            }


            // assert
            var actual = result.Count();
            var expected = 4;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseUrlsFromFile_ValidStreamWith4Urls_ShouldMatch1stUrl()
        {
            // arrange
            var httpClient = new Mock<IHttpClient>();
            var streamFactory = new Mock<IStreamFactory>();
            SitemapsParser target = new SitemapsParser(httpClient.Object, streamFactory.Object);
            IEnumerable<string> result;

            using (Stream file = new MemoryStream())
            {
                WriteValidSitemapsStreamWith4Urls(file);

                // act
                result = target.ParseUrlsFromFile(file);
            }


            // assert
            var actual = result.First();
            var expected = "http://www.google.com/";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseUrlsFromFile_ValidStreamWith4Urls_ShouldMatch2ndUrl()
        {
            // arrange
            var httpClient = new Mock<IHttpClient>();
            var streamFactory = new Mock<IStreamFactory>();
            SitemapsParser target = new SitemapsParser(httpClient.Object, streamFactory.Object);
            IEnumerable<string> result;

            using (Stream file = new MemoryStream())
            {
                WriteValidSitemapsStreamWith4Urls(file);

                // act
                result = target.ParseUrlsFromFile(file);
            }


            // assert
            var actual = result.ElementAt(1);
            var expected = "http://www.google.com/test.aspx";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseUrlsFromFile_ValidUrlWith4Urls_ShouldMatch1stUrl()
        {
            // arrange
            IEnumerable<string> result;
            string sitemapsUrl = "http://www.mydomain.com/sitemaps.xml";
            using (var sitemapsFile = new MemoryStream())
            {
                WriteValidSitemapsStreamWith4Urls(sitemapsFile);

                var httpClient = new Mock<IHttpClient>();
                httpClient.Setup(x => x.GetResponseStream(sitemapsUrl)).Returns(sitemapsFile);
                var streamFactory = new Mock<IStreamFactory>();
                SitemapsParser target = new SitemapsParser(httpClient.Object, streamFactory.Object);
                

                // act
                result = target.ParseUrlsFromFile(sitemapsUrl);
            }

            // assert
            var actual = result.ElementAt(0);
            var expected = "http://www.google.com/";

            Assert.AreEqual(expected, actual);
        }

        public void ParseUrlsFromFile_ValidUrlWith4Urls_ShouldMatch2ndUrl()
        {
            // arrange
            IEnumerable<string> result;
            string sitemapsUrl = "http://www.mydomain.com/sitemaps.xml";
            using (var sitemapsFile = new MemoryStream())
            {
                WriteValidSitemapsStreamWith4Urls(sitemapsFile);

                var httpClient = new Mock<IHttpClient>();
                httpClient.Setup(x => x.GetResponseStream(sitemapsUrl)).Returns(sitemapsFile);
                var streamFactory = new Mock<IStreamFactory>();
                SitemapsParser target = new SitemapsParser(httpClient.Object, streamFactory.Object);

                // act
                result = target.ParseUrlsFromFile(sitemapsUrl);
            }


            // assert
            var actual = result.ElementAt(1);
            var expected = "http://www.google.com/test.aspx";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseUrlsFromFile_ValidFilePathWith4Urls_ShouldMatch1stUrl()
        {
            // arrange
            IEnumerable<string> result;
            string sitemapsPath = @"C:\sitemaps.xml";
            using (var sitemapsFile = new MemoryStream())
            {
                WriteValidSitemapsStreamWith4Urls(sitemapsFile);

                var httpClient = new Mock<IHttpClient>();
                var streamFactory = new Mock<IStreamFactory>();
                streamFactory.Setup(x => x.GetFileStream(sitemapsPath, FileMode.Open, FileAccess.Read)).Returns(sitemapsFile);
                SitemapsParser target = new SitemapsParser(httpClient.Object, streamFactory.Object);

                // act
                result = target.ParseUrlsFromFile(sitemapsPath);
            }

            // assert
            var actual = result.ElementAt(1);
            var expected = "http://www.google.com/test.aspx";

            Assert.AreEqual(expected, actual);
        }


        #endregion


        #region Private Classes
            
        public enum SitemapChangeFrequency
        {
            always,
            hourly,
            daily,
            weekly,
            monthly,
            yearly,
            never
        }

        public enum SitemapPriority
        {
            [System.ComponentModel.Description("0.0")]
            Priority0,
            [System.ComponentModel.Description("0.1")]
            Priority1,
            [System.ComponentModel.Description("0.2")]
            Priority2,
            [System.ComponentModel.Description("0.3")]
            Priority3,
            [System.ComponentModel.Description("0.4")]
            Priority4,
            [System.ComponentModel.Description("0.5")]
            Priority5,
            [System.ComponentModel.Description("0.6")]
            Priority6,
            [System.ComponentModel.Description("0.7")]
            Priority7,
            [System.ComponentModel.Description("0.8")]
            Priority8,
            [System.ComponentModel.Description("0.9")]
            Priority9,
            [System.ComponentModel.Description("1.0")]
            Priority10,
        }

        private class SitemapInfo
        {
            public SitemapInfo(string location, DateTime lastModification, SitemapChangeFrequency changeFrequency, SitemapPriority priority)
            {
                this.Location = location;
                this.LastModification = lastModification;
                this.ChangeFrequency = changeFrequency;
                this.Priority = priority;
            }

            public string Location { get; set; }
            public DateTime LastModification { get; set; }
            public SitemapChangeFrequency ChangeFrequency { get; set; }
            public SitemapPriority Priority { get; set; }
        }

        #endregion

    }
}
