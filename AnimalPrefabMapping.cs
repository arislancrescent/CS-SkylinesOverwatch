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
    public class AnimalPrefabMapping
    {
        private Settings _settings;
        private Helper _helper;
        private Data _data;

        private Dictionary<string, int> _prefabs;

        private PrefabMapping<ushort> _mapping;

        public AnimalPrefabMapping()
        {
            _settings = Settings.Instance;
            _helper = Helper.Instance;
            _data = Data.Instance;

            _prefabs = new Dictionary<string, int>();
            LoadTrackedPrefabs();

            _mapping = new PrefabMapping<ushort>();
        }

        public List<HashSet<ushort>> GetMapping(CitizenInfo animal)
        {
            int prefabID = animal.m_prefabDataIndex;

            if (!_mapping.PrefabMapped(prefabID))
                CategorizePrefab(animal);

            return _mapping.GetMapping(prefabID);
        }

        private void LoadTrackedPrefabs()
        {
            for (uint i = 0; i < PrefabCollection<CitizenInfo>.PrefabCount(); i++)
            {
                CitizenInfo prefab = PrefabCollection<CitizenInfo>.GetPrefab(i);

                if (prefab == null)
                    continue;

                string name = prefab.GetLocalizedTitle();

                if (String.IsNullOrEmpty(name))
                    continue;

                if (!_settings.Animals.Contains(name))
                    continue;

                if (_prefabs.ContainsKey(name))
                    continue;

                _prefabs.Add(name, (int)i);
            }
        }

        private void CategorizePrefab(CitizenInfo animal)
        {
            CitizenAI ai = animal.m_citizenAI;
            int prefabID = animal.m_prefabDataIndex;

            if (ai is AnimalAI)
            {
                _mapping.AddMapping(prefabID, _data._Animals);

                if (ai is BirdAI)
                {
                    _mapping.AddMapping(prefabID, _data._Birds);

                    if (_prefabs.ContainsKey("Seagull") && _prefabs["Seagull"] == prefabID)
                        _mapping.AddMapping(prefabID, _data._Seagulls);
                }
                else if (ai is LivestockAI)
                {
                    _mapping.AddMapping(prefabID, _data._Livestocks);

                    if (_prefabs.ContainsKey("Cow") && _prefabs["Cow"] == prefabID)
                        _mapping.AddMapping(prefabID, _data._Cows);

                    if (_prefabs.ContainsKey("Pig") && _prefabs["Pig"] == prefabID)
                        _mapping.AddMapping(prefabID, _data._Pigs);
                }
                else if (ai is PetAI)
                {
                    _mapping.AddMapping(prefabID, _data._Pets);

                    if (_prefabs.ContainsKey("Dog") && _prefabs["Dog"] == prefabID)
                        _mapping.AddMapping(prefabID, _data._Dogs);
                }
                else if (ai is WildlifeAI)
                {
                    _mapping.AddMapping(prefabID, _data._Wildlife);

                    if (_prefabs.ContainsKey("Wolf") && _prefabs["Wolf"] == prefabID)
                        _mapping.AddMapping(prefabID, _data._Wolves);

                    if (_prefabs.ContainsKey("Bear") && _prefabs["Bear"] == prefabID)
                        _mapping.AddMapping(prefabID, _data._Bears);

                    if (_prefabs.ContainsKey("Moose") && _prefabs["Moose"] == prefabID)
                        _mapping.AddMapping(prefabID, _data._Moose);
                }
                else
                {
                    _mapping.AddMapping(prefabID, _data._AnimalOther);
                }
            }
        }
    }
}