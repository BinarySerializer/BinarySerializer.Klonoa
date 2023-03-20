namespace BinarySerializer.Klonoa.LV
{
    public class BackgroundAnimation_Command : BinarySerializable
    {
        public BackgroundAnimation_CommandType Type { get; set; }

        #region Data Fields
        public int JumpOffset { get; set; } // Jump command
        public int DMAtagIndex { get; set; } // DMAtag command
        public int Time { get; set; } // Time command
        public float Red { get; set; } // VCS commands
        public float Green { get; set; } // VCS commands
        public float Blue { get; set; } // VCS commands
        public float FloatX { get; set; } // Scale commands
        public float FloatY { get; set; } // Scale commands
        public float FloatZ { get; set; } // Scale commands
        public int IntX { get; set; } // Rotation commands
        public int IntY { get; set; } // Rotation commands
        #endregion

        public override void SerializeImpl(SerializerObject s)
        {
            Type = s.Serialize<BackgroundAnimation_CommandType>(Type, nameof(Type));
            
            switch (Type)
            {
                case BackgroundAnimation_CommandType.Flag0:
                    break;
                case BackgroundAnimation_CommandType.Jump:
                    JumpOffset = s.Serialize<int>(JumpOffset, name: nameof(JumpOffset));
                    break;
                case BackgroundAnimation_CommandType.DMAtag:
                    DMAtagIndex = s.Serialize<int>(DMAtagIndex, name: nameof(DMAtagIndex));
                    break;
                case BackgroundAnimation_CommandType.Time:
                    Time = s.Serialize<int>(Time, name: nameof(Time));
                    break;
                case BackgroundAnimation_CommandType.Flag1:
                    break;
                case BackgroundAnimation_CommandType.VCS_RGB:
                    Red = s.Serialize<float>(Red, name: nameof(Red));
                    Green = s.Serialize<float>(Green, name: nameof(Green));
                    Blue = s.Serialize<float>(Blue, name: nameof(Blue));
                    break;
                case BackgroundAnimation_CommandType.VCS_R:
                    Red = s.Serialize<float>(Red, name: nameof(Red));
                    break;
                case BackgroundAnimation_CommandType.VCS_G:
                    Green = s.Serialize<float>(Green, name: nameof(Green));
                    break;
                case BackgroundAnimation_CommandType.VCS_B:
                    Blue = s.Serialize<float>(Blue, name: nameof(Blue));
                    break;
                case BackgroundAnimation_CommandType.Scale_XYZ:
                    FloatX = s.Serialize<float>(FloatX, name: nameof(FloatX));
                    FloatY = s.Serialize<float>(FloatY, name: nameof(FloatY));
                    FloatZ = s.Serialize<float>(FloatZ, name: nameof(FloatZ));
                    break;
                case BackgroundAnimation_CommandType.Scale_X:
                    FloatX = s.Serialize<float>(FloatX, name: nameof(FloatX));
                    break;
                case BackgroundAnimation_CommandType.Scale_Y:
                    FloatY = s.Serialize<float>(FloatY, name: nameof(FloatY));
                    break;
                case BackgroundAnimation_CommandType.Scale_Z:
                    FloatZ = s.Serialize<float>(FloatZ, name: nameof(FloatZ));
                    break;
                case BackgroundAnimation_CommandType.ScaleSpeed_XYZ:
                    FloatX = s.Serialize<float>(FloatX, name: nameof(FloatX));
                    FloatY = s.Serialize<float>(FloatY, name: nameof(FloatY));
                    FloatZ = s.Serialize<float>(FloatZ, name: nameof(FloatZ));
                    break;
                case BackgroundAnimation_CommandType.ScaleSpeed_X:
                    FloatX = s.Serialize<float>(FloatX, name: nameof(FloatX));
                    break;
                case BackgroundAnimation_CommandType.ScaleSpeed_Y:
                    FloatY = s.Serialize<float>(FloatY, name: nameof(FloatY));
                    break;
                case BackgroundAnimation_CommandType.ScaleSpeed_Z:
                    FloatZ = s.Serialize<float>(FloatZ, name: nameof(FloatZ));
                    break;
                case BackgroundAnimation_CommandType.Rotation_XY:
                    IntX = s.Serialize<int>(IntX, name: nameof(IntX));
                    IntY = s.Serialize<int>(IntY, name: nameof(IntY));
                    break;
                case BackgroundAnimation_CommandType.Rotation_X:
                    IntX = s.Serialize<int>(IntX, name: nameof(IntX));
                    break;
                case BackgroundAnimation_CommandType.Rotation_Y:
                    IntY = s.Serialize<int>(IntY, name: nameof(IntY));
                    break;
                case BackgroundAnimation_CommandType.RotationSpeed_XY:
                    IntX = s.Serialize<int>(IntX, name: nameof(IntX));
                    IntY = s.Serialize<int>(IntY, name: nameof(IntY));
                    break;
                case BackgroundAnimation_CommandType.RotationSpeed_X:
                    IntX = s.Serialize<int>(IntX, name: nameof(IntX));
                    break;
                case BackgroundAnimation_CommandType.RotationSpeed_Y:
                    IntY = s.Serialize<int>(IntY, name: nameof(IntY));
                    break;
                case BackgroundAnimation_CommandType.RotationAcceleration_XY:
                    IntX = s.Serialize<int>(IntX, name: nameof(IntX));
                    IntY = s.Serialize<int>(IntY, name: nameof(IntY));
                    break;
                case BackgroundAnimation_CommandType.RotationAcceleration_X:
                    IntX = s.Serialize<int>(IntX, name: nameof(IntX));
                    break;
                case BackgroundAnimation_CommandType.RotationAcceleration_Y:
                    IntY = s.Serialize<int>(IntY, name: nameof(IntY));
                    break;
                case BackgroundAnimation_CommandType.Flag8:
                    break;
            }
        }
    }

    public enum BackgroundAnimation_CommandType {
        Flag0,
        Jump,
        DMAtag,
        Time,
        Flag1,
        VCS_RGB,
        VCS_R,
        VCS_G,
        VCS_B,
        Scale_XYZ,
        Scale_X,
        Scale_Y,
        Scale_Z,
        ScaleSpeed_XYZ,
        ScaleSpeed_X,
        ScaleSpeed_Y,
        ScaleSpeed_Z,
        Rotation_XY,
        Rotation_X,
        Rotation_Y,
        RotationSpeed_XY,
        RotationSpeed_X,
        RotationSpeed_Y,
        RotationAcceleration_XY,
        RotationAcceleration_X,
        RotationAcceleration_Y,
        Flag8
    }
}