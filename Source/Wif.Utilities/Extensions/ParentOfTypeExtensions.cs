/**************************************************************************
*      File Name：ParentOfTypeExtensions.cs
*    Description：ParentOfTypeExtensions.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Frontier.Wif.Utilities.Extensions
{
	/// <summary>
	/// Contains extension methods for enumerating the parents of an element.
	/// </summary>
	public static class ParentOfTypeExtensions
	{
		/// <summary>
		/// Gets the parent element from the visual tree by given type.
		/// </summary>
		public static T ParentOfType<T>(this DependencyObject element) where T : DependencyObject
		{
            return element?.GetParents().OfType<T>().FirstOrDefault();
		}

		/// <summary>
		/// Searches up in the visual tree for parent element of the specified type.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the parent that will be searched up in the visual object hierarchy. 
		/// The type should be <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <param name="element">The target <see cref="DependencyObject"/> which visual parents will be traversed.</param>
		/// <returns>Visual parent of the specified type if there is any, otherwise null.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static T GetVisualParent<T>(this DependencyObject element) where T : DependencyObject
		{
			return element.ParentOfType<T>();
		}

		/// <summary>  
		/// This recurse the visual tree for ancestors of a specific type.
		/// </summary>  
		internal static IEnumerable<T> GetAncestors<T>(this DependencyObject element) where T : class
		{
			return element.GetParents().OfType<T>();
		}

		/// <summary>  
		/// This recurse the visual tree for a parent of a specific type.
		/// </summary>  
		internal static T GetParent<T>(this DependencyObject element) where T : FrameworkElement
		{
			return element.ParentOfType<T>();
		}

		/// <summary>
		/// Enumerates through element's parents in the visual tree.
		/// </summary>
		public static IEnumerable<DependencyObject> GetParents(this DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			while ((element = element.GetParent()) != null)
			{
				yield return element;
			}
		}

		private static DependencyObject GetParent(this DependencyObject element)
		{
			DependencyObject parent = null;
			try
			{
                parent = VisualTreeHelper.GetParent(element);
			}
			catch (InvalidOperationException)
			{
				parent = null;
			}
			if (parent == null)
			{
                if (element is FrameworkElement frameworkElement)
				{
					parent = frameworkElement.Parent;
				}
            }
			return parent;
		}
	}
}