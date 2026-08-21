using ZwellTech;

#nullable enable

namespace ManaMagic.Core
{
    /// <summary>
    /// Represents the parameter array of a command.
    /// </summary>
    public sealed record ParameterArray : NotifyRecordPropertyChanged
    {
        private ushort parameter1 = 0;
        private ushort parameter2 = 0;
        private ushort parameter3 = 0;
        private ushort parameter4 = 0;

        /// <summary>
        /// Gets the <see cref="ParameterInfo"/> structure for the array.
        /// </summary>
        public ParameterInfo ParameterInfo { get; private set; }

        /// <summary>
        /// Gets or sets the value of parameter 1.
        /// </summary>
        public ushort Parameter1
        {
            get { return this.parameter1; }
            set { this.SetProperty(ref this.parameter1, value); }
        }

        /// <summary>
        /// Gets or sets the value of parameter 2.
        /// </summary>
        public ushort Parameter2
        {
            get { return this.parameter2; }
            set { this.SetProperty(ref this.parameter2, value); }
        }

        /// <summary>
        /// Gets or sets the value of parameter 3.
        /// </summary>
        public ushort Parameter3
        {
            get { return this.parameter3; }
            set { this.SetProperty(ref this.parameter3, value); }
        }

        /// <summary>
        /// Gets or sets the value of parameter 4.
        /// </summary>
        public ushort Parameter4
        {
            get { return this.parameter4; }
            set { this.SetProperty(ref this.parameter4, value); }
        }

        /// <summary>
        /// Gets the size of the parameter array (in bytes).
        /// </summary>
        public int Size { get { return this.CalculateSize(); } }

        /// <summary>
        /// Create a new instance of the <see cref="ParameterArray"/> class with the specified parameter info.
        /// </summary>
        /// <param name="parameterInfo">A <see cref="ParameterInfo"/> structure containing information on how the parameters are used.</param>
        public ParameterArray(ParameterInfo parameterInfo) : base(0, false)
        {
            this.SetParameterInfo(parameterInfo);
        }

        internal void SetParameterInfo(ParameterInfo parameterInfo)
        {
            this.ParameterInfo = parameterInfo;
        }

        private int CalculateSize()
        {
            int size = 0;
            if (this.ParameterInfo.UsesParameter1)
            {
                size += GetParameterSize(this.ParameterInfo.Parameter1Type);
            }
            if (this.ParameterInfo.UsesParameter2)
            {
                size += GetParameterSize(this.ParameterInfo.Parameter2Type);
            }
            if (this.ParameterInfo.UsesParameter3)
            {
                size += GetParameterSize(this.ParameterInfo.Parameter3Type);
            }
            if (this.ParameterInfo.UsesParameter4)
            {
                size += GetParameterSize(this.ParameterInfo.Parameter4Type);
            }

            return size;

            static int GetParameterSize(ParameterType type)
            {
                switch (type)
                {
                    case ParameterType.Byte:
                    case ParameterType.SByte:
                        return 1;
                    case ParameterType.UInt16:
                        return 2;
                }
                return 0;
            }
        }
    }


