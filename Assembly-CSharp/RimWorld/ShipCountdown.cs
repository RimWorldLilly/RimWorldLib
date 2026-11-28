using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019FD RID: 6653
	public static class ShipCountdown
	{
		// Token: 0x17001D52 RID: 7506
		// (get) Token: 0x0600A785 RID: 42885 RVA: 0x003BA91C File Offset: 0x003B8B1C
		public static bool CountingDown
		{
			get
			{
				return ShipCountdown.timeLeft >= 0f;
			}
		}

		// Token: 0x0600A786 RID: 42886 RVA: 0x003BA92D File Offset: 0x003B8B2D
		public static void InitiateCountdown(Building launchingShipRoot)
		{
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
			ShipCountdown.shipRoot = launchingShipRoot;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = null;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x0600A787 RID: 42887 RVA: 0x003BA95F File Offset: 0x003B8B5F
		public static void InitiateCountdown(string launchString)
		{
			ShipCountdown.shipRoot = null;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = launchString;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x0600A788 RID: 42888 RVA: 0x003BA986 File Offset: 0x003B8B86
		public static void ShipCountdownUpdate()
		{
			if (ShipCountdown.timeLeft > 0f)
			{
				ShipCountdown.timeLeft -= Time.deltaTime;
				if (ShipCountdown.timeLeft <= 0f)
				{
					ShipCountdown.CountdownEnded();
				}
			}
		}

		// Token: 0x0600A789 RID: 42889 RVA: 0x003BA9B5 File Offset: 0x003B8BB5
		public static void CancelCountdown()
		{
			ShipCountdown.timeLeft = -1000f;
			ScreenFader.SetColor(Color.clear);
		}

		// Token: 0x0600A78A RID: 42890 RVA: 0x003BA9CC File Offset: 0x003B8BCC
		private static void CountdownEnded()
		{
			if (ShipCountdown.shipRoot != null)
			{
				TaggedString taggedString = "GameOverShipLaunchedEnding".Translate();
				List<Building> list = ShipUtility.ShipBuildingsAttachedTo(ShipCountdown.shipRoot).ToList<Building>();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Building building in list)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = building as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
					{
						stringBuilder.AppendLine("   " + building_CryptosleepCasket.ContainedThing.LabelCap);
						Find.StoryWatcher.statsRecord.colonistsLaunched++;
						TaleRecorder.RecordTale(TaleDefOf.LaunchedShip, new object[]
						{
							building_CryptosleepCasket.ContainedThing
						});
					}
				}
				GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), taggedString, stringBuilder.ToString(), "GameOverColonistsEscaped", null), SongDefOf.EndCreditsSong, false, 5f);
				using (List<Building>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Building building2 = enumerator.Current;
						building2.Destroy(DestroyMode.Vanish);
					}
					return;
				}
			}
			if (!ShipCountdown.customLaunchString.NullOrEmpty())
			{
				GameVictoryUtility.ShowCredits(ShipCountdown.customLaunchString, SongDefOf.EndCreditsSong, false, 5f);
				return;
			}
			GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), "GameOverShipLaunchedEnding".Translate(), null, "GameOverColonistsEscaped", null), SongDefOf.EndCreditsSong, false, 5f);
		}

		// Token: 0x04006294 RID: 25236
		private static float timeLeft = -1000f;

		// Token: 0x04006295 RID: 25237
		private static Building shipRoot;

		// Token: 0x04006296 RID: 25238
		private static string customLaunchString;

		// Token: 0x04006297 RID: 25239
		private const float InitialTime = 7.2f;
	}
}
