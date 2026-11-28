using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FEE RID: 8174
	public class Verb_MeleeAttackDamage : Verb_MeleeAttack
	{
		// Token: 0x0600CF3E RID: 53054 RVA: 0x004C564A File Offset: 0x004C384A
		private IEnumerable<DamageInfo> DamageInfosToApply(LocalTargetInfo target)
		{
			Verb_MeleeAttackDamage.<DamageInfosToApply>d__2 <DamageInfosToApply>d__ = new Verb_MeleeAttackDamage.<DamageInfosToApply>d__2(-2);
			<DamageInfosToApply>d__.<>4__this = this;
			<DamageInfosToApply>d__.<>3__target = target;
			return <DamageInfosToApply>d__;
		}

		// Token: 0x0600CF3F RID: 53055 RVA: 0x004C5664 File Offset: 0x004C3864
		protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			DamageWorker.DamageResult result = new DamageWorker.DamageResult();
			foreach (DamageInfo dinfo in this.DamageInfosToApply(target))
			{
				if (target.ThingDestroyed)
				{
					break;
				}
				result = target.Thing.TakeDamage(dinfo);
			}
			return result;
		}

		// Token: 0x040083C3 RID: 33731
		private const float MeleeDamageRandomFactorMin = 0.8f;

		// Token: 0x040083C4 RID: 33732
		private const float MeleeDamageRandomFactorMax = 1.2f;
	}
}
