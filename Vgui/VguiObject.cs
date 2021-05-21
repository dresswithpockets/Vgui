using System.Collections.Generic;

namespace Vgui
{
    public class VguiObject
    {
        public string Name { get; }
        public string? Value { get; }
        public bool IsValue => Value != null;
        public Dictionary<string, VguiObject> Properties { get; } = new();
        private Dictionary<VguiNameFlagKey, VguiObject> _properties = new();
        public Dictionary<string, bool> Flags { get; } = new();

        public VguiObject(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public VguiObject(string name)
        {
            Name = name;
            Value = null;
        }

        public VguiObject? Get(string name) => Properties.TryGetValue(name, out var val) ? val : null;

        internal void MergeOrAddProperty(VguiObject other)
        {
            // if _properties contains a matching NameFlagKey based on the iterated prop,
            // merge. Otherwise, add to both Properties and _properties.
            var key = other.GetNameFlagKey();
            if (_properties.TryGetValue(key, out var prop) && !prop.IsValue && !other.IsValue)
            {
                // merge
                prop.TryMerge(other);
            }
            else
            {
                // add to both Properties and _properties
                _properties.Add(key, other);
                Properties.Add(other.Name, other);
            }
        }
        
        internal bool TryMerge(VguiObject other)
        {
            if (other.Name != Name || !CompareFlags(other) || IsValue || other.IsValue) return false;
            foreach (var prop in other.Properties)
            {
                MergeOrAddProperty(prop.Value);
            }
            return true;
        }

        internal bool CompareFlags(VguiObject other)
        {
            foreach (var flag in Flags)
            {
                if (!other.Flags.TryGetValue(flag.Key, out var value) || flag.Value != value)
                    return false;
            }

            return true;
        }

        internal VguiNameFlagKey GetNameFlagKey() => new(Name, Flags);
    }
}