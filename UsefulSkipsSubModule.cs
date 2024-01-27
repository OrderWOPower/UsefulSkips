using HarmonyLib;
using TaleWorlds.MountAndBlade;
using Module = TaleWorlds.MountAndBlade.Module;

namespace UsefulSkips
{
    // This mod adds options to skip the splash screen, campaign intro, character creation and tutorial.
    public class UsefulSkipsSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad() => new Harmony("mod.bannerlord.usefulskips").PatchAll();

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipSplashScreen)
            {
                // Skip the splash screen.
                AccessTools.Field(typeof(Module), "_splashScreenPlayed").SetValue(Module.CurrentModule, true);
            }
        }
    }
}
