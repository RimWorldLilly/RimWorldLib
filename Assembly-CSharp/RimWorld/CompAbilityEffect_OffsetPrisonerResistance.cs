using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001424 RID: 5156
	public class CompAbilityEffect_OffsetPrisonerResistance : CompAbilityEffect
	{
		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x06007F5A RID: 32602 RVA: 0x002D806C File Offset: 0x002D626C
		public new CompProperties_AbilityOffsetPrisonerResistance Props
		{
			get
			{
				return (CompProperties_AbilityOffsetPrisonerResistance)this.props;
			}
		}

		// Token: 0x06007F5B RID: 32603 RVA: 0x002D807C File Offset: 0x002D627C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				pawn.guest.resistance = Mathf.Max(pawn.guest.resistance + this.Props.offset, 0f);
			}
		}

		// Token: 0x06007F5C RID: 32604 RVA: 0x002D80C8 File Offset: 0x002D62C8
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!pawn.IsPrisonerOfColony)
				{
					return false;
				}
				if (pawn != null && pawn.guest.resistance < 1E-45f)
				{
					return false;
				}
				if (pawn.Downed)
				{
					return false;
				}
			}
			return this.Valid(target, false);
		}

		// Token: 0x06007F5D RID: 32605 RVA: 0x002D8114 File Offset: 0x002D6314
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateHasResistance(pawn, throwMessages, this.parent);
		}
	}
}
