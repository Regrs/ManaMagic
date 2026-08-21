using System;

#nullable enable

namespace ManaMagic.Core.Events
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class EventOpCodeTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the event op-code.
        /// </summary>
        public EventOpCodeType OperationCode { get; }

        public EventOpCodeTypeAttribute(EventOpCodeType operationCode)
        {
            this.OperationCode = operationCode;
        }
    }
}