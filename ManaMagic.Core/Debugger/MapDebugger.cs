using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Metadata;
using ZwellTech;
using ZwellTech.Logging;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Debugger
{
    internal static class MapDebugger
    {
        public static SecretOfManaContext? Context { get; set; } = null;

        [MemberNotNullWhen(true, nameof(MapDebugger.Context))]
        public static bool CanDebug { get { return MapDebugger.Context != null; } }

        [Conditional("DEBUG")]
        public static void RunDebugger(MapDebuggerOption option)
        {
            switch (option)
            {
                case MapDebuggerOption.DrawAllMaps:
                    MapDebugger.DrawAllMaps();
                    break;
                case MapDebuggerOption.ScanForTileIndex:
                    MapDebugger.ScanForTileIndex();
                    break;
                case MapDebuggerOption.PrintSpiteActions:
                    MapDebugger.PrintSpiteActions();
                    break;
                case MapDebuggerOption.CountLayerSettings:
                    MapDebugger.CountLayerSettings();
                    break;
                case MapDebuggerOption.SpriteUnknownBits:
                    MapDebugger.SpriteUnknownBits();
                    break;
                case MapDebuggerOption.MapUnknownByte:
                    MapDebugger.MapUnknownByte();
                    break;
                case MapDebuggerOption.ScanForF8FFCommands:
                    MapDebugger.ScanForF8FFCommands();
                    break;
                case MapDebuggerOption.PrintSpecialItem20:
                    MapDebugger.PrintSpecialItem20();
                    break;
                case MapDebuggerOption.PrintSpecialItem40:
                    MapDebugger.PrintSpecialItem40();
                    break;
                case MapDebuggerOption.ScanForDisplaySettingsByteBits:
                    MapDebugger.ScanForDisplaySettingsByteBits();
                    break;
                case MapDebuggerOption.PrintMaxSpriteCount:
                    MapDebugger.PrintMaxSpriteCount();
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("Unknown debugger option", nameof(option));
                    break;
            }
        }

        [Conditional("DEBUG")]
        private static void DrawAllMaps()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            _ = Task.Run(() =>
            {
                MapDrawingOptions options = new MapDrawingOptions(true, true, true, true, true);
                for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
                {
                    try
                    {
                        ManaMap map = MapDebugger.Context.MapContext.CreateMap(i);
                        try
                        {
                            if (map.IsValid)
                            {
                                using SuperNintendoGraphics graphics = map.DrawMap(options, MapDebugger.Context.SpriteContext, -1);
                                continue;
                            }

                            if (!map.Header.IsValid && !map.ObjectTable.IsValid)
                            {
                                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {i:X4}. Header and Object Table are invalid.");
                            }
                            else if (!map.Header.IsValid)
                            {
                                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {i:X4}. Header is invalid.");
                            }
                            else if (!map.ObjectTable.IsValid)
                            {
                                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {i:X4}. Object Table is invalid.");
                            }
                            else
                            {
                                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {i:X4}.");
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {i:X4}. Exception: {ex.GetType()}, Message: {ex.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot create map index {i:X4}. Exception: {ex.GetType()}, Message: {ex.Message}");
                    }
                }
            });
        }

        [Conditional("DEBUG")]
        private static void ScanForTileIndex()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            _ = Task.Run(() =>
            {
                for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
                {
                    ManaMap map = MapDebugger.Context.MapContext.CreateMap(i);
                    if (map.IsValid)
                    {
                        //if (DoScan(map, i, 0x01))
                        //{
                        //DoTileScan(map, i, 0xAB, false);
                        //}
                        if (DoScan(map, i, 0x0A))
                        {
                            DoTileScan(map, i, 0x80, false);
                        }
                        //if (DoScan(map, i, 0x0F))
                        //{
                        //DoTileScan(map, i, 0xAE, false);
                        //DoTileScan(map, i, 0xAF, false);
                        //}
                    }
                }
                LoggerEngine.Logger.LogDebug(LogComponent.General, $"Tile ID Scan Complete.");
            });

            static bool DoScan(ManaMap map, int mapIndex, byte tileset16Index)
            {
                if (map.Header.Tileset16x16Index == tileset16Index)
                {
                    LoggerEngine.Logger.LogDebug(LogComponent.General, $"Map index {mapIndex:X2} uses Tileset {tileset16Index:X2}.");
                    return true;
                }
                return false;
            }

            static void DoTileScan(ManaMap map, int mapIndex, byte tileID, bool layer2)
            {
                if (!layer2)
                {
                    if (map.Layer1Background.TileIdTable.Contains(tileID))
                    {
                        LoggerEngine.Logger.LogDebug(LogComponent.General, $"Map index {mapIndex:X2} uses Tile {tileID:X2} On Layer 1 Background.");
                    }
                    foreach (MapPiece piece in map.Layer1)
                    {
                        if (piece.TileIdTable.Contains(tileID))
                        {
                            LoggerEngine.Logger.LogDebug(LogComponent.General, $"Map index {mapIndex:X2} uses Tile {tileID:X2} On Map Piece: {piece.Index:X4}.");
                        }
                    }
                    return;
                }

                if (map.Layer2Background.TileIdTable.Contains(tileID))
                {
                    LoggerEngine.Logger.LogDebug(LogComponent.General, $"Map index {mapIndex:X2} uses Tile {tileID:X2} On Layer 2 Background.");
                }
                foreach (MapPiece piece in map.Layer2)
                {
                    if (piece.TileIdTable.Contains(tileID))
                    {
                        LoggerEngine.Logger.LogDebug(LogComponent.General, $"Map index {mapIndex:X2} uses Tile {tileID:X2} On Map Piece: {piece.Index:X4}.");
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        private static void PrintSpiteActions()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            int index = 0;
            foreach (MapHeader header in MapDebugger.Context.MapContext.MapHeaderTable)
            {
                foreach (MapSpriteObject spriteObject in header.ObjectTable)
                {
                    LoggerEngine.Logger.LogDebug(LogComponent.General, $"[{index:X4}]: {spriteObject.Name} - {spriteObject.Interactions}");
                }
                index++;
            }
        }

        [Conditional("DEBUG")]
        private static void CountLayerSettings()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            Dictionary<int, int> counts = new Dictionary<int, int>();
            for (int i = 0; i <= 0x13; i++)
            {
                counts.Add(i, 0);
            }
            ushort index = 0;
            foreach (MapHeader header in MapDebugger.Context.MapContext.MapHeaderTable)
            {
                counts[header.LayerScrollSettingsIndex]++;
                if (header.LayerScrollSettingsIndex == 0x0A ||
                    header.LayerScrollSettingsIndex == 0x0B ||
                    header.LayerScrollSettingsIndex == 0x11 ||
                    header.LayerScrollSettingsIndex == 0x12 ||
                    header.LayerScrollSettingsIndex == 0x13)
                {
                    ManaDebugger.DebugPrint($"{header.LayerScrollSettingsIndex:X2}: {ManaMetadata.GetMapFriendlyName(index)}");
                }
                index++;
            }
            foreach (KeyValuePair<int, int> kvp in counts)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"{ManaMetadata.LayerScrollNameStrings[(byte)kvp.Key]}: {kvp.Value}");
            }
        }

        [Conditional("DEBUG")]
        private static void SpriteUnknownBits()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            int index = 0;
            foreach (MapHeader header in MapDebugger.Context.MapContext.MapHeaderTable)
            {
                foreach (MapSpriteObject spriteObject in header.ObjectTable)
                {
                    if ((spriteObject.UnknownBits) > 0)
                    {
                        LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{index:X4}]: {spriteObject.Name} - {(spriteObject.UnknownBits):X2}");
                    }
                }
                index++;
            }
        }

        [Conditional("DEBUG")]
        private static void MapUnknownByte()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            int index = 0;
            foreach (MapHeader header in MapDebugger.Context.MapContext.MapHeaderTable)
            {
                if (header.IsValid && header.Unused > 0)
                {
                    LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{index:X4}]: {header.Unused:X2}");
                }
                index++;
            }
            LoggerEngine.Logger.LogDebug(LogComponent.Debug, "Unknown Byte Scan Complete");
        }

        [Conditional("DEBUG")]
        private static void ScanForF8FFCommands()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            _ = Task.Run(() =>
            {
                MapDrawingOptions options = new MapDrawingOptions(true, true, true, true, true);
                for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
                {
                    try
                    {
                        ManaMap map = MapDebugger.Context.MapContext.CreateMap(i);
                        try
                        {
                            if (map.IsValid)
                            {
                                if (map.Layer1Background.HasReplaceMapPieceCommand)
                                {
                                    LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{i:X4}]: Piece uses F8-FF Decompression Command");
                                }
                                foreach (MapPiece mapPiece in map.Layer1)
                                {
                                    if (mapPiece.HasReplaceMapPieceCommand)
                                    {
                                        LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{i:X4}]: Piece uses F8-FF Decompression Command");
                                    }
                                }
                                if (map.ObjectTable.HasLayer2)
                                {
                                    if (map.Layer2Background.HasReplaceMapPieceCommand)
                                    {
                                        LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{i:X4}]: Piece uses F8-FF Decompression Command");
                                    }
                                    foreach (MapPiece mapPiece in map.Layer2)
                                    {
                                        if (mapPiece.HasReplaceMapPieceCommand)
                                        {
                                            LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{i:X4}]: Piece uses F8-FF Decompression Command");
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {i:X4}. Exception: {ex.GetType()}, Message: {ex.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot create map index {i:X4}. Exception: {ex.GetType()}, Message: {ex.Message}");
                    }
                }
            });
        }

        [Conditional("DEBUG")]
        private static void PrintSpecialItem40()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            int index = 0;
            foreach (MapHeader header in MapDebugger.Context.MapContext.MapHeaderTable)
            {
                if (header.IsValid)
                {
                    if (header.SpecialItemsOptions.HasFlag(MapSpecialItemsOptions.MagicRopeAllowed))
                    {
                        LoggerEngine.Logger.LogDebug(LogComponent.General, $"[{index:X4}]: Has Special Item Flag: {MapSpecialItemsOptions.MagicRopeAllowed}");
                    }
                }
                index++;
            }
        }

        [Conditional("DEBUG")]
        private static void PrintSpecialItem20()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            int index = 0;
            foreach (MapHeader header in MapDebugger.Context.MapContext.MapHeaderTable)
            {
                if (header.IsValid)
                {
                    if (!header.SpecialItemsOptions.HasFlag(MapSpecialItemsOptions.Dungeon) && 
                        //!header.SpecialItemsOptions.HasFlag(MapSpecialItemsOptions.MagicRopeAllowed) && 
                        header.IsCombatMap)
                    {
                        LoggerEngine.Logger.LogDebug(LogComponent.General, $"[{index:X4}]: Has Special Item Flag: {MapSpecialItemsOptions.Dungeon}");
                    }
                }
                index++;
            }
        }

        [Conditional("DEBUG")]
        private static void ScanForDisplaySettingsByteBits()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            int index = 0;
            foreach (MapHeader header in MapDebugger.Context.MapContext.MapHeaderTable)
            {
                if (header.IsValid && header.UnknownDSBits > 0)
                {
                    LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{index:X4}]: {header.UnknownDSBits}");
                }
                index++;
            }
            LoggerEngine.Logger.LogDebug(LogComponent.Debug, "Unknown Byte Scan Complete");
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSpriteCount()
        {
            if (!MapDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Map debugger has not been initialized.");
                return;
            }

            int index = 0;
            int maxSprites = 0;
            int mapId = 0;
            foreach (MapHeader header in MapDebugger.Context.MapContext.MapHeaderTable)
            {
                if (header.IsValid)
                {
                    maxSprites = Math.Max(maxSprites, header.ObjectTable.RowCount);
                    if (maxSprites == header.ObjectTable.RowCount) { mapId = index; }
                }
                index++;
            }
            LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"Max sprites on a single map is {maxSprites}, on map {mapId:X4}.");
        }
    }
}