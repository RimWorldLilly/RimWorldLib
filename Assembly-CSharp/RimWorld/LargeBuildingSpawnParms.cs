using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E4B RID: 7755
	public struct LargeBuildingSpawnParms
	{
		// Token: 0x17002510 RID: 9488
		// (get) Token: 0x0600C915 RID: 51477 RVA: 0x0049DBB8 File Offset: 0x0049BDB8
		public IntVec2 Size
		{
			get
			{
				IntVec2? intVec = this.overrideSize;
				if (intVec == null)
				{
					return this.thingDef.size;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x0600C916 RID: 51478 RVA: 0x0049DBE8 File Offset: 0x0049BDE8
		public LargeBuildingSpawnParms ForThing(ThingDef thingDef)
		{
			LargeBuildingSpawnParms result = this;
			result.thingDef = thingDef;
			return result;
		}

		// Token: 0x04007288 RID: 29320
		public ThingDef thingDef;

		// Token: 0x04007289 RID: 29321
		public float minDistanceToColonyBuilding;

		// Token: 0x0400728A RID: 29322
		public float maxDistanceToColonyBuilding;

		// Token: 0x0400728B RID: 29323
		public float minDistanceFromUsedRects;

		// Token: 0x0400728C RID: 29324
		public float maxDistanceFromPlayerStartPosition;

		// Token: 0x0400728D RID: 29325
		public bool canSpawnOnImpassable;

		// Token: 0x0400728E RID: 29326
		public int minDistToEdge;

		// Token: 0x0400728F RID: 29327
		public SpawnLocationType attemptSpawnLocationType;

		// Token: 0x04007290 RID: 29328
		public bool attemptNotUnderBuildings;

		// Token: 0x04007291 RID: 29329
		public bool preferFarFromColony;

		// Token: 0x04007292 RID: 29330
		public bool ignoreTerrainAffordance;

		// Token: 0x04007293 RID: 29331
		public bool ignoreFoundations;

		// Token: 0x04007294 RID: 29332
		public bool allowFogged;

		// Token: 0x04007295 RID: 29333
		public IntVec2? overrideSize;

		// Token: 0x04007296 RID: 29334
		public static readonly LargeBuildingSpawnParms Default = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = 50f,
			canSpawnOnImpassable = true
		};
	}
}
