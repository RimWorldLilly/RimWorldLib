using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000237 RID: 567
	public class AnimalPenBalanceCalculator
	{
		// Token: 0x06001083 RID: 4227 RVA: 0x0005EFB9 File Offset: 0x0005D1B9
		public AnimalPenBalanceCalculator(Map map, bool considerInProgressMovement)
		{
			this.map = map;
			this.considerInProgressMovement = considerInProgressMovement;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0005EFE1 File Offset: 0x0005D1E1
		public void MarkDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0005EFEC File Offset: 0x0005D1EC
		public bool IsBetterPen(CompAnimalPenMarker markerA, CompAnimalPenMarker markerB, bool leavingMarkerB, Pawn animal)
		{
			this.RecalculateIfDirty();
			District district = markerA.parent.GetDistrict(RegionType.Set_Passable);
			District district2 = markerB.parent.GetDistrict(RegionType.Set_Passable);
			if (district == district2)
			{
				return false;
			}
			float bodySize = animal.BodySize;
			float num = this.TotalBodySizeIn(district) + bodySize;
			float num2 = this.TotalBodySizeIn(district2) + (leavingMarkerB ? (-bodySize) : bodySize);
			float num3 = num / (float)district.CellCount;
			float num4 = num2 / (float)district2.CellCount;
			return num3 * 1.2f < num4;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0005F060 File Offset: 0x0005D260
		public float TotalBodySizeIn(District district)
		{
			this.RecalculateIfDirty();
			float num = 0f;
			foreach (AnimalPenBalanceCalculator.AnimalMembershipInfo animalMembershipInfo in this.membership)
			{
				if (animalMembershipInfo.pen == district)
				{
					num += animalMembershipInfo.animal.BodySize;
				}
			}
			return num;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0005F0D0 File Offset: 0x0005D2D0
		private void RecalculateIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.membership.Clear();
			foreach (Pawn pawn in this.map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					District district = null;
					if (this.considerInProgressMovement && pawn.roping.IsRopedByPawn && pawn.roping.RopedByPawn.jobs.curDriver is JobDriver_RopeToPen)
					{
						Thing thing = pawn.roping.RopedByPawn.CurJob.GetTarget(TargetIndex.C).Thing;
						District district2 = (thing != null) ? thing.GetDistrict(RegionType.Set_Passable) : null;
						if (district2 != null && !district2.TouchesMapEdge)
						{
							district = district2;
						}
					}
					if (district == null)
					{
						CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
						district = ((currentPenOf != null) ? currentPenOf.parent.GetDistrict(RegionType.Set_Passable) : null);
					}
					this.membership.Add(new AnimalPenBalanceCalculator.AnimalMembershipInfo
					{
						animal = pawn,
						pen = district
					});
				}
			}
		}

		// Token: 0x04000F58 RID: 3928
		private const float DensityTolerance = 0.2f;

		// Token: 0x04000F59 RID: 3929
		private readonly Map map;

		// Token: 0x04000F5A RID: 3930
		private bool considerInProgressMovement;

		// Token: 0x04000F5B RID: 3931
		private readonly List<AnimalPenBalanceCalculator.AnimalMembershipInfo> membership = new List<AnimalPenBalanceCalculator.AnimalMembershipInfo>();

		// Token: 0x04000F5C RID: 3932
		private bool dirty = true;

		// Token: 0x020025B4 RID: 9652
		private struct AnimalMembershipInfo
		{
			// Token: 0x04009AA0 RID: 39584
			public Pawn animal;

			// Token: 0x04009AA1 RID: 39585
			public District pen;
		}
	}
}
