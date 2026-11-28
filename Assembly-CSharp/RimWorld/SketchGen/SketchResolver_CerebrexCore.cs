using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020CB RID: 8395
	public class SketchResolver_CerebrexCore : SketchResolver
	{
		// Token: 0x0600D355 RID: 54101 RVA: 0x004DD71C File Offset: 0x004DB91C
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null;
		}

		// Token: 0x0600D356 RID: 54102 RVA: 0x004DD728 File Offset: 0x004DB928
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckOdyssey("Cerebrex core"))
			{
				return;
			}
			Sketch sketch = new Sketch();
			sketch.AddThing(ThingDefOf.CerebrexCore, new IntVec3(0, 0, 0), Rot4.North, null, 1, null, null, true, 2f);
			for (int i = 0; i < 61; i++)
			{
				for (int j = 0; j < 61; j++)
				{
					IntVec3 pos = new IntVec3(i - 30, 0, j - 30);
					if (pos.LengthManhattan <= 19)
					{
						sketch.AddTerrain(TerrainDefOf.AncientTile, pos, true);
					}
				}
			}
			for (int k = 0; k < 61; k++)
			{
				sketch.AddTerrain(TerrainDefOf.AncientTile, new IntVec3(k - 30, 0, -1), true);
				sketch.AddTerrain(TerrainDefOf.AncientTile, new IntVec3(k - 30, 0, 0), true);
				sketch.AddTerrain(TerrainDefOf.AncientTile, new IntVec3(k - 30, 0, 1), true);
				sketch.AddTerrain(TerrainDefOf.AncientTile, new IntVec3(-1, 0, k - 30), true);
				sketch.AddTerrain(TerrainDefOf.AncientTile, new IntVec3(0, 0, k - 30), true);
				sketch.AddTerrain(TerrainDefOf.AncientTile, new IntVec3(1, 0, k - 30), true);
			}
			sketch.AddPrefab(PrefabDefOf.CerebrexCore, new IntVec3(0, 0, 0), Rot4.North);
			foreach (Rot4 dir in Rot4.AllRotations)
			{
				SketchResolver_CerebrexCore.GenerateBuilding(sketch, dir);
				SketchResolver_CerebrexCore.AddBuildingPathway(sketch, dir);
			}
			parms.sketch.Merge(sketch, true);
		}

		// Token: 0x0600D357 RID: 54103 RVA: 0x004DD8E8 File Offset: 0x004DBAE8
		private static int GenerateBuilding(Sketch sketch, Rot4 dir)
		{
			int num = Mathf.FloorToInt(30.5f);
			int num2 = num - Mathf.CeilToInt(1.5f);
			for (int i = 0; i < num2; i++)
			{
				sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - i, 0, num).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
				sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num, 0, num - i).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
				if (i < 7)
				{
					sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - i, 0, 2).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
					sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(2, 0, num - i).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
				}
				if (i >= 6)
				{
					sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - i, 0, num - 7 + 1).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
					sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - 7 + 1, 0, num - i).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
				}
				for (int j = 1; j < 6; j++)
				{
					sketch.AddTerrain(TerrainDefOf.Space, new IntVec3(num - i, 0, num - j).RotatedBy(dir), true);
					sketch.AddTerrain(TerrainDefOf.Space, new IntVec3(num - j, 0, num - i).RotatedBy(dir), true);
				}
			}
			for (int k = -2; k <= 2; k++)
			{
				sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(k, 0, num + 4).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
			}
			return num;
		}

		// Token: 0x0600D358 RID: 54104 RVA: 0x004DDB3C File Offset: 0x004DBD3C
		private static void AddBuildingPathway(Sketch sketch, Rot4 dir)
		{
			int num = Mathf.FloorToInt(30.5f);
			int num2 = 0;
			sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - 2, 0, num - 2).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
			int num3 = Rand.Range(14, 21);
			Rot4 rot = dir.Rotated(RotationDirection.Counterclockwise);
			for (int i = 0; i < num3; i++)
			{
				sketch.AddTerrain(TerrainDefOf.MechanoidPlatform, new IntVec3(num - 3 - i, 0, num - 3).RotatedBy(dir), true);
				sketch.AddTerrain(TerrainDefOf.MechanoidPlatform, new IntVec3(num - 3 - i, 0, num - 2).RotatedBy(dir), true);
				sketch.AddTerrain(TerrainDefOf.MechanoidPlatform, new IntVec3(num - 3 - i, 0, num - 4).RotatedBy(dir), true);
				sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - 3 - i, 0, num - 2).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
				if (i != 0)
				{
					sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - 3 - i, 0, num - 4).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
					if (i - num2 > 5 && i < num3 - 5 && Rand.Chance(0.05f))
					{
						num2 = i;
						SketchResolver_CerebrexCore.AddConnection(sketch, new IntVec3(num - 3 - i, 0, num - 3).RotatedBy(dir), rot.Rotated(Rand.Bool ? RotationDirection.Clockwise : RotationDirection.Counterclockwise));
					}
				}
			}
			sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - 3 - num3 + 1, 0, num - 3).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
			Rot4 direction = rot.Rotated(Rand.Bool ? RotationDirection.Clockwise : RotationDirection.Counterclockwise);
			SketchResolver_CerebrexCore.AddConnection(sketch, new IntVec3(num - 3 - num3 + 2, 0, num - 3).RotatedBy(dir), direction);
			if (Rand.Chance(0.25f))
			{
				SketchResolver_CerebrexCore.AddConnection(sketch, new IntVec3(num - 3 - num3 + 2, 0, num - 3).RotatedBy(dir), direction.Opposite);
			}
			num3 = Rand.Range(14, 21);
			rot = dir.Rotated(RotationDirection.Opposite);
			for (int j = 0; j < num3; j++)
			{
				sketch.AddTerrain(TerrainDefOf.MechanoidPlatform, new IntVec3(num - 3, 0, num - 3 - j).RotatedBy(dir), true);
				sketch.AddTerrain(TerrainDefOf.MechanoidPlatform, new IntVec3(num - 2, 0, num - 3 - j).RotatedBy(dir), true);
				sketch.AddTerrain(TerrainDefOf.MechanoidPlatform, new IntVec3(num - 4, 0, num - 3 - j).RotatedBy(dir), true);
				sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - 2, 0, num - 3 - j).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
				if (j != 0)
				{
					sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - 4, 0, num - 3 - j).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
					if (j - num2 > 5 && j < num3 - 5 && Rand.Chance(0.05f))
					{
						num2 = j;
						SketchResolver_CerebrexCore.AddConnection(sketch, new IntVec3(num - 3, 0, num - 3 - j).RotatedBy(dir), rot.Rotated(Rand.Bool ? RotationDirection.Clockwise : RotationDirection.Counterclockwise));
					}
				}
			}
			sketch.AddThing(ThingDefOf.AncientFortifiedWall, new IntVec3(num - 3, 0, num - 3 - num3 + 1).RotatedBy(dir), Rot4.North, null, 1, null, null, true, 2f);
			direction = rot.Rotated(Rand.Bool ? RotationDirection.Clockwise : RotationDirection.Counterclockwise);
			SketchResolver_CerebrexCore.AddConnection(sketch, new IntVec3(num - 3, 0, num - 3 - num3 + 2).RotatedBy(dir), direction);
			if (Rand.Chance(0.25f))
			{
				SketchResolver_CerebrexCore.AddConnection(sketch, new IntVec3(num - 3, 0, num - 3 - num3 + 2).RotatedBy(dir), direction.Opposite);
			}
		}

		// Token: 0x0600D359 RID: 54105 RVA: 0x004DDFA8 File Offset: 0x004DC1A8
		private static void AddConnection(Sketch sketch, IntVec3 rootCell, Rot4 direction)
		{
			for (int i = 1; i < 4; i++)
			{
				IntVec3 intVec = rootCell + direction.AsIntVec3 * i;
				sketch.Remove(sketch.ThingsAt(intVec).FirstOrDefault<SketchThing>());
				sketch.AddTerrain(TerrainDefOf.MechanoidPlatform, intVec, true);
				sketch.AddTerrain(TerrainDefOf.MechanoidPlatform, intVec + direction.Rotated(RotationDirection.Clockwise).AsIntVec3, true);
				sketch.AddTerrain(TerrainDefOf.MechanoidPlatform, intVec + direction.Rotated(RotationDirection.Counterclockwise).AsIntVec3, true);
				sketch.AddThing(ThingDefOf.AncientFortifiedWall, intVec + direction.Rotated(RotationDirection.Clockwise).AsIntVec3, Rot4.North, null, 1, null, null, true, 2f);
				sketch.AddThing(ThingDefOf.AncientFortifiedWall, intVec + direction.Rotated(RotationDirection.Counterclockwise).AsIntVec3, Rot4.North, null, 1, null, null, true, 2f);
			}
			sketch.AddThing(ThingDefOf.AncientBlastDoor, rootCell + direction.AsIntVec3 * 3, direction, null, 1, null, null, true, 2f);
		}

		// Token: 0x040085E9 RID: 34281
		public const int Size = 61;

		// Token: 0x040085EA RID: 34282
		private const int DiamondRadius = 19;

		// Token: 0x040085EB RID: 34283
		private const int MiddlePathWidth = 3;

		// Token: 0x040085EC RID: 34284
		private const int BuildingWidth = 7;
	}
}
