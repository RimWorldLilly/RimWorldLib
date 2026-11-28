using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A2 RID: 4770
	public class GenStep_FleshSacks : GenStep
	{
		// Token: 0x1700140D RID: 5133
		// (get) Token: 0x0600764A RID: 30282 RVA: 0x0029A4D4 File Offset: 0x002986D4
		public override int SeedPart
		{
			get
			{
				return 1234731256;
			}
		}

		// Token: 0x0600764B RID: 30283 RVA: 0x0029A4DC File Offset: 0x002986DC
		public override void Generate(Map map, GenStepParams parms)
		{
			SitePart sitePart = parms.sitePart;
			float? num;
			if (sitePart == null)
			{
				num = null;
			}
			else
			{
				Site site = sitePart.site;
				num = ((site != null) ? new float?(site.desiredThreatPoints) : null);
			}
			float x = num ?? StorytellerUtility.DefaultThreatPointsNow(map);
			SimpleCurve simpleCurve = this.numFleshSacksFromPoints;
			int num2 = Mathf.RoundToInt((simpleCurve != null) ? simpleCurve.Evaluate(x) : 0f);
			int i = 0;
			Predicate<IntVec3> <>9__0;
			Predicate<IntVec3> <>9__1;
			while (i < num2)
			{
				if (!this.trySpawnInRoom)
				{
					goto IL_C5;
				}
				Map map2 = map;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => this.Validator(c, map, true)));
				}
				IntVec3 intVec;
				if (!CellFinder.TryFindRandomCell(map2, validator, out intVec))
				{
					goto IL_C5;
				}
				goto IL_F8;
				IL_244:
				i++;
				continue;
				IL_C5:
				Map map3 = map;
				Predicate<IntVec3> validator2;
				if ((validator2 = <>9__1) == null)
				{
					validator2 = (<>9__1 = ((IntVec3 c) => this.Validator(c, map, false)));
				}
				if (!CellFinder.TryFindRandomCell(map3, validator2, out intVec))
				{
					goto IL_244;
				}
				IL_F8:
				foreach (IntVec3 intVec2 in GridShapeMaker.IrregularLump(intVec, map, this.sackClumpSize, null))
				{
					if (this.spawnSurroundingFleshmass)
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Fleshmass, null), intVec2, map, Rot4.Random, WipeMode.Vanish, false, false).SetFaction(Faction.OfEntities, null);
					}
					map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.Flesh);
				}
				List<Thing> list;
				if (this.forceAtLeastOneShard && i == num2 - 1)
				{
					list = Gen.YieldSingle<Thing>(ThingMaker.MakeThing(ThingDefOf.Shard, null)).ToList<Thing>();
				}
				else
				{
					ThingSetMakerParams parms2 = default(ThingSetMakerParams);
					parms2.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
					parms2.makingFaction = Faction.OfEntities;
					list = ThingSetMakerDefOf.MapGen_FleshSackLoot.root.Generate(parms2);
				}
				Building_Casket building_Casket = ThingMaker.MakeThing(ThingDefOf.FleshSack, null) as Building_Casket;
				GenSpawn.Spawn(building_Casket, intVec, map, WipeMode.Vanish);
				building_Casket.SetFaction(Faction.OfEntities, null);
				for (int j = list.Count - 1; j >= 0; j--)
				{
					Thing thing = list[j];
					if (!building_Casket.TryAcceptThing(thing, false))
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				goto IL_244;
			}
		}

		// Token: 0x0600764C RID: 30284 RVA: 0x0029A74C File Offset: 0x0029894C
		private bool Validator(IntVec3 c, Map map, bool mustBeInRoom)
		{
			return GenSpawn.CanSpawnAt(ThingDefOf.FleshSack, c, map, null, true) && c.DistanceToEdge(map) > 2 && (!mustBeInRoom || c.GetRoom(map) != null) && c.GetRoom(map).ProperRoom && (map.generatorDef.isUnderground || map.reachability.CanReachMapEdge(c, TraverseMode.PassDoors));
		}

		// Token: 0x04004A2C RID: 18988
		private int sackClumpSize = 50;

		// Token: 0x04004A2D RID: 18989
		private SimpleCurve numFleshSacksFromPoints;

		// Token: 0x04004A2E RID: 18990
		private bool forceAtLeastOneShard;

		// Token: 0x04004A2F RID: 18991
		private bool spawnSurroundingFleshmass = true;

		// Token: 0x04004A30 RID: 18992
		private bool trySpawnInRoom;
	}
}
