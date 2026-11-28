using System;
using RimWorld.Utility;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FE9 RID: 8169
	public class Verb_Jump : Verb
	{
		// Token: 0x17002554 RID: 9556
		// (get) Token: 0x0600CF1D RID: 53021 RVA: 0x004C460C File Offset: 0x004C280C
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

		// Token: 0x17002555 RID: 9557
		// (get) Token: 0x0600CF1E RID: 53022 RVA: 0x004C465A File Offset: 0x004C285A
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600CF1F RID: 53023 RVA: 0x004C4660 File Offset: 0x004C2860
		protected override bool TryCastShot()
		{
			return JumpUtility.DoJump(this.CasterPawn, this.currentTarget, base.ReloadableCompSource, this.verbProps, null, default(LocalTargetInfo), null);
		}

		// Token: 0x0600CF20 RID: 53024 RVA: 0x004C4695 File Offset: 0x004C2895
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			JumpUtility.OrderJump(this.CasterPawn, target, this, this.EffectiveRange);
		}

		// Token: 0x0600CF21 RID: 53025 RVA: 0x004C46AC File Offset: 0x004C28AC
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return this.caster != null && this.CanHitTarget(target) && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, target.Cell) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x0600CF22 RID: 53026 RVA: 0x004C4704 File Offset: 0x004C2904
		public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			return JumpUtility.CanHitTargetFrom(this.CasterPawn, root, targ, this.EffectiveRange);
		}

		// Token: 0x0600CF23 RID: 53027 RVA: 0x004C4719 File Offset: 0x004C2919
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, target.Cell))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x0600CF24 RID: 53028 RVA: 0x004C4758 File Offset: 0x004C2958
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (this.caster != null && !this.caster.Spawned)
			{
				return;
			}
			if (target.IsValid && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, target.Cell))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
			}
			GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.caster.Position, c, this.caster.Map) && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, c));
		}

		// Token: 0x040083C0 RID: 33728
		private float cachedEffectiveRange = -1f;
	}
}
