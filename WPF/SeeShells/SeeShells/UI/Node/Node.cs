﻿using System.Windows.Controls;

namespace SeeShells.UI.Node
{
    public class Node
    {
        public IEvent aEvent;
        public InformedDot dot;
        public TextBlock block;

        /// <summary>
        /// The Node is an object that stores event data and has graphical objects to be displayed on a timeline.
        /// </summary>
        /// <param name="aEvent">object that stores event/shellbag data</param>
        /// <param name="dot">object to represent an event on a timeline</param>
        /// <param name="block">object to display event details on a timeline</param>
        public Node(IEvent aEvent, InformedDot dot, TextBlock block)
        {
            this.aEvent = aEvent;
            this.dot = dot;
            this.block = block;
        }
    }
}
