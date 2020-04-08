using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SeeShells.UI.Pages;

namespace SeeShells.UI.Node
{
    /// <summary>
    /// Used to navigate trough nodes one by one
    /// </summary>
    public class NodeNavigation
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        ResourceDictionary resourceDictionary = new ResourceDictionary();
        List<Object> nodes;
        Object currNode;
        int currIndex;

        public NodeNavigation(List<Object> nodes)
        {
            resourceDictionary.Source = new Uri("pack://application:,,,/SeeShells;component/UI/Templates/NodeStyle.xaml");
            this.nodes = nodes;
            SortNodes();
            currIndex = 0;
        }

        /// <summary>
        /// Navigates to the next node further in time.
        /// </summary>
        public void GoToNextNode()
        {
            currIndex++;
            if (nodes.ElementAtOrDefault(currIndex) != null)
            {
                if(nodes[currIndex] is Node)
                {
                    ResetPrevNodeStyle();
                    currNode = (Node)nodes[currIndex];
                    (currNode as Node).BringIntoView();
                    (currNode as Node).Style = (Style)resourceDictionary["LitUpNode"];

                }
                else if (nodes[currIndex] is StackedNodes)
                {
                    ResetPrevNodeStyle();
                    currNode = (StackedNodes)nodes[currIndex];
                    (currNode as StackedNodes).BringIntoView();
                    (currNode as StackedNodes).Style = (Style)resourceDictionary["LitUpStackedNode"];
                }
            }
            else
            {
                currIndex--;
            }

        }

        /// <summary>
        /// Navigates to the next node beack in time.
        /// </summary
        public void GoToPreviousNode()
        {
            currIndex--;
            if (nodes.ElementAtOrDefault(currIndex) != null)
            {
                if (nodes[currIndex] is Node)
                {
                    ResetPrevNodeStyle();
                    currNode = (Node)nodes[currIndex];
                    (currNode as Node).BringIntoView();
                    (currNode as Node).Style = (Style)resourceDictionary["LitUpNode"];

                }
                else if (nodes[currIndex] is StackedNodes)
                {
                    ResetPrevNodeStyle();
                    currNode = (StackedNodes)nodes[currIndex];
                    (currNode as StackedNodes).BringIntoView();
                    (currNode as StackedNodes).Style = (Style)resourceDictionary["LitUpStackedNode"];
                }
            }
            else
            {
                currIndex++;
            }
        }

        /// <summary>
        /// Used to set a new starting point from where to navigate nodes one by one.
        /// </summary>
        /// <param name="node">the node that a user clicks from the timeline to become the begining node for navigation</param>
        public void SetNewStartingPoint(Object node)
        {
            currIndex = nodes.IndexOf(node);
        }

        /// <summary>
        /// Changes the style of the node that last visited trough navigation back to default.
        /// </summary>
        private void ResetPrevNodeStyle()
        {
            if(currNode != null)
            {
                if(currNode is Node)
                {
                    (currNode as Node).Style = (Style)resourceDictionary["Node"];
                }
                else if(currNode is StackedNodes)
                {
                    (currNode as StackedNodes).Style = (Style)resourceDictionary["StackedNode"];
                }
            }
        }

        /// <summary>
        /// Sorts the nodes based on the time the event occures. This is done because there are ordering issues if the nodes are parsed directly from the childern of UI elements that contain them.
        /// </summary>
        private void SortNodes()
        {
            Comparison<Object> nodesComparer = new Comparison<Object>(CompareNodes);
            nodes.Sort(nodesComparer);
        }

        /// <summary>
        /// Compares nodes by their event time fields, reguardless of the node type.
        /// </summary>
        /// <param name="x">First node for camparison</param>
        /// <param name="y">Second node for comparison</param>
        /// <returns></returns>
        private static int CompareNodes(Object x, Object y)
        {
            if(x is Node && y is Node)
            {
                if (DateTime.Compare((x as Node).GetBlockTime(), (y as Node).GetBlockTime()) < 0)
                {
                    return -1;
                }
                else if (DateTime.Compare((x as Node).GetBlockTime(), (y as Node).GetBlockTime()) > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if(x is Node && y is StackedNodes)
            {
                if (DateTime.Compare((x as Node).GetBlockTime(), (y as StackedNodes).GetBlockTime()) < 0)
                {
                    return -1;
                }
                else if (DateTime.Compare((x as Node).GetBlockTime(), (y as StackedNodes).GetBlockTime()) > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if(x is StackedNodes && y is StackedNodes)
            {
                if (DateTime.Compare((x as StackedNodes).GetBlockTime(), (y as StackedNodes).GetBlockTime()) < 0)
                {
                    return -1;
                }
                else if (DateTime.Compare((x as StackedNodes).GetBlockTime(), (y as StackedNodes).GetBlockTime()) > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if(x is StackedNodes && y is Node)
            {
                if (DateTime.Compare((x as StackedNodes).GetBlockTime(), (y as Node).GetBlockTime()) < 0)
                {
                    return -1;
                }
                else if (DateTime.Compare((x as StackedNodes).GetBlockTime(), (y as Node).GetBlockTime()) > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            logger.Warn("Sorting the nodes for navigation failed!");
            return 0;
        }
    }
}
