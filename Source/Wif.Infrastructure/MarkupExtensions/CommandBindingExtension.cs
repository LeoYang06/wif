/**************************************************************************
*      File Name：CommandBindingExtension.cs
*    Description：CommandBindingExtension.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// Defines the <see cref="CommandBindingExtension" />
    ///     https://www.thomaslevesque.com/2009/03/17/wpf-using-inputbindings-with-the-mvvm-pattern/
    /// </summary>
    [MarkupExtensionReturnType(typeof(ICommand))]
    public class CommandBindingExtension : MarkupExtension
    {
        #region Fields

        /// <summary>
        /// Defines the dataContextChangeHandlerSet
        /// </summary>
        private bool _dataContextChangeHandlerSet;

        /// <summary>
        /// Defines the targetObject
        /// </summary>
        private object _targetObject;

        /// <summary>
        /// Defines the targetProperty
        /// </summary>
        private object _targetProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CommandName
        /// Gets or sets the CommandName
        /// </summary>
        [ConstructorArgument("commandName")]
        public string CommandName { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBindingExtension"/> class.
        /// </summary>
        public CommandBindingExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBindingExtension"/> class.
        /// </summary>
        /// <param name="commandName">The <see cref="string" /></param>
        public CommandBindingExtension(string commandName)
        {
            CommandName = commandName;
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
            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget != null)
            {
                _targetObject = provideValueTarget.TargetObject;
                _targetProperty = provideValueTarget.TargetProperty;
            }

            if (!string.IsNullOrEmpty(CommandName))
            {
                // The serviceProvider is actually a ProvideValueServiceProvider, which has a private field "_context" of type ParserContext
                var parserContext = GetPrivateFieldValue<ParserContext>(serviceProvider, "_context");
                if (parserContext != null)
                {
                    // A ParserContext has a private field "_rootElement", which returns the root element of the XAML file
                    var rootElement = GetPrivateFieldValue<FrameworkElement>(parserContext, "_rootElement");
                    if (rootElement != null)
                    {
                        // Now we can retrieve the DataContext
                        var dataContext = rootElement.DataContext;

                        // The DataContext may not be set yet when the FrameworkElement is first created, and it may change afterwards,
                        // so we handle the DataContextChanged event to update the Command when needed
                        if (!_dataContextChangeHandlerSet)
                        {
                            rootElement.DataContextChanged += rootElement_DataContextChanged;
                            _dataContextChangeHandlerSet = true;
                        }

                        if (dataContext != null)
                        {
                            var command = GetCommand(dataContext, CommandName);
                            if (command != null)
                                return command;
                        }
                    }
                }
            }

            // The Command property of an InputBinding cannot be null, so we return a dummy extension instead
            return DummyCommand.Instance;
        }

        /// <summary>
        /// The AssignCommand
        /// </summary>
        /// <param name="command">The <see cref="ICommand" /></param>
        private void AssignCommand(ICommand command)
        {
            if (_targetObject != null && _targetProperty != null)
                if (_targetProperty is DependencyProperty)
                {
                    var depObj = _targetObject as DependencyObject;
                    var depProp = _targetProperty as DependencyProperty;
                    depObj.SetValue(depProp, command);
                }
                else
                {
                    var prop = _targetProperty as PropertyInfo;
                    prop.SetValue(_targetObject, command, null);
                }
        }

        /// <summary>
        /// The GetCommand
        /// </summary>
        /// <param name="dataContext">The <see cref="object" /></param>
        /// <param name="commandName">The <see cref="string" /></param>
        /// <returns>The <see cref="ICommand" /></returns>
        private ICommand GetCommand(object dataContext, string commandName)
        {
            var prop = dataContext.GetType().GetProperty(commandName);
            if (prop != null)
            {
                var command = prop.GetValue(dataContext, null) as ICommand;
                if (command != null)
                    return command;
            }

            return null;
        }

        /// <summary>
        /// The GetPrivateFieldValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The <see cref="object" /></param>
        /// <param name="fieldName">The <see cref="string" /></param>
        /// <returns>The <see cref="T" /></returns>
        private T GetPrivateFieldValue<T>(object target, string fieldName)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null) return (T) field.GetValue(target);
            return default;
        }

        /// <summary>
        /// The rootElement_DataContextChanged
        /// </summary>
        /// <param name="sender">The <see cref="object" /></param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /></param>
        private void rootElement_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var rootElement = sender as FrameworkElement;
            if (rootElement != null)
            {
                var dataContext = rootElement.DataContext;
                if (dataContext != null)
                {
                    var command = GetCommand(dataContext, CommandName);
                    if (command != null) AssignCommand(command);
                }
            }
        }

        #endregion

        /// <summary>
        /// Defines the <see cref="DummyCommand" />
        /// </summary>
        private class DummyCommand : ICommand
        {
            #region Fields

            /// <summary>
            /// Defines the _instance
            /// </summary>
            private static DummyCommand _instance;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the Instance
            /// Gets the Instance
            /// </summary>
            public static DummyCommand Instance
            {
                get
                {
                    if (_instance == null) _instance = new DummyCommand();
                    return _instance;
                }
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Prevents a default instance of the <see cref="DummyCommand"/> class from being created.
            /// </summary>
            private DummyCommand()
            {
            }

            #endregion

            #region Events

            /// <summary>
            /// Defines the CanExecuteChanged
            /// </summary>
            public event EventHandler CanExecuteChanged;

            #endregion

            #region Methods

            /// <summary>
            /// The CanExecute
            /// </summary>
            /// <param name="parameter">The <see cref="object" /></param>
            /// <returns>The <see cref="bool" /></returns>
            public bool CanExecute(object parameter)
            {
                return false;
            }

            /// <summary>
            /// The Execute
            /// </summary>
            /// <param name="parameter">The <see cref="object" /></param>
            public void Execute(object parameter)
            {
            }

            #endregion
        }
    }
}