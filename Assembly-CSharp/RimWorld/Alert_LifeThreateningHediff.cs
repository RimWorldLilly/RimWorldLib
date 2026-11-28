using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA4 RID: 7076
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		// Token: 0x170020D8 RID: 8408
		// (get) Token: 0x0600B5AA RID: 46506 RVA: 0x003FFBC0 File Offset: 0x003FDDC0
		private List<Pawn> SickPawns
		{
			get
			{
				this.sickPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep)
				{
					bool flag = ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Deathless);
					for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
					{
						Hediff hediff = pawn.health.hediffSet.hediffs[i];
						if (hediff.IsCurrentlyLifeThreatening && !hediff.FullyImmune())
						{
							if (flag)
							{
								HediffStage curStage = hediff.CurStage;
								if (curStage == null || !curStage.mtbDeathDestroysBrain)
								{
									goto IL_9B;
								}
							}
							this.sickPawnsResult.Add(pawn);
							break;
						}
						IL_9B:;
					}
				}
				return this.sickPawnsResult;
			}
		}

		// Token: 0x0600B5AB RID: 46507 RVA: 0x003FFCB8 File Offset: 0x003FDEB8
		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		// Token: 0x0600B5AC RID: 46508 RVA: 0x003FFCCC File Offset: 0x003FDECC
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (Pawn pawn in this.SickPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					if (hediff.CurStage != null && hediff.IsCurrentlyLifeThreatening && hediff.Part != null && hediff.Part != pawn.RaceProps.body.corePart)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return "PawnsWithLifeThreateningDiseaseAmputationDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
			}
			return "PawnsWithLifeThreateningDiseaseDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B5AD RID: 46509 RVA: 0x003FFE00 File Offset: 0x003FE000
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.SickPawns);
		}

		// Token: 0x04006884 RID: 26756
		private List<Pawn> sickPawnsResult = new List<Pawn>();
	}
}
