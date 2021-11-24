using System;

namespace BinarySerializer.Klonoa.KH
{
    public class CutsceneCommand : BinarySerializable
    {
        // Functions at 0x0825135c
        public byte SecondaryType { get; set; }
        public byte PrimaryType { get; set; }
        public CommandType Type { get; set; }
        public bool EndOfFrame { get; set; }

        // Unknown arguments
        public short Arg1_Short { get; set; }
        public bool Arg1_Bool { get; set; }
        public short Arg2_Short { get; set; }
        public bool Arg2_Bool { get; set; }
        public short Arg3_Short { get; set; }
        public short Arg4_Short { get; set; }
        public uint Arg4_Uint { get; set; }
        public short Arg5_Short { get; set; }
        public short Arg_Padding { get; set; }
        
        // Generic arguments
        public short CommandOffset1 { get; set; } // Value * 4
        public short CommandOffset2 { get; set; } // Value * 4
        public short Frames { get; set; }
        
        // Conditional
        public bool InvertCondition { get; set; }
        public bool ForceConditionToFalse { get; set; }

        // Cutscene file indexes for where the current file is
        public short FileIndex_0 { get; set; }
        public short FileIndex_1 { get; set; }
        public short FileIndex_2 { get; set; }
        public short FileIndexRelated_3 { get; set; } // ?

        // Text
        public short TextOffsetOffset { get; set; } // Value * 4
        public int TextOffset { get; set; } // Value * 4
        public TextCommands TextCommands { get; set; }

