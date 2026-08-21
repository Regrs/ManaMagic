using System;

#nullable enable

namespace ManaMagic.Controls
{
    public sealed class ColorChanged : EventArgs
    {
        public int Index { get; }

        public ColorChanged(int index)
        {
            this.Index = index;
        }
    }
}