using System;
using System.Collections.Generic;
using System.Linq;

namespace Vgui
{
    public class VguiNameFlagKey
    {
        public string Name { get; }
        public string[] Flags { get; }
        public bool[] FlagValues { get; }

        public VguiNameFlagKey(string name, Dictionary<string, bool> flags)
        {
            Name = name;
            Flags = new string[flags.Count];
            FlagValues = new bool[flags.Count];
            var i = 0;
            foreach (var flag in flags)
            {
                Flags[i] = flag.Key;
                FlagValues[i] = flag.Value;
                i++;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash *= 23 + Name.GetHashCode();
                for (var i = 0; i < Flags.Length; i++)
                {
                    hash *= 23 + Flags[i].GetHashCode();
                    hash *= 23 + FlagValues[i].GetHashCode();
                }
                return hash;
            }
        }

        public override bool Equals(object obj) =>
            obj is VguiNameFlagKey key && Name == key.Name && Flags.SequenceEqual(key.Flags) &&
            FlagValues.SequenceEqual(key.FlagValues);
    }
}