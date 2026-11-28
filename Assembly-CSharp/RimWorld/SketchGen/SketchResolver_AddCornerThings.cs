using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020C5 RID: 8389
	public class SketchResolver_AddCornerThings : SketchResolver
	{
		// Token: 0x0600D341 RID: 54081 RVA: 0x004DC1AC File Offset: 0x004DA3AC
		protected override void ResolveInt(SketchResolveParams parms)
		{
			this.wallPositions.Clear();
			for (int i = 0; i < parms.sketch.Things.Count; i++)
			{
				if (parms.sketch.Things[i].def == ThingDefOf.Wall)
				{
					this.wallPositions.Add(parms.sketch.Things[i].pos);
				}
			}
			bool allowWood = parms.allowWood.GetValueOrDefault(true);
			ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(parms.cornerThing, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, true, parms.cornerThing));
			bool valueOrDefault = parms.requireFloor.GetValueOrDefault();
			try
			{
				foreach (IntVec3 intVec in this.wallPositions)
				{
					if (Rand.Chance(0.09f))
					{
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z - 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z)) && (!valueOrDefault || (parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, stuff, 1, null, null, false, 2f);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z - 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x, 0, intVec.z - 1)) && (!valueOrDefault || (parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z - 1)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z - 1)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x, 0, intVec.z - 1), Rot4.North, stuff, 1, null, null, false, 2f);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z + 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x, 0, intVec.z + 1)) && (!valueOrDefault || (parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z + 1)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z + 1)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x, 0, intVec.z + 1), Rot4.North, stuff, 1, null, null, false, 2f);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z + 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z)) && (!valueOrDefault || (parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, stuff, 1, null, null, false, 2f);
						}
					}
				}
			}
			finally
			{
				this.wallPositions.Clear();
			}
		}

		// Token: 0x0600D342 RID: 54082 RVA: 0x004DC6F8 File Offset: 0x004DA8F8
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x040085DE RID: 34270
		private HashSet<IntVec3> wallPositions = new HashSet<IntVec3>();

		// Token: 0x040085DF RID: 34271
		private const float Chance = 0.09f;
	}
}
