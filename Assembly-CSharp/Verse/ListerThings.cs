using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000292 RID: 658
	public sealed class ListerThings
	{
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x0007026D File Offset: 0x0006E46D
		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00070278 File Offset: 0x0006E478
		public ListerThings(ListerThingsUse use, ThingListChangedCallbacks thingListChangedCallbacks = null)
		{
			this.use = use;
			this.thingListChangedCallbacks = thingListChangedCallbacks;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.stateHashByGroup = new int[ThingListGroupHelper.AllGroups.Length];
			this.haulSources = new List<IHaulSource>();
			this.listsByGroup[2] = new List<Thing>();
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x000702E5 File Offset: 0x0006E4E5
		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x000702F4 File Offset: 0x0006E4F4
		public int StateHashOfGroup(ThingRequestGroup group)
		{
			if (this.use == ListerThingsUse.Region && !group.StoreInRegion())
			{
				Log.ErrorOnce("Tried to get state hash of group " + group.ToString() + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968738832);
				return -1;
			}
			return Gen.HashCombineInt(85693994, this.stateHashByGroup[(int)group]);
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0007034C File Offset: 0x0006E54C
		public List<Thing> ThingsOfDef(ThingDef def)
		{
			if (def == ThingDefOf.MinifiedThing)
			{
				Log.ErrorOnce("Tried to get ThingsOfDef of MinifiedThing, use ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MinifiedThing)) instead", 1927834012);
			}
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x00070371 File Offset: 0x0006E571
		public bool AnyThingWithDef(ThingDef def)
		{
			return this.listsByDef.ContainsKey(def) && this.listsByDef[def].Count > 0;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00070398 File Offset: 0x0006E598
		public List<Thing> ThingsMatching(ThingRequest req)
		{
			if (req.singleDef != null)
			{
				return this.listsByDef.GetValueOrDefault(req.singleDef, ListerThings.EmptyList);
			}
			if (req.group == ThingRequestGroup.Undefined)
			{
				string str = "Invalid ThingRequest ";
				ThingRequest thingRequest = req;
				throw new InvalidOperationException(str + thingRequest.ToString());
			}
			if (this.use == ListerThingsUse.Region && !req.group.StoreInRegion())
			{
				Log.ErrorOnce("Tried to get things in group " + req.group.ToString() + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968735132);
				return ListerThings.EmptyList;
			}
			return this.listsByGroup[(int)req.group] ?? ListerThings.EmptyList;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00070448 File Offset: 0x0006E648
		public List<Thing> ThingsMatchingFilter(ThingFilter filter)
		{
			ListerThings.tmpThingsMatchingFilter.Clear();
			foreach (ThingDef def in filter.AllowedThingDefs)
			{
				ListerThings.tmpThingsMatchingFilter.AddRange(this.ThingsOfDef(def));
			}
			return ListerThings.tmpThingsMatchingFilter;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x000704B0 File Offset: 0x0006E6B0
		public void GetThingsOfType<T>(List<T> list) where T : Thing
		{
			if (typeof(T) == typeof(Thing))
			{
				Log.Error("Do not call this method with type 'Thing' directly, as it will return all things currently registered.");
				return;
			}
			List<Thing> allThings = this.AllThings;
			for (int i = 0; i < this.AllThings.Count; i++)
			{
				T t = allThings[i] as T;
				if (t != null)
				{
					list.Add(t);
				}
			}
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00070521 File Offset: 0x0006E721
		public IEnumerable<T> GetThingsOfType<T>() where T : Thing
		{
			ListerThings.<GetThingsOfType>d__18<T> <GetThingsOfType>d__ = new ListerThings.<GetThingsOfType>d__18<T>(-2);
			<GetThingsOfType>d__.<>4__this = this;
			return <GetThingsOfType>d__;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00070531 File Offset: 0x0006E731
		public IEnumerable<Thing> GetAllThings(Predicate<Thing> validator = null, bool lookInHaulSources = false)
		{
			ListerThings.<GetAllThings>d__19 <GetAllThings>d__ = new ListerThings.<GetAllThings>d__19(-2);
			<GetAllThings>d__.<>4__this = this;
			<GetAllThings>d__.<>3__validator = validator;
			<GetAllThings>d__.<>3__lookInHaulSources = lookInHaulSources;
			return <GetAllThings>d__;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00070550 File Offset: 0x0006E750
		public void GetAllThings(in List<Thing> list, Predicate<Thing> validator = null, bool lookInHaulSources = false)
		{
			if (validator != null)
			{
				using (List<Thing>.Enumerator enumerator = this.AllThings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing thing = enumerator.Current;
						if (validator(thing))
						{
							list.Add(thing);
						}
					}
					goto IL_50;
				}
			}
			list.AddRange(this.AllThings);
			IL_50:
			if (!lookInHaulSources)
			{
				return;
			}
			foreach (IHaulSource haulSource in this.haulSources)
			{
				foreach (Thing thing2 in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					if (validator == null || validator(thing2))
					{
						list.Add(thing2);
					}
				}
			}
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00070648 File Offset: 0x0006E848
		public void GetAllThings(in List<Thing> list, ThingRequestGroup group, Predicate<Thing> validator = null, bool lookInHaulSources = false)
		{
			if (validator != null)
			{
				using (List<Thing>.Enumerator enumerator = this.ThingsInGroup(group).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing thing = enumerator.Current;
						if (validator(thing))
						{
							list.Add(thing);
						}
					}
					goto IL_52;
				}
			}
			list.AddRange(this.ThingsInGroup(group));
			IL_52:
			if (!lookInHaulSources)
			{
				return;
			}
			foreach (IHaulSource haulSource in this.haulSources)
			{
				foreach (Thing thing2 in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					if (this.GroupIncludes(thing2, group) && (validator == null || validator(thing2)))
					{
						list.Add(thing2);
					}
				}
			}
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0007074C File Offset: 0x0006E94C
		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0007075C File Offset: 0x0006E95C
		public void Add(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			List<Thing> list;
			if (!this.listsByDef.TryGetValue(t.def, out list))
			{
				list = new List<Thing>();
				this.listsByDef.Add(t.def, list);
			}
			list.Add(t);
			IHaulSource haulSource = t as IHaulSource;
			if (haulSource != null)
			{
				this.haulSources.Add(haulSource);
			}
			foreach (ThingRequestGroup thingRequestGroup in ThingListGroupHelper.AllGroups)
			{
				if (this.GroupIncludes(t, thingRequestGroup))
				{
					List<Thing> list2 = this.listsByGroup[(int)thingRequestGroup];
					if (list2 == null)
					{
						list2 = new List<Thing>();
						this.listsByGroup[(int)thingRequestGroup] = list2;
						this.stateHashByGroup[(int)thingRequestGroup] = 0;
					}
					list2.Add(t);
					this.stateHashByGroup[(int)thingRequestGroup]++;
				}
			}
			ThingListChangedCallbacks thingListChangedCallbacks = this.thingListChangedCallbacks;
			if (thingListChangedCallbacks == null)
			{
				return;
			}
			Action<Thing> onThingAdded = thingListChangedCallbacks.onThingAdded;
			if (onThingAdded == null)
			{
				return;
			}
			onThingAdded(t);
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0007084B File Offset: 0x0006EA4B
		private bool GroupIncludes(Thing thing, ThingRequestGroup group)
		{
			return (this.use != ListerThingsUse.Region || group.StoreInRegion()) && group.Includes(thing.def);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0007086C File Offset: 0x0006EA6C
		public void Remove(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			List<Thing> list;
			if (this.listsByDef.TryGetValue(t.def, out list))
			{
				ListerThings.RemoveSwap(list, t);
			}
			IHaulSource haulSource = t as IHaulSource;
			if (haulSource != null)
			{
				this.haulSources.Remove(haulSource);
			}
			ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
			for (int i = 0; i < allGroups.Length; i++)
			{
				ThingRequestGroup thingRequestGroup = allGroups[i];
				if ((this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
				{
					ListerThings.RemoveSwap(this.listsByGroup[i], t);
					this.stateHashByGroup[(int)thingRequestGroup]++;
				}
			}
			ThingListChangedCallbacks thingListChangedCallbacks = this.thingListChangedCallbacks;
			if (thingListChangedCallbacks == null)
			{
				return;
			}
			Action<Thing> onThingRemoved = thingListChangedCallbacks.onThingRemoved;
			if (onThingRemoved == null)
			{
				return;
			}
			onThingRemoved(t);
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x00070938 File Offset: 0x0006EB38
		private static void RemoveSwap(List<Thing> list, Thing t)
		{
			int num = list.IndexOf(t);
			if (num < 0)
			{
				return;
			}
			int index = list.Count - 1;
			list[num] = list[index];
			list.RemoveAt(index);
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00070970 File Offset: 0x0006EB70
		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			return (def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) && (def.category != ThingCategory.Projectile || use != ListerThingsUse.Region);
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0007099C File Offset: 0x0006EB9C
		public void Clear()
		{
			this.listsByDef.Clear();
			for (int i = 0; i < this.listsByGroup.Length; i++)
			{
				if (this.listsByGroup[i] != null)
				{
					this.listsByGroup[i].Clear();
				}
				this.stateHashByGroup[i] = 0;
			}
		}

		// Token: 0x0400111C RID: 4380
		private readonly Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		// Token: 0x0400111D RID: 4381
		private readonly List<Thing>[] listsByGroup;

		// Token: 0x0400111E RID: 4382
		private readonly List<IHaulSource> haulSources;

		// Token: 0x0400111F RID: 4383
		private readonly int[] stateHashByGroup;

		// Token: 0x04001120 RID: 4384
		public ListerThingsUse use;

		// Token: 0x04001121 RID: 4385
		public ThingListChangedCallbacks thingListChangedCallbacks;

		// Token: 0x04001122 RID: 4386
		private static readonly List<Thing> EmptyList = new List<Thing>();

		// Token: 0x04001123 RID: 4387
		private static readonly List<Thing> tmpThingsMatchingFilter = new List<Thing>(1024);
	}
}
