using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013EA RID: 5098
	public class CompAbilityEffect_DeactivateMechanoid : CompAbilityEffect
	{
		// Token: 0x06007EA9 RID: 32425 RVA: 0x002D4900 File Offset: 0x002D2B00
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			CompMechanoid comp = pawn.GetComp<CompMechanoid>();
			if (comp != null)
			{
				comp.Deactivate();
			}
			Effecter effecter = EffecterDefOf.MechDisabled.Spawn();
			effecter.Trigger(new TargetInfo(target.Cell, this.parent.pawn.MapHeld, false), new TargetInfo(target.Cell, this.parent.pawn.MapHeld, false), -1);
			effecter.Cleanup();
		}

		// Token: 0x06007EAA RID: 32426 RVA: 0x002D497B File Offset: 0x002D2B7B
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06007EAB RID: 32427 RVA: 0x002D4988 File Offset: 0x002D2B88
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && pawn.RaceProps.IsMechanoid && pawn.HostileTo(Faction.OfPlayer) && base.Valid(target, throwMessages);
		}
	}
}
