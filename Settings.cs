using ColossalFramework.IO;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InvalidDataException = System.IO.InvalidDataException;

namespace CitizenDumber
{
    public class Settings : ColossalFramework.IO.IDataContainer
    {
        // The key for our data in the savegame
        public const string DataId = "CitizenDumberSettings";

        public int basic_education { get; set; } = 0;
        public int high_education { get; set; } = 0;
        public int highest_education { get; set; } = 0;
        public bool update { get; set; } = true;
        public double refresh_period_seconds { get; set; } = 60.0;

        public void Serialize(DataSerializer s)
        {
            s.WriteInt32(basic_education);
            s.WriteInt32(high_education);
            s.WriteInt32(highest_education);
            s.WriteBool(update);
            s.WriteDouble(refresh_period_seconds);
        }

        public void Deserialize(DataSerializer s)
        {
            basic_education = s.ReadInt32();
            high_education = s.ReadInt32();
            highest_education = s.ReadInt32();
            update = s.ReadBool();
            refresh_period_seconds = s.ReadDouble();
        }

        public void AfterDeserialize(DataSerializer s)
        {
            validate_education(basic_education, "basic_education");
            validate_education(high_education, "high_education");
            validate_education(highest_education, "highest_education");

            if (refresh_period_seconds < 1 || refresh_period_seconds > 3600) 
                throw new InvalidDataException("refresh_period_seconds have to be in range <1 ; 3600>");
        }

        private void validate_education(int value, string field_name)
        {
            if (value < 0 || value > 100) throw new InvalidDataException($"{field_name} have to be in range <0 ; 100>");
        }
    }
}
