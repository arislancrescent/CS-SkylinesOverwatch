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

            AiType.BuildingAI               = typeof(BuildingAI);

            AiType.PlayerBuildingAI         = typeof(PlayerBuildingAI);
            AiType.CemeteryAI               = typeof(CemeteryAI);
            AiType.LandfillSiteAI           = typeof(LandfillSiteAI);
            AiType.FireStationAI            = typeof(FireStationAI);
            AiType.PoliceStationAI          = typeof(PoliceStationAI);
            AiType.HospitalAI               = typeof(HospitalAI);
            AiType.ParkAI                   = typeof(ParkAI);

            AiType.PrivateBuildingAI        = typeof(PrivateBuildingAI);
            AiType.ResidentialBuildingAI    = typeof(ResidentialBuildingAI);
            AiType.CommercialBuildingAI     = typeof(CommercialBuildingAI);
            AiType.IndustrialBuildingAI     = typeof(IndustrialBuildingAI);
            AiType.OfficeBuildingAI         = typeof(OfficeBuildingAI);

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

            AiType.AnimalAI                 = typeof(AnimalAI);
            AiType.BirdAI                   = typeof(BirdAI);
            AiType.LivestockAI              = typeof(LivestockAI);
            AiType.PetAI                    = typeof(PetAI);
            AiType.WildlifeAI               = typeof(WildlifeAI);
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

        internal AnimalMonitor AnimalMonitor;

        public void RequestAnimalRemoval(ushort id)
        {
            if (AnimalMonitor != null)
                AnimalMonitor.RequestRemoval(id);
        }

        internal struct AiTypes
        {
            // Buildings
            public Type BuildingAI;

            public Type PlayerBuildingAI;
            public Type CemeteryAI;
            public Type LandfillSiteAI;
            public Type FireStationAI;
            public Type PoliceStationAI;
            public Type HospitalAI;
            public Type ParkAI;

            public Type PrivateBuildingAI;
            public Type ResidentialBuildingAI;
            public Type CommercialBuildingAI;
            public Type IndustrialBuildingAI;
            public Type OfficeBuildingAI;

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

            public Type AnimalAI;
            public Type BirdAI;
            public Type LivestockAI;
            public Type PetAI;
            public Type WildlifeAI;
        }

        internal void Log(string message)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("{0}: {1}", Settings.Instance.Tag, message));
        }
    }
}