using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200161C RID: 5660
	public class LandingOutcomeWorker_OverheatedGravEngine : LandingOutcomeWorker
	{
		// Token: 0x06008D06 RID: 36102 RVA: 0x00334B14 File Offset: 0x00332D14
		public LandingOutcomeWorker_OverheatedGravEngine(LandingOutcomeDef def) : base(def)
		{
		}

		// Token: 0x06008D07 RID: 36103 RVA: 0x00334B1D File Offset: 0x00332D1D
		public override void ApplyOutcome(Gravship gravship)
		{
			gravship.Engine.cooldownCompleteTick += Rand.Range(120000, 240000);
			base.SendStandardLetter(gravship.Engine, null, gravship.Engine);
		}
	}
}
