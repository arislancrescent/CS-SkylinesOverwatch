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

            Debug._BuildingMonitor          = false;
            Debug._VehicleMonitor           = false;
            Debug._HumanMonitor             = false;
            Debug._AnimalMonitor            = false;

            Enable._BuildingMonitor         = false;

            Enable._VehicleMonitor          = false;

            Enable._Cars                    = false;
            Enable._Trains                  = false;
            Enable._Aircrafts               = false;
            Enable._Ships                   = false;
            Enable._VehicleOther            = false;

            Enable._Hearses                 = false;
            Enable._GarbageTrucks           = false;
            Enable._FireTrucks              = false;
            Enable._PoliceCars              = false;
            Enable._Ambulances              = false;
            Enable._Buses                   = false;
            Enable._CarOther                = false;

            Enable._HumanMonitor            = false;

            Enable._Residents               = false;
            Enable._ServicePersons          = false;
            Enable._Tourists                = false;
            Enable._HumanOther              = false;

            Enable._AnimalMonitor           = false;
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
                set { if (value) _BuildingMonitor = true; }
            }

            public bool VehicleMonitor
            {
                get { return _VehicleMonitor; }
                set { if (value) _VehicleMonitor = true; }
            }

            public bool HumanMonitor
            {
                get { return _HumanMonitor; }
                set { if (value) _HumanMonitor = true; }
            }

            public bool AnimalMonitor
            {
                get { return _AnimalMonitor; }
                set
                {
                    if (value)
                    {
                        _BuildingMonitor = true;
                        _AnimalMonitor = true;
                    }
                }
            }
        }

        public struct EnableSettings
        {
            internal bool _BuildingMonitor;

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
                set
                {
                    if (value)
                    {
                        _BuildingMonitor = value;
                        _AnimalMonitor = value;
                    }
                }
            }
        }
    }
}
