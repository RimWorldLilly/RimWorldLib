using System;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001292 RID: 4754
	public class GenStep_EscapeShip : GenStep_Scatterer
	{
		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x060075EF RID: 30191 RVA: 0x00297A09 File Offset: 0x00295C09
		public override int SeedPart
		{
			get
			{
				return 860042045;
			}
		}

		// Token: 0x060075F0 RID: 30192 RVA: 0x00297A10 File Offset: 0x00295C10
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			CellRect cellRect = new CellRect(c.x - GenStep_EscapeShip.EscapeShipSizeWidth.min / 2, c.z - GenStep_EscapeShip.EscapeShipSizeHeight.min / 2, GenStep_EscapeShip.EscapeShipSizeWidth.min, GenStep_EscapeShip.EscapeShipSizeHeight.min);
			if (!cellRect.FullyContainedWithin(new CellRect(0, 0, map.Size.x, map.Size.z)))
			{
				return false;
			}
			foreach (IntVec3 c2 in cellRect)
			{
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(c2);
				if (!c2.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy) && (terrainDef.driesTo == null || !terrainDef.driesTo.affordances.Contains(TerrainAffordanceDefOf.Heavy)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060075F1 RID: 30193 RVA: 0x00297B48 File Offset: 0x00295D48
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			int randomInRange = GenStep_EscapeShip.EscapeShipSizeWidth.RandomInRange;
			int randomInRange2 = GenStep_EscapeShip.EscapeShipSizeHeight.RandomInRange;
			CellRect rect = new CellRect(c.x - randomInRange / 2, c.z - randomInRange2 / 2, randomInRange, randomInRange2);
			rect.ClipInsideMap(map);
			foreach (IntVec3 c2 in rect)
			{
				if (!c2.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy))
				{
					CompTerrainPumpDry.AffectCell(map, c2);
					for (int i = 0; i < 8; i++)
					{
						Vector3 b = Rand.InsideUnitCircleVec3 * 3f;
						IntVec3 c3 = IntVec3.FromVector3(c2.ToVector3Shifted() + b);
						if (c3.InBounds(map))
						{
							CompTerrainPumpDry.AffectCell(map, c3);
						}
					}
				}
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ship_core", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x040049F0 RID: 18928
		private static readonly IntRange EscapeShipSizeWidth = new IntRange(20, 28);

		// Token: 0x040049F1 RID: 18929
		private static readonly IntRange EscapeShipSizeHeight = new IntRange(34, 42);
	}
}
