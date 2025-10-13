using HarmonyLib;
using Helpers;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace UsefulSkips
{
    [HarmonyPatch(typeof(TutorialPhaseCampaignBehavior), "OnStoryModeTutorialEnded")]
    public class UsefulSkipsTutorialPhaseCampaignBehavior
    {
        private static bool Prefix()
        {
            if (UsefulSkipsSettings.Instance.ShouldSkipTutorial)
            {
                TextObject textObject = FactionHelper.GenerateClanNameforPlayer();

                // Ensure that the player's clan name matches the selected culture.
                Clan.PlayerClan.ChangeClanName(textObject, textObject);
                DisableHeroAction.Apply(StoryModeHeroes.ElderBrother);
                StoryModeHeroes.ElderBrother.Clan = null;
                PartyBase.MainParty.ItemRoster.Clear();
                PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
                Hero.MainHero.Heal(Hero.MainHero.MaxHitPoints, false);
                Hero.MainHero.Gold = 1000;

                return false;
            }

            return true;
        }
    }
}
