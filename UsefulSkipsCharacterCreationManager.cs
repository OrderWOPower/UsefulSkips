using HarmonyLib;
using Helpers;
using StoryMode;
using StoryMode.GameComponents.CampaignBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace UsefulSkips
{
    [HarmonyPatch(typeof(CharacterCreationManager), "NextStage")]
    public class UsefulSkipsCharacterCreationManager
    {
        public static void Prefix(CharacterCreationManager __instance)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipCharacterCreation && __instance.GetIndexOfCurrentStage() == -1)
            {
                CharacterCreationContent characterCreationContent = __instance.CharacterCreationContent;

                // Give the player a random culture and name.
                characterCreationContent.SetSelectedCulture(characterCreationContent.GetCultures().GetRandomElementInefficiently(), __instance);
                characterCreationContent.SetMainCharacterName(NameGenerator.Current.GenerateFirstNameForPlayer(characterCreationContent.SelectedCulture, Hero.MainHero.IsFemale).ToString());
            }
        }

        public static void Postfix(CharacterCreationManager __instance)
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipCharacterCreation && __instance.GetIndexOfCurrentStage() >= 0 && __instance.GetIndexOfCurrentStage() < __instance.GetTotalStagesCount())
            {
                if (__instance.CurrentStage is CharacterCreationNarrativeStage)
                {
                    string playerEquipmentId = (string)AccessTools.Method(typeof(CharacterCreationCampaignBehavior), "GetPlayerEquipmentId").Invoke(Campaign.Current.GetCampaignBehavior<CharacterCreationCampaignBehavior>(), new object[] { __instance, __instance.CharacterCreationContent.SelectedTitleType, __instance.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale });
                    TextObject textObject = FactionHelper.GenerateClanNameforPlayer();

                    // Ensure that the player's equipment matches the selected culture.
                    CharacterObject.PlayerCharacter.Equipment.FillFrom(MBObjectManager.Instance.GetObject<MBEquipmentRoster>(playerEquipmentId).DefaultEquipment, true);
                    CharacterObject.PlayerCharacter.FirstCivilianEquipment.FillFrom(MBObjectManager.Instance.GetObject<MBEquipmentRoster>(playerEquipmentId).GetRandomCivilianEquipment(), true);

                    if (Game.Current.GameType is CampaignStoryMode)
                    {
                        // Ensure that the player's family members match the selected culture.
                        AccessTools.Method(typeof(StoryModeCharacterCreationCampaignBehavior), "FinalizeParentsAndLittleSiblings").Invoke(Campaign.Current.GetCampaignBehavior<StoryModeCharacterCreationCampaignBehavior>(), new object[] { __instance });
                    }

                    // Ensure that the player's clan name matches the selected culture.
                    Clan.PlayerClan.ChangeClanName(textObject, textObject);
                    // Give the player 12 focus points and 6 attribute points.
                    Hero.MainHero.HeroDeveloper.UnspentFocusPoints += 12;
                    Hero.MainHero.HeroDeveloper.UnspentAttributePoints += 6;
                }

                // Skip character creation.
                __instance.NextStage();
            }
        }
    }
}
