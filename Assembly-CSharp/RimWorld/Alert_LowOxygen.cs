using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB0 RID: 7088
	public class Alert_LowOxygen : Alert_Critical
	{
		// Token: 0x0600B5E6 RID: 46566 RVA: 0x004010A5 File Offset: 0x003FF2A5
		public Alert_LowOxygen()
		{
			this.defaultLabel = "AlertVacuumExposure".Translate();
		}

		// Token: 0x170020E9 RID: 8425
		// (get) Token: 0x0600B5E7 RID: 46567 RVA: 0x004010D0 File Offset: 0x003FF2D0
		private List<Pawn> Pawns
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return this.pawns;
				}
				this.pawns.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
				{
					if (pawn.Position.GetVacuum(pawn.Map) >= 0.5f)
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.VacuumExposure, false);
						if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex >= 2)
						{
							this.pawns.Add(pawn);
						}
					}
				}
				return this.pawns;
			}
		}

		// Token: 0x0600B5E8 RID: 46568 RVA: 0x00401188 File Offset: 0x003FF388
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.pawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertVacuumExposureDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B5E9 RID: 46569 RVA: 0x00401214 File Offset: 0x003FF414
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Pawns);
		}

		// Token: 0x04006893 RID: 26771
		private readonly List<Pawn> pawns = new List<Pawn>();
	}
}
