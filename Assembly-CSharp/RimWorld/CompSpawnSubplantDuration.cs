using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001994 RID: 6548
	public class CompSpawnSubplantDuration : ThingComp
	{
		// Token: 0x17001C8E RID: 7310
		// (get) Token: 0x0600A461 RID: 42081 RVA: 0x003ABBB0 File Offset: 0x003A9DB0
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		// Token: 0x0600A462 RID: 42082 RVA: 0x003ABBBD File Offset: 0x003A9DBD
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.SetupNextSubplantTick();
			}
		}

		// Token: 0x0600A463 RID: 42083 RVA: 0x003ABBC8 File Offset: 0x003A9DC8
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame >= this.nextSubplantTick)
			{
				this.DoGrowSubplant(false);
				this.SetupNextSubplantTick();
			}
		}

		// Token: 0x0600A464 RID: 42084 RVA: 0x003ABBE9 File Offset: 0x003A9DE9
		public void SetupNextSubplantTick()
		{
			this.nextSubplantTick = Find.TickManager.TicksGame + (int)(60000f * this.Props.subplantSpawnDays);
		}

		// Token: 0x0600A465 RID: 42085 RVA: 0x003ABC10 File Offset: 0x003A9E10
		public void DoGrowSubplant(bool force = false)
		{
			if (!ModLister.CheckIdeology("Subplant duration spawning"))
			{
				return;
			}
			if (!force && ((Plant)this.parent).Growth < this.Props.minGrowthForSpawn)
			{
				return;
			}
			IntVec3 position = this.parent.Position;
			int num = GenRadial.NumCellsInRadius(this.Props.maxRadius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && WanderUtility.InSameRoom(position, intVec, this.parent.Map))
				{
					bool flag = false;
					List<Thing> thingList = intVec.GetThingList(this.parent.Map);
					foreach (Thing thing in thingList)
					{
						if (thing.def == this.Props.subplant)
						{
							flag = true;
							break;
						}
						if (!this.Props.plantsToNotOverwrite.NullOrEmpty<ThingDef>())
						{
							for (int j = 0; j < this.Props.plantsToNotOverwrite.Count; j++)
							{
								if (thing.def == this.Props.plantsToNotOverwrite[j])
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						if (!this.Props.canSpawnOverPlayerSownPlants)
						{
							Plant plant = intVec.GetPlant(this.parent.Map);
							Zone zone = this.parent.Map.zoneManager.ZoneAt(intVec);
							if (plant != null && plant.sown && zone != null && zone is Zone_Growing)
							{
								goto IL_251;
							}
						}
						if (this.Props.subplant.CanEverPlantAt(intVec, this.parent.Map, true, false))
						{
							for (int k = thingList.Count - 1; k >= 0; k--)
							{
								if (thingList[k].def.category == ThingCategory.Plant)
								{
									thingList[k].Destroy(DestroyMode.Vanish);
								}
							}
							Plant plant2 = (Plant)GenSpawn.Spawn(this.Props.subplant, intVec, this.parent.Map, WipeMode.Vanish);
							if (this.Props.initialGrowthRange != null)
							{
								plant2.Growth = this.Props.initialGrowthRange.Value.RandomInRange;
							}
							return;
						}
					}
				}
				IL_251:;
			}
		}

		// Token: 0x0600A466 RID: 42086 RVA: 0x003ABE8C File Offset: 0x003AA08C
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextSubplantTick, "nextSubplantTick", 0, false);
		}

		// Token: 0x0400612F RID: 24879
		private int nextSubplantTick;
	}
}
