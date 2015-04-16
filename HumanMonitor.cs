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
    public class HumanMonitor : ThreadingExtensionBase
    {
        private Settings _settings;
        private Helper _helper;
        private Data _data;

        private HumanPrefabMapping _mapping;

        private bool _initialized;
        private bool _terminated;
        private bool _paused;
        private int _lastProcessedFrame;

        private CitizenManager _instance;
        private int _capacity;

        private Citizen _human;
        private uint _id;
        private CitizenInfo _info;
        private List<HashSet<uint>> _categories;

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

            if (!_helper.HumanMonitorSpun)
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
         * Handles creation and removal of humans
         *
         * Note: Just because a human has been removed visually, it does not mean
         * it is removed as far as the game is concerned. The human is only truly removed
         * when the frame covers the human's id, and that's when we will remove the
         * human from our records.
         */
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (_terminated) return;

            if (!_helper.HumanMonitorSpinnable) return;

            if (!_settings.Enable._HumanMonitor) return;

            try
            {
                if (!_initialized)
                {
                    _data = Data.Instance;

                    _mapping = new HumanPrefabMapping();

                    _paused = false;

                    _instance = Singleton<CitizenManager>.instance;
                    _capacity = _instance.m_citizens.m_buffer.Length;

                    _id = (uint)_capacity;

                    for (int i = 0; i < _capacity; i++)
                        UpdateHuman((uint)i);

                    _lastProcessedFrame = GetFrame();

                    _initialized = true;
                    _helper.HumanMonitorSpun = true;
                    _helper.HumanMonitor = this;

                    _helper.Log("Human monitor initialized");
                }
                else if (!SimulationManager.instance.SimulationPaused)
                {
                    _data._HumansUpdated.Clear();
                    _data._HumansRemoved.Clear();

                    int end = GetFrame();

                    while (_lastProcessedFrame != end)
                    {
                        _lastProcessedFrame = GetFrame(_lastProcessedFrame + 1);

                        int[] boundaries = GetFrameBoundaries();
                        uint id;

                        for (int i = boundaries[0]; i <= boundaries[1]; i++)
                        {
                            id = (uint)i;

                            if (UpdateHuman(id))
                                _data._HumansUpdated.Add(id);
                            else if (_data._Humans.Contains(id))
                            {
                                _data._HumansRemoved.Add(id);
                                RemoveHuman(id);
                            }
                        }
                    }
                }

                OutputDebugLog();
            }
            catch (Exception e)
            {
                string error = "Human monitor failed to initialize\r\n";
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

            _helper.HumanMonitorSpun = false;
            _helper.HumanMonitor = null;

            if (_data != null)
            {
                _data._Humans.Clear();

                _data._Residents.Clear();
                _data._ServicePersons.Clear();
                _data._Tourists.Clear();
                _data._HumanOther.Clear();
            }

            base.OnReleased();
        }

        private static uint GetFrameFromId(uint id)
        {
            return id >> 8 & 4095;
        }

        private int GetFrame()
        {
            return GetFrame((int)Singleton<SimulationManager>.instance.m_currentFrameIndex);
        }

        private int GetFrame(int index)
        {
            return (int)(index & 4095);
        }

        public static int[] GetFrameBoundaries()
        {
            return GetFrameBoundaries((int)Singleton<SimulationManager>.instance.m_currentFrameIndex);
        }

        private static int[] GetFrameBoundaries(int index)
        {
            int frame = (int)(index & 4095);
            int frame_first = frame * 256;
            int frame_last = (frame + 1) * 256 - 1;

            return new int[2] { frame_first, frame_last };
        }

        private bool GetHuman()
        {
            _human = _instance.m_citizens.m_buffer[(int)_id];

            if (_human.Dead)
                return false;

            if ((_human.m_flags & Citizen.Flags.Created) == Citizen.Flags.None)
                return false;

            _info = _human.GetCitizenInfo(_id);

            if (_info == null)
                return false;

            if (_info.m_citizenAI.IsAnimal())
                return false;

            return true;
        }

        private bool UpdateHuman(uint id)
        {
            _id = id;

            if (!GetHuman())
                return false;

            _categories = _mapping.GetMapping(_info);

            if (_categories.Count == 0)
                return false;

            foreach (HashSet<uint> category in _categories)
                category.Add(_id);

            return true;
        }

        internal void RequestRemoval(uint id)
        {
            _id = id;

            if (!GetHuman())
                RemoveHuman(id);
        }

        private void RemoveHuman(uint id)
        {
            _data._Humans.Remove(id);

            _data._Residents.Remove(id);
            _data._ServicePersons.Remove(id);
            _data._Tourists.Remove(id);
            _data._HumanOther.Remove(id);
        }

        private void OutputDebugLog()
        {
            if (!_helper.HumanMonitorSpun) return;

            if (!_settings.Debug._HumanMonitor) return;

            if (!_settings.Enable._HumanMonitor) return;

            if (!_initialized) return;

            if (!SimulationManager.instance.SimulationPaused) return;

            if (_paused) return;

            string log = "\r\n";
            log += "==== HUMANS ====\r\n";
            log += "\r\n";
            log += String.Format("{0}   Total\r\n", _data._Humans.Count);
            log += String.Format("{0}   Updated\r\n", _data._HumansUpdated.Count);
            log += String.Format("{0}   Removed\r\n", _data._HumansRemoved.Count);
            log += "\r\n";
            log += String.Format("{0}   Resident(s)\r\n", _data._Residents.Count);
            log += String.Format("{0}   ServicePerson(s)\r\n", _data._ServicePersons.Count);
            log += String.Format("{0}   Tourist(s)\r\n", _data._Tourists.Count);
            log += String.Format("{0}   Other\r\n", _data._HumanOther.Count);
            log += "\r\n";

            _helper.Log(log);

            _paused = true;
        }
    }
}