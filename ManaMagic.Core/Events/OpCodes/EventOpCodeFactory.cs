using System;
using System.Collections.Generic;
using System.Reflection;
using ManaMagic.Core.Events.OpCodes.Graphical;
using ManaMagic.Core.Events.OpCodes.TextData;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes
{
    public static class EventOpCodeFactory
    {
        private static readonly Type[] parameterTypes = new Type[] { typeof(EventOpCodeType), typeof(EventOpCodeCommandType), typeof(ParameterInfo), typeof(ushort), typeof(ushort), typeof(ushort), typeof(ushort) };
        private static readonly IReadOnlyDictionary<EventOpCodeType, ParameterInfo> opCodeParameterInfo = new Dictionary<EventOpCodeType, ParameterInfo>()
        {
            // EventOpCodeCommandType.System
            { EventOpCodeType.End, ParameterInfo.Empty },
            { EventOpCodeType.NoOperation, ParameterInfo.Empty },
            { EventOpCodeType.ReturnToCaller, ParameterInfo.Empty },
            { EventOpCodeType.JumpToEvent1, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.JumpToEvent2, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.JumpToEvent3, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.JumpToEvent4, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.JumpToEvent5, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.JumpToEvent6, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.JumpToEvent7, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.JumpToEvent8, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.CallEvent1, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.CallEvent2, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.CallEvent3, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.CallEvent4, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.CallEvent5, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.CallEvent6, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.CallEvent7, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.CallEvent8, new ParameterInfo(true, ParameterType.Byte) },

            // EventOpCodeCommandType.Utility
            { EventOpCodeType.Utility, new ParameterInfo(true, ParameterType.Byte) },

            // EventOpCodeCommandType.Graphical
            { EventOpCodeType.RefreshNpcState, ParameterInfo.Empty },
            { EventOpCodeType.RefreshMapState, ParameterInfo.Empty },
            { EventOpCodeType.RefreshPartyStatusBars, ParameterInfo.Empty },
            { EventOpCodeType.ScreenEffect, new ParameterInfo(true, ParameterType.Byte) },

            // EventOpCodeCommandType.Timing
            { EventOpCodeType.WaitForAnimations, ParameterInfo.Empty },
            { EventOpCodeType.Wait, new ParameterInfo(true, ParameterType.Byte) },

            // EventOpCodeCommandType.Audio
            { EventOpCodeType.PlayAudio, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte, true, ParameterType.Byte, true, ParameterType.Byte) },

            // EventOpCodeCommandType.EventFlag
            { EventOpCodeType.IncrementEventFlag, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.DecrementEventFlag, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.SetEventFlag, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte) },

            // EventOpCodeCommandType.RingMenu
            { EventOpCodeType.OpenSellItemRing, ParameterInfo.Empty },
            { EventOpCodeType.OpenWeaponUpgradeRing, ParameterInfo.Empty },
            { EventOpCodeType.OpenShopRing, new ParameterInfo(true, ParameterType.Byte) },

            // EventOpCodeCommandType.Inventory
            { EventOpCodeType.AddItem, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.AddGold, new ParameterInfo(true, ParameterType.UInt16) },
            { EventOpCodeType.SubtractGold, new ParameterInfo(true, ParameterType.UInt16) },

            // EventOpCodeCommandType.Sprite
            { EventOpCodeType.ToggleInvisibilityA, ParameterInfo.Empty },
            { EventOpCodeType.ToggleInvisibilityB, ParameterInfo.Empty },
            { EventOpCodeType.LockSpriteBehavior, ParameterInfo.Empty },
            { EventOpCodeType.UnlockSpriteBehavior, ParameterInfo.Empty },
            { EventOpCodeType.AnimateActorSingle, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte) },
            { EventOpCodeType.MoveActor, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte) },
            { EventOpCodeType.AnimateActorLoop, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte) },
            { EventOpCodeType.SetCharacterSlotAddress, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte, true, ParameterType.Byte) },

            // EventOpCodeCommandType.Conditional
            { EventOpCodeType.IfCharacterSlotActivatedEvent, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.IfEventFlagInRange, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte) },
            { EventOpCodeType.IfCharacterSlotAddressGreaterThanOrEqual, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte, true, ParameterType.Byte) },
            { EventOpCodeType.IfCharacterSlotAddressLessThanOrEqual, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte, true, ParameterType.Byte) },
            { EventOpCodeType.IfCharacterSlotAddressBitTest, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte, true, ParameterType.Byte) },
            { EventOpCodeType.IfCharacterSlotAddressIsEqual, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte, true, ParameterType.Byte) },

            // EventOpCodeCommandType.Party
            { EventOpCodeType.BringPartyToEventActivator, ParameterInfo.Empty },
            { EventOpCodeType.AddToParty, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.RemoveFromParty, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.Restore, new ParameterInfo(true, ParameterType.Byte) },

            // EventOpCodeCommandType.Transport
            { EventOpCodeType.UseDoor1, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.UseDoor2, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.UseDoor3, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.UseDoor4, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.FlammieFlight, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.CannonTravel, new ParameterInfo(true, ParameterType.Byte) },

            // EventOpCodeCommandType.Text
            { EventOpCodeType.OpenTextWindow, ParameterInfo.Empty },
            { EventOpCodeType.CloseTextWindow, ParameterInfo.Empty },
            { EventOpCodeType.ClearTextWindow, ParameterInfo.Empty },
            { EventOpCodeType.PrintPlayerName, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.BeginOptionDialogSetup, ParameterInfo.Empty },
            { EventOpCodeType.PadLeft, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.SetOptionDialogOption, new ParameterInfo(true, ParameterType.Byte) },
            { EventOpCodeType.EndOptionDialogSetup, ParameterInfo.Empty },
            { EventOpCodeType.OpenCurrencyWindow, ParameterInfo.Empty },
            { EventOpCodeType.CloseCurrencyWindow, ParameterInfo.Empty },
            { EventOpCodeType.PrintCurrency, ParameterInfo.Empty },
            { EventOpCodeType.BeginASCIITextCrawl, ParameterInfo.Empty },
            { EventOpCodeType.EndASCIITextCrawl, ParameterInfo.Empty },


            // EventOpCodeCommandType.TextData
            { EventOpCodeType.TextStart, new ParameterInfo(true, ParameterType.VarByteArray) },
            { EventOpCodeType.ASCIITextStart, new ParameterInfo(true, ParameterType.VarByteArray) },
        };
        private static readonly IReadOnlyDictionary<EventOpCodeType, ParameterInfo> opCodeAltParameterInfo = new Dictionary<EventOpCodeType, ParameterInfo>()
        {
            // EventOpCodeCommandType.Graphical
            { EventOpCodeType.ScreenEffect, new ParameterInfo(true, ParameterType.Byte, true, ParameterType.UInt16) },

            // EventOpCodeCommandType.Text
            { EventOpCodeType.EndOptionDialogSetup, new ParameterInfo(true, ParameterType.UInt16) },
        };

        public static EventOpCode Create(EventOpCodeType type, ushort parameter1 = 0, ushort parameter2 = 0, ushort parameter3 = 0, ushort parameter4 = 0)
        {
            Type classType = EventUtil.GetEventOpCodeClassType(type);
            object[] constructorArgs = new object[]
            {
                type,
                EventUtil.GetEventOpCodeCommandTypeAttribute(classType)!.CommandType,
                EventOpCodeFactory.GetParameterInfo(type, parameter1),
                parameter1,
                parameter2,
                parameter3,
                parameter4
            };
            ConstructorInfo? constructor = classType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, EventOpCodeFactory.parameterTypes, null);
            if (constructor == null)
            {
                ThrowHelper.ThrowInvalidOperationException("Failed to find event op-code constructor.");
            }

            EventOpCode? opCode = (EventOpCode?)constructor.Invoke(constructorArgs);
            if (opCode == null)
            {
                ThrowHelper.ThrowInvalidOperationException("Failed to create event op-code.");
            }
            return opCode;
        }

        public static EventOpCode CreateText(IReadOnlyList<byte> textData, bool ascii)
        {
            if (!ascii)
            {
                return new ManaTextEventOpCode(textData, EventOpCodeFactory.opCodeParameterInfo[EventOpCodeType.TextStart]);
            }
            return new AsciiTextEventOpCode(textData, EventOpCodeFactory.opCodeParameterInfo[EventOpCodeType.ASCIITextStart]);
        }

        internal static ParameterInfo GetParameterInfo(EventOpCodeType type, ushort parameter1)
        {
            if (type == EventOpCodeType.EndOptionDialogSetup && parameter1 == 0)
            {
                return EventOpCodeFactory.opCodeAltParameterInfo[type];
            }
            else if (type == EventOpCodeType.ScreenEffect && ScreenEffectEventOpCode.HasAdditionalOperand((ScreenEffectEventOpCodeType)parameter1))
            {
                return EventOpCodeFactory.opCodeAltParameterInfo[type];
            }
            return EventOpCodeFactory.opCodeParameterInfo[type];
        }
    }
}