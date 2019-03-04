using System;

namespace FormsPlayground.Framework.Exceptions
{
    public class ControlledException : Exception
    {
        public string Title { get; }

        public ControlledException(string message, string title = null)
            : base(message)
        {
            Title = title;
        }
    }
}