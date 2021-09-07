/**************************************************************************
*      File Name：BulkObservableCollection.cs
*    Description：BulkObservableCollection.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Frontier.Wif.Core.Collections
{
    /// <summary>
    /// Bulk add and remove ObservableCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BulkObservableCollection<T> : ObservableCollection<T>
    {
        #region Fields

        /// <summary>
        /// Defines the _isNotificationSuspended
        /// </summary>
        private bool _isNotificationSuspended;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkObservableCollection{T}"/> class.
        /// </summary>
        public BulkObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="collection"></param>
        public BulkObservableCollection(IEnumerable<T> collection)
        {
            AddRange(collection);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the elements of the specified collection to the end of the
        ///     <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;" />.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            _isNotificationSuspended = true;
            var startIndex = Count;

            try
            {
                var items = Items;

                if (items != null)
                    using (var enumerator = collection.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                            items.Add(enumerator.Current);
                    }
            }
            finally
            {
                _isNotificationSuspended = false;

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the
        ///     <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;" />.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="clearExistingData">if set to <c>true</c> [clear existing data].</param>
        public void AddRange(IEnumerable<T> collection, bool clearExistingData)
        {
            if (clearExistingData)
            {
                _isNotificationSuspended = true;
                Clear();
            }

            AddRange(collection);
        }

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source List(Of T).
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public List<T> GetRange(int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (count < 0)
                throw new ArgumentOutOfRangeException("index");
            if (Count - index < count)
                throw new ArgumentException("Invalid Offset Length");

            return new List<T>(Items.Skip(index).Take(count));
        }

        /// <summary>
        /// Inserts the elements of the specified collection to the specified index of the
        ///     <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;" />.
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="collection">The collection.</param>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (index < 0 || index > Count)
                throw new IndexOutOfRangeException();

            _isNotificationSuspended = true;
            var startIndex = index;

            try
            {
                var items = Items;

                if (items != null)
                    using (var enumerator = collection.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            items.Insert(startIndex, enumerator.Current);
                            startIndex++;
                        }
                    }
            }
            finally
            {
                _isNotificationSuspended = false;

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Removes a range of elements specified in the collection from the
        ///     <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;" />.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public void RemoveRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            _isNotificationSuspended = true;

            try
            {
                RemoveItemsFromCollection(collection);
            }
            finally
            {
                _isNotificationSuspended = false;

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Resumes the notifications.
        /// </summary>
        public void ResumeNotifications()
        {
            _isNotificationSuspended = false;
        }

        /// <summary>
        /// Suspends the notifications.
        /// </summary>
        public void SuspendNotifications()
        {
            _isNotificationSuspended = true;
        }

        /// <summary>
        /// Raises the <see cref="E:CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">The e<see cref="NotifyCollectionChangedEventArgs"/></param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_isNotificationSuspended)
                base.OnCollectionChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs" /> instance containing the event data.</param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!_isNotificationSuspended)
                base.OnPropertyChanged(e);
        }

        /// <summary>
        /// The RemoveItemsFromCollection
        /// </summary>
        /// <param name="collection">The collection<see cref="IEnumerable{T}"/></param>
        private void RemoveItemsFromCollection(IEnumerable<T> collection)
        {
            var items = Items;
            if (items != null)
                using (IEnumerator<T> enumerator = new List<T>(collection).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                        items.Remove(enumerator.Current);
                }
        }

        #endregion
    }
}