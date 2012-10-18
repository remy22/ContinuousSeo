﻿// -----------------------------------------------------------------------
// <copyright file="IValidationReport.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IValidatorReportItem : IValidatorReportTimes
    {
        string DomainName { get; set; }
        string Url { get; set; }
        string FileName { get; set; }
        bool IsValid { get; set; }
        int Errors { get; set; }
        int Warnings { get; set; }
        string ErrorMessage { get; set; }
    }
}
