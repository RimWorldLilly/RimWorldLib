using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001863 RID: 6243
	public class CompDisableUnnaturalCorpse : CompInteractable
	{
		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x06009C39 RID: 39993 RVA: 0x003841FC File Offset: 0x003823FC
		public UnnaturalCorpse Corpse
		{
			get
			{
				return (UnnaturalCorpse)this.parent;
			}
		}

		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x06009C3A RID: 39994 RVA: 0x00384209 File Offset: 0x00382409
		public new CompProperties_DisableUnnaturalCorpse Props
		{
			get
			{
				return (CompProperties_DisableUnnaturalCorpse)this.props;
			}
		}

		// Token: 0x06009C3B RID: 39995 RVA: 0x00384216 File Offset: 0x00382416
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.OrderActivation(target.Pawn);
		}

		// Token: 0x06009C3C RID: 39996 RVA: 0x00384225 File Offset: 0x00382425
		protected override void OnInteracted(Pawn caster)
		{
			this.Corpse.DoStudiedDeactivation(caster);
		}

		// Token: 0x06009C3D RID: 39997 RVA: 0x00384233 File Offset: 0x00382433
		public override string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06009C3E RID: 39998 RVA: 0x00384236 File Offset: 0x00382436
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompDisableUnnaturalCorpse.<CompFloatMenuOptions>d__7 <CompFloatMenuOptions>d__ = new CompDisableUnnaturalCorpse.<CompFloatMenuOptions>d__7(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x06009C3F RID: 39999 RVA: 0x0038424D File Offset: 0x0038244D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompDisableUnnaturalCorpse.<CompGetGizmosExtra>d__8 <CompGetGizmosExtra>d__ = new CompDisableUnnaturalCorpse.<CompGetGizmosExtra>d__8(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009C40 RID: 40000 RVA: 0x00384260 File Offset: 0x00382460
		private void OrderActivation(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent);
			job.count = 1;
			job.playerForced = true;
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
