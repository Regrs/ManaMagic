using System.Collections.Generic;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.TextData;
using ZwellTech;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public class EventRomWriter : RomWriter
    {
        /// <inheritdoc/>
        public EventRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteEvents(EventContext context)
        {
            this.WriteEvents(context, Constants.Bank09Offset, (int)Constants.Bank09.EventIdMinimum, (int)(Constants.Bank09.EventIdMaximum + 1));
            this.WriteEvents(context, Constants.Bank0AOffset, (int)Constants.Bank0A.EventIdMinimum, (int)(Constants.Bank0A.EventIdMaximum + 1));
        }

        protected void WriteEvent(ManaEvent manaEvent)
        {
            foreach (EventOpCode opCode in manaEvent)
            {
                if (opCode is TextDataEventOpCode textOpCode)
                {
                    foreach (byte b in textOpCode.TextData)
                    {
                        this.Write(b);
                    }
                }
                else
                {
                    this.Write((byte)opCode.OperationCode);
                    if (opCode.Parameters.ParameterInfo.UsesParameter1)
                    {
                        WriteParameter(opCode.Parameters.ParameterInfo.Parameter1Type, opCode.Parameters.Parameter1);
                    }
                    if (opCode.Parameters.ParameterInfo.UsesParameter2)
                    {
                        WriteParameter(opCode.Parameters.ParameterInfo.Parameter2Type, opCode.Parameters.Parameter2);
                    }
                    if (opCode.Parameters.ParameterInfo.UsesParameter3)
                    {
                        WriteParameter(opCode.Parameters.ParameterInfo.Parameter3Type, opCode.Parameters.Parameter3);
                    }
                    if (opCode.Parameters.ParameterInfo.UsesParameter4)
                    {
                        WriteParameter(opCode.Parameters.ParameterInfo.Parameter4Type, opCode.Parameters.Parameter4);
                    }
                }
            }

            void WriteParameter(ParameterType parameterType, ushort value)
            {
                switch (parameterType)
                {
                    case ParameterType.Byte:
                        this.Write((byte)value);
                        break;
                    case ParameterType.SByte:
                        this.Write((sbyte)value);
                        break;
                    case ParameterType.UInt16:
                        this.WriteUInt16(value);
                        break;
                    default:
                        ThrowHelper.ThrowInvalidOperationException("Invalid Event Parameter Type");
                        break;
                }
            }
        }

        private void WriteEvents(EventContext context, int bankOffset, int minimumIndex, int maximumIndex)
        {
            List<ushort> pointerTable = new List<ushort>(maximumIndex);

            int offset = maximumIndex * sizeof(ushort);
            this.Seek(bankOffset | offset);

            for (int i = minimumIndex; i < maximumIndex; i++)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));

                ManaEvent manaEvent = context.Events[i];
                this.WriteEvent(manaEvent);
            }

            this.Seek(bankOffset | 0x0000);
            foreach (ushort pointer in pointerTable)
            {
                this.WriteUInt16(pointer);
            }
        }

        public void WriteByteAt(int index, byte value)
        {
            this.RomFile.WriteByteAt(index, value);
        }

        public void WriteUInt16At(int index, ushort value)
        {
            this.RomFile.WriteUInt16At(index, value);
        }
    }
}