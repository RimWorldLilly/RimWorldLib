using System;
using RimWorld;
using RimWorld.Utility;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200080F RID: 2063
	public abstract class Verb_CastBase : Verb
	{
		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x060040FE RID: 16638 RVA: 0x0019D048 File Offset: 0x0019B248
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x0019D04B File Offset: 0x0019B24B
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, true) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x0019D070 File Offset: 0x0019B270
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStatic, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x0019D0AC File Offset: 0x0019B2AC
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.verbProps.targetParams.CanTarget(target.ToTargetInfo(this.caster.Map), null))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x0019D0FC File Offset: 0x0019B2FC
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && this.CanHitTarget(target))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
				base.DrawHighlightFieldRadiusAroundTarget(target);
			}
			GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange, Color.white, (IntVec3 c) => this.CanHitTarget(c));
		}
	}
}
