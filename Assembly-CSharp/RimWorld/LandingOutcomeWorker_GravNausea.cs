using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200161D RID: 5661
	public class LandingOutcomeWorker_GravNausea : LandingOutcomeWorker
	{
		// Token: 0x06008D08 RID: 36104 RVA: 0x00334B58 File Offset: 0x00332D58
		public LandingOutcomeWorker_GravNausea(LandingOutcomeDef def) : base(def)
		{
		}

		// Token: 0x06008D09 RID: 36105 RVA: 0x00334B64 File Offset: 0x00332D64
		public override void ApplyOutcome(Gravship gravship)
		{
			foreach (Pawn pawn in gravship.Pawns)
			{
				if (pawn.RaceProps.IsFlesh)
				{
					Pawn_HealthTracker health = pawn.health;
					if (health != null)
					{
						health.AddHediff(HediffDefOf.GravNausea, null, null, null);
					}
				}
			}
			base.SendStandardLetter(gravship.Engine, null, new LookTargets(gravship.Pawns));
		}
	}
}
