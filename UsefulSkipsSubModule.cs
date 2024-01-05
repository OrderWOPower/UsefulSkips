using HarmonyLib;
using SandBox;
using StoryMode;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using Module = TaleWorlds.MountAndBlade.Module;

namespace UsefulSkips
{
    // This mod adds options to skip the splash screen, campaign intro, character creation and tutorial.
    public class UsefulSkipsSubModule : MBSubModuleBase
    {
        private Harmony _harmony;

        protected override void OnSubModuleLoad()
        {
            _harmony = new Harmony("mod.bannerlord.usefulskips");
            _harmony.PatchAll();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipSplashScreen)
            {
                // Skip the splash screen.
                AccessTools.Field(typeof(Module), "_splashScreenPlayed").SetValue(Module.CurrentModule, true);
            }
        }

        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipCampaignIntro)
            {
                _harmony.Patch(AccessTools.Method(typeof(SandBoxGameManager), "OnLoadFinished"), transpiler: new HarmonyMethod(AccessTools.Method(typeof(UsefulSkipsGameManager), "Transpiler")));
                _harmony.Patch(AccessTools.Method(typeof(StoryModeGameManager), "OnLoadFinished"), transpiler: new HarmonyMethod(AccessTools.Method(typeof(UsefulSkipsGameManager), "Transpiler")));

                // Check whether BKCE is loaded.
                if (AccessTools.TypeByName("BKCEGameManager") != null)
                {
                    _harmony.Patch(AccessTools.Method(AccessTools.TypeByName("BKCEGameManager"), "OnLoadFinished"), transpiler: new HarmonyMethod(AccessTools.Method(typeof(UsefulSkipsGameManager), "Transpiler")));
                }
            }
        }

        public override void OnGameEnd(Game game)
        {
            _harmony.Unpatch(AccessTools.Method(typeof(SandBoxGameManager), "OnLoadFinished"), AccessTools.Method(typeof(UsefulSkipsGameManager), "Transpiler"));
            _harmony.Unpatch(AccessTools.Method(typeof(StoryModeGameManager), "OnLoadFinished"), AccessTools.Method(typeof(UsefulSkipsGameManager), "Transpiler"));

            if (AccessTools.TypeByName("BKCEGameManager") != null)
            {
                _harmony.Unpatch(AccessTools.Method(AccessTools.TypeByName("BKCEGameManager"), "OnLoadFinished"), AccessTools.Method(typeof(UsefulSkipsGameManager), "Transpiler"));
            }
        }
    }
}
