using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012CE RID: 4814
	public abstract class GenStep_SpecialTrees : GenStep
	{
		// Token: 0x0600776E RID: 30574 RVA: 0x002A1FF0 File Offset: 0x002A01F0
		public override void Generate(Map map, GenStepParams parms)
		{
			if (map.Biome.isExtremeBiome)
			{
				return;
			}
			int i = this.DesiredTreeCountForMap(map);
			int num = 0;
			Predicate<IntVec3> <>9__0;
			while (i > 0)
			{
				int minEdgeDistance = 25;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 x) => this.CanSpawnAt(x, map, 0, 50, (float)this.minProximityToSameTree, 22, 10, 0, -1, 15)));
				}
				IntVec3 cell;
				if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(minEdgeDistance, validator, map, out cell))
				{
					break;
				}
				Thing thing;
				if (this.TrySpawnAt(cell, map, this.GetGrowth(), out thing))
				{
					i--;
				}
				num++;
				if (num > 1000)
				{
					Log.Error("Could not place " + this.treeDef.label + "; too many iterations.");
					return;
				}
			}
		}

		// Token: 0x0600776F RID: 30575
		protected abstract float GetGrowth();

		// Token: 0x06007770 RID: 30576 RVA: 0x002A20B4 File Offset: 0x002A02B4
		public virtual bool TrySpawnAt(IntVec3 cell, Map map, float growth, out Thing plant)
		{
			Plant plant2 = cell.GetPlant(map);
			if (plant2 != null)
			{
				plant2.Destroy(DestroyMode.Vanish);
			}
			plant = GenSpawn.Spawn(this.treeDef, cell, map, WipeMode.Vanish);
			((Plant)plant).Growth = growth;
			if (Current.ProgramState == ProgramState.MapInitializing)
			{
				List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
				if (this.createUsedRect)
				{
					orGenerateVar.Add(CellRect.CenteredOn(cell, 0));
				}
			}
			return plant != null;
		}

		// Token: 0x06007771 RID: 30577
		public abstract int DesiredTreeCountForMap(Map map);

		// Token: 0x06007772 RID: 30578 RVA: 0x002A2120 File Offset: 0x002A0320
		public virtual bool CanSpawnAt(IntVec3 c, Map map, int minProximityToArtificialStructures = 40, int minProximityToCenter = 0, float fertilityRequirement = 0f, int minFertileUnroofedCells = 22, int maxFertileUnroofedCellRadius = 10, int minProximityToSameTree = 0, int maxProximityToSameTree = -1, int minDistFromMapEdge = 15)
		{
			if (!c.Standable(map) || c.Fogged(map) || !c.GetRoom(map).PsychologicallyOutdoors)
			{
				return false;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.growDays > 10f)
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == this.treeDef)
				{
					return false;
				}
			}
			if (Current.ProgramState == ProgramState.MapInitializing && MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Any((CellRect ur) => ur.Contains(c)))
			{
				return false;
			}
			if (!this.requireUnfogged && c.Fogged(map))
			{
				return false;
			}
			if (minProximityToCenter > 0 && map.Center.InHorDistOf(c, (float)minProximityToCenter))
			{
				return false;
			}
			if (minDistFromMapEdge > 0 && c.DistanceToEdge(map) < minDistFromMapEdge)
			{
				return false;
			}
			if (!map.reachability.CanReachFactionBase(c, map.ParentFaction))
			{
				return false;
			}
			if (c.GetTerrain(map).avoidWander)
			{
				return false;
			}
			if (c.GetFertility(map) <= 0f)
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (minProximityToArtificialStructures != 0 && GenRadial.RadialDistinctThingsAround(c, map, (float)minProximityToArtificialStructures, false).Any(new Func<Thing, bool>(MeditationUtility.CountsAsArtificialBuilding)))
			{
				return false;
			}
			if (minProximityToSameTree > 0 && GenRadial.RadialDistinctThingsAround(c, map, (float)minProximityToSameTree, false).Any((Thing t) => t.def == this.treeDef))
			{
				return false;
			}
			if (maxProximityToSameTree > 0 && GenRadial.RadialDistinctThingsAround(c, map, (float)maxProximityToSameTree, false).All((Thing t) => t.def != this.treeDef))
			{
				return false;
			}
			if (fertilityRequirement > 0f && c.GetFertility(map) < fertilityRequirement)
			{
				return false;
			}
			if (this.validators != null)
			{
				for (int j = 0; j < this.validators.Count; j++)
				{
					if (!this.validators[j].Allows(c, map))
					{
						return false;
					}
				}
			}
			int num = GenRadial.NumCellsInRadius((float)maxFertileUnroofedCellRadius);
			int num2 = 0;
			for (int k = 0; k < num; k++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[k];
				if (WanderUtility.InSameRoom(intVec, c, map))
				{
					if (intVec.InBounds(map) && !intVec.Roofed(map) && intVec.GetFertility(map) > 0f)
					{
						num2++;
					}
					if (num2 >= minFertileUnroofedCells)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004AE9 RID: 19177
		protected ThingDef treeDef;

		// Token: 0x04004AEA RID: 19178
		protected int minProximityToSameTree;

		// Token: 0x04004AEB RID: 19179
		public int pollutionNone;

		// Token: 0x04004AEC RID: 19180
		public int pollutionLight;

		// Token: 0x04004AED RID: 19181
		public int pollutionModerate = 1;

		// Token: 0x04004AEE RID: 19182
		public int pollutionExtreme = 3;

		// Token: 0x04004AEF RID: 19183
		public bool createUsedRect;

		// Token: 0x04004AF0 RID: 19184
		public bool requireUnfogged;

		// Token: 0x04004AF1 RID: 19185
		public List<ScattererValidator> validators = new List<ScattererValidator>();

		// Token: 0x04004AF2 RID: 19186
		private const int MinDistanceToEdge = 25;
	}
}
