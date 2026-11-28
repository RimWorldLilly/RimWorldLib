using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C2 RID: 4802
	public class GenStep_ScatterAncientUtilityBuilding : GenStep_Scatterer
	{
		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x06007729 RID: 30505 RVA: 0x002A05E5 File Offset: 0x0029E7E5
		public override int SeedPart
		{
			get
			{
				return 1872954345;
			}
		}

		// Token: 0x0600772A RID: 30506 RVA: 0x002A05EC File Offset: 0x0029E7EC
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient outdoor building"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			this.randomSize = Mathf.RoundToInt(Rand.ByCurve(GenStep_ScatterAncientUtilityBuilding.SizeChanceCurve));
			base.Generate(map, parms);
		}

		// Token: 0x0600772B RID: 30507 RVA: 0x002A0628 File Offset: 0x0029E828
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			if (!loc.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			CellRect rect = new CellRect(loc.x, loc.z, this.randomSize, this.randomSize);
			return this.CanPlaceAt(rect, map);
		}

		// Token: 0x0600772C RID: 30508 RVA: 0x002A0680 File Offset: 0x0029E880
		private bool CanPlaceAt(CellRect rect, Map map)
		{
			if (MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Any((CellRect ur) => ur.Overlaps(rect)))
			{
				return false;
			}
			foreach (IntVec3 c in rect.Cells)
			{
				if (!c.InBounds(map))
				{
					return false;
				}
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
				if (terrainDef.IsWater || terrainDef.IsRoad)
				{
					return false;
				}
				if (c.GetEdifice(map) != null)
				{
					return false;
				}
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable)
					{
						return false;
					}
				}
				if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600772D RID: 30509 RVA: 0x002A0790 File Offset: 0x0029E990
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			SketchResolveParams parms2 = default(SketchResolveParams);
			parms2.utilityBuildingSize = new IntVec2?(new IntVec2(this.randomSize, this.randomSize));
			parms2.sketch = new Sketch();
			SketchGen.Generate(SketchResolverDefOf.AncientUtilityBuilding, parms2).Spawn(map, loc, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, false, null, null, null);
		}

		// Token: 0x04004AC5 RID: 19141
		private static readonly SimpleCurve SizeChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(8f, 0f),
				true
			},
			{
				new CurvePoint(12f, 4f),
				true
			},
			{
				new CurvePoint(18f, 0f),
				true
			}
		};

		// Token: 0x04004AC6 RID: 19142
		private int randomSize;
	}
}
