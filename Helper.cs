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
        }

        private static readonly Helper _Instance = new Helper();
        public static Helper Instance { get { return _Instance; } }

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

        internal BuildingMonitor BuildingMonitor;
        internal VehicleMonitor VehicleMonitor;
        internal AnimalMonitor AnimalMonitor;
        internal HumanMonitor HumanMonitor;

        public void RequestBuildingRemoval(ushort id)
        {
            if (BuildingMonitor != null)
                BuildingMonitor.RequestRemoval(id);
        }

        public void RequestVehicleRemoval(ushort id)
        {
            if (VehicleMonitor != null)
                VehicleMonitor.RequestRemoval(id);
        }

        public void RequestHumanRemoval(uint id)
        {
            if (HumanMonitor != null)
                HumanMonitor.RequestRemoval(id);
        }

        public void RequestAnimalRemoval(ushort id)
        {
            if (AnimalMonitor != null)
                AnimalMonitor.RequestRemoval(id);
        }

        internal void Log(string message)
        {
            Debug.Log(String.Format("{0}: {1}", Settings.Instance.Tag, message));
        }

        public void NotifyPlayer(string message)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("{0}: {1}", Settings.Instance.Tag, message));
            Log(message);
        }
    }
}