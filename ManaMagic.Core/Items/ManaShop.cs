using System.Collections;
using System.Collections.Generic;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Items
{
    public sealed record ManaShop : NotifyRecordPropertyChanged, IEnumerable<ShopItem>
    {
        public static ManaShop Empty { get; } = new ManaShop(0, new List<ShopItem>());

        private readonly List<ShopItem> items;

        public IReadOnlyList<ShopItem> Items { get { return this.items; } }
        public ShopItem this[int index]
        {
            get { return this.items[index]; }
            set
            {
                this.items[index] = value;
                this.OnPropertyChanged(nameof(this.Items));
            }
        }

        public ManaShop(byte index, List<ShopItem> shopItems, bool userModified = false) : base(index, userModified)
        {
            this.items = shopItems;
        }

        public void Add(ShopItem item)
        {
            this.items.Add(item);
            this.OnPropertyChanged(nameof(this.Items));
        }

        public void Insert(int index, ShopItem item)
        {
            this.items.Insert(index, item);
            this.OnPropertyChanged(nameof(this.Items));
        }

        public bool Remove(ShopItem item)
        {
            bool removed = this.items.Remove(item);
            this.OnPropertyChanged(nameof(this.Items));
            return removed;
        }

        /// <inheritdoc />
        public IEnumerator<ShopItem> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}