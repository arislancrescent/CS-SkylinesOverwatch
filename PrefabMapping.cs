using System;
using System.Collections.Generic;
using System.Threading;

using ICities;
using ColossalFramework;
using ColossalFramework.Math;
using ColossalFramework.UI;
using UnityEngine;

namespace SkylinesOverwatch
{
    public class PrefabMapping<T>
    {
        private Dictionary<int, List<HashSet<T>>> _mapping;

        public PrefabMapping()
        {
            _mapping = new Dictionary<int, List<HashSet<T>>>();
        }

        public bool PrefabMapped(int prefabID)
        {
            return _mapping.ContainsKey(prefabID);
        }

        public List<HashSet<T>> GetMapping(int prefabID)
        {
            if (_mapping.ContainsKey(prefabID))
                return _mapping[prefabID];
            else
                return new List<HashSet<T>>();
        }

        public void AddMapping(int prefabID, HashSet<T> storage)
        {
            if (!_mapping.ContainsKey(prefabID))
                _mapping.Add(prefabID, new List<HashSet<T>>());

            _mapping[prefabID].Add(storage);
        }
    }
}

