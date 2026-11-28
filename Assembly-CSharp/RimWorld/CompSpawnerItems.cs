using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001986 RID: 6534
	public class CompSpawnerItems : ThingComp
	{
		// Token: 0x17001C7D RID: 7293
		// (get) Token: 0x0600A40F RID: 41999 RVA: 0x003A9DA2 File Offset: 0x003A7FA2
		public CompProperties_SpawnerItems Props
		{
			get
			{
				return (CompProperties_SpawnerItems)this.props;
			}
		}

		// Token: 0x17001C7E RID: 7294
		// (get) Token: 0x0600A410 RID: 42000 RVA: 0x003A9DB0 File Offset: 0x003A7FB0
		public bool Active
		{
			get
			{
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				return comp == null || comp.Awake;
			}
		}

		// Token: 0x0600A411 RID: 42001 RVA: 0x003A9DD4 File Offset: 0x003A7FD4
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompSpawnerItems.<CompGetGizmosExtra>d__5 <CompGetGizmosExtra>d__ = new CompSpawnerItems.<CompGetGizmosExtra>d__5(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A412 RID: 42002 RVA: 0x003A9DE4 File Offset: 0x003A7FE4
		private void SpawnItems()
		{
			ThingDef thingDef;
			if (this.Props.MatchingItems.TryRandomElement(out thingDef))
			{
				int stackCount = Mathf.CeilToInt(this.Props.approxMarketValuePerDay / thingDef.BaseMarketValue);
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = stackCount;
				GenPlace.TryPlaceThing(thing, this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, null, null, 1);
			}
		}

		// Token: 0x0600A413 RID: 42003 RVA: 0x003A9E54 File Offset: 0x003A8054
		public override void CompTickRare()
		{
			if (!this.Active)
			{
				return;
			}
			this.ticksPassed += 250;
			if (this.ticksPassed >= this.Props.spawnInterval)
			{
				this.SpawnItems();
				this.ticksPassed -= this.Props.spawnInterval;
			}
		}

		// Token: 0x0600A414 RID: 42004 RVA: 0x003A9EAD File Offset: 0x003A80AD
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x0600A415 RID: 42005 RVA: 0x003A9EC4 File Offset: 0x003A80C4
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "NextSpawnedResourceIn".Translate() + ": " + (this.Props.spawnInterval - this.ticksPassed).ToStringTicksToPeriod(true, false, true, true, false);
			}
			return null;
		}

		// Token: 0x040060EE RID: 24814
		private int ticksPassed;
	}
}
