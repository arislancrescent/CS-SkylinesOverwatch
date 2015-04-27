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
    public class BuildingPrefabMapping
    {
        private Settings _settings;
        private Helper _helper;
        private Data _data;

        private PrefabMapping<ushort> _mapping;

        public BuildingPrefabMapping()
        {
            _settings = Settings.Instance;
            _helper = Helper.Instance;
            _data = Data.Instance;

            _mapping = new PrefabMapping<ushort>();
        }

        public List<HashSet<ushort>> GetMapping(BuildingInfo building)
        {
            int prefabID = building.m_prefabDataIndex;

            if (!_mapping.PrefabMapped(prefabID))
                CategorizePrefab(building);

            return _mapping.GetMapping(prefabID);
        }

        private void CategorizePrefab(BuildingInfo building)
        {
            BuildingAI ai = building.m_buildingAI;
            int prefabID = building.m_prefabDataIndex;

            _mapping.AddMapping(prefabID, _data._Buildings);

            if (ai is PlayerBuildingAI)
            {
                _mapping.AddMapping(prefabID, _data._PlayerBuildings);

                if (ai is CemeteryAI)
                    _mapping.AddMapping(prefabID, _data._Cemeteries);
                else if (ai is LandfillSiteAI)
                    _mapping.AddMapping(prefabID, _data._LandfillSites);
                else if (ai is FireStationAI)
                    _mapping.AddMapping(prefabID, _data._FireStations);
                else if (ai is PoliceStationAI)
                    _mapping.AddMapping(prefabID, _data._PoliceStations);
                else if (ai is HospitalAI)
                    _mapping.AddMapping(prefabID, _data._Hospitals);
                else if (ai is ParkAI)
                    _mapping.AddMapping(prefabID, _data._Parks);
                else if (ai is PowerPlantAI)
                    _mapping.AddMapping(prefabID, _data._PowerPlants);
                else
                    _mapping.AddMapping(prefabID, _data._PlayerOther);
            }
            else if (ai is PrivateBuildingAI)
            {
                _mapping.AddMapping(prefabID, _data._PrivateBuildings);

                if (ai is ResidentialBuildingAI)
                    _mapping.AddMapping(prefabID, _data._ResidentialBuildings);
                else if (ai is CommercialBuildingAI)
                    _mapping.AddMapping(prefabID, _data._CommercialBuildings);
                else if (ai is IndustrialBuildingAI)
                    _mapping.AddMapping(prefabID, _data._IndustrialBuildings);
                else if (ai is OfficeBuildingAI)
                    _mapping.AddMapping(prefabID, _data._OfficeBuildings);
                else
                    _mapping.AddMapping(prefabID, _data._PrivateOther);
            }
            else
                _mapping.AddMapping(prefabID, _data._BuildingOther);
        }
    }
}