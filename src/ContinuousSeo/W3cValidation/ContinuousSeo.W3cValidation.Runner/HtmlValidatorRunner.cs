﻿// -----------------------------------------------------------------------
// <copyright file="HtmlValidatonRunner.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ContinuousSeo.Core.Diagnostics;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Processors;
    using ContinuousSeo.W3cValidation.Runner.DI;
    using StructureMap;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorRunner : IValidatorRunner
    {
        private readonly IHtmlValidatorRunnerContext mContext;
        protected IUrlAggregator mUrlAggregator;
        protected IUrlProcessorFactory mUrlProcessorFactory;

        public HtmlValidatorRunner(IHtmlValidatorRunnerContext context, IUrlAggregator urlAggregator, IUrlProcessorFactory urlProcessorFactory) 
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            if (urlProcessorFactory == null)
                throw new ArgumentNullException("urlProcessorFactory");

            this.mContext = context;
            this.mUrlAggregator = urlAggregator;
            this.mUrlProcessorFactory = urlProcessorFactory;
        }

        protected virtual void Intialize()
        {
            mContext.Announcer.Header("W3C HTML Validator");

            // Keep track of how long this takes
            mContext.TotalTimeStopwatch.Start();
        }

        #region IValidatorRunner Members

        public ValidationResult Execute()
        {
            Intialize();

            var urls = mUrlAggregator.AggregateUrls();
            var processor = mUrlProcessorFactory.GetUrlProcessor(mContext.OutputFormat);
            var result = processor.Process(urls, mContext.OutputPath);

            return result;
        }

        #endregion
    }
}
