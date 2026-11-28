using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127D RID: 4733
	public class ListerBuildingWithTagInProximity
	{
		// Token: 0x0600753D RID: 30013 RVA: 0x0029280E File Offset: 0x00290A0E
		public ListerBuildingWithTagInProximity(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600753E RID: 30014 RVA: 0x00292828 File Offset: 0x00290A28
		public void Notify_BuildingSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x0600753F RID: 30015 RVA: 0x00292835 File Offset: 0x00290A35
		public void Notify_BuildingDeSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x06007540 RID: 30016 RVA: 0x00292844 File Offset: 0x00290A44
		public List<Thing> GetForCell(IntVec3 cell, float radius, string tag, Thing forThing = null)
		{
			ListerBuildingWithTagInProximity.CellRequest key = new ListerBuildingWithTagInProximity.CellRequest(cell, radius, tag, forThing);
			List<Thing> list;
			if (!this.requestCache.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					ThingDef thingDef = thing.def;
					if (thing.def.IsBlueprint || thing.def.isFrameInt)
					{
						ThingDef thingDef2 = thing.def.entityDefToBuild as ThingDef;
						if (thingDef2 != null)
						{
							thingDef = thingDef2;
						}
					}
					if (thingDef.building != null && thingDef.building.buildingTags.Contains(tag) && thing.GetRoom(RegionType.Set_All) == cell.GetRoom(this.map) && thing != forThing)
					{
						list.Add(thing);
					}
				}
				list.SortBy((Thing t) => t.Position.DistanceTo(cell));
				this.requestCache[key] = list;
			}
			return list;
		}

		// Token: 0x04004976 RID: 18806
		private Map map;

		// Token: 0x04004977 RID: 18807
		private Dictionary<ListerBuildingWithTagInProximity.CellRequest, List<Thing>> requestCache = new Dictionary<ListerBuildingWithTagInProximity.CellRequest, List<Thing>>();

		// Token: 0x02003176 RID: 12662
		public struct CellRequest : IEquatable<ListerBuildingWithTagInProximity.CellRequest>
		{
			// Token: 0x06012907 RID: 76039 RVA: 0x00639BC7 File Offset: 0x00637DC7
			public CellRequest(IntVec3 c, float r, string tag, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.tag = tag;
				this.forThing = t;
			}

			// Token: 0x06012908 RID: 76040 RVA: 0x00639BE8 File Offset: 0x00637DE8
			public bool Equals(ListerBuildingWithTagInProximity.CellRequest other)
			{
				return this.cell.Equals(other.cell) && this.radius.Equals(other.radius) && this.tag == other.tag && this.forThing == other.forThing;
			}

			// Token: 0x06012909 RID: 76041 RVA: 0x00639C44 File Offset: 0x00637E44
			public override int GetHashCode()
			{
				int num = this.cell.GetHashCode();
				num = (num * 397 ^ this.radius.GetHashCode());
				if (this.forThing != null)
				{
					num = (num * 397 ^ this.forThing.GetHashCode());
				}
				return num ^ this.tag.GetHashCode();
			}

			// Token: 0x0400C80B RID: 51211
			public readonly IntVec3 cell;

			// Token: 0x0400C80C RID: 51212
			public readonly float radius;

			// Token: 0x0400C80D RID: 51213
			public readonly Thing forThing;

			// Token: 0x0400C80E RID: 51214
			public readonly string tag;
		}
	}
}
