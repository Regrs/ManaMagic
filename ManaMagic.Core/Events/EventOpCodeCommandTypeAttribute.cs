using System;

#nullable enable

namespace ManaMagic.Core.Events
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class EventOpCodeCommandTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets the command type of the event op-code.
        /// </summary>
        public EventOpCodeCommandType CommandType { get; }

        public EventOpCodeCommandTypeAttribute(EventOpCodeCommandType commandType)
        {
            this.CommandType = commandType;
        }
    }
}