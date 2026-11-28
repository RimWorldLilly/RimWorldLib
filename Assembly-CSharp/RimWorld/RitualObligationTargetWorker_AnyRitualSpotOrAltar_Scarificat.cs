using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200169F RID: 5791
	public class RitualObligationTargetWorker_AnyRitualSpotOrAltar_Scarification : RitualObligationTargetWorker_AnyRitualSpotOrAltar
	{
		// Token: 0x06009127 RID: 37159 RVA: 0x00352C79 File Offset: 0x00350E79
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Scarification()
		{
		}

		// Token: 0x06009128 RID: 37160 RVA: 0x00352C81 File Offset: 0x00350E81
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Scarification(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009129 RID: 37161 RVA: 0x00352C8C File Offset: 0x00350E8C
		public override bool ObligationTargetsValid(RitualObligation obligation)
		{
			Pawn pawn = obligation.targetA.Thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (pawn.Dead)
			{
				return false;
			}
			int hediffCount = pawn.health.hediffSet.GetHediffCount(HediffDefOf.Scarification);
			return pawn.Ideo != null && pawn.Ideo.RequiredScars > hediffCount;
		}

		// Token: 0x0600912A RID: 37162 RVA: 0x00352CE7 File Offset: 0x00350EE7
		public override string LabelExtraPart(RitualObligation obligation)
		{
			return obligation.targetA.Thing.LabelShort;
		}
	}
}
