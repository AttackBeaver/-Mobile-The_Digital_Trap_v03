using System;
using System.Collections.Generic;
using Crew.Model.Data.Properties;
using UnityEngine;

namespace Crew.Model.Data
{
    [Serializable]
    public class PerksData
    {
        [SerializeField] private StringProperty _used = new StringProperty();
        [SerializeField] public List<string> _unlocked;
        public StringProperty Used => _used;

        public void AddPerk(string id)
        {
            if (!_unlocked.Contains(id))
                _unlocked.Add(id);
        }

        public bool IsUnlocked(string id)
        {
            return _unlocked.Contains(id);
        }
    }
}