using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA7 RID: 3239
	public class PsychicRitualRoleDef_DeathRefusalTarget : PsychicRitualRoleDef
	{
		// Token: 0x06005BB6 RID: 23478 RVA: 0x0021B3B4 File Offset: 0x002195B4
		protected override bool PawnCanDo(PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target, out AnyEnum reason)
		{
			if (!base.PawnCanDo(context, pawn, target, out reason))
			{
				return false;
			}
			Hediff_DeathRefusal firstHediff = pawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			if (firstHediff != null && firstHediff.UsesLeft >= firstHediff.MaxUses)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef_DeathRefusalTarget.DeathRefusalTargetReason>(PsychicRitualRoleDef_DeathRefusalTarget.DeathRefusalTargetReason.MaxDeathRefusal);
				return false;
			}
			return true;
		}

		// Token: 0x06005BB7 RID: 23479 RVA: 0x0021B404 File Offset: 0x00219604
		public override TaggedString PawnCannotDoReason(AnyEnum reason, PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target)
		{
			PsychicRitualRoleDef_DeathRefusalTarget.DeathRefusalTargetReason? deathRefusalTargetReason = reason.As<PsychicRitualRoleDef_DeathRefusalTarget.DeathRefusalTargetReason>();
			if (deathRefusalTargetReason != null)
			{
				deathRefusalTargetReason.GetValueOrDefault();
				return this.psychicRitualLeaveReason_MaxDeathRefusal.Formatted(pawn.Named("PAWN"));
			}
			return base.PawnCannotDoReason(reason, context, pawn, target);
		}

		// Token: 0x0400347B RID: 13435
		[MustTranslate]
		public string psychicRitualLeaveReason_MaxDeathRefusal;

		// Token: 0x02002E98 RID: 11928
		public enum DeathRefusalTargetReason
		{
			// Token: 0x0400BCF8 RID: 48376
			None,
			// Token: 0x0400BCF9 RID: 48377
			MaxDeathRefusal
		}
	}
}
