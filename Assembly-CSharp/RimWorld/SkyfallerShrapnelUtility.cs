using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B58 RID: 7000
	public static class SkyfallerShrapnelUtility
	{
		// Token: 0x0600B3CC RID: 46028 RVA: 0x003F6259 File Offset: 0x003F4459
		public static void MakeShrapnel(IntVec3 center, Map map, float angle, float distanceFactor, int metalShrapnelCount, int rubbleShrapnelCount, bool spawnMotes)
		{
			angle -= 90f;
			SkyfallerShrapnelUtility.SpawnShrapnel(ThingDefOf.ChunkSlagSteel, metalShrapnelCount, center, map, angle, distanceFactor);
			SkyfallerShrapnelUtility.SpawnShrapnel(ThingDefOf.Filth_RubbleBuilding, rubbleShrapnelCount, center, map, angle, distanceFactor);
			if (spawnMotes)
			{
				SkyfallerShrapnelUtility.ThrowShrapnelMotes((metalShrapnelCount + rubbleShrapnelCount) * 2, center, map, angle, distanceFactor);
			}
		}

		// Token: 0x0600B3CD RID: 46029 RVA: 0x003F6298 File Offset: 0x003F4498
		private static void SpawnShrapnel(ThingDef def, int quantity, IntVec3 center, Map map, float angle, float distanceFactor)
		{
			for (int i = 0; i < quantity; i++)
			{
				IntVec3 intVec = SkyfallerShrapnelUtility.GenerateShrapnelLocation(center, angle, distanceFactor);
				if (SkyfallerShrapnelUtility.IsGoodShrapnelCell(intVec, map) && (def.category != ThingCategory.Item || intVec.GetFirstItem(map) == null) && intVec.GetFirstThing(map, def) == null)
				{
					GenSpawn.Spawn(def, intVec, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x0600B3CE RID: 46030 RVA: 0x003F62EC File Offset: 0x003F44EC
		private static void ThrowShrapnelMotes(int count, IntVec3 center, Map map, float angle, float distanceFactor)
		{
			for (int i = 0; i < count; i++)
			{
				IntVec3 c = SkyfallerShrapnelUtility.GenerateShrapnelLocation(center, angle, distanceFactor);
				if (SkyfallerShrapnelUtility.IsGoodShrapnelCell(c, map))
				{
					FleckMaker.ThrowDustPuff(c.ToVector3Shifted() + Gen.RandomHorizontalVector(0.5f), map, 2f);
				}
			}
		}

		// Token: 0x0600B3CF RID: 46031 RVA: 0x003F6339 File Offset: 0x003F4539
		private static bool IsGoodShrapnelCell(IntVec3 c, Map map)
		{
			return c.InBounds(map) && !c.Impassable(map) && !c.Filled(map) && map.roofGrid.RoofAt(c) == null;
		}

		// Token: 0x0600B3D0 RID: 46032 RVA: 0x003F636C File Offset: 0x003F456C
		private static IntVec3 GenerateShrapnelLocation(IntVec3 center, float angleOffset, float distanceFactor)
		{
			float num = SkyfallerShrapnelUtility.ShrapnelAngleDistribution.Evaluate(Rand.Value);
			float d = SkyfallerShrapnelUtility.ShrapnelDistanceFromAngle.Evaluate(num) * Rand.Value * distanceFactor;
			return (Vector3Utility.HorizontalVectorFromAngle(num + angleOffset) * d).ToIntVec3() + center;
		}

		// Token: 0x040067AE RID: 26542
		private const float ShrapnelDistanceFront = 6f;

		// Token: 0x040067AF RID: 26543
		private const float ShrapnelDistanceSide = 4f;

		// Token: 0x040067B0 RID: 26544
		private const float ShrapnelDistanceBack = 30f;

		// Token: 0x040067B1 RID: 26545
		private const int MotesPerShrapnel = 2;

		// Token: 0x040067B2 RID: 26546
		private static readonly SimpleCurve ShrapnelDistanceFromAngle = new SimpleCurve
		{
			{
				new CurvePoint(0f, 6f),
				true
			},
			{
				new CurvePoint(90f, 4f),
				true
			},
			{
				new CurvePoint(135f, 4f),
				true
			},
			{
				new CurvePoint(180f, 30f),
				true
			},
			{
				new CurvePoint(225f, 4f),
				true
			},
			{
				new CurvePoint(270f, 4f),
				true
			},
			{
				new CurvePoint(360f, 6f),
				true
			}
		};

		// Token: 0x040067B3 RID: 26547
		private static readonly SimpleCurve ShrapnelAngleDistribution = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 90f),
				true
			},
			{
				new CurvePoint(0.25f, 135f),
				true
			},
			{
				new CurvePoint(0.5f, 180f),
				true
			},
			{
				new CurvePoint(0.75f, 225f),
				true
			},
			{
				new CurvePoint(0.9f, 270f),
				true
			},
			{
				new CurvePoint(1f, 360f),
				true
			}
		};
	}
}
