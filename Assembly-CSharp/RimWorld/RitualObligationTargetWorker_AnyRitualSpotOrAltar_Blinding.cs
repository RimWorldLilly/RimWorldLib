using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016A1 RID: 5793
	public class RitualObligationTargetWorker_AnyRitualSpotOrAltar_Blinding : RitualObligationTargetWorker_AnyRitualSpotOrAltar
	{
		// Token: 0x06009131 RID: 37169 RVA: 0x00352DF0 File Offset: 0x00350FF0
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Blinding()
		{
		}

		// Token: 0x06009132 RID: 37170 RVA: 0x00352DF8 File Offset: 0x00350FF8
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Blinding(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009133 RID: 37171 RVA: 0x00352E04 File Offset: 0x00351004
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
			return pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord p) => p.def == BodyPartDefOf.Eye);
		}
	}
}
