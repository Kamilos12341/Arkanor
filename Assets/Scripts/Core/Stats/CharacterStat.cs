using System;
using System.Collections.Generic;

namespace Arkanor.Characters
{  public class CharacterStat
    {
        private float baseValue;

        private readonly List<StatModifier> modifiers = new();

        private bool isDirty = true;
        private float currentValue;

        public CharacterStat(float baseValue)
        {
            this.baseValue = baseValue;
        }

        public float Value
        {
            get
            {
                if (isDirty)
                {
                    Recalculate();
                }

                return currentValue;
            }
        }

        public float BaseValue
        {
            get => baseValue;

            set
            {
                baseValue = value;
                isDirty = true;
            }
        }

        public void AddModifier(StatModifier modifier)
        {
            modifiers.Add(modifier);
            isDirty = true;
        }

        public void RemoveModifier(StatModifier modifier)
        {
            modifiers.Remove(modifier);
            isDirty = true;
        }

        public void RemoveAllFromSource(object source)
        {
            modifiers.RemoveAll(x => x.Source == source);
            isDirty = true;
        }

        private void Recalculate()
        {
            currentValue = baseValue;

            foreach (var modifier in modifiers)
            {
                currentValue += modifier.Value;
            }

            isDirty = false;
        }
    }
}