#nullable enable

namespace ManaMagic
{
    public enum TreeViewSection
    {
        // Events
        EventEditor,

        // Events -> Text
        TownNameEditor,
        //EnemyNameEditor,
        ItemErrorEditor,

        // Events -> Field Messages
        StatusEffectMessageEditor,
        ElementalFearMessageEditor,
        TrapMessageEditor,
        WeaponNameMessageEditor,
        BossSkillNameMessageEditor,
        BuffDebuffMessageEditor,
        LunarMagicMessageEditor,
        //MiscellaneousMessageEditor,
        TreasureChestMessageEditor,
        CombatMessageEditor,
        AnalyzerMessageEditor,
        LevelUpMessageEditor,

        // Character
        DefaultPartyDataEditor,
        RandiStatsByLevelEditor,
        PurimStatsByLevelEditor,
        PopoieStatsByLevelEditor,
        ExperiencePerLevelEditor,

        // Boss
        BossEditor,
        BossPaletteEditor,
        BossWeaponEditor,
        BossTilesetAndFrameViewer,
        BossSkillViewer,

        // Sprites
        EnemyEditor,
        SpritePaletteEditor,

        // Maps
        MapEditor,
        MapDisplaySettingsEditor,
        MapPaletteEditor,
        FlammieFlightCoordinateEditor,
        Map8x8TilesetViewer,
        Map16x16TilesetViewer,
        //MapCollisionEditor,
        //DoorEditor,

        // World Map
        CannonTravelEditor,
        WorldMapLandingLocationEditor,

        // Items
        WeaponEditor,
        EquipmentEditor,
        ConsumablesEditor,

        // Items -> Shops
        ShopEditor,
        ShopPriceEditor,
        ShopMessageEditor,

        // References
        ReferenceSearch,

        // References -> RAM Maps
        RamLayout,
        RamSpriteSlot,
        RamBossSpriteSlot,

        // References -> Usage
        MapPieceUsageCounter,
        MapCollisionUsageCounter,
        MapPaletteUsageCounter,
        SpriteUsageCounter,
        EventUsageCounter,

        // Help
        HelpSpriteSlotMemoryMap,
        HelpSpecialValues,
        HelpCutContent,

        // ----------------------------------------- //

        // Debug
        DebugDisassembly,
        DebugDebugger,
        DebugDrawCanvas,

        // Debug -> Views
        DebugDoorView,
        DebugCollisionTypeView,
        DebugMapTriggerView,
        DebugEnemyStatisticsView,
        DebugLootTableView,
        DebugWeaponDefinitionView,
    }
}