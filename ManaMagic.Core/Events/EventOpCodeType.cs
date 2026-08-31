#nullable enable

// SetMapRopePoint, ClearMapRopePoint, and ActivateMapRopePoint are unused but can be control the Magic Rope.
// SetMapBackgroundColor and SetAnimationOverride might also be used internally somewhere, but it is unlikely.
// I'm reasonably certain the others marked as unused but have functionality are not used by anything.

// NoOperation is used by the crystal orb processing events because Conditional op-codes always skip two bytes.

// ToggleInvisibilityA and ToggleInvisibilityB are interesting. Both point to the same code and both are used.
// The way some events use it suggest it wasn't originally a toggle and A turned invisiblity on and B turned it off again.

// Text Op-Codes are stored in Bank 00 rather than Bank 01.

using System.ComponentModel;
using ZwellTech;

namespace ManaMagic.Core.Events
{
    public enum EventOpCodeType : byte
    {
        End = 0x00,
        NoOperation = 0x01,
        ReturnToCaller = 0x02,
        BringPartyToEventActivator = 0x03,
        ToggleInvisibilityA = 0x04,
        ToggleInvisibilityB = 0x05,
        LockSpriteBehavior = 0x06,
        UnlockSpriteBehavior = 0x07,
        WaitForAnimations = 0x08,
        RefreshNpcState = 0x09,
        RefreshMapState = 0x0A,
        //SetMapRopePoint = 0x0B,                        // Unused. Sets $0108 (Rope Door Index) to the value contained in $010E, which is the 16-bit door index used to enter the current map.
        //ClearMapRopePoint = 0x0C,                      // Unused. Sets  $0108 to zero.
        //ActivateMapRopePoint = 0x0D,                   // Unused. Uses the door at the index stored at $0108.
        OpenSellItemRing = 0x0E,
        OpenWeaponUpgradeRing = 0x0F,
        [FieldDisplayName("Jump To Event")]
        JumpToEvent1 = 0x10,
        JumpToEvent2 = 0x11,
        JumpToEvent3 = 0x12,
        JumpToEvent4 = 0x13,
        JumpToEvent5 = 0x14,
        JumpToEvent6 = 0x15,
        JumpToEvent7 = 0x16,
        JumpToEvent8 = 0x17,

        [FieldDisplayName("Use Door")]
        UseDoor1 = 0x18,
        UseDoor2 = 0x19,
        UseDoor3 = 0x1A,
        UseDoor4 = 0x1B,

        [FieldDisplayName("Flammie Flight")]
        FlammieFlight = 0x1C,
        CannonTravel = 0x1D,
        AddItem = 0x1E,
        Utility = 0x1F,

        [FieldDisplayName("Call Event")]
        CallEvent1 = 0x20,
        CallEvent2 = 0x21,
        CallEvent3 = 0x22,
        CallEvent4 = 0x23,
        CallEvent5 = 0x24,
        CallEvent6 = 0x25,
        CallEvent7 = 0x26,
        CallEvent8 = 0x27,
        Wait = 0x28,
        IncrementEventFlag = 0x29,
        DecrementEventFlag = 0x2A,
        AddToParty = 0x2B,
        RemoveFromParty = 0x2C,
        ScreenEffect = 0x2D,
        OpenShopRing = 0x2E,
        Restore = 0x2F,
        SetEventFlag = 0x30,
        AnimateActorSingle = 0x31,
        MoveActor = 0x32,
        //SetMapBackgroundColor = 0x33,                  // Unused. Takes a 16-bit parameter and sets it to the maps background color.
        AnimateActorLoop = 0x34,
        RefreshPartyStatusBars = 0x35,
        AddGold = 0x36,
        SubtractGold = 0x37,
        IfCharacterSlotActivatedEvent = 0x38,
        SetCharacterSlotAddress = 0x39,
        //SetAnimationOverride = 0x3A,                   // Unused. Takes 3 byte parameters. First is the slot index, second is the animation ID, third is the tick duration.
        //IfCharacterSlotControlledByController = 0x3B,  // Unused. Takes a byte parameter. High nibble is the controller index, low nibble is the character ID.
        //IfNumberOfPlayersPlaying = 0x3C,               // Unused. Takes a byte parameter that is the number of controllers that have to be active.
        //DummiedCommand3D = 0x3D,                       // Will hang the game due to not incrementing the event pointer.
        //DummiedCommand3E = 0x3E,                       // Will hang the game due to not incrementing the event pointer.
        //DummiedCommand3F = 0x3F,                       // Will hang the game due to not incrementing the event pointer.
        PlayAudio = 0x40,
        //RefreshMapStateOld = 0x41,                     // Unused. Takes two byte parameters and a 16-bit parameter. None appear to be used. Calls the same routine as 0A.
        IfEventFlagInRange = 0x42,
        //BrokenEventFlagConditional = 0x43,             // Unused and Broken. Takes two byte parameters. The first are operation bits that do nothing, the second is the event flag index.
        //DummiedCommand44 = 0x44,                       // Will hang the game due to not incrementing the event pointer.
        //DummiedCommand45 = 0x45,                       // Will hang the game due to not incrementing the event pointer.
        //DummiedCommand46 = 0x46,                       // Will hang the game due to not incrementing the event pointer.
        //DummiedCommand47 = 0x47,                       // Will hang the game due to not incrementing the event pointer.
        //BrokenCompareEventFlagConditional = 0x48,      // Unused. Takes three byte parameters. First are operation bits, 2nd & 3rd are event flag indexes. Mostly uses BrokenEventFlagConditional's code.
        IfCharacterSlotAddressGreaterThanOrEqual = 0x49, // =>
        IfCharacterSlotAddressLessThanOrEqual = 0x4A,    // =<
        IfCharacterSlotAddressBitTest = 0x4B,            // &
        //IfCharacterSlotAddressAndOperandIsZero = 0x4C, // | Unused. Takes two byte parameters.
        //IfCharacterSlotAddressIsNotEqual = 0x4D,       // ^ Unused. Takes two byte parameters.
        IfCharacterSlotAddressIsEqual = 0x4E,            // 
        //DummiedCommand4F = 0x4F,                       // Will hang the game due to not incrementing the event pointer.
        OpenTextWindow = 0x50,
        CloseTextWindow = 0x51,
        ClearTextWindow = 0x52,
        //DummiedTextCommand53 = 0x53,                   // Does Nothing.
        //PrintEnemyName = 0x54,                         // Used by the menu system.
        //PrintItemName = 0x55,                          // Used by the menu system. Weapons > Helms > Armor > Accessories > Consumables > Ring Items
        //PrintMagicName = 0x56,                         // Used by the menu system.
        PrintPlayerName = 0x57,
        BeginOptionDialogSetup = 0x58,
        PadLeft = 0x59,
        SetOptionDialogOption = 0x5A,
        EndOptionDialogSetup = 0x5B,
        //PadLeftAlternate = 0x5C,                       // Used by the status menu. Same as 59 except it sets and restores the DB register.
        OpenCurrencyWindow = 0x5D,
        CloseCurrencyWindow = 0x5E,
        PrintCurrency = 0x5F,
        // 60-7C are DTE.

        [FieldDisplayName("ASCII Text")]
        ASCIITextStart = 0x65,
        ASCIITextEnd = 0x90,

        BeginASCIITextCrawl = 0x7D,
        EndASCIITextCrawl = 0x7E,

        [FieldDisplayName("Text")]
        TextStart = 0x7F,                                // Doubles as the NewLine character in ASCII mode.
        TextEnd = 0xD2,
    }
}