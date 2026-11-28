using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF0 RID: 6896
	public class SeasonalFlood : Flood
	{
		// Token: 0x17001FC1 RID: 8129
		// (get) Token: 0x0600B02B RID: 45099 RVA: 0x003E12F6 File Offset: 0x003DF4F6
		protected override int MaxFloodDurationTicks
		{
			get
			{
				return 240000;
			}
		}

		// Token: 0x0600B02C RID: 45100 RVA: 0x003E12FD File Offset: 0x003DF4FD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.remainFloodedTicks, "floodedTicks", 0, false);
		}

		// Token: 0x0600B02D RID: 45101 RVA: 0x003E1318 File Offset: 0x003DF518
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.remainFloodedTicks = SeasonalFlood.FloodedTicksRange.RandomInRange;
			IntVec3 c2;
			CellFinder.TryFindRandomCell(map, (IntVec3 c) => c.GetTerrain(map).IsRiver && !c.Fogged(map), out c2);
			Find.LetterStack.ReceiveLetter("LetterLabelSeasonalFlooding".Translate(), "LetterSeasonalFlooding".Translate(), LetterDefOf.ThreatSmall, new LookTargets(c2, map), null, null, null, null, 0, true);
		}

		// Token: 0x0600B02E RID: 45102 RVA: 0x003E13B0 File Offset: 0x003DF5B0
		protected override void Tick()
		{
			if (base.Map.mapTemperature.OutdoorTemp < -7f)
			{
				Messages.Message("MessageSeasonalFloodingFrozen".Translate(), MessageTypeDefOf.NeutralEvent, true);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (base.Destroyed)
			{
				return;
			}
			if (this.remainFloodedTicks > 0 && Find.TickManager.TicksGame > this.spawnedTick + base.FloodingTicks + this.remainFloodedTicks)
			{
				Messages.Message("MessageFloodingSubsiding".Translate(), MessageTypeDefOf.NeutralEvent, true);
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B02F RID: 45103 RVA: 0x003E144F File Offset: 0x003DF64F
		protected override IEnumerable<ValueTuple<IntVec3, int>> GetInitialCells(Map map)
		{
			SeasonalFlood.<GetInitialCells>d__8 <GetInitialCells>d__ = new SeasonalFlood.<GetInitialCells>d__8(-2);
			<GetInitialCells>d__.<>4__this = this;
			<GetInitialCells>d__.<>3__map = map;
			return <GetInitialCells>d__;
		}

		// Token: 0x0600B030 RID: 45104 RVA: 0x003E1468 File Offset: 0x003DF668
		protected override void SpreadFlood(IntVec3 cell, TerrainDef sourceTerrain)
		{
			int num = this.estimatedFloodedTiles - this.floodedTileCount;
			base.Map.terrainGrid.SetTempTerrain(cell, TerrainDefOf.ShallowFloodwater);
			base.Map.tempTerrain.QueueRemoveTerrain(cell, this.spawnedTick + base.FloodingTicks + this.remainFloodedTicks + num);
		}

		// Token: 0x040065AC RID: 26028
		private const int RecedeIntervalTicks = 1;

		// Token: 0x040065AD RID: 26029
		private static readonly IntRange FloodedTicksRange = new IntRange(240000, 360000);

		// Token: 0x040065AE RID: 26030
		private int remainFloodedTicks;
	}
}
