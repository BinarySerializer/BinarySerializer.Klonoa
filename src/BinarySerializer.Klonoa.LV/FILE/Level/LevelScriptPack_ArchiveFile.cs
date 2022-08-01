namespace BinarySerializer.Klonoa.LV
{
    public class LevelScriptPack_ArchiveFile : ArchiveFile
    {
        /* 
            Field names inferred from prototype debug strings: https://hiddenpalace.org/Klonoa_2:_Lunatea's_Veil_(Sep_17,_2001_prototype)
                0-5 = BgDecode
                6-11 = BgAnm
                12 = FlatMirror
                13 = CurveMirror
                14 = VPA
                15 = VPO
                16 = MTexVPM
                17 = MiniPuppet
                18 = Puppet
                19 = m1100a
                20 = m1100b
        */

        public VIFGeometry_File[] BackgroundMeshes { get; set; }
        public RawData_File[] BackgroundAnimations { get; set; }
        public RawData_File FlatMirror { get; set; }
        public RawData_File CurveMirror { get; set; }
        public RawData_File VPA { get; set; }
        public RawData_File VPO { get; set; }
        public VIFGeometry_File MTexVPM { get; set; }
        public RawData_ArchiveFile MiniCutscenes { get; set; }
        public Cutscenes_ArchiveFile Cutscenes { get; set; }
        public VIFGeometry_File M1100a { get; set; }
        public VIFGeometry_File M1100b { get; set; }
        public RawData_File DummyFile { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            for (int i = 0; i < OffsetTable.FilesCount; i++)
            {
                BackgroundMeshes ??= new VIFGeometry_File[6];
                BackgroundAnimations ??= new RawData_File[6];

                if (!IsDummy(i))
                {
                    if (i < 6)
                        BackgroundMeshes[i] = SerializeFile(s, BackgroundMeshes[i], i, name: $"{nameof(BackgroundMeshes)}[{i}]");
                    else if (i < 12)
                        BackgroundAnimations[i - 6] = SerializeFile(s, BackgroundAnimations[i - 6], i, name: $"{nameof(BackgroundAnimations)}[{i - 6}]");
                    else
                    {
                        switch (i)
                        {
                            case 12:
                                FlatMirror = SerializeFile(s, FlatMirror, i, name: nameof(FlatMirror));
                                break;
                            case 13:
                                CurveMirror = SerializeFile(s, CurveMirror, i, name: nameof(CurveMirror));
                                break;
                            case 14:
                                VPA = SerializeFile(s, VPA, i, name: nameof(VPA));
                                break;
                            case 15:
                                VPO = SerializeFile(s, VPO, i, name: nameof(VPO));
                                break;
                            case 16:
                                MTexVPM = SerializeFile(s, MTexVPM, i, name: nameof(MTexVPM));
                                break;
                            case 17:
                                MiniCutscenes = SerializeFile(s, MiniCutscenes, i, name: nameof(MiniCutscenes));
                                break;
                            case 18:
                                Cutscenes = SerializeFile(s, Cutscenes, i, name: nameof(Cutscenes));
                                break;
                            case 19:
                                M1100a = SerializeFile(s, M1100a, i, name: nameof(M1100a));
                                break;
                            case 20:
                                M1100b = SerializeFile(s, M1100b, i, name: nameof(M1100b));
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    SerializeDummy(s, i);
                }
            }
        }

        private bool IsDummy(int index) => GetFileEndPointer(index) - OffsetTable.FilePointers[index] == 0x10;

        private void SerializeDummy(SerializerObject s, int index)
        {
            DummyFile ??= SerializeFile(s, DummyFile, index, name: $"{nameof(DummyFile)}");
            FlagAsParsed(index, DummyFile, name: $"{nameof(DummyFile)}");
        }

        
    }
}