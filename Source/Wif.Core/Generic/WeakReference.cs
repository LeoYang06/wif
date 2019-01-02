using System;
using System.Runtime.Serialization;

namespace Frontier.Wif.Core.Generic
{
    /// <summary>
    /// Represents a weak reference, which references an object while still allowing   
    /// that object to be reclaimed by garbage collection.
    /// </summary>
    /// <typeparam name="T">The type of the object that is referenced.</typeparam>
    [Serializable]
    public class WeakReference<T> : WeakReference where T : class
    {
        #region Properties

        /// <summary>
        /// Gets or sets the object (the target) referenced by the current WeakReference{T} 
        /// object.
        /// </summary>
        public new T Target
        {
            get => (T) base.Target;
            set => base.Target = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class.
        /// </summary>
        /// <param name="target">The object to reference.</param>
        public WeakReference(T target) : base(target)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class.
        /// </summary>
        /// <param name="target">An object to track.</param>
        /// <param name="trackResurrection">The trackResurrection<see cref="bool"/></param>
        public WeakReference(T target, bool trackResurrection) : base(target, trackResurrection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class.
        /// </summary>
        /// <param name="info">The info<see cref="SerializationInfo"/></param>
        /// <param name="context">The context<see cref="StreamingContext"/></param>
        protected WeakReference(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion


        /// <summary> 
        /// Casts an object of the type T to a weak reference 
        /// of T. 
        /// </summary>
        public static implicit operator WeakReference<T>(T target)
        {
            if (target == null) throw new ArgumentNullException("target");
            return new WeakReference<T>(target);
        }

        /// <summary> 
        /// Casts a weak reference to an object of the type the 
        /// reference represents. 
        /// </summary>
        public static implicit operator T(WeakReference<T> reference)
        {
            if (reference == null) return default;
            return reference.Target;
        }
    }
}