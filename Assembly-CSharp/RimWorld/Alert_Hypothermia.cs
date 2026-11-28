using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA2 RID: 7074
	public class Alert_Hypothermia : Alert_Critical
	{
		// Token: 0x0600B5A0 RID: 46496 RVA: 0x003FF81C File Offset: 0x003FDA1C
		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		// Token: 0x170020D6 RID: 8406
		// (get) Token: 0x0600B5A1 RID: 46497 RVA: 0x003FF844 File Offset: 0x003FDA44
		private List<Pawn> HypothermiaDangerColonists
		{
			get
			{
				this.hypothermiaDangerColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (!pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature))
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex >= 3)
						{
							this.hypothermiaDangerColonistsResult.Add(pawn);
						}
					}
				}
				return this.hypothermiaDangerColonistsResult;
			}
		}

		// Token: 0x0600B5A2 RID: 46498 RVA: 0x003FF8E8 File Offset: 0x003FDAE8
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.hypothermiaDangerColonistsResult)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertHypothermiaDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B5A3 RID: 46499 RVA: 0x003FF974 File Offset: 0x003FDB74
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerColonists);
		}

		// Token: 0x04006882 RID: 26754
		private List<Pawn> hypothermiaDangerColonistsResult = new List<Pawn>();
	}
}
