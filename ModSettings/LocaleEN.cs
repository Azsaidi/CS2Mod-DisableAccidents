using Colossal;
using System.Collections.Generic;

namespace DisableAccidents.ModSettings
{
    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), ModAssemblyInfo.Title },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModEnabled)), "Disable All Accidents" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModEnabled)), "When enabled, completely prevents all accidents and disables the probability slider." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AccidentProbability)), "Accident Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AccidentProbability)), "Adjust base accident probability (0-100%). Only active when 'Disable All Accidents' is off." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.VersionDisplay)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.VersionDisplay)), "Current version number of the Disable Accidents mod. Useful for troubleshooting and reporting issues." },
            };
        }

        public void Unload()
        {
        }
    }
}