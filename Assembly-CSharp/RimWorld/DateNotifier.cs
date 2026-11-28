using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001024 RID: 4132
	public class DateNotifier : IExposable
	{
		// Token: 0x06006811 RID: 26641 RVA: 0x0024C690 File Offset: 0x0024A890
		public void ExposeData()
		{
			Scribe_Values.Look<Season>(ref this.lastSeason, "lastSeason", Season.Undefined, false);
		}

		// Token: 0x06006812 RID: 26642 RVA: 0x0024C6A4 File Offset: 0x0024A8A4
		public void DateNotifierTick()
		{
			Map map = this.FindPlayerHomeWithMinTimezone();
			float latitude = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).y : 0f;
			float longitude = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).x : 0f;
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, latitude, longitude);
			if (season != this.lastSeason && (this.lastSeason == Season.Undefined || season != this.lastSeason.GetPreviousSeason()))
			{
				if (this.lastSeason != Season.Undefined && this.AnyPlayerHomeSeasonsAreMeaningful())
				{
					if (GenDate.YearsPassed == 0 && season == Season.Summer && this.AnyPlayerHomeAvgTempIsLowInWinter())
					{
						Find.LetterStack.ReceiveLetter("LetterLabelFirstSummerWarning".Translate(), "FirstSummerWarning".Translate(), LetterDefOf.NeutralEvent, null, 0, true);
					}
					else if (GenDate.DaysPassed > 5)
					{
						Messages.Message("MessageSeasonBegun".Translate(season.Label()).CapitalizeFirst(), MessageTypeDefOf.NeutralEvent, true);
					}
				}
				this.lastSeason = season;
			}
		}

		// Token: 0x06006813 RID: 26643 RVA: 0x0024C7B8 File Offset: 0x0024A9B8
		private Map FindPlayerHomeWithMinTimezone()
		{
			List<Map> maps = Find.Maps;
			Map map = null;
			int num = -1;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					int num2 = GenDate.TimeZoneAt(Find.WorldGrid.LongLatOf(maps[i].Tile).x);
					if (map == null || num2 < num)
					{
						map = maps[i];
						num = num2;
					}
				}
			}
			return map;
		}

		// Token: 0x06006814 RID: 26644 RVA: 0x0024C824 File Offset: 0x0024AA24
		private bool AnyPlayerHomeSeasonsAreMeaningful()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].mapTemperature.LocalSeasonsAreMeaningful() && maps[i].Tile.Valid && !maps[i].Tile.LayerDef.isSpace)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006815 RID: 26645 RVA: 0x0024C8A0 File Offset: 0x0024AAA0
		private bool AnyPlayerHomeAvgTempIsLowInWinter()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && GenTemperature.AverageTemperatureAtTileForTwelfth(maps[i].Tile, Season.Winter.GetMiddleTwelfth(Find.WorldGrid.LongLatOf(maps[i].Tile).y)) < 8f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400424E RID: 16974
		private Season lastSeason;
	}
}
