using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD6 RID: 7126
	public class Alert_Heatstroke : Alert
	{
		// Token: 0x0600B680 RID: 46720 RVA: 0x00404ACD File Offset: 0x00402CCD
		public Alert_Heatstroke()
		{
			this.defaultLabel = "AlertHeatstroke".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170020F9 RID: 8441
		// (get) Token: 0x0600B681 RID: 46721 RVA: 0x00404B08 File Offset: 0x00402D08
		private List<Pawn> HeatstrokePawns
		{
			get
			{
				this.heatstrokePawnsResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i];
					if (pawn.health != null && !pawn.IsAnimal && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, true) != null)
					{
						this.heatstrokePawnsResult.Add(pawn);
					}
				}
				return this.heatstrokePawnsResult;
			}
		}

		// Token: 0x0600B682 RID: 46722 RVA: 0x00404B80 File Offset: 0x00402D80
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.HeatstrokePawns)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return string.Format("AlertHeatstrokeDesc".Translate(), this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B683 RID: 46723 RVA: 0x00404C28 File Offset: 0x00402E28
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HeatstrokePawns);
		}

		// Token: 0x040068D1 RID: 26833
		private List<Pawn> heatstrokePawnsResult = new List<Pawn>();

		// Token: 0x040068D2 RID: 26834
		private StringBuilder sb = new StringBuilder();
	}
}
