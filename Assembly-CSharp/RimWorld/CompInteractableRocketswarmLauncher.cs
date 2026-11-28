using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020018D0 RID: 6352
	public class CompInteractableRocketswarmLauncher : CompInteractable
	{
		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x06009F4B RID: 40779 RVA: 0x00391FB7 File Offset: 0x003901B7
		private Building_TurretGun ParentGun
		{
			get
			{
				return (Building_TurretGun)this.parent;
			}
		}

		// Token: 0x06009F4C RID: 40780 RVA: 0x00391FC4 File Offset: 0x003901C4
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (base.ValidateTarget(target, false))
			{
				this.TargetLocation(target.Pawn);
			}
		}

		// Token: 0x06009F4D RID: 40781 RVA: 0x00391FDD File Offset: 0x003901DD
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompInteractableRocketswarmLauncher.<CompFloatMenuOptions>d__3 <CompFloatMenuOptions>d__ = new CompInteractableRocketswarmLauncher.<CompFloatMenuOptions>d__3(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x06009F4E RID: 40782 RVA: 0x00391FF4 File Offset: 0x003901F4
		private void TargetLocation(Pawn caster)
		{
			Find.Targeter.BeginTargeting(this.ParentGun.AttackVerb, null, true, null, delegate()
			{
				if (this.ParentGun.ForcedTarget.IsValid)
				{
					Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent);
					caster.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}
			}, true);
		}

		// Token: 0x06009F4F RID: 40783 RVA: 0x0039203C File Offset: 0x0039023C
		protected override bool TryInteractTick()
		{
			this.ParentGun.TryActivateBurst();
			if (this.ParentGun.CurrentTarget.IsValid)
			{
				this.ParentGun.Top.ForceFaceTarget(this.ParentGun.CurrentTarget);
				return true;
			}
			return true;
		}

		// Token: 0x06009F50 RID: 40784 RVA: 0x00392088 File Offset: 0x00390288
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (activateBy != null && activateBy.WorkTagIsDisabled(WorkTags.Violent))
			{
				return "IsIncapableOfViolence".Translate(activateBy.LabelShort, activateBy);
			}
			return true;
		}
	}
}
