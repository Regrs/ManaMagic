#nullable enable

namespace ManaMagic.Core.Events
{
    /// <summary>
    /// Represents the utility sub-type of a <see cref="UtilityEventOpCode"/>.
    /// </summary>
    public enum UtilityEventOpCodeType : byte
    {
        /// <summary>
        /// Opens Purim's Naming Dialog and initializes their character slot.
        /// </summary>
        /// <remarks>Only used indirectly when starting a new game.</remarks>
        OpenNameRandiDialog = 0x00,
        /// <summary>
        /// Opens Purim's Naming Dialog and initializes their character slot.
        /// </summary>
        OpenNamePurimDialog = 0x01,
        /// <summary>
        /// Opens Popoie's Naming Dialog and initializes their character slot.
        /// </summary>
        OpenNamePopoieDialog = 0x02,
        /// <summary>
        /// Plays the cutscene of the Sunken Continent rising from the sea.
        /// </summary>
        PlayCutsceneSunkenContinentRises = 0x03,
        /// <summary>
        /// Plays the cutscene of the Mana Fortress rising from the Sunken Continent.
        /// </summary>
        PlayCutsceneManaFortressRises = 0x04,
        /// <summary>
        /// Does Nothing.
        /// </summary>
        DummiedUtility05 = 0x05,
        /// <summary>
        /// Opens the Save Game Dialog.
        /// </summary>
        OpenSaveGameDialog = 0x06,
        /// <summary>
        /// Resets the game.
        /// </summary>
        /// <remarks>Calls the Reset Vector.</remarks>
        ForceGameReset = 0x07,
        /// <summary>
        /// Updates the character state for all equipped weapons.
        /// </summary>
        /// <remarks>08-0B all point to the same routine. Only 08 is used by the game.</remarks>
        UpdateEquippedWeapons = 0x08,
        /// <summary>
        /// Rewards the player with the contents of the treasure chest that triggered the event, if able.
        /// </summary>
        ProcessChestContents = 0x0C,
        /// <summary>
        /// Prepares the party to perform a whip jump.
        /// </summary>
        /// <remarks>Used by whip post special tiles.</remarks>
        PrepareForWhipPostJump = 0x10,
        /// <summary>
        /// Displays the end game slide.
        /// </summary>
        /// <remarks>Any undefined operand (0D, 0E, 0F, 12-FF) will trigger this, but this is the value used by the game.</remarks>
        DisplayEndGameSlide = 0x11,
    }
}