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

        private bool _initialized;
        private bool _terminated;
        private bool _paused;
        private int _lastProcessedFrame;

        private BuildingManager _instance;
        private int _capacity;

        private Building _building;
        private ushort _id;
        private HashSet<Type> _types;
        private bool _isPlayer;
        private bool _isPrivate;

        public override void OnCreated(IThreading threading)
        {
            _settings = Settings.Instance;
            _helper = Helper.Instance;

            _initialized = false;
            _terminated = false;

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
                            ProcessBuilding((ushort)(i << 6 | j));
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

                    _paused = false;

                    _instance = Singleton<BuildingManager>.instance;
                    _capacity = _instance.m_buildings.m_buffer.Length;

                    _id = (ushort)_capacity;
                    _types = new HashSet<Type>();

                    for (ushort i = 0; i < _capacity; i++)
                    {
                        if (ProcessBuilding(i))
                            UpdateBuilding();
                    }

                    _lastProcessedFrame = GetFrame();

                    _initialized = true;
                    _helper.BuildingMonitorSpun = true;

                    _helper.Log("Building monitor initialized");
                }
                else if (!SimulationManager.instance.SimulationPaused)
                {
                    _data._BuildingsUpdated.Clear();
                    _data._BuildingsRemoved.Clear();

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
                _data._PlayerOther.Clear();

                _data._PrivateBuildings.Clear();
                _data._ResidentialBuildings.Clear();
                _data._CommercialBuildings.Clear();
                _data._IndustrialBuildings.Clear();
                _data._OfficeBuildings.Clear();
                _data._PrivateOther.Clear();

                _data._BuildingsWithDead.Clear();
                _data._BuildingsWithGarbage.Clear();
                _data._BuildingsWithFire.Clear();
                _data._BuildingsWithCrime.Clear();
                _data._BuildingsWithSick.Clear();
                _data._BuildingsAbandoned.Clear();
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

            if ((_building.m_flags & Building.Flags.Created) == Building.Flags.None)
                return false;

            if (_building.Info == null)
                return false;

            _isPlayer = false;
            _isPrivate = false;

            _types.Clear();
            _types.Add(_helper.AiType.BuildingAI);

            Type t = _building.Info.m_buildingAI.GetType();

            while (!_types.Contains(t))
            {
                _types.Add(t);

                t = t.BaseType;
            }

            return true;
        }

        private bool ProcessBuilding(ushort id)
        {
            if (_data._Buildings.Contains(id))
                RemoveBuilding(id);

            _id = id;

            if (!GetBuilding() || !CheckBuilding())
                return false;

            if (_settings.Enable._PlayerBuildings            && _isPlayer)
            {
                if (_settings.Enable._Cemeteries             && CheckCemetery())
                    return true;
                if (_settings.Enable._LandfillSites          && CheckLandfillSite())
                    return true;
                if (_settings.Enable._FireStations           && CheckFireStation())
                    return true;
                if (_settings.Enable._PoliceStations         && CheckPoliceStation())
                    return true;
                if (_settings.Enable._Hospitals              && CheckHospital())
                    return true;
                if (_settings.Enable._Parks                  && CheckPark())
                    return true;
                if (_settings.Enable._PlayerOther            && CheckPlayerOther())
                    return true;
            }
            else if (_settings.Enable._PrivateBuildings      && _isPrivate)
            {
                if (_settings.Enable._ResidentialBuildings	 && CheckResidentialBuilding())
                    return true;
                if (_settings.Enable._CommercialBuildings    && CheckCommercialBuilding())
                    return true;
                if (_settings.Enable._IndustrialBuildings    && CheckIndustrialBuilding())
                    return true;
                if (_settings.Enable._OfficeBuildings        && CheckOfficeBuilding())
                    return true;
                if (_settings.Enable._PrivateOther           && CheckPrivateOther())
                    return true;
            }

            return false;
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
            if (!CheckAbandoned())
            {
                if (_settings.Enable._DeadStatus     && CheckDead())
                    return true;
                if (_settings.Enable._GarbageStatus	 && CheckGarbage())
                    return true;
                if (_settings.Enable._FireStatus     && CheckFire())
                    return true;
                if (_settings.Enable._CrimeStatus    && CheckCrime())
                    return true;
                if (_settings.Enable._SickStatus     && CheckSick())
                    return true;
            }

            return true;
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
            _data._PlayerOther.Remove(id);

            _data._PrivateBuildings.Remove(id);
            _data._ResidentialBuildings.Remove(id);
            _data._CommercialBuildings.Remove(id);
            _data._IndustrialBuildings.Remove(id);
            _data._OfficeBuildings.Remove(id);
            _data._PrivateOther.Remove(id);

            _data._BuildingsWithDead.Remove(id);
            _data._BuildingsWithGarbage.Remove(id);
            _data._BuildingsWithFire.Remove(id);
            _data._BuildingsWithCrime.Remove(id);
            _data._BuildingsWithSick.Remove(id);
            _data._BuildingsAbandoned.Remove(id);
        }

        private bool Check(Type aiType, HashSet<ushort> category)
        {
            if (_types.Contains(aiType))
            {
                category.Add(_id);
                return true;
            }
            else
                return false;
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

        private bool CheckBuilding()
        {
            _isPlayer = CheckPlayerBuilding();
            _isPrivate = CheckPrivateBuilding();

            if (_isPlayer || _isPrivate)
            {
                _data._Buildings.Add(_id);
                return true;
            }
            else
                return false;
        }

        #region Player buildings

        private bool CheckPlayerBuilding()
        {
            return Check(_helper.AiType.PlayerBuildingAI, _data._PlayerBuildings);
        }

        private bool CheckCemetery()
        {
            return Check(_helper.AiType.CemeteryAI, _data._Cemeteries);
        }

        private bool CheckLandfillSite()
        {
            return Check(_helper.AiType.LandfillSiteAI, _data._LandfillSites);
        }

        private bool CheckFireStation()
        {
            return Check(_helper.AiType.FireStationAI, _data._FireStations);
        }

        private bool CheckPoliceStation()
        {
            return Check(_helper.AiType.PoliceStationAI, _data._PoliceStations);
        }

        private bool CheckHospital()
        {
            return Check(_helper.AiType.HospitalAI, _data._Hospitals);
        }

        private bool CheckPark()
        {
            return Check(_helper.AiType.ParkAI, _data._Parks);
        }

        private bool CheckPlayerOther()
        {
            _data._PlayerOther.Add(_id);
            return true;
        }

        #endregion

        #region Private (NPC) buildings

        private bool CheckPrivateBuilding()
        {
            return Check(_helper.AiType.PrivateBuildingAI, _data._PrivateBuildings);
        }

        private bool CheckResidentialBuilding()
        {
            return Check(_helper.AiType.ResidentialBuildingAI, _data._ResidentialBuildings);
        }

        private bool CheckCommercialBuilding()
        {
            return Check(_helper.AiType.CommercialBuildingAI, _data._CommercialBuildings);
        }

        private bool CheckIndustrialBuilding()
        {
            return Check(_helper.AiType.IndustrialBuildingAI, _data._IndustrialBuildings);
        }

        private bool CheckOfficeBuilding()
        {
            return Check(_helper.AiType.OfficeBuildingAI, _data._OfficeBuildings);
        }

        private bool CheckPrivateOther()
        {
            _data._PrivateOther.Add(_id);
            return true;
        }

        #endregion

        #region Building status

        private bool CheckAbandoned()
        {
            return Check(Building.Flags.Abandoned, _data._BuildingsAbandoned);
        }

        private bool CheckDead()
        {
            return Check(Notification.Problem.Death, _data._BuildingsWithDead);
        }

        private bool CheckGarbage()
        {
            return Check(Notification.Problem.Garbage, _data._BuildingsWithGarbage);
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

        #endregion

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
            log += String.Format("{0}   Updated\r\n", _data._BuildingsUpdated.Count);
            log += String.Format("{0}   Removed\r\n", _data._BuildingsRemoved.Count);
            log += "\r\n";
            log += String.Format("{0}   PlayerBuildingAI\r\n", _data._PlayerBuildings.Count);
            log += String.Format(" =>   {0}   CemeteryAI\r\n", _data._Cemeteries.Count);
            log += String.Format(" =>   {0}   LandfillSiteAI\r\n", _data._LandfillSites.Count);
            log += String.Format(" =>   {0}   FireStationAI\r\n", _data._FireStations.Count);
            log += String.Format(" =>   {0}   PoliceStationAI\r\n", _data._PoliceStations.Count);
            log += String.Format(" =>   {0}   HospitalAI\r\n", _data._Hospitals.Count);
            log += String.Format(" =>   {0}   ParkAI\r\n", _data._Parks.Count);
            log += String.Format(" =>   {0}   Other\r\n", _data._PlayerOther.Count);
            log += "\r\n";
            log += String.Format("{0}   PrivateBuildingAI\r\n", _data._PrivateBuildings.Count);
            log += String.Format(" =>   {0}   ResidentialBuildingAI\r\n", _data._ResidentialBuildings.Count);
            log += String.Format(" =>   {0}   CommercialBuildingAI\r\n", _data._CommercialBuildings.Count);
            log += String.Format(" =>   {0}   IndustrialBuildingAI\r\n", _data._IndustrialBuildings.Count);
            log += String.Format(" =>   {0}   OfficeBuildingAI\r\n", _data._OfficeBuildings.Count);
            log += String.Format(" =>   {0}   Other\r\n", _data._PrivateOther.Count);
            log += "\r\n";
            log += String.Format("{0}   w/Death\r\n", _data._BuildingsWithDead.Count);
            log += String.Format("{0}   w/Garbage\r\n", _data._BuildingsWithGarbage.Count);
            log += String.Format("{0}   w/Fire\r\n", _data._BuildingsWithFire.Count);
            log += String.Format("{0}   w/Crime\r\n", _data._BuildingsWithCrime.Count);
            log += String.Format("{0}   w/Illness\r\n", _data._BuildingsWithSick.Count);
            log += String.Format("{0}   Abandoned\r\n", _data._BuildingsAbandoned.Count);
            log += "\r\n";

            _helper.Log(log);

            _paused = true;
        }
    }
}