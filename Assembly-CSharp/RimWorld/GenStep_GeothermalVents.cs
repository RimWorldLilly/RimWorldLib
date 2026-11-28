using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A5 RID: 4773
	public class GenStep_GeothermalVents : GenStep_ScatterThings
	{
		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x06007656 RID: 30294 RVA: 0x0029AA37 File Offset: 0x00298C37
		public override int SeedPart
		{
			get
			{
				return 2140896;
			}
		}

		// Token: 0x06007657 RID: 30295 RVA: 0x0029AA40 File Offset: 0x00298C40
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Generate(map, parms);
			float x = this.densityByDistance.MaxBy((CurvePoint p) => p.x).x;
			ThingGrid thingGrid = map.thingGrid;
			foreach (IntVec3 intVec in this.geothermalVents)
			{
				foreach (IntVec3 intVec2 in GenRadial.RadialCellsAround(intVec, x, false))
				{
					if (thingGrid.ThingAt(intVec2, ThingDefOf.GeothermalVent) == null)
					{
						float lengthHorizontal = (intVec2 - intVec).LengthHorizontal;
						if (Rand.Chance(this.densityByDistance.Evaluate(lengthHorizontal)))
						{
							this.DoPlaceRandomFloraAt(intVec2, map);
						}
					}
				}
			}
			this.geothermalVents.Clear();
		}

		// Token: 0x06007658 RID: 30296 RVA: 0x0029AB64 File Offset: 0x00298D64
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			this.geothermalVents.Add(loc);
			base.ScatterAt(loc, map, parms, stackCount);
		}

		// Token: 0x06007659 RID: 30297 RVA: 0x0029AB80 File Offset: 0x00298D80
		private void DoPlaceRandomFloraAt(IntVec3 pos, Map map)
		{
			ThingDef plant = this.floraToScatter.RandomElementByWeight((BiomePlantRecord f) => f.commonality).plant;
			Thing thing;
			if (!plant.CanEverPlantAt(pos, map, out thing, false, true, true))
			{
				return;
			}
			Plant plant2 = (Plant)ThingMaker.MakeThing(plant, null);
			plant2.Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
			if (plant2.def.plant.LimitedLifespan)
			{
				plant2.Age = Rand.Range(0, Mathf.Max(plant2.def.plant.LifespanTicks - 50, 0));
			}
			GenSpawn.Spawn(plant2, pos, map, WipeMode.Vanish);
		}

		// Token: 0x04004A31 RID: 18993
		public List<BiomePlantRecord> floraToScatter;

		// Token: 0x04004A32 RID: 18994
		public SimpleCurve densityByDistance;

		// Token: 0x04004A33 RID: 18995
		[Unsaved(false)]
		private List<IntVec3> geothermalVents = new List<IntVec3>();
	}
}
