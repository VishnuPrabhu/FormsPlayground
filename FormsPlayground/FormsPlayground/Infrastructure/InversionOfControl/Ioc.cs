using System;
using System.Collections.Generic;
using DryIoc;
using FormsPlayground.Resources;

namespace FormsPlayground.Infrastructure.InversionOfControl
{
    public sealed class Ioc
    {
        private static readonly Lazy<IContainer> Lazy = new Lazy<IContainer>(CreateContainer);
        private static readonly Lazy<Ioc> LazyIoc = new Lazy<Ioc>(() => new Ioc());
        
        public static IContainer Container => Lazy.Value;
        public static Ioc Bootstrapper => LazyIoc.Value;
        
        private static IContainer CreateContainer() =>
            new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
        
        private readonly List<string> _actions;

        private Ioc() 
            => _actions = new List<string>();

        public void Bootstrap(string identifier, Action<IContainer> bootStrapAction)
        {
            // Make sure we don´t bootstrap anything twice
            if (_actions.Contains(identifier))
                return;
            
            if(string.IsNullOrEmpty(identifier))
                throw new ArgumentException(
                    Strings.Ioc_Bootstrap_Argument_is_null_or_empty, 
                    nameof(identifier));
            
            _actions.Add(identifier);

            bootStrapAction(Container);
        }
    }
}