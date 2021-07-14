﻿namespace BinarySerializer.KlonoaDTP
{
    public class MovementPathBlock : BinarySerializable
    {
        // TODO: Parse
        public byte[] Data { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Data = s.SerializeArray<byte>(Data, 28, name: nameof(Data));
        }
    }
}