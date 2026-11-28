using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003F0 RID: 1008
	public class DamageWorker_Nerve : DamageWorker_AddInjury
	{
		// Token: 0x06001D7D RID: 7549 RVA: 0x000B4AD0 File Offset: 0x000B2CD0
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.ApplySpecialEffectsToPart(pawn, totalDamage, dinfo, result);
			if (!pawn.RaceProps.IsMechanoid && !pawn.Dead && result.totalDamageDealt > 0f)
			{
				DamageInfo dinfo2 = dinfo;
				dinfo2.Def = DamageDefOf.NerveStun;
				float amount = DamageWorker_Nerve.NerveStunSecondsFromBodySizeCurve.Evaluate(pawn.BodySize) * 2f * this.StunDamageFactorFromQuality(dinfo.WeaponQuality);
				dinfo2.SetAmount(amount);
				pawn.TakeDamage(dinfo2);
			}
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x000B4B4E File Offset: 0x000B2D4E
		private float StunDamageFactorFromQuality(QualityCategory category)
		{
			if (category == QualityCategory.Awful)
			{
				return 0.75f;
			}
			if (category == QualityCategory.Masterwork)
			{
				return 1.25f;
			}
			if (category != QualityCategory.Legendary)
			{
				return 1f;
			}
			return 1.5f;
		}

		// Token: 0x04001776 RID: 6006
		private const float StunDamageFactor_Awful = 0.75f;

		// Token: 0x04001777 RID: 6007
		private const float StunDamageFactor_Default = 1f;

		// Token: 0x04001778 RID: 6008
		private const float StunDamageFactor_Masterwork = 1.25f;

		// Token: 0x04001779 RID: 6009
		private const float StunDamageFactor_Legendary = 1.5f;

		// Token: 0x0400177A RID: 6010
		private static readonly SimpleCurve NerveStunSecondsFromBodySizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.01f, 3f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			},
			{
				new CurvePoint(2f, 2f),
				true
			},
			{
				new CurvePoint(4f, 1f),
				true
			}
		};
	}
}
