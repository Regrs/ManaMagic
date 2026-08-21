using System;
using System.Collections.Generic;
using System.ComponentModel;
using ZwellTech;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record MapCollision
    {
        public byte Byte1 { get; }
        public byte Byte2 { get; }
        public byte Byte3 { get; }
        public byte Byte4 { get; }

        public MapCollisionType CollisionType { get; }
        public TileCollisionTypes TileTypes { get; }
        public TileCollisionOptions TileOptions { get; }
        public TileCollisionAction InteractAction { get; }
        public bool IsEvent { get { return this.InteractAction == TileCollisionAction.ExecuteEvent; } }
        public bool IsTileReplacer { get { return this.InteractAction == TileCollisionAction.ReplaceTile; } }
        public ushort EventIndex { get; }
        public byte ReplaceTileIndex { get; }

        public MapCollision(MapCollisionType type, byte b1, byte b2, byte b3, byte b4)
        {
            this.Byte1 = b1;
            this.Byte2 = b2;
            this.Byte3 = b3;
            this.Byte4 = b4;

            this.CollisionType = type;
            this.TileTypes = (TileCollisionTypes)b1;
            this.TileOptions = (TileCollisionOptions)b2;
            this.InteractAction = TileCollisionAction.None;
            if ((b3 & 0B_1000_0000) > 0)
            {
                this.InteractAction = ((b3 & 0B_0011_0000) > 0) ? TileCollisionAction.ExecuteEvent : TileCollisionAction.ReplaceTile;
            }
            if (this.IsEvent) //1300 FC1E [72: Executes Event 1EC]
            {
                this.EventIndex = (ushort)((b4 << 4) | (b3 & 0x0F));
            }
            else if (this.IsTileReplacer)
            {
                this.ReplaceTileIndex = b4;
            }

            // 80: 1000_0000
            // C0: 1100_0000
            // C2: 1100_0010
            // D0: 1101_0000
            // F0: 1111_0000

            // Byte 01
            // $7E:E00B - > (B1 & 0x03) | $7E:E00B

            // Byte 02
            // $7E:E08E -> B2 & 0x07
        }
    }

    [Flags]
    public enum TileCollisionTypes : byte
    {
        None = 0x00,
        Unknown01 = 0x01,
        EntireTile = 0x02,
        Unknown04 = 0x04,
        Unknown08 = 0x08,
        Unknown10 = 0x10,
        Unknown20 = 0x20,
        Unknown40 = 0x40,
        Unknown80 = 0x80,
    }

    [Flags]
    public enum TileCollisionOptions : byte
    {
        None = 0x00,
        Unknown01 = 0x01,
        HalfPriorityOverSprite = 0x02,
        PriorityOverSprite = 0x04,
        Unknown08 = 0x08,
        ReturnTile = 0x10,
        Unknown20 = 0x20,
        SpecialEventTile = 0x40,
        TriggerTile = 0x80,
    }

    public enum TileCollisionAction : byte
    {
        None = 0x00,
        ReplaceTile = 0x01,
        ExecuteEvent = 0x02
    }

    public enum MapCollisionType : byte
    {
        CollisionType00 = 0x00,
        CollisionType01 = 0x01,
        CollisionType02 = 0x02,
        CollisionType03 = 0x03,
        CollisionType04 = 0x04,
        CollisionType05 = 0x05,
        CollisionType06 = 0x06,
        CollisionType07 = 0x07,
        CollisionType08 = 0x08,
        CollisionType09 = 0x09,
        CollisionType0A = 0x0A,
        CollisionType0B = 0x0B,
        CollisionType0C = 0x0C,
        CollisionType0D = 0x0D,
        CollisionType0E = 0x0E,
        CollisionType0F = 0x0F,

        CollisionType10 = 0x10,
        CollisionType11 = 0x11,
        CollisionType12 = 0x12,
        CollisionType13 = 0x13,
        CollisionType14 = 0x14,
        CollisionType15 = 0x15,
        CollisionType16 = 0x16,
        CollisionType17 = 0x17,
        CollisionType18 = 0x18,
        CollisionType19 = 0x19, // Stairs
        CollisionType1A = 0x1A,
        CollisionType1B = 0x1B,
        CollisionType1C = 0x1C,
        CollisionType1D = 0x1D,
        CollisionType1E = 0x1E,
        CollisionType1F = 0x1F,

        CollisionType20 = 0x20,
        CollisionType21 = 0x21,
        CollisionType22 = 0x22,
        CollisionType23 = 0x23,
        CollisionType24 = 0x24,
        CollisionType25 = 0x25,
        CollisionType26 = 0x26,
        CollisionType27 = 0x27,
        CollisionType28 = 0x28,
        CollisionType29 = 0x29,
        CollisionType2A = 0x2A,
        CollisionType2B = 0x2B,
        CollisionType2C = 0x2C,
        CollisionType2D = 0x2D,
        CollisionType2E = 0x2E,
        CollisionType2F = 0x2F,

        CollisionType30 = 0x30,
        CollisionType31 = 0x31,
        CollisionType32 = 0x32,
        CollisionType33 = 0x33,
        CollisionType34 = 0x34,
        CollisionType35 = 0x35,
        CollisionType36 = 0x36,
        CollisionType37 = 0x37,
        CollisionType38 = 0x38,
        CollisionType39 = 0x39,
        CollisionType3A = 0x3A,
        CollisionType3B = 0x3B,
        CollisionType3C = 0x3C,
        CollisionType3D = 0x3D,
        CollisionType3E = 0x3E,
        CollisionType3F = 0x3F,

        CollisionType40 = 0x40,
        CollisionType41 = 0x41,
        CollisionType42 = 0x42,
        CollisionType43 = 0x43,
        CollisionType44 = 0x44,
        CollisionType45 = 0x45,
        CollisionType46 = 0x46,
        CollisionType47 = 0x47,
        CollisionType48 = 0x48,
        CollisionType49 = 0x49,
        CollisionType4A = 0x4A,
        CollisionType4B = 0x4B,
        CollisionType4C = 0x4C,
        CollisionType4D = 0x4D,
        [FieldDisplayName("Plains/Forest Exterior - Cuttable Grass")]
        PlainsForestExteriorCuttableGrass = 0x4E,
        [FieldDisplayName("Plains/Forest Exterior - Cuttable Grass Bottom")]
        PlainsForestExteriorCuttableGrassBottom = 0x4F,

        CollisionType50 = 0x50,                       // Unused.
        [FieldDisplayName("Haunted Forest - Cuttable Left-Facing Flower")]
        HauntedForestCuttableLeftFacingFlower = 0x51,
        [FieldDisplayName("Haunted Forest - Cuttable Right-Facing Flower")]
        HauntedForestCuttableRightFacingFlower = 0x52,
        [FieldDisplayName("Cave Interior - Axeable Rock")]
        CaveInteriorAxeableRock = 0x53,
        CollisionType54 = 0x54,                       // Unused.
        CollisionType55 = 0x55,                       // Unused.
        [FieldDisplayName("Snow Plains/Forest Exterior - Cuttable Bush")]
        SnowPlainsForestExteriorCuttableBush = 0x56,
        [FieldDisplayName("Plains Exterior - Cuttable Bush")]
        PlainsExteriorCuttableBush = 0x57,
        [FieldDisplayName("Plains Exterior - Cuttable Flower")]
        PlainsExteriorCuttableFlower = 0x58,
        [FieldDisplayName("Plains Exterior - Statue")]
        PlainsExteriorStatue = 0x59,
        [FieldDisplayName("Plains Exterior - Tongue Statue")]
        PlainsExteriorTongueStatue = 0x5A,
        CollisionType5B = 0x5B,                       // Unused.
        CollisionType5C = 0x5C,                       // Unused.
        [FieldDisplayName("Grand Palace Sand Interior - Blue Axeable Rocks")]
        GrandPalaceSandInteriorBlueAxeableRocks = 0x5D,
        [FieldDisplayName("Grand Palace Sand Interior - Flat Red Axeable Rocks")]
        GrandPalaceSandInteriorFlatRedAxeableRocks = 0x5E,
        [FieldDisplayName("Grand Palace Sand Interior - Red Axeable Rocks")]
        GrandPalaceSandInteriorRedAxeableRocks = 0x5F,

        [FieldDisplayName("Grand Palace Sand Interior - Backwards L-Shaped Red Axeable Rocks")]
        GrandPalaceSandInteriorBackwardsLShapedRedAxeableRocks = 0x60,
        [FieldDisplayName("Grand Palace Sand Interior - L-Shaped Red Axeable Rocks")]
        GrandPalaceSandInteriorLShapedRedAxeableRocks = 0x61,
        [FieldDisplayName("Grand Palace Sand Interior - Backwards Upside Down L-Shaped Red Axeable Rocks")]
        GrandPalaceSandInteriorBackwardsUpsideDownLShapedRedAxeableRocks = 0x62,
        [FieldDisplayName("Grand Palace Sand Interior - Upside Down L-Shaped Red Axeable Rocks")]
        GrandPalaceSandInteriorUpsideDownLShapedRedAxeableRocks = 0x63,
        [FieldDisplayName("Cave Interior - Axeable Stalagmite")]
        CaveInteriorAxeableStalagmite = 0x64,
        [FieldDisplayName("Pure Land Exterior - Cuttable Bush")]
        PureLandExteriorCuttableBush = 0x65,
        [FieldDisplayName("Mana Fortress Interior - Cuttable Crystal Stage 1")]
        ManaFortressInteriorCuttableCrystalStage1 = 0x66,
        [FieldDisplayName("Mana Fortress Interior - Cuttable Crystal Stage 2")]
        ManaFortressInteriorCuttableCrystalStage2 = 0x67,
        [FieldDisplayName("Mana Fortress Interior - Cuttable Crystal Stage 3")]
        ManaFortressInteriorCuttableCrystalStage3 = 0x68,
        CollisionType69 = 0x69,
        CollisionType6A = 0x6A,
        CollisionType6B = 0x6B,
        CollisionType6C = 0x6C,
        [FieldDisplayName("Whip Post")]
        WhipPost = 0x6D,
        [FieldDisplayName("Cave Interior - Blank Tile (Gaia's Navel Controller)")]
        CaveInteriorBlankTileGaiasNavelController = 0x6E,
        CollisionType6F = 0x6F,                       // Unused.

        CollisionType70 = 0x70,                       // Unused.
        [FieldDisplayName("Seed Palace - Wall Switch")]
        SeedPalaceWallSwitch = 0x71,
        [FieldDisplayName("Haunted Forest - Skull Head")]
        HauntedForestSkullHead = 0x72,
        [FieldDisplayName("Ship Interior - Wall Switch - Up Position (Dummied Out)")]
        ShipInteriorWallSwitchUpPosition = 0x73,      // Unused.
        [FieldDisplayName("Ship Interior - Wall Switch - Down Position (Dummied Out)")]
        ShipInteriorWallSwitchDownPosition = 0x74,    // Unused.
        [FieldDisplayName("Castle Interior - Blank Tile - Wall Switch")]
        CastleInteriorBlankTileWallSwitch = 0x75,
        [FieldDisplayName("Castle Interior - Blank Tile - Door Controller (Dummied Out)")]
        CastleInteriorBlankTileDoorController = 0x76, // Unused.
        CollisionType77 = 0x77,                       // Unused.
        [FieldDisplayName("Grand Palace Sand Interior - Left Wall Switch")]
        GrandPalaceSandInteriorLeftWallSwitch = 0x78,
        [FieldDisplayName("Grand Palace Sand Interior - Right Wall Switch")]
        GrandPalaceSandInteriorRightWallSwitch = 0x79,
        [FieldDisplayName("Grand Palace Interior - Red Switch")]
        GrandPalaceInteriorRedSwitch = 0x7A,
        [FieldDisplayName("Grand Palace Interior - Blue Switch")]
        GrandPalaceInteriorBlueSwitch = 0x7B,
        [FieldDisplayName("Grand Palace Interior - Yellow Switch")]
        GrandPalaceInteriorYellowSwitch = 0x7C,
        [FieldDisplayName("Grand Palace Interior - Green Switch")]
        GrandPalaceInteriorGreenSwitch = 0x7D,
        Invalid7E = 0x7E,
        Invalid7F = 0x7F,
    }

    public sealed class MapCollisionSet
    {
        public static MapCollisionSet Empty { get; } = new MapCollisionSet(DataTable<MapCollisionType>.Empty, DataTable<MapCollisionType>.Empty);

        public DataTable<MapCollisionType> Layer1 { get; }
        public DataTable<MapCollisionType> Layer2 { get; }

        public MapCollisionSet(DataTable<MapCollisionType> layer1, DataTable<MapCollisionType> layer2)
        {
            this.Layer1 = layer1;
            this.Layer2 = layer2;
        }

        public MapCollisionSet(IReadOnlyList<MapCollisionType> layer1, IReadOnlyList<MapCollisionType> layer2)
        {
            this.Layer1 = new DataTable<MapCollisionType>(layer1);
            this.Layer2 = new DataTable<MapCollisionType>(layer2);
        }
    }
}

