using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019FE RID: 6654
	public static class GameVictoryUtility
	{
		// Token: 0x0600A78C RID: 42892 RVA: 0x003BAB7C File Offset: 0x003B8D7C
		public static string MakeEndCredits(string intro, string ending, string escapees, string colonistsEscapeeTKey = "GameOverColonistsEscaped", IList<Pawn> escaped = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(intro);
			if (!escapees.NullOrEmpty())
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(colonistsEscapeeTKey.Translate(escapees));
			}
			stringBuilder.AppendLine();
			string text = GameVictoryUtility.PawnsLeftBehind(escaped);
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLine("GameOverColonistsLeft".Translate(text));
			}
			stringBuilder.AppendLine(ending);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(GameVictoryUtility.InMemoryOfSection());
			return stringBuilder.ToString();
		}

		// Token: 0x0600A78D RID: 42893 RVA: 0x003BAC18 File Offset: 0x003B8E18
		public static void ShowCredits(string victoryText, SongDef endCreditsSong, bool exitToMainMenu = false, float songStartDelay = 5f)
		{
			Screen_Credits screen_Credits = new Screen_Credits(victoryText, 500);
			screen_Credits.wonGame = true;
			screen_Credits.exitToMainMenu = exitToMainMenu;
			Find.WindowStack.Add(screen_Credits);
			if (endCreditsSong != null)
			{
				screen_Credits.endCreditsSong = endCreditsSong;
				screen_Credits.songStartDelay = songStartDelay;
				Find.MusicManagerPlay.ForceSilenceFor(999f);
			}
			ScreenFader.StartFade(Color.clear, 3f);
		}

		// Token: 0x0600A78E RID: 42894 RVA: 0x003BAC7C File Offset: 0x003B8E7C
		public static string PawnsLeftBehind(IList<Pawn> escaped = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (escaped == null || !escaped.Contains(pawn))
				{
					stringBuilder.AppendLine("   " + pawn.LabelCap);
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (caravan.IsPlayerControlled)
				{
					List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
					for (int j = 0; j < pawnsListForReading.Count; j++)
					{
						Pawn pawn2 = pawnsListForReading[j];
						if (pawn2.IsColonist && pawn2.HostFaction == null && (escaped == null || !escaped.Contains(pawn2)))
						{
							stringBuilder.AppendLine("   " + pawn2.LabelCap);
						}
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				return string.Empty;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A78F RID: 42895 RVA: 0x003BAD9C File Offset: 0x003B8F9C
		public static string InMemoryOfSection()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Pawn> enumerable = from p in Find.WorldPawns.AllPawnsDead
			where p.IsColonist
			select p;
			if (enumerable.Any<Pawn>())
			{
				stringBuilder.AppendLine("GameOverInMemoryOf".Translate());
				foreach (Pawn pawn in enumerable)
				{
					stringBuilder.AppendLine("   " + pawn.LabelCap);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
