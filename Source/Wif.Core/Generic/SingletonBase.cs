using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace Frontier.Wif.Core.Generic
{
    /// <summary>
    /// To recap, in order to create a singleton class using the singleton base class, you need to do the following:
    /// 
    /// 1) Define a sealed class which derives from SingletonBase[T], where T is the class name you are defining. It ensures that you cannot create subclasses from this singleton class.
    /// 2) Define a single parameterless private constructor inside the class. It ensures that no instances of this class can be created externally.
    /// 3) Access the class’ singleton instance and public members by calling the Instance property.
    /// 
    /// got this implementation from http://liquidsilver.codeplex.com
    /// http://codebender.denniland.com/a-singleton-base-class-to-implement-the-singleton-pattern-in-c/
    /// 
    /// also show http://www.yoda.arachsys.com/csharp/singleton.html
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonBase<T> where T : class
    {
        #region Properties

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T Instance => SingletonFactory.Instance;

        #endregion

        #region Constructors

        #endregion

        /// <summary>
        /// The singleton class factory to create the singleton instance.
        /// </summary>
        private class SingletonFactory
        {
            #region Fields

            /// <summary>
            /// Defines the _instance
            /// </summary>
            private static WeakReference _instance;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the Instance
            /// </summary>
            internal static T Instance
            {
                get
                {
                    if (!(_instance?.Target is T comparer))
                    {
                        comparer = GetInstance();
                        _instance = new WeakReference(comparer);
                    }

                    return comparer;
                }
            }

            #endregion

            #region Constructors

            // Prevent the compiler from generating a default constructor.
            /// <summary>
            /// Prevents a default instance of the <see cref="SingletonFactory"/> class from being created.
            /// </summary>
            private SingletonFactory()
            {
            }

            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            /// <summary>
            /// Initializes static members of the <see cref="SingletonFactory"/> class.
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            static SingletonFactory()
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// The GetInstance
            /// </summary>
            /// <returns>The <see cref="T"/></returns>
            [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId =
                    "System.Type.InvokeMember")]
            private static T GetInstance()
            {
                var theType = typeof(T);

                T inst;

                try
                {
                    inst = (T) theType.InvokeMember(theType.Name,
                            BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic,
                            null, null, null,
                            CultureInfo.InvariantCulture);
                }
                catch (MissingMethodException ex)
                {
                    throw new TypeLoadException(
                            string.Format(CultureInfo.CurrentCulture,
                                    "The type '{0}' must have a private constructor to be used in the Singleton pattern.",
                                    theType.FullName), ex);
                }

                return inst;
            }

            #endregion
        }
    }
}