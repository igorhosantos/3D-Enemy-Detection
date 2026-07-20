using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyDetection.DataStructures
{
    /// <summary>
    /// A K-Dimensional Tree (also known as K-D Tree) is a space-partitioning
    /// data structure for organizing points in a K-Dimensional space.
    /// This data structure acts similar to a binary search tree with each node representing
    /// data in the multidimensional space.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KdTree<T> : IEnumerable<T>, IEnumerable where T : Component
    {
        protected KdNode _root;
        protected KdNode _last;
        protected int _count;
        protected bool _just2D;
        protected KdNode[] _open;

        public int Count
        {
            get { return _count; }
        }

        public float AverageSearchLength { protected set; get; }
        public float AverageSearchDeep { protected set; get; }

        /// <summary>
        /// create a tree
        /// </summary>
        /// <param name="just2D">just use x/z</param>
        public KdTree(bool just2D = false)
        {
            _just2D = just2D;
        }

        public T this[int key]
        {
            get
            {
                if (key >= _count)
                    throw new ArgumentOutOfRangeException();
                var current = _root;
                for (var i = 0; i < key; i++)
                    current = current.Next;
                return current.Component;
            }
        }
        
        public void Add(T item)
        {
            Add(new KdNode() { Component = item });
        }
        
        public void Clear()
        {
            //rest for the garbage collection
            _root = null;
            _last = null;
            _count = 0;
        }
        
        /// <summary>
        /// Update positions (if objects moved)
        /// </summary>
        public void UpdatePositions()
        {
            //save old traverse
            var current = _root;
            while (current != null)
            {
                current.OldRef = current.Next;
                current = current.Next;
            }

            //save root
            current = _root;

            //reset values
            Clear();

            //readd
            while (current != null)
            {
                Add(current);
                current = current.OldRef;
            }
        }

        /// <summary>
        /// Method to enable foreach-loops
        /// </summary>
        /// <returns>Enumberator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var current = _root;
            while (current != null)
            {
                yield return current.Component;
                current = current.Next;
            }
        }
        
        /// <summary>
        /// Method to enable foreach-loops
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected float CalculatedDistance(Vector3 a, Vector3 b)
        {
            if (_just2D)
                return (a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z);
           
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
        }

        protected float GetSplitValue(int level, Vector3 position)
        {
            if (_just2D)
                return (level % 2 == 0) ? position.x : position.z;
               
            return (level % 3 == 0) ? position.x : (level % 3 == 1) ? position.y : position.z;
        }

        private void Add(KdNode newNode)
        {
            _count++;
            newNode.Left = null;
            newNode.Right = null;
            newNode.Level = 0;
            var parent = _findParent(newNode.Component.transform.position);

            //set last
            if (_last != null)
                _last.Next = newNode;
            _last = newNode;

            //set root
            if (parent == null)
            {
                _root = newNode;
                return;
            }

            var splitParent = GetSplitValue(parent);
            var splitNew = GetSplitValue(parent.Level, newNode.Component.transform.position);

            newNode.Level = parent.Level + 1;

            if (splitNew < splitParent)
                parent.Left = newNode; //go left
            else
                parent.Right = newNode; //go right
        }

        private KdNode _findParent(Vector3 position)
        {
            //travers from root to bottom and check every node
            var current = _root;
            var parent = _root;
            while (current != null)
            {
                var splitCurrent = GetSplitValue(current);
                var splitSearch = GetSplitValue(current.Level, position);

                parent = current;
                if (splitSearch < splitCurrent)
                    current = current.Left; //go left
                else
                    current = current.Right; //go right

            }

            return parent;
        }

        /// <summary>
        /// Find closer object to given position
        /// </summary>
        /// <param name="position">position</param>
        /// <returns>closest object</returns>
        public T FindCloserObject(Vector3 position)
        {
            return FindLocalCloserObject(position);
        }

        private T FindLocalCloserObject(Vector3 position, List<T> traversed = null)
        {
            if (_root == null)
                return null;

            var nearestDist = float.MaxValue;
            KdNode nearest = null;

            if (_open == null || _open.Length < Count)
                _open = new KdNode[Count];
            for (int i = 0; i < _open.Length; i++)
                _open[i] = null;

            var openAdd = 0;
            var openCur = 0;

            if (_root != null)
                _open[openAdd++] = _root;

            while (openCur < _open.Length && _open[openCur] != null)
            {
                var current = _open[openCur++];
                if (traversed != null)
                    traversed.Add(current.Component);

                var nodeDist = CalculatedDistance(position, current.Component.transform.position);
                if (nodeDist < nearestDist && nodeDist != 0)
                {
                    nearestDist = nodeDist;
                    nearest = current;
                }

                var splitCurrent = GetSplitValue(current);
                var splitSearch = GetSplitValue(current.Level, position);

                if (splitSearch < splitCurrent)
                {
                    if (current.Left != null)
                        _open[openAdd++] = current.Left; //go left
                    if (Mathf.Abs(splitCurrent - splitSearch) * Mathf.Abs(splitCurrent - splitSearch) < nearestDist &&
                        current.Right != null)
                        _open[openAdd++] = current.Right; //go right
                }
                else
                {
                    if (current.Right != null)
                        _open[openAdd++] = current.Right; //go right
                    if (Mathf.Abs(splitCurrent - splitSearch) * Mathf.Abs(splitCurrent - splitSearch) < nearestDist &&
                        current.Left != null)
                        _open[openAdd++] = current.Left; //go left
                }
            }

            AverageSearchLength = (99f * AverageSearchLength + openCur) / 100f;
            AverageSearchDeep = (99f * AverageSearchDeep + nearest.Level) / 100f;

            return nearest.Component;
        }

        private float GetSplitValue(KdNode node)
        {
            return GetSplitValue(node.Level, node.Component.transform.position);
        }

        protected class KdNode
        {
            internal T Component;
            internal int Level;
            internal KdNode Left;
            internal KdNode Right;
            internal KdNode Next;
            internal KdNode OldRef;
        }
    }
}