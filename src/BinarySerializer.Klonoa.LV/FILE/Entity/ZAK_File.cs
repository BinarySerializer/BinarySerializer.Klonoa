namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// Data for each entity in the level sector.
    /// TODO: Find out what each entity ID is for (i.e. Walk00 = Moo, Walc00 = Sleeping Moo)
    /// </summary>
    public class ZAK_File : BaseFile
    {
        public ushort Count { get; set; }

        #region Data Sizes
        public ushort MooDataSize { get; set; }
        public ushort Walk10DataSize { get; set; }
        public ushort Fly00DataSize { get; set; }
        public ushort Fly01DataSize { get; set; }
        public ushort Jalc00DataSize { get; set; }
        public ushort Falc00DataSize { get; set; }
        public ushort SleepingMooDataSize { get; set; }
        public ushort Walk11DataSize { get; set; }
        public ushort Fly11DataSize { get; set; }
        public ushort HeroRootHaniDataSize { get; set; }
        public ushort ShutugenDataSize { get; set; }
        public ushort BakeDataSize { get; set; }
        public ushort Walk34DataSize { get; set; }
        public ushort Shutugen2DataSize { get; set; }
        public ushort Jump00DataSize { get; set; }
        public ushort Gum00DataSize { get; set; }
        public ushort Wnd00DataSize { get; set; }
        public ushort Wnd10DataSize { get; set; }
        public ushort Gun00DataSize { get; set; }
        public ushort Grnd00DataSize { get; set; }
        public ushort Cam00DataSize { get; set; }
        public ushort Cam01DataSize { get; set; }
        public ushort Cam02DataSize { get; set; }
        public ushort Snd00DataSize { get; set; }
        public ushort Walk02DataSize { get; set; }
        public ushort Walk03DataSize { get; set; }
        public ushort Walk01DataSize { get; set; }
        public ushort Fire00DataSize { get; set; }
        public ushort Fire10DataSize { get; set; }
        public ushort Sub00DataSize { get; set; }
        public ushort Swit00DataSize { get; set; }
        public ushort Swit01DataSize { get; set; }
        public ushort Base00DataSize { get; set; }
        public ushort Base10DataSize { get; set; }
        public ushort Base11DataSize { get; set; }
        public ushort Base12DataSize { get; set; }
        public ushort Base14DataSize { get; set; }
        public ushort Base16DataSize { get; set; }
        public ushort Base20DataSize { get; set; }
        public ushort Base17DataSize { get; set; }
        public ushort Door00DataSize { get; set; }
        public ushort Fix00DataSize { get; set; }
        public ushort Fix01DataSize { get; set; }
        public ushort Walk04DataSize { get; set; }
        public ushort Walk05DataSize { get; set; }
        public ushort Walk06DataSize { get; set; }
        public ushort Walk07DataSize { get; set; }
        public ushort Walk08DataSize { get; set; }
        public ushort Fly02DataSize { get; set; }
        public ushort Flic00DataSize { get; set; }
        #endregion

        public override void SerializeImpl(SerializerObject s)
        {
            Count = s.Serialize<ushort>(Count, name: nameof(Count));

            MooDataSize = s.Serialize<ushort>(MooDataSize, name: nameof(MooDataSize)); // Walk00
            Walk10DataSize = s.Serialize<ushort>(Walk10DataSize, name: nameof(Walk10DataSize));
            Fly00DataSize = s.Serialize<ushort>(Fly00DataSize, name: nameof(Fly00DataSize));
            Fly01DataSize = s.Serialize<ushort>(Fly01DataSize, name: nameof(Fly01DataSize));
            Jalc00DataSize = s.Serialize<ushort>(Jalc00DataSize, name: nameof(Jalc00DataSize));
            Falc00DataSize = s.Serialize<ushort>(Falc00DataSize, name: nameof(Falc00DataSize));
            SleepingMooDataSize = s.Serialize<ushort>(SleepingMooDataSize, name: nameof(SleepingMooDataSize)); // Walc00
            Walk11DataSize = s.Serialize<ushort>(Walk11DataSize, name: nameof(Walk11DataSize));
            Fly11DataSize = s.Serialize<ushort>(Fly11DataSize, name: nameof(Fly11DataSize));
            HeroRootHaniDataSize = s.Serialize<ushort>(HeroRootHaniDataSize, name: nameof(HeroRootHaniDataSize));
            ShutugenDataSize = s.Serialize<ushort>(ShutugenDataSize, name: nameof(ShutugenDataSize));
            BakeDataSize = s.Serialize<ushort>(BakeDataSize, name: nameof(BakeDataSize));
            Walk34DataSize = s.Serialize<ushort>(Walk34DataSize, name: nameof(Walk34DataSize));
            Shutugen2DataSize = s.Serialize<ushort>(Shutugen2DataSize, name: nameof(Shutugen2DataSize));
            Jump00DataSize = s.Serialize<ushort>(Jump00DataSize, name: nameof(Jump00DataSize));
            Gum00DataSize = s.Serialize<ushort>(Gum00DataSize, name: nameof(Gum00DataSize));
            Wnd00DataSize = s.Serialize<ushort>(Wnd00DataSize, name: nameof(Wnd00DataSize));
            Wnd10DataSize = s.Serialize<ushort>(Wnd10DataSize, name: nameof(Wnd10DataSize));
            Gun00DataSize = s.Serialize<ushort>(Gun00DataSize, name: nameof(Gun00DataSize));
            Grnd00DataSize = s.Serialize<ushort>(Grnd00DataSize, name: nameof(Grnd00DataSize));
            Cam00DataSize = s.Serialize<ushort>(Cam00DataSize, name: nameof(Cam00DataSize));
            Cam01DataSize = s.Serialize<ushort>(Cam01DataSize, name: nameof(Cam01DataSize));
            Cam02DataSize = s.Serialize<ushort>(Cam02DataSize, name: nameof(Cam02DataSize));
            Snd00DataSize = s.Serialize<ushort>(Snd00DataSize, name: nameof(Snd00DataSize));
            Walk02DataSize = s.Serialize<ushort>(Walk02DataSize, name: nameof(Walk02DataSize));
            Walk03DataSize = s.Serialize<ushort>(Walk03DataSize, name: nameof(Walk03DataSize));
            Walk01DataSize = s.Serialize<ushort>(Walk01DataSize, name: nameof(Walk01DataSize));
            Fire00DataSize = s.Serialize<ushort>(Fire00DataSize, name: nameof(Fire00DataSize));
            Fire10DataSize = s.Serialize<ushort>(Fire10DataSize, name: nameof(Fire10DataSize));
            Sub00DataSize = s.Serialize<ushort>(Sub00DataSize, name: nameof(Sub00DataSize));
            Swit00DataSize = s.Serialize<ushort>(Swit00DataSize, name: nameof(Swit00DataSize));
            Swit01DataSize = s.Serialize<ushort>(Swit01DataSize, name: nameof(Swit01DataSize));
            Base00DataSize = s.Serialize<ushort>(Base00DataSize, name: nameof(Base00DataSize));
            Base10DataSize = s.Serialize<ushort>(Base10DataSize, name: nameof(Base10DataSize));
            Base11DataSize = s.Serialize<ushort>(Base11DataSize, name: nameof(Base11DataSize));
            Base12DataSize = s.Serialize<ushort>(Base12DataSize, name: nameof(Base12DataSize));
            Base14DataSize = s.Serialize<ushort>(Base14DataSize, name: nameof(Base14DataSize));
            Base16DataSize = s.Serialize<ushort>(Base16DataSize, name: nameof(Base16DataSize));
            Base20DataSize = s.Serialize<ushort>(Base20DataSize, name: nameof(Base20DataSize));
            Base17DataSize = s.Serialize<ushort>(Base17DataSize, name: nameof(Base17DataSize));
            Door00DataSize = s.Serialize<ushort>(Door00DataSize, name: nameof(Door00DataSize));
            Fix00DataSize = s.Serialize<ushort>(Fix00DataSize, name: nameof(Fix00DataSize));
            Fix01DataSize = s.Serialize<ushort>(Fix01DataSize, name: nameof(Fix01DataSize));
            Walk04DataSize = s.Serialize<ushort>(Walk04DataSize, name: nameof(Walk04DataSize));
            Walk05DataSize = s.Serialize<ushort>(Walk05DataSize, name: nameof(Walk05DataSize));
            Walk06DataSize = s.Serialize<ushort>(Walk06DataSize, name: nameof(Walk06DataSize));
            Walk07DataSize = s.Serialize<ushort>(Walk07DataSize, name: nameof(Walk07DataSize));
            Walk08DataSize = s.Serialize<ushort>(Walk08DataSize, name: nameof(Walk08DataSize));
            Fly02DataSize = s.Serialize<ushort>(Fly02DataSize, name: nameof(Fly02DataSize));
            Flic00DataSize = s.Serialize<ushort>(Flic00DataSize, name: nameof(Flic00DataSize));
        }
    }
}