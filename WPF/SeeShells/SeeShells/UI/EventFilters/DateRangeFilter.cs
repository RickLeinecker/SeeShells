using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI.EventFilters {
    /// <summary>
    /// Filter's a list of <see cref="Node.Node"/>s by a specific <see cref="DateTime"/> criteria.
    /// </summary>
    public class DateRangeFilter : INodeFilter
    {
        private readonly DateTime startDate;
        private readonly DateTime endDate;

        /// <summary>
        /// Filter's a list of <see cref="Node.Node"/>s by a specific <see cref="DateTime"/> criteria.
        /// </summary>
        /// <param name="startDate">The earilest date acceptable (inclusive) for the return List of <see cref="Node"/>. Can be null.</param>
        /// <param name="endDate">The latest date acceptable (inclusive) for the return list of <see cref="Node"/>. Can be null.</param>
        public DateRangeFilter(DateTime? startDate, DateTime? endDate)
            {
                this.startDate = startDate ?? DateTime.MinValue;
                this.endDate = endDate ?? DateTime.MaxValue;
            }

        public void Apply(ref List<Node.Node> events)
        {
            for (int i = events.Count-1; i >= 0; i--) //iterate backwards because iterating forwards would be an issue with a list of changing size.
            {
                Node.Node node = events[i];
                IEvent nEvent = node.aEvent;

                //if the event falls outside of our two DateTime bounds
                if (DateTime.Compare(startDate, nEvent.EventTime) > 0 || DateTime.Compare(nEvent.EventTime, endDate) > 0)
                {
                    events.Remove(node);   
                }
            }

        }
    }

}
