using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200197E RID: 6526
	public class CompSpawner : ThingComp
	{
		// Token: 0x17001C75 RID: 7285
		// (get) Token: 0x0600A3DE RID: 41950 RVA: 0x003A8D3D File Offset: 0x003A6F3D
		public CompProperties_Spawner PropsSpawner
		{
			get
			{
				return (CompProperties_Spawner)this.props;
			}
		}

		// Token: 0x17001C76 RID: 7286
		// (get) Token: 0x0600A3DF RID: 41951 RVA: 0x003A8D4C File Offset: 0x003A6F4C
		private bool PowerOn
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp != null && comp.PowerOn;
			}
		}

		// Token: 0x0600A3E0 RID: 41952 RVA: 0x003A8D70 File Offset: 0x003A6F70
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.ResetCountdown();
			}
		}

		// Token: 0x0600A3E1 RID: 41953 RVA: 0x003A8D88 File Offset: 0x003A6F88
		public override void CompTick()
		{
			this.TickIntervalDelta(1);
		}

		// Token: 0x0600A3E2 RID: 41954 RVA: 0x003A8D91 File Offset: 0x003A6F91
		public override void CompTickRare()
		{
			this.TickIntervalDelta(250);
		}

		// Token: 0x0600A3E3 RID: 41955 RVA: 0x003A8DA0 File Offset: 0x003A6FA0
		private void TickIntervalDelta(int interval)
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			if (comp != null)
			{
				if (!comp.Awake)
				{
					return;
				}
			}
			else if (this.parent.Position.Fogged(this.parent.Map))
			{
				return;
			}
			if (this.PropsSpawner.requiresPower && !this.PowerOn)
			{
				return;
			}
			this.ticksUntilSpawn -= interval;
			this.CheckShouldSpawn();
		}

		// Token: 0x0600A3E4 RID: 41956 RVA: 0x003A8E1B File Offset: 0x003A701B
		private void CheckShouldSpawn()
		{
			if (this.ticksUntilSpawn <= 0)
			{
				this.ResetCountdown();
				this.TryDoSpawn();
			}
		}

		// Token: 0x0600A3E5 RID: 41957 RVA: 0x003A8E34 File Offset: 0x003A7034
		public bool TryDoSpawn()
		{
			if (!this.parent.Spawned)
			{
				return false;
			}
			if (this.PropsSpawner.spawnMaxAdjacent >= 0)
			{
				int num = 0;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = this.parent.Position + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.parent.Map))
					{
						List<Thing> thingList = c.GetThingList(this.parent.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def == this.PropsSpawner.thingToSpawn)
							{
								num += thingList[j].stackCount;
								if (num >= this.PropsSpawner.spawnMaxAdjacent)
								{
									return false;
								}
							}
						}
					}
				}
			}
			IntVec3 center;
			if (!CompSpawner.TryFindSpawnCell(this.parent, this.PropsSpawner.thingToSpawn, this.PropsSpawner.spawnCount, out center))
			{
				return false;
			}
			Thing thing = ThingMaker.MakeThing(this.PropsSpawner.thingToSpawn, null);
			thing.stackCount = this.PropsSpawner.spawnCount;
			if (this.PropsSpawner.inheritFaction && thing.Faction != this.parent.Faction)
			{
				thing.SetFaction(this.parent.Faction, null);
			}
			Thing t;
			if (!GenPlace.TryPlaceThing(thing, center, this.parent.Map, ThingPlaceMode.Direct, out t, null, null, null, 1))
			{
				return false;
			}
			if (this.PropsSpawner.spawnForbidden)
			{
				t.SetForbidden(true, true);
			}
			if (this.PropsSpawner.showMessageIfOwned && this.parent.Faction == Faction.OfPlayer)
			{
				Messages.Message("MessageCompSpawnerSpawnedItem".Translate(this.PropsSpawner.thingToSpawn.LabelCap), thing, MessageTypeDefOf.PositiveEvent, true);
			}
			return true;
		}

		// Token: 0x0600A3E6 RID: 41958 RVA: 0x003A9020 File Offset: 0x003A7220
		public static bool TryFindSpawnCell(Thing parent, ThingDef thingToSpawn, int spawnCount, out IntVec3 result)
		{
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(parent).InRandomOrder(null))
			{
				if (intVec.Walkable(parent.Map))
				{
					Building edifice = intVec.GetEdifice(parent.Map);
					if (edifice != null)
					{
						if (thingToSpawn.IsEdifice() || edifice is IHaulDestination)
						{
							continue;
						}
						Building_Door building_Door = edifice as Building_Door;
						if (building_Door != null && !building_Door.FreePassage)
						{
							continue;
						}
					}
					if (parent.def.passability == Traversability.Impassable || GenSight.LineOfSight(parent.Position, intVec, parent.Map))
					{
						bool flag = false;
						List<Thing> thingList = intVec.GetThingList(parent.Map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							if (thing.def.category == ThingCategory.Item && (thing.def != thingToSpawn || thing.stackCount > thingToSpawn.stackLimit - spawnCount))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							result = intVec;
							return true;
						}
					}
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600A3E7 RID: 41959 RVA: 0x003A9160 File Offset: 0x003A7360
		private void ResetCountdown()
		{
			this.ticksUntilSpawn = this.PropsSpawner.spawnIntervalRange.RandomInRange;
		}

		// Token: 0x0600A3E8 RID: 41960 RVA: 0x003A9178 File Offset: 0x003A7378
		public override void PostExposeData()
		{
			string str = this.PropsSpawner.saveKeysPrefix.NullOrEmpty() ? null : (this.PropsSpawner.saveKeysPrefix + "_");
			Scribe_Values.Look<int>(ref this.ticksUntilSpawn, str + "ticksUntilSpawn", 0, false);
		}

		// Token: 0x0600A3E9 RID: 41961 RVA: 0x003A91C8 File Offset: 0x003A73C8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompSpawner.<CompGetGizmosExtra>d__14 <CompGetGizmosExtra>d__ = new CompSpawner.<CompGetGizmosExtra>d__14(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A3EA RID: 41962 RVA: 0x003A91D8 File Offset: 0x003A73D8
		public override string CompInspectStringExtra()
		{
			if (this.PropsSpawner.writeTimeLeftToSpawn && (!this.PropsSpawner.requiresPower || this.PowerOn))
			{
				return "NextSpawnedItemIn".Translate(GenLabel.ThingLabel(this.PropsSpawner.thingToSpawn, null, this.PropsSpawner.spawnCount)).Resolve() + ": " + this.ticksUntilSpawn.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
			}
			return null;
		}

		// Token: 0x040060D7 RID: 24791
		private int ticksUntilSpawn;
	}
}
