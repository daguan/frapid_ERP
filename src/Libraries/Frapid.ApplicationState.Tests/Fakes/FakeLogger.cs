using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Frapid.ApplicationState.Tests.Fakes
{
    public sealed class FakeLogger:ILogger
    {
        ILogger ILogger.ForContext(IEnumerable<ILogEventEnricher> enrichers)
        {
            return this;
        }

        ILogger ILogger.ForContext(string propertyName, object value, bool destructureObjects)
        {
            return this;
        }

        ILogger ILogger.ForContext<TSource>()
        {
            return this;
        }

        void ILogger.Write(LogEvent logEvent)
        {
        }

        void ILogger.Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
        {
        }

        bool ILogger.IsEnabled(LogEventLevel level)
        {
            return true;
        }

        void ILogger.Verbose(string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Debug(string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Information(string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Warning(string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Error(string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Fatal(string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
        }

        void ILogger.Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
        }

        ILogger ILogger.ForContext(Type source)
        {
            return this;
        }
    }
}