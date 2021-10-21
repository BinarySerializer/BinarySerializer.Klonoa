namespace BinarySerializer.Klonoa.KH
{
    public class GameplayPack_ArchiveFile : TP_ArchiveFile
    {
        // File 0 is null
        public Animation_File Klonoa { get; set; }
        public Animation_File File_2 { get; set; }
        public Animation_File File_3 { get; set; }
        public Graphics_File File_4 { get; set; }
        public Graphics_File File_5 { get; set; }
        public Graphics_File File_6 { get; set; }
        public Graphics_File File_7 { get; set; }
        public Animation_File File_8 { get; set; }
        public Animation_File Doors { get; set; }
        public Graphics_File File_10 { get; set; }
        public Graphics_File File_11 { get; set; }
        public ArchiveFile<Graphics_File> File_12 { get; set; }
        public Graphics_File File_13 { get; set; }
        public Animation_File File_14 { get; set; }
        public ArchiveFile<Graphics_File> File_15 { get; set; }
        public Graphics_File File_16 { get; set; }
        public Graphics_File File_17 { get; set; }
        public Animation_File File_18 { get; set; }
        public Animation_File File_19 { get; set; }
        public Animation_File File_20 { get; set; }
        public ArchiveFile<Graphics_File> File_21 { get; set; }
        public Graphics_File File_22 { get; set; }
        public RawData_File File_23 { get; set; } // TODO: HM file
        public GameplayPack_File24_ArchiveFile File_24 { get; set; }
        public ArchiveFile<Graphics_File> File_25 { get; set; }
        public ArchiveFile<Graphics_File> File_26 { get; set; }
        public Graphics_File File_27 { get; set; }
        public Graphics_File File_28 { get; set; }
        public Animation_File File_29 { get; set; }
        public Graphics_File File_30 { get; set; }
        public Graphics_File File_31 { get; set; }
        public Graphics_File File_32 { get; set; }
        public Graphics_File File_33 { get; set; }
        public Graphics_File File_34 { get; set; }
        public BytePairEncoded_ArchiveFile<Graphics_File> File_35 { get; set; }
        public Animation_File File_36 { get; set; }
        public Graphics_File File_37 { get; set; }
        public Animation_File File_38 { get; set; }
        public Animation_File File_39 { get; set; }
        public Animation_File File_40 { get; set; }
        public ArchiveFile<Graphics_File> File_41 { get; set; }
        public Animation_File File_42 { get; set; }
        public ArchiveFile<Graphics_File> File_43 { get; set; }
        public Animation_File File_44 { get; set; }
        public ArchiveFile<Graphics_File> MusicPlayerGraphics { get; set; }
        public Animation_File File_46 { get; set; }
        public Animation_File File_47 { get; set; }
        public Animation_File File_48 { get; set; }
        public Animation_File File_49 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Klonoa = SerializeFile<Animation_File>(s, Klonoa, 1, name: nameof(Klonoa));
            File_2 = SerializeFile<Animation_File>(s, File_2, 2, name: nameof(File_2));
            File_3 = SerializeFile<Animation_File>(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile<Graphics_File>(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile<Graphics_File>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<Graphics_File>(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile<Graphics_File>(s, File_7, 7, name: nameof(File_7));
            File_8 = SerializeFile<Animation_File>(s, File_8, 8, name: nameof(File_8));
            Doors = SerializeFile<Animation_File>(s, Doors, 9, name: nameof(Doors));
            File_10 = SerializeFile<Graphics_File>(s, File_10, 10, name: nameof(File_10));
            File_11 = SerializeFile<Graphics_File>(s, File_11, 11, name: nameof(File_11));
            File_12 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_12, 12, name: nameof(File_12));
            File_13 = SerializeFile<Graphics_File>(s, File_13, 13, name: nameof(File_13));
            File_14 = SerializeFile<Animation_File>(s, File_14, 14, name: nameof(File_14));
            File_15 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_15, 15, name: nameof(File_15));
            File_16 = SerializeFile<Graphics_File>(s, File_16, 16, name: nameof(File_16));
            File_17 = SerializeFile<Graphics_File>(s, File_17, 17, name: nameof(File_17));
            File_18 = SerializeFile<Animation_File>(s, File_18, 18, name: nameof(File_18));
            File_19 = SerializeFile<Animation_File>(s, File_19, 19, name: nameof(File_19));
            File_20 = SerializeFile<Animation_File>(s, File_20, 20, name: nameof(File_20));
            File_21 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_21, 21, name: nameof(File_21));
            File_22 = SerializeFile<Graphics_File>(s, File_22, 22, name: nameof(File_22));
            File_23 = SerializeFile<RawData_File>(s, File_23, 23, name: nameof(File_23));
            File_24 = SerializeFile<GameplayPack_File24_ArchiveFile>(s, File_24, 24, name: nameof(File_24));
            File_25 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_25, 25, name: nameof(File_25));
            File_26 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_26, 26, name: nameof(File_26));
            File_27 = SerializeFile<Graphics_File>(s, File_27, 27, name: nameof(File_27));
            File_28 = SerializeFile<Graphics_File>(s, File_28, 28, name: nameof(File_28));
            File_29 = SerializeFile<Animation_File>(s, File_29, 29, name: nameof(File_29));
            File_30 = SerializeFile<Graphics_File>(s, File_30, 30, name: nameof(File_30));
            File_31 = SerializeFile<Graphics_File>(s, File_31, 31, name: nameof(File_31));
            File_32 = SerializeFile<Graphics_File>(s, File_32, 32, name: nameof(File_32));
            File_33 = SerializeFile<Graphics_File>(s, File_33, 33, name: nameof(File_33));
            File_34 = SerializeFile<Graphics_File>(s, File_34, 34, name: nameof(File_34));
            File_35 = SerializeFile<BytePairEncoded_ArchiveFile<Graphics_File>>(s, File_35, 35, name: nameof(File_35));
            File_36 = SerializeFile<Animation_File>(s, File_36, 36, name: nameof(File_36));
            File_37 = SerializeFile<Graphics_File>(s, File_37, 37, name: nameof(File_37));
            File_38 = SerializeFile<Animation_File>(s, File_38, 38, name: nameof(File_38));
            File_39 = SerializeFile<Animation_File>(s, File_39, 39, name: nameof(File_39));
            File_40 = SerializeFile<Animation_File>(s, File_40, 40, name: nameof(File_40));
            File_41 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_41, 41, name: nameof(File_41));
            File_42 = SerializeFile<Animation_File>(s, File_42, 42, name: nameof(File_42));
            File_43 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_43, 43, name: nameof(File_43));
            File_44 = SerializeFile<Animation_File>(s, File_44, 44, name: nameof(File_44));
            MusicPlayerGraphics = SerializeFile<ArchiveFile<Graphics_File>>(s, MusicPlayerGraphics, 45, name: nameof(MusicPlayerGraphics));
            File_46 = SerializeFile<Animation_File>(s, File_46, 46, name: nameof(File_46));
            File_47 = SerializeFile<Animation_File>(s, File_47, 47, name: nameof(File_47));
            File_48 = SerializeFile<Animation_File>(s, File_48, 48, name: nameof(File_48));
            File_49 = SerializeFile<Animation_File>(s, File_49, 49, name: nameof(File_49));
        }
    }
}