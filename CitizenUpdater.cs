using System;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using Random = System.Random;

namespace CitizenDumber
{
    internal class CitizenUpdater
    {
        private const Citizen.Flags no_education_modifier = Citizen.Flags.All & (~Citizen.Flags.Education1) & (~Citizen.Flags.Education2) & (~Citizen.Flags.Education3);
        private const Citizen.Flags basic_education_modifier = no_education_modifier | Citizen.Flags.Education1;
        private const Citizen.Flags high_education_modifier = basic_education_modifier | Citizen.Flags.Education2;
        private const Citizen.Flags highest_education_modifier = high_education_modifier | Citizen.Flags.Education3;

        private static Settings settings() { return SettingsDataManager.settings; }

        public static void set_education() { set_education(true); }
        public static void set_education(bool update_age)
        {
            if (settings().basic_education >= 0)
            {
                if (settings().high_education >= 0)
                {
                    if (settings().high_education > settings().basic_education)
                    {
                        show_error_message_box("Citizens with higher education must be lesser or equal than citizens with basic education! No actions were taken.");
                        return;
                    }

                    if (settings().highest_education >= 0)
                    {
                        if (settings().highest_education > settings().high_education)
                        {
                            show_error_message_box("Citizen with highest education must be lesser or equal than citizens with high education! No actions were taken.");
                            return;
                        }
                    }
                }
            }

            int total_citizens_count = CitizenManager.instance.m_citizens.m_buffer.Length;
            int citizens_with_basic_education = count_citizens_for_education_level(settings().basic_education, total_citizens_count);
            int citizens_with_higher_education = count_citizens_for_education_level(settings().high_education, total_citizens_count);
            int citizens_with_highest_education = count_citizens_for_education_level(settings().highest_education, total_citizens_count);

            for (int i = 0; i < total_citizens_count; i++)
            {
                set_proper_flags(
                    ref CitizenManager.instance.m_citizens.m_buffer[i],
                    update_age,
                    citizens_with_basic_education-- > 0,
                    citizens_with_higher_education-- > 0,
                    citizens_with_highest_education-- > 0
                );
            }
        }

        private static void set_proper_flags(ref Citizen citizen, bool update_age, bool primary, bool higher, bool highest)
        {
            Random rng = new Random();
            if (!(primary && higher && highest))
            {
                citizen.m_flags &= no_education_modifier;
                if(update_age) citizen.Age = rng.Next(18, 65);
                return;
            }
            if (highest)
            {
                citizen.m_flags &= highest_education_modifier;
                if (update_age) citizen.Age = rng.Next(18, 30);
                return;
            }
            if (higher)
            {
                citizen.m_flags &= high_education_modifier;
                if (update_age) citizen.Age = rng.Next(12, 17);
                return;
            }

            citizen.m_flags &= basic_education_modifier;
            if (update_age) citizen.Age = rng.Next(3, 12);
        }

        private static void show_error_message_box(string message)
        {
            ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
            panel.SetMessage("Citizen Dumber", message, false);
        }

        private static int count_citizens_for_education_level(float level, int total_citizens)
        {
            if (level <= 0) return 0;
            return (int)(total_citizens * (level / 100.0));
        }
    }
}
