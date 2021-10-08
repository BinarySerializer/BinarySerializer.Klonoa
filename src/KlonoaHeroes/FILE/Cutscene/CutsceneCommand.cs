using System;

namespace BinarySerializer.Klonoa.KH
{
    public class CutsceneCommand : BinarySerializable
    {
        // Functions at 0x0825135c
        public byte SecondaryType { get; set; }
        public byte PrimaryType { get; set; }
        public bool EndOfFrame { get; set; }

        // Unknown arguments
        public ushort Arg_Ushort_02 { get; set; }
        public ushort Arg_Ushort_04 { get; set; }
        public ushort Arg_Offset { get; set; } // Value * 2

        // Cutscene file indexes for where the current file is
        public byte FileIndex_0 { get; set; }
        public byte FileIndex_1 { get; set; }
        public byte FileIndex_2 { get; set; }

        // Text
        public ushort TextOffset { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SecondaryType = s.Serialize<byte>(SecondaryType, name: nameof(SecondaryType));
            PrimaryType = s.Serialize<byte>(PrimaryType, name: nameof(PrimaryType));

            switch (PrimaryType)
            {
                case 0:
                    switch (SecondaryType)
                    {
                        case 0:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 3:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            EndOfFrame = true;
                            break;

                        case 4:
                            Arg_Offset = s.Serialize<ushort>(Arg_Offset, name: nameof(Arg_Offset)); // Conditional goto
                            break;

                        case 7:
                            Arg_Offset = s.Serialize<ushort>(Arg_Offset, name: nameof(Arg_Offset)); // Goto
                            break;

                        case 10:
                            TextOffset = s.Serialize<ushort>(TextOffset, name: nameof(TextOffset));
                            // TODO: Parse text at offset
                            break;

                        case 12:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 15:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            EndOfFrame = true;
                            break;

                        case 41:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 43:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 45:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 47:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 56:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            s.SerializeBitValues<ushort>(bitFunc =>
                            {
                                FileIndex_2 = (byte)bitFunc(FileIndex_2, 4, name: nameof(FileIndex_2));
                                FileIndex_1 = (byte)bitFunc(FileIndex_1, 4, name: nameof(FileIndex_1));
                                FileIndex_0 = (byte)bitFunc(FileIndex_0, 4, name: nameof(FileIndex_0));
                            });
                            s.SerializePadding(2, logIfNotNull: true);
                            break;

                        default:
                            throw new NotImplementedException($"Not implemented cutscene command {PrimaryType}-{SecondaryType}");
                    }
                    break;

                case 1:
                    switch (SecondaryType)
                    {
                        case 4:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 23:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 51:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 82:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        default:
                            throw new NotImplementedException($"Not implemented cutscene command {PrimaryType}-{SecondaryType}");
                    }
                    break;

                case 2:
                    switch (SecondaryType)
                    {
                        case 0:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02)); // Checks for 0x18
                            break;

                        case 1:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 2:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 3:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        default:
                            throw new NotImplementedException($"Not implemented cutscene command {PrimaryType}-{SecondaryType}");
                    }
                    break;

                case 3:
                    switch (SecondaryType)
                    {
                        case 0:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 2:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 8:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 10:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02)); // A bool, either 0 or 1
                            break;

                        case 11:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 14:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        case 17:
                            Arg_Ushort_02 = s.Serialize<ushort>(Arg_Ushort_02, name: nameof(Arg_Ushort_02));
                            break;

                        default:
                            throw new NotImplementedException($"Not implemented cutscene command {PrimaryType}-{SecondaryType}");
                    }
                    break;

                default:
                    throw new BinarySerializableException(this, $"Invalid primary type {PrimaryType}");
            }
        }
    }
}