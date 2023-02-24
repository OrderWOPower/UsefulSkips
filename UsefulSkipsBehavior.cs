﻿using HarmonyLib;
using StoryMode;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace UsefulSkips
{
    public class UsefulSkipsBehavior
    {
        [HarmonyPatch(typeof(TrainingFieldCampaignBehavior), "OnCharacterCreationIsOver")]
        public class UsefulSkipsTrainingFieldBehavior
        {
            // Skip the tutorial.
            private static void Prefix(ref bool ___SkipTutorialMission)
            {
                if (UsefulSkipsSettings.Instance.ShouldSkipTutorial)
                {
                    ___SkipTutorialMission = true;
                    MobileParty.MainParty.Position2D = Settlement.Find("tutorial_training_field").Position2D;
                    ((MapState)GameStateManager.Current.ActiveState).Handler.TeleportCameraToMainParty();
                    TutorialPhase.Instance.PlayerTalkedWithBrotherForTheFirstTime();
                    StoryModeManager.Current.MainStoryLine.CompleteTutorialPhase(true);
                }
            }
        }

        [HarmonyPatch(typeof(TutorialPhaseCampaignBehavior), "OnStoryModeTutorialEnded")]
        public class UsefulSkipsTutorialPhaseBehavior
        {
            private static bool Prefix()
            {
                if (UsefulSkipsSettings.Instance.ShouldSkipTutorial)
                {
                    DisableHeroAction.Apply(StoryModeHeroes.ElderBrother);
                    StoryModeHeroes.ElderBrother.Clan = CampaignData.NeutralFaction;
                    PartyBase.MainParty.ItemRoster.Clear();
                    PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
                    Hero.MainHero.Gold = 1000;
                    return false;
                }
                return true;
            }
        }
    }
}
