using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;

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

        public override void SetDefaults()
        {
            ModEnabled = false;
        }

        public override void Apply()
        {
            base.Apply();
        }
    }
}