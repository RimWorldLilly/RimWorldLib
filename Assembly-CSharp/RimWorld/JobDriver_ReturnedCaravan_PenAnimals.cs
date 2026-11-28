using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009AE RID: 2478
	public class JobDriver_ReturnedCaravan_PenAnimals : JobDriver_RopeToDestination
	{
		// Token: 0x06004B18 RID: 19224 RVA: 0x001D2804 File Offset: 0x001D0A04
		protected override bool UpdateDestination()
		{
			this.tmpRopees.Clear();
			this.tmpRopees.AddRange(this.pawn.roping.Ropees);
			bool result;
			try
			{
				IntVec3 cell = this.job.GetTarget(TargetIndex.B).Cell;
				District district = cell.IsValid ? cell.GetDistrict(base.Map, RegionType.Set_Passable) : null;
				IntVec3 c = IntVec3.Invalid;
				Thing t = null;
				foreach (Pawn pawn in this.tmpRopees)
				{
					string text;
					CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, pawn, out text, false, true, true, true, RopingPriority.Closest, null);
					if (penAnimalShouldBeTakenTo == null)
					{
						this.pawn.roping.DropRope(pawn);
					}
					else
					{
						if (penAnimalShouldBeTakenTo.parent.GetDistrict(RegionType.Set_Passable) == district)
						{
							return false;
						}
						if (!c.IsValid)
						{
							t = penAnimalShouldBeTakenTo.parent;
							c = penAnimalShouldBeTakenTo.parent.Position;
						}
					}
				}
				if (c.IsValid)
				{
					this.job.SetTarget(TargetIndex.B, c);
					this.job.SetTarget(TargetIndex.C, t);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			finally
			{
				this.tmpRopees.Clear();
			}
			return result;
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x001D2980 File Offset: 0x001D0B80
		private bool PenIsEnclosed()
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.C);
			if (!target.HasThing)
			{
				return false;
			}
			ThingWithComps thingWithComps = target.Thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return false;
			}
			CompAnimalPenMarker comp = thingWithComps.GetComp<CompAnimalPenMarker>();
			return comp != null && comp.PenState.Enclosed;
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x001D29CE File Offset: 0x001D0BCE
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			if (this.job.ropeToUnenclosedPens && !this.PenIsEnclosed())
			{
				return roperWaitingAtDest;
			}
			return AnimalPenUtility.GetCurrentPenOf(ropee, this.job.ropeToUnenclosedPens) != null;
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x001D29FC File Offset: 0x001D0BFC
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			Lord lord = ropee.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_PawnLost(ropee, PawnLostCondition.LeftVoluntarily, null);
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x001D2A24 File Offset: 0x001D0C24
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			return false;
		}

		// Token: 0x04002FB4 RID: 12212
		public const TargetIndex DestPenInd = TargetIndex.C;

		// Token: 0x04002FB5 RID: 12213
		private List<Pawn> tmpRopees = new List<Pawn>();
	}
}
