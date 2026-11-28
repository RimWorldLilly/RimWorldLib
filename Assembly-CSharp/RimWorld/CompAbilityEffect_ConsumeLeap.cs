using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E3 RID: 5091
	public class CompAbilityEffect_ConsumeLeap : CompAbilityEffect, ICompAbilityEffectOnJumpCompleted
	{
		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x06007E8D RID: 32397 RVA: 0x002D3778 File Offset: 0x002D1978
		private new CompProperties_ConsumeLeap Props
		{
			get
			{
				return (CompProperties_ConsumeLeap)this.props;
			}
		}

		// Token: 0x06007E8E RID: 32398 RVA: 0x002D3788 File Offset: 0x002D1988
		public void OnJumpCompleted(IntVec3 origin, LocalTargetInfo target)
		{
			CompDevourer compDevourer;
			if (this.parent.pawn.TryGetComp(out compDevourer))
			{
				compDevourer.StartDigesting(origin, target);
			}
		}

		// Token: 0x06007E8F RID: 32399 RVA: 0x002D37B4 File Offset: 0x002D19B4
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && pawn.BodySize <= this.Props.maxBodySize;
		}

		// Token: 0x06007E90 RID: 32400 RVA: 0x002D37E4 File Offset: 0x002D19E4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return target.Pawn.BodySize <= this.Props.maxBodySize && base.Valid(target, throwMessages);
		}
	}
}
