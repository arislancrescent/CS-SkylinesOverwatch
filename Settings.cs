using System;
using System.Collections.Generic;

namespace SkylinesOverwatch
{
    public sealed class Settings
    {
        private Settings()
        {
            Tag = "[ARIS] Skylines Overwatch";

            Animals = new HashSet<string>();
            Animals.Add("Seagull");
            Animals.Add("Cow");
            Animals.Add("Pig");
            Animals.Add("Dog");
            Animals.Add("Wolf");
            Animals.Add("Bear");
            Animals.Add("MooseMale");
            Animals.Add("MooseFemale");

            Debug._BuildingMonitor          = true;
            Debug._VehicleMonitor           = true;
            Debug._HumanMonitor             = true;
            Debug._AnimalMonitor            = true;

            Disabled._All                   = false;

            Disabled._BuildingMonitor       = false;

            Disabled._PlayerBuildings       = false;
            Disabled._Cemeteries            = false;
            Disabled._LandfillSites         = false;
            Disabled._FireStations          = false;
            Disabled._PoliceStations        = false;
            Disabled._Hospitals             = false;
            Disabled._Parks                 = false;
            Disabled._PlayerOther           = false;

            Disabled._PrivateBuildings      = false;
            Disabled._ResidentialBuildings  = false;
            Disabled._CommercialBuildings   = false;
            Disabled._IndustrialBuildings   = false;
            Disabled._OfficeBuildings       = false;
            Disabled._PrivateOther          = false;

            Disabled._DeadStatus            = false;
            Disabled._GarbageStatus         = false;
            Disabled._FireStatus            = false;
            Disabled._CrimeStatus           = false;
            Disabled._SickStatus            = false;

            Disabled._VehicleMonitor        = false;

            Disabled._Cars                  = false;
            Disabled._Trains                = false;
            Disabled._Aircrafts             = false;
            Disabled._Ships                 = false;
            Disabled._VehicleOther          = false;

            Disabled._Hearses               = false;
            Disabled._GarbageTrucks         = false;
            Disabled._FireTrucks            = false;
            Disabled._PoliceCars            = false;
            Disabled._Ambulances            = false;
            Disabled._Buses                 = false;
            Disabled._CarOther              = false;

            Disabled._HumanMonitor          = false;

            Disabled._Residents             = false;
            Disabled._ServicePersons        = false;
            Disabled._Tourists              = false;
            Disabled._HumanOther            = false;

            Disabled._AnimalMonitor         = false;

            Disabled._Birds                 = false;
            Disabled._Livestocks            = false;
            Disabled._Pets                  = false;
            Disabled._Wildlife              = false;
            Disabled._AnimalOther           = false;
        }

        private static readonly Settings _Instance = new Settings();
        public static Settings Instance { get { return _Instance; } }

        public readonly string Tag;
        public readonly HashSet<string> Animals;

        public DebugSettings Debug;
        public DisabledSettings Disabled;

        public struct DebugSettings
        {
            internal bool _BuildingMonitor;
            internal bool _VehicleMonitor;
            internal bool _HumanMonitor;
            internal bool _AnimalMonitor;

            public bool BuildingMonitor
            {
                get { return _BuildingMonitor; }
                set { if (value && !_BuildingMonitor) _BuildingMonitor = value; }
            }

            public bool VehicleMonitor
            {
                get { return _VehicleMonitor; }
                set { if (value && !_VehicleMonitor) _VehicleMonitor = value; }
            }

            public bool HumanMonitor
            {
                get { return _HumanMonitor; }
                set { if (value && !_HumanMonitor) _HumanMonitor = value; }
            }

            public bool AnimalMonitor
            {
                get { return _AnimalMonitor; }
                set { if (value && !_AnimalMonitor) _AnimalMonitor = value; }
            }
        }

        public struct DisabledSettings
        {
            internal bool _All;

            internal bool _BuildingMonitor;

