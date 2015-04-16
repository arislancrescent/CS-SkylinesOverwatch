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
    public class HumanPrefabMapping
    {
        private Settings _settings;
        private Helper _helper;
        private Data _data;

        private PrefabMapping<uint> _mapping;

        public HumanPrefabMapping()
        {
            _settings = Settings.Instance;
            _helper = Helper.Instance;
            _data = Data.Instance;

            _mapping = new PrefabMapping<uint>();
        }

        public List<HashSet<uint>> GetMapping(CitizenInfo human)
        {
            int prefabID = human.m_prefabDataIndex;

            if (!_mapping.PrefabMapped(prefabID))
                CategorizePrefab(human);

            return _mapping.GetMapping(prefabID);
        }

        private void CategorizePrefab(CitizenInfo human)
        {
            CitizenAI ai = human.m_citizenAI;
            int prefabID = human.m_prefabDataIndex;

            /*
             * Create a blank entry. This way, even if this prefab does not belong here
             * for some bizarre reason, we will have a record of it. This eliminates
             * the chance of a prefab getting evaluated more than once, ever.
             */
            _mapping.AddEntry(prefabID);

            if (ai is HumanAI)
            {
                _mapping.AddMapping(prefabID, _data._Humans);

                if (ai is ResidentAI)
                    _mapping.AddMapping(prefabID, _data._Residents);
                else if (ai is ServicePersonAI)
                    _mapping.AddMapping(prefabID, _data._ServicePersons);
                else if (ai is TouristAI)
                    _mapping.AddMapping(prefabID, _data._Tourists);
                else
                    _mapping.AddMapping(prefabID, _data._HumanOther);
            }
        }
    }
}