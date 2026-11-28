using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FEC RID: 8172
	public class Verb_MeleeApplyHediff : Verb_MeleeAttack
	{
		// Token: 0x0600CF2E RID: 53038 RVA: 0x004C4AFC File Offset: 0x004C2CFC
		protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (this.tool == null)
			{
				Log.ErrorOnce("Attempted to apply melee hediff without a tool", 38381735);
				return damageResult;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				Log.ErrorOnce("Attempted to apply melee hediff without pawn target", 78330053);
				return damageResult;
			}
			foreach (BodyPartRecord part in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, this.verbProps.bodypartTagTarget, null))
			{
				damageResult.AddHediff(pawn.health.AddHediff(this.tool.hediff, part, null, null));
				damageResult.AddPart(pawn, part);
				damageResult.wounded = true;
			}
			return damageResult;
		}

		// Token: 0x0600CF2F RID: 53039 RVA: 0x004C4BD4 File Offset: 0x004C2DD4
		public override bool IsUsableOn(Thing target)
		{
			return target is Pawn;
		}
	}
}
