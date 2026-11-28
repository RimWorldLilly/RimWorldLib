using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001429 RID: 5161
	public class CompAbilityEffect_PreachHealth : CompAbilityEffect
	{
		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x06007F68 RID: 32616 RVA: 0x002D825F File Offset: 0x002D645F
		public new CompProperties_PreachHealth Props
		{
			get
			{
				return (CompProperties_PreachHealth)this.props;
			}
		}

		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x06007F69 RID: 32617 RVA: 0x002D826C File Offset: 0x002D646C
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007F6A RID: 32618 RVA: 0x002D8270 File Offset: 0x002D6470
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeHuman(pawn, throwMessages, this.parent) && AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent) && AbilityUtility.ValidateSameIdeo(this.parent.pawn, pawn, throwMessages, this.parent) && AbilityUtility.ValidateSickOrInjured(pawn, throwMessages, this.parent);
		}
	}
}