            internal bool _PlayerBuildings;
            internal bool _Cemeteries;
            internal bool _LandfillSites;
            internal bool _FireStations;
            internal bool _PoliceStations;
            internal bool _Hospitals;
            internal bool _Parks;
            internal bool _PlayerOther;

            internal bool _PrivateBuildings;
            internal bool _ResidentialBuildings;
            internal bool _CommercialBuildings;
            internal bool _IndustrialBuildings;
            internal bool _OfficeBuildings;
            internal bool _PrivateOther;

            internal bool _DeadStatus;
            internal bool _GarbageStatus;
            internal bool _FireStatus;
            internal bool _CrimeStatus;
            internal bool _SickStatus;

            internal bool _VehicleMonitor;

            internal bool _Cars;
            internal bool _Trains;
            internal bool _Aircrafts;
            internal bool _Ships;
            internal bool _VehicleOther;

            internal bool _Hearses;
            internal bool _GarbageTrucks;
            internal bool _FireTrucks;
            internal bool _PoliceCars;
            internal bool _Ambulances;
            internal bool _Buses;
            internal bool _CarOther;

            internal bool _HumanMonitor;

            internal bool _Residents;
            internal bool _ServicePersons;
            internal bool _Tourists;
            internal bool _HumanOther;

            internal bool _AnimalMonitor;

            internal bool _Birds;
            internal bool _Livestocks;
            internal bool _Pets;
            internal bool _Wildlife;
            internal bool _AnimalOther;

            public bool BuildingMonitor
            {
                get { return _BuildingMonitor; }
                set { if (value && !_BuildingMonitor) _BuildingMonitor = value; }
            }

            public bool PlayerBuildings
            {
                get { return _PlayerBuildings; }
                set { if (value && !_PlayerBuildings) _PlayerBuildings = value; }
            }

            public bool Cemeteries
            {
                get { return _Cemeteries; }
                set { if (value && !_Cemeteries) _Cemeteries = value; }
            }

            public bool LandfillSites
            {
                get { return _LandfillSites; }
                set { if (value && !_LandfillSites) _LandfillSites = value; }
            }

            public bool FireStations
            {
                get { return _FireStations; }
                set { if (value && !_FireStations) _FireStations = value; }
            }

            public bool PoliceStations
            {
                get { return _PoliceStations; }
                set { if (value && !_PoliceStations) _PoliceStations = value; }
            }

            public bool Hospitals
            {
                get { return _Hospitals; }
                set { if (value && !_Hospitals) _Hospitals = value; }
            }

            public bool Parks
            {
                get { return _Parks; }
                set { if (value && !_Parks) _Parks = value; }
            }

            public bool PlayerOther
            {
                get { return _PlayerOther; }
                set { if (value && !_PlayerOther) _PlayerOther = value; }
            }

            public bool PrivateBuildings
            {
                get { return _PrivateBuildings; }
                set { if (value && !_PrivateBuildings) _PrivateBuildings = value; }
            }

            public bool ResidentialBuildings
            {
                get { return _ResidentialBuildings; }
                set { if (value && !_ResidentialBuildings) _ResidentialBuildings = value; }
            }

            public bool CommercialBuildings
            {
                get { return _CommercialBuildings; }
                set { if (value && !_CommercialBuildings) _CommercialBuildings = value; }
            }

            public bool IndustrialBuildings
            {
                get { return _IndustrialBuildings; }
                set { if (value && !_IndustrialBuildings) _IndustrialBuildings = value; }
            }

            public bool OfficeBuildings
            {
                get { return _OfficeBuildings; }
                set { if (value && !_OfficeBuildings) _OfficeBuildings = value; }
            }

            public bool PrivateOther
            {
                get { return _PrivateOther; }
                set { if (value && !_PrivateOther) _PrivateOther = value; }
            }

            public bool DeadStatus
            {
                get { return _DeadStatus; }
                set { if (value && !_DeadStatus) _DeadStatus = value; }
            }

