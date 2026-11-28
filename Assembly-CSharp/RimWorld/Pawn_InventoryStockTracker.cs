using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155B RID: 5467
	public class Pawn_InventoryStockTracker : IExposable
	{
		// Token: 0x060085BD RID: 34237 RVA: 0x00304D4F File Offset: 0x00302F4F
		public Pawn_InventoryStockTracker()
		{
		}

		// Token: 0x060085BE RID: 34238 RVA: 0x00304D62 File Offset: 0x00302F62
		public Pawn_InventoryStockTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060085BF RID: 34239 RVA: 0x00304D7C File Offset: 0x00302F7C
		public InventoryStockEntry GetCurrentEntryFor(InventoryStockGroupDef group)
		{
			if (!this.stockEntries.ContainsKey(group))
			{
				this.stockEntries[group] = this.CreateDefaultEntryFor(group);
			}
			return this.stockEntries[group];
		}

		// Token: 0x060085C0 RID: 34240 RVA: 0x00304DAB File Offset: 0x00302FAB
		public int GetDesiredCountForGroup(InventoryStockGroupDef group)
		{
			return this.GetCurrentEntryFor(group).count;
		}

		// Token: 0x060085C1 RID: 34241 RVA: 0x00304DB9 File Offset: 0x00302FB9
		public ThingDef GetDesiredThingForGroup(InventoryStockGroupDef group)
		{
			return this.GetCurrentEntryFor(group).thingDef;
		}

		// Token: 0x060085C2 RID: 34242 RVA: 0x00304DC8 File Offset: 0x00302FC8
		public void SetCountForGroup(InventoryStockGroupDef group, int count)
		{
			InventoryStockEntry currentEntryFor = this.GetCurrentEntryFor(group);
			int count2 = Mathf.Clamp(count, group.min, group.max);
			currentEntryFor.count = count2;
		}

		// Token: 0x060085C3 RID: 34243 RVA: 0x00304DF8 File Offset: 0x00302FF8
		public void SetThingForGroup(InventoryStockGroupDef group, ThingDef thing)
		{
			if (!group.thingDefs.Contains(thing))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Inventory stock group ");
				defaultInterpolatedStringHandler.AppendFormatted(group.defName);
				defaultInterpolatedStringHandler.AppendLiteral(" does not contain ");
				defaultInterpolatedStringHandler.AppendFormatted(thing.defName);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.GetCurrentEntryFor(group).thingDef = thing;
		}

		// Token: 0x060085C4 RID: 34244 RVA: 0x00304E75 File Offset: 0x00303075
		private InventoryStockEntry CreateDefaultEntryFor(InventoryStockGroupDef group)
		{
			return new InventoryStockEntry
			{
				count = group.min,
				thingDef = group.DefaultThingDef
			};
		}

		// Token: 0x060085C5 RID: 34245 RVA: 0x00304E94 File Offset: 0x00303094
		public bool AnyThingsRequiredNow()
		{
			foreach (KeyValuePair<InventoryStockGroupDef, InventoryStockEntry> keyValuePair in this.stockEntries)
			{
				if (this.pawn.inventory.Count(keyValuePair.Value.thingDef) < keyValuePair.Value.count)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060085C6 RID: 34246 RVA: 0x00304F14 File Offset: 0x00303114
		public void ExposeData()
		{
			Scribe_Collections.Look<InventoryStockGroupDef, InventoryStockEntry>(ref this.stockEntries, "stockEntries", LookMode.Def, LookMode.Deep, ref this.tmpInventoryStockGroups, ref this.tmpInventoryStock, true, false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (InventoryStockGroupDef inventoryStockGroupDef in DefDatabase<InventoryStockGroupDef>.AllDefs)
				{
					InventoryStockEntry currentEntryFor = this.GetCurrentEntryFor(inventoryStockGroupDef);
					if (!inventoryStockGroupDef.thingDefs.Contains(currentEntryFor.thingDef))
					{
						currentEntryFor.thingDef = inventoryStockGroupDef.DefaultThingDef;
					}
				}
			}
		}

		// Token: 0x0400526A RID: 21098
		public Pawn pawn;

		// Token: 0x0400526B RID: 21099
		public Dictionary<InventoryStockGroupDef, InventoryStockEntry> stockEntries = new Dictionary<InventoryStockGroupDef, InventoryStockEntry>();

		// Token: 0x0400526C RID: 21100
		private List<InventoryStockGroupDef> tmpInventoryStockGroups;

		// Token: 0x0400526D RID: 21101
		private List<InventoryStockEntry> tmpInventoryStock;
	}
}
