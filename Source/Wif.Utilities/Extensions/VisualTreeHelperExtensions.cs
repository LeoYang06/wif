/**************************************************************************
 *      File Name：VisualTreeHelperExtensions.cs
 *    Description：VisualTreeHelperExtensions.cs class description...
 *      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
 *        Creator：Leo Yang
 *    Create Time：2020/12/15
 *Project Address：https://github.com/LeoYang-Chuese/wif
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 查找控件扩展方法 http://www.hardcodet.net/uploads/2009/06/UIHelper.cs
    /// </summary>
    public static class VisualTreeHelperExtensions
    {
        #region Methods

        /// <summary>
        /// The GetParent
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject" /></param>
        /// <param name="recurseIntoPopup">The <see cref="bool" /></param>
        /// <returns>The <see cref="DependencyObject" /></returns>
        private static DependencyObject GetParent(DependencyObject element, bool recurseIntoPopup)
        {
            if (recurseIntoPopup)
            {
                // Case 126732 : To correctly detect parent of a popup we must do that exception case

                if (element is Popup popup && popup.PlacementTarget != null)
                {
                    return popup.PlacementTarget;
                }
            }

            DependencyObject parent = element is not Visual visual ? null : VisualTreeHelper.GetParent(visual);

            if (parent == null)
            {
                // No Visual parent. Check in the logical tree.

                if (element is FrameworkElement fe)
                {
                    parent = fe.Parent;

                    if (parent == null)
                    {
                        parent = fe.TemplatedParent;
                    }
                }
                else
                {
                    if (element is FrameworkContentElement fce)
                    {
                        parent = fce.Parent;

                        if (parent == null)
                        {
                            parent = fce.TemplatedParent;
                        }
                    }
                }
            }

            return parent;
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <param name="childName">x:Name or Name of child.</param>
        /// <returns>The <see cref="T" /></returns>
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                if (child is not T childType)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    // If the child's name is set for search
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// This will search for a child of the specified type. The search is performed
        /// hierarchically, breadth first (as opposed to depth first).
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="parent">
        /// The root of the tree to search for. This element itself is not checked.
        /// </param>
        /// <param name="additionalCheck">The additionalCheck <see cref="Func{T, bool}" /></param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        public static T FindChild<T>(this DependencyObject parent, Func<T, bool> additionalCheck = null) where T : DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            T   child;

            for (var index = 0; index < childrenCount; index++)
            {
                child = VisualTreeHelper.GetChild(parent, index) as T;

                if (child != null)
                {
                    if (additionalCheck == null)
                    {
                        return child;
                    }
                    else
                    {
                        if (additionalCheck(child))
                        {
                            return child;
                        }
                    }
                }
            }

            for (var index = 0; index < childrenCount; index++)
            {
                child = FindChild(VisualTreeHelper.GetChild(parent, index), additionalCheck);

                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        /// <summary>
        /// 根据控件名称，查找子控件 elementName为空时，查找指定类型的子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj <see cref="DependencyObject" /></param>
        /// <param name="elementName">The elementName <see cref="string" /></param>
        /// <returns>The <see cref="T" /></returns>
        public static T FindChildByName<T>(this DependencyObject obj, string elementName) where T : FrameworkElement
        {
            DependencyObject child = null;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if ((child is T && ((T)child).Name == elementName) || string.IsNullOrEmpty(elementName))
                {
                    return (T)child;
                }

                var grandChild = FindChildByName<T>(child, elementName);
                if (grandChild != null)
                {
                    return grandChild;
                }
            }

            return null;
        }

        /// <summary>
        /// Analyzes both visual and logical tree in order to find all elements of a given type that
        /// are descendants of the <paramref name="source" /> item.
        /// </summary>
        /// <typeparam name="T">The type of the queried items.</typeparam>
        /// <param name="source">The source <see cref="DependencyObject" /></param>
        /// <param name="forceUsingTheVisualTreeHelper">
        /// Sometimes it's better to search in the VisualTree (e.g. in tests)
        /// </param>
        /// <returns>All descendants of <paramref name="source" /> that match the requested type.</returns>
        public static IEnumerable<T> FindChildren<T>(DependencyObject source, bool forceUsingTheVisualTreeHelper = false) where T : DependencyObject
        {
            if (source != null)
            {
                var childs = GetChildObjects(source, forceUsingTheVisualTreeHelper);
                foreach (DependencyObject child in childs)
                {
                    //analyze if children match the requested type
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    //recurse tree
                    foreach (T descendant in FindChildren<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        /// <summary>
        /// 根据控件名称，查找子控件集合 elementName为空时，查找指定类型的所有子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj <see cref="DependencyObject" /></param>
        /// <param name="elementName">The elementName <see cref="string" /></param>
        /// <returns>The <see cref="List{T}" /></returns>
        public static List<T> FindChildsByName<T>(this DependencyObject obj, string elementName) where T : FrameworkElement
        {
            DependencyObject child     = null;
            var              childList = new List<T>();
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if ((child is T && ((T)child).Name == elementName) || string.IsNullOrEmpty(elementName))
                {
                    childList.Add((T)child);
                }
                else
                {
                    var grandChildList = FindChildsByName<T>(child, elementName);
                    if (grandChildList != null)
                    {
                        childList.AddRange(grandChildList);
                    }
                }
            }

            return childList;
        }

        /// <summary>
        /// This will search for a parent of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="startingObject">
        /// The node where the search begins. This element is not checked.
        /// </param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        public static T FindParent<T>(DependencyObject startingObject) where T : DependencyObject
        {
            return FindParent<T>(startingObject, false, null);
        }

        /// <summary>
        /// This will search for a parent of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="startingObject">The node where the search begins.</param>
        /// <param name="checkStartingObject">Should the specified startingObject be checked first.</param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        public static T FindParent<T>(DependencyObject startingObject, bool checkStartingObject) where T : DependencyObject
        {
            return FindParent<T>(startingObject, checkStartingObject, null);
        }

        /// <summary>
        /// This will search for a parent of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="startingObject">The node where the search begins.</param>
        /// <param name="checkStartingObject">Should the specified startingObject be checked first.</param>
        /// <param name="additionalCheck">The additionalCheck <see cref="Func{T, bool}" /></param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        public static T FindParent<T>(DependencyObject startingObject, bool checkStartingObject, Func<T, bool> additionalCheck) where T : DependencyObject
        {
            T                foundElement;
            DependencyObject parent = checkStartingObject ? startingObject : GetParent(startingObject, true);

            while (parent != null)
            {
                foundElement = parent as T;

                if (foundElement != null)
                {
                    if (additionalCheck == null)
                    {
                        return foundElement;
                    }
                    else
                    {
                        if (additionalCheck(foundElement))
                        {
                            return foundElement;
                        }
                    }
                }

                parent = GetParent(parent, true);
            }

            return null;
        }

        /// <summary>
        /// 根据控件名称，查找父控件 elementName为空时，查找指定类型的父控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj <see cref="DependencyObject" /></param>
        /// <param name="elementName">The elementName <see cref="string" /></param>
        /// <returns>The <see cref="T" /></returns>
        public static T FindParentByName<T>(this DependencyObject obj, string elementName) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == elementName || string.IsNullOrEmpty(elementName)))
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// The FindVisualChildItem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The <see cref="DependencyObject" /></param>
        /// <returns>The <see cref="T" /></returns>
        public static T FindVisualChildItem<T>(this DependencyObject obj) where T : DependencyObject
        {
            if (null != obj)
            {
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    var childOfChild = FindVisualChildItem<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The FindVisualChildItem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The <see cref="DependencyObject" /></param>
        /// <param name="name">The <see cref="string" /></param>
        /// <returns>The <see cref="T" /></returns>
        public static T FindVisualChildItem<T>(this DependencyObject obj, string name) where T : FrameworkElement
        {
            if (null != obj)
            {
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T && (child as T).Name.Equals(name))
                    {
                        return (T)child;
                    }

                    var childOfChild = FindVisualChildItem<T>(child, name);
                    if (childOfChild != null && childOfChild is T && childOfChild.Name.Equals(name))
                    {
                        return childOfChild;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// This method is an alternative to WPF's <see cref="VisualTreeHelper.GetChild" /> method,
        /// which also supports content elements. Keep in mind that for content elements, this
        /// method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="parent">The item to be processed.</param>
        /// <param name="forceUsingTheVisualTreeHelper">
        /// Sometimes it's better to search in the VisualTree (e.g. in tests)
        /// </param>
        /// <returns>The submitted item's child elements, if available.</returns>
        public static IEnumerable<DependencyObject> GetChildObjects(DependencyObject parent, bool forceUsingTheVisualTreeHelper = false)
        {
            if (parent == null)
            {
                yield break;
            }

            if (!forceUsingTheVisualTreeHelper && (parent is ContentElement || parent is FrameworkElement))
            {
                //use the logical tree for content / framework elements
                foreach (object obj in LogicalTreeHelper.GetChildren(parent))
                {
                    if (obj is DependencyObject depObj)
                    {
                        yield return (DependencyObject)obj;
                    }
                }
            }
            else
            {
                //use the visual tree per default
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (var i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(parent, i);
                }
            }
        }

        /// <summary>
        /// 返回从叶节点开始的完整逻辑祖先链。
        /// </summary>
        /// <param name="leaf">叶节点</param>
        /// <returns>返回一个包含从叶节点开始的所有逻辑祖先的迭代器</returns>
        public static IEnumerable<DependencyObject> GetLogicalAncestry(this DependencyObject leaf)
        {
            while (leaf != null)
            {
                yield return leaf;
                leaf = LogicalTreeHelper.GetParent(leaf);
            }
        }

        /// <summary>
        /// Tries its best to return the specified element's parent. It will try to find, in this
        /// order, the VisualParent, LogicalParent, LogicalTemplatedParent. It only works for
        /// Visual, FrameworkElement or FrameworkContentElement.
        /// </summary>
        /// <param name="element">The element <see cref="DependencyObject" /></param>
        /// <returns>The <see cref="DependencyObject" /></returns>
        public static DependencyObject GetParent(DependencyObject element)
        {
            return GetParent(element, true);
        }

        /// <summary>
        /// This method is an alternative to WPF's <see cref="VisualTreeHelper.GetParent" /> method,
        /// which also supports content elements. Keep in mind that for content element, this method
        /// falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The <see cref="DependencyObject" /></returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null)
            {
                return null;
            }

            //handle content elements separately
            if (child is ContentElement contentElement)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                {
                    return parent;
                }

                return contentElement is FrameworkContentElement fce ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            if (child is FrameworkElement frameworkElement)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null)
                {
                    return parent;
                }
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        /// <summary>
        /// 返回从叶节点开始的完整视觉祖先链。
        /// <para>如果元素不是 <see cref="Visual" /> 或 <see cref="Visual3D" />， 则使用逻辑祖先链。</para>
        /// </summary>
        /// <param name="leaf">叶节点</param>
        /// <returns>返回一个包含从叶节点开始的所有祖先的迭代器</returns>
        public static IEnumerable<DependencyObject> GetVisualAncestry(this DependencyObject leaf)
        {
            while (leaf != null)
            {
                yield return leaf;
                leaf = leaf is Visual || leaf is Visual3D ? VisualTreeHelper.GetParent(leaf) : LogicalTreeHelper.GetParent(leaf);
            }
        }

        /// <summary>
        /// Returns true if the specified element is a child of parent somewhere in the visual tree.
        /// This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="element">The element that is potentially a child of the specified parent.</param>
        /// <param name="parent">The element that is potentially a parent of the specified element.</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool IsDescendantOf(DependencyObject element, DependencyObject parent)
        {
            return IsDescendantOf(element, parent, true);
        }

        /// <summary>
        /// Returns true if the specified element is a child of parent somewhere in the visual tree.
        /// This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="element">The element that is potentially a child of the specified parent.</param>
        /// <param name="parent">The element that is potentially a parent of the specified element.</param>
        /// <param name="recurseIntoPopup">The recurseIntoPopup <see cref="bool" /></param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool IsDescendantOf(DependencyObject element, DependencyObject parent, bool recurseIntoPopup)
        {
            while (element != null)
            {
                if (element == parent)
                {
                    return true;
                }

                element = GetParent(element, recurseIntoPopup);
            }

            return false;
        }

        /// <summary>
        /// 判断一个DependencyObject是否是另一个DependencyObject的后代。
        /// </summary>
        /// <param name="leaf">可能是后代的DependencyObject</param>
        /// <param name="ancestor">可能是祖先的DependencyObject</param>
        /// <returns>如果leaf是ancestor的后代，则返回true，否则返回false</returns>
        public static bool IsDescendantOfDependencyObject(this DependencyObject leaf, DependencyObject ancestor)
        {
            DependencyObject parent = null;
            foreach (DependencyObject node in leaf.GetVisualAncestry())
            {
                if (Equals(node, ancestor))
                {
                    return true;
                }

                parent = node;
            }

            return parent?.GetLogicalAncestry().Contains(ancestor) == true;
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">The child <see cref="DependencyObject" /></param>
        /// <returns>The <see cref="T" /></returns>
        public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null)
            {
                return null;
            }

            //check if the parent matches the type we're looking for
            var parent = parentObject as T;
            return parent ?? TryFindParent<T>(parentObject);
        }

        /// <summary>
        /// 提供了一种以深度优先的方式遍历DependencyObject的视觉树的方法。
        /// </summary>
        /// <param name="node">需要遍历的DependencyObject节点。</param>
        /// <returns>返回一个包含视觉树中所有DependencyObject的迭代器。</returns>
        /// <exception cref="ArgumentNullException">当提供的DependencyObject为null时，抛出此异常。</exception>
        public static IEnumerable<DependencyObject> VisualDepthFirstTraversal(this DependencyObject node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            yield return node;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(node, i);
                foreach (DependencyObject descendant in child.VisualDepthFirstTraversal())
                {
                    yield return descendant;
                }
            }
        }

        #endregion Methods
    }
}