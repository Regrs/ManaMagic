using System.Collections.Generic;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.Graphical;
using ZwellTech;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Events
{
    public sealed class EventParser
    {
        private readonly RomReader romReader;

        public EventParser(RomReader romReader)
        {
            this.romReader = romReader;
        }

        public ManaEvent ParseEvent(ushort index)
        {
            int bankoffset = index <= Constants.Bank09.EventIdMaximum ? Constants.Bank09Offset : Constants.Bank0AOffset;
            index = (ushort)(index <= Constants.Bank09.EventIdMaximum ? index : index - 0x0400);
            this.romReader.Seek(bankoffset | (index * 2));

            ushort pointer = this.romReader.ReadUInt16();
            return this.ParseEventAtAddress(bankoffset | pointer);
        }

        public ManaEvent ParseEventAtAddress(uint address)
        {
            return this.ParseEventAtAddress((int)address);
        }

        public ManaEvent ParseEventAtAddress(int address)
        {
            List<EventOpCode> opCodes = new List<EventOpCode>();
            this.romReader.Seek(address);

            bool done = false;
            while (!done)
            {
                EventOpCodeType opCode = (EventOpCodeType)this.romReader.Read();
                ushort parameter1 = 0;
                ushort parameter2 = 0;
                ushort parameter3 = 0;
                ushort parameter4 = 0;
                if (EventParser.IsText(opCode))
                {
                    this.ParseTextData(opCode, opCodes);
                }
                else if (opCode == EventOpCodeType.BeginASCIITextCrawl)
                {
                    this.ParseAsciiTextEventCode(opCode, opCodes);
                }
                else
                {
                    switch (opCode)
                    {
                        // Op-Codes with no parameters.
                        case EventOpCodeType.End:
                            opCodes.Add(EventOpCodeFactory.Create(opCode));
                            done = true;
                            break;
                        case EventOpCodeType.NoOperation:
                        case EventOpCodeType.ReturnToCaller:
                        case EventOpCodeType.BringPartyToEventActivator:
                        case EventOpCodeType.ToggleInvisibilityA:
                        case EventOpCodeType.ToggleInvisibilityB:
                        case EventOpCodeType.LockSpriteBehavior:
                        case EventOpCodeType.UnlockSpriteBehavior:
                        case EventOpCodeType.WaitForAnimations:
                        case EventOpCodeType.RefreshNpcState:
                        case EventOpCodeType.RefreshMapState:
                        case EventOpCodeType.OpenSellItemRing:
                        case EventOpCodeType.OpenWeaponUpgradeRing:
                        case EventOpCodeType.RefreshPartyStatusBars:
                        case EventOpCodeType.OpenTextWindow:
                        case EventOpCodeType.CloseTextWindow:
                        case EventOpCodeType.ClearTextWindow:
                        case EventOpCodeType.BeginOptionDialogSetup:
                        case EventOpCodeType.OpenCurrencyWindow:
                        case EventOpCodeType.CloseCurrencyWindow:
                        case EventOpCodeType.PrintCurrency:
                            opCodes.Add(EventOpCodeFactory.Create(opCode));
                            break;
                        // Op-Codes with 1 byte parameter.
                        case EventOpCodeType.JumpToEvent1:
                        case EventOpCodeType.JumpToEvent2:
                        case EventOpCodeType.JumpToEvent3:
                        case EventOpCodeType.JumpToEvent4:
                        case EventOpCodeType.JumpToEvent5:
                        case EventOpCodeType.JumpToEvent6:
                        case EventOpCodeType.JumpToEvent7:
                        case EventOpCodeType.JumpToEvent8:
                        case EventOpCodeType.UseDoor1:
                        case EventOpCodeType.UseDoor2:
                        case EventOpCodeType.UseDoor3:
                        case EventOpCodeType.UseDoor4:
                        case EventOpCodeType.FlammieFlight:
                        case EventOpCodeType.CannonTravel:
                        case EventOpCodeType.AddItem:
                        case EventOpCodeType.Utility:
                        case EventOpCodeType.CallEvent1:
                        case EventOpCodeType.CallEvent2:
                        case EventOpCodeType.CallEvent3:
                        case EventOpCodeType.CallEvent4:
                        case EventOpCodeType.CallEvent5:
                        case EventOpCodeType.CallEvent6:
                        case EventOpCodeType.CallEvent7:
                        case EventOpCodeType.CallEvent8:
                        case EventOpCodeType.Wait:
                        case EventOpCodeType.IncrementEventFlag:
                        case EventOpCodeType.DecrementEventFlag:
                        case EventOpCodeType.AddToParty:
                        case EventOpCodeType.RemoveFromParty:
                        case EventOpCodeType.OpenShopRing:
                        case EventOpCodeType.Restore:
                        case EventOpCodeType.IfCharacterSlotActivatedEvent:
                        case EventOpCodeType.PrintPlayerName:
                        case EventOpCodeType.PadLeft:
                        case EventOpCodeType.SetOptionDialogOption:
                            parameter1 = this.romReader.Read();
                            opCodes.Add(EventOpCodeFactory.Create(opCode, parameter1));
                            break;
                        // Op-Codes with 2 byte parameters.
                        case EventOpCodeType.SetEventFlag:
                        case EventOpCodeType.AnimateActorSingle:
                        case EventOpCodeType.MoveActor:
                        case EventOpCodeType.AnimateActorLoop:
                        case EventOpCodeType.IfEventFlagInRange:
                            parameter1 = this.romReader.Read();
                            parameter2 = this.romReader.Read();
                            opCodes.Add(EventOpCodeFactory.Create(opCode, parameter1, parameter2));
                            break;
                        // Op-Codes with 3 byte parameters.
                        case EventOpCodeType.SetCharacterSlotAddress:
                        case EventOpCodeType.IfCharacterSlotAddressGreaterThanOrEqual:
                        case EventOpCodeType.IfCharacterSlotAddressLessThanOrEqual:
                        case EventOpCodeType.IfCharacterSlotAddressBitTest:
                        case EventOpCodeType.IfCharacterSlotAddressIsEqual:
                            parameter1 = this.romReader.Read();
                            parameter2 = this.romReader.Read();
                            parameter3 = this.romReader.Read();
                            opCodes.Add(EventOpCodeFactory.Create(opCode, parameter1, parameter2, parameter3));
                            break;
                        case EventOpCodeType.PlayAudio:
                            // Op-Codes with 4 byte parameters.
                            parameter1 = this.romReader.Read();
                            parameter2 = this.romReader.Read();
                            parameter3 = this.romReader.Read();
                            parameter4 = this.romReader.Read();
                            opCodes.Add(EventOpCodeFactory.Create(opCode, parameter1, parameter2, parameter3, parameter4));
                            break;
                        // Op-Codes with 1 ushort parameter.
                        case EventOpCodeType.AddGold:
                        case EventOpCodeType.SubtractGold:
                            parameter1 = this.romReader.ReadUInt16();
                            opCodes.Add(EventOpCodeFactory.Create(opCode, parameter1));
                            break;
                        // Op-Codes with variable parameters.
                        case EventOpCodeType.ScreenEffect:
                            parameter1 = this.romReader.Read();
                            parameter2 = ScreenEffectEventOpCode.HasAdditionalOperand((ScreenEffectEventOpCodeType)parameter1) ? this.romReader.ReadUInt16() : (ushort)0u;
                            opCodes.Add(EventOpCodeFactory.Create(opCode, parameter1, parameter2));
                            break;
                        case EventOpCodeType.EndOptionDialogSetup:
                            // If the next two bytes are zero then those bytes are part of the op-code and the dialog cannot be canceled.
                            // If the next two bytes are not zero then they belong to the next op-code and the dialog can be canceled.
                            bool canCancel = this.romReader.PeekUInt16() != 0x0000;
                            parameter1 = (ushort)(canCancel ? 1 : 0);
                            if (!canCancel) { _ = this.romReader.ReadUInt16(); }

                            opCodes.Add(EventOpCodeFactory.Create(opCode, parameter1));
                            break;
                        default:
                            ThrowHelper.ThrowInvalidOperationException("Unimplemented Event Op Code");
                            break;
                    }
                }
            }

            return new ManaEvent(opCodes);
        }

        private void ParseTextData(EventOpCodeType opCode, List<EventOpCode> opCodes)
        {
            int lineCount = opCode == EventOpCodeType.TextStart ? 2 : 1;
            List<byte> textData = new List<byte>() { (byte)opCode };

            // Checking for 2A is a hack to successfully read the Cave-In skill's bugged name.
            // The bugged op-code is in the middle of the string and we have to have started reading a string to get here.
            // So there should be no danger of any 2As slipping though except for the one in Cave-In's name.
            while (EventParser.IsText((EventOpCodeType)this.romReader.Peek()) || this.romReader.Peek() == 0x2A)
            {
                byte textChar = this.romReader.Read();
                textData.Add(textChar);
                if (textChar == 0x7F) { lineCount++; }
            }
            opCodes.Add(EventOpCodeFactory.CreateText(textData, false));
        }

        private void ParseAsciiTextEventCode(EventOpCodeType opCode, List<EventOpCode> opCodes)
        {
            // For whatever reason the BeginASCIITextCrawl op-code is also responsible for loading the snowflake graphics into memory.
            // FLAG_SNOWFALL will not work properly until this op-code is called.
            // The ending event calls BeginASCIITextCrawl and EndASCIITextCrawl back-to-back to load the graphics without printing any text.
            opCodes.Add(EventOpCodeFactory.Create(EventOpCodeType.BeginASCIITextCrawl));

            int lineCount = 0;
            List<byte> textData = new List<byte>();

            // Once the BeginASCIITextCrawl op-code is read, all further bytes are treated as ASCII text until the EndASCIITextCrawl op-code is read.
            // ASCII text will do a Star Wars text crawl on the screen.
            while ((EventOpCodeType)this.romReader.Peek() != EventOpCodeType.EndASCIITextCrawl)
            {
                byte textChar = this.romReader.Read();
                textData.Add(textChar);
                if (textChar == 0x7F) { lineCount++; }
            }

            if (textData.Count > 0)
            {
                opCodes.Add(EventOpCodeFactory.CreateText(textData, true));
            }

            _ = this.romReader.Read();
            opCodes.Add(EventOpCodeFactory.Create(EventOpCodeType.EndASCIITextCrawl));
        }

        private static bool IsText(EventOpCodeType opCode)
        {
            return (opCode >= EventOpCodeType.TextStart) && (opCode <= EventOpCodeType.TextEnd);
        }

        public static bool IsTextEventtCode(EventOpCodeType opCode)
        {
            return (opCode >= EventOpCodeType.OpenTextWindow) && (opCode <= EventOpCodeType.PrintCurrency);
        }
    }
}