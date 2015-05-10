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
    public class BuildingMonitor : ThreadingExtensionBase
    {
        private Settings _settings;
        private Helper _helper;
        private Data _data;

        private BuildingPrefabMapping _mapping;

        private bool _initialized;
        private bool _terminated;
        private bool _paused;
        private int _lastProcessedFrame;

        private BuildingManager _instance;
        private int _capacity;

        private Building _building;
        private ushort _id;
        private List<HashSet<ushort>> _categories;

        private HashSet<ushort> _added;
        private HashSet<ushort> _removed;

        public override void OnCreated(IThreading threading)
        {
            _settings = Settings.Instance;
            _helper = Helper.Instance;

            _initialized = false;
            _terminated = false;

            _added = new HashSet<ushort>();
            _removed = new HashSet<ushort>();

            base.OnCreated(threading);
        }

        /*
         * Handles creation of new buildings and reallocation of existing buildings.
         *
         * Note: This needs to happen before simulation TICK; otherwise, we might miss the
         * building update tracking. The building update record gets cleared whether the
         * simulation is paused or not.
         */
        public override void OnBeforeSimulationTick()
        {
            if (_terminated) return;

            if (!_helper.BuildingMonitorSpun)
            {
                _initialized = false;
                return;
            }

            if (!_settings.Enable._BuildingMonitor) return;

            if (!_initialized) return;

            if (!_instance.m_buildingsUpdated) return;

            for (int i = 0; i < _instance.m_updatedBuildings.Length; i++)
            {
                ulong ub = _instance.m_updatedBuildings[i];

                if (ub != 0)
                {
                    for (int j = 0; j < 64; j++)
                    {
                        if ((ub & (ulong)1 << j) != 0)
                        {
                            ushort id = (ushort)(i << 6 | j);

                            if (ProcessBuilding(id))
                                _added.Add(id);
                            else
                                _removed.Add(id);
                        }
                    }
                }
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
         * Handles removal of buildings and status changes
         *
         * Note: Just because a building has been removed visually, it does not mean
         * it is removed as far as the game is concerned. The building is only truly removed
         * when the frame covers the building's id, and that's when we will remove the
         * building from our records.
         */
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (_terminated) return;

            if (!_helper.BuildingMonitorSpinnable) return;

            if (!_settings.Enable._BuildingMonitor) return;

            try
            {
                if (!_initialized)
                {
                    _data = Data.Instance;

                    _mapping = new BuildingPrefabMapping();

                    _paused = false;

                    _instance = Singleton<BuildingManager>.instance;
                    _capacity = _instance.m_buildings.m_buffer.Length;

                    _id = (ushort)_capacity;

                    _added.Clear();
                    _removed.Clear();

                    for (ushort i = 0; i < _capacity; i++)
                    {
                        if (ProcessBuilding(i))
                            UpdateBuilding();
                    }

                    _lastProcessedFrame = GetFrame();

                    _initialized = true;
                    _helper.BuildingMonitorSpun = true;
                    _helper.BuildingMonitor = this;

                    _helper.NotifyPlayer("Building monitor initialized");
                }
                else if (!SimulationManager.instance.SimulationPaused)
                {
                    _data._BuildingsAdded.Clear();
                    _data._BuildingsUpdated.Clear();
                    _data._BuildingsRemoved.Clear();

                    foreach (ushort i in _added)
                        _data._BuildingsAdded.Add(i);

                    _added.Clear();

                    foreach (ushort i in _removed)
                        _data._BuildingsRemoved.Add(i);

                    _removed.Clear();

                    int end = GetFrame();

                    while (_lastProcessedFrame != end)
                    {
                        _lastProcessedFrame = GetFrame(_lastProcessedFrame + 1);

                        int[] boundaries = GetFrameBoundaries(_lastProcessedFrame);
                        ushort id;

                        for (int i = boundaries[0]; i <= boundaries[1]; i++)
                        {
                            id = (ushort)i;

                            if (UpdateBuilding(id))
                                _data._BuildingsUpdated.Add(id);
                            else if (_data._Buildings.Contains(id))
                            {
                                _data._BuildingsRemoved.Add(id);
                                RemoveBuilding(id);
                            }
                        }
                    }
                }

                OutputDebugLog();
            }
            catch (Exception e)
            {
                string error = "Building monitor failed to initialize\r\n";
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

            _helper.BuildingMonitorSpun = false;
            _helper.BuildingMonitor = null;

            if (_data != null)
            {
                _data._Buildings.Clear();

                _data._PlayerBuildings.Clear();
                _data._Cemeteries.Clear();
                _data._LandfillSites.Clear();
                _data._FireStations.Clear();
                _data._PoliceStations.Clear();
                _data._Hospitals.Clear();
                _data._Parks.Clear();
                _data._PowerPlants.Clear();
                _data._PlayerOther.Clear();

                _data._PrivateBuildings.Clear();
                _data._ResidentialBuildings.Clear();
                _data._CommercialBuildings.Clear();
                _data._IndustrialBuildings.Clear();
                _data._OfficeBuildings.Clear();
                _data._PrivateOther.Clear();

                _data._BuildingOther.Clear();

                _data._BuildingsAbandoned.Clear();
                _data._BuildingsBurnedDown.Clear();

                _data._BuildingsWithFire.Clear();
                _data._BuildingsWithCrime.Clear();
                _data._BuildingsWithSick.Clear();
                _data._BuildingsWithDead.Clear();
                _data._BuildingsWithGarbage.Clear();

                _data._BuildingsCapacityFull.Clear();
                _data._BuildingsCapacityStep1.Clear();
                _data._BuildingsCapacityStep2.Clear();
            }

            base.OnReleased();
        }

        public int GetFrameFromId(ushort id)
        {
            return id >> 7 & 255;
        }

        public int GetFrame()
        {
            return GetFrame((int)Singleton<SimulationManager>.instance.m_currentFrameIndex);
        }

        private int GetFrame(int index)
        {
            return (int)(index & 255);
        }

        private int[] GetFrameBoundaries()
        {
            return GetFrameBoundaries((int)Singleton<SimulationManager>.instance.m_currentFrameIndex);
        }

        private int[] GetFrameBoundaries(int index)
        {
            int frame = (int)(index & 255);
            int frame_first = frame * 128;
            int frame_last = (frame + 1) * 128 - 1;

            return new int[2] { frame_first, frame_last };
        }

        private bool GetBuilding()
        {
            _building = _instance.m_buildings.m_buffer[(int)_id];

            if (_building.Info == null)
                return false;

            if ((_building.m_flags & Building.Flags.Created) == Building.Flags.None)
                return false;

            return true;
        }

        private bool ProcessBuilding(ushort id)
        {
            if (_data._Buildings.Contains(id))
                RemoveBuilding(id);

            _id = id;

            if (!GetBuilding())
                return false;

            _categories = _mapping.GetMapping(_building.Info);

            if (_categories.Count == 0)
                return false;

            foreach (HashSet<ushort> category in _categories)
                category.Add(_id);

            return true;
        }

        private bool UpdateBuilding(ushort id)
        {
            _id = id;

            if (!GetBuilding())
                return false;

            return UpdateBuilding();
        }

        private bool UpdateBuilding()
        {
            if (CheckAbandoned())
            {
                _data._BuildingsWithDead.Remove(_id);
                _data._BuildingsWithGarbage.Remove(_id);
                _data._BuildingsWithFire.Remove(_id);
                _data._BuildingsWithCrime.Remove(_id);
                _data._BuildingsWithSick.Remove(_id);

                _data._BuildingsCapacityStep1.Remove(_id);
                _data._BuildingsCapacityStep2.Remove(_id);
                _data._BuildingsCapacityFull.Remove(_id);
            }
            else if (CheckBurnedDown())
            {
                _data._BuildingsWithDead.Remove(_id);
                _data._BuildingsWithGarbage.Remove(_id);
                _data._BuildingsWithFire.Remove(_id);
                _data._BuildingsWithCrime.Remove(_id);
                _data._BuildingsWithSick.Remove(_id);
                _data._BuildingsAbandoned.Remove(_id);

                _data._BuildingsCapacityStep1.Remove(_id);
                _data._BuildingsCapacityStep2.Remove(_id);
                _data._BuildingsCapacityFull.Remove(_id);
            }
            else
            {
                CheckDead();
                CheckGarbage();
                CheckFire();
                CheckCrime();
                CheckSick();

                CheckCapacityStep1();
                CheckCapacityStep2();
                CheckCapacityFull();
            }

            return true;
        }

        internal void RequestRemoval(ushort id)
        {
            _id = id;

            if (!GetBuilding())
                RemoveBuilding(id);
        }

        private void RemoveBuilding(ushort id)
        {
            _data._Buildings.Remove(id);

            _data._PlayerBuildings.Remove(id);
            _data._Cemeteries.Remove(id);
            _data._LandfillSites.Remove(id);
            _data._FireStations.Remove(id);
            _data._PoliceStations.Remove(id);
            _data._Hospitals.Remove(id);
            _data._Parks.Remove(id);
            _data._PowerPlants.Remove(id);
            _data._PlayerOther.Remove(id);

            _data._PrivateBuildings.Remove(id);
            _data._ResidentialBuildings.Remove(id);
            _data._CommercialBuildings.Remove(id);
            _data._IndustrialBuildings.Remove(id);
            _data._OfficeBuildings.Remove(id);
            _data._PrivateOther.Remove(id);

            _data._BuildingOther.Remove(id);

            _data._BuildingsAbandoned.Remove(id);
            _data._BuildingsBurnedDown.Remove(id);

            _data._BuildingsWithDead.Remove(id);
            _data._BuildingsWithGarbage.Remove(id);
            _data._BuildingsWithFire.Remove(id);
            _data._BuildingsWithCrime.Remove(id);
            _data._BuildingsWithSick.Remove(id);

            _data._BuildingsCapacityStep1.Remove(id);
            _data._BuildingsCapacityStep2.Remove(id);
            _data._BuildingsCapacityFull.Remove(id);
        }

        private bool Check(Building.Flags problems, HashSet<ushort> category)
        {
            if ((_building.m_flags & problems) != Building.Flags.None)
            {
                category.Add(_id);
                return true;
            }
            else
            {
                category.Remove(_id);
                return false;
            }
        }

        private bool Check(Notification.Problem problems, HashSet<ushort> category)
        {
            if ((_building.m_problems & problems) != Notification.Problem.None)
            {
                category.Add(_id);
                return true;
            }
            else
            {
                category.Remove(_id);
                return false;
            }
        }

        private bool CheckDead()
        {
            if (_building.m_deathProblemTimer > 0)
            {
                _data._BuildingsWithDead.Add(_id);
                return true;
            }
            else
            {
                _data._BuildingsWithDead.Remove(_id);
                return false;
            }
        }

        private bool CheckAbandoned()
        {
            return Check(Building.Flags.Abandoned, _data._BuildingsAbandoned);
        }

        private bool CheckBurnedDown()
        {
            return Check(Building.Flags.BurnedDown, _data._BuildingsBurnedDown);
        }

        private bool CheckGarbage()
        {
            if (_building.Info.m_buildingAI.GetGarbageAmount(_id, ref _building) > 2500)
            {
                _data._BuildingsWithGarbage.Add(_id);
                return true;
            }
            else
            {
                _data._BuildingsWithGarbage.Remove(_id);
                return false;
            }
        }

        private bool CheckFire()
        {
            return Check(Notification.Problem.Fire, _data._BuildingsWithFire);
        }

        private bool CheckCrime()
        {
            return Check(Notification.Problem.Crime, _data._BuildingsWithCrime);
        }

        private bool CheckSick()
        {
            return Check(Notification.Problem.DirtyWater | Notification.Problem.Pollution | Notification.Problem.Noise, _data._BuildingsWithSick);
        }

        private bool CheckCapacityStep1()
        {
            return Check(Building.Flags.CapacityStep1, _data._BuildingsCapacityStep1);
        }

        private bool CheckCapacityStep2()
        {
            return Check(Building.Flags.CapacityStep2, _data._BuildingsCapacityStep2);
        }

        private bool CheckCapacityFull()
        {
            return Check(Building.Flags.CapacityFull, _data._BuildingsCapacityFull);
        }

        private void OutputDebugLog()
        {
            if (!_helper.BuildingMonitorSpun) return;

            if (!_settings.Debug._BuildingMonitor) return;

            if (!_settings.Enable._BuildingMonitor) return;

            if (!_initialized) return;

            if (!SimulationManager.instance.SimulationPaused) return;

            if (_paused) return;

            string log = "\r\n";
            log += "==== BUILDINGS ====\r\n";
            log += "\r\n";
            log += String.Format("{0}   Total\r\n", _data._Buildings.Count);
            log += String.Format("{0}   Added\r\n", _data._BuildingsAdded.Count);
            log += String.Format("{0}   Updated\r\n", _data._BuildingsUpdated.Count);
            log += String.Format("{0}   Removed\r\n", _data._BuildingsRemoved.Count);
            log += "\r\n";
            log += String.Format("{0}   Player Building(s)\r\n", _data._PlayerBuildings.Count);
            log += String.Format(" =>   {0}   Cemetery(s)\r\n", _data._Cemeteries.Count);
            log += String.Format(" =>   {0}   LandfillSite(s)\r\n", _data._LandfillSites.Count);
            log += String.Format(" =>   {0}   FireStation(s)\r\n", _data._FireStations.Count);
            log += String.Format(" =>   {0}   PoliceStation(s)\r\n", _data._PoliceStations.Count);
            log += String.Format(" =>   {0}   Hospital(s)\r\n", _data._Hospitals.Count);
            log += String.Format(" =>   {0}   Park(s)\r\n", _data._Parks.Count);
            log += String.Format(" =>   {0}   PowerPlant(s)\r\n", _data._PowerPlants.Count);
            log += String.Format(" =>   {0}   Other\r\n", _data._PlayerOther.Count);
            log += "\r\n";
            log += String.Format("{0}   Private Building(s)\r\n", _data._PrivateBuildings.Count);
            log += String.Format(" =>   {0}   Residential\r\n", _data._ResidentialBuildings.Count);
            log += String.Format(" =>   {0}   Commercial\r\n", _data._CommercialBuildings.Count);
            log += String.Format(" =>   {0}   Industrial\r\n", _data._IndustrialBuildings.Count);
            log += String.Format(" =>   {0}   Office(s)\r\n", _data._OfficeBuildings.Count);
            log += String.Format(" =>   {0}   Other\r\n", _data._PrivateOther.Count);
            log += "\r\n";
            log += String.Format("{0}   Other Building(s)\r\n", _data._BuildingOther.Count);
            log += "\r\n";
            log += String.Format("{0}   Abandoned\r\n", _data._BuildingsAbandoned.Count);
            log += String.Format("{0}   BurnedDown\r\n", _data._BuildingsBurnedDown.Count);
            log += "\r\n";
            log += String.Format("{0}   w/Death\r\n", _data._BuildingsWithDead.Count);
            log += String.Format("{0}   w/Garbage\r\n", _data._BuildingsWithGarbage.Count);
            log += String.Format("{0}   w/Fire\r\n", _data._BuildingsWithFire.Count);
            log += String.Format("{0}   w/Crime\r\n", _data._BuildingsWithCrime.Count);
            log += String.Format("{0}   w/Illness\r\n", _data._BuildingsWithSick.Count);
            log += "\r\n";
            log += String.Format("{0}   CapacityStep1\r\n", _data._BuildingsCapacityStep1.Count);
            log += String.Format("{0}   CapacityStep2\r\n", _data._BuildingsCapacityStep2.Count);
            log += String.Format("{0}   CapacityFull\r\n", _data._BuildingsCapacityFull.Count);
            log += "\r\n";

            _helper.Log(log);

            _paused = true;
        }
    }
}