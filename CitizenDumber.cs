using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ColossalFramework.UI;
using Epic.OnlineServices;
using ICities;
using UnityEngine;

namespace CitizenDumber
{
    public class CitizenDumber : IUserMod
    {
        public string Name
        {
            get { return "Citizen Dumber"; }
        }

        public string Description
        { 
            get { return "Cheange education level of citizens"; }
        }
        private static Settings settings() { return SettingsDataManager.settings; }

        public void OnSettingsUI(UIHelperBase helper)
        {

            const float seconds_in_minute = 60.0f;

            this.add_education_slider(helper,
                "Percent of citizens with Basic Education - Primary School [%]",
                settings().basic_education,
                x => { settings().basic_education = (int)x; }
            );

            this.add_education_slider(helper,
                "Percent of citizens with High Education - High School [%]",
                settings().high_education,
                x => { settings().high_education = (int)x; }
            );

            this.add_education_slider(helper,
                "Percent of citizens with Highest Education - University [%]",
                settings().highest_education,
                x => { settings().highest_education = (int)x; }
            );

            helper.AddButton("Apply!", CitizenUpdater.set_education);
            add_spacer(helper);

            helper.AddCheckbox("Keep updating to selected values", settings().update, x => {settings().update = x;});
            add_spacer(helper);

            helper.AddSlider("Update interval (range: 1-60; default: 1) [minutes]", 
                1.0f, // min
                60.0f, // max
                1.0f, // step
                (float)(settings().refresh_period_seconds / seconds_in_minute), // default
                x => { settings().refresh_period_seconds = ((double)x) * seconds_in_minute; }
            );
        }

        

        private void add_education_slider(UIHelperBase helper, string description, int default_value, OnValueChanged valueChanged)
        {
            helper.AddSlider(
                description,
                0.0f,  // min
                100.0f, // max
                1.0f, // step
                (float)default_value,
                valueChanged
            );
            add_spacer(helper);
        }
        
        private void add_spacer(UIHelperBase helper) { helper.AddSpace(50); }
    }
}
