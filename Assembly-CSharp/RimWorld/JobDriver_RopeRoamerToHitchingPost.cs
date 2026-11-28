using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AB RID: 2475
	public class JobDriver_RopeRoamerToHitchingPost : JobDriver_RopeToDestination
	{
		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06004B08 RID: 19208 RVA: 0x001D2577 File Offset: 0x001D0777
		protected Building HitchingPost
		{
			get
			{
				return base.TargetThingB as Building;
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06004B09 RID: 19209 RVA: 0x001D2584 File Offset: 0x001D0784
		protected IntVec3 HitchingPostLoc
		{
			get
			{
				return this.HitchingPost.Position;
			}
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x001D2594 File Offset: 0x001D0794
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			if (!this.HitchingPostLoc.IsValid)
			{
				return false;
			}
			if (!this.pawn.Position.InHorDistOf(this.HitchingPostLoc, 2f))
			{
				return false;
			}
			District district = this.HitchingPostLoc.GetDistrict(this.pawn.Map, RegionType.Set_Passable);
			return district == this.pawn.GetDistrict(RegionType.Set_Passable) && district == ropee.GetDistrict(RegionType.Set_Passable);
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x001D260C File Offset: 0x001D080C
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			if (this.HitchingPostLoc.IsValid)
			{
				ropee.roping.RopeToSpot(this.HitchingPostLoc);
			}
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x001D263C File Offset: 0x001D083C
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			if (animal.roping.RopedByPawn == this.pawn)
			{
				return false;
			}
			string text;
			Building hitchingPostAnimalShouldBeTakenTo = AnimalPenUtility.GetHitchingPostAnimalShouldBeTakenTo(this.pawn, animal, out text, false);
			return hitchingPostAnimalShouldBeTakenTo != null && this.HitchingPost == hitchingPostAnimalShouldBeTakenTo && AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, animal, out text, false, true, false, true, this.job.ropingPriority, null) == null;
		}
	}
}
