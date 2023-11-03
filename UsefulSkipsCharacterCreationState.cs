using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation;
using TaleWorlds.Core;

namespace UsefulSkips
{
    [HarmonyPatch]
    public class UsefulSkipsCharacterCreationState
    {
        private static CharacterCreationReviewStageVM _characterCreationReviewStageVM;

        [HarmonyPatch(typeof(CharacterCreationState), "NextStage")]
        public static void Prefix(CharacterCreationState __instance)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipCharacterCreation && __instance.GetIndexOfCurrentStage() == -1)
            {
                // Set a random culture.
                __instance.CurrentCharacterCreationContent.SetSelectedCulture(__instance.CurrentCharacterCreationContent.GetCultures().GetRandomElementInefficiently(), __instance.CharacterCreation);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CharacterCreationState), "NextStage")]
        public static void Postfix1(CharacterCreationState __instance)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipCharacterCreation && __instance.GetIndexOfCurrentStage() >= 0 && __instance.GetIndexOfCurrentStage() < __instance.GetTotalStagesCount())
            {
                CharacterCreationStageBase currentStage = __instance.CurrentStage;
                CharacterCreationContentBase currentCharacterCreationContent = __instance.CurrentCharacterCreationContent;
                Type characterCreationContent = currentCharacterCreationContent.GetType();

                if (currentStage is CharacterCreationGenericStage)
                {
                    // Ensure that the player's equipment matches the selected culture.
                    AccessTools.Method(characterCreationContent, "ApplyEquipments").Invoke(currentCharacterCreationContent, new object[] { __instance.CharacterCreation });
                    // Ensure that the player's siblings match the selected culture.
                    AccessTools.Method(characterCreationContent, "EscapeOnInit")?.Invoke(currentCharacterCreationContent, new object[] { __instance.CharacterCreation });
                }
                else if (currentStage is CharacterCreationReviewStage)
                {
                    // Set a random name.
                    _characterCreationReviewStageVM.ExecuteRandomizeName();
                }
                else if (currentStage is CharacterCreationOptionsStage)
                {
                    // Ensure that the player's family members are initialized correctly.
                    AccessTools.Method(characterCreationContent, "FinalizeParents").Invoke(currentCharacterCreationContent, null);
                    // Give the player 6 attribute points and 12 focus points.
                    Hero.MainHero.HeroDeveloper.UnspentAttributePoints = 6;
                    Hero.MainHero.HeroDeveloper.UnspentFocusPoints = 12;
                }

                // Skip character creation.
                __instance.NextStage();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CharacterCreationReviewStageVM), "AddReviewedItems")]
        public static void Postfix2(CharacterCreationReviewStageVM __instance) => _characterCreationReviewStageVM = __instance;
    }
}
