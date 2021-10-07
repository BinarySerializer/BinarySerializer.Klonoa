using System;
using System.Collections.Generic;
using System.IO;

namespace BinarySerializer.Klonoa.KH
{
    /// <summary>
    /// Encoder for compressed data in Moonlight Museum and Heroes
    /// </summary>
    public class MM_KH_Encoder : IStreamEncoder
    {
        public string Name => nameof(MM_KH_Encoder);

        // Decompressed at 0x0804E428 in the ROM
        public Stream DecodeStream(Stream s)
        {
            var initialCompressedPosition = s.Position;

            // Create a reader for the input
            var reader = new Reader(s, isLittleEndian: true);

            // Create a stream to store the decompressed data
            var decompressedStream = new MemoryStream();

            var writer = new Writer(decompressedStream, isLittleEndian: true, leaveOpen: true);

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
            while (s.Position - initialCompressedPosition < compressedSize)
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

                        // Check if we've reached the end
                        if (translationByteKey >= 0x100)
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
            if (decompressedSize != decompressedStream.Length)
                throw new Exception($"Data was not decompressed correctly!");

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