using System;
using System.Collections.Generic;
using System.Windows;
using System.Xaml;
using SeeShells.ShellParser.ShellItems;

namespace SeeShells.UI.EventFilters
{
    /// <summary>
    /// Filters Events by <see cref="IShellItem"/>'s which have a property set specifying the owner of the registry object.
    /// </summary>
    public class EventUserFilter : INodeFilter
    {
        private readonly HashSet<string> acceptableUsers;

        /// <summary>
        /// Filters Events by <see cref="IShellItem"/>'s which have a property set specifying the owner of the registry object.
        /// If multiple Users are specified, returned events are one of the specified types.
        /// </summary>
        /// <param name="users">one or more acceptable user (names, SID, etc.) to filter on.</param>
        public EventUserFilter(params string[] users)
        {
            acceptableUsers = new HashSet<string>(users);
        }

        public void Apply(ref List<Node.Node> nodes)
        {
            if (acceptableUsers.Count == 0)
            {
                return; //dont apply filter if no users to filter on
            }

            //iterate backwards because iterating forwards would be an issue with a list of changing size.
            for (int i = nodes.Count - 1; i >= 0; i--) 
            {
                Node.Node node = nodes[i];
                IShellItem nParent = node.aEvent.Parent;

                var props = nParent.GetAllProperties();
                var keyName = "RegistryOwner";

                //check for the property and if it exists, verify its a user we want
                if (props.ContainsKey(keyName))
                {
                    if (!acceptableUsers.Contains(props[keyName]))
                    {
                        node.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    node.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}