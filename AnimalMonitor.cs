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
    public class AnimalMonitor : ThreadingExtensionBase
    {
        private Settings _settings;
        private Helper _helper;
        private Data _data;

        private Dictionary<ushort, HashSet<ushort>> _buildingsAnimals;

        private bool _initialized;
        private bool _terminated;
        private bool _paused;

        private CitizenManager _instance;
        private int _capacity;

        private CitizenInstance _animal;
        private ushort _id;
        private HashSet<Type> _types;
        private Dictionary<string, int> _prefabs;

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

            if (!_helper.AnimalMonitorSpun)
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
         * Handles creation and removal of animals
         *
         * Note: Just because a animal has been removed visually, it does not mean
         * it is removed as far as the game is concerned. The animal is only truly removed
         * when the frame covers the animal's id, and that's when we will remove the
         * animal from our records.
         */
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (_terminated) return;

            if (!_helper.AnimalMonitorSpinnable) return;

            if (!_settings.Enable._AnimalMonitor) return;

            try
            {
                if (!_initialized)
                {
                    _data = Data.Instance;

                    _buildingsAnimals = new Dictionary<ushort, HashSet<ushort>>();

                    _paused = false;

                    _instance = Singleton<CitizenManager>.instance;
                    _capacity = _instance.m_instances.m_buffer.Length;

                    _id = (ushort)_capacity;
                    _types = new HashSet<Type>();
                    _prefabs = new Dictionary<string, int>();

                    LoadPrefabs();

                    for (int i = 0; i < _capacity; i++)
                        UpdateAnimal((ushort)i);

                    _initialized = true;
                    _helper.AnimalMonitorSpun = true;
                    _helper.AnimalMonitor = this;

                    _helper.Log("Animal monitor initialized");
                }
                else if (_data.BuildingsUpdated.Length > 0)
                {
                    _data._AnimalsUpdated.Clear();
                    _data._AnimalsRemoved.Clear();

                    foreach (ushort building in _data._BuildingsUpdated)
                    {
                        ushort id = Singleton<BuildingManager>.instance.m_buildings.m_buffer[building].m_targetCitizens;

                        while (id != 0)
                        {
                            if (UpdateAnimal(id))
                            {
                                _data._AnimalsUpdated.Add(id);
                                AddBuildingsAnimal(building, id);
                            }
                            else
                            {
                                _data._AnimalsRemoved.Add(id);

                                if (_data._Animals.Contains(id))
                                    RemoveAnimal(id);
                            }

                            id = _instance.m_instances.m_buffer[(int)id].m_nextTargetInstance;
                        }

                        CheckBuildingsAnimals(building);
                    }

                    foreach (ushort building in _data._BuildingsRemoved)
                        RemoveBuildingsAnimals(building);
                }

                OutputDebugLog();
            }
            catch (Exception e)
            {
                string error = "Animal monitor failed to initialize\r\n";
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
            _buildingsAnimals = new Dictionary<ushort, HashSet<ushort>>();

            _initialized = false;
            _terminated = false;
            _paused = false;

            _helper.AnimalMonitorSpun = false;
            _helper.AnimalMonitor = null;

            if (_data != null)
            {
                _data._Animals.Clear();

                _data._Birds.Clear();
                _data._Seagulls.Clear();

                _data._Livestocks.Clear();
                _data._Cows.Clear();
                _data._Pigs.Clear();

                _data._Pets.Clear();
                _data._Dogs.Clear();

                _data._Wildlife.Clear();
                _data._Wolves.Clear();
                _data._Bears.Clear();
                _data._Moose.Clear();

                _data._AnimalOther.Clear();
            }

            base.OnReleased();
        }

        private void LoadPrefabs()
        {
            for (uint i = 0; i < PrefabCollection<CitizenInfo>.PrefabCount(); i++)
            {
                CitizenInfo prefab = PrefabCollection<CitizenInfo>.GetPrefab(i);

                if (prefab == null)
                    continue;

                string name = prefab.GetLocalizedTitle();

                if (String.IsNullOrEmpty(name))
                    continue;

                if (!_settings.Animals.Contains(name))
                    continue;

                if (_prefabs.ContainsKey(name))
                    continue;

                _prefabs.Add(name, (int)i);
            }
        }

        private void AddBuildingsAnimal(ushort building, ushort animal)
        {
            if (!_buildingsAnimals.ContainsKey(building))
                _buildingsAnimals.Add(building, new HashSet<ushort>());

            _buildingsAnimals[building].Add(animal);
        }

        private void CheckBuildingsAnimals(ushort building)
        {
            if (!_buildingsAnimals.ContainsKey(building))
                return;

            HashSet<ushort> animals = _buildingsAnimals[building];

            if (animals.Count == 0)
            {
                _buildingsAnimals.Remove(building);
                return;
            }

            List<ushort> removals = new List<ushort>();

            foreach (ushort animal in animals)
            {
                if (_data._AnimalsUpdated.Contains(animal) || _data._AnimalsRemoved.Contains(animal))
                    continue;

                removals.Add(animal);
            }

            foreach (ushort animal in removals)
            {
                animals.Remove(animal);

                _data._AnimalsRemoved.Add(animal);
                RemoveAnimal(animal);
            }

            if (animals.Count == 0)
                _buildingsAnimals.Remove(building);
        }

        private void RemoveBuildingsAnimals(ushort building)
        {
            if (!_buildingsAnimals.ContainsKey(building))
                return;

            HashSet<ushort> animals = _buildingsAnimals[building];

            foreach (ushort animal in animals)
            {
                _data._AnimalsRemoved.Add(animal);
                RemoveAnimal(animal);
            }

            _buildingsAnimals.Remove(building);
        }

        private bool GetAnimal()
        {
            _animal = _instance.m_instances.m_buffer[(int)_id];

            if (_animal.Info == null)
                return false;

            if (!_animal.Info.m_citizenAI.IsAnimal())
                return false;

            if ((_animal.m_flags & CitizenInstance.Flags.Created) == CitizenInstance.Flags.None)
                return false;

            if (float.IsNegativeInfinity(_animal.Info.m_maxRenderDistance))
                return false;

            _types.Clear();
            _types.Add(_helper.AiType.CitizenAI);

            Type t = _animal.Info.m_citizenAI.GetType();

            while (!_types.Contains(t))
            {
                _types.Add(t);

                t = t.BaseType;
            }

            return true;
        }

        private bool UpdateAnimal(ushort id)
        {
            _id = id;

            if (!GetAnimal())
                return false;

            _data._Animals.Add(_id);

            if (_settings.Enable._Birds          && CheckBird())
            {
                if (CheckSeagull()) return true;

                return true;
            }
            if (_settings.Enable._Livestocks     && CheckLivestock())
            {
                if (CheckCow()) return true;
                if (CheckPig()) return true;

                return true;
            }
            if (_settings.Enable._Pets           && CheckPet())
            {
                if (CheckDog()) return true;

                return true;
            }
            if (_settings.Enable._Wildlife       && CheckWildlife())
            {
                if (CheckWolf()) return true;
                if (CheckBear()) return true;
                if (CheckMoose()) return true;

                return true;
            }
            if (_settings.Enable._AnimalOther	 && CheckAnimalOther())
            {
                return true;
            }

            return false;
        }

        internal void RequestRemoval(ushort id)
        {
            CitizenInstance animal = _instance.m_instances.m_buffer[(int)id];

            bool isCreated = (animal.m_flags & CitizenInstance.Flags.Created) != CitizenInstance.Flags.None;
            bool isAnimal = animal.Info != null && animal.Info.m_citizenAI.IsAnimal();
            bool isHidden = float.IsNegativeInfinity(animal.Info.m_maxRenderDistance);

            if (!isCreated || !isAnimal || isHidden)
                RemoveAnimal(id);
        }

        private void RemoveAnimal(ushort id)
        {
            _data._Animals.Remove(id);

            _data._Birds.Remove(id);
            _data._Seagulls.Remove(id);

            _data._Livestocks.Remove(id);
            _data._Cows.Remove(id);
            _data._Pigs.Remove(id);

            _data._Pets.Remove(id);
            _data._Dogs.Remove(id);

            _data._Wildlife.Remove(id);
            _data._Wolves.Remove(id);
            _data._Bears.Remove(id);
            _data._Moose.Remove(id);

            _data._AnimalOther.Remove(id);
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

        private bool Check(string name, HashSet<ushort> category)
        {
            if (!_prefabs.ContainsKey(name))
                return false;

            if (_animal.Info.m_prefabDataIndex == _prefabs[name])
            {
                category.Add(_id);
                return true;
            }
            else
                return false;
        }

        #region Animals

        private bool CheckAnimal()
        {
            return Check(_helper.AiType.AnimalAI, _data._Animals);
        }

        private bool CheckBird()
        {
            return Check(_helper.AiType.BirdAI, _data._Birds);
        }

        private bool CheckSeagull()
        {
            return Check("Seagull", _data._Seagulls);
        }

        private bool CheckLivestock()
        {
            return Check(_helper.AiType.LivestockAI, _data._Livestocks);
        }

        private bool CheckCow()
        {
            return Check("Cow", _data._Cows);
        }

        private bool CheckPig()
        {
            return Check("Pig", _data._Pigs);
        }

        private bool CheckPet()
        {
            return Check(_helper.AiType.PetAI, _data._Pets);
        }

        private bool CheckDog()
        {
            return Check("Dog", _data._Dogs);
        }

        private bool CheckWildlife()
        {
            return Check(_helper.AiType.WildlifeAI, _data._Wildlife);
        }

        private bool CheckWolf()
        {
            return Check("Wolf", _data._Wolves);
        }

        private bool CheckBear()
        {
            return Check("Bear", _data._Bears);
        }

        private bool CheckMoose()
        {
            return Check("Moose", _data._Moose);
        }

        private bool CheckAnimalOther()
        {
            _data._AnimalOther.Add(_id);
            return true;
        }

        #endregion

        private void OutputDebugLog()
        {
            if (!_helper.AnimalMonitorSpun) return;

            if (!_settings.Debug._AnimalMonitor) return;

            if (!_settings.Enable._AnimalMonitor) return;

            if (!_initialized) return;

            if (!SimulationManager.instance.SimulationPaused) return;

            if (_paused) return;

            string log = "\r\n";
            log += "==== ANIMALS ====\r\n";
            log += "\r\n";
            log += String.Format("{0}   Total\r\n", _data._Animals.Count);
            log += String.Format("{0}   Updated\r\n", _data._AnimalsUpdated.Count);
            log += String.Format("{0}   Removed\r\n", _data._AnimalsRemoved.Count);
            log += "\r\n";

            if (_settings.Enable._Birds)
            {
                log += String.Format("{0}   BirdAI\r\n", _data._Birds.Count);
                log += String.Format(" =>   {0}   Seagull(s)\r\n", _data._Seagulls.Count);
                log += "\r\n";
            }

            if (_settings.Enable._Livestocks)
            {
                log += String.Format("{0}   LivestockAI\r\n", _data._Livestocks.Count);
                log += String.Format(" =>   {0}   Cow(s)\r\n", _data._Cows.Count);
                log += String.Format(" =>   {0}   Pig(s)\r\n", _data._Pigs.Count);
                log += "\r\n";
            }

            if (_settings.Enable._Pets)
            {
                log += String.Format("{0}   PetAI\r\n", _data._Pets.Count);
                log += String.Format(" =>   {0}   Dog(s)\r\n", _data._Dogs.Count);
                log += "\r\n";
            }

            if (_settings.Enable._Wildlife)
            {
                log += String.Format("{0}   WildlifeAI\r\n", _data._Wildlife.Count);
                log += String.Format(" =>   {0}   Wolf(s)\r\n", _data._Wolves.Count);
                log += String.Format(" =>   {0}   Bear(s)\r\n", _data._Bears.Count);
                log += String.Format(" =>   {0}   Moose\r\n", _data._Moose.Count);
                log += "\r\n";
            }

            if (_settings.Enable._AnimalOther)
            {
                log += String.Format("{0}   Other\r\n", _data._AnimalOther.Count);
                log += "\r\n";
            }

            _helper.Log(log);

            _paused = true;
        }
    }
}