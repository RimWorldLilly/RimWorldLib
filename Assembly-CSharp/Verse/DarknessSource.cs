using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000371 RID: 881
	public class DarknessSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x06001AFD RID: 6909 RVA: 0x000A60AC File Offset: 0x000A42AC
		public DarknessSource(Map map) : base(map)
		{
			map.events.GlowChanged += this.NotifyGlowChanged;
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x000A60D7 File Offset: 0x000A42D7
		private void NotifyGlowChanged(IntVec3 cell)
		{
			if (this.map.GameConditionManager.GetActiveCondition<GameCondition_UnnaturalDarkness>() != null)
			{
				this.dirtyCells.Add(cell);
			}
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x000A60F8 File Offset: 0x000A42F8
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.Clear();
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (!GameCondition_UnnaturalDarkness.UnnaturalDarknessOnMap(this.map))
			{
				return;
			}
			this.wasActive = true;
			for (int i = 0; i < this.cellCount; i++)
			{
				this.data.Set(i, GameCondition_UnnaturalDarkness.UnnaturalDarknessAt(this.map.cellIndices[i], this.map));
			}
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x000A6168 File Offset: 0x000A4368
		public override bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			bool flag = GameCondition_UnnaturalDarkness.UnnaturalDarknessOnMap(this.map);
			bool result = false;
			if (flag && this.dirtyCells.Any<IntVec3>())
			{
				foreach (IntVec3 cell in this.dirtyCells)
				{
					this.data.Set(this.map.cellIndices[cell], GameCondition_UnnaturalDarkness.UnnaturalDarknessAt(cell, this.map));
				}
				result = true;
				this.dirtyCells.Clear();
			}
			if (!flag && !this.wasActive)
			{
				return false;
			}
			if (flag && this.wasActive)
			{
				return result;
			}
			if (!flag)
			{
				this.data.Clear();
			}
			else
			{
				for (int i = 0; i < this.cellCount; i++)
				{
					this.data.Set(i, GameCondition_UnnaturalDarkness.UnnaturalDarknessAt(this.map.cellIndices[i], this.map));
				}
			}
			this.wasActive = flag;
			return true;
		}

		// Token: 0x0400162E RID: 5678
		private bool wasActive;

		// Token: 0x0400162F RID: 5679
		private readonly HashSet<IntVec3> dirtyCells = new HashSet<IntVec3>();
	}
}
