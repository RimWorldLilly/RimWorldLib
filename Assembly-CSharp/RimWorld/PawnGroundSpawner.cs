using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFE RID: 6910
	public class PawnGroundSpawner : GroundSpawner, IThingHolder
	{
		// Token: 0x17001FDE RID: 8158
		// (get) Token: 0x0600B0B8 RID: 45240 RVA: 0x003E46B8 File Offset: 0x003E28B8
		protected override IntRange ResultSpawnDelay
		{
			get
			{
				return this.spawnDelay;
			}
		}

		// Token: 0x0600B0B9 RID: 45241 RVA: 0x003E46C0 File Offset: 0x003E28C0
		public PawnGroundSpawner()
		{
			this.dustMoteSpawnMTB = 0.5f;
			this.filthSpawnMTB = 0.5f;
			this.filthSpawnRadius = 1f;
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Deep, true);
		}

		// Token: 0x0600B0BA RID: 45242 RVA: 0x003E46F8 File Offset: 0x003E28F8
		public void Init(Pawn pawnToSpawn, IntRange delayTicksRange)
		{
			this.spawnDelay = delayTicksRange;
			this.innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			this.innerContainer.TryAdd(pawnToSpawn, true);
		}

		// Token: 0x0600B0BB RID: 45243 RVA: 0x003E471B File Offset: 0x003E291B
		protected override void Spawn(Map map, IntVec3 loc)
		{
			this.innerContainer.TryDropAll(loc, map, ThingPlaceMode.Direct, null, null, false);
		}

		// Token: 0x0600B0BC RID: 45244 RVA: 0x003E472F File Offset: 0x003E292F
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600B0BD RID: 45245 RVA: 0x003E473D File Offset: 0x003E293D
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600B0BE RID: 45246 RVA: 0x003E4748 File Offset: 0x003E2948
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.spawnDelay, "spawnDelay", default(IntRange), false);
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x040065FB RID: 26107
		private IntRange spawnDelay;

		// Token: 0x040065FC RID: 26108
		private ThingOwner<Pawn> innerContainer;
	}
}
