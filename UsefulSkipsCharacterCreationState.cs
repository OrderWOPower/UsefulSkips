using HarmonyLib;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;

namespace UsefulSkips
{
    [HarmonyPatch(typeof(CharacterCreationState), "NextStage")]
    public class UsefulSkipsCharacterCreationState
    {
        public static void Prefix(CharacterCreationState __instance)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipCharacterCreation)
            {
                // Set a random culture.
                __instance.CurrentCharacterCreationContent.SetSelectedCulture(__instance.CurrentCharacterCreationContent.GetCultures().GetRandomElementInefficiently(), __instance.CharacterCreation);
            }
        }

        public static void Postfix(CharacterCreationState __instance)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipCharacterCreation)
            {
                // Skip character creation.
                __instance.FinalizeCharacterCreation();
            }
        }
    }
}