    /// <summary>
    /// Provides a container for data about how the parameters in a <see cref="ParameterArray"/> are used.
    /// </summary>
    public readonly struct ParameterInfo
    {
        /// <summary>
        /// Represents a <see cref="ParameterInfo"/> that does not use parameters.
        /// </summary>
        public static ParameterInfo Empty { get; } = new ParameterInfo();

        /// <summary>
        /// Gets a value indicating if Parameter1 is used.
        /// </summary>
        public bool UsesParameter1 { get; }

        /// <summary>
        /// Gets a value indicating if Parameter2 is used.
        /// </summary>
        public bool UsesParameter2 { get; }

        /// <summary>
        /// Gets a value indicating if Parameter3 is used.
        /// </summary>
        public bool UsesParameter3 { get; }

        /// <summary>
        /// Gets a value indicating if Parameter4 is used.
        /// </summary>
        public bool UsesParameter4 { get; }

        /// <summary>
        /// Gets a value indicating the actual type of Parameter1.
        /// </summary>
        public ParameterType Parameter1Type { get; }

        /// <summary>
        /// Gets a value indicating the actual type of Parameter2.
        /// </summary>
        public ParameterType Parameter2Type { get; }

        /// <summary>
        /// Gets a value indicating the actual type of Parameter3.
        /// </summary>
        public ParameterType Parameter3Type { get; }

        /// <summary>
        /// Gets a value indicating the actual type of Parameter4.
        /// </summary>
        public ParameterType Parameter4Type { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ParameterInfo"/> with the specified parameter values.
        /// </summary>
        /// <param name="usesParameter1">True if the <see cref="ParameterArray"/> uses Parameter1; false otherwise.</param>
        /// <param name="parameter1Type">The <see cref="ParameterType"/> of Parameter1.</param>
        public ParameterInfo(bool usesParameter1, ParameterType parameter1Type) : this(usesParameter1, parameter1Type, false, ParameterType.Byte) { }

        /// <summary>
        /// Creates a new instance of <see cref="ParameterInfo"/> with the specified parameter values.
        /// </summary>
        /// <param name="usesParameter1">True if the <see cref="ParameterArray"/> uses Parameter1; false otherwise.</param>
        /// <param name="parameter1Type">The <see cref="ParameterType"/> of Parameter1.</param>
        /// <param name="usesParameter2">True if the <see cref="ParameterArray"/> uses Parameter2; false otherwise.</param>
        /// <param name="parameter2Type">The <see cref="ParameterType"/> of Parameter2.</param>
        public ParameterInfo(bool usesParameter1, ParameterType parameter1Type, bool usesParameter2, ParameterType parameter2Type)
            : this(usesParameter1, parameter1Type, usesParameter2, parameter2Type, false, ParameterType.Byte, false, ParameterType.Byte) { }

        /// <summary>
        /// Creates a new instance of <see cref="ParameterInfo"/> with the specified parameter values.
        /// </summary>
        /// <param name="usesParameter1">True if the <see cref="ParameterArray"/> uses Parameter1; false otherwise.</param>
        /// <param name="parameter1Type">The <see cref="ParameterType"/> of Parameter1.</param>
        /// <param name="usesParameter2">True if the <see cref="ParameterArray"/> uses Parameter2; false otherwise.</param>
        /// <param name="parameter2Type">The <see cref="ParameterType"/> of Parameter2.</param>
        /// <param name="usesParameter3">True if the <see cref="ParameterArray"/> uses Parameter3; false otherwise.</param>
        /// <param name="parameter3Type">The <see cref="ParameterType"/> of Parameter3.</param>
        public ParameterInfo(bool usesParameter1, ParameterType parameter1Type, bool usesParameter2, ParameterType parameter2Type, bool usesParameter3, ParameterType parameter3Type)
            : this(usesParameter1, parameter1Type, usesParameter2, parameter2Type, usesParameter3, parameter3Type, false, ParameterType.Byte) { }

        /// <summary>
        /// Creates a new instance of <see cref="ParameterInfo"/> with the specified parameter values.
        /// </summary>
        /// <param name="usesParameter1">True if the <see cref="ParameterArray"/> uses Parameter1; false otherwise.</param>
        /// <param name="parameter1Type">The <see cref="ParameterType"/> of Parameter1.</param>
        /// <param name="usesParameter2">True if the <see cref="ParameterArray"/> uses Parameter2; false otherwise.</param>
        /// <param name="parameter2Type">The <see cref="ParameterType"/> of Parameter2.</param>
        /// <param name="usesParameter3">True if the <see cref="ParameterArray"/> uses Parameter3; false otherwise.</param>
        /// <param name="parameter3Type">The <see cref="ParameterType"/> of Parameter3.</param>
        /// <param name="usesParameter4">True if the <see cref="ParameterArray"/> uses Parameter4; false otherwise.</param>
        /// <param name="parameter4Type">The <see cref="ParameterType"/> of Parameter4.</param>
        public ParameterInfo(bool usesParameter1, ParameterType parameter1Type,
                             bool usesParameter2, ParameterType parameter2Type,
                             bool usesParameter3, ParameterType parameter3Type,
                             bool usesParameter4, ParameterType parameter4Type)
        {
            this.UsesParameter1 = usesParameter1;
            this.UsesParameter2 = usesParameter2;
            this.UsesParameter3 = usesParameter3;
            this.UsesParameter4 = usesParameter4;

            this.Parameter1Type = parameter1Type;
            this.Parameter2Type = parameter2Type;
            this.Parameter3Type = parameter3Type;
            this.Parameter4Type = parameter4Type;
        }
    }

    /// <summary>
    /// Represents the parameter type of a parameter.
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// The type is a byte.
        /// </summary>
        Byte = 0,
        /// <summary>
        /// The type is a signed byte.
        /// </summary>
        SByte,
        /// <summary>
        /// The type is an unsigned short.
        /// </summary>
        UInt16,
        /// <summary>
        /// The type is a variable length byte array.
        /// </summary>
        VarByteArray,
    }
}