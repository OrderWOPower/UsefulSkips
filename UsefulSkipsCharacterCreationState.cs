using HarmonyLib;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;

namespace UsefulSkips
{
    [HarmonyPatch(typeof(CharacterCreationState), "NextStage")]
    public class UsefulSkipsCharacterCreationState
    {
        // Set a random culture and skip character creation.
        public static void Postfix(CharacterCreationState __instance)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipCharacterCreation)
            {
                CharacterCreationContentBase characterCreationContent = __instance.CurrentCharacterCreationContent;
                characterCreationContent.SetSelectedCulture(characterCreationContent.GetCultures().GetRandomElementInefficiently(), __instance.CharacterCreation);
                __instance.FinalizeCharacterCreation();
            }
        }
    }
}
