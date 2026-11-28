using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009A9 RID: 2473
	public class JobDriver_RopeToPen : JobDriver_RopeToDestination
	{
		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06004AFA RID: 19194 RVA: 0x001D244C File Offset: 0x001D064C
		private Thing DestinationThing
		{
			get
			{
				return this.job.targetC.Thing;
			}
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06004AFB RID: 19195 RVA: 0x001D245E File Offset: 0x001D065E
		protected CompAnimalPenMarker DestinationMarker
		{
			get
			{
				return this.DestinationThing.TryGetComp<CompAnimalPenMarker>();
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06004AFC RID: 19196 RVA: 0x001D246B File Offset: 0x001D066B
		private District DestinationDistrict
		{
			get
			{
				return this.DestinationThing.GetDistrict(RegionType.Set_Passable);
			}
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x001D247A File Offset: 0x001D067A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.C);
			return base.MakeNewToils();
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x001D248C File Offset: 0x001D068C
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			PenMarkerState penState = this.DestinationMarker.PenState;
			return !penState.Enclosed || penState.ContainsConnectedRegion(ropee.GetRegion(RegionType.Set_Passable));
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x001D24BD File Offset: 0x001D06BD
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x001D24C0 File Offset: 0x001D06C0
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			if (animal.roping.RopedByPawn == this.pawn)
			{
				return false;
			}
			string text;
			CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, animal, out text, false, true, this.job.ropeToUnenclosedPens, true, this.job.ropingPriority, null);
			return penAnimalShouldBeTakenTo != null && this.ShouldOpportunisticallyRopeAnimal(animal, penAnimalShouldBeTakenTo);
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x001D2518 File Offset: 0x001D0718
		protected virtual bool ShouldOpportunisticallyRopeAnimal(Pawn animal, CompAnimalPenMarker targetPenMarker)
		{
			return targetPenMarker.parent.GetDistrict(RegionType.Set_Passable) == this.DestinationDistrict;
		}

		// Token: 0x04002FB3 RID: 12211
		public const TargetIndex DestMarkerInd = TargetIndex.C;
	}
}
