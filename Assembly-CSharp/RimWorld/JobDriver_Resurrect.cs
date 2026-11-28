using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000A6E RID: 2670
	public class JobDriver_Resurrect : JobDriver
	{
		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x0600508B RID: 20619 RVA: 0x001E46A0 File Offset: 0x001E28A0
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x0600508C RID: 20620 RVA: 0x001E46C8 File Offset: 0x001E28C8
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x001E46EC File Offset: 0x001E28EC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x001E473F File Offset: 0x001E293F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Resurrect.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_Resurrect.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x001E4750 File Offset: 0x001E2950
		private void Resurrect()
		{
			Pawn innerPawn = this.Corpse.InnerPawn;
			CompTargetEffect_Resurrect compTargetEffect_Resurrect = this.Item.TryGetComp<CompTargetEffect_Resurrect>();
			bool flag = true;
			if (!compTargetEffect_Resurrect.Props.withSideEffects)
			{
				if (!ResurrectionUtility.TryResurrect(innerPawn, null))
				{
					flag = false;
				}
			}
			else if (!ResurrectionUtility.TryResurrectWithSideEffects(innerPawn))
			{
				flag = false;
			}
			if (flag)
			{
				SoundDefOf.MechSerumUsed.PlayOneShot(SoundInfo.InMap(innerPawn, MaintenanceType.None));
				Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
				if (compTargetEffect_Resurrect.Props.moteDef != null)
				{
					MoteMaker.MakeAttachedOverlay(innerPawn, compTargetEffect_Resurrect.Props.moteDef, Vector3.zero, 1f, -1f);
				}
				if (compTargetEffect_Resurrect.Props.addsHediff != null)
				{
					innerPawn.health.AddHediff(compTargetEffect_Resurrect.Props.addsHediff, null, null, null);
				}
			}
			this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
		}

		// Token: 0x04003149 RID: 12617
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x0400314A RID: 12618
		private const TargetIndex ItemInd = TargetIndex.B;

		// Token: 0x0400314B RID: 12619
		private const int DurationTicks = 600;

		// Token: 0x0400314C RID: 12620
		private Mote warmupMote;
	}
}
