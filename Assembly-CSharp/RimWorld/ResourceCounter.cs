using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001372 RID: 4978
	public sealed class ResourceCounter
	{
		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x06007B6A RID: 31594 RVA: 0x002BF148 File Offset: 0x002BD348
		public int Silver
		{
			get
			{
				return this.GetCount(ThingDefOf.Silver);
			}
		}

		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x06007B6B RID: 31595 RVA: 0x002BF155 File Offset: 0x002BD355
		public Dictionary<ThingDef, int> AllCountedAmounts
		{
			get
			{
				return this.countedAmounts;
			}
		}

		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x06007B6C RID: 31596 RVA: 0x002BF160 File Offset: 0x002BD360
		public float TotalHumanEdibleNutrition
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
				{
					if (keyValuePair.Key.IsNutritionGivingIngestible && keyValuePair.Key.ingestible.HumanEdible)
					{
						num += keyValuePair.Key.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)keyValuePair.Value;
					}
				}
				return num;
			}
		}

		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x06007B6D RID: 31597 RVA: 0x002BF1F4 File Offset: 0x002BD3F4
		public float TotalHumanBabyEdibleNutrition
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
				{
					ThingDef thingDef;
					int num2;
					keyValuePair.Deconstruct(out thingDef, out num2);
					ThingDef thingDef2 = thingDef;
					int num3 = num2;
					if (thingDef2.IsNutritionGivingIngestibleForHumanlikeBabies)
					{
						num += thingDef2.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)num3;
					}
				}
				return num;
			}
		}

		// Token: 0x06007B6E RID: 31598 RVA: 0x002BF274 File Offset: 0x002BD474
		public static void ResetDefs()
		{
			ResourceCounter.resources.Clear();
			ResourceCounter.resources.AddRange(from def in DefDatabase<ThingDef>.AllDefs
			where def.CountAsResource
			orderby def.resourceReadoutPriority descending
			select def);
		}

		// Token: 0x06007B6F RID: 31599 RVA: 0x002BF2E2 File Offset: 0x002BD4E2
		public ResourceCounter(Map map)
		{
			this.map = map;
			this.ResetResourceCounts();
		}

		// Token: 0x06007B70 RID: 31600 RVA: 0x002BF304 File Offset: 0x002BD504
		private void ResetResourceCounts()
		{
			this.countedAmounts.Clear();
			for (int i = 0; i < ResourceCounter.resources.Count; i++)
			{
				this.countedAmounts.Add(ResourceCounter.resources[i], 0);
			}
		}

		// Token: 0x06007B71 RID: 31601 RVA: 0x002BF348 File Offset: 0x002BD548
		public int GetCount(ThingDef rDef)
		{
			if (rDef.resourceReadoutPriority == ResourceCountPriority.Uncounted)
			{
				return 0;
			}
			int result;
			if (this.countedAmounts.TryGetValue(rDef, out result))
			{
				return result;
			}
			Log.Error("Looked for nonexistent key " + ((rDef != null) ? rDef.ToString() : null) + " in counted resources.");
			this.countedAmounts.Add(rDef, 0);
			return 0;
		}

		// Token: 0x06007B72 RID: 31602 RVA: 0x002BF3A0 File Offset: 0x002BD5A0
		public int GetCountIn(ThingRequestGroup group)
		{
			int num = 0;
			foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
			{
				if (group.Includes(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num;
		}

		// Token: 0x06007B73 RID: 31603 RVA: 0x002BF408 File Offset: 0x002BD608
		public int GetCountIn(ThingCategoryDef cat)
		{
			int num = 0;
			for (int i = 0; i < cat.childThingDefs.Count; i++)
			{
				num += this.GetCount(cat.childThingDefs[i]);
			}
			for (int j = 0; j < cat.childCategories.Count; j++)
			{
				if (!cat.childCategories[j].resourceReadoutRoot)
				{
					num += this.GetCountIn(cat.childCategories[j]);
				}
			}
			return num;
		}

		// Token: 0x06007B74 RID: 31604 RVA: 0x002BF481 File Offset: 0x002BD681
		public void ResourceCounterTick()
		{
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				this.UpdateResourceCounts();
			}
		}

		// Token: 0x06007B75 RID: 31605 RVA: 0x002BF49C File Offset: 0x002BD69C
		public void UpdateResourceCounts()
		{
			this.ResetResourceCounts();
			List<SlotGroup> allGroupsListForReading = this.map.haulDestinationManager.AllGroupsListForReading;
			for (int i = 0; i < allGroupsListForReading.Count; i++)
			{
				foreach (Thing outerThing in allGroupsListForReading[i].HeldThings)
				{
					Thing innerIfMinified = outerThing.GetInnerIfMinified();
					if (innerIfMinified.def.CountAsResource && this.ShouldCount(innerIfMinified))
					{
						Dictionary<ThingDef, int> dictionary = this.countedAmounts;
						ThingDef def = innerIfMinified.def;
						dictionary[def] += innerIfMinified.stackCount;
					}
				}
			}
		}

		// Token: 0x06007B76 RID: 31606 RVA: 0x002BF554 File Offset: 0x002BD754
		public void CheckUpdateResource(Thing t)
		{
			if (t == null)
			{
				return;
			}
			Thing innerIfMinified = t.GetInnerIfMinified();
			if (!innerIfMinified.def.CountAsResource || !this.ShouldCount(innerIfMinified))
			{
				return;
			}
			this.UpdateResourceCounts();
		}

		// Token: 0x06007B77 RID: 31607 RVA: 0x002BF589 File Offset: 0x002BD789
		private bool ShouldCount(Thing t)
		{
			return !t.IsNotFresh() && (!t.SpawnedOrAnyParentSpawned || !t.PositionHeld.Fogged(t.MapHeld));
		}

		// Token: 0x04004DE7 RID: 19943
		private Map map;

		// Token: 0x04004DE8 RID: 19944
		private readonly Dictionary<ThingDef, int> countedAmounts = new Dictionary<ThingDef, int>();

		// Token: 0x04004DE9 RID: 19945
		private static readonly List<ThingDef> resources = new List<ThingDef>();
	}
}
