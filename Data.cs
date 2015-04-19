using System;
using System.Collections.Generic;
using System.Linq;

namespace SkylinesOverwatch
{
    public sealed class Data
    {
        private Data()
        {
            _Buildings               = new HashSet<ushort>();
            _BuildingsUpdated        = new HashSet<ushort>();
            _BuildingsRemoved        = new HashSet<ushort>();

            _PlayerBuildings         = new HashSet<ushort>();
            _Cemeteries              = new HashSet<ushort>();
            _LandfillSites           = new HashSet<ushort>();
            _FireStations            = new HashSet<ushort>();
            _PoliceStations          = new HashSet<ushort>();
            _Hospitals               = new HashSet<ushort>();
            _Parks                   = new HashSet<ushort>();
            _PlayerOther             = new HashSet<ushort>();

            _PrivateBuildings        = new HashSet<ushort>();
            _ResidentialBuildings    = new HashSet<ushort>();
            _CommercialBuildings     = new HashSet<ushort>();
            _IndustrialBuildings     = new HashSet<ushort>();
            _OfficeBuildings         = new HashSet<ushort>();
            _PrivateOther            = new HashSet<ushort>();

            _BuildingOther           = new HashSet<ushort>();

            _BuildingsAbandoned      = new HashSet<ushort>();
            _BuildingsWithDead       = new HashSet<ushort>();
            _BuildingsWithGarbage    = new HashSet<ushort>();
            _BuildingsWithFire       = new HashSet<ushort>();
            _BuildingsWithCrime      = new HashSet<ushort>();
            _BuildingsWithSick       = new HashSet<ushort>();

            // Vehicles
            _Vehicles                = new HashSet<ushort>();
            _VehiclesUpdated         = new HashSet<ushort>();
            _VehiclesRemoved         = new HashSet<ushort>();

            _Cars                    = new HashSet<ushort>();
            _Trains                  = new HashSet<ushort>();
            _Aircraft                = new HashSet<ushort>();
            _Ships                   = new HashSet<ushort>();
            _VehicleOther            = new HashSet<ushort>();

            _Hearses                 = new HashSet<ushort>();
            _GarbageTrucks           = new HashSet<ushort>();
            _FireTrucks              = new HashSet<ushort>();
            _PoliceCars              = new HashSet<ushort>();
            _Ambulances              = new HashSet<ushort>();
            _Buses                   = new HashSet<ushort>();
            _CarOther                = new HashSet<ushort>();

            _PassengerTrains         = new HashSet<ushort>();
            _MetroTrains             = new HashSet<ushort>();
            _CargoTrains             = new HashSet<ushort>();
            _TrainOther              = new HashSet<ushort>();

            // Humans
            _Humans                  = new HashSet<uint>();
            _HumansUpdated           = new HashSet<uint>();
            _HumansRemoved           = new HashSet<uint>();

            _Residents               = new HashSet<uint>();
            _ServicePersons          = new HashSet<uint>();
            _Tourists                = new HashSet<uint>();
            _HumanOther              = new HashSet<uint>();

            // Animals
            _Animals                 = new HashSet<ushort>();
            _AnimalsUpdated          = new HashSet<ushort>();
            _AnimalsRemoved          = new HashSet<ushort>();

            _Birds                   = new HashSet<ushort>();
            _Seagulls                = new HashSet<ushort>();

            _Livestock               = new HashSet<ushort>();
            _Cows                    = new HashSet<ushort>();
            _Pigs                    = new HashSet<ushort>();

            _Pets                    = new HashSet<ushort>();
            _Dogs                    = new HashSet<ushort>();

            _Wildlife                = new HashSet<ushort>();
            _Wolves                  = new HashSet<ushort>();
            _Bears                   = new HashSet<ushort>();
            _Moose                   = new HashSet<ushort>();

            _AnimalOther             = new HashSet<ushort>();
        }

        private static readonly Data _Instance = new Data();
        public static Data Instance { get { return _Instance; } }

        // Buildings
        internal HashSet<ushort> _Buildings;
        internal HashSet<ushort> _BuildingsUpdated;
        internal HashSet<ushort> _BuildingsRemoved;

        internal HashSet<ushort> _PlayerBuildings;
        internal HashSet<ushort> _Cemeteries;
        internal HashSet<ushort> _LandfillSites;
        internal HashSet<ushort> _FireStations;
        internal HashSet<ushort> _PoliceStations;
        internal HashSet<ushort> _Hospitals;
        internal HashSet<ushort> _Parks;
        internal HashSet<ushort> _PlayerOther;

