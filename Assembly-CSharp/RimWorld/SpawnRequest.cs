using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200120D RID: 4621
	public class SpawnRequest : IExposable
	{
		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x060072C6 RID: 29382 RVA: 0x00282233 File Offset: 0x00280433
		public int PreEffecterTick
		{
			get
			{
				return this.startedTick + this.initialDelay + this.preSpawnEffecterOffsetTicks;
			}
		}

		// Token: 0x060072C7 RID: 29383 RVA: 0x00282249 File Offset: 0x00280449
		public SpawnRequest()
		{
		}

		// Token: 0x060072C8 RID: 29384 RVA: 0x0028225C File Offset: 0x0028045C
		public SpawnRequest(List<Thing> thingsToSpawn, List<IntVec3> spawnPositions, int batchSize, float intervalSeconds, Lord lord = null)
		{
			this.thingsToSpawn = thingsToSpawn;
			this.unspawnedThings = thingsToSpawn.ToList<Thing>();
			this.spawnPositions = spawnPositions;
			this.batchSize = batchSize;
			this.lord = lord;
			this.intervalTicks = intervalSeconds.SecondsToTicks();
			this.startedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060072C9 RID: 29385 RVA: 0x002822C0 File Offset: 0x002804C0
		public SpawnRequest(List<Thing> thingsToSpawn, int batchSize, float intervalSeconds, Lord lord = null) : this(thingsToSpawn, (from t in thingsToSpawn
		select t.Position).ToList<IntVec3>(), batchSize, intervalSeconds, lord)
		{
		}

		// Token: 0x060072CA RID: 29386 RVA: 0x002822F8 File Offset: 0x002804F8
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.unspawnedThings, "unspawnedThings", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsToSpawn, "thingsToSpawn", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.spawnPositions, "spawnPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.spawnedThings, "spawnedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<DeferredSpawnWorker>(ref this.spawnWorker, "spawnWorker", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.batchSize, "batchSize", 0, false);
			Scribe_Values.Look<int>(ref this.intervalTicks, "intervalTicks", 0, false);
			Scribe_Values.Look<int>(ref this.initialDelay, "initialDelay", 0, false);
			Scribe_Values.Look<int>(ref this.startedTick, "startedTick", 0, false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Values.Look<int>(ref this.preSpawnEffecterOffsetTicks, "effecterOffsetTicks", 0, false);
		}

		// Token: 0x0400488F RID: 18575
		public List<Thing> thingsToSpawn;

		// Token: 0x04004890 RID: 18576
		public List<Thing> spawnedThings = new List<Thing>();

		// Token: 0x04004891 RID: 18577
		public List<Thing> unspawnedThings;

		// Token: 0x04004892 RID: 18578
		public List<IntVec3> spawnPositions;

		// Token: 0x04004893 RID: 18579
		public DeferredSpawnWorker spawnWorker;

		// Token: 0x04004894 RID: 18580
		public Lord lord;

		// Token: 0x04004895 RID: 18581
		public int batchSize;

		// Token: 0x04004896 RID: 18582
		public int intervalTicks;

		// Token: 0x04004897 RID: 18583
		public int initialDelay;

		// Token: 0x04004898 RID: 18584
		public int startedTick;

		// Token: 0x04004899 RID: 18585
		public bool done;

		// Token: 0x0400489A RID: 18586
		public SoundDef spawnSound;

		// Token: 0x0400489B RID: 18587
		public EffecterDef spawnEffect;

		// Token: 0x0400489C RID: 18588
		public EffecterDef preSpawnEffect;

		// Token: 0x0400489D RID: 18589
		public int preSpawnEffecterOffsetTicks;
	}
}
