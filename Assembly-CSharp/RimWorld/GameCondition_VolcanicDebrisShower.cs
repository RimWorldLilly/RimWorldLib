using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001154 RID: 4436
	public class GameCondition_VolcanicDebrisShower : GameCondition
	{
		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x06006F6F RID: 28527 RVA: 0x0026FADC File Offset: 0x0026DCDC
		public override bool Expired
		{
			get
			{
				return this.remainingLavaRockCount <= 0;
			}
		}

		// Token: 0x06006F70 RID: 28528 RVA: 0x0026FAEC File Offset: 0x0026DCEC
		public override void Init()
		{
			base.Init();
			this.nextLavaRockTick = Find.TickManager.TicksGame + GameCondition_VolcanicDebrisShower.IntervalRange.RandomInRange;
			this.remainingLavaRockCount = GameCondition_VolcanicDebrisShower.MeteoriteCount.RandomInRange;
		}

		// Token: 0x06006F71 RID: 28529 RVA: 0x0026FB30 File Offset: 0x0026DD30
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame < this.nextLavaRockTick)
			{
				return;
			}
			Map singleMap = base.SingleMap;
			IntVec3 pos;
			if (!GameCondition_VolcanicDebrisShower.TryFindCell(out pos, singleMap))
			{
				Log.Error("GameCondition_VolcanicDebrisShower could not find a suitable cell.");
				return;
			}
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.LavaRockIncoming, pos, singleMap);
			this.nextLavaRockTick = Find.TickManager.TicksGame + GameCondition_VolcanicDebrisShower.IntervalRange.RandomInRange;
			this.remainingLavaRockCount--;
		}

		// Token: 0x06006F72 RID: 28530 RVA: 0x0026FBA8 File Offset: 0x0026DDA8
		private static bool TryFindCell(out IntVec3 cell, Map map)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.LavaRockIncoming, map, TerrainAffordanceDefOf.Walkable, out cell, 10, default(IntVec3), -1, true, false, false, false, true, true, null);
		}

		// Token: 0x06006F73 RID: 28531 RVA: 0x0026FBD9 File Offset: 0x0026DDD9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.nextLavaRockTick, "nextLavaRockTick", 0, false);
			Scribe_Values.Look<int>(ref this.remainingLavaRockCount, "remainingLavaRockCount", 0, false);
		}

		// Token: 0x0400472D RID: 18221
		private static readonly IntRange MeteoriteCount = new IntRange(8, 12);

		// Token: 0x0400472E RID: 18222
		private static readonly IntRange IntervalRange = new IntRange(60, 180);

		// Token: 0x0400472F RID: 18223
		private int nextLavaRockTick;

		// Token: 0x04004730 RID: 18224
		private int remainingLavaRockCount;
	}
}
