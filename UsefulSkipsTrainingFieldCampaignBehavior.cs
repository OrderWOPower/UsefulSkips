using HarmonyLib;
using StoryMode;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace UsefulSkips
{
    [HarmonyPatch(typeof(TrainingFieldCampaignBehavior), "OnCharacterCreationIsOver")]
    public class UsefulSkipsTrainingFieldCampaignBehavior
    {
        private static void Prefix(ref bool ___SkipTutorialMission)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipTutorial)
            {
                // Skip the tutorial.
                ___SkipTutorialMission = true;

                MobileParty.MainParty.Position = Settlement.Find("tutorial_training_field").Position;
                ((MapState)GameStateManager.Current.ActiveState)?.Handler.TeleportCameraToMainParty();
                TutorialPhase.Instance.PlayerTalkedWithBrotherForTheFirstTime();
                StoryModeManager.Current.MainStoryLine.CompleteTutorialPhase(false);
            }
        }
    }
}
