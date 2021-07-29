namespace BinarySerializer.KlonoaDTP
{
    public class UnknownModelObjectsData_File : BaseFile
    {
        public uint Pre_ObjsCount { get; set; }

        public Entry[] Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Entries = s.SerializeObjectArray<Entry>(Entries, Pre_ObjsCount, name: nameof(Entries));
        }

        public class Entry : BinarySerializable
        {
            public int[] Data { get; set; } // TODO: Parse - setting some -1 values to 0 completely breaks the vram

            public override void SerializeImpl(SerializerObject s)
            {
                Data = s.SerializeArray<int>(Data, 8, name: nameof(Data));
            }
        }
    }
}