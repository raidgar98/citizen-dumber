using ColossalFramework.IO;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using Epic.OnlineServices.Auth;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using UnityEngine;

namespace CitizenDumber
{
    public class SettingsDataManager : SerializableDataExtensionBase
    {
        public static Settings settings { get; private set; } = new Settings();


        public override void OnLoadData()
        {
            byte[] bytes = serializableDataManager.LoadData(Settings.DataId);
            if (bytes != null)
            {
                using (var stream = new MemoryStream(bytes))
                {
                    settings = DataSerializer.Deserialize<Settings>(stream, DataSerializer.Mode.Memory);
                }
            }
        }

        public override void OnSaveData()
        {
            byte[] bytes;

            using (var stream = new MemoryStream())
            {
                DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, 0, settings);
                bytes = stream.ToArray();
            }

            serializableDataManager.SaveData(Settings.DataId, bytes);
        }
    }
}
