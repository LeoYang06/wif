[TOC]

## 前言

在上一篇译文——《[深入理解C#——在C#中实现单例模式](https://www.cnblogs.com/leolion/p/10241822.html)》中，对在C#中实现单例模式进行了详细阐述。我们在日常的开发中可以采用解决方案4或解决方案6来实现单例模式，但每个单例类都需要单独实现。

我们再来看看使用单例模式的一些场景：

> 主要意图：保证一个类仅有一个实例，并提供一个访问它的全局访问点。
> 主要解决：一个全局使用的类频繁地创建与销毁。
> 何时使用：当您想控制实例数目，节省系统资源的时候。
> 使用场景： 
>
> 1. 要求生产唯一序列号；
> 2. WEB 中的计数器，不用每次刷新都在数据库里加一次，用单例先缓存起来；
> 3. 创建的一个对象需要消耗的资源过多，比如 I/O 与数据库的连接等；
> 4. 全局配置文件访问，单例类来保证数据唯一性；
> 5. 日志记录帮助类，为节省资源，全局一个实例一般就够了；
> 6. 桌面应用常常要求只能打开一个程序实例或一个窗口。

## 单例基类

可以看到单例模式在程序开发中是非常常见的。既然我们会频繁的使用单例模式，那么有没有什么方式可以更方便的生产我们的单例。当然有，我们往下看。

对于没有基类的一些类的单例模式实现，可以考虑继承自单例基类。由单例基类派生的类必须是密封类，它确保您不能从这个单例类创建子类。单例的生产就由基类来完成，派生类只需要定义一个无参数的私有构造函数即可，它确保不能在外部创建此类的实例。通过调用继承的实例属性访问类的单例实例和公共成员。

```C#
    /// <summary>
    /// 总括来说，为了使用单例基类创建单例类，您需要执行以下操作:
    /// 
    /// 1) 定义一个派生自SingletonBase [T]的密封类，其中T是您定义的类名。 它确保您不能从此单例类创建子类。
    /// 2) 在类中定义一个无参数的私有构造函数。它确保不能在外部创建此类的实例。
    /// 3) 通过调用Instance属性来访问类的单例实例和公共成员。
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonBase<T> where T : class
    {
        #region Properties

        /// <summary>
        /// 获取该类的单例实例。
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static T Instance => SingletonFactory.Instance;

        #endregion

        #region Constructors

        #endregion

        /// <summary>
        /// 创建单例实例的单例类工厂。
        /// </summary>
        private class SingletonFactory
        {
            #region Fields

            /// <summary>
            /// 定义弱引用实例。
            /// </summary>
            private static WeakReference _instance;

            #endregion

            #region Properties

            /// <summary>
            /// 获取实例。
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

            /// <summary>
            /// 防止编译器生成默认构造函数。
            /// </summary>
            private SingletonFactory()
            {
            }

            /// <summary>
            /// 显式静态构造函数，告诉c#编译器不要将类型标记为BeforeFieldInit。
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            static SingletonFactory()
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// 获取特定类型的实例。
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
```

在[SingletonBase<T>](https://github.com/LeoYang610/wif/blob/master/Source/Wif.Core/Generic/SingletonBase.cs)中我们用到了[WeakReference<T>](https://docs.microsoft.com/zh-cn/dotnet/api/system.weakreference?redirectedfrom=MSDN&view=netframework-4.7.2)，它表示弱引用，即在引用对象的同时仍然允许通过垃圾回收来回收该对象。一般使用场景：对象过大，并且不经常访问。这样我们就可以创建一个弱引用，当不常用该对象的时候，GC可以回收该对象，当需要引用对象，可以先判断弱引用的对象是不是存在，如果存在，就直接使用，如果弱引用的对象已经被回收，那就重新创建一个对象来使用。对于单例对象来说，一般生命周期和应用程序域同步，是无法自动回收的。通过使用[WeakReference<T>](https://docs.microsoft.com/zh-cn/dotnet/api/system.weakreference?redirectedfrom=MSDN&view=netframework-4.7.2)来确保单例对象在长时间未使用时可以自动释放资源。

当然这里的[WeakReference<T>](https://docs.microsoft.com/zh-cn/dotnet/api/system.weakreference?redirectedfrom=MSDN&view=netframework-4.7.2)也可以替换为[Lazy<T>](https://docs.microsoft.com/zh-cn/dotnet/api/system.lazy-1?redirectedfrom=MSDN&view=netframework-4.7.2)，根据是否需自动回收单例对象和子类是否包含属性和字段这两个方面来选择。如子类中包含属性和字段，则自动回收会导致属性和字段值重置，从而出现不可预估的后果。

## 单例提供者

对于有基类的类来说，上面的单例基类显然是不合适的。我们可以考虑实现一个单例提供者来生产我们的单例。

通过泛型类传参的方式实现如下：

```C#
    /// <summary>
    /// 用于从另一个类创建或获取单例的静态助手。
    /// </summary>
    /// <typeparam name="T">要创建或获取单例的类型。</typeparam>
    public static class SingletonProvider<T> where T : class, new()
    {
        #region Fields

        /// <summary>
        /// 获取给定类型的单例。
        /// </summary>
        private static readonly Lazy<T> _lazy = new Lazy<T>(() => new T());

        #endregion

        #region Properties

        /// <summary>
        /// 获取给定类型的单例。
        /// </summary>
        public static T Instance => _lazy.Value;

        #endregion
    }
```

除了泛型类传参，还可以通过Get方法传参，实现如下：

```C#
    /// <summary>
    /// 用于从另一个类创建或获取单例的静态助手。
    /// </summary>
    public static class SingletonProvider
    {
        #region Methods

        /// <summary>
        /// 获取指定类型的单例。
        /// </summary>
        /// <typeparam name="TParameter">单例类型。</typeparam>
        /// <returns>The <see cref="TParameter" />单例对象。</returns>
        public static TParameter Get<TParameter>() where TParameter : class, new()
        {
            return SingletonProvider<TParameter>.Instance;
        }

        #endregion
    }
```

## 总结

有了以上两种生产单例的方式，我们可以在开发中愉快的使用单例，而免除了具体的繁琐实现。



> **wif 项目代码：[https://github.com/LeoYang610/wif](https://github.com/LeoYang610/wif)**   