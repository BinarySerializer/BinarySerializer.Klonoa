using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinarySerializer.Klonoa.KH
{
    /// <summary>
    /// Encoder for compressed data in Moonlight Museum and Heroes
    /// </summary>
    public class BytePairEncoder : IStreamEncoder
    {
        public string Name => "BytePair";

        /// <summary>
        /// The block size to use when encoding
        /// </summary>
        public int BlockSize { get; set; } = 1024;

        public const int MaxRecursionCount = 16;

        // Decompressed at 0x0804E428 in the ROM
        public void DecodeStream(Stream input, Stream output)
        {
            var initialCompressedPosition = input.Position;

            // Create a reader for the input
            using var reader = new Reader(input, isLittleEndian: true, leaveOpen: true);

            var writer = new Writer(output, isLittleEndian: true, leaveOpen: true);

            var initialOutputPos = output.Position;

            // Read the header
            var compressedSize = reader.ReadUInt32();
            var decompressedSize = reader.ReadUInt32();

            // Any data after the header is always big endian
            reader.IsLittleEndian = false;
            
            // Don't count the header size
            compressedSize -= 8;

            if (compressedSize <= 0)
                throw new Exception($"Invalid compressed data");

            // Decompress until all the data has been parsed
            while (input.Position - initialCompressedPosition < compressedSize)
            {
                // Create a translation table. Specific bytes will be represented by two other bytes.
                var translationTable = new Dictionary<byte, byte[]>();

                uint translationByteKey = 0;

                // We start by creating the translation table, setting it for each possible byte
                while (translationByteKey < 0x100)
                {
                    // First read the count
                    byte count = reader.ReadByte();

                    // If the count is more than 0x7F (128) then we skip instead
                    if (0x7f < count)
                    {
                        translationByteKey = translationByteKey + count - 0x7f;

                        if (translationByteKey > 0x100)
                            throw new Exception($"Failed to decompress data! Translation byte key is more than 0x100.");

                        // Check if we've reached the end
                        if (translationByteKey == 0x100)
                            break;

                        count = 0;
                    }

                    count++;

                    for (int i = 0; i < count; i++)
                    {
                        // Read the value
                        byte value = reader.ReadByte();

                        if (translationByteKey >= 0x100)
                            throw new Exception($"Failed to decompress data! Translation index is out of bounds.");

                        // If the index doesn't match the byte value then it will be translated
                        if (translationByteKey != value)
                        {
                            translationTable[(byte)translationByteKey] = new byte[]
                            {
                                reader.ReadByte(),
                                value,
                            };
                        }

                        translationByteKey++;
                    }
                }

                if (translationByteKey > 0x100)
                    throw new Exception($"Failed to decompress data! Translation byte key is more than 0x100.");

                int writeCount = reader.ReadUInt16();

                // Create a temporary buffer. Translated values might need to be translated multiple times!
                using var tempBuffer = new MemoryStream();

                for (int i = 0; i < writeCount; i++)
                {
                    var writeTranslationIndex = reader.ReadByte();

                    while (true)
                    {
                        // If the translation table doesn't have the byte it should be used as is
                        if (!translationTable.ContainsKey(writeTranslationIndex))
                        {
                            writer.Write(writeTranslationIndex);
                        }
                        // If the byte should be translated we write the two bytes it represents into a buffer (those values might get translated too!)
                        else
                        {
                            tempBuffer.Write(translationTable[writeTranslationIndex], 0, 2);
                        }

                        // Once we've translated all the values in the temporary buffer this cycle is complete
                        if (tempBuffer.Position == 0)
                            break;

                        // Sanity check
                        if (tempBuffer.Length > 0xFFFF)
                            throw new Exception($"Failed to decompile data");

                        // Read the next value from the buffer (we read backwards)
                        tempBuffer.Position--;
                        writeTranslationIndex = (byte)tempBuffer.ReadByte();
                        tempBuffer.Position--;
                    }
                }
            }

            // Verify the size
            if (decompressedSize != output.Length - initialOutputPos)
                throw new Exception($"Data was not decompressed correctly!");
        }

        public void EncodeStream(Stream input, Stream output)
        {
            long decompressedSize = input.Length - input.Position;

            var writer = new Writer(output, isLittleEndian: false, leaveOpen: true);

            var initialOutputPos = output.Position;

            // Skip the header for now (we write that last)
            output.Position += 8;

            while (input.Position < input.Length)
            {
                // Read a block
                byte[] originalBlock = new byte[BlockSize];
                var blockSize = input.Read(originalBlock, 0, originalBlock.Length);
                List<byte> block = originalBlock.Take(blockSize).ToList();

                // Keep track of used bytes
                bool[] usedBytes = new bool[256];

                var translationTable = new Dictionary<byte, byte[]>();

                // Recursively loop and replace bytes. Each loop we find the most common byte pair and an unused byte to use as the key.
                for (int r = 0; r < MaxRecursionCount; r++)
                {
                    var bytePairCounts = new Dictionary<ushort, int>();

                    // Enumerate every byte
                    for (int i = 0; i < block.Count; i++)
                    {
                        // Flag that the byte is used
                        usedBytes[block[i]] = true;

                        // If we're not at the last byte we check the byte pair
                        if (i + 1 < block.Count)
                        {
                            ushort v = (ushort)(block[i] | block[i + 1] << 8);
                            bytePairCounts[v] = bytePairCounts.TryGetValue(v, out int prevCount) ? prevCount + 1 : 1;
                        }
                    }

                    // If no more byte pairs occur more than 4 times we break
                    if (!bytePairCounts.Values.Any(x => x > 4))
                        break;

                    // Get the most common byte pair
                    var mostCommonPair = bytePairCounts.OrderBy(x => x.Value).Last().Key;

                    // Get an unused byte to use
                    byte? unusedByte = null;

                    for (int i = 0; i < usedBytes.Length; i++)
                    {
                        if (!usedBytes[i])
                        {
                            unusedByte = (byte)i;
                            break;
                        }
                    }

                    // Break if there is no unused byte to use
                    if (unusedByte == null)
                        break;

                    var b1 = (byte)BitHelpers.ExtractBits(mostCommonPair, 8, 0);
                    var b2 = (byte)BitHelpers.ExtractBits(mostCommonPair, 8, 8);

                    // Add to the translation table
                    translationTable.Add(unusedByte.Value, new byte[] { b1, b2 });

                    // Update the block
                    for (int i = 0; i < block.Count; i++)
                    {
                        if (i + 1 < block.Count && block[i] == b1 && block[i + 1] == b2)
                        {
                            block[i] = unusedByte.Value;
                            block.RemoveAt(i + 1);
                        }
                    }
                }

                var translationByteKey = 0;
                var sortedTranslationTable = translationTable.OrderBy(x => x.Key).ToArray();

                for (var i = 0; i < sortedTranslationTable.Length; i++)
                {
                    var translationTableItem = sortedTranslationTable[i];
                    int count;

                    // If not equal to the current key we need to skip forward
                    if (translationTableItem.Key != translationByteKey)
                    {
                        var difference = translationTableItem.Key - translationByteKey;

                        if (difference < 0)
                            throw new Exception("Negative difference!");

                        if (difference <= 128)
                        {
                            writer.Write((byte)(0x7F + difference)); // Skip difference
                            translationByteKey += difference;
                        }
                        else
                        {
                            writer.Write((byte)(0x7F + 127)); // Skip 127
                            translationByteKey += 127;
                            writer.Write((byte)translationByteKey); // Write same value as index to ignore
                            translationByteKey++;
                            writer.Write((byte)(0x7F + (difference - 127 - 1))); // Skip remaining difference
                            translationByteKey += difference - 127 - 1;
                        }

                        count = 1;
                    }
                    else
                    {
                        // TODO: Optimize this by setting a higher count if the values are in sequence
                        count = 1;
                        writer.Write((byte)(count - 1));
                    }

                    for (int j = 0; j < count; j++)
                    {
                        writer.Write(translationTableItem.Value[0]);
                        writer.Write(translationTableItem.Value[1]);

                        translationByteKey++;
                    }
                }

                var diff = 0x100 - translationByteKey;

                if (diff != 0)
                {
                    if (diff < 0)
                        throw new Exception("Negative difference!");

                    if (diff <= 128)
                    {
                        writer.Write((byte)(0x7F + diff)); // Skip difference
                    }
                    else
                    {
                        writer.Write((byte)(0x7F + 127)); // Skip 127
                        translationByteKey += 127;
                        writer.Write((byte)translationByteKey); // Write same value as index to ignore
                        translationByteKey++;
                        writer.Write((byte)(0x7F + (diff - 127 - 1))); // Skip remaining difference
                    }
                }

                // Write the block size
                writer.Write((ushort)block.Count);

                // Write the block
                writer.Write(block.ToArray());
            }

            var outputEndPosition = output.Position;

            // Write the header
            output.Position = initialOutputPos;
            writer.IsLittleEndian = true;
            writer.Write((uint)(outputEndPosition - initialOutputPos)); // Compressed size
            writer.Write((uint)decompressedSize); // Decompressed size

            output.Position = outputEndPosition;
        }
    }
}