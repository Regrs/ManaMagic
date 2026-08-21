using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events
{
    /// <summary>
    /// Represents the command group of an event op-code.
    /// </summary>
    public enum EventOpCodeCommandType : byte
    {
        /// <summary>
        /// The System command type.
        /// </summary>
        System = 0x00,
        /// <summary>
        /// The Conditional command type.
        /// </summary>
        Conditional,
        /// <summary>
        /// The Text command type.
        /// </summary>
        Text,
        /// <summary>
        /// The Text Data command type.
        /// </summary>
        TextData,
        /// <summary>
        /// The Timing command type.
        /// </summary>
        Timing,
        /// <summary>
        /// The Sprite command type.
        /// </summary>
        Sprite,
        /// <summary>
        /// The Party command type.
        /// </summary>
        Party,
        /// <summary>
        /// The Graphical command type.
        /// </summary>
        Graphical,
        /// <summary>
        /// The Audio command type.
        /// </summary>
        Audio,
        /// <summary>
        /// The Transport command type.
        /// </summary>
        Transport,
        /// <summary>
        /// The Inventory command type.
        /// </summary>
        Inventory,
        /// <summary>
        /// The Ring Menu command type.
        /// </summary>
        [FieldDisplayName("Ring Menu")]
        RingMenu,
        /// <summary>
        /// The Event Flag command type.
        /// </summary>
        [FieldDisplayName("Event Flag")]
        EventFlag,
        /// <summary>
        /// The Utility command type.
        /// </summary>
        Utility,
    }
}