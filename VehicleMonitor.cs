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
    public class VehicleMonitor : ThreadingExtensionBase
    {
        private Settings _settings;
        private Helper _helper;
        private Data _data;

        private VehiclePrefabMapping _mapping;

        private bool _initialized;
        private bool _terminated;
        private bool _paused;
        private int _lastProcessedFrame;

        private VehicleManager _instance;
        private int _capacity;

        private Vehicle _vehicle;
        private ushort _id;
        private List<HashSet<ushort>> _categories;

        public override void OnCreated(IThreading threading)
        {
            _settings = Settings.Instance;
            _helper = Helper.Instance;

            _initialized = false;
            _terminated = false;

            base.OnCreated(threading);
        }

        public override void OnBeforeSimulationTick()
        {
            if (_terminated) return;

            if (!_helper.VehicleMonitorSpun)
            {
                _initialized = false;
                return;
            }

            base.OnBeforeSimulationTick();
        }

        public override void OnBeforeSimulationFrame()
        {
            base.OnBeforeSimulationFrame();
        }

        public override void OnAfterSimulationFrame()
        {
            _paused = false;

            base.OnAfterSimulationFrame();
        }

        public override void OnAfterSimulationTick()
        {
            base.OnAfterSimulationTick();
        }

        /*
         * Handles creation and removal of vehicles
         *
         * Note: Just because a vehicle has been removed visually, it does not mean
         * it is removed as far as the game is concerned. The vehicle is only truly removed
         * when the frame covers the vehicle's id, and that's when we will remove the
         * vehicle from our records.
         */
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (_terminated) return;

            if (!_helper.VehicleMonitorSpinnable) return;

            if (!_settings.Enable._VehicleMonitor) return;

            try
            {
                if (!_initialized)
                {
                    _data = Data.Instance;

                    _mapping = new VehiclePrefabMapping();

                    _paused = false;

                    _instance = Singleton<VehicleManager>.instance;
                    _capacity = _instance.m_vehicles.m_buffer.Length;

                    _id = (ushort)_capacity;

                    _initialized = true;
                    _helper.VehicleMonitorSpun = true;
                    _helper.VehicleMonitor = this;

                    _helper.Log("Vehicle monitor initialized");
                }
                else if (!SimulationManager.instance.SimulationPaused)
                {
                    _data._VehiclesUpdated.Clear();
                    _data._VehiclesRemoved.Clear();

                    int end = GetFrame();

                    while (_lastProcessedFrame != end)
                    {
                        _lastProcessedFrame = GetFrame(_lastProcessedFrame + 1);

                        int[] boundaries = GetFrameBoundaries();
                        ushort id;

                        for (int i = boundaries[0]; i <= boundaries[1]; i++)
                        {
                            id = (ushort)i;

                            if (UpdateVehicle(id))
                                _data._VehiclesUpdated.Add(id);
                            else if (_data._Vehicles.Contains(id))
                            {
                                _data._VehiclesRemoved.Add(id);
                                RemoveVehicle(id);
                            }
                        }
                    }
                }

                OutputDebugLog();
            }
            catch (Exception e)
            {
                string error = "Vehicle monitor failed to initialize\r\n";
                error += String.Format("Error: {0}\r\n", e.Message);
                error += "\r\n";
                error += "==== STACK TRACE ====\r\n";
                error += e.StackTrace;

                _helper.Log(error);

                _terminated = true;
            }

            base.OnUpdate(realTimeDelta, simulationTimeDelta);
        }

        public override void OnReleased()
        {
            _initialized = false;
            _terminated = false;
            _paused = false;

            _helper.VehicleMonitorSpun = false;
            _helper.VehicleMonitor = null;

            if (_data != null)
            {
                _data._Vehicles.Clear();

                _data._Cars.Clear();
                _data._Trains.Clear();
                _data._Aircrafts.Clear();
                _data._Ships.Clear();
                _data._VehicleOther.Clear();

                _data._Hearses.Clear();
                _data._GarbageTrucks.Clear();
                _data._FireTrucks.Clear();
                _data._PoliceCars.Clear();
                _data._Ambulances.Clear();
                _data._Buses.Clear();
                _data._CarOther.Clear();
            }

            base.OnReleased();
        }

        public int GetFrameFromId(ushort id)
        {
            return id >> 6 & 15;
        }

        private int GetFrame()
        {
            return GetFrame((int)Singleton<SimulationManager>.instance.m_currentFrameIndex);
        }

        private int GetFrame(int index)
        {
            return (int)(index & 15);
        }

        public static int[] GetFrameBoundaries()
        {
            return GetFrameBoundaries((int)Singleton<SimulationManager>.instance.m_currentFrameIndex);
        }

        private static int[] GetFrameBoundaries(int index)
        {
            int frame = (int)(index & 15);
            int frame_first = frame * 1024;
            int frame_last = (frame + 1) * 1024 - 1;

            return new int[2] { frame_first, frame_last };
        }

        private bool GetVehicle()
        {
            _vehicle = _instance.m_vehicles.m_buffer[(int)_id];

            if (_vehicle.Info == null)
                return false;

            if ((_vehicle.m_flags & Vehicle.Flags.Spawned) == Vehicle.Flags.None)
                return false;

            return true;
        }

        private bool UpdateVehicle(ushort id)
        {
            _id = id;

            if (!GetVehicle())
                return false;

            _categories = _mapping.GetMapping(_vehicle.Info);

            if (_categories.Count == 0)
                return false;

            foreach (HashSet<ushort> category in _mapping.GetMapping(_vehicle.Info))
                category.Add(_id);

            return true;
        }

        internal void RequestRemoval(ushort id)
        {
            Vehicle vehicle = _instance.m_vehicles.m_buffer[(int)id];

            bool isCreated = (vehicle.m_flags & Vehicle.Flags.Spawned) == Vehicle.Flags.None;

            if (!isCreated)
                RemoveVehicle(id);
        }

        private void RemoveVehicle(ushort id)
        {
            _data._Vehicles.Remove(id);

            _data._Cars.Remove(id);
            _data._Trains.Remove(id);
            _data._Aircrafts.Remove(id);
            _data._Ships.Remove(id);
            _data._VehicleOther.Remove(id);

            _data._Hearses.Remove(id);
            _data._GarbageTrucks.Remove(id);
            _data._FireTrucks.Remove(id);
            _data._PoliceCars.Remove(id);
            _data._Ambulances.Remove(id);
            _data._Buses.Remove(id);
            _data._CarOther.Remove(id);
        }

        private void OutputDebugLog()
        {
            if (!_helper.VehicleMonitorSpun) return;

            if (!_settings.Debug._VehicleMonitor) return;

            if (!_settings.Enable._VehicleMonitor) return;

            if (!_initialized) return;

            if (!SimulationManager.instance.SimulationPaused) return;

            if (_paused) return;

            string log = "\r\n";
            log += "==== VEHICLES ====\r\n";
            log += "\r\n";
            log += String.Format("{0}   Total\r\n", _data._Vehicles.Count);
            log += String.Format("{0}   Updated\r\n", _data._VehiclesUpdated.Count);
            log += String.Format("{0}   Removed\r\n", _data._VehiclesRemoved.Count);
            log += "\r\n";
            log += String.Format("{0}   CarAI\r\n", _data._Cars.Count);
            log += String.Format(" =>   {0}   HearseAI\r\n", _data._Hearses.Count);
            log += String.Format(" =>   {0}   GarbageTruckAI\r\n", _data._GarbageTrucks.Count);
            log += String.Format(" =>   {0}   FireTruckAI\r\n", _data._FireTrucks.Count);
            log += String.Format(" =>   {0}   PoliceCarAI\r\n", _data._PoliceCars.Count);
            log += String.Format(" =>   {0}   AmbulanceAI\r\n", _data._Ambulances.Count);
            log += String.Format(" =>   {0}   BusAI\r\n", _data._Buses.Count);
            log += String.Format(" =>   {0}   Other\r\n", _data._CarOther.Count);
            log += "\r\n";
            log += String.Format("{0}   TrainAI\r\n", _data._Trains.Count);
            log += String.Format("{0}   AircraftAI\r\n", _data._Aircrafts.Count);
            log += String.Format("{0}   ShipAI\r\n", _data._Ships.Count);
            log += String.Format("{0}   Other\r\n", _data._VehicleOther.Count);
            log += "\r\n";

            _helper.Log(log);

            _paused = true;
        }
    }
}