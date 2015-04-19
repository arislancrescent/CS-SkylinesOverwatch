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
    public class VehiclePrefabMapping
    {
        private Settings _settings;
        private Helper _helper;
        private Data _data;

        private PrefabMapping<ushort> _mapping;

        public VehiclePrefabMapping()
        {
            _settings = Settings.Instance;
            _helper = Helper.Instance;
            _data = Data.Instance;

            _mapping = new PrefabMapping<ushort>();
        }

        public List<HashSet<ushort>> GetMapping(VehicleInfo vehicle)
        {
            int prefabID = vehicle.m_prefabDataIndex;

            if (!_mapping.PrefabMapped(prefabID))
                CategorizePrefab(vehicle);

            return _mapping.GetMapping(prefabID);
        }

        private void CategorizePrefab(VehicleInfo vehicle)
        {
            VehicleAI ai = vehicle.m_vehicleAI;
            int prefabID = vehicle.m_prefabDataIndex;

            _mapping.AddMapping(prefabID, _data._Vehicles);

            if (ai is CarTrailerAI)
                return;
            else if (ai is CarAI)
            {
                _mapping.AddMapping(prefabID, _data._Cars);

                if (ai is HearseAI)
                    _mapping.AddMapping(prefabID, _data._Hearses);
                else if (ai is GarbageTruckAI)
                    _mapping.AddMapping(prefabID, _data._GarbageTrucks);
                else if (ai is FireTruckAI)
                    _mapping.AddMapping(prefabID, _data._FireTrucks);
                else if (ai is PoliceCarAI)
                    _mapping.AddMapping(prefabID, _data._PoliceCars);
                else if (ai is AmbulanceAI)
                    _mapping.AddMapping(prefabID, _data._Ambulances);
                else if (ai is BusAI)
                    _mapping.AddMapping(prefabID, _data._Buses);
                else
                    _mapping.AddMapping(prefabID, _data._CarOther);
            }
            else if (ai is TrainAI)
            {
                _mapping.AddMapping(prefabID, _data._Trains);

                if (ai is MetroTrainAI)
                    _mapping.AddMapping(prefabID, _data._MetroTrains);
                else if (ai is PassengerTrainAI)
                    _mapping.AddMapping(prefabID, _data._PassengerTrains);
                else if (ai is CargoTrainAI)
                    _mapping.AddMapping(prefabID, _data._CargoTrains);
                else
                    _mapping.AddMapping(prefabID, _data._TrainOther);
            }
            else if (ai is AircraftAI)
                _mapping.AddMapping(prefabID, _data._Aircraft);
            else if (ai is ShipAI)
                _mapping.AddMapping(prefabID, _data._Ships);
            else
                _mapping.AddMapping(prefabID, _data._VehicleOther);
        }
    }
}