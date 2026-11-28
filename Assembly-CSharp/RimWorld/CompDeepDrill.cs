using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001859 RID: 6233
	public class CompDeepDrill : ThingComp
	{
		// Token: 0x17001AA2 RID: 6818
		// (get) Token: 0x06009BF8 RID: 39928 RVA: 0x00382F0E File Offset: 0x0038110E
		[Obsolete("Use WorkPerPortionBase constant directly.")]
		public static float WorkPerPortionCurrentDifficulty
		{
			get
			{
				return 10000f;
			}
		}

		// Token: 0x17001AA3 RID: 6819
		// (get) Token: 0x06009BF9 RID: 39929 RVA: 0x00382F15 File Offset: 0x00381115
		public float ProgressToNextPortionPercent
		{
			get
			{
				return this.portionProgress / 10000f;
			}
		}

		// Token: 0x06009BFA RID: 39930 RVA: 0x00382F24 File Offset: 0x00381124
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06009BFB RID: 39931 RVA: 0x00382F37 File Offset: 0x00381137
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.portionProgress, "portionProgress", 0f, false);
			Scribe_Values.Look<float>(ref this.portionYieldPct, "portionYieldPct", 0f, false);
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", 0, false);
		}

		// Token: 0x06009BFC RID: 39932 RVA: 0x00382F78 File Offset: 0x00381178
		public void DrillWorkDone(Pawn driller, int delta)
		{
			float num = driller.GetStatValue(StatDefOf.DeepDrillingSpeed, true, -1) * (float)delta;
			this.portionProgress += num;
			this.portionYieldPct += num * driller.GetStatValue(StatDefOf.MiningYield, true, -1) / 10000f;
			this.lastUsedTick = Find.TickManager.TicksGame;
			if (this.portionProgress > 10000f)
			{
				this.TryProducePortion(this.portionYieldPct, driller);
				this.portionProgress = 0f;
				this.portionYieldPct = 0f;
			}
		}

		// Token: 0x06009BFD RID: 39933 RVA: 0x00383006 File Offset: 0x00381206
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			this.portionProgress = 0f;
			this.portionYieldPct = 0f;
			this.lastUsedTick = -99999;
		}

		// Token: 0x06009BFE RID: 39934 RVA: 0x0038302C File Offset: 0x0038122C
		private void TryProducePortion(float yieldPct, Pawn driller = null)
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			bool nextResource = this.GetNextResource(out thingDef, out num, out intVec);
			if (thingDef == null)
			{
				return;
			}
			int num2 = Mathf.Min(num, thingDef.deepCountPerPortion);
			if (nextResource)
			{
				this.parent.Map.deepResourceGrid.SetAt(intVec, thingDef, num - num2);
			}
			int stackCount = Mathf.Max(1, GenMath.RoundRandom((float)num2 * yieldPct));
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = stackCount;
			GenPlace.TryPlaceThing(thing, this.parent.InteractionCell, this.parent.Map, ThingPlaceMode.Near, null, (IntVec3 p) => p != this.parent.Position && p != this.parent.InteractionCell, null, 1);
			if (driller != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mined, driller.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (nextResource && !this.ValuableResourcesPresent())
			{
				if (DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position) == null)
				{
					Messages.Message("DeepDrillExhaustedNoFallback".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					return;
				}
				Messages.Message("DeepDrillExhausted".Translate(Find.ActiveLanguageWorker.Pluralize(DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position).label, -1)), this.parent, MessageTypeDefOf.TaskCompletion, true);
				for (int i = 0; i < 21; i++)
				{
					IntVec3 c = intVec + GenRadial.RadialPattern[i];
					if (c.InBounds(this.parent.Map))
					{
						ThingWithComps firstThingWithComp = c.GetFirstThingWithComp(this.parent.Map);
						if (firstThingWithComp != null && !firstThingWithComp.GetComp<CompDeepDrill>().ValuableResourcesPresent())
						{
							firstThingWithComp.SetForbidden(true, true);
						}
					}
				}
			}
		}

		// Token: 0x06009BFF RID: 39935 RVA: 0x00383200 File Offset: 0x00381400
		private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			return DeepDrillUtility.GetNextResource(this.parent.Position, this.parent.Map, out resDef, out countPresent, out cell);
		}

		// Token: 0x06009C00 RID: 39936 RVA: 0x00383220 File Offset: 0x00381420
		public bool CanDrillNow()
		{
			return (this.powerComp == null || this.powerComp.PowerOn) && (DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position) != null || this.ValuableResourcesPresent());
		}

		// Token: 0x06009C01 RID: 39937 RVA: 0x00383260 File Offset: 0x00381460
		public bool ValuableResourcesPresent()
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			return this.GetNextResource(out thingDef, out num, out intVec);
		}

		// Token: 0x06009C02 RID: 39938 RVA: 0x00383279 File Offset: 0x00381479
		public bool UsedLastTick()
		{
			return this.lastUsedTick >= Find.TickManager.TicksGame - 1;
		}

		// Token: 0x06009C03 RID: 39939 RVA: 0x00383292 File Offset: 0x00381492
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompDeepDrill.<CompGetGizmosExtra>d__18 <CompGetGizmosExtra>d__ = new CompDeepDrill.<CompGetGizmosExtra>d__18(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009C04 RID: 39940 RVA: 0x003832A4 File Offset: 0x003814A4
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			this.GetNextResource(out thingDef, out num, out intVec);
			if (thingDef == null)
			{
				return "DeepDrillNoResources".Translate();
			}
			return "ResourceBelow".Translate() + ": " + thingDef.LabelCap + "\n" + "ProgressToNextPortion".Translate() + ": " + this.ProgressToNextPortionPercent.ToStringPercent("F0");
		}

		// Token: 0x04005CBB RID: 23739
		private CompPowerTrader powerComp;

		// Token: 0x04005CBC RID: 23740
		private float portionProgress;

		// Token: 0x04005CBD RID: 23741
		private float portionYieldPct;

		// Token: 0x04005CBE RID: 23742
		private int lastUsedTick = -99999;

		// Token: 0x04005CBF RID: 23743
		private const float WorkPerPortionBase = 10000f;
	}
}