        internal HashSet<ushort> _PrivateBuildings;
        internal HashSet<ushort> _ResidentialBuildings;
        internal HashSet<ushort> _CommercialBuildings;
        internal HashSet<ushort> _IndustrialBuildings;
        internal HashSet<ushort> _OfficeBuildings;
        internal HashSet<ushort> _PrivateOther;

        internal HashSet<ushort> _BuildingOther;

        internal HashSet<ushort> _BuildingsAbandoned;
        internal HashSet<ushort> _BuildingsWithDead;
        internal HashSet<ushort> _BuildingsWithGarbage;
        internal HashSet<ushort> _BuildingsWithFire;
        internal HashSet<ushort> _BuildingsWithCrime;
        internal HashSet<ushort> _BuildingsWithSick;

        // Vehicles
        internal HashSet<ushort> _Vehicles;
        internal HashSet<ushort> _VehiclesUpdated;
        internal HashSet<ushort> _VehiclesRemoved;

        internal HashSet<ushort> _Cars;
        internal HashSet<ushort> _Trains;
        internal HashSet<ushort> _Aircraft;
        internal HashSet<ushort> _Ships;
        internal HashSet<ushort> _VehicleOther;

        internal HashSet<ushort> _Hearses;
        internal HashSet<ushort> _GarbageTrucks;
        internal HashSet<ushort> _FireTrucks;
        internal HashSet<ushort> _PoliceCars;
        internal HashSet<ushort> _Ambulances;
        internal HashSet<ushort> _Buses;
        internal HashSet<ushort> _CarOther;

        internal HashSet<ushort> _PassengerTrains;
        internal HashSet<ushort> _MetroTrains;
        internal HashSet<ushort> _CargoTrains;
        internal HashSet<ushort> _TrainOther;

        // Humans
        internal HashSet<uint>   _Humans;
        internal HashSet<uint>   _HumansUpdated;
        internal HashSet<uint>   _HumansRemoved;

        internal HashSet<uint>   _Residents;
        internal HashSet<uint>   _ServicePersons;
        internal HashSet<uint>   _Tourists;
        internal HashSet<uint>   _HumanOther;

        // Animals
        internal HashSet<ushort> _Animals;
        internal HashSet<ushort> _AnimalsUpdated;
        internal HashSet<ushort> _AnimalsRemoved;

        internal HashSet<ushort> _Birds;
        internal HashSet<ushort> _Seagulls;

        internal HashSet<ushort> _Livestock;
        internal HashSet<ushort> _Cows;
        internal HashSet<ushort> _Pigs;

        internal HashSet<ushort> _Pets;
        internal HashSet<ushort> _Dogs;

        internal HashSet<ushort> _Wildlife;
        internal HashSet<ushort> _Wolves;
        internal HashSet<ushort> _Bears;
        internal HashSet<ushort> _Moose;

        internal HashSet<ushort> _AnimalOther;

        // Public accessors
        public ushort[] Buildings               { get { return _Buildings.ToArray<ushort>(); } }
        public ushort[] BuildingsUpdated        { get { return _BuildingsUpdated.ToArray<ushort>(); } }
        public ushort[] BuildingsRemoved        { get { return _BuildingsRemoved.ToArray<ushort>(); } }

        public ushort[] PlayerBuildings         { get { return _PlayerBuildings.ToArray<ushort>(); } }
        public ushort[] Cemeteries              { get { return _Cemeteries.ToArray<ushort>(); } }
        public ushort[] LandfillSites           { get { return _LandfillSites.ToArray<ushort>(); } }
        public ushort[] FireStations            { get { return _FireStations.ToArray<ushort>(); } }
        public ushort[] PoliceStations          { get { return _PoliceStations.ToArray<ushort>(); } }
        public ushort[] Hospitals               { get { return _Hospitals.ToArray<ushort>(); } }
        public ushort[] Parks                   { get { return _Parks.ToArray<ushort>(); } }
        public ushort[] PlayerOther             { get { return _PlayerOther.ToArray<ushort>(); } }

        public ushort[] PrivateBuildings        { get { return _PrivateBuildings.ToArray<ushort>(); } }
        public ushort[] ResidentialBuildings    { get { return _ResidentialBuildings.ToArray<ushort>(); } }
        public ushort[] CommercialBuildings     { get { return _CommercialBuildings.ToArray<ushort>(); } }
        public ushort[] IndustrialBuildings     { get { return _IndustrialBuildings.ToArray<ushort>(); } }
        public ushort[] OfficeBuildings         { get { return _OfficeBuildings.ToArray<ushort>(); } }
        public ushort[] PrivateOther            { get { return _PrivateOther.ToArray<ushort>(); } }

        public ushort[] BuildingOther           { get { return _BuildingOther.ToArray<ushort>(); } }

