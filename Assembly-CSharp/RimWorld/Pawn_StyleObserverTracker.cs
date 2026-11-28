using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A7 RID: 5543
	public class Pawn_StyleObserverTracker : IExposable
	{
		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x06008932 RID: 35122 RVA: 0x0031B26F File Offset: 0x0031946F
		public int StyleDominanceThoughtIndex
		{
			get
			{
				return this.styleDominanceThoughtIndex;
			}
		}

		// Token: 0x06008933 RID: 35123 RVA: 0x0031B277 File Offset: 0x00319477
		public Pawn_StyleObserverTracker()
		{
		}

		// Token: 0x06008934 RID: 35124 RVA: 0x0031B286 File Offset: 0x00319486
		public Pawn_StyleObserverTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008935 RID: 35125 RVA: 0x0031B29C File Offset: 0x0031949C
		private bool CellValid(IntVec3 cell)
		{
			return cell.InBounds(this.pawn.Map) && !cell.Fogged(this.pawn.Map) && GenSight.LineOfSight(this.pawn.Position, cell, this.pawn.Map);
		}

		// Token: 0x06008936 RID: 35126 RVA: 0x0031B2F0 File Offset: 0x003194F0
		public void StyleObserverTickInterval(int delta)
		{
			if (this.pawn.IsHashIntervalTick(900, delta) && ModsConfig.IdeologyActive && this.pawn.Ideo != null && this.pawn.Spawned)
			{
				int lastIndex = this.styleDominanceThoughtIndex;
				this.styleDominanceThoughtIndex = -1;
				if (this.pawn.Awake() && !PawnUtility.IsBiologicallyOrArtificiallyBlind(this.pawn))
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					if (((needs != null) ? needs.mood : null) != null)
					{
						Room room = this.pawn.GetRoom(RegionType.Set_All);
						IntVec3 center;
						if (room != null && CellFinder.TryFindRandomCellNear(this.pawn.Position, this.pawn.Map, 5, new Predicate<IntVec3>(this.CellValid), out center, -1))
						{
							float styleDominanceFromCellsCenteredOn = IdeoUtility.GetStyleDominanceFromCellsCenteredOn(center, this.pawn.Position, this.pawn.Map, this.pawn.Ideo);
							float pointsThreshold = (!room.IsDoorway && (float)room.CellCount < 10f) ? ((float)room.CellCount) : 10f;
							this.UpdateStyleDominanceThoughtIndex(styleDominanceFromCellsCenteredOn, pointsThreshold, lastIndex);
						}
					}
				}
			}
		}

		// Token: 0x06008937 RID: 35127 RVA: 0x0031B420 File Offset: 0x00319620
		private void UpdateStyleDominanceThoughtIndex(float styleDominance, float pointsThreshold, int lastIndex)
		{
			if (styleDominance >= pointsThreshold)
			{
				this.styleDominanceThoughtIndex = 0;
			}
			else if (styleDominance <= -pointsThreshold)
			{
				this.styleDominanceThoughtIndex = 1;
			}
			else
			{
				this.styleDominanceThoughtIndex = -1;
			}
			if (lastIndex != this.styleDominanceThoughtIndex)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}

		// Token: 0x06008938 RID: 35128 RVA: 0x0031B477 File Offset: 0x00319677
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.styleDominanceThoughtIndex, "styleDominanceThoughtIndex", -1, false);
		}

		// Token: 0x04005482 RID: 21634
		public Pawn pawn;

		// Token: 0x04005483 RID: 21635
		private int styleDominanceThoughtIndex = -1;

		// Token: 0x04005484 RID: 21636
		private const int StyleObservationCenterCellRadius = 5;

		// Token: 0x04005485 RID: 21637
		private const int StyleObservationInterval = 900;

		// Token: 0x04005486 RID: 21638
		private const float BaseDominancePointsThreshold = 10f;
	}
}
