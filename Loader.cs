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
    public class Loader : LoadingExtensionBase
    {
        Helper _helper;

        public override void OnCreated(ILoading loading)
        {
            _helper = Helper.Instance;

            _helper._GameLoaded = loading.loadingComplete;
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame)
                _helper._GameLoaded = true;
        }

        public override void OnLevelUnloading()
        {
            _helper._GameLoaded = false;
        }
    }
}