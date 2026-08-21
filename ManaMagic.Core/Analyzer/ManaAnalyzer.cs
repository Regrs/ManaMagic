using System.Collections.Generic;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.Conditional;
using ManaMagic.Core.Events.OpCodes.EventFlags;
using ManaMagic.Core.Events.OpCodes.System;
using ManaMagic.Core.Maps;
using ZwellTech;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Analyzer
{
    public sealed class ManaAnalyzer
    {
        private readonly SecretOfManaContext context;

        public ManaAnalyzer(SecretOfManaContext context)
        {
            this.context = context;
        }

        public IReadOnlyList<AnalyzerResult> FindEventReferences(ushort eventIndex)
        {
            ushort index = 0;

            List<AnalyzerResult> results = new List<AnalyzerResult>();
            AnalyzerHardcodedResults.AddHardcodedEvents(eventIndex, results);

            foreach (ManaEvent manaEvent in this.context.EventContext.Events)
            {
                uint lineIndex = 0;
                foreach (EventOpCode opCode in manaEvent.OpCodes)
                {
                    switch (opCode.OperationCode)
                    {
                        case EventOpCodeType.JumpToEvent1:
                        case EventOpCodeType.JumpToEvent2:
                        case EventOpCodeType.JumpToEvent3:
                        case EventOpCodeType.JumpToEvent4:
                        case EventOpCodeType.JumpToEvent5:
                        case EventOpCodeType.JumpToEvent6:
                        case EventOpCodeType.JumpToEvent7:
                        case EventOpCodeType.JumpToEvent8:
                            if (opCode is JumpToEventOpCode jumpOpCode && jumpOpCode.EventId == eventIndex)
                            {
                                string info = $"Operation Code Index: {lineIndex:X2}, Operation Code: JumpToEvent.";
                                results.Add(new AnalyzerResult(AnalyzerReferenceType.Event, index, lineIndex, manaEvent, info));
                            }
                            break;
                        case EventOpCodeType.CallEvent1:
                        case EventOpCodeType.CallEvent2:
                        case EventOpCodeType.CallEvent3:
                        case EventOpCodeType.CallEvent4:
                        case EventOpCodeType.CallEvent5:
                        case EventOpCodeType.CallEvent6:
                        case EventOpCodeType.CallEvent7:
                        case EventOpCodeType.CallEvent8:
                            if (opCode is CallEventOpCode callOpCode && callOpCode.EventId == eventIndex)
                            {
                                string info = $"Operation Code Index: {lineIndex:X2}, Operation Code: CallEvent.";
                                results.Add(new AnalyzerResult(AnalyzerReferenceType.Event, index, lineIndex, manaEvent, info));
                            }
                            break;
                    }
                    lineIndex++;
                }

                index++;
            }

            foreach (MapCollision collisionType in this.context.MapContext.MapCollisionDefinitionTable)
            {
                if (collisionType.IsEvent && collisionType.EventIndex == eventIndex)
                {
                    results.Add(new AnalyzerResult(AnalyzerReferenceType.Collision, (byte)collisionType.CollisionType, 0, collisionType));
                }
            }

            index = 0;
            foreach (MapHeader header in this.context.MapContext.MapHeaderTable)
            {
                foreach (MapSpriteObject spriteObject in header.ObjectTable)
                {
                    if (spriteObject.EventIndex == eventIndex)
                    {
                        results.Add(new AnalyzerResult(AnalyzerReferenceType.SpriteObject, index, 0, spriteObject));
                    }
                }
                index++;
            }

            index = 0;
            foreach (DataTable<MapTrigger> triggerList in context.MapContext.MapTriggersTable)
            {
                int triggerIndex = 0;
                foreach (MapTrigger trigger in triggerList)
                {
                    if (trigger.IsEvent && trigger.Value == eventIndex)
                    {
                        string info = $"Map Index: {index:X4}, Trigger Index: {triggerIndex:X2}";
                        results.Add(new AnalyzerResult(AnalyzerReferenceType.MapTrigger, index, 0, trigger, info));
                    }
                    triggerIndex++;
                }
                index++;
            }

            return results;
        }

        public IReadOnlyList<AnalyzerResult> FindEventFlagReferences(EventFlag eventFlag)
        {
            ushort index = 0;

            List<AnalyzerResult> results = new List<AnalyzerResult>();
            AnalyzerHardcodedResults.AddHardcodedEventFlags(eventFlag, results);

            foreach (ManaEvent manaEvent in this.context.EventContext.Events)
            {
                uint lineIndex = 0;
                foreach (EventOpCode opCode in manaEvent.OpCodes)
                {
                    switch (opCode.OperationCode)
                    {
                        case EventOpCodeType.IncrementEventFlag:
                            if (opCode is IncrementEventFlagEventOpCode incrementOpCode && incrementOpCode.Flag == eventFlag)
                            {
                                string info = $"Operation Code Index: {lineIndex:X2}, Operation Code: IncrementEventFlag.";
                                results.Add(new AnalyzerResult(AnalyzerReferenceType.Event, index, lineIndex, manaEvent, info));
                            }
                            break;
                        case EventOpCodeType.DecrementEventFlag:
                            if (opCode is DecrementEventFlagEventOpCode decrementOpCode && decrementOpCode.Flag == eventFlag)
                            {
                                string info = $"Operation Code Index: {lineIndex:X2}, Operation Code: DecrementEventFlag.";
                                results.Add(new AnalyzerResult(AnalyzerReferenceType.Event, index, lineIndex, manaEvent, info));
                            }
                            break;
                        case EventOpCodeType.SetEventFlag:
                            if (opCode is SetEventFlagEventOpCode setOpCode && setOpCode.Flag == eventFlag)
                            {
                                string info = $"Operation Code Index: {lineIndex:X2}, Operation Code: SetEventFlag, Value: {setOpCode.Value:X2}.";
                                results.Add(new AnalyzerResult(AnalyzerReferenceType.Event, index, lineIndex, manaEvent, info));
                            }
                            break;
                        case EventOpCodeType.IfEventFlagInRange:
                            if (opCode is IfEventFlagInRangeEventOpCode ifOpCode && ifOpCode.Flag == eventFlag)
                            {
                                string info = $"Operation Code Index: {lineIndex:X2}, Operation Code: IfEventFlagInRange, Minimum: {ifOpCode.Minimum:X2}, Maximum: {ifOpCode.Maximum:X2}.";
                                results.Add(new AnalyzerResult(AnalyzerReferenceType.Event, index, lineIndex, manaEvent, info));
                            }
                            break;
                            //case EventOpCodeType.BrokenEventFlagConditional:
                            //case EventOpCodeType.BrokenCompareEventFlagConditional:
                    }
                    lineIndex++;
                }

                index++;
            }

            index = 0;
            foreach (MapHeader header in this.context.MapContext.MapHeaderTable)
            {
                foreach (MapSpriteObject spriteObject in header.ObjectTable)
                {
                    if (spriteObject.EventFlag == eventFlag)
                    {
                        results.Add(new AnalyzerResult(AnalyzerReferenceType.SpriteObject, index, 0, spriteObject));
                    }
                }
                index++;
            }

            index = 0;
            foreach (MapObjectTable objectTable in this.context.MapContext.MapObjectTable)
            {
                if (objectTable.Layer1Background.EventFlag == eventFlag)
                {
                    //string info = $"Minimum: {objectTable.Layer1Background.EventFlagRange.Minimum:X2}, Maximum: {objectTable.Layer1Background.EventFlagRange.Maximum:X2}.";
                    results.Add(new AnalyzerResult(AnalyzerReferenceType.MapLayer1Background, index, 0, objectTable.Layer1Background));
                }
                foreach (MapObject mapObject in objectTable.Layer1)
                {
                    if (mapObject.EventFlag == eventFlag)
                    {
                        results.Add(new AnalyzerResult(AnalyzerReferenceType.MapLayer1Object, index, 0, mapObject));
                    }
                }

                if (objectTable.HasLayer2)
                {
                    if (objectTable.Layer2Background.EventFlag == eventFlag)
                    {
                        results.Add(new AnalyzerResult(AnalyzerReferenceType.MapLayer2Background, index, 0, objectTable.Layer2Background));
                    }
                    foreach (MapObject mapObject in objectTable.Layer2)
                    {
                        if (mapObject.EventFlag == eventFlag)
                        {
                            results.Add(new AnalyzerResult(AnalyzerReferenceType.MapLayer2Object, index, 0, mapObject));
                        }
                    }
                }
                index++;
            }

            return results;
        }

        public IReadOnlyList<AnalyzerResult> FindEventOpCodeReferences(EventOpCodeType opCodeType)
        {
            ushort index = 0;

            List<AnalyzerResult> results = new List<AnalyzerResult>();
            //AnalyzerHardcodedResults.AddHardcodedEventFlags(eventFlag, results);

            foreach (ManaEvent manaEvent in this.context.EventContext.Events)
            {
                uint lineIndex = 0;
                foreach (EventOpCode opCode in manaEvent.OpCodes)
                {
                    if (opCodeType == opCode.OperationCode)
                    {
                        string info = $"Operation Code Index: {lineIndex:X2}, Operation Code: {opCode.OperationCode.GetDisplayName()}";
                        results.Add(new AnalyzerResult(AnalyzerReferenceType.Event, index, lineIndex, manaEvent, info));
                    }
                    lineIndex++;
                }

                index++;
            }

            return results;
        }
    }

    public sealed class AnalyzerResult
    {
        public AnalyzerReferenceType ReferenceType { get; } = AnalyzerReferenceType.None;
        public uint ReferenceIndex { get; } = 0;
        public uint SubIndex { get; } = 0;
        public object? ReferenceObject { get; } = null;
        public string AdditionalInformation { get; } = string.Empty;
        public Bounds8 ReferenceRange { get; } = Bounds8.Empty;

        internal AnalyzerResult(AnalyzerReferenceType type, uint index, uint subIndex, object? refObject) : this(type, index, subIndex, refObject, string.Empty) { }

        internal AnalyzerResult(AnalyzerReferenceType type, uint index, uint subIndex, object? refObject, string additionalInfo) : this(type, index, subIndex, refObject, additionalInfo, Bounds8.Empty) { }

        internal AnalyzerResult(AnalyzerReferenceType type, uint index, uint subIndex, object? refObject, string additionalInfo, Bounds8 bounds)
        {
            this.ReferenceType = type;
            this.ReferenceIndex = index;
            this.SubIndex = subIndex;
            this.ReferenceObject = refObject;
            this.AdditionalInformation = additionalInfo;
            this.ReferenceRange = bounds;
        }
    }

    public enum AnalyzerReferenceType
    {
        None,
        Event,
        EventFlag,

        HardcodedEvent,
        HardcodedEventFlag,

        MapLayer1Background,
        MapLayer2Background,
        MapLayer1Object,
        MapLayer2Object,
        MapTrigger,
        Collision,
        SpriteObject,
    }
}