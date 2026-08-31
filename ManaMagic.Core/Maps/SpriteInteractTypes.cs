using System;

#nullable enable

namespace ManaMagic.Core.Maps
{
    [Flags]
    public enum SpriteInteractTypes : byte
    {
        None = 0x00,
        EventInRadius = 0x10,
        DoNotFaceOnInteract = 0x20,
        EventOnInteract = 0x40,
        Pushable = 0x80,
    }
}