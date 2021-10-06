using System;

namespace BinarySerializer.Klonoa.DTP
{
    public class CollisionTriangle : BinarySerializable
    {
        public short NormalX { get; set; }
        public short NormalY { get; set; }

        public short X1 { get; set; }
        public short Y1 { get; set; }

        public short X2 { get; set; }
        public short Y2 { get; set; }

        public short X3 { get; set; }
        public short Y3 { get; set; }

        public short NormalZ { get; set; }

        public short Z1 { get; set; }
        public short Z2 { get; set; }
        public short Z3 { get; set; }

        public CollisionType Type { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            NormalX = s.Serialize<short>(NormalX, name: nameof(NormalX));
            NormalY = s.Serialize<short>(NormalY, name: nameof(NormalY));
            X1 = s.Serialize<short>(X1, name: nameof(X1));
            Y1 = s.Serialize<short>(Y1, name: nameof(Y1));
            X2 = s.Serialize<short>(X2, name: nameof(X2));
            Y2 = s.Serialize<short>(Y2, name: nameof(Y2));
            X3 = s.Serialize<short>(X3, name: nameof(X3));
            Y3 = s.Serialize<short>(Y3, name: nameof(Y3));
            NormalZ = s.Serialize<short>(NormalZ, name: nameof(NormalZ));
            Z1 = s.Serialize<short>(Z1, name: nameof(Z1));
            Z2 = s.Serialize<short>(Z2, name: nameof(Z2));
            Z3 = s.Serialize<short>(Z3, name: nameof(Z3));
            Type = s.Serialize<CollisionType>(Type, name: nameof(Type));

            if (!Enum.IsDefined(typeof(CollisionType), Type))
                s.LogWarning($"Collision type 0x{(uint)Type:X8} is not defined");
        }

        public enum CollisionType : uint
        {
            Solid = 0x00FFFFFF,
            Pit = 0x00FF0000,
        }
    }
}