using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class LightInfo : BinarySerializable
    {
        public Light[] Lights { get; set; }
        public SerializableColor AmbientColor { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Lights = s.SerializeObjectArray<Light>(Lights, 3, name: nameof(Lights));
            AmbientColor = s.SerializeInto<SerializableColor>(AmbientColor, PS2Color.RGBA8888, name: nameof(AmbientColor));
        }
    }
}