/*

Trigger Collision Types via SomEdit
CB/0044:	1080 2300 [11: ]
CB/0048:	1087 2300 [12: ]
CB/0058:	1010 0000 [16: ]

CB/0064:	10 00 21 00 [19: ] - Stairs

4E - Plains/Forest Exterior - Cuttable Grass
4F - Plains/Forest Exterior - Cuttable Grass Bottom
50 - Dummied Out
51 - Haunted Forest - Cuttable Left-Facing Flower
52 - Haunted Forest - Cuttable Right-Facing Flower
53 - Cave Interior - Axeable Rock
54 - Dummied Out
55 - Dummied Out
56 - Snow Plains/Forest Exterior - Cuttable Bush
57 - Plains Exterior - Cuttable Bush
58 - Plains Exterior - Cuttable Flower
59 - Plains Exterior - Statue
5A - Plains Exterior - Tongue Statue
5B - Dummied Out
5C - Dummied Out - Sprite Villiage Tileset (0x05) has a deleted tile matching the swap index.
5D - Grand Palace Sand Interior - Blue Axeable Rocks
5E - Grand Palace Sand Interior - Flat Red Axeable Rocks
5F - Grand Palace Sand Interior - 4x Red Axeable Rocks
60 - Grand Palace Sand Interior - 3x (Backwards L) Red Axeable Rocks
61 - Grand Palace Sand Interior - 3x (L-Shaped) Red Axeable Rocks
62 - Grand Palace Sand Interior - 3x (Backwards Upside Down L) Red Axeable Rocks
63 - Grand Palace Sand Interior - 3x (Upside Down L-Shaped) Red Axeable Rocks
64 - Cave Interior - Axeable Stalagmite
65 - Pure Land Exterior - Cuttable Bush
66 - Mana Fortress Interior - Cuttable Crystal Stage 1
67 - Mana Fortress Interior - Cuttable Crystal Stage 2
68 - Mana Fortress Interior - Cuttable Crystal Stage 3

6D - Whip Posts
6E - Cave Interior - Blank Tile (Gaia's Navel Controller)
6F - Unused
70 - Unused
71 - Seed Palace - Wall Switch
72 - Haunted Forest - Skull Head
73 - Unused - Ship Interior Wall Switch - Up Position
74 - Unused - Ship Interior Wall Switch - Down Position
75 - Castle Interior - Blank Tile (Wall Switch)
76 - Castle Interior - Blank Tile (Dummied Out) (Seems meant to open a door)
77 - Unused
78 - Grand Palace Sand Interior - Left Wall Switch
79 - Grand Palace Sand Interior - Right Wall Switch
7A - Grand Palace Interior - Red Switch
7B - Grand Palace Interior - Blue Switch
7C - Grand Palace Interior - Yellow Switch
7D - Grand Palace Interior - Green Switch

; 00, 04, 05, and 17 are the general purpose collisions. The rest are used only rarely.
;  Byte 00: Collision Type
     02: Full Collision
;  Byte 01: Tile Options
     02: Tile covers lower half of sprites.
     04: Tile covers sprites.
     10: "Return" Tile.
     40: Special Event Tile
     80: Trigger Tile
; Byte 03: Special Action
     20: Stairs?
     C0: Glove
     C1: Sword
     C2: 
     C8: Sword + Axe
     D0: Event Execute
     F0: Event Execute
; Byte 04: - Variable Data


C1: 1100_0001 (80, 40, 01) [Sword]
C8: 1100_1000 (80, 40, 08) [Sword/Axe]


CB/0000:	1206 0000 [00: ]
12: 0001_0010 (10, 02)
06: 0000_0110 (04, 02)

CB/0004:	1046 0000 [01: Executes A Map's "Special Event" when stepped on]
10: 0001_0000 (10)
46: 0100_0110 (40, 04, 02)

CB/0010:	1306 0000 [04: ]
13: 0001_0011 (10, 02, 01)
06: 0000_0110 (04, 02)

CB/0014:	7306 0000 [05: ]
73: 0111_0011 (40, 20, 10, 02, 01)
06: 0000_0110 (04, 02)

CB/005C:	1207 0000 [17: ]
12: 0001_0010 (10, 02)
07: 0000_0111 (04, 02, 01)

CB/0138:	1202 C100 [4E: Destroy W/Sword]
12: 0001_0010 (10, 02)
02: 0000_0010 (04, 02)
C1: 1100_0001 (80, 40, 01)

CB/0144:	1300 C87A [51: Destroy W/Sword & Axe]
13: 0001_0011 (10, 02, 01)
00: 0000_0000 (00)
C8: 1100_1000 (80, 40, 08)
7A: TileID?

CB/0174:	2300 C238 [5D: Destroy W/Axe]
23: 0010_0011 (20, 02, 01)
00: 0000_0000 (00)
C2: 1100_0010 (80, 40, 02)
38: TileID?

CB/01B4:	1307 F86F [6D: Executes Event 6F8 When Attacked With A Whip]
13: 0001_0011 (10, 02, 01)
07: 0000_0111 (04, 02, 01)
F8: 1111_1000 (80, 40, 20, 10, 08)

CB/01C8:	1300 FC1E [72: Executes Event 1EC]
13: 0001_0011 (10, 02, 01)
00: 0000_0000 (00)
FC: 1111_1100 (80, 40, 20, 10, 08, 04)

CB/01D8:	1000 D06F [76: Executes Event 6F0]
10: 0001_0000 (10)
00: 0000_0000 (00)
D0: 1101_0000 (80, 40, 10)
 */