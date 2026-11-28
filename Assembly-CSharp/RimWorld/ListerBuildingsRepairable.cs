using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127C RID: 4732
	public class ListerBuildingsRepairable
	{
		// Token: 0x06007532 RID: 30002 RVA: 0x0029258C File Offset: 0x0029078C
		public List<Thing> RepairableBuildings(Faction fac)
		{
			return this.ListFor(fac);
		}

		// Token: 0x06007533 RID: 30003 RVA: 0x00292595 File Offset: 0x00290795
		public bool Contains(Faction fac, Building b)
		{
			return this.HashSetFor(fac).Contains(b);
		}

		// Token: 0x06007534 RID: 30004 RVA: 0x002925A4 File Offset: 0x002907A4
		public void Notify_BuildingSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x06007535 RID: 30005 RVA: 0x002925B6 File Offset: 0x002907B6
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.ListFor(b.Faction).Remove(b);
			this.HashSetFor(b.Faction).Remove(b);
		}

		// Token: 0x06007536 RID: 30006 RVA: 0x002925E7 File Offset: 0x002907E7
		public void Notify_BuildingTookDamage(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x06007537 RID: 30007 RVA: 0x002925F9 File Offset: 0x002907F9
		public void Notify_BuildingRepaired(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x06007538 RID: 30008 RVA: 0x0029260C File Offset: 0x0029080C
		private void UpdateBuilding(Building b)
		{
			if (b.Faction == null || !b.def.building.repairable)
			{
				return;
			}
			List<Thing> list = this.ListFor(b.Faction);
			HashSet<Thing> hashSet = this.HashSetFor(b.Faction);
			if (b.HitPoints < b.MaxHitPoints)
			{
				if (!list.Contains(b))
				{
					list.Add(b);
				}
				hashSet.Add(b);
				return;
			}
			list.Remove(b);
			hashSet.Remove(b);
		}

		// Token: 0x06007539 RID: 30009 RVA: 0x00292688 File Offset: 0x00290888
		private List<Thing> ListFor(Faction fac)
		{
			List<Thing> list;
			if (!this.repairables.TryGetValue(fac, out list))
			{
				list = new List<Thing>();
				this.repairables.Add(fac, list);
			}
			return list;
		}

		// Token: 0x0600753A RID: 30010 RVA: 0x002926BC File Offset: 0x002908BC
		private HashSet<Thing> HashSetFor(Faction fac)
		{
			HashSet<Thing> hashSet;
			if (!this.repairablesSet.TryGetValue(fac, out hashSet))
			{
				hashSet = new HashSet<Thing>();
				this.repairablesSet.Add(fac, hashSet);
			}
			return hashSet;
		}

		// Token: 0x0600753B RID: 30011 RVA: 0x002926F0 File Offset: 0x002908F0
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				List<Thing> list = this.ListFor(faction);
				if (!list.NullOrEmpty<Thing>())
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string[] array = new string[5];
					array[0] = "=======";
					array[1] = faction.Name;
					array[2] = " (";
					int num = 3;
					FactionDef def = faction.def;
					array[num] = ((def != null) ? def.ToString() : null);
					array[4] = ")";
					stringBuilder2.AppendLine(string.Concat(array));
					foreach (Thing thing in list)
					{
						stringBuilder.AppendLine(thing.ThingID);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004974 RID: 18804
		private Dictionary<Faction, List<Thing>> repairables = new Dictionary<Faction, List<Thing>>();

		// Token: 0x04004975 RID: 18805
		private Dictionary<Faction, HashSet<Thing>> repairablesSet = new Dictionary<Faction, HashSet<Thing>>();
	}
}
