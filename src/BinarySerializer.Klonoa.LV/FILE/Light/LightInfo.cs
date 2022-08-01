using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class LightInfo : BinarySerializable
    {
        public Light[] Lights { get; set; }
        public PS2_RGBA8888Color AmbientColor { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Lights = s.SerializeObjectArray<Light>(Lights, 3, name: nameof(Lights));
            AmbientColor = s.SerializeObject<PS2_RGBA8888Color>(AmbientColor, name: nameof(AmbientColor));
        }
    }
}