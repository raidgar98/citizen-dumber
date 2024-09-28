using ColossalFramework.UI;
using Epic.OnlineServices.Auth;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using UnityEngine;

namespace CitizenDumber
{
    public class CitizenUpdaterBackgroundWorker: ThreadingExtensionBase
    {
        private DateTime last_update { get; set; } = DateTime.Now;
        private static Settings settings() { return SettingsDataManager.settings; }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (!settings().update || (DateTime.Now - last_update).TotalSeconds < settings().refresh_period_seconds)
            {
                // Debug.Log($"Not updating: {(DateTime.Now - last_update).TotalSeconds} < {refresh_period_seconds}");
                return;
            }

            Debug.Log($"Updating: {(DateTime.Now - last_update).TotalSeconds} >= {settings().refresh_period_seconds}");
            CitizenUpdater.set_education(true);
            last_update = DateTime.Now;
        }

    }
}
