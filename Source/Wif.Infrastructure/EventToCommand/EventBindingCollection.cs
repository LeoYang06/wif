/**************************************************************************
*      File Name：EventBindingCollection.cs
*    Description：EventBindingCollection.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace Frontier.Wif.Infrastructure.EventToCommand
{
    /// <summary>
    /// Represents an ordered collection of <see cref="EventBinding" /> objects.
    /// </summary>
    public class EventBindingCollection : FreezableCollection<EventBinding>
    {
        #region Fields

        /// <summary>
        /// Defines the eventBindingsCopy
        /// </summary>
        private readonly List<EventBinding> _eventBindingsCopy;

        /// <summary>
        /// Defines the owner
        /// </summary>
        private UIElement _owner;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBindingCollection"/> class.
        /// </summary>
        public EventBindingCollection()
        {
            _eventBindingsCopy = new List<EventBinding>();
            ((INotifyCollectionChanged) this).CollectionChanged += OnEventBindingCollectionChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The SetOwner
        /// </summary>
        /// <param name="sourceElement">The <see cref="UIElement" /></param>
        internal void SetOwner(UIElement sourceElement)
        {
            _owner = sourceElement;
            foreach (var binding in _eventBindingsCopy) binding.SetSource(sourceElement);
        }

        /// <summary>
        /// Creates new instance of <see cref="EventBindingCollection" />.
        /// </summary>
        /// <returns>New instance of <see cref="EventBindingCollection" />.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new EventBindingCollection();
        }

        /// <summary>
        /// The AddInputBinding
        /// </summary>
        /// <param name="eventBinding">The <see cref="EventBinding" /></param>
        private void AddInputBinding(EventBinding eventBinding)
        {
            if (eventBinding != null)
            {
                _eventBindingsCopy.Add(eventBinding);
                eventBinding.SetSource(_owner);
            }
        }

        /// <summary>
        /// The OnEventBindingCollectionChanged
        /// </summary>
        /// <param name="sender">The <see cref="object" /></param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /></param>
        private void OnEventBindingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                        foreach (EventBinding item in e.NewItems)
                            AddInputBinding(item);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                        foreach (EventBinding item in e.OldItems)
                            RemoveInputBinding(item);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    RemoveInputBinding(e.OldItems[0] as EventBinding);
                    AddInputBinding(e.NewItems[0] as EventBinding);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in _eventBindingsCopy) RemoveInputBinding(item);
                    _eventBindingsCopy.Clear();
                    break;
            }
        }

        /// <summary>
        /// The RemoveInputBinding
        /// </summary>
        /// <param name="eventBinding">The <see cref="EventBinding" /></param>
        private void RemoveInputBinding(EventBinding eventBinding)
        {
            if (eventBinding != null)
            {
                _eventBindingsCopy.Remove(eventBinding);
                eventBinding.SetSource(null);
            }
        }

        #endregion
    }
}