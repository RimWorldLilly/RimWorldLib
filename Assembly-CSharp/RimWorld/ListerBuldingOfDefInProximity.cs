using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127E RID: 4734
	public class ListerBuldingOfDefInProximity
	{
		// Token: 0x06007541 RID: 30017 RVA: 0x0029297C File Offset: 0x00290B7C
		public ListerBuldingOfDefInProximity(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007542 RID: 30018 RVA: 0x00292996 File Offset: 0x00290B96
		public void Notify_BuildingSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x06007543 RID: 30019 RVA: 0x002929A3 File Offset: 0x00290BA3
		public void Notify_BuildingDeSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x06007544 RID: 30020 RVA: 0x002929B0 File Offset: 0x00290BB0
		public List<Thing> GetForCell(IntVec3 cell, float radius, List<MeditationFocusOffsetPerBuilding> defs, Thing forThing = null)
		{
			ListerBuldingOfDefInProximity.CellRequest key = new ListerBuldingOfDefInProximity.CellRequest(cell, radius, defs, forThing);
			List<Thing> list;
			if (!this.requestCache.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				using (IEnumerator<Thing> enumerator = GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing t = enumerator.Current;
						if (defs.Any((MeditationFocusOffsetPerBuilding d) => d.building == t.def) && t.GetRoom(RegionType.Set_All) == cell.GetRoom(this.map) && t != forThing)
						{
							list.Add(t);
						}
					}
				}
				list.SortBy(delegate(Thing t)
				{
					float num = t.Position.DistanceTo(cell);
					MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = defs.FirstOrDefault((MeditationFocusOffsetPerBuilding d) => d.building == t.def);
					if (meditationFocusOffsetPerBuilding != null)
					{
						num -= meditationFocusOffsetPerBuilding.offset * 100000f;
					}
					return num;
				});
				this.requestCache[key] = list;
			}
			return list;
		}

		// Token: 0x06007545 RID: 30021 RVA: 0x00292AC8 File Offset: 0x00290CC8
		public List<Thing> GetForCell(IntVec3 cell, float radius, ThingDef def, Thing forThing = null)
		{
			ListerBuldingOfDefInProximity.CellRequest key = new ListerBuldingOfDefInProximity.CellRequest(cell, radius, def, forThing);
			List<Thing> list;
			if (!this.requestCache.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					if (def == thing.def && thing.GetRoom(RegionType.Set_All) == cell.GetRoom(this.map) && thing != forThing)
					{
						list.Add(thing);
					}
				}
				this.requestCache[key] = list;
			}
			return list;
		}

		// Token: 0x04004978 RID: 18808
		private Map map;

		// Token: 0x04004979 RID: 18809
		private Dictionary<ListerBuldingOfDefInProximity.CellRequest, List<Thing>> requestCache = new Dictionary<ListerBuldingOfDefInProximity.CellRequest, List<Thing>>();

		// Token: 0x02003178 RID: 12664
		public struct CellRequest : IEquatable<ListerBuldingOfDefInProximity.CellRequest>
		{
			// Token: 0x0601290C RID: 76044 RVA: 0x00639CC0 File Offset: 0x00637EC0
			public CellRequest(IntVec3 c, float r, List<MeditationFocusOffsetPerBuilding> d, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.defs = d;
				this.forThing = t;
				this.def = null;
			}

			// Token: 0x0601290D RID: 76045 RVA: 0x00639CE6 File Offset: 0x00637EE6
			public CellRequest(IntVec3 c, float r, ThingDef d, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.def = d;
				this.defs = null;
				this.forThing = t;
			}

			// Token: 0x0601290E RID: 76046 RVA: 0x00639D0C File Offset: 0x00637F0C
			public bool Equals(ListerBuldingOfDefInProximity.CellRequest other)
			{
				return this.cell.Equals(other.cell) && this.radius.Equals(other.radius) && GenCollection.ListsEqual<MeditationFocusOffsetPerBuilding>(this.defs, other.defs) && this.def == other.def && this.forThing == other.forThing;
			}

			// Token: 0x0601290F RID: 76047 RVA: 0x00639D74 File Offset: 0x00637F74
			public override int GetHashCode()
			{
				int num = this.cell.GetHashCode();
				num = (num * 397 ^ this.radius.GetHashCode());
				if (this.forThing != null)
				{
					num = (num * 397 ^ this.forThing.GetHashCode());
				}
				if (this.defs != null)
				{
					for (int i = 0; i < this.defs.Count; i++)
					{
						num ^= this.defs[i].GetHashCode();
					}
				}
				if (this.def != null)
				{
					num ^= this.def.GetHashCode();
				}
				return num;
			}

			// Token: 0x0400C810 RID: 51216
			public readonly IntVec3 cell;

			// Token: 0x0400C811 RID: 51217
			public readonly float radius;

			// Token: 0x0400C812 RID: 51218
			public readonly Thing forThing;

			// Token: 0x0400C813 RID: 51219
			public readonly ThingDef def;

			// Token: 0x0400C814 RID: 51220
			public readonly List<MeditationFocusOffsetPerBuilding> defs;
		}
	}
}
