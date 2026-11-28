using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001188 RID: 4488
	public class IncidentWorker_MetalhorrorImplantation : IncidentWorker
	{
		// Token: 0x06007077 RID: 28791 RVA: 0x00275E1C File Offset: 0x0027401C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return Find.Anomaly.CanNewMetalhorrorBiosignatureImplantOccur && this.GetPossiblePawns(map).Any<Pawn>();
		}

		// Token: 0x06007078 RID: 28792 RVA: 0x00275E50 File Offset: 0x00274050
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.GetAllPawnInfectionPathways(IncidentWorker_MetalhorrorImplantation.TmpPathways, map);
			if (IncidentWorker_MetalhorrorImplantation.TmpPathways.Empty<InfectionPathway>())
			{
				return false;
			}
			InfectionPathway infectionPathway = IncidentWorker_MetalhorrorImplantation.TmpPathways.RandomElementByWeight((InfectionPathway pathway) => IncidentWorker_MetalhorrorImplantation.PathwayAgeWeightCurve.Evaluate(pathway.AgePercent));
			IncidentWorker_MetalhorrorImplantation.TmpPathways.Clear();
			string explanation = infectionPathway.GetExplanation(HediffDefOf.MetalhorrorImplant);
			MetalhorrorUtility.Infect(infectionPathway.OwnerPawn, null, "InsectImplant", explanation);
			return true;
		}

		// Token: 0x06007079 RID: 28793 RVA: 0x00275ED4 File Offset: 0x002740D4
		private List<Pawn> GetPossiblePawns(Map map)
		{
			List<Pawn> freeColonistsAndPrisoners = map.mapPawns.FreeColonistsAndPrisoners;
			for (int i = freeColonistsAndPrisoners.Count - 1; i >= 0; i--)
			{
				Pawn pawn = freeColonistsAndPrisoners[i];
				if (!MetalhorrorUtility.CanBeInfected(pawn) || !pawn.infectionVectors.AnyPathwayForHediff(HediffDefOf.MetalhorrorImplant))
				{
					freeColonistsAndPrisoners.RemoveAt(i);
				}
			}
			return freeColonistsAndPrisoners;
		}

		// Token: 0x0600707A RID: 28794 RVA: 0x00275F2C File Offset: 0x0027412C
		private void GetAllPawnInfectionPathways(List<InfectionPathway> pathways, Map map)
		{
			pathways.Clear();
			List<Pawn> freeColonistsAndPrisoners = map.mapPawns.FreeColonistsAndPrisoners;
			for (int i = 0; i < freeColonistsAndPrisoners.Count; i++)
			{
				Pawn pawn = freeColonistsAndPrisoners[i];
				if (MetalhorrorUtility.CanBeInfected(pawn))
				{
					pathways.AddRange(pawn.infectionVectors.GetPathwaysForHediff(HediffDefOf.MetalhorrorImplant));
				}
			}
		}

		// Token: 0x040047AF RID: 18351
		private static readonly SimpleCurve PathwayAgeWeightCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.7f, 0.3f),
				true
			}
		};

		// Token: 0x040047B0 RID: 18352
		private static readonly List<InfectionPathway> TmpPathways = new List<InfectionPathway>();
	}
}
