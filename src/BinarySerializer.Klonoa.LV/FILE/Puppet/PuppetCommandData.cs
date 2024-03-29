namespace BinarySerializer.Klonoa.LV
{
    public abstract class PuppetCommandData : BinarySerializable {};

    public class PuppetCommandData32A : PuppetCommandData
    {
        public short Short0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short0 = s.Serialize<short>(Short0, nameof(Short0));
        }
    }

    public class PuppetCommandData32B : PuppetCommandData
    {
        public ushort UShort0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            UShort0 = s.Serialize<ushort>(UShort0, nameof(UShort0));
        }
    }

    public class PuppetCommandData64A : PuppetCommandData
    {
        public short Short0 { get; set; }
        public short Short1 { get; set; }
        public short Short2 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short0 = s.Serialize<short>(Short0, nameof(Short0));
            Short1 = s.Serialize<short>(Short1, nameof(Short1));
            Short2 = s.Serialize<short>(Short2, nameof(Short2));
        }
    }

    public class PuppetCommandData64B : PuppetCommandData
    {
        public ushort UShort0 { get; set; }
        public int Int0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            UShort0 = s.Serialize<ushort>(UShort0, nameof(UShort0));
            Int0 = s.Serialize<int>(Int0, nameof(Int0));
        }
    }

    public class PuppetCommandData64C : PuppetCommandData
    {
        public ushort UShort0 { get; set; }
        public float Float0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            UShort0 = s.Serialize<ushort>(UShort0, nameof(UShort0));
            Float0 = s.Serialize<float>(Float0, nameof(Float0));
        }
    }

    public class PuppetCommandData64D : PuppetCommandData
    {
        public byte Byte0 { get; set; }
        public byte Byte1 { get; set; }
        public int Int0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte0 = s.Serialize<byte>(Byte0, nameof(Byte0));
            Byte1 = s.Serialize<byte>(Byte1, nameof(Byte1));
            Int0 = s.Serialize<int>(Int0, nameof(Int0));
        }
    }

    public class PuppetCommandData64E : PuppetCommandData
    {
        public ushort UShort0 { get; set; }
        public byte Byte0 { get; set; }
        public byte Byte1 { get; set; }
        public byte Byte2 { get; set; }
        public byte Byte3 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            UShort0 = s.Serialize<ushort>(UShort0, nameof(UShort0));
            Byte0 = s.Serialize<byte>(Byte0, nameof(Byte0));
            Byte1 = s.Serialize<byte>(Byte1, nameof(Byte1));
            Byte2 = s.Serialize<byte>(Byte2, nameof(Byte2));
            Byte3 = s.Serialize<byte>(Byte3, nameof(Byte3));
        }
    }

    public class PuppetCommandData64F : PuppetCommandData
    {
        public byte Byte0 { get; set; }
        public byte Byte1 { get; set; }
        public float Float0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte0 = s.Serialize<byte>(Byte0, nameof(Byte0));
            Byte1 = s.Serialize<byte>(Byte1, nameof(Byte1));
            Float0 = s.Serialize<float>(Float0, nameof(Float0));
        }
    }

    public class PuppetCommandData96A : PuppetCommandData
    {
        public PuppetDraw Draw { get; set; }
        public ushort UShort0 { get; set; }
        public short Short0 { get; set; }
        public uint UInt0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Draw = s.Serialize<PuppetDraw>(Draw, nameof(Draw));
            UShort0 = s.Serialize<ushort>(UShort0, nameof(UShort0));
            Short0 = s.Serialize<short>(Short0, nameof(Short0));
            UInt0 = s.Serialize<uint>(UInt0, nameof(UInt0));
        }
    }

    public class PuppetCommandData96B : PuppetCommandData
    {
        public ushort UShort0 { get; set; }
        public ushort UShort1 { get; set; }
        public short Short0 { get; set; }
        public float Float0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            UShort0 = s.Serialize<ushort>(UShort0, nameof(UShort0));
            UShort1 = s.Serialize<ushort>(UShort1, nameof(UShort1));
            Short0 = s.Serialize<short>(Short0, nameof(Short0));
            Float0 = s.Serialize<float>(Float0, nameof(Float0));
        }
    }

    public class PuppetCommandData96C : PuppetCommandData
    {
        public ushort UShort0 { get; set; }
        public uint UInt0 { get; set; }
        public uint UInt1 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            UShort0 = s.Serialize<ushort>(UShort0, nameof(UShort0));
            UInt0 = s.Serialize<uint>(UInt0, nameof(UInt0));
            UInt1 = s.Serialize<uint>(UInt1, nameof(UInt1));
        }
    }

    public class PuppetCommandData96D : PuppetCommandData
    {
        public ushort UShort0 { get; set; }
        public int Int0 { get; set; }
        public float Float0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            UShort0 = s.Serialize<ushort>(UShort0, nameof(UShort0));
            Int0 = s.Serialize<int>(Int0, nameof(Int0));
            Float0 = s.Serialize<float>(Float0, nameof(Float0));
        }
    }

    public class PuppetCommandData96E : PuppetCommandData
    {
        public byte Byte0 { get; set; }
        public byte Byte1 { get; set; }
        public int Int0 { get; set; }
        public uint UInt0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte0 = s.Serialize<byte>(Byte0, nameof(Byte0));
            Byte1 = s.Serialize<byte>(Byte1, nameof(Byte1));
            Int0 = s.Serialize<int>(Int0, nameof(Int0));
            UInt0 = s.Serialize<uint>(UInt0, nameof(UInt0));
        }
    }

    public class PuppetCommandData96F : PuppetCommandData
    {
        public byte Byte0 { get; set; }
        public byte Byte1 { get; set; }
        public float Float0 { get; set; }
        public uint UInt0 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte0 = s.Serialize<byte>(Byte0, nameof(Byte0));
            Byte1 = s.Serialize<byte>(Byte1, nameof(Byte1));
            Float0 = s.Serialize<float>(Float0, nameof(Float0));
            UInt0 = s.Serialize<uint>(UInt0, nameof(UInt0));
        }
    }

    public class PuppetCommandData128A : PuppetCommandData
    {
        public short Short0 { get; set; }
        public float Float0 { get; set; }
        public float Float1 { get; set; }
        public float Float2 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short0 = s.Serialize<short>(Short0, nameof(Short0));
            Float0 = s.Serialize<float>(Float0, nameof(Float0));
            Float1 = s.Serialize<float>(Float1, nameof(Float1));
            Float2 = s.Serialize<float>(Float2, nameof(Float2));
        }
    }

    public class PuppetCommandData128B : PuppetCommandData
    {
        public byte Byte0 { get; set; }
        public byte Byte1 { get; set; }
        public float Float0 { get; set; }
        public float Float1 { get; set; }
        public float Float2 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte0 = s.Serialize<byte>(Byte0, nameof(Byte0));
            Byte1 = s.Serialize<byte>(Byte1, nameof(Byte1));
            Float0 = s.Serialize<float>(Float0, nameof(Float0));
            Float1 = s.Serialize<float>(Float1, nameof(Float1));
            Float2 = s.Serialize<float>(Float2, nameof(Float2));
        }
    }

    public class PuppetCommandData128C : PuppetCommandData
    {
        public ushort UShort0 { get; set; }
        public ushort UShort1 { get; set; }
        public short Short0 { get; set; }
        public uint UInt0 { get; set; }
        public short Short1 { get; set; }
        public short Short2 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            UShort0 = s.Serialize<ushort>(UShort0, nameof(UShort0));
            UShort1 = s.Serialize<ushort>(UShort1, nameof(UShort1));
            Short0 = s.Serialize<short>(Short0, nameof(Short0));
            UInt0 = s.Serialize<uint>(UInt0, nameof(UInt0));
            Short1 = s.Serialize<short>(Short1, nameof(Short1));
            Short2 = s.Serialize<short>(Short2, nameof(Short2));
        }
    }

    public class PuppetCommandData160A : PuppetCommandData
    {
        public byte Byte0 { get; set; }
        public byte Byte1 { get; set; }
        public float Float0 { get; set; }
        public float Float1 { get; set; }
        public float Float2 { get; set; }
        public short Short0 { get; set; }
        public short Short1 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte0 = s.Serialize<byte>(Byte0, nameof(Byte0));
            Byte1 = s.Serialize<byte>(Byte1, nameof(Byte1));
            Float0 = s.Serialize<float>(Float0, nameof(Float0));
            Float1 = s.Serialize<float>(Float1, nameof(Float1));
            Float2 = s.Serialize<float>(Float2, nameof(Float2));
            Short0 = s.Serialize<short>(Short0, nameof(Short0));
            Short1 = s.Serialize<short>(Short1, nameof(Short1));
        }
    }

    public enum PuppetDraw : short
    {
        On,
        Off,
        Num
    }
}