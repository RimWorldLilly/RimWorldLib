using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009FA RID: 2554
	public class JobDriver_PlayMusicalInstrument : JobDriver_SitFacingBuilding
	{
		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06004D4D RID: 19789 RVA: 0x001DA158 File Offset: 0x001D8358
		public Building_MusicalInstrument MusicalInstrument
		{
			get
			{
				return (Building_MusicalInstrument)((Thing)this.job.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x001DA170 File Offset: 0x001D8370
		protected override void ModifyPlayToil(Toil toil)
		{
			ModLister.CheckRoyaltyOrIdeology("Instrument");
			base.ModifyPlayToil(toil);
			Action<int> <>9__2;
			toil.AddPreInitAction(delegate
			{
				Building_MusicalInstrument musicalInstrument = this.MusicalInstrument;
				if (musicalInstrument != null)
				{
					musicalInstrument.StartPlaying(this.pawn);
				}
				Toil toil2 = toil;
				Action<int> tickIntervalAction;
				if ((tickIntervalAction = <>9__2) == null)
				{
					tickIntervalAction = (<>9__2 = delegate(int delta)
					{
						toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(this.TargetC.IsValid ? TargetIndex.C : TargetIndex.A));
						this.pawn.GainComfortFromCellIfPossible(delta, false);
						JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, this.MusicalInstrument);
					});
				}
				toil2.tickIntervalAction = tickIntervalAction;
			});
			toil.handlingFacing = true;
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.AddFinishAction(delegate
			{
				this.MusicalInstrument.StopPlaying();
			});
		}
	}
}
