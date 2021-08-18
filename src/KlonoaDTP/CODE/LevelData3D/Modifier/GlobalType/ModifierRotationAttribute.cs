using System;

namespace BinarySerializer.Klonoa.DTP
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ModifierRotationAttribute : Attribute
    {
        public ModifierRotationAttribute(RotAxis axis, int rotation)
        {
            Axis = axis;
            Rotation = rotation;
        }

        public RotAxis Axis { get; }
        public int Rotation { get; }

        public enum RotAxis
        {
            X,
            Y,
            Z
        }
    }
}