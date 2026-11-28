using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001983 RID: 6531
	public class CompSpawnerHives : ThingComp
	{
		// Token: 0x17001C7A RID: 7290
		// (get) Token: 0x0600A3FC RID: 41980 RVA: 0x003A95D5 File Offset: 0x003A77D5
		private CompProperties_SpawnerHives Props
		{
			get
			{
				return (CompProperties_SpawnerHives)this.props;
			}
		}

		// Token: 0x17001C7B RID: 7291
		// (get) Token: 0x0600A3FD RID: 41981 RVA: 0x003A95E2 File Offset: 0x003A77E2
		private bool CanSpawnChildHive
		{
			get
			{
				return this.canSpawnHives && HiveUtility.TotalSpawnedHivesCount(this.parent.Map, false) < 30 && Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f;
			}
		}

		// Token: 0x0600A3FE RID: 41982 RVA: 0x003A9619 File Offset: 0x003A7819
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x0600A3FF RID: 41983 RVA: 0x003A9624 File Offset: 0x003A7824
		public override void CompTick()
		{
			base.CompTick();
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			if ((comp == null || comp.Awake) && !this.wasActivated)
			{
				this.CalculateNextHiveSpawnTick();
				this.wasActivated = true;
			}
			if ((comp == null || comp.Awake) && Find.TickManager.TicksGame >= this.nextHiveSpawnTick)
			{
				Hive t;
				if (this.TrySpawnChildHive(false, out t))
				{
					Messages.Message("MessageHiveReproduced".Translate(), t, MessageTypeDefOf.NegativeEvent, true);
					return;
				}
				this.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x0600A400 RID: 41984 RVA: 0x003A96B8 File Offset: 0x003A78B8
		public override string CompInspectStringExtra()
		{
			if (!this.canSpawnHives || Find.Storyteller.difficulty.enemyReproductionRateFactor <= 0f)
			{
				return "DormantHiveNotReproducing".Translate();
			}
			if (this.CanSpawnChildHive)
			{
				return "HiveReproducesIn".Translate() + ": " + (this.nextHiveSpawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false);
			}
			return null;
		}

		// Token: 0x0600A401 RID: 41985 RVA: 0x003A9738 File Offset: 0x003A7938
		public void CalculateNextHiveSpawnTick()
		{
			Room room = this.parent.GetRoom(RegionType.Set_All);
			int num = 0;
			int num2 = GenRadial.NumCellsInRadius(9f);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && intVec.GetRoom(this.parent.Map) == room)
				{
					if (intVec.GetThingList(this.parent.Map).Any((Thing t) => t is Hive))
					{
						num++;
					}
				}
			}
			float num3 = this.Props.ReproduceRateFactorFromNearbyHiveCountCurve.Evaluate((float)num);
			if (Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f)
			{
				this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)(this.Props.HiveSpawnIntervalDays.RandomInRange * 60000f / (num3 * Find.Storyteller.difficulty.enemyReproductionRateFactor));
				return;
			}
			this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)this.Props.HiveSpawnIntervalDays.RandomInRange * 60000;
		}

		// Token: 0x0600A402 RID: 41986 RVA: 0x003A9888 File Offset: 0x003A7A88
		public bool TrySpawnChildHive(bool ignoreRoofedRequirement, out Hive newHive)
		{
			IntVec3 loc = CompSpawnerHives.FindChildHiveLocation(this.parent.Position, this.parent.Map, this.parent.def, this.Props, ignoreRoofedRequirement, false);
			return this.TrySpawnChildHive(loc, out newHive);
		}

		// Token: 0x0600A403 RID: 41987 RVA: 0x003A98CC File Offset: 0x003A7ACC
		public bool TrySpawnChildHive(IntVec3 loc, out Hive newHive)
		{
			if (!this.CanSpawnChildHive)
			{
				newHive = null;
				return false;
			}
			if (!loc.IsValid)
			{
				newHive = null;
				return false;
			}
			newHive = (Hive)ThingMaker.MakeThing(this.parent.def, null);
			if (newHive.Faction != this.parent.Faction)
			{
				newHive.SetFaction(this.parent.Faction, null);
			}
			Hive hive = this.parent as Hive;
			if (hive != null)
			{
				if (hive.CompDormant.Awake)
				{
					newHive.CompDormant.WakeUp();
				}
				newHive.questTags = hive.questTags;
			}
			GenSpawn.Spawn(newHive, loc, this.parent.Map, WipeMode.FullRefund);
			this.CalculateNextHiveSpawnTick();
			return true;
		}

		// Token: 0x0600A404 RID: 41988 RVA: 0x003A9984 File Offset: 0x003A7B84
		public static IntVec3 FindChildHiveLocation(IntVec3 pos, Map map, ThingDef parentDef, CompProperties_SpawnerHives props, bool ignoreRoofedRequirement, bool allowUnreachable)
		{
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 3; i++)
			{
				float minDist = props.HiveSpawnPreferredMinDist;
				bool flag;
				if (i < 2)
				{
					if (i == 1)
					{
						minDist = 0f;
					}
					flag = CellFinder.TryFindRandomReachableNearbyCell(pos, map, props.HiveSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), null, out intVec, 999999);
				}
				else
				{
					flag = (allowUnreachable && CellFinder.TryFindRandomCellNear(pos, map, (int)props.HiveSpawnRadius, (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), out intVec, -1));
				}
				if (flag)
				{
					intVec = CellFinder.FindNoWipeSpawnLocNear(intVec, map, parentDef, Rot4.North, 2, (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement));
					break;
				}
			}
			return intVec;
		}

		// Token: 0x0600A405 RID: 41989 RVA: 0x003A9AB0 File Offset: 0x003A7CB0
		private static bool CanSpawnHiveAt(IntVec3 c, Map map, IntVec3 parentPos, ThingDef parentDef, float minDist, bool ignoreRoofedRequirement)
		{
			if ((!ignoreRoofedRequirement && !c.Roofed(map)) || (!c.Walkable(map) || (minDist != 0f && (float)c.DistanceToSquared(parentPos) < minDist * minDist)) || c.GetFirstThing(map, ThingDefOf.InsectJelly) != null || c.GetFirstThing(map, ThingDefOf.GlowPod) != null)
			{
				return false;
			}
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] is Hive || thingList[j] is TunnelHiveSpawner)
						{
							return false;
						}
					}
				}
			}
			List<Thing> thingList2 = c.GetThingList(map);
			for (int k = 0; k < thingList2.Count; k++)
			{
				Thing thing = thingList2[k];
				if (thing.def.category == ThingCategory.Building && thing.def.passability == Traversability.Impassable && GenSpawn.SpawningWipes(parentDef, thing.def, false))
				{
					return true;
				}
			}
			return true;
		}

		// Token: 0x0600A406 RID: 41990 RVA: 0x003A9BCB File Offset: 0x003A7DCB
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompSpawnerHives.<CompGetGizmosExtra>d__16 <CompGetGizmosExtra>d__ = new CompSpawnerHives.<CompGetGizmosExtra>d__16(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A407 RID: 41991 RVA: 0x003A9BDB File Offset: 0x003A7DDB
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextHiveSpawnTick, "nextHiveSpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canSpawnHives, "canSpawnHives", true, false);
			Scribe_Values.Look<bool>(ref this.wasActivated, "wasActivated", true, false);
		}

		// Token: 0x040060E2 RID: 24802
		private int nextHiveSpawnTick = -1;

		// Token: 0x040060E3 RID: 24803
		public bool canSpawnHives = true;

		// Token: 0x040060E4 RID: 24804
		private bool wasActivated;

		// Token: 0x040060E5 RID: 24805
		public const int MaxHivesPerMap = 30;
	}
}
