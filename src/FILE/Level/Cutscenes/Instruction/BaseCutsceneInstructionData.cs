using System;

namespace BinarySerializer.KlonoaDTP
{
    public abstract class BaseCutsceneInstructionData : BinarySerializable
    {
        public Pointer Pre_ParamsBufferBaseOffset { get; set; }

        public uint ParamsBufferOffset { get; set; }

        public void DoAtParams(SerializerObject s, Action action)
        {
            s.DoAt(Pre_ParamsBufferBaseOffset + ParamsBufferOffset, action);
        }
    }
}