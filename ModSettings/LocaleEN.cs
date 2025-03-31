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
                { m_Setting.GetSettingsLocaleID(), "Disable Accidents" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModEnabled)), "Toggle Accident Prevention" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModEnabled)), "When the toggle is enabled, all accidents are prevented." }
            };
        }

        public void Unload()
        {
        }
    }
}