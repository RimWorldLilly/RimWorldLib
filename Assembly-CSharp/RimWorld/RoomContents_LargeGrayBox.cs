using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001255 RID: 4693
	public class RoomContents_LargeGrayBox : RoomContents_GrayBox
	{
		// Token: 0x170013CE RID: 5070
		// (get) Token: 0x06007457 RID: 29783 RVA: 0x0028B2DB File Offset: 0x002894DB
		protected virtual int MaxCrates { get; } = 20;

		// Token: 0x06007458 RID: 29784 RVA: 0x0028B2E4 File Offset: 0x002894E4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			this.placedCrates = 0;
			for (int i = 0; i < this.MaxCrates; i++)
			{
				IntVec3 cell;
				if (room.TryGetRandomCellInRoom(ThingDefOf.GrayBox, map, out cell, null, 1, 1, null, false))
				{
					this.SpawnBox(cell, map);
				}
			}
		}

		// Token: 0x06007459 RID: 29785 RVA: 0x0028B33C File Offset: 0x0028953C
		protected virtual void SpawnBox(IntVec3 cell, Map map)
		{
			ThingSetMakerDef rewardMaker = ThingSetMakerDefOf.Reward_GrayBoxLowReward;
			bool addRewards = true;
			if (this.placedCrates < 1)
			{
				rewardMaker = ThingSetMakerDefOf.Reward_GrayBox;
			}
			else if (Rand.Chance(0.5f))
			{
				addRewards = false;
			}
			this.placedCrates++;
			RoomContents_GrayBox.SpawnBoxInRoom(cell, map, rewardMaker, addRewards);
		}

		// Token: 0x04004922 RID: 18722
		private const int NormalCrates = 1;

		// Token: 0x04004923 RID: 18723
		private const float ChanceEmpty = 0.5f;

		// Token: 0x04004925 RID: 18725
		private int placedCrates;
	}
}