            public bool GarbageStatus
            {
                get { return _GarbageStatus; }
                set { if (value && !_GarbageStatus) _GarbageStatus = value; }
            }

            public bool FireStatus
            {
                get { return _FireStatus; }
                set { if (value && !_FireStatus) _FireStatus = value; }
            }

            public bool CrimeStatus
            {
                get { return _CrimeStatus; }
                set { if (value && !_CrimeStatus) _CrimeStatus = value; }
            }

            public bool SickStatus
            {
                get { return _SickStatus; }
                set { if (value && !_SickStatus) _SickStatus = value; }
            }

            public bool VehicleMonitor
            {
                get { return _VehicleMonitor; }
                set { if (value && !_VehicleMonitor) _VehicleMonitor = value; }
            }

            public bool Cars
            {
                get { return _Cars; }
                set { if (value && !_Cars) _Cars = value; }
            }

            public bool Trains
            {
                get { return _Trains; }
                set { if (value && !_Trains) _Trains = value; }
            }

            public bool Aircrafts
            {
                get { return _Aircrafts; }
                set { if (value && !_Aircrafts) _Aircrafts = value; }
            }

            public bool Ships
            {
                get { return _Ships; }
                set { if (value && !_Ships) _Ships = value; }
            }

            public bool VehicleOther
            {
                get { return _VehicleOther; }
                set { if (value && !_VehicleOther) _VehicleOther = value; }
            }

            public bool Hearses
            {
                get { return _Hearses; }
                set { if (value && !_Hearses) _Hearses = value; }
            }

            public bool GarbageTrucks
            {
                get { return _GarbageTrucks; }
                set { if (value && !_GarbageTrucks) _GarbageTrucks = value; }
            }

            public bool FireTrucks
            {
                get { return _FireTrucks; }
                set { if (value && !_FireTrucks) _FireTrucks = value; }
            }

            public bool PoliceCars
            {
                get { return _PoliceCars; }
                set { if (value && !_PoliceCars) _PoliceCars = value; }
            }

            public bool Ambulances
            {
                get { return _Ambulances; }
                set { if (value && !_Ambulances) _Ambulances = value; }
            }

            public bool Buses
            {
                get { return _Buses; }
                set { if (value && !_Buses) _Buses = value; }
            }

            public bool CarOther
            {
                get { return _CarOther; }
                set { if (value && !_CarOther) _CarOther = value; }
            }

            public bool HumanMonitor
            {
                get { return _HumanMonitor; }
                set { if (value && !_HumanMonitor) _HumanMonitor = value; }
            }

            public bool Residents
            {
                get { return _Residents; }
                set { if (value && !_Residents) _Residents = value; }
            }

            public bool ServicePersons
            {
                get { return _ServicePersons; }
                set { if (value && !_ServicePersons) _ServicePersons = value; }
            }

            public bool Tourists
            {
                get { return _Tourists; }
                set { if (value && !_Tourists) _Tourists = value; }
            }

            public bool HumanOther
            {
                get { return _HumanOther; }
                set { if (value && !_HumanOther) _HumanOther = value; }
            }

            public bool AnimalMonitor
            {
                get { return _AnimalMonitor; }
                set { if (value && !_AnimalMonitor) _AnimalMonitor = value; }
            }

            public bool Birds
            {
                get { return _Birds; }
                set { if (value && !_Birds) _Birds = value; }
            }

            public bool Livestocks
            {
                get { return _Livestocks; }
                set { if (value && !_Livestocks) _Livestocks = value; }
            }

            public bool Pets
            {
                get { return _Pets; }
                set { if (value && !_Pets) _Pets = value; }
            }

            public bool Wildlife
            {
                get { return _Wildlife; }
                set { if (value && !_Wildlife) _Wildlife = value; }
            }

            public bool AnimalOther
            {
                get { return _AnimalOther; }
                set { if (value && !_AnimalOther) _AnimalOther = value; }
            }
        }
    }
}
