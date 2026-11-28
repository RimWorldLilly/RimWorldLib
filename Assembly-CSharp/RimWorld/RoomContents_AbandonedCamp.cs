using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124E RID: 4686
	public class RoomContents_AbandonedCamp : RoomContentsWorker
	{
		// Token: 0x06007441 RID: 29761 RVA: 0x0028AE30 File Offset: 0x00289030
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(ThingDefOf.Bedroll, map, out loc, null, 3, 1, null, false))
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Bedroll, ThingDefOf.Cloth);
			GenSpawn.Spawn(thing, loc, map, Rot4.Random, WipeMode.Vanish, false, false);
			this.DropSpawnNear(ThingDefOf.MealSurvivalPack, thing.Position + IntVec3.East, map, RoomContents_AbandonedCamp.SurvivalPacksCountRange.RandomInRange);
			this.DropSpawnNear(ThingDefOf.TorchLamp, thing.Position, map, RoomContents_AbandonedCamp.TorchesCountRange.RandomInRange);
		}

		// Token: 0x06007442 RID: 29762 RVA: 0x0028AED0 File Offset: 0x002890D0
		private void DropSpawnNear(ThingDef thing, IntVec3 cell, Map map, int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				Thing thing2;
				GenDrop.TryDropSpawn(ThingMaker.MakeThing(thing, null), cell, map, ThingPlaceMode.Near, out thing2, null, null, true);
			}
		}

		// Token: 0x0400491C RID: 18716
		private static readonly IntRange SurvivalPacksCountRange = new IntRange(0, 1);

		// Token: 0x0400491D RID: 18717
		private static readonly IntRange TorchesCountRange = new IntRange(0, 1);
	}
}
