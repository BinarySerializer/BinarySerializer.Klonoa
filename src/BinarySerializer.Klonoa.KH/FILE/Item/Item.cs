using System;

namespace BinarySerializer.Klonoa.KH
{
    public class Item : BinarySerializable
    {
        public string Type { get; set; }
        public Item_Character ItemCharacter { get; set; }
        public Item_Category ItemCategory { get; set; }
        public byte Byte_02 { get; set; }
        public byte Byte_03 { get; set; }
        public byte Byte_04 { get; set; }
        public ushort Ushort_06 { get; set; }

        public TextCommands Name { get; set; }
        public TextCommands Description1 { get; set; }
        public TextCommands Description2 { get; set; }

        public uint Price { get; set; }

        public ushort W_AT { get; set; }
        public ushort W_SP { get; set; }

        public ushort D_DF { get; set; }
        public ushort D_AG { get; set; }

        public ushort A_EffectType { get; set; }
        public short A_AT { get; set; }
        public short A_SP { get; set; }
        public short A_DF { get; set; }
        public short A_AG { get; set; }
        public short A_HP { get; set; }
        public ushort A_Unknown { get; set; }
        public ushort A_EffectParam { get; set; }

        public ushort I_IconIndex { get; set; }
        public ushort I_Unknown { get; set; }

        public ushort E_IconIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Type = s.SerializeString(Type, 2, name: nameof(Type));
            ItemCharacter = (Item_Character)Enum.Parse(typeof(Item_Character), Type[0].ToString());
            ItemCategory = (Item_Category)Enum.Parse(typeof(Item_Category), Type[1].ToString());
            Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
            Byte_03 = s.Serialize<byte>(Byte_03, name: nameof(Byte_03));
            Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
            s.SerializePadding(1, logIfNotNull: true);
            Ushort_06 = s.Serialize<ushort>(Ushort_06, name: nameof(Ushort_06));

            switch (ItemCategory)
            {
                case Item_Category.W:
                    Name = s.SerializeObject<TextCommands>(Name, x => x.Pre_MaxLength = 9, name: nameof(Name));
                    W_AT = s.Serialize<ushort>(W_AT, name: nameof(W_AT));
                    W_SP = s.Serialize<ushort>(W_SP, name: nameof(W_SP));
                    Description1 = s.SerializeObject<TextCommands>(Description1, x => x.Pre_MaxLength = 17, name: nameof(Description1));
                    Price = s.Serialize<uint>(Price, name: nameof(Price));
                    Description2 = s.SerializeObject<TextCommands>(Description2, x => x.Pre_MaxLength = 26, name: nameof(Description2));
                    break;

                case Item_Category.D:
                    Name = s.SerializeObject<TextCommands>(Name, x => x.Pre_MaxLength = 9, name: nameof(Name));
                    D_DF = s.Serialize<ushort>(D_DF, name: nameof(D_DF));
                    D_AG = s.Serialize<ushort>(D_AG, name: nameof(D_AG));
                    Description1 = s.SerializeObject<TextCommands>(Description1, x => x.Pre_MaxLength = 17, name: nameof(Description1));
                    Price = s.Serialize<uint>(Price, name: nameof(Price));
                    Description2 = s.SerializeObject<TextCommands>(Description2, x => x.Pre_MaxLength = 26, name: nameof(Description2));
                    break;

                case Item_Category.A:
                    Name = s.SerializeObject<TextCommands>(Name, x => x.Pre_MaxLength = 9, name: nameof(Name));
                    A_EffectType = s.Serialize<ushort>(A_EffectType, name: nameof(A_EffectType));
                    A_AT = s.Serialize<short>(A_AT, name: nameof(A_AT));
                    A_SP = s.Serialize<short>(A_SP, name: nameof(A_SP));
                    A_DF = s.Serialize<short>(A_DF, name: nameof(A_DF));
                    A_AG = s.Serialize<short>(A_AG, name: nameof(A_AG));
                    A_HP = s.Serialize<short>(A_HP, name: nameof(A_HP));
                    A_Unknown = s.Serialize<ushort>(A_Unknown, name: nameof(A_Unknown));
                    Price = s.Serialize<uint>(Price, name: nameof(Price));
                    Description1 = s.SerializeObject<TextCommands>(Description1, x => x.Pre_MaxLength = 17, name: nameof(Description1));
                    A_EffectParam = s.Serialize<ushort>(A_EffectParam, name: nameof(A_EffectParam));
                    Description2 = s.SerializeObject<TextCommands>(Description2, x => x.Pre_MaxLength = 24, name: nameof(Description2));
                    break;

                case Item_Category.I:
                    Name = s.SerializeObject<TextCommands>(Name, x => x.Pre_MaxLength = 9, name: nameof(Name));
                    I_IconIndex = s.Serialize<ushort>(I_IconIndex, name: nameof(I_IconIndex));
                    I_Unknown = s.Serialize<ushort>(I_Unknown, name: nameof(I_Unknown));
                    Description1 = s.SerializeObject<TextCommands>(Description1, x => x.Pre_MaxLength = 17, name: nameof(Description1));
                    Price = s.Serialize<uint>(Price, name: nameof(Price));
                    Description2 = s.SerializeObject<TextCommands>(Description2, x => x.Pre_MaxLength = 24, name: nameof(Description2));
                    break;

                case Item_Category.E:
                    Name = s.SerializeObject<TextCommands>(Name, x => x.Pre_MaxLength = 9, name: nameof(Name));
                    E_IconIndex = s.Serialize<ushort>(E_IconIndex, name: nameof(E_IconIndex));
                    Description1 = s.SerializeObject<TextCommands>(Description1, x => x.Pre_MaxLength = 20, name: nameof(Description1));
                    Description2 = s.SerializeObject<TextCommands>(Description2, x => x.Pre_MaxLength = 24, name: nameof(Description2));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}