using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200028A RID: 650
	public sealed class ThingGrid
	{
		// Token: 0x06001394 RID: 5012 RVA: 0x0006F064 File Offset: 0x0006D264
		public ThingGrid(Map map)
		{
			this.map = map;
			CellIndices cellIndices = map.cellIndices;
			this.thingGrid = new List<Thing>[cellIndices.NumGridCells];
			for (int i = 0; i < cellIndices.NumGridCells; i++)
			{
				this.thingGrid[i] = new List<Thing>(4);
			}
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0006F0B8 File Offset: 0x0006D2B8
		public void Register(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.RegisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.RegisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0006F13C File Offset: 0x0006D33C
		private void RegisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				string str = (t != null) ? t.ToString() : null;
				string str2 = " tried to register out of bounds at ";
				IntVec3 intVec = c;
				Log.Warning(str + str2 + intVec.ToString() + ". Destroying.");
				t.Destroy(DestroyMode.Vanish);
				return;
			}
			this.thingGrid[this.map.cellIndices.CellToIndex(c)].Add(t);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x0006F1B0 File Offset: 0x0006D3B0
		public void Deregister(Thing t, bool doEvenIfDespawned = false)
		{
			if (!t.Spawned && !doEvenIfDespawned)
			{
				return;
			}
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.DeregisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.DeregisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0006F240 File Offset: 0x0006D440
		private void DeregisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				string str = (t != null) ? t.ToString() : null;
				string str2 = " tried to de-register out of bounds at ";
				IntVec3 intVec = c;
				Log.Error(str + str2 + intVec.ToString());
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.thingGrid[num].Contains(t))
			{
				this.thingGrid[num].Remove(t);
			}
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0006F2B8 File Offset: 0x0006D4B8
		public IEnumerable<Thing> ThingsAt(IntVec3 c)
		{
			ThingGrid.<ThingsAt>d__7 <ThingsAt>d__ = new ThingGrid.<ThingsAt>d__7(-2);
			<ThingsAt>d__.<>4__this = this;
			<ThingsAt>d__.<>3__c = c;
			return <ThingsAt>d__;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0006F2D0 File Offset: 0x0006D4D0
		public List<Thing> ThingsListAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				string str = "Got ThingsListAt out of bounds: ";
				IntVec3 intVec = c;
				Log.ErrorOnce(str + intVec.ToString(), 495287);
				return ThingGrid.EmptyThingList;
			}
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0006F32C File Offset: 0x0006D52C
		public List<Thing> ThingsListAtFast(IntVec3 c)
		{
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0006F346 File Offset: 0x0006D546
		public List<Thing> ThingsListAtFast(int index)
		{
			return this.thingGrid[index];
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0006F350 File Offset: 0x0006D550
		public bool CellContains(IntVec3 c, ThingCategory cat)
		{
			return this.ThingAt(c, cat) != null;
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0006F360 File Offset: 0x0006D560
		public Thing ThingAt(IntVec3 c, ThingCategory cat)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == cat)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0006F3C4 File Offset: 0x0006D5C4
		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0006F3D4 File Offset: 0x0006D5D4
		public Thing ThingAt(IntVec3 c, ThingDef def)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0006F434 File Offset: 0x0006D634
		public T ThingAt<T>(IntVec3 c) where T : Thing
		{
			if (!c.InBounds(this.map))
			{
				return default(T);
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x04001100 RID: 4352
		private Map map;

		// Token: 0x04001101 RID: 4353
		private List<Thing>[] thingGrid;

		// Token: 0x04001102 RID: 4354
		private static readonly List<Thing> EmptyThingList = new List<Thing>();
	}
}
