using HarmonyLib;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;

namespace UsefulSkips
{
    [HarmonyPatch(typeof(CharacterCreationState), "NextStage")]
    public class UsefulSkipsCharacterCreationState
    {
        public static void Postfix(CharacterCreationState __instance)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipCharacterCreation)
            {
                // Set a random culture and skip character creation.
                __instance.CurrentCharacterCreationContent.SetSelectedCulture(__instance.CurrentCharacterCreationContent.GetCultures().GetRandomElementInefficiently(), __instance.CharacterCreation);
                __instance.FinalizeCharacterCreation();
            }
        }
    }
}
