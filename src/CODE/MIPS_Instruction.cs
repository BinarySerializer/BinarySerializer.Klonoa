using UnityEngine;

namespace BinarySerializer.KlonoaDTP
{
    public class MIPS_Instruction : BinarySerializable
    {
        public int Value { get; set; }

        public int Funct { get; set; }
        public int Shift { get; set; }
        public int RD { get; set; }
        public int RT { get; set; }
        public int RS { get; set; }
        public int Opcode { get; set; }

        public int IMM { get; set; }

        public InstructionMnemonic Mnemonic { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Value = s.Serialize<int>(Value, name: nameof(Value));

            Opcode = BitHelpers.ExtractBits(Value, 6, 26);
            s.Log($"{nameof(Opcode)}: {Opcode}");

            if (Value == 0x00)
            {
                Mnemonic = InstructionMnemonic.nop;
            }
            else if (Opcode == 0x00)
            {
                Funct = BitHelpers.ExtractBits(Value, 6, 0);

                if (Funct == 0x00)
                    Mnemonic = InstructionMnemonic.sll;
                else if (Funct == 0x21)
                    Mnemonic = InstructionMnemonic.addu;
                else if (Funct == 0x08)
                    Mnemonic = InstructionMnemonic.jr;
                else
                    Debug.LogWarning($"Unknown MIPS instruction with opcode {Opcode} and funt {Funct}");
            }
            else if (Opcode == 0x0F)
                Mnemonic = InstructionMnemonic.lui;
            else if (Opcode == 0x23)
                Mnemonic = InstructionMnemonic.lw;
            else
                Debug.LogWarning($"Unknown MIPS instruction with opcode {Opcode}");

            s.Log($"{nameof(Mnemonic)}: {Mnemonic}");

            switch (Mnemonic)
            {
                // R Instructions
                case InstructionMnemonic.sll:
                case InstructionMnemonic.addu:
                case InstructionMnemonic.jr:
                    Funct = BitHelpers.ExtractBits(Value, 6, 0);
                    s.Log($"{nameof(Funct)}: {Funct}");
                    Shift = BitHelpers.ExtractBits(Value, 5, 6);
                    s.Log($"{nameof(Shift)}: {Shift}");
                    RD = BitHelpers.ExtractBits(Value, 5, 11);
                    s.Log($"{nameof(RD)}: {RD}");
                    RT = BitHelpers.ExtractBits(Value, 5, 16);
                    s.Log($"{nameof(RT)}: {RT}");
                    RS = BitHelpers.ExtractBits(Value, 5, 21);
                    s.Log($"{nameof(RS)}: {RS}");
                    break;

                // I Instructions
                case InstructionMnemonic.lui:
                case InstructionMnemonic.lw:
                    IMM = BitHelpers.ExtractBits(Value, 16, 0);
                    s.Log($"{nameof(IMM)}: {IMM}");
                    RT = BitHelpers.ExtractBits(Value, 5, 16);
                    s.Log($"{nameof(RT)}: {RT}");
                    RS = BitHelpers.ExtractBits(Value, 5, 21);
                    s.Log($"{nameof(RS)}: {RS}");
                    break;
            }
        }

        public enum InstructionMnemonic
        {
            Unknown,

            nop,

            sll, // Logical Shift Left
            addu, // Add Unsigned
            jr, // Jump to Address in Register

            lui, // Load Upper Immediate
            lw, // Load Word
        }
    }
}