        public ushort[] BuildingsAbandoned      { get { return _BuildingsAbandoned.ToArray<ushort>(); } }
        public ushort[] BuildingsWithDead       { get { return _BuildingsWithDead.ToArray<ushort>(); } }
        public ushort[] BuildingsWithGarbage    { get { return _BuildingsWithGarbage.ToArray<ushort>(); } }
        public ushort[] BuildingsWithFire       { get { return _BuildingsWithFire.ToArray<ushort>(); } }
        public ushort[] BuildingsWithCrime      { get { return _BuildingsWithCrime.ToArray<ushort>(); } }
        public ushort[] BuildingsWithSick       { get { return _BuildingsWithSick.ToArray<ushort>(); } }

        public ushort[] Vehicles                { get { return _Vehicles.ToArray<ushort>(); } }
        public ushort[] VehiclesUpdated         { get { return _VehiclesUpdated.ToArray<ushort>(); } }
        public ushort[] VehiclesRemoved         { get { return _VehiclesRemoved.ToArray<ushort>(); } }

        public ushort[] Cars                    { get { return _Cars.ToArray<ushort>(); } }
        public ushort[] Trains                  { get { return _Trains.ToArray<ushort>(); } }
        public ushort[] Aircraft                { get { return _Aircraft.ToArray<ushort>(); } }
        public ushort[] Ships                   { get { return _Ships.ToArray<ushort>(); } }
        public ushort[] VehicleOther            { get { return _VehicleOther.ToArray<ushort>(); } }

        public ushort[] Hearses                 { get { return _Hearses.ToArray<ushort>(); } }
        public ushort[] GarbageTrucks           { get { return _GarbageTrucks.ToArray<ushort>(); } }
        public ushort[] FireTrucks              { get { return _FireTrucks.ToArray<ushort>(); } }
        public ushort[] PoliceCars              { get { return _PoliceCars.ToArray<ushort>(); } }
        public ushort[] Ambulances              { get { return _Ambulances.ToArray<ushort>(); } }
        public ushort[] Buses                   { get { return _Buses.ToArray<ushort>(); } }
        public ushort[] CarOther                { get { return _CarOther.ToArray<ushort>(); } }

        public ushort[] PassengerTrains         { get { return _PassengerTrains.ToArray<ushort>(); } }
        public ushort[] MetroTrains             { get { return _MetroTrains.ToArray<ushort>(); } }
        public ushort[] CargoTrains             { get { return _CargoTrains.ToArray<ushort>(); } }
        public ushort[] TrainOther              { get { return _TrainOther.ToArray<ushort>(); } }

        public uint[]   Humans                  { get { return _Humans.ToArray<uint>(); } }
        public uint[]   HumansUpdated           { get { return _HumansUpdated.ToArray<uint>(); } }
        public uint[]   HumansRemoved           { get { return _HumansRemoved.ToArray<uint>(); } }

        public uint[]   Residents               { get { return _Residents.ToArray<uint>(); } }
        public uint[]   ServicePersons          { get { return _ServicePersons.ToArray<uint>(); } }
        public uint[]   Tourists                { get { return _Tourists.ToArray<uint>(); } }
        public uint[]   HumanOther              { get { return _HumanOther.ToArray<uint>(); } }

        public ushort[] Animals                 { get { return _Animals.ToArray<ushort>(); } }
        public ushort[] AnimalsUpdated          { get { return _AnimalsUpdated.ToArray<ushort>(); } }
        public ushort[] AnimalsRemoved          { get { return _AnimalsRemoved.ToArray<ushort>(); } }

        public ushort[] Birds                   { get { return _Birds.ToArray<ushort>(); } }
        public ushort[] Seagulls                { get { return _Seagulls.ToArray<ushort>(); } }

        public ushort[] Livestock               { get { return _Livestock.ToArray<ushort>(); } }
        public ushort[] Cows                    { get { return _Cows.ToArray<ushort>(); } }
        public ushort[] Pigs                    { get { return _Pigs.ToArray<ushort>(); } }

        public ushort[] Pets                    { get { return _Pets.ToArray<ushort>(); } }
        public ushort[] Dogs                    { get { return _Dogs.ToArray<ushort>(); } }

        public ushort[] Wildlife                { get { return _Wildlife.ToArray<ushort>(); } }
        public ushort[] Wolves                  { get { return _Wolves.ToArray<ushort>(); } }
        public ushort[] Bears                   { get { return _Bears.ToArray<ushort>(); } }
        public ushort[] Moose                   { get { return _Moose.ToArray<ushort>(); } }

        public ushort[] AnimalOther             { get { return _AnimalOther.ToArray<ushort>(); } }
    }
}