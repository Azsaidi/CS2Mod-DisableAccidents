using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;

namespace DisableAccidents.ModSettings
{
    [FileLocation(nameof(DisableAccidents))]
    public class Setting : ModSetting
    {
        public static Setting Instance;

        public Setting(IMod mod) : base(mod)
        {
            Instance = this;
        }

        [SettingsUISection("Main")]
        public bool ModEnabled { get; set; } = false;

        [SettingsUISlider(min = 0, max = 100, step = 10, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection("Main")]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsSliderDisabled))]
        public int AccidentProbability { get; set; } = 100;

        [SettingsUISection("Version")]
        public string VersionDisplay => $" {ModAssemblyInfo.Version}";

        public bool IsSliderDisabled => ModEnabled;

        public override void SetDefaults()
        {
            ModEnabled = false;
            AccidentProbability = 100;
        }

        public override void Apply()
        {
            base.Apply();
        }
    }
}