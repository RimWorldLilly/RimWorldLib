using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF1 RID: 6897
	public class TorrentialRainFlood : Flood
	{
		// Token: 0x17001FC2 RID: 8130
		// (get) Token: 0x0600B033 RID: 45107 RVA: 0x003E14DE File Offset: 0x003DF6DE
		protected override int StartDelayTicks
		{
			get
			{
				return 4000;
			}
		}

		// Token: 0x0600B034 RID: 45108 RVA: 0x003E14E8 File Offset: 0x003DF6E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3, int>(ref this.floodedCells, "floodedCells", LookMode.Value, LookMode.Value, ref this.tmpKeys, ref this.tmpValues, true, false, false);
		}

		// Token: 0x0600B035 RID: 45109 RVA: 0x003E151C File Offset: 0x003DF71C
		protected override IEnumerable<ValueTuple<IntVec3, int>> GetInitialCells(Map map)
		{
			TorrentialRainFlood.<GetInitialCells>d__8 <GetInitialCells>d__ = new TorrentialRainFlood.<GetInitialCells>d__8(-2);
			<GetInitialCells>d__.<>4__this = this;
			<GetInitialCells>d__.<>3__map = map;
			return <GetInitialCells>d__;
		}

		// Token: 0x0600B036 RID: 45110 RVA: 0x003E1534 File Offset: 0x003DF734
		protected override void SpreadFlood(IntVec3 cell, TerrainDef sourceTerrain)
		{
			int value = (this.estimatedFloodedTiles - this.floodedTileCount) * 10;
			base.Map.terrainGrid.SetTempTerrain(cell, sourceTerrain.floodTerrain);
			this.floodedCells[cell] = value;
		}

		// Token: 0x0600B037 RID: 45111 RVA: 0x003E1578 File Offset: 0x003DF778
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.floodedCells.Any<KeyValuePair<IntVec3, int>>())
			{
				int num = this.floodedCells.Values.Min();
				foreach (KeyValuePair<IntVec3, int> keyValuePair in this.floodedCells)
				{
					IntVec3 intVec;
					int num2;
					keyValuePair.Deconstruct(out intVec, out num2);
					IntVec3 c = intVec;
					int num3 = num2;
					base.Map.tempTerrain.QueueRemoveTerrain(c, Find.TickManager.TicksGame + 2000 + num3 - num);
				}
			}
			base.Destroy(mode);
		}

		// Token: 0x040065AF RID: 26031
		private const int RecedeIntervalTicks = 10;

		// Token: 0x040065B0 RID: 26032
		private Dictionary<IntVec3, int> floodedCells = new Dictionary<IntVec3, int>();

		// Token: 0x040065B1 RID: 26033
		private const int EndDelayTicks = 2000;

		// Token: 0x040065B2 RID: 26034
		private List<IntVec3> tmpKeys;

		// Token: 0x040065B3 RID: 26035
		private List<int> tmpValues;
	}
}
