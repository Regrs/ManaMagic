using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes
{
    /// <summary>
    /// Represents the base class for all event system op-codes in Secret of Mana.
    /// </summary>
    [DebuggerDisplay("{OperationCode}")]
    public abstract record EventOpCode : NotifyRecordPropertyChanged
    {
        internal const string ParametersCategoryName = "\tParameters";

        private EventOpCodeType operationCode = EventOpCodeType.End;

        /// <summary>
        /// Gets the type of the event op-code.
        /// </summary>
        [Category("Miscellaneous")]
        [Description("The operation code of this event op-code.")]
        public EventOpCodeType OperationCode
        {
            get { return this.operationCode; }
            protected set { this.SetProperty(ref this.operationCode, value); }
        }

        /// <summary>
        /// Gets the <see cref="ParameterArray"/> of the event op-code.
        /// </summary>
        [Browsable(false)]
        public ParameterArray Parameters { get; }

        /// <summary>
        /// Gets the size of the op-code (in bytes).
        /// </summary>
        [Browsable(false)]
        public virtual int Size { get { return this.Parameters.Size + 1; } }

        /// <summary>
        /// Gets the command type of the event op-code.
        /// </summary>
        [Browsable(false)]
        public EventOpCodeCommandType CommandType { get; }

        /// <summary>
        /// Gets the color the the op-code should be displayed to the user.
        /// </summary>
        [Browsable(false)]
        public abstract Color Color { get; }

        internal EventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo) : base(0, false)
        {
            this.OperationCode = opCodeType;
            this.CommandType = commandType;
            this.Parameters = new ParameterArray(parameterInfo);
            this.Parameters.PropertyChanged += this.Parameters_PropertyChanged;
        }

        internal EventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo, 
                             ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4) : this(opCodeType, commandType, parameterInfo)
        {
            this.Parameters.Parameter1 = parameter1;
            this.Parameters.Parameter2 = parameter2;
            this.Parameters.Parameter3 = parameter3;
            this.Parameters.Parameter4 = parameter4;
            this.Parameters.PropertyChanged += this.Parameters_PropertyChanged;
        }

        private void Parameters_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Parameters));
        }
    }
}