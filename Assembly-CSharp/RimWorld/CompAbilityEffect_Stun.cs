using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145A RID: 5210
	public class CompAbilityEffect_Stun : CompAbilityEffect_WithDuration
	{
		// Token: 0x0600800E RID: 32782 RVA: 0x002DAB54 File Offset: 0x002D8D54
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.HasThing)
			{
				base.Apply(target, dest);
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					pawn.stances.stunner.StunFor(base.GetDurationSeconds(pawn).SecondsToTicks(), this.parent.pawn, false, true, false);
				}
			}
		}
	}
}
