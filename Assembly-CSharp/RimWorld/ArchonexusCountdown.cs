using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A9A RID: 6810
	public static class ArchonexusCountdown
	{
		// Token: 0x17001E8C RID: 7820
		// (get) Token: 0x0600ABC1 RID: 43969 RVA: 0x003CA2A6 File Offset: 0x003C84A6
		public static bool CountdownActivated
		{
			get
			{
				return ArchonexusCountdown.timeLeft > 0f;
			}
		}

		// Token: 0x0600ABC2 RID: 43970 RVA: 0x003CA2B4 File Offset: 0x003C84B4
		public static void InitiateCountdown(Building_ArchonexusCore archonexusCore)
		{
			if (!ModLister.CheckIdeology("Archonexus victory countdown"))
			{
				return;
			}
			ArchonexusCountdown.archonexusCoreRoot = archonexusCore;
			ArchonexusCountdown.timeLeft = 6f;
			SoundDefOf.Archotech_Invoked.PlayOneShot(archonexusCore);
			ScreenFader.StartFade(Color.white, 6f);
		}

		// Token: 0x0600ABC3 RID: 43971 RVA: 0x003CA2F2 File Offset: 0x003C84F2
		public static void CancelCountdown()
		{
			ArchonexusCountdown.timeLeft = -1f;
			ArchonexusCountdown.archonexusCoreRoot = null;
			ScreenFader.SetColor(Color.clear);
		}

		// Token: 0x0600ABC4 RID: 43972 RVA: 0x003CA310 File Offset: 0x003C8510
		private static void EndGame()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<Pawn> list = (from p in ArchonexusCountdown.archonexusCoreRoot.Map.mapPawns.PawnsInFaction(Faction.OfPlayer)
			where p.RaceProps.Humanlike
			select p).ToList<Pawn>();
			foreach (Pawn pawn in list)
			{
				if (!pawn.Dead && !pawn.IsQuestLodger())
				{
					stringBuilder.AppendLine("   " + pawn.LabelCap);
					Find.StoryWatcher.statsRecord.colonistsLaunched++;
				}
			}
			GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverArchotechInvokedIntro".Translate(), "GameOverArchotechInvokedEnding".Translate(), stringBuilder.ToString(), "GameOverColonistsTranscended", list), SongDefOf.ArchonexusVictorySong, true, 2.5f);
		}

		// Token: 0x0600ABC5 RID: 43973 RVA: 0x003CA420 File Offset: 0x003C8620
		public static void ArchonexusCountdownUpdate()
		{
			if (ArchonexusCountdown.timeLeft > 0f)
			{
				ArchonexusCountdown.timeLeft -= Time.deltaTime;
				if (ArchonexusCountdown.timeLeft <= 0f)
				{
					ArchonexusCountdown.EndGame();
				}
			}
		}

		// Token: 0x040063F4 RID: 25588
		private const float ScreenFadeSeconds = 6f;

		// Token: 0x040063F5 RID: 25589
		private const float SongStartDelay = 2.5f;

		// Token: 0x040063F6 RID: 25590
		private static float timeLeft = -1f;

		// Token: 0x040063F7 RID: 25591
		private static Building_ArchonexusCore archonexusCoreRoot;
	}
}
