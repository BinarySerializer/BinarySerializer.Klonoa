namespace BinarySerializer.Klonoa.LV
{
    public class VTWave_Parameter : BinarySerializable
    {
        public KlonoaLV_FloatVector Translation { get; set; }
        public KlonoaLV_FloatVector Rotation { get; set; }
        public KlonoaLV_FloatVector Light_0 { get; set; }
        public KlonoaLV_FloatVector Light_1 { get; set; }
        public KlonoaLV_FloatVector Light_2 { get; set; }
        public KlonoaLV_FloatVector LightColor_0 { get; set; }
        public KlonoaLV_FloatVector LightColor_1 { get; set; }
        public KlonoaLV_FloatVector LightColor_2 { get; set; }
        public KlonoaLV_FloatVector AmbientColor { get; set; }
        public KlonoaLV_FloatVector FogColor { get; set; }
        public byte DrawEnable { get; set; }
        public byte Primitive { get; set; }
        public byte LOD { get; set; }
        public byte MMesh { get; set; }
        public byte BaseTexture { get; set; }
        public byte Fog { get; set; }
        public byte MultiTexture { get; set; }
        public byte HeadTexture { get; set; }
        public byte HeadTextureValue { get; set; }
        public byte HeightAdjust { get; set; }
        public byte BaseTextureArea { get; set; }
        public float PlaneSizeX { get; set; }
        public float PlaneSizeZ { get; set; }
        public float MeshSizeX { get; set; }
        public float MeshSizeZ { get; set; }
        public float Height { get; set; }
        public float IntervalX { get; set; }
        public float IntervalZ { get; set; }
        public float RadiusX { get; set; }
        public float RadiusZ { get; set; }
        public float TextureAdjust { get; set; }
        public float SpeedX { get; set; }
        public float SpeedZ { get; set; }
        public float Shear { get; set; }
        public float BaseTextureSpeedX { get; set; }
        public float BaseTextureSpeedZ { get; set; }
        public float MultiTextureSpeedX { get; set; }
        public float MultiTextureSpeedZ { get; set; }
        public float Random { get; set; }
        public float AdjustHeightFar { get; set; }
        public float AdjustHeightNear { get; set; }
        public float AdjustHeightValue { get; set; }
        public float AlphaBlend { get; set; }

        
        public override void SerializeImpl(SerializerObject s)
        {
            Translation = s.SerializeObject<KlonoaLV_FloatVector>(Translation, onPreSerialize: x => x.Pre_HasW = true, name: nameof(Translation));
            Rotation = s.SerializeObject<KlonoaLV_FloatVector>(Rotation, onPreSerialize: x => x.Pre_HasW = true, name: nameof(Rotation));
            Light_0 = s.SerializeObject<KlonoaLV_FloatVector>(Light_0, onPreSerialize: x => x.Pre_HasW = true, name: nameof(Light_0));
            Light_1 = s.SerializeObject<KlonoaLV_FloatVector>(Light_1, onPreSerialize: x => x.Pre_HasW = true, name: nameof(Light_1));
            Light_2 = s.SerializeObject<KlonoaLV_FloatVector>(Light_2, onPreSerialize: x => x.Pre_HasW = true, name: nameof(Light_2));
            LightColor_0 = s.SerializeObject<KlonoaLV_FloatVector>(LightColor_0, onPreSerialize: x => x.Pre_HasW = true, name: nameof(LightColor_0));
            LightColor_1 = s.SerializeObject<KlonoaLV_FloatVector>(LightColor_1, onPreSerialize: x => x.Pre_HasW = true, name: nameof(LightColor_1));
            LightColor_2 = s.SerializeObject<KlonoaLV_FloatVector>(LightColor_2, onPreSerialize: x => x.Pre_HasW = true, name: nameof(LightColor_2));
            AmbientColor = s.SerializeObject<KlonoaLV_FloatVector>(AmbientColor, onPreSerialize: x => x.Pre_HasW = true, name: nameof(AmbientColor));
            FogColor = s.SerializeObject<KlonoaLV_FloatVector>(FogColor, onPreSerialize: x => x.Pre_HasW = true, name: nameof(FogColor));
            DrawEnable = s.Serialize<byte>(DrawEnable, name: nameof(DrawEnable));
            Primitive = s.Serialize<byte>(Primitive, name: nameof(Primitive));
            LOD = s.Serialize<byte>(LOD, name: nameof(LOD));
            MMesh = s.Serialize<byte>(MMesh, name: nameof(MMesh));
            BaseTexture = s.Serialize<byte>(BaseTexture, name: nameof(BaseTexture));
            Fog = s.Serialize<byte>(Fog, name: nameof(Fog));
            MultiTexture = s.Serialize<byte>(MultiTexture, name: nameof(MultiTexture));
            HeadTexture = s.Serialize<byte>(HeadTexture, name: nameof(HeadTexture));
            HeadTextureValue = s.Serialize<byte>(HeadTextureValue, name: nameof(HeadTextureValue));
            HeightAdjust = s.Serialize<byte>(HeightAdjust, name: nameof(HeightAdjust));
            BaseTextureArea = s.Serialize<byte>(BaseTextureArea, name: nameof(BaseTextureArea));
            s.SerializePadding(1, name: nameof(Translation));
            PlaneSizeX = s.Serialize<float>(PlaneSizeX, name: nameof(PlaneSizeX));
            PlaneSizeZ = s.Serialize<float>(PlaneSizeZ, name: nameof(PlaneSizeZ));
            MeshSizeX = s.Serialize<float>(MeshSizeX, name: nameof(MeshSizeX));
            MeshSizeZ = s.Serialize<float>(MeshSizeZ, name: nameof(MeshSizeZ));
            Height = s.Serialize<float>(Height, name: nameof(Height));
            IntervalX = s.Serialize<float>(IntervalX, name: nameof(IntervalX));
            IntervalZ = s.Serialize<float>(IntervalZ, name: nameof(IntervalZ));
            RadiusX = s.Serialize<float>(RadiusX, name: nameof(RadiusX));
            RadiusZ = s.Serialize<float>(RadiusZ, name: nameof(RadiusZ));
            TextureAdjust = s.Serialize<float>(TextureAdjust, name: nameof(TextureAdjust));
            SpeedX = s.Serialize<float>(SpeedX, name: nameof(SpeedX));
            SpeedZ = s.Serialize<float>(SpeedZ, name: nameof(SpeedZ));
            Shear = s.Serialize<float>(Shear, name: nameof(Shear));
            BaseTextureSpeedX = s.Serialize<float>(BaseTextureSpeedX, name: nameof(BaseTextureSpeedX));
            BaseTextureSpeedZ = s.Serialize<float>(BaseTextureSpeedZ, name: nameof(BaseTextureSpeedZ));
            MultiTextureSpeedX = s.Serialize<float>(MultiTextureSpeedX, name: nameof(MultiTextureSpeedX));
            MultiTextureSpeedZ = s.Serialize<float>(MultiTextureSpeedZ, name: nameof(MultiTextureSpeedZ));
            Random = s.Serialize<float>(Random, name: nameof(Random)); // Padding?
            AdjustHeightFar = s.Serialize<float>(AdjustHeightFar, name: nameof(AdjustHeightFar));
            AdjustHeightNear = s.Serialize<float>(AdjustHeightNear, name: nameof(AdjustHeightNear));
            AdjustHeightValue = s.Serialize<float>(AdjustHeightValue, name: nameof(AdjustHeightValue));
            AlphaBlend = s.Serialize<float>(AlphaBlend, name: nameof(AlphaBlend));
            s.SerializePadding(12, name: nameof(Translation));
        }
    }
}