        // Text array
        public short DefaultTextIndex { get; set; }
        public ArchiveFile<TextCommands> TextCommandsArray { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SecondaryType = s.Serialize<byte>(SecondaryType, name: nameof(SecondaryType));
            PrimaryType = s.Serialize<byte>(PrimaryType, name: nameof(PrimaryType));

            Type = (CommandType)(PrimaryType * 100 + SecondaryType);
            s.Log($"Type: {Type}");

            if (!Enum.IsDefined(typeof(CommandType), Type))
                throw new BinarySerializableException(this, $"Invalid command type {PrimaryType}-{SecondaryType}");

            switch (Type)
            {
                case CommandType.CreateContextAt: // The cutscene can have 16 "contexts" which all run commands from different offsets in the script
                    CommandOffset1 = s.Serialize<short>(CommandOffset1, name: nameof(CommandOffset1));
                    break;

                case CommandType.End_0:
                case CommandType.End_1:
                    EndOfFrame = true;
                    break;

                case CommandType.Wait:
                    Frames = s.Serialize<short>(Frames, name: nameof(Frames));
                    EndOfFrame = true;
                    break;

                case CommandType.Call: // Goes to offset and saves current position to go back to when hitting a return
                    CommandOffset1 = s.Serialize<short>(CommandOffset1, name: nameof(CommandOffset1));
                    break;

                case CommandType.Return: // Return to the saved position
                    break;

                case CommandType.CMD_00_06:
                    Arg1_Bool = s.Serialize<bool>(Arg1_Bool, name: nameof(Arg1_Bool));
                    s.SerializePadding(1);
                    break;

                case CommandType.GoTo:
                    CommandOffset1 = s.Serialize<short>(CommandOffset1, name: nameof(CommandOffset1));
                    break;

                case CommandType.CMD_00_08:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    Arg4_Short = s.Serialize<short>(Arg4_Short, name: nameof(Arg4_Short));
                    Arg5_Short = s.Serialize<short>(Arg5_Short, name: nameof(Arg5_Short));
                    break;

                case CommandType.ConditionalGoTo_0:
                    InvertCondition = s.Serialize<bool>(InvertCondition, name: nameof(InvertCondition));
                    s.SerializePadding(1);
                    ForceConditionToFalse = s.Serialize<bool>(ForceConditionToFalse, name: nameof(ForceConditionToFalse));
                    s.SerializePadding(1);
                    CommandOffset1 = s.Serialize<short>(CommandOffset1, name: nameof(CommandOffset1));
                    Arg4_Uint = s.Serialize<uint>(Arg4_Uint, name: nameof(Arg4_Uint)); // Related to the condition
                    break;
                
                case CommandType.SetText:
                    TextOffsetOffset = s.Serialize<short>(TextOffsetOffset, name: nameof(TextOffsetOffset));
                    // Why does the game have an offset to an offset?? Seems to always be 1.
                    s.DoAt(GetPointerFromOffset(TextOffsetOffset), () => 
                        TextOffset = s.Serialize<int>(TextOffset, name: nameof(TextOffset)));
                    s.DoAt(Offset + TextOffsetOffset * 4 + TextOffset * 4, () =>
                        TextCommands = s.SerializeObject<TextCommands>(TextCommands, name: nameof(TextCommands)));
                    break;

                case CommandType.CMD_00_11:
                    break;

                case CommandType.CMD_00_12:
                case CommandType.CMD_00_13:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_00_14:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.UnknownEndOfFrame:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    EndOfFrame = true;
                    break;
                
                case CommandType.CMD_00_16:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short)); // Offset to some temp buffer with two values
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short)); // The value to set the temp buffer value to if 0, otherwise decrease
                    CommandOffset1 = s.Serialize<short>(CommandOffset1, name: nameof(CommandOffset1));
                    break;

                case CommandType.CMD_00_17:
                    break;

                case CommandType.CMD_00_18:
                case CommandType.CMD_00_19:
                case CommandType.CMD_00_20:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_00_21:
                case CommandType.CMD_00_22:
                case CommandType.CMD_00_23:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;
                
                case CommandType.CMD_00_24:
                case CommandType.CMD_00_25:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    Arg4_Short = s.Serialize<short>(Arg4_Short, name: nameof(Arg4_Short));
                    Arg5_Short = s.Serialize<short>(Arg5_Short, name: nameof(Arg5_Short));
                    break;

                case CommandType.CMD_00_26:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_00_27:
                case CommandType.CMD_00_28:
                case CommandType.CMD_00_29:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_00_30:
                case CommandType.CMD_00_31:
                case CommandType.CMD_00_32:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;

                case CommandType.CMD_00_33:
                case CommandType.CMD_00_34:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    Arg4_Short = s.Serialize<short>(Arg4_Short, name: nameof(Arg4_Short));
                    Arg5_Short = s.Serialize<short>(Arg5_Short, name: nameof(Arg5_Short));
                    break;

                case CommandType.CMD_00_35:
                case CommandType.CMD_00_36:
                case CommandType.CMD_00_37:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_00_38:
                case CommandType.CMD_00_39:
                case CommandType.CMD_00_40:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_00_41:
                    Arg1_Bool = s.Serialize<bool>(Arg1_Bool, name: nameof(Arg1_Bool));
                    s.SerializePadding(1);
                    break;

                case CommandType.CMD_00_42:
                case CommandType.CMD_00_43:
                case CommandType.CMD_00_44:
                case CommandType.CMD_00_45:
                case CommandType.CMD_00_46:
                case CommandType.CMD_00_47:
                case CommandType.CMD_00_48:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_00_49:
                    break;

                case CommandType.CMD_00_50:
                case CommandType.CMD_00_51:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;
                
                case CommandType.ConditionalGoTo_1: // These two are the same except for the memory value it checks
                case CommandType.ConditionalGoTo_2:
                    InvertCondition = s.Serialize<bool>(InvertCondition, name: nameof(InvertCondition));
                    s.SerializePadding(1);
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short)); // Checks if this value equals some other value in memory
                    CommandOffset1 = s.Serialize<short>(CommandOffset1, name: nameof(CommandOffset1));
                    break;

                case CommandType.CMD_00_54:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_00_55:
                    break;

                case CommandType.FileReference:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    s.DoBits<short>(b =>
                    {
                        FileIndex_2 = b.SerializeBits<short>(FileIndex_2, 4, name: nameof(FileIndex_2));
                        FileIndex_1 = b.SerializeBits<short>(FileIndex_1, 4, name: nameof(FileIndex_1));
                        FileIndex_0 = b.SerializeBits<short>(FileIndex_0, 4, name: nameof(FileIndex_0));
                        FileIndexRelated_3 = b.SerializeBits<short>(FileIndexRelated_3, 4, name: nameof(FileIndexRelated_3));
                    });
                    break;

                case CommandType.ConditionalGoTo_3:
                    InvertCondition = s.Serialize<bool>(InvertCondition, name: nameof(InvertCondition));
                    s.SerializePadding(1);
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short)); // Value used to determine condition
                    CommandOffset1 = s.Serialize<short>(CommandOffset1, name: nameof(CommandOffset1));
                    break;


                case CommandType.CMD_01_00:
                case CommandType.CMD_01_01:
                case CommandType.CMD_01_02:
                case CommandType.CMD_01_03:
                case CommandType.CMD_01_04:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_01_05: // Null
                case CommandType.CMD_01_06:
                case CommandType.CMD_01_07:
                    throw new BinarySerializableException(this, $"Invalid command type {PrimaryType}-{SecondaryType}");

                case CommandType.Blank_0: // Does nothing
                case CommandType.Blank_1:
                case CommandType.Blank_2:
                case CommandType.Blank_3:
                case CommandType.Blank_4:
                case CommandType.Blank_5:
                case CommandType.Blank_6:
                case CommandType.Blank_7:
                case CommandType.Blank_8:
                case CommandType.Blank_9:
                case CommandType.Blank_10:
                case CommandType.Blank_11:
                    break;

                case CommandType.CMD_01_20:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_01_21:
                case CommandType.CMD_01_22:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;

                case CommandType.CMD_01_23:
                case CommandType.CMD_01_24:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_01_25:
                case CommandType.CMD_01_26:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;

                case CommandType.CMD_01_27:
                case CommandType.CMD_01_28:
                case CommandType.CMD_01_29:
                case CommandType.CMD_01_30:
                case CommandType.CMD_01_31:
                case CommandType.CMD_01_32:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_01_33:
                case CommandType.CMD_01_34:
                case CommandType.CMD_01_35:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_01_36:
                case CommandType.CMD_01_37:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;

                case CommandType.CMD_01_38:
                case CommandType.CMD_01_39:
                case CommandType.CMD_01_40:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_01_41:
                case CommandType.CMD_01_42:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;

                case CommandType.CMD_01_43:
                case CommandType.CMD_01_44:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_01_45:
                case CommandType.CMD_01_46:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_01_47:
                    Arg1_Bool = s.Serialize<bool>(Arg1_Bool, name: nameof(Arg1_Bool));
                    s.SerializePadding(1);
                    break;

                case CommandType.CMD_01_48:
                case CommandType.CMD_01_49:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_01_50:
                    Arg1_Bool = s.Serialize<bool>(Arg1_Bool, name: nameof(Arg1_Bool));
                    s.SerializePadding(1);
                    break;

                case CommandType.CMD_01_51:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_01_52:
                case CommandType.CMD_01_53:
                case CommandType.CMD_01_54:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_01_55:
                case CommandType.CMD_01_56:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;

                case CommandType.CMD_01_57:
                case CommandType.CMD_01_58:
                case CommandType.CMD_01_59:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_01_60:
                case CommandType.CMD_01_61:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;
                
                case CommandType.CMD_01_62:
                case CommandType.CMD_01_63:
                    Arg1_Bool = s.Serialize<bool>(Arg1_Bool, name: nameof(Arg1_Bool));
                    s.SerializePadding(1);
                    break;

                case CommandType.CMD_01_64:
                case CommandType.CMD_01_65:
                case CommandType.CMD_01_66:
                case CommandType.CMD_01_67:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_01_68:
                case CommandType.CMD_01_69:
                case CommandType.CMD_01_70:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_01_71:
                case CommandType.CMD_01_72:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;

                case CommandType.CMD_01_73:
                case CommandType.CMD_01_74:
                case CommandType.CMD_01_75:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_01_76:
                case CommandType.CMD_01_77:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;

                case CommandType.CMD_01_78:
                    Arg1_Bool = s.Serialize<bool>(Arg1_Bool, name: nameof(Arg1_Bool));
                    s.SerializePadding(1);
                    break;

                case CommandType.CMD_01_79:
                case CommandType.CMD_01_80:
                case CommandType.CMD_01_81:
                case CommandType.CMD_01_82:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_01_83:
                case CommandType.CMD_01_84:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    break;

                case CommandType.CMD_01_85:
                    Arg1_Bool = s.Serialize<bool>(Arg1_Bool, name: nameof(Arg1_Bool));
                    s.SerializePadding(1);
                    break;

                case CommandType.CMD_01_86:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_01_87:
                case CommandType.CMD_01_88:
                case CommandType.CMD_01_89:
                    // Note really 3 params like this, seems some are bytes
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;


                case CommandType.CMD_02_00:
                case CommandType.CMD_02_01:
                case CommandType.CMD_02_02:
                case CommandType.CMD_02_03:
                case CommandType.CMD_02_04:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.CMD_02_05:
                    // Are some padding?
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    Arg2_Short = s.Serialize<short>(Arg2_Short, name: nameof(Arg2_Short));
                    Arg3_Short = s.Serialize<short>(Arg3_Short, name: nameof(Arg3_Short));
                    break;

                case CommandType.CMD_02_06:
                case CommandType.CMD_02_07:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;


                // Some of the 03 ones are just padding
                case CommandType.CMD_03_00:
                case CommandType.CMD_03_01:
                case CommandType.CMD_03_02:
                case CommandType.CMD_03_03:
                case CommandType.CMD_03_04:
                case CommandType.CMD_03_05:
                case CommandType.CMD_03_06:
                case CommandType.CMD_03_07:
                case CommandType.CMD_03_08:
                case CommandType.CMD_03_09:
                case CommandType.CMD_03_10:
                case CommandType.CMD_03_11:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.Blank_12: // Does nothing
                case CommandType.Blank_13:
                    break;

                case CommandType.CMD_03_14:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.Blank_14: // Does nothing
                case CommandType.Blank_15:
                    break;

                case CommandType.CMD_03_17:
                case CommandType.CMD_03_18:
                case CommandType.CMD_03_19:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.ConditionalMultiGoTo: // Conditionally go to either one or none
                    CommandOffset1 = s.Serialize<short>(CommandOffset1, name: nameof(CommandOffset1));
                    CommandOffset2 = s.Serialize<short>(CommandOffset2, name: nameof(CommandOffset2));
                    break;

                case CommandType.ConditionalGoTo_4:
                    CommandOffset1 = s.Serialize<short>(CommandOffset1, name: nameof(CommandOffset1));
                    break;

                case CommandType.CMD_03_22:
                case CommandType.CMD_03_23:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                case CommandType.SetTextMulti:
                    TextOffsetOffset = s.Serialize<short>(TextOffsetOffset, name: nameof(TextOffsetOffset));
                    DefaultTextIndex = s.Serialize<short>(DefaultTextIndex, name: nameof(DefaultTextIndex));

                    s.DoAt(GetPointerFromOffset(TextOffsetOffset), () =>
                        TextOffset = s.Serialize<int>(TextOffset, name: nameof(TextOffset)));
                    s.DoAt(Offset + TextOffsetOffset * 4 + TextOffset * 4, () =>
                        TextCommandsArray = s.SerializeObject<ArchiveFile<TextCommands>>(TextCommandsArray, name: nameof(TextCommandsArray)));
                    break;

                case CommandType.CMD_03_25:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short)); // If 2 it loads graphics
                    break;

                case CommandType.CMD_03_26:
                    Arg1_Short = s.Serialize<short>(Arg1_Short, name: nameof(Arg1_Short));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Parse the padding rather than skip it in case there was some data missed by accident. That way it doesn't get lost when writing back.
            if (s.CurrentFileOffset % 4 != 0)
            {
                Arg_Padding = s.Serialize<short>(Arg_Padding, name: nameof(Arg_Padding));

                if (Arg_Padding != 0)
                    s.LogWarning($"Padding for command {PrimaryType}-{SecondaryType} has data!");
            }
        }

        public Pointer GetPointerFromOffset(int offset) => Offset + offset * 4;

        public enum CommandType
        {
            CreateContextAt = 0000, 
            End_0 = 0001,
            End_1 = 0002,
            Wait = 0003,
            Call = 0004,
            Return = 0005,
            CMD_00_06 = 0006,
            GoTo = 0007,
            CMD_00_08 = 0008,
            ConditionalGoTo_0 = 0009,
            SetText = 0010,
            CMD_00_11 = 0011,
            CMD_00_12 = 0012,
            CMD_00_13 = 0013,
            CMD_00_14 = 0014,
            UnknownEndOfFrame = 0015,
            CMD_00_16 = 0016,
            CMD_00_17 = 0017,
            CMD_00_18 = 0018,
            CMD_00_19 = 0019,
            CMD_00_20 = 0020,
            CMD_00_21 = 0021,
            CMD_00_22 = 0022,
            CMD_00_23 = 0023,
            CMD_00_24 = 0024,
            CMD_00_25 = 0025,
            CMD_00_26 = 0026,
            CMD_00_27 = 0027,
            CMD_00_28 = 0028,
            CMD_00_29 = 0029,
            CMD_00_30 = 0030,
            CMD_00_31 = 0031,
            CMD_00_32 = 0032,
            CMD_00_33 = 0033,
            CMD_00_34 = 0034,
            CMD_00_35 = 0035,
            CMD_00_36 = 0036,
            CMD_00_37 = 0037,
            CMD_00_38 = 0038,
            CMD_00_39 = 0039,
            CMD_00_40 = 0040,
            CMD_00_41 = 0041,
            CMD_00_42 = 0042,
            CMD_00_43 = 0043,
            CMD_00_44 = 0044,
            CMD_00_45 = 0045,
            CMD_00_46 = 0046,
            CMD_00_47 = 0047,
            CMD_00_48 = 0048,
            CMD_00_49 = 0049,
            CMD_00_50 = 0050,
            CMD_00_51 = 0051,
            ConditionalGoTo_1 = 0052,
            ConditionalGoTo_2 = 0053,
            CMD_00_54 = 0054,
            CMD_00_55 = 0055,
            FileReference = 0056,
            ConditionalGoTo_3 = 0057,

            CMD_01_00 = 0100,
            CMD_01_01 = 0101,
            CMD_01_02 = 0102,
            CMD_01_03 = 0103,
            CMD_01_04 = 0104,
            CMD_01_05 = 0105,
            CMD_01_06 = 0106,
            CMD_01_07 = 0107,
            Blank_0 = 0108,
            Blank_1 = 0109,
            Blank_2 = 0110,
            Blank_3 = 0111,
            Blank_4 = 0112,
            Blank_5 = 0113,
            Blank_6 = 0114,
            Blank_7 = 0115,
            Blank_8 = 0116,
            Blank_9 = 0117,
            Blank_10 = 0118,
            Blank_11 = 0119,
            CMD_01_20 = 0120,
            CMD_01_21 = 0121,
            CMD_01_22 = 0122,
            CMD_01_23 = 0123,
            CMD_01_24 = 0124,
            CMD_01_25 = 0125,
            CMD_01_26 = 0126,
            CMD_01_27 = 0127,
            CMD_01_28 = 0128,
            CMD_01_29 = 0129,
            CMD_01_30 = 0130,
            CMD_01_31 = 0131,
            CMD_01_32 = 0132,
            CMD_01_33 = 0133,
            CMD_01_34 = 0134,
            CMD_01_35 = 0135,
            CMD_01_36 = 0136,
            CMD_01_37 = 0137,
            CMD_01_38 = 0138,
            CMD_01_39 = 0139,
            CMD_01_40 = 0140,
            CMD_01_41 = 0141,
            CMD_01_42 = 0142,
            CMD_01_43 = 0143,
            CMD_01_44 = 0144,
            CMD_01_45 = 0145,
            CMD_01_46 = 0146,
            CMD_01_47 = 0147,
            CMD_01_48 = 0148,
            CMD_01_49 = 0149,
            CMD_01_50 = 0150,
            CMD_01_51 = 0151,
            CMD_01_52 = 0152,
            CMD_01_53 = 0153,
            CMD_01_54 = 0154,
            CMD_01_55 = 0155,
            CMD_01_56 = 0156,
            CMD_01_57 = 0157,
            CMD_01_58 = 0158,
            CMD_01_59 = 0159,
            CMD_01_60 = 0160,
            CMD_01_61 = 0161,
            CMD_01_62 = 0162,
            CMD_01_63 = 0163,
            CMD_01_64 = 0164,
            CMD_01_65 = 0165,
            CMD_01_66 = 0166,
            CMD_01_67 = 0167,
            CMD_01_68 = 0168,
            CMD_01_69 = 0169,
            CMD_01_70 = 0170,
            CMD_01_71 = 0171,
            CMD_01_72 = 0172,
            CMD_01_73 = 0173,
            CMD_01_74 = 0174,
            CMD_01_75 = 0175,
            CMD_01_76 = 0176,
            CMD_01_77 = 0177,
            CMD_01_78 = 0178,
            CMD_01_79 = 0179,
            CMD_01_80 = 0180,
            CMD_01_81 = 0181,
            CMD_01_82 = 0182,
            CMD_01_83 = 0183,
            CMD_01_84 = 0184,
            CMD_01_85 = 0185,
            CMD_01_86 = 0186,
            CMD_01_87 = 0187,
            CMD_01_88 = 0188,
            CMD_01_89 = 0189,

            CMD_02_00 = 0200,
            CMD_02_01 = 0201,
            CMD_02_02 = 0202,
            CMD_02_03 = 0203,
            CMD_02_04 = 0204,
            CMD_02_05 = 0205,
            CMD_02_06 = 0206,
            CMD_02_07 = 0207,

            CMD_03_00 = 0300,
            CMD_03_01 = 0301,
            CMD_03_02 = 0302,
            CMD_03_03 = 0303,
            CMD_03_04 = 0304,
            CMD_03_05 = 0305,
            CMD_03_06 = 0306,
            CMD_03_07 = 0307,
            CMD_03_08 = 0308,
            CMD_03_09 = 0309,
            CMD_03_10 = 0310,
            CMD_03_11 = 0311,
            Blank_12 = 0312,
            Blank_13 = 0313,
            CMD_03_14 = 0314,
            Blank_14 = 0315,
            Blank_15 = 0316,
            CMD_03_17 = 0317,
            CMD_03_18 = 0318,
            CMD_03_19 = 0319,
            ConditionalMultiGoTo = 0320,
            ConditionalGoTo_4 = 0321,
            CMD_03_22 = 0322,
            CMD_03_23 = 0323,
            SetTextMulti = 0324,
            CMD_03_25 = 0325,
            CMD_03_26 = 0326,
        }
    }
}