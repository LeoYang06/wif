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
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 查找控件扩展方法
    ///     http://www.hardcodet.net/uploads/2009/06/UIHelper.cs
    /// </summary>
    public static class VisualTreeHelperExtensions
    {
        #region Methods

        /// <summary>
        /// Finds a Child of a given item in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The <see cref="T"/></returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
                where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                var childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T) child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T) child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// This will search for a child of the specified type. The search is performed
        ///     hierarchically, breadth first (as opposed to depth first).
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="parent">The root of the tree to search for. This element itself is not checked.</param>
        /// <param name="additionalCheck">The additionalCheck<see cref="Func{T, bool}"/></param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        public static T FindChild<T>(this DependencyObject parent, Func<T, bool> additionalCheck = null)
                where T : DependencyObject
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            T child;

            for (var index = 0; index < childrenCount; index++)
            {
                child = VisualTreeHelper.GetChild(parent, index) as T;

                if (child != null)
                    if (additionalCheck == null)
                    {
                        return child;
                    }
                    else
                    {
                        if (additionalCheck(child))
                            return child;
                    }
            }

            for (var index = 0; index < childrenCount; index++)
            {
                child = FindChild(VisualTreeHelper.GetChild(parent, index), additionalCheck);

                if (child != null)
                    return child;
            }

            return null;
        }

        /// <summary>
        /// 根据控件名称，查找子控件
        ///     elementName为空时，查找指定类型的子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj<see cref="DependencyObject"/></param>
        /// <param name="elementName">The elementName<see cref="string"/></param>
        /// <returns>The <see cref="T"/></returns>
        public static T FindChildByName<T>(this DependencyObject obj, string elementName)
                where T : FrameworkElement
        {
            DependencyObject child = null;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && ((T) child).Name == elementName || string.IsNullOrEmpty(elementName))
                    return (T) child;

                var grandChild = FindChildByName<T>(child, elementName);
                if (grandChild != null) return grandChild;
            }

            return null;
        }

        /// <summary>
        /// Analyzes both visual and logical tree in order to find all elements of a given
        ///     type that are descendants of the <paramref name="source" /> item.
        /// </summary>
        /// <typeparam name="T">The type of the queried items.</typeparam>
        /// <param name="source">The source<see cref="DependencyObject"/></param>
        /// <param name="forceUsingTheVisualTreeHelper">Sometimes it's better to search in the VisualTree (e.g. in tests)</param>
        /// <returns>All descendants of <paramref name="source" /> that match the requested type.</returns>
        public static IEnumerable<T> FindChildren<T>(DependencyObject source,
                bool forceUsingTheVisualTreeHelper = false) where T : DependencyObject
        {
            if (source != null)
            {
                var childs = GetChildObjects(source, forceUsingTheVisualTreeHelper);
                foreach (var child in childs)
                {
                    //analyze if children match the requested type
                    if (child != null && child is T) yield return (T) child;

                    //recurse tree
                    foreach (var descendant in FindChildren<T>(child)) yield return descendant;
                }
            }
        }

        /// <summary>
        /// 根据控件名称，查找子控件集合
        ///     elementName为空时，查找指定类型的所有子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj<see cref="DependencyObject"/></param>
        /// <param name="elementName">The elementName<see cref="string"/></param>
        /// <returns>The <see cref="List{T}"/></returns>
        public static List<T> FindChildsByName<T>(this DependencyObject obj, string elementName)
                where T : FrameworkElement
        {
            DependencyObject child = null;
            var childList = new List<T>();
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && ((T) child).Name == elementName || string.IsNullOrEmpty(elementName))
                {
                    childList.Add((T) child);
                }
                else
                {
                    var grandChildList = FindChildsByName<T>(child, elementName);
                    if (grandChildList != null) childList.AddRange(grandChildList);
                }
            }

            return childList;
        }

        /// <summary>
        /// This will search for a parent of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="startingObject">The node where the search begins. This element is not checked.</param>
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
        public static T FindParent<T>(DependencyObject startingObject, bool checkStartingObject)
                where T : DependencyObject
        {
            return FindParent<T>(startingObject, checkStartingObject, null);
        }

        /// <summary>
        /// This will search for a parent of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="startingObject">The node where the search begins.</param>
        /// <param name="checkStartingObject">Should the specified startingObject be checked first.</param>
        /// <param name="additionalCheck">The additionalCheck<see cref="Func{T, bool}"/></param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        public static T FindParent<T>(DependencyObject startingObject, bool checkStartingObject,
                Func<T, bool> additionalCheck) where T : DependencyObject
        {
            T foundElement;
            var parent = checkStartingObject ? startingObject : GetParent(startingObject, true);

            while (parent != null)
            {
                foundElement = parent as T;

                if (foundElement != null)
                    if (additionalCheck == null)
                    {
                        return foundElement;
                    }
                    else
                    {
                        if (additionalCheck(foundElement))
                            return foundElement;
                    }

                parent = GetParent(parent, true);
            }

            return null;
        }

        /// <summary>
        /// 根据控件名称，查找父控件
        ///     elementName为空时，查找指定类型的父控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj<see cref="DependencyObject"/></param>
        /// <param name="elementName">The elementName<see cref="string"/></param>
        /// <returns>The <see cref="T"/></returns>
        public static T FindParentByName<T>(this DependencyObject obj, string elementName)
                where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T && (((T) parent).Name == elementName || string.IsNullOrEmpty(elementName)))
                    return (T) parent;
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
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    var child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T) return (T) child;

                    var childOfChild = FindVisualChildItem<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
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
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    var child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T && (child as T).Name.Equals(name)) return (T) child;

                    var childOfChild = FindVisualChildItem<T>(child, name);
                    if (childOfChild != null && childOfChild is T && childOfChild.Name.Equals(name))
                        return childOfChild;
                }

            return null;
        }

        /// <summary>
        /// This method is an alternative to WPF's
        ///     <see cref="VisualTreeHelper.GetChild" /> method, which also
        ///     supports content elements. Keep in mind that for content elements,
        ///     this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="parent">The item to be processed.</param>
        /// <param name="forceUsingTheVisualTreeHelper">Sometimes it's better to search in the VisualTree (e.g. in tests)</param>
        /// <returns>The submitted item's child elements, if available.</returns>
        public static IEnumerable<DependencyObject> GetChildObjects(DependencyObject parent,
                bool forceUsingTheVisualTreeHelper = false)
        {
            if (parent == null) yield break;

            if (!forceUsingTheVisualTreeHelper && (parent is ContentElement || parent is FrameworkElement))
            {
                //use the logical tree for content / framework elements
                foreach (var obj in LogicalTreeHelper.GetChildren(parent))
                {
                    var depObj = obj as DependencyObject;
                    if (depObj != null) yield return (DependencyObject) obj;
                }
            }
            else
            {
                //use the visual tree per default
                var count = VisualTreeHelper.GetChildrenCount(parent);
                for (var i = 0; i < count; i++) yield return VisualTreeHelper.GetChild(parent, i);
            }
        }

        /// <summary>
        /// Tries its best to return the specified element's parent. It will
        ///     try to find, in this order, the VisualParent, LogicalParent, LogicalTemplatedParent.
        ///     It only works for Visual, FrameworkElement or FrameworkContentElement.
        /// </summary>
        /// <param name="element">The element<see cref="DependencyObject"/></param>
        /// <returns>The <see cref="DependencyObject"/></returns>
        public static DependencyObject GetParent(DependencyObject element)
        {
            return GetParent(element, true);
        }

        /// <summary>
        /// This method is an alternative to WPF's
        ///     <see cref="VisualTreeHelper.GetParent" /> method, which also
        ///     supports content elements. Keep in mind that for content element,
        ///     this method falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The <see cref="DependencyObject"/></returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            var contentElement = child as ContentElement;
            if (contentElement != null)
            {
                var parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                var fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            var frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                var parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        /// <summary>
        /// Returns true if the specified element is a child of parent somewhere in the visual
        ///     tree. This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="element">The element that is potentially a child of the specified parent.</param>
        /// <param name="parent">The element that is potentially a parent of the specified element.</param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool IsDescendantOf(DependencyObject element, DependencyObject parent)
        {
            return IsDescendantOf(element, parent, true);
        }

        /// <summary>
        /// Returns true if the specified element is a child of parent somewhere in the visual
        ///     tree. This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="element">The element that is potentially a child of the specified parent.</param>
        /// <param name="parent">The element that is potentially a parent of the specified element.</param>
        /// <param name="recurseIntoPopup">The recurseIntoPopup<see cref="bool"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool IsDescendantOf(DependencyObject element, DependencyObject parent, bool recurseIntoPopup)
        {
            while (element != null)
            {
                if (element == parent)
                    return true;

                element = GetParent(element, recurseIntoPopup);
            }

            return false;
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">The child<see cref="DependencyObject"/></param>
        /// <returns>The <see cref="T"/></returns>
        public static T TryFindParent<T>(this DependencyObject child)
                where T : DependencyObject
        {
            //get parent item
            var parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            var parent = parentObject as T;
            return parent ?? TryFindParent<T>(parentObject);
        }

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
                var popup = element as Popup;

                if (popup != null && popup.PlacementTarget != null)
                    return popup.PlacementTarget;
            }

            var visual = element as Visual;
            var parent = visual == null ? null : VisualTreeHelper.GetParent(visual);

            if (parent == null)
            {
                // No Visual parent. Check in the logical tree.
                var fe = element as FrameworkElement;

                if (fe != null)
                {
                    parent = fe.Parent;

                    if (parent == null) parent = fe.TemplatedParent;
                }
                else
                {
                    var fce = element as FrameworkContentElement;

                    if (fce != null)
                    {
                        parent = fce.Parent;

                        if (parent == null) parent = fce.TemplatedParent;
                    }
                }
            }

            return parent;
        }

        #endregion
    }
}