using System;
using RimWorld.BaseGen;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BE RID: 4798
	public class GenStep_ScatterAncientLandingPad : GenStep_Scatterer
	{
		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x0600770F RID: 30479 RVA: 0x0029FBD0 File Offset: 0x0029DDD0
		public override int SeedPart
		{
			get
			{
				return 1872954345;
			}
		}

		// Token: 0x06007710 RID: 30480 RVA: 0x0029FBD7 File Offset: 0x0029DDD7
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient landing"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			this.randomSize = Mathf.RoundToInt(Rand.ByCurve(GenStep_ScatterAncientLandingPad.SizeChanceCurve));
			base.Generate(map, parms);
		}

		// Token: 0x06007711 RID: 30481 RVA: 0x0029FC14 File Offset: 0x0029DE14
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(loc, this.randomSize, this.randomSize);
			return this.CanPlaceAt(rect, map);
		}

		// Token: 0x06007712 RID: 30482 RVA: 0x0029FC50 File Offset: 0x0029DE50
		private bool CanPlaceAt(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.ExpandedBy(2))
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
				if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Light))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007713 RID: 30483 RVA: 0x0029FCF8 File Offset: 0x0029DEF8
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.filthDensity = new FloatRange?(new FloatRange(0.025f, 0.05f));
			resolveParams.filthDef = ThingDefOf.Filth_Ash;
			resolveParams.rect = CellRect.CenteredOn(loc, this.randomSize, this.randomSize);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("filth", resolveParams, null);
			BaseGen.Generate();
			SketchResolveParams parms2 = default(SketchResolveParams);
			parms2.landingPadSize = new IntVec2?(new IntVec2(this.randomSize, this.randomSize));
			parms2.sketch = new Sketch();
			SketchGen.Generate(SketchResolverDefOf.AncientLandingPad, parms2).Spawn(map, loc - new IntVec3(this.randomSize / 2, 0, this.randomSize / 2), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, false, null, null, null);
		}

		// Token: 0x04004ABB RID: 19131
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

		// Token: 0x04004ABC RID: 19132
		private const int Gap = 2;

		// Token: 0x04004ABD RID: 19133
		private int randomSize;
	}
}
