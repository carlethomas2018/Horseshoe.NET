using Horseshoe.NET.CodeTrace;
using Horseshoe.NET.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Horseshoe.NET
{
    /// <summary>
    /// A robust base class for both system and client objects that brings a host of functions and capabilities closer within developers' reach.
    /// </summary>
    public abstract class BaseObj
    {
        protected string TraceGroup { get; }

        protected BaseObj() 
        {
            TraceGroup = GetType().Namespace;
        }

        /* * * * * * * * * * * * * * * * * *
         *    CODE TRACE
         * * * * * * * * * * * * * * * * * */

        /// <inheritdoc cref="SystemCodeTracing.RelayMultiLineMessage(IEnumerable{string}, IndentAction, int?, string)"/>
        protected void RelayMessage(string line, IndentAction indentAction = IndentAction.None, int? overrideIndentLevel = null, string traceGroup = null)
        {
            SystemCodeTracing.RelayMessage(line, indentAction: indentAction, overrideIndentLevel: overrideIndentLevel, traceGroup: traceGroup ?? TraceGroup);
        }

        /// <inheritdoc cref="SystemCodeTracing.RelayMultiLineMessage(IEnumerable{string}, IndentAction, int?, string)"/>
        protected void RelayMultiLineMessage(IEnumerable<string> multiLines, IndentAction indentAction = IndentAction.None, int? overrideIndentLevel = null, string traceGroup = null)
        {
            SystemCodeTracing.RelayMultiLineMessage(multiLines, indentAction: indentAction, overrideIndentLevel: overrideIndentLevel, traceGroup: traceGroup ?? TraceGroup);
        }

        /// <inheritdoc cref="SystemCodeTracing.RelayException(Exception, bool, bool, bool, string)"/>
        protected void RelayException(Exception exception, bool includeStackTrace = false, bool indentException = false, bool throwException = false, string traceGroup = null)
        {
            SystemCodeTracing.RelayException(exception, includeStackTrace: includeStackTrace, indentException: indentException, throwException: throwException, traceGroup: traceGroup ?? TraceGroup);
        }

        /// <inheritdoc cref="SystemCodeTracing.RelayMethodEntered(string, Dictionary{string, object}, string)"/>
        protected void RelayMethodEntered(string methodName = null, Dictionary<string, object> paramsAndArgs = null, string traceGroup = null)
        {
            SystemCodeTracing.RelayMethodEntered(methodName: methodName ?? new StackTrace().GetFrame(1).GetMethod().ToDisplayString(), paramsAndArgs: paramsAndArgs, traceGroup: traceGroup ?? TraceGroup);
        }

        /// <inheritdoc cref="SystemCodeTracing.RelayMethodReturning(string, string)"/>
        protected void RelayMethodReturning(string message = null, string traceGroup = null)
        {
            SystemCodeTracing.RelayMethodReturning(message: message, traceGroup: traceGroup ?? TraceGroup);
        }

        /// <inheritdoc cref="SystemCodeTracing.RelayMethodReturningValue{T}(string, T, string)"/>
        protected T RelayMethodReturningValue<T>(string message = null, T returnValue = default, string traceGroup = null)
        {
            return SystemCodeTracing.RelayMethodReturningValue(message: message, returnValue: returnValue, traceGroup: traceGroup ?? TraceGroup);
        }


        /* * * * * * * * * * * * * * * * * *
         *    COLLECTIONS
         * * * * * * * * * * * * * * * * * */
       
        /// <inheritdoc cref="CollectionUtil.HasAny{T}(IEnumerable{T})"/>
        public static bool HasAny<T>(IEnumerable<T> collection) =>
            CollectionUtil.HasAny(collection);

        /// <inheritdoc cref="CollectionUtil.HasAny{T}(IEnumerable{T}, Func{T, bool})"/>
        public static bool HasAny<T>(IEnumerable<T> collection, Func<T, bool> predicate) =>
            CollectionUtil.HasAny(collection, predicate);

        /// <inheritdoc cref="CollectionUtil.IsNullOrEmpty{T}(IEnumerable{T})"/>
        public static bool IsNullOrEmpty<T>(IEnumerable<T> collection) =>
            CollectionUtil.IsNullOrEmpty(collection);

    }
}
