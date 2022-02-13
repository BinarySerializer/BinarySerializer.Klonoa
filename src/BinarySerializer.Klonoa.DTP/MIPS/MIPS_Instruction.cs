namespace BinarySerializer.Klonoa.DTP
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
            s.Log("{0}: {1}", nameof(Opcode), Opcode);

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
                    s.LogWarning("Unknown MIPS instruction with opcode {0} and funt {1}", Opcode, Funct);
            }
            else if (Opcode == 0x0F)
                Mnemonic = InstructionMnemonic.lui;
            else if (Opcode == 0x23)
                Mnemonic = InstructionMnemonic.lw;
            else
                s.LogWarning("Unknown MIPS instruction with opcode {0}", Opcode);

            s.Log("{0}: {1}", nameof(Mnemonic), Mnemonic);

            switch (Mnemonic)
            {
                // R Instructions
                case InstructionMnemonic.sll:
                case InstructionMnemonic.addu:
                case InstructionMnemonic.jr:
                    Funct = BitHelpers.ExtractBits(Value, 6, 0);
                    s.Log("{0}: {1}", nameof(Funct), Funct);
                    Shift = BitHelpers.ExtractBits(Value, 5, 6);
                    s.Log("{0}: {1}", nameof(Shift), Shift);
                    RD = BitHelpers.ExtractBits(Value, 5, 11);
                    s.Log("{0}: {1}", nameof(RD), RD);
                    RT = BitHelpers.ExtractBits(Value, 5, 16);
                    s.Log("{0}: {1}", nameof(RT), RT);
                    RS = BitHelpers.ExtractBits(Value, 5, 21);
                    s.Log("{0}: {1}", nameof(RS), RS);
                    break;

                // I Instructions
                case InstructionMnemonic.lui:
                case InstructionMnemonic.lw:
                    IMM = BitHelpers.ExtractBits(Value, 16, 0);
                    s.Log("{0}: {1}", nameof(IMM), IMM);
                    RT = BitHelpers.ExtractBits(Value, 5, 16);
                    s.Log("{0}: {1}", nameof(RT), RT);
                    RS = BitHelpers.ExtractBits(Value, 5, 21);
                    s.Log("{0}: {1}", nameof(RS), RS);
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