using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Frontier.Wif.Core.ComponentModel;
using Frontier.Wif.Utilities.Extensions;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// Defines the <see cref="EventBindingExtension" />
    /// <remarks>https://github.com/JonghoL/EventBindingMarkup</remarks>
    /// <code>
    /// <Rectangle MouseDown="{wif:EventBinding MouseDownCommand}" /> 普通无参数绑定。
    /// <Rectangle MouseDown="{wif:EventBinding Command=MouseDownCommand, CommandParameter=blue}" /> 普通绑定。
    /// <Rectangle MouseDown="{wif:EventBinding Command=MouseDownCommand, CommandParameter=$this.Fill}" /> 绑定到当前控件的属性上。
    /// <Rectangle MouseDown="{wif:EventBinding Command=MouseDownCommand, CommandParameter=$e}" />  绑定到当前上下文上。
    /// </code>
    /// </summary>
    public class EventBindingExtension : MarkupExtension
    {
        /// <summary>
        /// Defines the getMethod
        /// </summary>
        internal static readonly MethodInfo getMethod = typeof(EventBindingExtension).GetMethod("HandlerIntern", new[] { typeof(object), typeof(object), typeof(string), typeof(string) });

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBindingExtension" /> class.
        /// </summary>
        public EventBindingExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBindingExtension" /> class.
        /// </summary>
        /// <param name="commandName">The commandName<see cref="string" /></param>
        public EventBindingExtension(string commandName) : this()
        {
            Command = commandName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBindingExtension" /> class.
        /// </summary>
        /// <param name="command">The command<see cref="string" /></param>
        /// <param name="commandParameter">The commandParameter<see cref="string" /></param>
        public EventBindingExtension(string command, string commandParameter) : this(command)
        {
            Command = command;
            CommandParameter = commandParameter;
        }

        /// <summary>
        /// Gets or sets the Command
        /// </summary>
        [ConstructorArgument("command")]
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the CommandParameter
        /// </summary>
        public string CommandParameter { get; set; }

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider" /></param>
        /// <returns>The <see cref="object" /></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (targetProvider == null)
                throw new InvalidOperationException();

            var targetObject = targetProvider.TargetObject as FrameworkElement;
            if (targetObject == null)
                throw new InvalidOperationException();

            var memberInfo = targetProvider.TargetProperty as MemberInfo;
            if (memberInfo == null)
                throw new InvalidOperationException();

            if (string.IsNullOrWhiteSpace(Command))
            {
                Command = memberInfo.Name.Replace("Add", "");
                if (Command.Contains("Handler"))
                    Command = Command.Replace("Handler", "Command");
                else
                    Command = Command + "Command";
            }

            return CreateHandler(memberInfo, Command, targetObject.GetType());
        }

        /// <summary>
        /// The GetEventHandlerType
        /// </summary>
        /// <param name="memberInfo">The memberInfo<see cref="MemberInfo" /></param>
        /// <returns>The <see cref="Type" /></returns>
        private Type GetEventHandlerType(MemberInfo memberInfo)
        {
            Type eventHandlerType = null;
            if (memberInfo is EventInfo info1)
            {
                var eventInfo = info1;
                eventHandlerType = eventInfo.EventHandlerType;
            }
            else if (memberInfo is MethodInfo info)
            {
                var methodInfo = info;
                var pars = methodInfo.GetParameters();
                eventHandlerType = pars[1].ParameterType;
            }

            return eventHandlerType;
        }

        /// <summary>
        /// The CreateHandler
        /// </summary>
        /// <param name="memberInfo">The memberInfo<see cref="MemberInfo" /></param>
        /// <param name="cmdName">The cmdName<see cref="string" /></param>
        /// <param name="targetType">The targetType<see cref="Type" /></param>
        /// <returns>The <see cref="object" /></returns>
        private object CreateHandler(MemberInfo memberInfo, string cmdName, Type targetType)
        {
            var eventHandlerType = GetEventHandlerType(memberInfo);

            if (eventHandlerType == null)
                return null;

            var handlerInfo = eventHandlerType.GetMethod("Invoke");
            var method = new DynamicMethod("", handlerInfo?.ReturnType, handlerInfo?.GetParameters().Select(x => x.ParameterType).ToArray());

            var gen = method.GetILGenerator();
            gen.Emit(OpCodes.Ldarg, 0);
            gen.Emit(OpCodes.Ldarg, 1);
            gen.Emit(OpCodes.Ldstr, cmdName);
            if (CommandParameter == null)
                gen.Emit(OpCodes.Ldnull);
            else
                gen.Emit(OpCodes.Ldstr, CommandParameter);
            gen.Emit(OpCodes.Call, getMethod);
            gen.Emit(OpCodes.Ret);

            return method.CreateDelegate(eventHandlerType);
        }

        /// <summary>
        /// The Handler
        /// </summary>
        /// <param name="sender">The sender<see cref="object" /></param>
        /// <param name="args">The args<see cref="object" /></param>
        internal static void Handler(object sender, object args)
        {
            HandlerIntern(sender, args, "cmd", null);
        }

        /// <summary>
        /// The HandlerIntern
        /// </summary>
        /// <param name="sender">The sender<see cref="object" /></param>
        /// <param name="args">The args<see cref="object" /></param>
        /// <param name="cmdName">The cmdName<see cref="string" /></param>
        /// <param name="commandParameter">The commandParameter<see cref="string" /></param>
        public static void HandlerIntern(object sender, object args, string cmdName, string commandParameter)
        {
            if (sender is FrameworkElement fe)
            {
                var cmd = GetCommand(fe, cmdName);
                object commandParam = null;
                if (!string.IsNullOrWhiteSpace(commandParameter))
                    commandParam = GetCommandParameter(fe, args, commandParameter);
                if (cmd != null && cmd.CanExecute(commandParam))
                    cmd.Execute(commandParam);
            }
        }

        /// <summary>
        /// The GetCommand
        /// </summary>
        /// <param name="target">The target<see cref="FrameworkElement" /></param>
        /// <param name="cmdName">The cmdName<see cref="string" /></param>
        /// <returns>The <see cref="ICommand" /></returns>
        internal static ICommand GetCommand(FrameworkElement target, string cmdName)
        {
            var vm = FindViewModel(target);
            if (vm == null)
                return null;

            var vmType = vm.GetType();
            var cmdProp = vmType.GetProperty(cmdName);
            if (cmdProp != null)
                return cmdProp.GetValue(vm) as ICommand;
#if DEBUG
            throw new Exception("EventBinding path error: '" + cmdName + "' property not found on '" + vmType + "' 'DelegateCommand'");
#endif

            return null;
        }

        /// <summary>
        /// The GetCommandParameter
        /// </summary>
        /// <param name="target">The target<see cref="FrameworkElement" /></param>
        /// <param name="args">The args<see cref="object" /></param>
        /// <param name="commandParameter">The commandParameter<see cref="string" /></param>
        /// <returns>The <see cref="object" /></returns>
        internal static object GetCommandParameter(FrameworkElement target, object args, string commandParameter)
        {
            object ret = null;
            var classify = commandParameter.Split('.');
            switch (classify[0])
            {
                case "$e":
                    ret = args;
                    break;
                case "$this":
                    ret = classify.Length > 1 ? FollowPropertyPath(target, commandParameter.Replace("$this.", ""), target.GetType()) : target;
                    break;
                default:
                    ret = commandParameter;
                    break;
            }

            return ret;
        }

        /// <summary>
        /// The FindViewModel
        /// </summary>
        /// <param name="target">The target<see cref="FrameworkElement" /></param>
        /// <returns>The <see cref="ViewModelBase" /></returns>
        internal static ViewModelBase FindViewModel(FrameworkElement target)
        {
            if (target == null)
                return null;

            if (target.DataContext is ViewModelBase vm)
                return vm;

            var parent = target.GetParentObject() as FrameworkElement;

            return FindViewModel(parent);
        }

        /// <summary>
        /// The FollowPropertyPath
        /// </summary>
        /// <param name="target">The target<see cref="object" /></param>
        /// <param name="path">The path<see cref="string" /></param>
        /// <param name="valueType">The valueType<see cref="Type" /></param>
        /// <returns>The <see cref="object" /></returns>
        internal static object FollowPropertyPath(object target, string path, Type valueType = null)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target) + "is null");
            if (path == null)
                throw new ArgumentNullException(nameof(path) + "is null");

            var currentType = valueType ?? target.GetType();

            foreach (var propertyName in path.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                if (property == null)
                    throw new NullReferenceException(nameof(property) + "is null");

                target = property.GetValue(target);
                currentType = property.PropertyType;
            }

            return target;
        }
    }
}
