using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

using ICities;
using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.Math;
using ColossalFramework.UI;
using UnityEngine;

namespace SkylinesOverwatch
{
    public sealed class Helper
    {
        private Helper()
        {
            GameLoaded                      = false;

            _BuildingMonitorSpun            = false;
            _VehicleMonitorSpun             = false;
            _HumanMonitorSpun               = false;
            _AnimalMonitorSpun              = false;

            // Vehicles
            AiType.VehicleAI                = typeof(VehicleAI);

            AiType.CarAI                    = typeof(CarAI);
            AiType.TrainAI                  = typeof(TrainAI);
            AiType.AircraftAI               = typeof(AircraftAI);
            AiType.ShipAI                   = typeof(ShipAI);

            AiType.CarTrailerAI             = typeof(CarTrailerAI);
            AiType.HearseAI                 = typeof(HearseAI);
            AiType.GarbageTruckAI           = typeof(GarbageTruckAI);
            AiType.FireTruckAI              = typeof(FireTruckAI);
            AiType.PoliceCarAI              = typeof(PoliceCarAI);
            AiType.AmbulanceAI              = typeof(AmbulanceAI);
            AiType.BusAI                    = typeof(BusAI);

            // Citizens
            AiType.CitizenAI                = typeof(CitizenAI);

            AiType.HumanAI                  = typeof(HumanAI);
            AiType.ResidentAI               = typeof(ResidentAI);
            AiType.ServicePersonAI          = typeof(ServicePersonAI);
            AiType.TouristAI                = typeof(TouristAI);
        }

        private static readonly Helper _Instance = new Helper();
        public static Helper Instance { get { return _Instance; } }

        internal AiTypes AiType;
        internal bool GameLoaded;

        private bool _BuildingMonitorSpun;
        private bool _VehicleMonitorSpun;
        private bool _HumanMonitorSpun;
        private bool _AnimalMonitorSpun;

        internal bool BuildingMonitorSpun
        {
            get { return BuildingMonitorSpinnable && _BuildingMonitorSpun; }
            set { _BuildingMonitorSpun = BuildingMonitorSpinnable ? value : false; }
        }

        internal bool VehicleMonitorSpun
        {
            get { return VehicleMonitorSpinnable && _VehicleMonitorSpun; }
            set { _VehicleMonitorSpun = VehicleMonitorSpinnable ? value : false; }
        }

        internal bool HumanMonitorSpun
        {
            get { return HumanMonitorSpinnable && _HumanMonitorSpun; }
            set { _HumanMonitorSpun = HumanMonitorSpinnable ? value : false; }
        }

        internal bool AnimalMonitorSpun
        {
            get { return AnimalMonitorSpinnable && _AnimalMonitorSpun; }
            set { _AnimalMonitorSpun = AnimalMonitorSpinnable ? value : false; }
        }

        internal bool BuildingMonitorSpinnable  { get { return GameLoaded; } }
        internal bool VehicleMonitorSpinnable   { get { return GameLoaded; } }
        internal bool HumanMonitorSpinnable     { get { return GameLoaded; } }
        internal bool AnimalMonitorSpinnable    { get { return BuildingMonitorSpun; } }

        internal BuildingMonitor BuildingMonitor;
        internal AnimalMonitor AnimalMonitor;

        public void RequestBuildingRemoval(ushort id)
        {
            if (BuildingMonitor != null)
                BuildingMonitor.RequestRemoval(id);
        }

        public void RequestAnimalRemoval(ushort id)
        {
            if (AnimalMonitor != null)
                AnimalMonitor.RequestRemoval(id);
        }

        internal struct AiTypes
        {
            // Vehicles
            public Type VehicleAI;

            public Type CarAI;
            public Type TrainAI;
            public Type AircraftAI;
            public Type ShipAI;

            public Type CarTrailerAI;
            public Type HearseAI;
            public Type GarbageTruckAI;
            public Type FireTruckAI;
            public Type PoliceCarAI;
            public Type AmbulanceAI;
            public Type BusAI;

            // Citizens
            public Type CitizenAI;

            public Type HumanAI;
            public Type ResidentAI;
            public Type ServicePersonAI;
            public Type TouristAI;
        }

        internal void Log(string message)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("{0}: {1}", Settings.Instance.Tag, message));
        }
    }
}