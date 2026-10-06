using System;
using System.Collections.Generic;

using Horseshoe.NET.CodeTrace;
using Horseshoe.NET.Collections;

namespace Horseshoe.NET
{
    public static class SystemSettings
    {
        public static class CodeTrace
        {
            /// <summary>
            /// The default maximum length of a string representation of a collection (e.g., array, list, dictionary) when logged by CodeTrace.
            /// </summary>
            public const int DefaultRenderedArgMaxLength = 100;

            private static int? _renderedArgMaxLength;

            /// <summary>
            /// The maximum length of a string representation of a collection (e.g., array, list, dictionary) when logged by CodeTrace.
            /// </summary>
            public static int RenderedArgMaxLength 
            { 
                get => _renderedArgMaxLength ?? DefaultRenderedArgMaxLength; 
                set => _renderedArgMaxLength = value; 
            }

            /// <summary>
            /// Collection of relay group names (i.e. C# namespaces) whose code trace messages will be relayed.  Message relaying is opt-in only.
            /// </summary>
            internal static IList<string> ListeningGroups { get; set; }

            /// <summary>
            /// Global set of listeners to which code trace messages will be relayed.
            /// </summary>
            internal static IList<ITraceListener> TraceListeners { get; set; }

            /// <summary>
            /// Determines if client code has added any listening groups whose code trace messages will be relayed.
            /// </summary>
            public static bool HasListeningGroups => CollectionUtil.HasAny(ListeningGroups);

            /// <summary>
            /// Determines if there are any registered trace listeners to which code trace messages will be relayed.
            /// </summary>
            public static bool HasTraceListeners => CollectionUtil.HasAny(TraceListeners);

            /// <summary>
            /// Determines if the provided listening group matches any of the client added listening groups. 
            /// A match occurs if the registered group is "*", or if it equals the provided listening group, 
            /// or if it starts with the provided listening group followed by a dot (indicating a sub-namespace).
            /// </summary>
            /// <param name="listeningGroup">The listening group to check.</param>
            /// <returns><c>true</c> if a match is found; otherwise, <c>false</c>.</returns>
            public static bool HasMatchingListeningGroup(string listeningGroup) =>
                HasMatchingListeningGroup(grp => grp.Equals("*") || grp.Equals(listeningGroup) || grp.StartsWith(listeningGroup + "."));

            /// <summary>
            /// Determines if the provided listening group matches any of the client added listening groups. 
            /// A match occurs if the registered group is "*", or if it equals the provided listening group, 
            /// or if it starts with the provided listening group followed by a dot (indicating a sub-namespace).
            /// </summary>
            /// <param name="predicate">The predicate to check against the listening groups.</param>
            /// <returns><c>true</c> if a match is found; otherwise, <c>false</c>.</returns>
            public static bool HasMatchingListeningGroup(Func<string, bool> predicate) =>
                CollectionUtil.HasAny(ListeningGroups, predicate);

            /// <summary>
            /// Adds one or more listening groups to the collection of groups whose code trace messages will be relayed.
            /// </summary>
            /// <param name="listeningGroups">The listening group(s) to add.</param>
            public static void AddListeningGroups(params string[] listeningGroups)
            {
                foreach (string group in listeningGroups)
                {
                    if (ListeningGroups == null)
                        ListeningGroups = new List<string>();
                    else if (ListeningGroups.Contains(group))
                        continue;
                    ListeningGroups.Add(group);
                }
            }

            /// <summary>
            /// Removes one or more listening groups from the collection of groups whose code trace messages will be relayed.
            /// </summary>
            /// <param name="listeningGroups">The listening group(s) to remove.</param>
            public static void RemoveListeningGroups(params string[] listeningGroups)
            {
                if (ListeningGroups == null)
                    return;

                foreach (string group in listeningGroups)
                {
                    ListeningGroups.Remove(group);
                }
            }

            /// <summary>
            /// Registers a trace listener to receive code trace messages. If the listener is already registered, it will not be added again.
            /// </summary>
            /// <param name="listener">The trace listener to register.</param>
            public static void RegisterTraceListener(ITraceListener listener)
            {
                if (TraceListeners == null)
                    TraceListeners = new List<ITraceListener>();
                else if (TraceListeners.Contains(listener))
                    return;
                TraceListeners.Add(listener);
            }

            /// <summary>
            /// Unregisters a trace listener so that it no longer receives code trace messages. If the listener is not registered, this method does nothing.
            /// </summary>
            /// <param name="listener">The trace listener to unregister.</param>
            public static void UnregisterTraceListener(ITraceListener listener)
            {
                if (TraceListeners == null)
                    return;
                TraceListeners.Remove(listener);
            }
        }
    }
}
