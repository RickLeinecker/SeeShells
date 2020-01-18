using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI
{
    /// <summary>
    /// A Discrete time-based event that is derived from a ShellBag.
    /// </summary>
    interface IEvent
    {
        /// <summary>
        /// Identifier for the entity which was modified at this particular <see cref="EventTime"/>
        /// (e.g. "C:\", "Minecraft.zip") 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Details the time in which an event took place.
        /// </summary>
        DateTime EventTime { get; }

        /// <summary>
        /// The ShellItem which this event was derived from.
        /// Provides optional extra information to the formulation of this event.
        /// </summary>
        IShellItem Parent { get; }

        /// <summary>
        /// Categorizes the action which was preformed.
        /// </summary>
        string EventType { get; }
    }
}
