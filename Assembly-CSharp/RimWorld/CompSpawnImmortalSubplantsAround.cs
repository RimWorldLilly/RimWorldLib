using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001993 RID: 6547
	public class CompSpawnImmortalSubplantsAround : ThingComp
	{
		// Token: 0x17001C8C RID: 7308
		// (get) Token: 0x0600A456 RID: 42070 RVA: 0x003AB66D File Offset: 0x003A986D
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		// Token: 0x17001C8D RID: 7309
		// (get) Token: 0x0600A457 RID: 42071 RVA: 0x003AB67A File Offset: 0x003A987A
		public int CheckRespawnIntervalTicks
		{
			get
			{
				return this.Props.checkRespawnIntervalHours * 2500;
			}
		}

		// Token: 0x0600A458 RID: 42072 RVA: 0x003AB68D File Offset: 0x003A988D
		public void Disable()
		{
			this.disabled = true;
		}

		// Token: 0x0600A459 RID: 42073 RVA: 0x003AB698 File Offset: 0x003A9898
		private void WipeAllExistingPlants(IntVec3 cell)
		{
			List<Thing> thingList = cell.GetThingList(this.parent.Map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				if (thingList[i].def.category == ThingCategory.Plant && thingList[i].def != this.Props.dontWipePlant)
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600A45A RID: 42074 RVA: 0x003AB704 File Offset: 0x003A9904
		private Plant SpawnPlant(IntVec3 cell)
		{
			Plant plant = (Plant)GenSpawn.Spawn(this.Props.subplant, cell, this.parent.Map, WipeMode.Vanish);
			plant.Growth = Rand.Range(this.Props.minGrowthForSpawn, 1f);
			plant.Age = (plant.def.plant.LimitedLifespan ? Rand.Range(0, Mathf.Max(plant.def.plant.LifespanTicks - 2500, 0)) : 0);
			return plant;
		}

		// Token: 0x0600A45B RID: 42075 RVA: 0x003AB790 File Offset: 0x003A9990
		private void RespawnCheck()
		{
			foreach (IntVec3 intVec in this.cells.InRandomOrder(null))
			{
				Plant plant = intVec.GetPlant(this.parent.Map);
				Thing thing;
				if (((plant != null) ? plant.def : null) != this.Props.subplant && this.Props.subplant.CanEverPlantAt(intVec, this.parent.Map, out thing, false, true, true))
				{
					if (this.parent.Map.thingGrid.CellContains(intVec, ThingCategory.Item))
					{
						break;
					}
					this.SpawnPlant(intVec);
					break;
				}
			}
		}

		// Token: 0x0600A45C RID: 42076 RVA: 0x003AB858 File Offset: 0x003A9A58
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.disabled)
			{
				return;
			}
			if (respawningAfterLoad)
			{
				if (this.cells == null)
				{
					this.cells = new List<IntVec3>();
					foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.Props.maxRadius, false))
					{
						if (intVec.InBounds(this.parent.Map))
						{
							Plant plant = intVec.GetPlant(this.parent.Map);
							if (((plant != null) ? plant.def : null) == this.Props.subplant)
							{
								this.cells.Add(intVec);
							}
						}
					}
				}
				return;
			}
			if (this.cells == null)
			{
				this.cells = new List<IntVec3>();
				foreach (IntVec3 intVec2 in GenRadial.RadialCellsAround(this.parent.Position, this.Props.maxRadius, false).InRandomOrder(null))
				{
					if (intVec2.InBounds(this.parent.Map) && Rand.Chance(this.Props.chanceOverDistance.Evaluate(intVec2.DistanceTo(this.parent.Position))))
					{
						if (this.Props.maxPlants != -1 && this.cells.Count >= this.Props.maxPlants)
						{
							break;
						}
						Plant plant2 = intVec2.GetPlant(this.parent.Map);
						if (plant2 == null || plant2.def != this.Props.dontWipePlant)
						{
							this.WipeAllExistingPlants(intVec2);
							Thing thing;
							if (this.Props.subplant.CanEverPlantAt(intVec2, this.parent.Map, out thing, true, true, true))
							{
								this.cells.Add(intVec2);
								this.SpawnPlant(intVec2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600A45D RID: 42077 RVA: 0x003ABA68 File Offset: 0x003A9C68
		public override void CompTick()
		{
			if (this.disabled || !this.parent.Spawned)
			{
				return;
			}
			if (this.cells == null)
			{
				return;
			}
			for (int i = 0; i < 100; i++)
			{
				this.curCheckIndex++;
				if (this.curCheckIndex >= this.cells.Count)
				{
					this.curCheckIndex = 0;
					break;
				}
				Plant plant = this.cells[this.curCheckIndex].GetPlant(this.parent.Map);
				if (plant != null && plant.Spawned && plant.def == this.Props.subplant)
				{
					plant.Age = 0;
				}
			}
			if (this.parent.IsHashIntervalTick(this.CheckRespawnIntervalTicks))
			{
				this.RespawnCheck();
			}
		}

		// Token: 0x0600A45E RID: 42078 RVA: 0x003ABB2B File Offset: 0x003A9D2B
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompSpawnImmortalSubplantsAround.<CompGetGizmosExtra>d__13 <CompGetGizmosExtra>d__ = new CompSpawnImmortalSubplantsAround.<CompGetGizmosExtra>d__13(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A45F RID: 42079 RVA: 0x003ABB3C File Offset: 0x003A9D3C
		public override void PostExposeData()
		{
			string str = this.Props.saveKeysPrefix.NullOrEmpty() ? null : (this.Props.saveKeysPrefix + "_");
			Scribe_Values.Look<bool>(ref this.disabled, str + "disabled", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, str + "cells", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0400612C RID: 24876
		private bool disabled;

		// Token: 0x0400612D RID: 24877
		private List<IntVec3> cells;

		// Token: 0x0400612E RID: 24878
		private int curCheckIndex;
	}
}
