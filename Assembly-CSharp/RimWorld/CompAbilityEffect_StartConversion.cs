using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144F RID: 5199
	public class CompAbilityEffect_StartConversion : CompAbilityEffect_StartRitualOnPawn
	{
		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x06007FE3 RID: 32739 RVA: 0x002DA10F File Offset: 0x002D830F
		public new CompProperties_AbilityStartConversion Props
		{
			get
			{
				return (CompProperties_AbilityStartConversion)this.props;
			}
		}

		// Token: 0x06007FE4 RID: 32740 RVA: 0x002DA11C File Offset: 0x002D831C
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return RitualBehaviorWorker_Conversion.ValidateConvertee(target.Pawn, this.parent.pawn, throwMessages) && base.Valid(target, throwMessages);
		}
	}
}
