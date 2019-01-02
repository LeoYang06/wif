using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// Defines the <see cref="EventArgsExtension" />
    /// </summary>
    public class EventArgsExtension : MarkupExtension
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Converter
        /// Gets or sets the Converter
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the ConverterCulture
        /// Gets or sets the ConverterCulture
        /// </summary>
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture { get; set; }

        /// <summary>
        /// Gets or sets the ConverterParameter
        /// Gets or sets the ConverterParameter
        /// </summary>
        public object ConverterParameter { get; set; }

        /// <summary>
        /// Gets or sets the ConverterTargetType
        /// Gets or sets the ConverterTargetType
        /// </summary>
        public Type ConverterTargetType { get; set; }

        /// <summary>
        /// Gets or sets the Path
        /// Gets or sets the Path
        /// </summary>
        public PropertyPath Path { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgsExtension"/> class.
        /// </summary>
        public EventArgsExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgsExtension"/> class.
        /// </summary>
        /// <param name="path">The <see cref="string" /></param>
        public EventArgsExtension(string path)
        {
            Path = new PropertyPath(path);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /></param>
        /// <returns>The <see cref="object" /></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <summary>
        /// The GetArgumentValue
        /// </summary>
        /// <param name="eventArgs">The <see cref="EventArgs" /></param>
        /// <param name="language">The <see cref="XmlLanguage" /></param>
        /// <returns>The <see cref="object" /></returns>
        internal object GetArgumentValue(EventArgs eventArgs, XmlLanguage language)
        {
            if (Path == null)
                return eventArgs;

            var value = PropertyPathHelpers.Evaluate(Path, eventArgs);

            if (Converter != null)
                value = Converter.Convert(value, ConverterTargetType ?? typeof(object), ConverterParameter,
                        ConverterCulture ?? language.GetSpecificCulture());

            return value;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="EventSenderExtension" />
    /// </summary>
    public class EventSenderExtension : MarkupExtension
    {
        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /></param>
        /// <returns>The <see cref="object" /></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion
    }

    /// <summary>
    /// http://www.singulink.com/CodeIndex/post/building-the-ultimate-wpf-event-method-binding-extension
    ///     http://www.singulink.com/CodeIndex/post/updated-ultimate-wpf-event-method-binding
    /// </summary>
    public class MethodBindingExtension : MarkupExtension
    {
        #region Fields

        /// <summary>
        /// Defines the _storageProperties
        /// </summary>
        private static readonly List<DependencyProperty> _storageProperties = new List<DependencyProperty>();

        /// <summary>
        /// Defines the _argumentProperties
        /// </summary>
        private readonly List<DependencyProperty> _argumentProperties = new List<DependencyProperty>();

        /// <summary>
        /// Defines the _arguments
        /// </summary>
        private readonly object[] _arguments;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBindingExtension"/> class.
        /// </summary>
        /// <param name="arg0">The <see cref="object" /></param>
        /// <param name="arg1">The <see cref="object" /></param>
        public MethodBindingExtension(object arg0, object arg1) : this(new[] {arg0, arg1})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBindingExtension"/> class.
        /// </summary>
        /// <param name="arg0">The <see cref="object" /></param>
        /// <param name="arg1">The <see cref="object" /></param>
        /// <param name="arg2">The <see cref="object" /></param>
        public MethodBindingExtension(object arg0, object arg1, object arg2) : this(new[] {arg0, arg1, arg2})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBindingExtension"/> class.
        /// </summary>
        /// <param name="arg0">The <see cref="object" /></param>
        /// <param name="arg1">The <see cref="object" /></param>
        /// <param name="arg2">The <see cref="object" /></param>
        /// <param name="arg3">The <see cref="object" /></param>
        public MethodBindingExtension(object arg0, object arg1, object arg2, object arg3) : this(new[]
                {arg0, arg1, arg2, arg3})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBindingExtension"/> class.
        /// </summary>
        /// <param name="arg0">The <see cref="object" /></param>
        /// <param name="arg1">The <see cref="object" /></param>
        /// <param name="arg2">The <see cref="object" /></param>
        /// <param name="arg3">The <see cref="object" /></param>
        /// <param name="arg4">The <see cref="object" /></param>
        public MethodBindingExtension(object arg0, object arg1, object arg2, object arg3, object arg4) : this(new[]
                {arg0, arg1, arg2, arg3, arg4})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBindingExtension"/> class.
        /// </summary>
        /// <param name="arg0">The <see cref="object" /></param>
        /// <param name="arg1">The <see cref="object" /></param>
        /// <param name="arg2">The <see cref="object" /></param>
        /// <param name="arg3">The <see cref="object" /></param>
        /// <param name="arg4">The <see cref="object" /></param>
        /// <param name="arg5">The <see cref="object" /></param>
        public MethodBindingExtension(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5) :
                this(new[] {arg0, arg1, arg2, arg3, arg4, arg5})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBindingExtension"/> class.
        /// </summary>
        /// <param name="arg0">The <see cref="object" /></param>
        /// <param name="arg1">The <see cref="object" /></param>
        /// <param name="arg2">The <see cref="object" /></param>
        /// <param name="arg3">The <see cref="object" /></param>
        /// <param name="arg4">The <see cref="object" /></param>
        /// <param name="arg5">The <see cref="object" /></param>
        /// <param name="arg6">The <see cref="object" /></param>
        public MethodBindingExtension(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5,
                object arg6) : this(new[] {arg0, arg1, arg2, arg3, arg4, arg5, arg6})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBindingExtension"/> class.
        /// </summary>
        /// <param name="arg0">The <see cref="object" /></param>
        /// <param name="arg1">The <see cref="object" /></param>
        /// <param name="arg2">The <see cref="object" /></param>
        /// <param name="arg3">The <see cref="object" /></param>
        /// <param name="arg4">The <see cref="object" /></param>
        /// <param name="arg5">The <see cref="object" /></param>
        /// <param name="arg6">The <see cref="object" /></param>
        /// <param name="arg7">The <see cref="object" /></param>
        public MethodBindingExtension(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5,
                object arg6, object arg7) : this(new[] {arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBindingExtension"/> class.
        /// </summary>
        /// <param name="arg0">The <see cref="object" /></param>
        /// <param name="arg1">The <see cref="object" /></param>
        /// <param name="arg2">The <see cref="object" /></param>
        /// <param name="arg3">The <see cref="object" /></param>
        /// <param name="arg4">The <see cref="object" /></param>
        /// <param name="arg5">The <see cref="object" /></param>
        /// <param name="arg6">The <see cref="object" /></param>
        /// <param name="arg7">The <see cref="object" /></param>
        /// <param name="arg8">The <see cref="object" /></param>
        public MethodBindingExtension(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5,
                object arg6, object arg7, object arg8) : this(new[]
                {arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBindingExtension"/> class.
        /// </summary>
        /// <param name="method">The <see cref="object" /></param>
        public MethodBindingExtension(object method) : this(new[] {method})
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="MethodBindingExtension"/> class from being created.
        /// </summary>
        /// <param name="arguments">The <see cref="object[]" /></param>
        private MethodBindingExtension(object[] arguments)
        {
            _arguments = arguments;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /></param>
        /// <returns>The <see cref="object" /></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));
            var target = provideValueTarget.TargetObject as FrameworkElement;
            Type eventHandlerType = null;

            if (provideValueTarget.TargetProperty is EventInfo eventInfo)
            {
                eventHandlerType = eventInfo.EventHandlerType;
            }
            else if (provideValueTarget.TargetProperty is MethodInfo methodInfo)
            {
                var parameters = methodInfo.GetParameters();

                if (parameters.Length == 2)
                    eventHandlerType = parameters[1].ParameterType;
            }

            if (target == null || eventHandlerType == null)
                return this;

            foreach (var argument in _arguments)
            {
                var argumentProperty = SetUnusedStorageProperty(target, argument);
                _argumentProperties.Add(argumentProperty);
            }

            return CreateEventHandler(target, eventHandlerType);
        }

        /// <summary>
        /// The CreateEventHandler
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement" /></param>
        /// <param name="eventHandlerType">The <see cref="Type" /></param>
        /// <returns>The <see cref="Delegate" /></returns>
        private Delegate CreateEventHandler(FrameworkElement element, Type eventHandlerType)
        {
            EventHandler handler = (sender, eventArgs) =>
            {
                var arg0 = element.GetValue(_argumentProperties[0]);

                if (arg0 == null)
                {
                    Debug.WriteLine(
                            "[MethodBinding] First method binding argument is required and cannot resolve to null - method name or method target expected.");
                    return;
                }

                int methodArgsStart;
                object methodTarget;

                // If the first argument is a string then it must be the name of the method to invoke on the data context.
                // If not then it is the excplicit method target object and the second argument will be name of the method to invoke.

                if (arg0 is string methodName)
                {
                    methodTarget = element.DataContext;
                    methodArgsStart = 1;
                }
                else if (_argumentProperties.Count >= 2)
                {
                    methodTarget = arg0;
                    methodArgsStart = 2;

                    var arg1 = element.GetValue(_argumentProperties[1]);

                    if (arg1 == null)
                    {
                        Debug.WriteLine(
                                $"[MethodBinding] First argument resolved as a method target object of type '{methodTarget.GetType()}', second argument must resolve to a method name and cannot resolve to null.");
                        return;
                    }

                    methodName = arg1 as string;

                    if (methodName == null)
                    {
                        Debug.WriteLine(
                                $"[MethodBinding] First argument resolved as a method target object of type '{methodTarget.GetType()}', second argument (method name) must resolve to a '{typeof(string)}' (actual type: '{arg1.GetType()}').");
                        return;
                    }
                }
                else
                {
                    Debug.WriteLine(
                            $"[MethodBinding] Method name must resolve to a '{typeof(string)}' (actual type: '{arg0.GetType()}').");
                    return;
                }

                var arguments = new object[_argumentProperties.Count - methodArgsStart];

                for (var i = methodArgsStart; i < _argumentProperties.Count; i++)
                {
                    var argValue = element.GetValue(_argumentProperties[i]);

                    if (argValue is EventSenderExtension)
                        argValue = sender;
                    else if (argValue is EventArgsExtension eventArgsEx)
                        argValue = eventArgsEx.GetArgumentValue(eventArgs, element.Language);

                    arguments[i - methodArgsStart] = argValue;
                }

                var methodTargetType = methodTarget.GetType();

                // Try invoking the method by resolving it based on the arguments provided

                try
                {
                    methodTargetType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, methodTarget, arguments);
                    return;
                }
                catch (MissingMethodException)
                {
                }

                // Couldn't match a method with the raw arguments, so check if we can find a method with the same name
                // and parameter count and try to convert any XAML string arguments to match the method parameter types

                var method = methodTargetType.GetMethods().SingleOrDefault(m =>
                        m.Name == methodName && m.GetParameters().Length == arguments.Length);

                if (method != null)
                {
                    var parameters = method.GetParameters();

                    for (var i = 0; i < _arguments.Length; i++)
                        if (arguments[i] == null)
                        {
                            if (parameters[i].ParameterType.IsValueType)
                            {
                                method = null;
                                break;
                            }
                        }
                        else if (_arguments[i] is string && parameters[i].ParameterType != typeof(string))
                        {
                            // The original value provided for this argument was a XAML string so try to convert it
                            arguments[i] = TypeDescriptor.GetConverter(parameters[i].ParameterType)
                                    .ConvertFromString((string) _arguments[i]);
                        }
                        else if (!parameters[i].ParameterType.IsInstanceOfType(arguments[i]))
                        {
                            method = null;
                            break;
                        }

                    method?.Invoke(methodTarget, arguments);
                }

                if (method == null)
                    Debug.WriteLine(
                            $"[MethodBinding] Could not find a method '{methodName}' on target type '{methodTargetType}' that accepts the parameters provided.");
            };

            return Delegate.CreateDelegate(eventHandlerType, handler.Target, handler.Method);
        }

        /// <summary>
        /// The SetUnusedStorageProperty
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject" /></param>
        /// <param name="value">The <see cref="object" /></param>
        /// <returns>The <see cref="DependencyProperty" /></returns>
        private DependencyProperty SetUnusedStorageProperty(DependencyObject obj, object value)
        {
            var property =
                    _storageProperties.FirstOrDefault(p => obj.ReadLocalValue(p) == DependencyProperty.UnsetValue);

            if (property == null)
            {
                property = DependencyProperty.RegisterAttached("Storage" + _storageProperties.Count, typeof(object),
                        typeof(MethodBindingExtension), new PropertyMetadata());
                _storageProperties.Add(property);
            }

            var markupExtension = value as MarkupExtension;

            if (markupExtension != null)
            {
                var resolvedValue = markupExtension.ProvideValue(new ServiceProvider(obj, property));
                obj.SetValue(property, resolvedValue);
            }
            else
            {
                obj.SetValue(property, value);
            }

            return property;
        }

        #endregion

        /// <summary>
        /// Defines the <see cref="ServiceProvider" />
        /// </summary>
        private class ServiceProvider : IServiceProvider, IProvideValueTarget
        {
            #region Properties

            /// <summary>
            /// Gets the TargetObject
            /// Gets the TargetObject
            /// </summary>
            public object TargetObject { get; }

            /// <summary>
            /// Gets the TargetProperty
            /// Gets the TargetProperty
            /// </summary>
            public object TargetProperty { get; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ServiceProvider"/> class.
            /// </summary>
            /// <param name="targetObject">The <see cref="object" /></param>
            /// <param name="targetProperty">The <see cref="object" /></param>
            public ServiceProvider(object targetObject, object targetProperty)
            {
                TargetObject = targetObject;
                TargetProperty = targetProperty;
            }

            #endregion

            #region Methods

            /// <summary>
            /// The GetService
            /// </summary>
            /// <param name="serviceType">The <see cref="Type" /></param>
            /// <returns>The <see cref="object" /></returns>
            public object GetService(Type serviceType)
            {
                return serviceType.IsInstanceOfType(this) ? this : null;
            }

            #endregion
        }
    }

    /// <summary>
    /// Defines the <see cref="PropertyPathHelpers" />
    /// </summary>
    public static class PropertyPathHelpers
    {
        #region Methods

        /// <summary>
        /// The Evaluate
        /// </summary>
        /// <param name="path">The <see cref="PropertyPath" /></param>
        /// <param name="source">The <see cref="object" /></param>
        /// <returns>The <see cref="object" /></returns>
        public static object Evaluate(PropertyPath path, object source)
        {
            var target = new DependencyTarget();
            var binding = new System.Windows.Data.Binding {Path = path, Source = source, Mode = BindingMode.OneTime};
            BindingOperations.SetBinding(target, DependencyTarget.ValueProperty, binding);

            return target.Value;
        }

        #endregion

        /// <summary>
        /// Defines the <see cref="DependencyTarget" />
        /// </summary>
        private class DependencyTarget : DependencyObject
        {
            #region Fields

            /// <summary>
            /// Defines the ValueProperty
            /// </summary>
            public static readonly DependencyProperty ValueProperty =
                    DependencyProperty.Register("Value", typeof(object), typeof(DependencyTarget));

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the Value
            /// Gets or sets the Value
            /// </summary>
            public object Value
            {
                get => GetValue(ValueProperty);
                set => SetValue(ValueProperty, value);
            }

            #endregion
        }
    }
}