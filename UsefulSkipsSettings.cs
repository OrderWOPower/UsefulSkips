using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace UsefulSkips
{
    public class UsefulSkipsSettings : AttributeGlobalSettings<UsefulSkipsSettings>
    {
        public override string Id => "UsefulSkips";

        public override string DisplayName => "Useful Skips";

        public override string FolderName => "UsefulSkips";

        public override string FormatType => "json2";

        [SettingPropertyBool("Splash Screen", Order = 0, RequireRestart = false, HintText = "Skip the splash screen. Enabled by default.")]
        [SettingPropertyGroup("Skips", GroupOrder = 0)]
        public bool ShouldSkipSplashScreen { get; set; } = true;

        [SettingPropertyBool("Campaign Intro", Order = 1, RequireRestart = false, HintText = "Skip the campaign intro. Enabled by default.")]
        [SettingPropertyGroup("Skips", GroupOrder = 0)]
        public bool ShouldSkipCampaignIntro { get; set; } = true;

        [SettingPropertyBool("Character Creation", Order = 2, RequireRestart = false, HintText = "Skip character creation. Disabled by default.")]
        [SettingPropertyGroup("Skips", GroupOrder = 0)]
        public bool ShouldSkipCharacterCreation { get; set; } = false;

        [SettingPropertyBool("Tutorial", Order = 3, RequireRestart = false, HintText = "Skip the tutorial. Disabled by default.")]
        [SettingPropertyGroup("Skips", GroupOrder = 0)]
        public bool ShouldSkipTutorial { get; set; } = false;
    }
}
