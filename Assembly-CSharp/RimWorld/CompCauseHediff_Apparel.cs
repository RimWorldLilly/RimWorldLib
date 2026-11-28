using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184B RID: 6219
	public class CompCauseHediff_Apparel : ThingComp
	{
		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x06009BA8 RID: 39848 RVA: 0x00381BF4 File Offset: 0x0037FDF4
		private CompProperties_CauseHediff_Apparel Props
		{
			get
			{
				return (CompProperties_CauseHediff_Apparel)this.props;
			}
		}

		// Token: 0x06009BA9 RID: 39849 RVA: 0x00381C04 File Offset: 0x0037FE04
		public override void Notify_Equipped(Pawn pawn)
		{
			if (pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false) == null)
			{
				HediffComp_RemoveIfApparelDropped hediffComp_RemoveIfApparelDropped = pawn.health.AddHediff(this.Props.hediff, pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrFallback((BodyPartRecord p) => p.def == this.Props.part, null), null, null).TryGetComp<HediffComp_RemoveIfApparelDropped>();
				if (hediffComp_RemoveIfApparelDropped != null)
				{
					hediffComp_RemoveIfApparelDropped.wornApparel = (Apparel)this.parent;
				}
			}
		}
	}
}
