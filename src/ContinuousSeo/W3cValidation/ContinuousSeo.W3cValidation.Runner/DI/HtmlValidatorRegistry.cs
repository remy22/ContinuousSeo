﻿// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorRegistry.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.DI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using StructureMap.Configuration.DSL;
    using ContinuousSeo.Core;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.Core.Net;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.Processors;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Initialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorRegistry : Registry
    {
        public HtmlValidatorRegistry()
        {

            //this.Scan(s =>
            //{
            //    s.LookForRegistries();
            //    s.AssemblyContainingType<Mainland.Commerce.Controllers.HomeController>();
            //    s.AssemblyContainingType<Mainland.Commerce.Library.ApplicationContext>();
            //});

            //// x.For<IExample>().Use<Example>();
            //this.Scan(scan =>
            //{
            //    scan.TheCallingAssembly();
            //    scan.WithDefaultConventions();
            //});

            //this.For<IItemListFactory>().Use(x => x.GetInstance<ItemListFactory>());

            this.Scan(x =>
                {
                    x.LookForRegistries();
                    x.AssemblyContainingType<ContinuousSeo.Core.DefaultGuidProvider>();
                    x.AssemblyContainingType<ContinuousSeo.W3cValidation.Core.ResourceCopier>();
                });


            //container.Configure(r => r.For<IUrlProcessor>().Use<HtmlValidatorUrlProcessor>());

            this.For<IValidatorRunner>().Use(x => x.GetInstance<HtmlValidatorRunner>());

            // HtmlValidationRunner
            this.For<HtmlValidatorRunnerContext>().Use(x => x.GetInstance<HtmlValidatorRunnerContext>());
            this.For<IUrlProcessor>().Use(x => x.GetInstance<HtmlValidatorUrlProcessor>());

            // HtmlValidationUrlProcessor
            this.For<IValidatorWrapper>().Use(x => x.GetInstance<HtmlValidatorWrapper>());
            //this.For<HtmlValidatorRunnerContext>().Use(x => x.GetInstance<HtmlValidatorRunnerContext>()); // TODO: make singleton scope
            this.For<IUrlAggregator>().Use(x => x.GetInstance<UrlAggregator>());
            this.For<IFileNameGenerator>().Use(x => x.GetInstance<FileNameGenerator>());
            this.For<ResourceCopier>().Use(x => x.GetInstance<HtmlValidatorResourceCopier>()); // From validation.core
            this.For<IValidatorReportWriterFactory>().Use(x => x.GetInstance<ValidatorReportWriterFactory>());
            this.For<IStreamFactory>().Use(x => x.GetInstance<StreamFactory>()); // from core
            this.For<IXslTransformer>().Use(x => x.GetInstance<XslTransformer>());

            // HtmlValidationWrapper
            //this.For<HtmlValidator>().Use(x => x.GetInstance<HtmlValidator>()); // from validation.core
            this.For<HtmlValidator>().Use(x => new HtmlValidator());

            // UrlAggregator
            this.For<ISitemapsParser>().Use(x => x.GetInstance<SitemapsParser>());
            this.For<IUrlFileParser>().Use(x => x.GetInstance<UrlFileParser>());

            // FileNameGenerator
            //this.For<GuidProvider>().Use(x => x.GetInstance<GuidProvider>());
            this.For<GuidProvider>().Use(x => GuidProvider.Current);

            // ValidatorReportWriterFactory
            this.For<IValidatorSoap12ResponseParser>().Use(x => x.GetInstance<HtmlValidatorSoap12ResponseParser>());

            // Sitemaps Parser
            this.For<IHttpClient>().Use(x => x.GetInstance<HttpClient>());



            //this.For<IValidatorRunner>().Use<HtmlValidatorRunner>();

            //// HtmlValidationRunner
            //this.For<HtmlValidatorRunnerContext>().Use<HtmlValidatorRunnerContext>();
            //this.For<IUrlProcessor>().Use<HtmlValidatorUrlProcessor>();

            //// HtmlValidationUrlProcessor
            //this.For<IValidatorWrapper>().Use<HtmlValidatorWrapper>();
            ////this.For<HtmlValidatorRunnerContext>().Use(x => x.GetInstance<HtmlValidatorRunnerContext>()); // TODO: make singleton scope
            //this.For<IUrlAggregator>().Use<UrlAggregator>();
            //this.For<IFileNameGenerator>().Use<FileNameGenerator>();
            //this.For<ResourceCopier>().Use<HtmlValidatorResourceCopier>(); // From validation.core
            //this.For<IValidatorReportWriterFactory>().Use<ValidatorReportWriterFactory>();
            //this.For<IStreamFactory>().Use<StreamFactory>(); // from core

            //// HtmlValidationWrapper
            //this.For<HtmlValidator>().Use<HtmlValidator>(); // from validation.core

            //// UrlAggregator
            //this.For<ISitemapsParser>().Use<mSitemapsParser>();
            //this.For<IUrlFileParser>().Use<mUrlFileParser>();

            //// FileNameGenerator
            //this.For<GuidProvider>().Use<GuidProvider>();

            //// ValidatorReportWriterFactory
            //this.For<IValidatorSoap12ResponseParser>().Use<HtmlValidatorSoap12ResponseParser>();

            //// Sitemaps Parser
            //this.For<IHttpClient>().Use<HttpClient>();


        }
    }
}