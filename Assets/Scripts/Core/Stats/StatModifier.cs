using System;

namespace Arkanor.Characters
{
    public class StatModifier
    {
        public float Value { get; }

        public object Source { get; }

        public StatModifier(float value, object source)
        {
            Value = value;
            Source = source;
        }
    }
}