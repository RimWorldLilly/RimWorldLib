using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD5 RID: 7125
	public class Alert_Exhaustion : Alert
	{
		// Token: 0x0600B67C RID: 46716 RVA: 0x00404975 File Offset: 0x00402B75
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170020F8 RID: 8440
		// (get) Token: 0x0600B67D RID: 46717 RVA: 0x004049B0 File Offset: 0x00402BB0
		private List<Pawn> ExhaustedColonists
		{
			get
			{
				this.exhaustedColonistsResult.Clear();
				List<Pawn> allMaps_FreeColonistsSpawned = PawnsFinder.AllMaps_FreeColonistsSpawned;
				for (int i = 0; i < allMaps_FreeColonistsSpawned.Count; i++)
				{
					if (allMaps_FreeColonistsSpawned[i].needs.rest != null && allMaps_FreeColonistsSpawned[i].needs.rest.CurCategory == RestCategory.Exhausted)
					{
						this.exhaustedColonistsResult.Add(allMaps_FreeColonistsSpawned[i]);
					}
				}
				return this.exhaustedColonistsResult;
			}
		}

		// Token: 0x0600B67E RID: 46718 RVA: 0x00404A24 File Offset: 0x00402C24
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ExhaustionDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B67F RID: 46719 RVA: 0x00404AC0 File Offset: 0x00402CC0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}

		// Token: 0x040068CF RID: 26831
		private List<Pawn> exhaustedColonistsResult = new List<Pawn>();

		// Token: 0x040068D0 RID: 26832
		private StringBuilder sb = new StringBuilder();
	}
}
