using System;
using System.Collections.Generic;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.System;
using ManaMagic.Core.Maps;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Analyzer
{
    public sealed class ManaReferenceCounter
    {
        private readonly Dictionary<ushort, int> mapPieceUsageCount = new Dictionary<ushort, int>();
        private readonly Dictionary<MapCollisionType, int> mapCollisionTypeUsageCount = new Dictionary<MapCollisionType, int>();
        private readonly Dictionary<byte, int> mapPaletteUsageCount = new Dictionary<byte, int>();
        private readonly Dictionary<byte, int> spriteUsageCount = new Dictionary<byte, int>();
        private readonly Dictionary<ushort, int> eventUsageCount = new Dictionary<ushort, int>();

        public IReadOnlyDictionary<ushort, int> MapPieceUsageCount { get { return this.mapPieceUsageCount; } }
        public IReadOnlyDictionary<MapCollisionType, int> MapCollisionTypeUsageCount { get { return this.mapCollisionTypeUsageCount; } }
        public IReadOnlyDictionary<byte, int> MapPaletteUsageCount { get { return this.mapPaletteUsageCount; } }
        public IReadOnlyDictionary<byte, int> SpriteUsageCount { get { return this.spriteUsageCount; } }
        public IReadOnlyDictionary<ushort, int> EventUsageCount { get { return this.eventUsageCount; } }

        public void RunUsageCounters(SecretOfManaContext context)
        {
            this.RunMapPieceUsageCounter(context);
            this.RunMapCollisionUsageCounter(context);
            this.RunMapPaletteUsageCounter(context);
            this.RunSpriteUsageCounter(context);
            this.RunEventUsageCounter(context);
        }

        public void RunMapPieceUsageCounter(SecretOfManaContext context)
        {
            this.mapPieceUsageCount.Clear();
            for (ushort i = 1; i <= Constants.Bank0D.MapPiecePointerTableSize; i++)
            {
                this.mapPieceUsageCount.Add(i, 0);
            }

            foreach (MapObjectTable objectTable in context.MapContext.MapObjectTable)
            {
                if (objectTable.Layer1Background.Index != 0) { this.mapPieceUsageCount[objectTable.Layer1Background.Index]++; }

                foreach (MapObject mapObject in objectTable.Layer1)
                {
                    if (mapObject.Index != 0)
                    {
                        this.mapPieceUsageCount[mapObject.Index]++;
                    }
                }
                if (objectTable.HasLayer2)
                {
                    this.mapPieceUsageCount[objectTable.Layer2Background.Index]++;
                    foreach (MapObject mapObject in objectTable.Layer2)
                    {
                        this.mapPieceUsageCount[mapObject.Index]++;
                    }
                }
            }
        }

        private void RunMapCollisionUsageCounter(SecretOfManaContext context)
        {
            this.mapCollisionTypeUsageCount.Clear();
            Array.ForEach(Enum.GetValues<MapCollisionType>(), item => this.mapCollisionTypeUsageCount.Add(item, 0));
            this.mapCollisionTypeUsageCount.Add((MapCollisionType)0xFF, 0); // For the dummied out tileset.

            foreach (MapCollisionSet collisionSet in context.MapContext.MapTilesetCollisionTable)
            {
                foreach (MapCollisionType layer1Type in collisionSet.Layer1)
                {
                    this.mapCollisionTypeUsageCount[layer1Type]++;
                }
                foreach (MapCollisionType layer2Type in collisionSet.Layer2)
                {
                    this.mapCollisionTypeUsageCount[layer2Type]++;
                }
            }
        }

        private void RunMapPaletteUsageCounter(SecretOfManaContext context)
        {
            this.mapPaletteUsageCount.Clear();
            for (byte i = 0; i < Constants.Bank0C.MapPaletteSetTableSize; i++)
            {
                this.mapPaletteUsageCount.Add(i, 0);
            }
            foreach (MapHeader header in context.MapContext.MapHeaderTable)
            {
                if (header.IsValid) { this.mapPaletteUsageCount[header.PaletteSetIndex]++; }
            }
        }

        private void RunSpriteUsageCounter(SecretOfManaContext context)
        {
            this.spriteUsageCount.Clear();
            for (int i = 0; i < Constants.Bank10.SpriteGraphicsTableSize; i++)
            {
                this.spriteUsageCount.Add((byte)i, 0);
            }
            foreach (MapHeader header in context.MapContext.MapHeaderTable)
            {
                foreach (MapSpriteObject spriteObject in header.ObjectTable)
                {
                    this.spriteUsageCount[spriteObject.SpriteIndex]++;
                }
            }
        }

        private void RunEventUsageCounter(SecretOfManaContext context)
        {
            this.eventUsageCount.Clear();
            for (ushort index = 0; index <= Constants.Bank0A.EventIdMaximum; index++)
            {
                this.eventUsageCount.Add(index, 0);
                foreach (ManaEvent manaEvent in context.EventContext.Events)
                {
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
                                if (opCode is JumpToEventOpCode jumpOpCode && jumpOpCode.EventId == index)
                                {
                                    this.eventUsageCount[index]++;
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
                                if (opCode is CallEventOpCode callOpCode && callOpCode.EventId == index)
                                {
                                    this.eventUsageCount[index]++;
                                }
                                break;
                        }
                    }
                }
            }

            foreach (MapHeader header in context.MapContext.MapHeaderTable)
            {
                foreach (MapSpriteObject spriteObject in header.ObjectTable)
                {
                    this.eventUsageCount[spriteObject.EventIndex]++;
                }
            }

            foreach (DataTable<MapTrigger> triggerTable in context.MapContext.MapTriggersTable)
            {
                foreach (MapTrigger trigger in triggerTable)
                {
                    if (trigger.IsEvent)
                    {
                        this.eventUsageCount[trigger.Value]++;
                    }
                }
            }

            foreach (MapCollision collision in context.MapContext.MapCollisionDefinitionTable)
            {
                if (collision.IsEvent && collision.CollisionType != MapCollisionType.Invalid7E && collision.CollisionType != MapCollisionType.Invalid7F)
                {
                    this.eventUsageCount[collision.EventIndex]++;
                }
            }
        }
    }
}