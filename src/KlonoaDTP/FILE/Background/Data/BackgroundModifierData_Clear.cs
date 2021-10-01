namespace BinarySerializer.Klonoa.DTP
{
    public class BackgroundModifierData_Clear : BinarySerializable
    {
        public Entry[] Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Entries = s.SerializeObjectArray<Entry>(Entries, 4, name: nameof(Entries));
        }

        public class Entry : BinarySerializable
        {
            public short XPos { get; set; }
            public short XPos_RelativeObj { get; set; } // An index to a background modifier, can be -1

            public short YPos { get; set; }
            public short YPos_RelativeObj { get; set; } // An index to a background modifier, can be -1

            public RGBA8888Color Color { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                XPos = s.Serialize<short>(XPos, name: nameof(XPos));
                XPos_RelativeObj = s.Serialize<short>(XPos_RelativeObj, name: nameof(XPos_RelativeObj));
                YPos = s.Serialize<short>(YPos, name: nameof(YPos));
                YPos_RelativeObj = s.Serialize<short>(YPos_RelativeObj, name: nameof(YPos_RelativeObj));
                Color = s.SerializeObject<RGBA8888Color>(Color, name: nameof(Color));
            }
        }
    }
}