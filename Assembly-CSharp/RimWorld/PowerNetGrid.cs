using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136F RID: 4975
	public class PowerNetGrid
	{
		// Token: 0x06007B51 RID: 31569 RVA: 0x002BE4D4 File Offset: 0x002BC6D4
		public PowerNetGrid(Map map)
		{
			this.map = map;
			this.netGrid = new PowerNet[map.cellIndices.NumGridCells];
		}

		// Token: 0x06007B52 RID: 31570 RVA: 0x002BE504 File Offset: 0x002BC704
		public PowerNet TransmittedPowerNetAt(IntVec3 c)
		{
			return this.netGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06007B53 RID: 31571 RVA: 0x002BE520 File Offset: 0x002BC720
		public void Notify_PowerNetCreated(PowerNet newNet)
		{
			if (this.powerNetCells.ContainsKey(newNet))
			{
				Log.Warning("Net " + ((newNet != null) ? newNet.ToString() : null) + " is already registered in PowerNetGrid.");
				this.powerNetCells.Remove(newNet);
			}
			List<IntVec3> list = new List<IntVec3>();
			this.powerNetCells.Add(newNet, list);
			for (int i = 0; i < newNet.transmitters.Count; i++)
			{
				CellRect cellRect = newNet.transmitters[i].parent.OccupiedRect();
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					for (int k = cellRect.minX; k <= cellRect.maxX; k++)
					{
						int num = this.map.cellIndices.CellToIndex(k, j);
						if (this.netGrid[num] != null)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Two power nets on the same cell (",
								k.ToString(),
								", ",
								j.ToString(),
								"). First transmitters: ",
								newNet.transmitters[0].parent.LabelCap,
								" and ",
								this.netGrid[num].transmitters.NullOrEmpty<CompPower>() ? "[none]" : this.netGrid[num].transmitters[0].parent.LabelCap,
								"."
							}));
						}
						this.netGrid[num] = newNet;
						list.Add(new IntVec3(k, 0, j));
					}
				}
			}
		}

		// Token: 0x06007B54 RID: 31572 RVA: 0x002BE6CC File Offset: 0x002BC8CC
		public void Notify_PowerNetDeleted(PowerNet deadNet)
		{
			List<IntVec3> list;
			if (!this.powerNetCells.TryGetValue(deadNet, out list))
			{
				Log.Warning("Net " + ((deadNet != null) ? deadNet.ToString() : null) + " does not exist in PowerNetGrid's dictionary.");
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				int num = this.map.cellIndices.CellToIndex(list[i]);
				if (this.netGrid[num] == deadNet)
				{
					this.netGrid[num] = null;
				}
				else
				{
					Log.Warning("Multiple nets on the same cell " + list[i].ToString() + ". This is probably a result of an earlier error.");
				}
			}
			this.powerNetCells.Remove(deadNet);
		}

		// Token: 0x06007B55 RID: 31573 RVA: 0x002BE780 File Offset: 0x002BC980
		public void DrawDebugPowerNetGrid()
		{
			if (!DebugViewSettings.drawPowerNetGrid)
			{
				return;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (this.map != Find.CurrentMap)
			{
				return;
			}
			Rand.PushState();
			foreach (IntVec3 c in Find.CameraDriver.CurrentViewRect.ClipInsideMap(this.map))
			{
				PowerNet powerNet = this.netGrid[this.map.cellIndices.CellToIndex(c)];
				if (powerNet != null)
				{
					Rand.Seed = powerNet.GetHashCode();
					CellRenderer.RenderCell(c, Rand.Value);
				}
			}
			Rand.PopState();
		}

		// Token: 0x04004DDE RID: 19934
		private Map map;

		// Token: 0x04004DDF RID: 19935
		private PowerNet[] netGrid;

		// Token: 0x04004DE0 RID: 19936
		private Dictionary<PowerNet, List<IntVec3>> powerNetCells = new Dictionary<PowerNet, List<IntVec3>>();
	}
}
