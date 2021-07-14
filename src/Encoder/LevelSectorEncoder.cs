using System;
using System.IO;

namespace BinarySerializer.KlonoaDTP
{
    // I have no clue what compression type this actually is. With this ugly code, copied from Ghidra, it at least works...
    public class LevelSectorEncoder : IStreamEncoder
    {
        public string Name => $"LevelSectorEncoder";

        // Re-implemented from 0x8007841c
        public Stream DecodeStream(Stream s)
        {
            // Create a reader for the input
            var reader = new Reader(s, isLittleEndian: true);

            // Create a stream to store the decompressed data
            var decompressedStream = new MemoryStream();

            var writer = new Writer(decompressedStream, isLittleEndian: true, leaveOpen: true);

            int cycles = reader.ReadByte();
            var buffer0 = new byte[0x14]; // 800E4158
            var buffer1 = new byte[0x212 - 0x14]; // 800E416C
            var buffer2 = new ushort[(0x412 - 0x212) / 2]; // 800E436A
            var buffer3 = new ushort[(0x2412 - 0x412) / 2];
            var buffer4 = new ushort[(0x2c08 - 0x2412) / 2];
            //var buffer5 = new ushort[(0xFC60 - 0x2c08) / 2]; // Might be too big
            var buffer5 = new ushort[buffer4.Length]; // Not sure what the length is since it's the last buffer. Hopefully same as previous one.

            bool hasFinishedDecompressing = false;
            ushort USHORT_1f80001c = 0;
            ushort USHORT_1f800018 = 0;
            ushort USHORT_1f80001a = 0;
            FUN_80078cdc(0x10);
            ushort USHORT_1f80001e = 0;

            void FUN_80078cdc(int param_1)
            {
                uint uVar3 = USHORT_1f80001c;
                USHORT_1f80001a = (ushort)(USHORT_1f80001a << (int)((short)param_1 & 0x1fU));
                if (USHORT_1f80001c < (short)param_1)
                {
                    uint uVar2;

                    do
                    {
                        param_1 = (int)(param_1 - uVar3);
                        uVar2 = (uint)(param_1 * 0x10000 >> 0x10);
                        USHORT_1f80001a = (ushort)(USHORT_1f80001a | USHORT_1f800018 << (int)(uVar2 & 0x1f));
                        USHORT_1f80001c = 8;
                        USHORT_1f800018 = reader.ReadByte();
                        uVar3 = 8;
                    } while (8 < (int)uVar2);
                }
                USHORT_1f80001a = (ushort)(USHORT_1f80001a | (ushort)(USHORT_1f800018 >> (int)((uint)USHORT_1f80001c - param_1 & 0x1f)));
                USHORT_1f80001c = (ushort)((uint)USHORT_1f80001c - param_1);
            }

            uint UnknownDecomp2_GetNextValue()
            {
                if (USHORT_1f80001e == 0)
                {
                    cycles--;

                    if (cycles < 0)
                    {
                        hasFinishedDecompressing = true;
                        return 0;
                    }

                    USHORT_1f80001e = (ushort)FUN_80078c98(0x10);
                    FUN_800787b0(0x13, 5, 3);
                    FUN_80078a24();
                    FUN_800787b0(0x10, 4, -1);

                    if (hasFinishedDecompressing)
                        return 0;
                }

                uint uVar2;

                USHORT_1f80001e--;
                var uVar1 = buffer3[USHORT_1f80001a >> 4];
                uint uVar4 = 8;
                while (0x1fd < (uVar2 = uVar1)) 
                {
                    var puVar3 = buffer4;
                    
                    if ((USHORT_1f80001a & uVar4) != 0)
                        puVar3 = buffer5;
                    
                    uVar1 = puVar3[uVar2];
                    uVar4 = uVar4 >> 1;
                }
                FUN_80078cdc(buffer1[uVar2]);
                return uVar2;
            }

            void FUN_80078a24()
            {
                if (hasFinishedDecompressing)
                    return;

                int iVar6;
                int iVar9;

                var sVar3 = (short)FUN_80078c98(9);
                if (sVar3 == 0)
                {
                    var uVar4 = (ushort)FUN_80078c98(9);
                    iVar9 = 0;
                    do
                    {
                        sVar3 = (short)iVar9;
                        iVar9++;
                        buffer1[sVar3] = 0;
                    } while (iVar9 * 0x10000 >> 0x10 < 0x1fe);
                    iVar9 = 0;
                    do
                    {
                        iVar6 = iVar9 << 0x10;
                        iVar9 ++;
                        buffer3[(iVar6 >> 0xf) / 2] = uVar4;
                    } while (iVar9 * 0x10000 >> 0x10 < 0x1000);
                }
                else
                {
                    iVar9 = 0;
                    if (0 < sVar3)
                    {
                        do
                        {
                            var uVar1 = buffer2[USHORT_1f80001a >> 8];
                            uint uVar8 = 0x80;

                            uint uVar2;
                            while (0x12 < (uVar2 = uVar1))
                            {
                                var puVar7 = buffer4;
                                
                                if ((USHORT_1f80001a & uVar8) != 0)
                                    puVar7 = buffer5;
                                
                                uVar1 = puVar7[uVar2];
                                uVar8 = uVar8 >> 1;
                            }

                            FUN_80078cdc(buffer0[uVar2]);
                            short sVar5 = (short)iVar9;
                            if (uVar2 < 3)
                            {
                                if (uVar2 == 0)
                                {
                                    iVar6 = 1;
                                }
                                else
                                {
                                    if (uVar2 == 1)
                                    {
                                        uVar8 = FUN_80078c98(4);
                                        iVar6 = (int)((uVar8 & 0xffff) + 3);
                                    }
                                    else
                                    {
                                        uVar8 = FUN_80078c98(9);
                                        iVar6 = (int)((uVar8 & 0xffff) + 0x14);
                                    }
                                }
                                while (-1 < (iVar6 += -1)) 
                                {
                                    sVar5 = (short)iVar9;
                                    iVar9++;
                                    buffer1[sVar5] = 0;
                                }
                            }
                            else 
                            {
                                iVar9++;
                                buffer1[sVar5] = (byte)((byte)uVar1 - 2);
                            }
                        } while ((short)iVar9 < sVar3);
                    }
                    if ((short)iVar9 < 0x1fe)
                    {
                        do
                        {
                            sVar3 = (short)iVar9;
                            iVar9++;
                            buffer1[sVar3] = 0;
                        } while (iVar9 * 0x10000 >> 0x10 < 0x1fe);
                    }
                    FUN_80078dc0(0x1fe, buffer1, 0xc, buffer3);
                }
            }

            uint FUN_80078c98(short param_1)
            {
                int uVar1 = USHORT_1f80001a;
                FUN_80078cdc(param_1);
                return (uint)(uVar1 >> (int)(0x10U - param_1 & 0x1f) & 0xffff);
            }

            void FUN_800787b0(short param_1, short param_2, short param_3)
            {
                if (hasFinishedDecompressing) 
                    return;
                
                short sVar2 = (short)FUN_80078c98(param_2);
                if (sVar2 == 0)
                {
                    ushort uVar3 = (ushort)FUN_80078c98(param_2);

                    int iVar9 = 0;

                    if (0 < param_1)
                    {
                        do
                        {
                            sVar2 = (short)iVar9;
                            iVar9++;
                            buffer0[sVar2] = 0;
                        } while (iVar9 * 0x10000 >> 0x10 < param_1);
                    }

                    iVar9 = 0;

                    do
                    {
                        int iVar4 = iVar9 << 0x10;
                        iVar9++;
                        buffer2[iVar4 >> 0xf] = uVar3;
                    } while (iVar9 * 0x10000 >> 0x10 < 0x100);
                }
                else
                {
                    int iVar9 = 0;
                    int iVar4;

                    if (0 < sVar2)
                    {
                        short sVar5;

                        do
                        {
                            uint uVar8 = (uint)(USHORT_1f80001a >> 0xd);
                            bool bVar1 = uVar8 < 7;
                            if (uVar8 == 7)
                            {
                                uint uVar6 = 0x1000;
                                if ((USHORT_1f80001a & 0x1000) != 0)
                                {
                                    do
                                    {
                                        uVar6 = uVar6 >> 1;
                                        uVar8++;
                                    } while ((uVar6 & USHORT_1f80001a) != 0);
                                }
                                bVar1 = (int)uVar8 < 7;
                            }

                            iVar4 = 3;
                            
                            if (!bVar1)
                                iVar4 = (int)((uVar8 - 3) * 0x10000) >> 0x10;

                            FUN_80078cdc(iVar4);

                            short sVar7 = (short)iVar9;

                            iVar9++;

                            sVar5 = (short)iVar9;

                            buffer0[sVar7] = (byte)uVar8;
                            
                            if (iVar9 * 0x10000 >> 0x10 == param_3)
                            {
                                uVar8 = FUN_80078c98(2);
                                iVar4 = (int)((uVar8 & 0xffff) - 1);

                                if (-1 < iVar4)
                                {
                                    do
                                    {
                                        iVar4--;
                                        sVar5 = (short)iVar9;
                                        iVar9++;
                                        buffer0[sVar5] = 0;
                                    } while (-1 < iVar4);

                                    sVar5 = (short)iVar9;
                                }
                            }
                            else
                            {
                                sVar5 = (short)iVar9;
                            }
                        } while (sVar5 < sVar2);
                    }

                    iVar4 = (short)iVar9;
                    
                    while (iVar4 < param_1)
                    {
                        sVar2 = (short)iVar9;
                        iVar9++;
                        iVar4 = iVar9 * 0x10000 >> 0x10;
                        buffer0[sVar2] = 0;
                    }
                    
                    FUN_80078dc0((ushort)param_1, buffer0, 8, buffer2);
                }
            }

            void FUN_80078dc0(ushort param_1, byte[] paramBuffer0, uint param_3, ushort[] paramBuffer1)
            {
                uint uVar2;
                var asStack120 = new short[20];
                var auStack80 = new ushort[20];
                var auStack40 = new ushort[18];

                ushort uVar8 = 1;
                do
                {
                    asStack120[uVar8] = 0;
                    uVar8++;
                } while (uVar8 < 0x11);

                uVar8 = 0;

                if (param_1 != 0)
                {
                    uVar2 = 0;
                    do
                    {
                        uVar8++;
                        asStack120[paramBuffer0[uVar2]] = (short)(asStack120[paramBuffer0[uVar2]] + 1);
                        uVar2 = uVar8;
                    } while (uVar8 < param_1);
                }
                auStack40[1] = 0;
                uVar8 = 1;
                do
                {
                    uVar2 = uVar8;
                    uVar8++;
                    auStack40[uVar2 + 1] = (ushort)(auStack40[uVar2] + (asStack120[uVar2] << (int)(0x10 - uVar2 & 0x1f)));
                } while (uVar8 < 0x11);

                if (auStack40[17] == 0)
                {
                    var uVar11 = 0x10 - param_3;
                    var uVar6 = param_3 & 0xffff;
                    uVar2 = 1;
                    uint uVar3;
                    if (uVar6 != 0)
                    {
                        do
                        {
                            uVar3 = uVar2 & 0xffff;
                            uVar2++;
                            auStack80[uVar3] = (ushort)(1 << (int)(uVar6 - uVar3 & 0x1f));
                            auStack40[uVar3] = (ushort)(auStack40[uVar3] >> (int)(uVar11 & 0x1f));
                        } while ((uVar2 & 0xffff) <= uVar6);
                    }
                    while ((uVar6 = uVar2 & 0xffff) < 0x11) 
                    {
                        auStack80[uVar6] = (ushort)(1 << (int)(0x10 - uVar6 & 0x1f));
                        uVar2++;
                    }
                    uVar2 = (uint)(auStack40[(param_3 & 0xffff) + 1] >> (int)(uVar11 & 0x1f));

                    if (((uVar2 & 0xffff) != 0))
                    {
                        uVar6 = (uint)(1 << (int)(param_3 & 0x1f));
                        if ((uVar2 & 0xffff) != uVar6)
                        {
                            uVar3 = uVar2 & 0xffff;
                            do
                            {
                                paramBuffer1[uVar3] = 0;
                                uVar2++;
                                uVar3 = uVar2 & 0xffff;
                            } while (uVar3 != uVar6);
                        }
                    }

                    uVar8 = 0;

                    if (param_1 == 0) 
                        return;

                    uVar2 = 0;
                    var uVar9 = param_1;
                    do
                    {
                        uVar2 = paramBuffer0[uVar2];
                        if (uVar2 != 0)
                        {
                            uVar6 = auStack40[uVar2];
                            var uVar10 = uVar6 + auStack80[uVar2];
                            uVar3 = uVar6;
                            if ((param_3 & 0xffff) < uVar2)
                            {
                                var bufferOffset = (auStack40[uVar2] >> (int)(uVar11 & 0x1f));
                                var buffer = paramBuffer1;

                                uVar3 = uVar2 - param_3;
                                while ((uVar3 & 0xffff) != 0)
                                {
                                    if (buffer[bufferOffset] == 0)
                                    {
                                        buffer5[uVar9] = 0;
                                        buffer4[uVar9] = 0;
                                        buffer[bufferOffset] = uVar9;
                                        uVar9++;
                                    }

                                    ushort uVar1;
                                    if ((uVar6 & 1 << (int)(0xf - (param_3 & 0xffff) & 0x1f) & 0xffffU) == 0)
                                    {
                                        uVar1 = buffer[bufferOffset];
                                        buffer = buffer4;
                                        bufferOffset = 0;
                                    }
                                    else
                                    {
                                        uVar1 = buffer[bufferOffset];
                                        buffer = buffer5;
                                        bufferOffset = 0;
                                    }
                                    bufferOffset += uVar1;
                                    uVar3--;
                                    uVar6 = uVar6 << 1;
                                }
                                buffer[bufferOffset] = uVar8;
                            }
                            else
                            {
                                while (uVar3 < uVar10)
                                {
                                    paramBuffer1[uVar3] = uVar8;
                                    uVar6++;
                                    uVar3 = uVar6 & 0xffff;
                                }
                            }
                            auStack40[uVar2] = (ushort)uVar10;
                        }
                        uVar8++;
                        uVar2 = uVar8;
                    } while (uVar8 < param_1);
                }
                else
                {
                    throw new Exception();
                }
            }

            uint FUN_800786d0()
            {
                uint uVar5;

                var uVar1 = buffer2[USHORT_1f80001a >> 8];
                uint uVar4 = 0x80;
                while (0xf < (uVar5 = uVar1)) 
                {
                    var puVar3 = buffer4;
                    if ((USHORT_1f80001a & uVar4) != 0)
                    {
                        puVar3 = buffer5;
                    }
                    uVar1 = puVar3[uVar5];
                    uVar4 = uVar4 >> 1;
                }
                FUN_80078cdc(buffer0[uVar5]);
                if (uVar5 != 0)
                {
                    var iVar2 = (int)FUN_80078c98((short)((int)((uVar5 - 1) * 0x10000) >> 0x10));
                    uVar5 = (uint)(iVar2 + (1 << (int)(uVar5 - 1 & 0x1f)));
                }
                return uVar5 & 0xffff;
            }

            uint value = UnknownDecomp2_GetNextValue();

            while (hasFinishedDecompressing == false) 
            {
                uint count = value & 0xffff;

                if (count < 0x100)
                {
                    writer.Write((byte)value);
                }
                else
                {
                    value = FUN_800786d0();

                    var outPos = writer.BaseStream.Position;
                    var outBackPos = outPos + -((value & 0xffff) + 1);

                    int index = 0xfd;

                    if (0xfd < count)
                    {
                        do
                        {
                            writer.BaseStream.Position = outBackPos;
                            byte uVar1 = (byte)writer.BaseStream.ReadByte();
                            outBackPos++;

                            index++;

                            writer.BaseStream.Position = outPos;
                            writer.Write(uVar1);
                            outPos++;
                        } while (index < (int)count);
                    }
                }

                value = UnknownDecomp2_GetNextValue();
            }

            // Set position back to 0
            decompressedStream.Position = 0;

            // Return the compressed data stream
            return decompressedStream;
        }

        public Stream EncodeStream(Stream s)
        {
            throw new NotImplementedException();
        }
    }
}