using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BAE RID: 7086
	public class Alert_GhoulHypothermia : Alert_Critical
	{
		// Token: 0x0600B5DE RID: 46558 RVA: 0x00400DC1 File Offset: 0x003FEFC1
		public Alert_GhoulHypothermia()
		{
			this.defaultLabel = "AlertGhoulHypothermia".Translate();
		}

		// Token: 0x170020E8 RID: 8424
		// (get) Token: 0x0600B5DF RID: 46559 RVA: 0x00400DEC File Offset: 0x003FEFEC
		private List<Pawn> HypothermiaDangerGhouls
		{
			get
			{
				this.hypothermiaDangerGhoulsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended)
				{
					if (pawn.IsGhoul && !pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature))
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex >= 3)
						{
							this.hypothermiaDangerGhoulsResult.Add(pawn);
						}
					}
				}
				return this.hypothermiaDangerGhoulsResult;
			}
		}

		// Token: 0x0600B5E0 RID: 46560 RVA: 0x00400E98 File Offset: 0x003FF098
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.hypothermiaDangerGhoulsResult)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertGhoulHypothermiaDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B5E1 RID: 46561 RVA: 0x00400F24 File Offset: 0x003FF124
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerGhouls);
		}

		// Token: 0x04006891 RID: 26769
		private List<Pawn> hypothermiaDangerGhoulsResult = new List<Pawn>();
	}
}
