using System;
using RimWorld.Utility;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001479 RID: 5241
	public class Verb_CastAbilityJump : Verb_CastAbility
	{
		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x0600809B RID: 32923 RVA: 0x002DDE12 File Offset: 0x002DC012
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x0600809C RID: 32924 RVA: 0x002DDE15 File Offset: 0x002DC015
		public virtual ThingDef JumpFlyerDef
		{
			get
			{
				return ThingDefOf.PawnFlyer;
			}
		}

		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x0600809D RID: 32925 RVA: 0x002DDE1C File Offset: 0x002DC01C
		public override float EffectiveRange
		{
			get
			{
				if (this.cachedEffectiveRange < 0f)
				{
					if (base.EquipmentSource != null)
					{
						this.cachedEffectiveRange = base.EquipmentSource.GetStatValue(StatDefOf.JumpRange, true, -1);
					}
					else
					{
						this.cachedEffectiveRange = base.EffectiveRange;
					}
				}
				return this.cachedEffectiveRange;
			}
		}

		// Token: 0x0600809E RID: 32926 RVA: 0x002DDE6A File Offset: 0x002DC06A
		protected override bool TryCastShot()
		{
			return base.TryCastShot() && JumpUtility.DoJump(this.CasterPawn, this.currentTarget, base.ReloadableCompSource, this.verbProps, this.ability, base.CurrentTarget, this.JumpFlyerDef);
		}

		// Token: 0x0600809F RID: 32927 RVA: 0x002DDEA5 File Offset: 0x002DC0A5
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && JumpUtility.ValidJumpTarget(this.CasterPawn, this.caster.Map, target.Cell))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x060080A0 RID: 32928 RVA: 0x002DDEE1 File Offset: 0x002DC0E1
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			JumpUtility.OrderJump(this.CasterPawn, target, this, this.EffectiveRange);
		}

		// Token: 0x060080A1 RID: 32929 RVA: 0x002DDEF8 File Offset: 0x002DC0F8
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return this.caster != null && this.CanHitTarget(target) && JumpUtility.ValidJumpTarget(this.CasterPawn, this.caster.Map, target.Cell) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x060080A2 RID: 32930 RVA: 0x002DDF50 File Offset: 0x002DC150
		public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			return JumpUtility.CanHitTargetFrom(this.CasterPawn, root, targ, this.EffectiveRange);
		}

		// Token: 0x060080A3 RID: 32931 RVA: 0x002DDF68 File Offset: 0x002DC168
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && JumpUtility.ValidJumpTarget(this.CasterPawn, this.caster.Map, target.Cell))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
			}
			GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.caster.Position, c, this.caster.Map) && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, c));
		}

		// Token: 0x0400502D RID: 20525
		private float cachedEffectiveRange = -1f;
	}
}
