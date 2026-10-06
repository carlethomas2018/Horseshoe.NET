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
        protected string TraceListeningGroup { get; }

        protected BaseObj() 
        {
            TraceListeningGroup = GetType().Namespace;
        }

        /* * * * * * * * * * * * * * * * * *
         *    CODE TRACE
         * * * * * * * * * * * * * * * * * */

        /// <inheritdoc cref="CodeTraceEngine.RelayMultiLineMessage(IEnumerable{string}, IndentAction, int?, string)"/>
        protected void RelayMessage(string line, IndentAction indentAction = IndentAction.None, int? overrideIndentLevel = null, string listeningGroup = null) =>
            CodeTraceEngine.RelayMessage(line, indentAction: indentAction, overrideIndentLevel: overrideIndentLevel, listeningGroup: listeningGroup ?? TraceListeningGroup);

        /// <inheritdoc cref="CodeTraceEngine.RelayMultiLineMessage(IEnumerable{string}, IndentAction, int?, string)"/>
        protected void RelayMultiLineMessage(IEnumerable<string> multiLines, IndentAction indentAction = IndentAction.None, int? overrideIndentLevel = null, string listeningGroup = null) =>
            CodeTraceEngine.RelayMultiLineMessage(multiLines, indentAction: indentAction, overrideIndentLevel: overrideIndentLevel, listeningGroup: listeningGroup ?? TraceListeningGroup);

        /// <inheritdoc cref="CodeTraceEngine.RelayException(Exception, bool, bool, bool, string)"/>
        protected void RelayException(Exception exception, bool includeStackTrace = false, bool indentException = false, bool throwException = false, string listeningGroup = null) =>
            CodeTraceEngine.RelayException(exception, includeStackTrace: includeStackTrace, indentException: indentException, throwException: throwException, listeningGroup: listeningGroup ?? TraceListeningGroup);

        /// <inheritdoc cref="CodeTraceEngine.RelayMethodEntered(string, Dictionary{string, object}, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string, object, string)"/>
        protected void RelayMethodEntered
        (
            string methodName = null,
            Dictionary<string, object> paramsAndArgs = null,
            string param01 = null, object arg01 = null,
            string param02 = null, object arg02 = null,
            string param03 = null, object arg03 = null,
            string param04 = null, object arg04 = null,
            string param05 = null, object arg05 = null,
            string param06 = null, object arg06 = null,
            string param07 = null, object arg07 = null,
            string param08 = null, object arg08 = null,
            string param09 = null, object arg09 = null,
            string param10 = null, object arg10 = null,
            string param11 = null, object arg11 = null,
            string param12 = null, object arg12 = null,
            string param13 = null, object arg13 = null,
            string param14 = null, object arg14 = null,
            string param15 = null, object arg15 = null,
            string param16 = null, object arg16 = null,
            string listeningGroup = null
        ) =>
            CodeTraceEngine.RelayMethodEntered
            (
                methodName: methodName ?? new StackTrace().GetFrame(1).GetMethod().ToDisplayString(), 
                paramsAndArgs: paramsAndArgs,
                param01: param01, arg01: arg01,
                param02: param02, arg02: arg02,
                param03: param03, arg03: arg03,
                param04: param04, arg04: arg04,
                param05: param05, arg05: arg05,
                param06: param06, arg06: arg06,
                param07: param07, arg07: arg07,
                param08: param08, arg08: arg08,
                param09: param09, arg09: arg09,
                param10: param10, arg10: arg10,
                param11: param11, arg11: arg11,
                param12: param12, arg12: arg12,
                param13: param13, arg13: arg13,
                param14: param14, arg14: arg14,
                param15: param15, arg15: arg15,
                param16: param16, arg16: arg16,
                listeningGroup: listeningGroup ?? TraceListeningGroup
            );

        /// <inheritdoc cref="CodeTraceEngine.RelayMethodReturning(string, string)"/>
        protected void RelayMethodReturning(string message = null, string listeningGroup = null) =>
            CodeTraceEngine.RelayMethodReturning(message: message, listeningGroup: listeningGroup ?? TraceListeningGroup);

        /// <inheritdoc cref="CodeTraceEngine.RelayMethodReturningValue{T}(string, T, string)"/>
        protected T RelayMethodReturningValue<T>(string message = null, T returnValue = default, string listeningGroup = null) =>
            CodeTraceEngine.RelayMethodReturningValue(message: message, returnValue: returnValue, listeningGroup: listeningGroup ?? TraceListeningGroup);

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

        /// <inheritdoc cref="CollectionUtil.Render{T}(IEnumerable{T})"/>
        public static string Render<T>(IEnumerable<T> collection) =>
            CollectionUtil.Render(collection);

        /// <inheritdoc cref="CollectionUtil.RenderCountAndLastItem{T}(IEnumerable{T})"/>
        public static string RenderCountAndLastItem<T>(IEnumerable<T> collection) =>
            CollectionUtil.RenderCountAndLastItem(collection);

        /// <inheritdoc cref="ListUtil.AsList{T}(IEnumerable{T})"/>
        public static List<T> AsList<T>(IEnumerable<T> collection) =>
            ListUtil.AsList(collection);
    }
}
