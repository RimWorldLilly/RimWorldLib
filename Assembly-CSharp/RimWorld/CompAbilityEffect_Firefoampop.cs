using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013FF RID: 5119
	public class CompAbilityEffect_Firefoampop : CompAbilityEffect
	{
		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x06007EE7 RID: 32487 RVA: 0x002D5C3C File Offset: 0x002D3E3C
		public new CompProperties_AbilityFirefoampop Props
		{
			get
			{
				return (CompProperties_AbilityFirefoampop)this.props;
			}
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x06007EE8 RID: 32488 RVA: 0x002D5C49 File Offset: 0x002D3E49
		public bool ShouldHaveInspectString
		{
			get
			{
				return ModsConfig.BiotechActive && this.parent.pawn.RaceProps.IsMechanoid && this.parent.pawn.IsColonyMech;
			}
		}

		// Token: 0x06007EE9 RID: 32489 RVA: 0x002D5C7C File Offset: 0x002D3E7C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Effecter effecter = EffecterDefOf.ExtinguisherExplosion.Spawn();
			effecter.Trigger(new TargetInfo(target.Cell, this.parent.pawn.MapHeld, false), new TargetInfo(target.Cell, this.parent.pawn.MapHeld, false), -1);
			effecter.Cleanup();
			GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.firefoamRadius, DamageDefOf.Extinguish, null, -1, -1f, SoundDefOf.Explosion_FirefoamPopper, null, null, null, ThingDefOf.Filth_FireFoam, 1f, 1, null, null, 255, true, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x06007EEA RID: 32490 RVA: 0x002D5D76 File Offset: 0x002D3F76
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(target.Cell, this.Props.firefoamRadius);
		}

		// Token: 0x06007EEB RID: 32491 RVA: 0x002D5D90 File Offset: 0x002D3F90
		public override string CompInspectStringExtra()
		{
			if (!this.ShouldHaveInspectString)
			{
				return null;
			}
			if (this.parent.CanCast)
			{
				return "AbilityMechFirefoampopCharged".Translate();
			}
			return "AbilityMechFirefoampopRecharging".Translate(this.parent.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false));
		}
	}
}
