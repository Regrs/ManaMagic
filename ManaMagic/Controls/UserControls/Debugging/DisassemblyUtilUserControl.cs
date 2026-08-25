using System;
using System.Text;
using System.Windows.Forms;
using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;

#nullable enable
#pragma warning disable IDE1006 // Naming Styles

namespace ManaMagic.Controls.UserControls.Debug
{
    public partial class DisassemblyUtilUserControl : UserControl
    {
        public DisassemblyUtilUserControl()
        {
            InitializeComponent();
        }

//#pragma warning disable IDE0051 // Remove unused private members
        private void DisassembleMapData()
//#pragma warning restore IDE0051 // Remove unused private members
        {
            StringBuilder sb = new StringBuilder();
            MapDisassemblyRomReader romReader = RomReaderFactory.GetRomReader<MapDisassemblyRomReader>();

            sb.Append(romReader.ReadMapLayerDefinitionPointerTable());

            sb.AppendLine();
            sb.AppendLine("------------------------------------------------------------------------------------------");
            sb.AppendLine();

            sb.Append(romReader.ReadMapLayerData());
            //sb.Append(romReader.ReadAnimationTileTable());

            mapLayerDataRichTextBox.Text = sb.ToString();
            sb.Clear();

            sb.Append(romReader.ReadMapHeaderNPCPointerTable());

            sb.AppendLine();
            sb.AppendLine("------------------------------------------------------------------------------------------");
            sb.AppendLine();

            sb.Append(romReader.ReadMapHeaderAndNpcData());

            mapHeaderNpcDataRichTextBox.Text = sb.ToString();
            sb.Clear();

            //Dictionary<byte, int> npcRefCounts = romReader.ReadNpcReferenceCount();
            //for (int i = 0; i < MapDisassemblyRomReader.SpriteNameStrings.Count; i++)
            //{
            //    sb.Append(MapDisassemblyRomReader.SpriteNameStrings[(byte)i]).Append(": ").AppendLine(npcRefCounts[(byte)i].ToString());
            //}

            //mapSpriteCountRichTextBox.Text = sb.ToString();
            //sb.Clear();

            //Dictionary<ushort, int> mapPieceRefCounts = romReader.ReadMapPieceReferenceCount();
            //for (int i = 1; i <= Constants.Bank0D.MapPiecePointerTableSize; i++)
            //{
            //    sb.Append(i.ToString("X2")).Append(": ").AppendLine(mapPieceRefCounts[(ushort)i].ToString());
            //}

            //mapPieceCountRichTextBox.Text = sb.ToString();
            //sb.Clear();

            //foreach (KeyValuePair<byte, int> kvp in SecretOfManaContext.Default.ReferenceCounts.MapPaletteUsageCount)
            //{
            //    sb.AppendLine($"[{kvp.Key:X2}]: {kvp.Value}");
            //}
            //mapPieceCountRichTextBox.Text = sb.ToString();

            //Dictionary<byte, int> collisionRefCounts = romReader.ReadCollisionReferenceCount();
            //for (int i = 0; i < collisionRefCounts.Count; i++)
            //{
            //    sb.Append(i.ToString("X2")).Append(": ").AppendLine(collisionRefCounts[(byte)i].ToString());
            //}

            //mapCollisionCountRichTextBox.Text = sb.ToString();
        }

        private void DisassembleWeaponPaletteData()
        {
            StringBuilder sb = new StringBuilder();
            GenericRomReader reader = RomReaderFactory.GetRomReader<GenericRomReader>();

            reader.Seek(0x11F600);

            // Not sure how big this palette table actually is...its at least B0.
            // Well, FF fits perfectly to the end of the bank.
            for (int i = 0; i <= 0xFF; i++)
            {
                sb.Append(reader.Position.ToString("X6")).Append(": ");
                for (int colorIndex = 0; colorIndex < 5; colorIndex++)
                {
                    sb.Append(reader.Read().ToString("X2")).Append(reader.Read().ToString("X2")).Append(" ");
                }
                sb.AppendLine($" [{i:X2}: ]");
            }
            mapLayerDataRichTextBox.Text = sb.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.DisassembleWeaponPaletteData();
            this.DisassembleMapData();
        }
    }
}
/* 18E: Matches Tileset 0x06 (Grassy Interior)
 *      - Is a shop interior.
 * 
 * 1E2: Map Piece E0: Matches Tileset 0x09 (Ruins Interior) for Layer 2.
 *                   - Also set to Layer 1, but no tileset lines up.
 *                   - Apparently one of the altar rooms.
 * 
 * 1E4: Map Piece 8D - Matches Tileset 0x12 (Ship Exterior) for Layer 1.
 *                   - Also set to Layer 2, but no tileset lines up.
 *                   - Apparently a copy of the outside Scorpion Army Ship.
 *                   
 * 1E5: Duplicate of above.
 * 
 * 1E6: Okay, this one is weird.
 *      Map Piece 145 - Matches Tileset 0x01 (Plains Exterior) for Layer 1.
 *                    - This is a large lake bed.
 *      Map Piece 146 - Matches Tileset 0x01 (Plains Exterior) for Layer 1, but is set to Layer 2, so is always garbage.
 *                    - This is a long pathway with trees and rocks. It in no way goes with the first layer.
 *                    
 * 1E7: Duplicate of above.
 * 
 * mapHeaderNpcDataRichTextBox
[00: Debug Map - Corrupt Kippo Village A (Dummied Out)]
C8/5402: FE             [Layer 1 Objects]
         FG RG ID XC YC
C8/5403: 00 0F 4D 00 00 [00: ]
C8/5408: FF             [Layer 2 Objects]
         FG RG ID XC YC
C8/5409: 00 0F 4E 00 00 [00: ]

[00: ]
08/5402: FE [Layer 1 Objects]
         FG RG ID XC YC
08/5403: 00 0F 4D 00 00
08/5408: FF [Layer 2 Objects]
         FG RG ID XC YC
08/5409: 00 0F 4E 00 00


[0F: ]
        T8 PL T6 SI SE DS ?? P2
089018: 00 18 80 04 E0 00 00 0C

        Fl Rg XC YC DR Tp Evnt
089020: 00 0F 22 96 40 BD 0044 [00: ]
089028: 00 0F 26 96 90 C0 0114 [01: ]
089030: 00 0F 2A 96 00 C1 0224 [02: ]
089038: 00 0F 2E 96 D0 AE 0334 [03: ]
089040: 00 0F 22 9E 40 B6 0454 [04: ]
089048: 00 0F 26 9E 90 B7 0554 [05: ]
089050: 0D 00 AA 1E 60 8B 8EC0 [06: ]
089058: 0E 00 AE 1E 70 8C 8FC0 [07: ]
089060: 00 0F 3C 9F 40 EE 0080 [08: ]

C8/53C0:	7488		[1E0]
C8/53C2:	7488		[1E1]
C8/53C4:	7488		[1E2]
C8/53C6:	8088		[1E3]
C8/53C8:	8088		[1E4]
C8/53CA:	8C88		[1E5]
C8/53CC:	9888		[1E6]
C8/53CE:	A488		[1E7]

*/