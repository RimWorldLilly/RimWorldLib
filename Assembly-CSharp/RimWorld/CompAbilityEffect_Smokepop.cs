using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001445 RID: 5189
	public class CompAbilityEffect_Smokepop : CompAbilityEffect
	{
		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x06007FC4 RID: 32708 RVA: 0x002D96E0 File Offset: 0x002D78E0
		public new CompProperties_AbilitySmokepop Props
		{
			get
			{
				return (CompProperties_AbilitySmokepop)this.props;
			}
		}

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x06007FC5 RID: 32709 RVA: 0x002D96ED File Offset: 0x002D78ED
		public bool ShouldHaveInspectString
		{
			get
			{
				return ModsConfig.BiotechActive && this.parent.pawn.RaceProps.IsMechanoid;
			}
		}

		// Token: 0x06007FC6 RID: 32710 RVA: 0x002D9710 File Offset: 0x002D7910
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.smokeRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, new GasType?(GasType.BlindSmoke), null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x06007FC7 RID: 32711 RVA: 0x002D97AE File Offset: 0x002D79AE
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(target.Cell, this.Props.smokeRadius);
		}

		// Token: 0x06007FC8 RID: 32712 RVA: 0x002D97C8 File Offset: 0x002D79C8
		public override string CompInspectStringExtra()
		{
			if (!this.ShouldHaveInspectString)
			{
				return null;
			}
			if (this.parent.CanCast)
			{
				return "AbilityMechSmokepopCharged".Translate();
			}
			return "AbilityMechSmokepopRecharging".Translate(this.parent.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false));
		}
	}
}
