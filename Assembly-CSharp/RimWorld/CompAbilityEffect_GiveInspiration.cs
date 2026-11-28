using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001411 RID: 5137
	public class CompAbilityEffect_GiveInspiration : CompAbilityEffect
	{
		// Token: 0x06007F1F RID: 32543 RVA: 0x002D6D5C File Offset: 0x002D4F5C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				InspirationDef randomAvailableInspirationDef = pawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
				if (randomAvailableInspirationDef != null)
				{
					base.Apply(target, dest);
					pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, "LetterPsychicInspiration".Translate(pawn.Named("PAWN"), this.parent.pawn.Named("CASTER")), true);
				}
			}
		}

		// Token: 0x06007F20 RID: 32544 RVA: 0x002D6DD2 File Offset: 0x002D4FD2
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06007F21 RID: 32545 RVA: 0x002D6DDC File Offset: 0x002D4FDC
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!AbilityUtility.ValidateNoInspiration(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (!AbilityUtility.ValidateCanGetInspiration(pawn, throwMessages, this.parent))
				{
					return false;
				}
			}
			return base.Valid(target, throwMessages);
		}
	}
}
