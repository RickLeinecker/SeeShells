using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SeeShells.UI
{
    /// <summary>
    /// A collection of static methods used to filter out a <see cref="IList{T}"/> of <see cref="IEvent"/>'s
    /// </summary>
    public class FilterEventUtils
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Filter's <see cref="IEvent"/>s by their <see cref="IEvent.EventTime"/> property.
        /// </summary>
        /// <param name="events">A Collection of events to filter on.</param>
        /// <param name="startDate">The earilest date acceptable (inclusive) for the return List of events. Can be null.</param>
        /// <param name="endDate">The latest date acceptable (inclusive) for the retunr list of events. Can be null.</param>
        /// <returns>A list of events filtered by the supplied <see cref="DateTime"/> restrictions.</returns>
        public static IList<IEvent> FilterDateRange(IEnumerable<IEvent> events, DateTime? startDate, DateTime? endDate)
        {
            IList<IEvent> returnList = new List<IEvent>();

            DateTime start = startDate ?? DateTime.MinValue;
            DateTime end = endDate ?? DateTime.MaxValue;
            foreach (IEvent item in events)
            {
                if (DateTime.Compare(start, item.EventTime) <= 0 && DateTime.Compare(item.EventTime, end) <= 0)
                {
                    returnList.Add(item);
                }
                    
            }

            return returnList;
        }

        /// <summary>
        /// Filter's <see cref="IEvent"/>s by their <see cref="IEvent.EventType"/> property.
        /// If multiple EventTypes are specified, returned events are one of the specified types.
        /// </summary>
        /// <param name="events">A Collection of events to filter on.</param>
        /// <param name="eventTypes">One or more acceptable types to filter on. </param>
        /// <returns>A list of events filtered by the supplied <see cref="IEvent.EventType"/> restrictions.</returns>
        public static IList<IEvent> FilterEventType(IEnumerable<IEvent> events, params string[] eventTypes)
        {
            IList<IEvent> retList = new List<IEvent>();

            foreach (IEvent item in events)
            {
                bool acceptableType = false;
                foreach (string type in eventTypes)
                {
                    if (item.EventType.Trim().Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        acceptableType = true;
                        break;
                    }
                }

                if (acceptableType)
                {
                    retList.Add(item);
                }
            }

            return retList;
        }

        /// <summary>
        /// Filter's <see cref="IEvent"/>s by their <see cref="IEvent.Parent"/> property.
        /// If multiple <see cref="IEvent.Parent"/> are specified, returned events are one of the specified types.
        /// </summary>
        /// <param name="events">A Collection of events to filter on.</param>
        /// <param name="shellItems">One or more acceptable <see cref="IShellItem"/> to filter on. </param>
        /// <returns>A list of events filtered by the supplied <see cref="IEvent.Parent"/> restrictions.</returns>
        public static IList<IEvent> FilterParent(IEnumerable<IEvent> events, params IShellItem[] shellItems)
        {
            IList<IEvent> retList = new List<IEvent>();

            foreach (IEvent item in events)
            {
                bool acceptableParent = false;
                foreach (var parent in shellItems)
                {
                    if (item.Parent.Equals(parent))
                    {
                        acceptableParent = true;
                        break;
                    }
                }

                if (acceptableParent)
                {
                    retList.Add(item);
                }
            }

            return retList;

        }
        
        /// <summary>
        /// Filter's <see cref="IEvent"/>s by their <see cref="IEvent.Name"/> property.
        /// If multiple <see cref="IEvent.Name"/> are specified, returned events are one of the specified types.
        /// </summary>
        /// <param name="events">A Collection of events to filter on.</param>
        /// <param name="names">One or more acceptable <see cref="string"/> to filter on. </param>
        /// <returns>A list of events filtered by the supplied <see cref="IEvent.Name"/> restrictions.</returns>
        public static IList<IEvent> FilterName(IEnumerable<IEvent> events, params string[] names)
        {
            IList<IEvent> retList = new List<IEvent>();

            foreach (IEvent item in events)
            {
                bool acceptableName = false;
                foreach (string name in names)
                {
                    if (item.Name.Equals(name))
                    {
                        acceptableName = true;
                        break;
                    }
                }

                if (acceptableName)
                {
                    retList.Add(item);
                }
            }

            return retList;

        }

        /// <summary>
        /// Filter's <see cref="IEvent"/>s by a property in the <see cref="IEvent.Parent"/>'s <see cref="IShellItem.GetAllProperties"/> property.
        /// This generic filter allows for finding <see cref="IEvent"/>'s that share a property that arent supported via a combination of other filter methods.
        /// 
        /// Example: to filter on IEvent's whos parent has the <see cref="IShellItem.TypeName"/> of "Volume" (aka 0x20 shellbags) the following would be performed:
        /// <code> var filteredList = FilterStringProperty(events, "Volume", false)</code>
        /// Because this uses <see cref="IShellItem.GetAllProperties"/> if a <see cref="IEvent.Name"/> also contained "Volume" it would also be returned by the filter.
        /// 
        /// This Filter accepts partial matches by default. based on the previous example, items following values would return if they existed: "Volume" & "Volume - Named"
        /// Regex searches also check for partial matches due the high variability in possible results. 
        /// Regex searches will time out after <paramref name="regexTimeoutInMilliseconds"/> milliseocnds of searching and return no results. (15 seconds by default)
        /// 
        /// On a large list of <see cref="IEvent"/>'s or <see cref="IEvent.Parent"/>s, this operation can take a signficant amount of time.
        /// Caching results is HIGHLY recommened. 
        /// </summary>
        /// <param name="events">A Collection of events to filter on.</param>
        /// <param name="value"></param>
        /// <param name="useRegex">True if <paramref name="value"/> is a regex pattern to be matched. False for a standard <see cref="String.Contains(string)"/></param>
        /// <param name="regexTimeoutInMilliseconds">Timeout value in milliseconds where a Regex search will stop and return no value. Ignored if <paramref name="useRegex"/> is false.</param>
        /// <returns></returns>
        public static IList<IEvent> FilterAnyString(IEnumerable<IEvent> events, string value, bool useRegex, long regexTimeoutInMilliseconds = 6000)
        {
            IList<IEvent> retList = new List<IEvent>();
            Regex regex = null;
            //if we've seen the parent, mark true if a previous search found the value - false otherwise.
            Dictionary<IShellItem, bool> seenParents = new Dictionary<IShellItem, bool>();
            
            //obtain all string values from the IEvent's Parent objects
            foreach (IEvent ievent in events)
            {
                //only check unique parent values 
                if (seenParents.ContainsKey(ievent.Parent))
                {
                    //check if we need to add this event becuase it has the property we've searched for
                    if (seenParents[ievent.Parent])
                    {
                        retList.Add(ievent);
                    }
                } 
                else 
                { //grab all values from the parent's properties
                    //add all properties names and values to the searchable strings
                    var keyValues = ievent.Parent.GetAllProperties();
                    bool foundMatch = false;
                    foreach (var key in keyValues)
                    {
                        //begin search 
                        if (useRegex)
                        {
                            try
                            {
                                regex = regex ?? new Regex(value, RegexOptions.None, TimeSpan.FromMilliseconds(regexTimeoutInMilliseconds));
                            } catch (Exception ex) when ( ex is ArgumentException ||  ex is RegexMatchTimeoutException ) //catch parsing error and timeout
                            {
                                logger.Warn(ex);
                                return retList; //no results if regex is broken.
                            }

                            if (regex.IsMatch(key.Key) || (regex.IsMatch(key.Value)))
                            {
                                foundMatch = true;
                                break;
                            }
                        }
                        else
                        {
                            if (key.Key.Contains(value) || key.Value.Contains(value))
                            {
                                foundMatch = true;
                                break;
                            }
                        }
                    }

                    //if we found a match add result
                    if (foundMatch)
                    {
                        retList.Add(ievent);
                    }
                    seenParents.Add(ievent.Parent, foundMatch);

                }
            }
            
            return retList;
        }





    }
}
