﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Test.ObjectComparison
{
    /// <summary>
    /// Creates a graph by extracting public instance properties in the object. If the
    /// property is an IEnumerable, extract the items. If an exception is thrown
    /// when accessing a property on the left object, it is considered a match if 
    /// the same exception type is thrown when accessing the property on the right
    /// object.
    /// </summary>
    public sealed class PublicPropertyObjectGraphFactory : ObjectGraphFactory
    {
        #region Public Members

        /// <summary>
        /// Creates a graph for the given object by extracting public properties.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="typePropertiesToIgnore">A set of properties which will constitute leaf nodes</param>
        /// <param name="objectPropertiesToIgnore">A set of specific object properties which will constitute leaf nodes</param>
        /// <returns>The root node of the created graph.</returns>
<<<<<<< HEAD
<<<<<<< HEAD
        public override GraphNode CreateObjectGraph(object value,
                                                    IEnumerable<MemberInfo> typePropertiesToIgnore,
                                                    Dictionary<object, List<MemberInfo>> objectPropertiesToIgnore)
=======
        public override GraphNode CreateObjectGraph(object value, IEnumerable<MemberInfo> propertiesToIgnore)
>>>>>>> c8bb31f489161031b89e5649a4c57a760e58c337
=======
        public override GraphNode CreateObjectGraph(object value,
                                                    IEnumerable<MemberInfo> typePropertiesToIgnore,
                                                    Dictionary<object, List<MemberInfo>> objectPropertiesToIgnore)
>>>>>>> cd25d43eeaa9f998f9f2b5ca9cbfe5233f9ae584
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            // Queue of pending nodes 
            Queue<GraphNode> pendingQueue = new Queue<GraphNode>();

            // Dictionary of < object hashcode, node > - to lookup already visited objects 
            Dictionary<int, GraphNode> visitedObjects = new Dictionary<int, GraphNode>();

            // Build the root node and enqueue it
            var root = new GraphNode()
                           {
                               Name = value is IEnumerable ? "" : value.GetType().Name,
                               ObjectValue = value
                           };
            pendingQueue.Enqueue(root);

            while (pendingQueue.Count != 0)
            {
                GraphNode currentNode = pendingQueue.Dequeue();
                object nodeData = currentNode.ObjectValue;
                Type nodeType = currentNode.ObjectType;

                // If we have reached a leaf node -
                // no more processing is necessary
                if (IsLeafNode(nodeData, nodeType))
                {
                    continue;
                }

                // Handle loops by checking the visted objects 
                if (visitedObjects.Keys.Contains(nodeData.GetHashCode()))
                {
                    // Caused by a cycle - we have alredy seen this node so
                    // use the existing node instead of creating a new one
                    GraphNode prebuiltNode = visitedObjects[nodeData.GetHashCode()];
                    currentNode.Children.Add(prebuiltNode);
                    continue;
                }
                visitedObjects.Add(nodeData.GetHashCode(), currentNode);

                // Extract and add child nodes for current object //
<<<<<<< HEAD
<<<<<<< HEAD
                Collection<GraphNode> childNodes = GetChildNodes(nodeData, typePropertiesToIgnore, objectPropertiesToIgnore);
=======
                Collection<GraphNode> childNodes = GetChildNodes(nodeData, propertiesToIgnore);
>>>>>>> c8bb31f489161031b89e5649a4c57a760e58c337
=======
                Collection<GraphNode> childNodes = GetChildNodes(nodeData, typePropertiesToIgnore, objectPropertiesToIgnore);
>>>>>>> cd25d43eeaa9f998f9f2b5ca9cbfe5233f9ae584
                foreach (GraphNode childNode in childNodes)
                {
                    childNode.Parent = currentNode;
                    currentNode.Children.Add(childNode);

                    pendingQueue.Enqueue(childNode);
                }
            }

            return root;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Given an object, get a list of the immediate child nodes
        /// </summary>
        /// <param name="nodeData">The object whose child nodes need to be extracted</param>
        /// <returns>Collection of child graph nodes</returns>
<<<<<<< HEAD
<<<<<<< HEAD
        private Collection<GraphNode> GetChildNodes(object nodeData,
                                                    IEnumerable<MemberInfo> typePropertiesToIgnore,
                                                    Dictionary<object, List<MemberInfo>> objectPropertiesToIgnore)
=======
        private Collection<GraphNode> GetChildNodes(object nodeData, IEnumerable<MemberInfo> propertiesToIgnore)
>>>>>>> c8bb31f489161031b89e5649a4c57a760e58c337
=======
        private Collection<GraphNode> GetChildNodes(object nodeData,
                                                    IEnumerable<MemberInfo> typePropertiesToIgnore,
                                                    Dictionary<object, List<MemberInfo>> objectPropertiesToIgnore)
>>>>>>> cd25d43eeaa9f998f9f2b5ca9cbfe5233f9ae584
        {
            Collection<GraphNode> childNodes = new Collection<GraphNode>();

            // Extract and add properties 
<<<<<<< HEAD
<<<<<<< HEAD
            foreach (GraphNode child in ExtractProperties(nodeData, typePropertiesToIgnore, objectPropertiesToIgnore))
=======
            foreach (GraphNode child in ExtractProperties(nodeData, propertiesToIgnore))
>>>>>>> c8bb31f489161031b89e5649a4c57a760e58c337
=======
            foreach (GraphNode child in ExtractProperties(nodeData, typePropertiesToIgnore, objectPropertiesToIgnore))
>>>>>>> cd25d43eeaa9f998f9f2b5ca9cbfe5233f9ae584
            {
                childNodes.Add(child);
            }

            // Extract and add IEnumerable content 
            if (IsIEnumerable(nodeData))
            {
                foreach (GraphNode child in GetIEnumerableChildNodes(nodeData))
                {
                    childNodes.Add(child);
                }
            }

            return childNodes;
        }

<<<<<<< HEAD
<<<<<<< HEAD
        private List<GraphNode> ExtractProperties(object nodeData,
                                                  IEnumerable<MemberInfo> typePropertiesToIgnore,
                                                  Dictionary<object, List<MemberInfo>> objectPropertiesToIgnore)
=======
        private List<GraphNode> ExtractProperties(object nodeData, IEnumerable<MemberInfo> propertiesToIgnore)
>>>>>>> c8bb31f489161031b89e5649a4c57a760e58c337
=======
        private List<GraphNode> ExtractProperties(object nodeData,
                                                  IEnumerable<MemberInfo> typePropertiesToIgnore,
                                                  Dictionary<object, List<MemberInfo>> objectPropertiesToIgnore)
>>>>>>> cd25d43eeaa9f998f9f2b5ca9cbfe5233f9ae584
        {
            List<GraphNode> childNodes = new List<GraphNode>();

            if (IsIEnumerable(nodeData))
                return childNodes;

            PropertyInfo[] properties = nodeData.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
<<<<<<< HEAD
<<<<<<< HEAD
                if (typePropertiesToIgnore.Contains(property))
=======
                if(propertiesToIgnore.Contains(property))
>>>>>>> c8bb31f489161031b89e5649a4c57a760e58c337
=======
                if (typePropertiesToIgnore.Contains(property))
>>>>>>> cd25d43eeaa9f998f9f2b5ca9cbfe5233f9ae584
                    continue;

                object value = null;

                ParameterInfo[] parameters = property.GetIndexParameters();
                // Skip indexed properties and properties that cannot be read
                if (property.CanRead && parameters.Length == 0)
                {
                    try
                    {
                        value = property.GetValue(nodeData, null);
                    }
                    catch (Exception ex)
                    {
                        // If accessing the property threw an exception
                        // then make the type of exception as the child.
                        // Do we want to validate the entire exception object 
                        // here ? - currently not doing to improve perf.
                        value = ex.GetType().ToString();
                    }

                    GraphNode childNode = new GraphNode()
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> cd25d43eeaa9f998f9f2b5ca9cbfe5233f9ae584
                                              {
                                                  Name = property.Name,
                                                  ObjectValue = value,
                                                  Ignore = IsPropertySupposedToBeIgnored(nodeData,
                                                                                         property,
                                                                                         objectPropertiesToIgnore)
                                              };
<<<<<<< HEAD
=======
                    {
                        Name = property.Name,
                        ObjectValue = value
                    };
>>>>>>> c8bb31f489161031b89e5649a4c57a760e58c337
=======
>>>>>>> cd25d43eeaa9f998f9f2b5ca9cbfe5233f9ae584

                    childNodes.Add(childNode);
                }
            };

            return childNodes;
        }

        private bool IsPropertySupposedToBeIgnored(object nodeData, PropertyInfo property, Dictionary<object, List<MemberInfo>> objectPropertiesToIgnore)
        {
            return objectPropertiesToIgnore.ContainsKey(nodeData) &&
                   objectPropertiesToIgnore[nodeData].Contains(property);
        }

        private static List<GraphNode> GetIEnumerableChildNodes(object nodeData)
        {
            List<GraphNode> childNodes = new List<GraphNode>();

            IEnumerable enumerableData = nodeData as IEnumerable;
            IEnumerator enumerator = enumerableData.GetEnumerator();

            int count = 0;
            while (enumerator.MoveNext())
            {
                GraphNode childNode = new GraphNode()
                {
                    Name = "IEnumerable" + count++,
                    ObjectValue = enumerator.Current,
                };

                childNodes.Add(childNode);
            }

            return childNodes;
        }

        private static bool IsIEnumerable(object nodeData)
        {
            IEnumerable enumerableData = nodeData as IEnumerable;
            if (enumerableData != null &&
                enumerableData.GetType().IsPrimitive == false &&
                nodeData.GetType() != typeof(System.String))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsLeafNode(object nodeData, Type nodeType)
        {
            return nodeData == null ||
                               nodeType.IsPrimitive ||
                               nodeType == typeof(string);
        }

        #endregion
    }
}