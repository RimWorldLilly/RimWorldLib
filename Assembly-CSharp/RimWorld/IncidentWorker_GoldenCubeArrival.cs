using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117C RID: 4476
	public class IncidentWorker_GoldenCubeArrival : IncidentWorker_MeteoriteImpact
	{
		// Token: 0x0600704E RID: 28750 RVA: 0x002751B0 File Offset: 0x002733B0
		protected override List<Thing> GenerateMeteorContents(IncidentParms parms)
		{
			Thing item = ThingMaker.MakeThing(ThingDefOf.GoldenCube, null);
			if (!QuestUtility.TryGetIdealColonist(out this.affectedPawn, (Map)parms.target, new Func<Pawn, bool>(this.ValidatePawn)))
			{
				return null;
			}
			this.affectedPawn.health.AddHediff(HediffDefOf.CubeInterest, null, null, null);
			return new List<Thing>
			{
				item
			};
		}

		// Token: 0x0600704F RID: 28751 RVA: 0x0027521C File Offset: 0x0027341C
		protected override Letter MakeLetter(Skyfaller meteorite, List<Thing> contents)
		{
			return LetterMaker.MakeLetter("GoldenCubeArrivalLabel".Translate(), "GoldenCubeArrivalText".Translate(this.affectedPawn.Named("PAWN")), LetterDefOf.ThreatSmall, new TargetInfo(meteorite.Position, meteorite.Map, false), null, null, null);
		}

		// Token: 0x06007050 RID: 28752 RVA: 0x00275274 File Offset: 0x00273474
		private bool ValidatePawn(Pawn pawn)
		{
			return (pawn.IsColonist || pawn.IsSlaveOfColony) && !pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false) && !pawn.health.hediffSet.HasHediff(HediffDefOf.CubeComa, false);
		}

		// Token: 0x040047A9 RID: 18345
		private Pawn affectedPawn;
	}
}
