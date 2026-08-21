using ManaMagic.Core.Items;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Character
{
    public sealed record ManaDefaultCharacterData : NotifyRecordPropertyChanged
    {
        public static ManaDefaultCharacterData Empty { get; } = new ManaDefaultCharacterData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        private ushort gold = 0;
        private byte sealedManaSeeds = 0;
        private byte saveLocation = 0;
        // Quadrants: 0-3
        private byte randiActionGridQuadrant = 0;
        private byte purimActionGridQuadrant = 0;
        private byte popoieActionGridQuadrant = 0;
        // Index: 0-F
        private byte randiActionGridLocation = 0;
        private byte purimActionGridLocation = 0;
        private byte popoieActionGridLocation = 0;

        private HelmetType randiHelmet = HelmetType.BareHead;
        private ArmorType randiArmor = ArmorType.No;
        private AccessoryType randiAccessory = AccessoryType.Nothing;
        private WeaponType randiWeapon = WeaponType.SpikeKnuckle;

        private HelmetType purimHelmet = HelmetType.BareHead;
        private ArmorType purimArmor = ArmorType.No;
        private AccessoryType purimAccessory = AccessoryType.Nothing;
        private WeaponType purimWeapon = WeaponType.SpikeKnuckle;

        private HelmetType popoieHelmet = HelmetType.BareHead;
        private ArmorType popoieArmor = ArmorType.No;
        private AccessoryType popoieAccessory = AccessoryType.Nothing;
        private WeaponType popoieWeapon = WeaponType.SpikeKnuckle;

        public ushort Gold
        {
            get { return this.gold; }
            set { this.SetProperty(ref this.gold, value); }
        }

        public byte SealedManaSeeds
        {
            get { return this.sealedManaSeeds; }
            set { this.SetProperty(ref this.sealedManaSeeds, value); }
        }

        public byte SaveLocation
        {
            get { return this.saveLocation; }
            set { this.SetProperty(ref this.saveLocation, value); }
        }

        public byte RandiActionGridQuadrant
        {
            get { return this.randiActionGridQuadrant; }
            set { this.SetProperty(ref this.randiActionGridQuadrant, value); }
        }

        public byte PurimActionGridQuadrant
        {
            get { return this.purimActionGridQuadrant; }
            set { this.SetProperty(ref this.purimActionGridQuadrant, value); }
        }

        public byte PopoieActionGridQuadrant
        {
            get { return this.popoieActionGridQuadrant; }
            set { this.SetProperty(ref this.popoieActionGridQuadrant, value); }
        }

        public byte RandiActionGridLocation
        {
            get { return this.randiActionGridLocation; }
            set { this.SetProperty(ref this.randiActionGridLocation, value); }
        }

        public byte PurimActionGridLocation
        {
            get { return this.purimActionGridLocation; }
            set { this.SetProperty(ref this.purimActionGridLocation, value); }
        }

        public byte PopoieActionGridLocation
        {
            get { return this.popoieActionGridLocation; }
            set { this.SetProperty(ref this.popoieActionGridLocation, value); }
        }

        public HelmetType RandiHelmet
        {
            get { return this.randiHelmet; }
            set { this.SetProperty(ref this.randiHelmet, value); }
        }

        public ArmorType RandiArmor
        {
            get { return this.randiArmor; }
            set { this.SetProperty(ref this.randiArmor, value); }
        }

        public AccessoryType RandiAccessory
        {
            get { return this.randiAccessory; }
            set { this.SetProperty(ref this.randiAccessory, value); }
        }

        public WeaponType RandiWeapon
        {
            get { return this.randiWeapon; }
            set { this.SetProperty(ref this.randiWeapon, value); }
        }

        public HelmetType PurimHelmet
        {
            get { return this.purimHelmet; }
            set { this.SetProperty(ref this.purimHelmet, value); }
        }

        public ArmorType PurimArmor
        {
            get { return this.purimArmor; }
            set { this.SetProperty(ref this.purimArmor, value); }
        }

        public AccessoryType PurimAccessory
        {
            get { return this.purimAccessory; }
            set { this.SetProperty(ref this.purimAccessory, value); }
        }

        public WeaponType PurimWeapon
        {
            get { return this.purimWeapon; }
            set { this.SetProperty(ref this.purimWeapon, value); }
        }

        public HelmetType PopoieHelmet
        {
            get { return this.popoieHelmet; }
            set { this.SetProperty(ref this.popoieHelmet, value); }
        }

        public ArmorType PopoieArmor
        {
            get { return this.popoieArmor; }
            set { this.SetProperty(ref this.popoieArmor, value); }
        }

        public AccessoryType PopoieAccessory
        {
            get { return this.popoieAccessory; }
            set { this.SetProperty(ref this.popoieAccessory, value); }
        }

        public WeaponType PopoieWeapon
        {
            get { return this.popoieWeapon; }
            set { this.SetProperty(ref this.popoieWeapon, value); }
        }

        public ManaDefaultCharacterData(byte index,
                                        ushort gold,
                                        byte sealedManaSeeds,
                                        byte saveLocation,
                                        byte randiActionGridQuadrant,
                                        byte purimActionGridQuadrant,
                                        byte popoieActionGridQuadrant,
                                        byte randiActionGridLocation,
                                        byte purimActionGridLocation,
                                        byte popoieActionGridLocation,
                                        byte randiHelmet,
                                        byte randiArmor,
                                        byte randiAccessory,
                                        byte randiWeapon,
                                        byte purimHelmet,
                                        byte purimArmor,
                                        byte purimAccessory,
                                        byte purimWeapon,
                                        byte popoieHelmet,
                                        byte popoieArmor,
                                        byte popoieAccessory,
                                        byte popoieWeapon,
                                        bool userModified = false) : base(index, userModified)
        {
            this.gold = gold;
            this.sealedManaSeeds = sealedManaSeeds;
            this.saveLocation = saveLocation;

            this.randiActionGridQuadrant = randiActionGridQuadrant;
            this.purimActionGridQuadrant = purimActionGridQuadrant;
            this.popoieActionGridQuadrant = popoieActionGridQuadrant;

            this.randiActionGridLocation = randiActionGridLocation;
            this.purimActionGridLocation = purimActionGridLocation;
            this.popoieActionGridLocation = popoieActionGridLocation;

            this.randiHelmet = (HelmetType)randiHelmet;
            this.randiArmor = (ArmorType)randiArmor;
            this.randiAccessory = (AccessoryType)randiAccessory;
            this.randiWeapon = (WeaponType)randiWeapon;

            this.purimHelmet = (HelmetType)purimHelmet;
            this.purimArmor = (ArmorType)purimArmor;
            this.purimAccessory = (AccessoryType)purimAccessory;
            this.purimWeapon = (WeaponType)purimWeapon;

            this.popoieHelmet = (HelmetType)popoieHelmet;
            this.popoieArmor = (ArmorType)popoieArmor;
            this.popoieAccessory = (AccessoryType)popoieAccessory;
            this.popoieWeapon = (WeaponType)popoieWeapon;
        }
    }
}