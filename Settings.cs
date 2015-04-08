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

            Enable._BuildingMonitor         = true;

            Enable._PlayerBuildings         = true;
            Enable._Cemeteries              = true;
            Enable._LandfillSites           = true;
            Enable._FireStations            = true;
            Enable._PoliceStations          = true;
            Enable._Hospitals               = true;
            Enable._Parks                   = true;
            Enable._PlayerOther             = true;

            Enable._PrivateBuildings        = true;
            Enable._ResidentialBuildings    = true;
            Enable._CommercialBuildings     = true;
            Enable._IndustrialBuildings     = true;
            Enable._OfficeBuildings         = true;
            Enable._PrivateOther            = true;

            Enable._DeadStatus              = true;
            Enable._GarbageStatus           = true;
            Enable._FireStatus              = true;
            Enable._CrimeStatus             = true;
            Enable._SickStatus              = true;

            Enable._VehicleMonitor          = true;

            Enable._Cars                    = true;
            Enable._Trains                  = true;
            Enable._Aircrafts               = true;
            Enable._Ships                   = true;
            Enable._VehicleOther            = true;

            Enable._Hearses                 = true;
            Enable._GarbageTrucks           = true;
            Enable._FireTrucks              = true;
            Enable._PoliceCars              = true;
            Enable._Ambulances              = true;
            Enable._Buses                   = true;
            Enable._CarOther                = true;

            Enable._HumanMonitor            = true;

            Enable._Residents               = true;
            Enable._ServicePersons          = true;
            Enable._Tourists                = true;
            Enable._HumanOther              = true;

            Enable._AnimalMonitor           = true;

            Enable._Birds                   = true;
            Enable._Livestocks              = true;
            Enable._Pets                    = true;
            Enable._Wildlife                = true;
            Enable._AnimalOther             = true;
        }

        private static readonly Settings _Instance = new Settings();
        public static Settings Instance { get { return _Instance; } }

        public readonly string Tag;
        public readonly HashSet<string> Animals;

        public DebugSettings Debug;
        public EnableSettings Enable;

        public struct DebugSettings
        {
            internal bool _BuildingMonitor;
            internal bool _VehicleMonitor;
            internal bool _HumanMonitor;
            internal bool _AnimalMonitor;

            public bool BuildingMonitor
            {
                get { return _BuildingMonitor; }
                set { if (value) _BuildingMonitor = value; }
            }

            public bool VehicleMonitor
            {
                get { return _VehicleMonitor; }
                set { if (value) _VehicleMonitor = value; }
            }

            public bool HumanMonitor
            {
                get { return _HumanMonitor; }
                set { if (value) _HumanMonitor = value; }
            }

            public bool AnimalMonitor
            {
                get { return _AnimalMonitor; }
                set { if (value) _AnimalMonitor = value; }
            }
        }

        public struct EnableSettings
        {
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
                set { if (value) _BuildingMonitor = value; }
            }

            public bool PlayerBuildings
            {
                get { return _PlayerBuildings; }
                set { if (value) _PlayerBuildings = value; }
            }

            public bool Cemeteries
            {
                get { return _Cemeteries; }
                set { if (value) _Cemeteries = value; }
            }

            public bool LandfillSites
            {
                get { return _LandfillSites; }
                set { if (value) _LandfillSites = value; }
            }

            public bool FireStations
            {
                get { return _FireStations; }
                set { if (value) _FireStations = value; }
            }

            public bool PoliceStations
            {
                get { return _PoliceStations; }
                set { if (value) _PoliceStations = value; }
            }

            public bool Hospitals
            {
                get { return _Hospitals; }
                set { if (value) _Hospitals = value; }
            }

            public bool Parks
            {
                get { return _Parks; }
                set { if (value) _Parks = value; }
            }

            public bool PlayerOther
            {
                get { return _PlayerOther; }
                set { if (value) _PlayerOther = value; }
            }

            public bool PrivateBuildings
            {
                get { return _PrivateBuildings; }
                set { if (value) _PrivateBuildings = value; }
            }

            public bool ResidentialBuildings
            {
                get { return _ResidentialBuildings; }
                set { if (value) _ResidentialBuildings = value; }
            }

            public bool CommercialBuildings
            {
                get { return _CommercialBuildings; }
                set { if (value) _CommercialBuildings = value; }
            }

            public bool IndustrialBuildings
            {
                get { return _IndustrialBuildings; }
                set { if (value) _IndustrialBuildings = value; }
            }

            public bool OfficeBuildings
            {
                get { return _OfficeBuildings; }
                set { if (value) _OfficeBuildings = value; }
            }

            public bool PrivateOther
            {
                get { return _PrivateOther; }
                set { if (value) _PrivateOther = value; }
            }

            public bool DeadStatus
            {
                get { return _DeadStatus; }
                set { if (value) _DeadStatus = value; }
            }

            public bool GarbageStatus
            {
                get { return _GarbageStatus; }
                set { if (value) _GarbageStatus = value; }
            }

            public bool FireStatus
            {
                get { return _FireStatus; }
                set { if (value) _FireStatus = value; }
            }

            public bool CrimeStatus
            {
                get { return _CrimeStatus; }
                set { if (value) _CrimeStatus = value; }
            }

            public bool SickStatus
            {
                get { return _SickStatus; }
                set { if (value) _SickStatus = value; }
            }

            public bool VehicleMonitor
            {
                get { return _VehicleMonitor; }
                set { if (value) _VehicleMonitor = value; }
            }

            public bool Cars
            {
                get { return _Cars; }
                set { if (value) _Cars = value; }
            }

            public bool Trains
            {
                get { return _Trains; }
                set { if (value) _Trains = value; }
            }

            public bool Aircrafts
            {
                get { return _Aircrafts; }
                set { if (value) _Aircrafts = value; }
            }

            public bool Ships
            {
                get { return _Ships; }
                set { if (value) _Ships = value; }
            }

            public bool VehicleOther
            {
                get { return _VehicleOther; }
                set { if (value) _VehicleOther = value; }
            }

            public bool Hearses
            {
                get { return _Hearses; }
                set { if (value) _Hearses = value; }
            }

            public bool GarbageTrucks
            {
                get { return _GarbageTrucks; }
                set { if (value) _GarbageTrucks = value; }
            }

            public bool FireTrucks
            {
                get { return _FireTrucks; }
                set { if (value) _FireTrucks = value; }
            }

            public bool PoliceCars
            {
                get { return _PoliceCars; }
                set { if (value) _PoliceCars = value; }
            }

            public bool Ambulances
            {
                get { return _Ambulances; }
                set { if (value) _Ambulances = value; }
            }

            public bool Buses
            {
                get { return _Buses; }
                set { if (value) _Buses = value; }
            }

            public bool CarOther
            {
                get { return _CarOther; }
                set { if (value) _CarOther = value; }
            }

            public bool HumanMonitor
            {
                get { return _HumanMonitor; }
                set { if (value) _HumanMonitor = value; }
            }

            public bool Residents
            {
                get { return _Residents; }
                set { if (value) _Residents = value; }
            }

            public bool ServicePersons
            {
                get { return _ServicePersons; }
                set { if (value) _ServicePersons = value; }
            }

            public bool Tourists
            {
                get { return _Tourists; }
                set { if (value) _Tourists = value; }
            }

            public bool HumanOther
            {
                get { return _HumanOther; }
                set { if (value) _HumanOther = value; }
            }

            public bool AnimalMonitor
            {
                get { return _AnimalMonitor; }
                set { if (value) _AnimalMonitor = value; }
            }

            public bool Birds
            {
                get { return _Birds; }
                set { if (value) _Birds = value; }
            }

            public bool Livestocks
            {
                get { return _Livestocks; }
                set { if (value) _Livestocks = value; }
            }

            public bool Pets
            {
                get { return _Pets; }
                set { if (value) _Pets = value; }
            }

            public bool Wildlife
            {
                get { return _Wildlife; }
                set { if (value) _Wildlife = value; }
            }

            public bool AnimalOther
            {
                get { return _AnimalOther; }
                set { if (value) _AnimalOther = value; }
            }
        }
    }
}
