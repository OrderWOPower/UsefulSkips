using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace UsefulSkips
{
    [HarmonyPatch(typeof(GameStateManager), "CleanAndPushState")]
    public class UsefulSkipsGameStateManager
    {
        public static void Prefix(GameState gameState)
        {
            if (gameState is VideoPlaybackState videoPlaybackState && videoPlaybackState.VideoPath.Contains("campaign_intro") && UsefulSkipsSettings.Instance.ShouldSkipCampaignIntro)
            {
                AccessTools.Property(typeof(VideoPlaybackState), "AudioPath").SetValue(videoPlaybackState, null);
            }
        }

        public static void Postfix(GameState gameState)
        {
            if (gameState is VideoPlaybackState videoPlaybackState && videoPlaybackState.VideoPath.Contains("campaign_intro") && UsefulSkipsSettings.Instance.ShouldSkipCampaignIntro)
            {
                videoPlaybackState.OnVideoFinished();
            }
        }
    }
}
