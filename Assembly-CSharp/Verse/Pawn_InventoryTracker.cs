using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020004E5 RID: 1253
	[StaticConstructorOnStartup]
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x000E1D0E File Offset: 0x000DFF0E
		// (set) Token: 0x06002520 RID: 9504 RVA: 0x000E1D20 File Offset: 0x000DFF20
		public bool UnloadEverything
		{
			get
			{
				return this.unloadEverything && this.HasAnyUnloadableThing;
			}
			set
			{
				if (value && this.HasAnyUnloadableThing)
				{
					this.unloadEverything = true;
					return;
				}
				this.unloadEverything = false;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x000E1D3C File Offset: 0x000DFF3C
		public bool HasAnyUnpackedCaravanItems
		{
			get
			{
				return this.unpackedCaravanItems.Count > 0;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x000E1D4C File Offset: 0x000DFF4C
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x000E1D70 File Offset: 0x000DFF70
		public ThingCount FirstUnloadableThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return default(ThingCount);
				}
				Pawn_DrugPolicyTracker drugs = this.pawn.drugs;
				if (((drugs != null) ? drugs.CurrentPolicy : null) != null)
				{
					DrugPolicy currentPolicy = this.pawn.drugs.CurrentPolicy;
					Pawn_InventoryTracker.tmpItemsToKeep.Clear();
					for (int i = 0; i < currentPolicy.Count; i++)
					{
						if (currentPolicy[i].takeToInventory > 0)
						{
							Pawn_InventoryTracker.tmpItemsToKeep.Add(new ThingDefCount(currentPolicy[i].drug, currentPolicy[i].takeToInventory));
						}
					}
				}
				Pawn_InventoryStockTracker inventoryStock = this.pawn.inventoryStock;
				bool flag;
				if (inventoryStock == null)
				{
					flag = false;
				}
				else
				{
					Dictionary<InventoryStockGroupDef, InventoryStockEntry> stockEntries = inventoryStock.stockEntries;
					int? num = (stockEntries != null) ? new int?(stockEntries.Count) : null;
					int num2 = 0;
					flag = (num.GetValueOrDefault() > num2 & num != null);
				}
				if (flag)
				{
					foreach (InventoryStockEntry inventoryStockEntry in this.pawn.inventoryStock.stockEntries.Values)
					{
						Pawn_InventoryTracker.tmpItemsToKeep.Add(new ThingDefCount(inventoryStockEntry.thingDef, inventoryStockEntry.count));
					}
				}
				foreach (Thing thing in this.innerContainer)
				{
					int num3 = -1;
					for (int j = 0; j < Pawn_InventoryTracker.tmpItemsToKeep.Count; j++)
					{
						if (thing.def == Pawn_InventoryTracker.tmpItemsToKeep[j].ThingDef)
						{
							num3 = j;
							break;
						}
					}
					if (this.pawn.IsColonist && thing.def.IsNutritionGivingIngestible && !thing.def.IsDrug && JobGiver_PackFood.IsGoodPackableFoodFor(thing, this.pawn, false))
					{
						float inventoryPackableFoodNutrition = JobGiver_PackFood.GetInventoryPackableFoodNutrition(this.pawn);
						float maxLevel = this.pawn.needs.food.MaxLevel;
						if (inventoryPackableFoodNutrition - thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thing.stackCount <= maxLevel)
						{
							int num4 = 0;
							while (inventoryPackableFoodNutrition - thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)num4 > maxLevel)
							{
								num4++;
							}
							if (thing.stackCount - num4 > 0)
							{
								Pawn_InventoryTracker.tmpItemsToKeep.Add(new ThingDefCount(thing.def, thing.stackCount - num4));
								num3 = Pawn_InventoryTracker.tmpItemsToKeep.Count - 1;
							}
						}
					}
					if (num3 < 0)
					{
						return new ThingCount(thing, thing.stackCount, false);
					}
					if (thing.stackCount > Pawn_InventoryTracker.tmpItemsToKeep[num3].Count)
					{
						return new ThingCount(thing, thing.stackCount - Pawn_InventoryTracker.tmpItemsToKeep[num3].Count, false);
					}
					Pawn_InventoryTracker.tmpItemsToKeep[num3] = new ThingDefCount(Pawn_InventoryTracker.tmpItemsToKeep[num3].ThingDef, Pawn_InventoryTracker.tmpItemsToKeep[num3].Count - thing.stackCount);
				}
				return default(ThingCount);
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x000E20F8 File Offset: 0x000E02F8
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x000E2100 File Offset: 0x000E0300
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000E2140 File Offset: 0x000E0340
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.unpackedCaravanItems, "unpackedCaravanItems", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x000E21A5 File Offset: 0x000E03A5
		public void InventoryTrackerTick()
		{
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000E21BE File Offset: 0x000E03BE
		public void DropAllNearPawn(IntVec3 pos, bool forbid = false, bool unforbid = false)
		{
			this.DropAllNearPawnHelper(pos, forbid, unforbid, false);
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000E21CC File Offset: 0x000E03CC
		private void DropAllNearPawnHelper(IntVec3 pos, bool forbid = false, bool unforbid = false, bool caravanHaulOnly = false)
		{
			if (this.pawn.MapHeld == null)
			{
				string str = "Tried to drop all inventory near pawn but the pawn is unspawned. pawn=";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			if (caravanHaulOnly)
			{
				Pawn_InventoryTracker.tmpThingList.AddRange(this.unpackedCaravanItems);
			}
			else
			{
				Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			}
			int i;
			Action<Thing, int> <>9__0;
			int j;
			for (i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i = j + 1)
			{
				if (caravanHaulOnly && !this.innerContainer.Contains(Pawn_InventoryTracker.tmpThingList[i]))
				{
					this.unpackedCaravanItems.Remove(Pawn_InventoryTracker.tmpThingList[i]);
					Log.Warning("Could not drop unpacked caravan item " + Pawn_InventoryTracker.tmpThingList[i].Label + ", inventory no longer contains it");
				}
				else
				{
					ThingOwner<Thing> thingOwner = this.innerContainer;
					Thing thing = Pawn_InventoryTracker.tmpThingList[i];
					Map mapHeld = this.pawn.MapHeld;
					ThingPlaceMode mode = ThingPlaceMode.Near;
					Action<Thing, int> placedAction;
					if ((placedAction = <>9__0) == null)
					{
						placedAction = (<>9__0 = delegate(Thing t, int unused)
						{
							if (forbid)
							{
								t.SetForbiddenIfOutsideHomeArea();
							}
							if (unforbid)
							{
								t.SetForbidden(false, false);
							}
							if (t.def.IsPleasureDrug)
							{
								LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
							}
							Lord formAndSendCaravanLord = CaravanFormingUtility.GetFormAndSendCaravanLord(this.pawn);
							LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = ((formAndSendCaravanLord != null) ? formAndSendCaravanLord.LordJob : null) as LordJob_FormAndSendCaravan;
							if (caravanHaulOnly && lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.GatheringItemsNow)
							{
								CaravanFormingUtility.TryAddItemBackToTransferables(t, lordJob_FormAndSendCaravan.transferables, Pawn_InventoryTracker.tmpThingList[i].stackCount);
							}
							this.unpackedCaravanItems.Remove(Pawn_InventoryTracker.tmpThingList[i]);
						});
					}
					Thing thing2;
					thingOwner.TryDrop(thing, pos, mapHeld, mode, out thing2, placedAction, null);
				}
				j = i;
			}
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x000E2348 File Offset: 0x000E0548
		public void DropCount(ThingDef def, int count, bool forbid = false, bool unforbid = false)
		{
			if (this.pawn.MapHeld == null)
			{
				string str = "Tried to drop a thing near pawn but the pawn is unspawned. pawn=";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			int num = 0;
			Action<Thing, int> <>9__0;
			for (int i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i++)
			{
				Thing thing = Pawn_InventoryTracker.tmpThingList[i];
				if (thing.def == def)
				{
					int num2 = Math.Min(thing.stackCount, count);
					ThingOwner<Thing> thingOwner = this.innerContainer;
					Thing thing2 = Pawn_InventoryTracker.tmpThingList[i];
					IntVec3 position = this.pawn.Position;
					Map mapHeld = this.pawn.MapHeld;
					ThingPlaceMode mode = ThingPlaceMode.Near;
					int count2 = num2;
					Action<Thing, int> placedAction;
					if ((placedAction = <>9__0) == null)
					{
						placedAction = (<>9__0 = delegate(Thing t, int unused)
						{
							if (forbid)
							{
								t.SetForbiddenIfOutsideHomeArea();
							}
							if (unforbid)
							{
								t.SetForbidden(false, false);
							}
							if (t.def.IsPleasureDrug)
							{
								LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
							}
						});
					}
					Thing thing3;
					thingOwner.TryDrop(thing2, position, mapHeld, mode, count2, out thing3, placedAction, null);
					num += num2;
					if (num >= count)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000E2450 File Offset: 0x000E0650
		public void RemoveCount(ThingDef def, int count, bool destroy = true)
		{
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			foreach (Thing thing in Pawn_InventoryTracker.tmpThingList)
			{
				if (thing.def == def)
				{
					if (thing.stackCount > count)
					{
						thing.stackCount -= count;
						break;
					}
					this.innerContainer.Remove(thing);
					if (destroy)
					{
						thing.Destroy(DestroyMode.Vanish);
						break;
					}
					break;
				}
			}
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000E24F0 File Offset: 0x000E06F0
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000E24FE File Offset: 0x000E06FE
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000E250C File Offset: 0x000E070C
		public int Count(ThingDef def)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.def == def)
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x000E2570 File Offset: 0x000E0770
		public int Count(Func<Thing, bool> validator)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (validator(thing))
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000E25D4 File Offset: 0x000E07D4
		public void AddHauledCaravanItem(Thing item)
		{
			Thing thing;
			if (this.pawn.carryTracker.innerContainer.TryTransferToContainer(item, this.innerContainer, item.stackCount, out thing, false) > 0)
			{
				this.unpackedCaravanItems.Add(thing);
			}
			CompForbiddable compForbiddable = (thing != null) ? thing.TryGetComp<CompForbiddable>() : null;
			if (compForbiddable != null)
			{
				compForbiddable.Forbidden = false;
			}
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000E262C File Offset: 0x000E082C
		public void TryAddAndUnforbid(Thing item)
		{
			CompForbiddable compForbiddable = item.TryGetComp<CompForbiddable>();
			if (this.innerContainer.TryAdd(item, true) && compForbiddable != null)
			{
				compForbiddable.Forbidden = false;
			}
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000E265C File Offset: 0x000E085C
		public void TransferCaravanItemsToCarrier(Pawn_InventoryTracker carrierInventory)
		{
			List<Thing> list = new List<Thing>();
			list.AddRange(this.pawn.inventory.unpackedCaravanItems);
			foreach (Thing thing in list)
			{
				if (MassUtility.IsOverEncumbered(carrierInventory.pawn))
				{
					break;
				}
				if (this.innerContainer.Contains(thing))
				{
					this.pawn.inventory.innerContainer.TryTransferToContainer(thing, carrierInventory.innerContainer, thing.stackCount, true);
				}
				this.unpackedCaravanItems.Remove(thing);
			}
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000E270C File Offset: 0x000E090C
		public void DropAllPackingCaravanThings()
		{
			if (this.pawn.Spawned)
			{
				this.DropAllNearPawnHelper(this.pawn.Position, false, false, true);
				this.ClearHaulingCaravanCache();
			}
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000E2735 File Offset: 0x000E0935
		public void ClearHaulingCaravanCache()
		{
			this.unpackedCaravanItems.Clear();
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000E2742 File Offset: 0x000E0942
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000E2750 File Offset: 0x000E0950
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000E276D File Offset: 0x000E096D
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			this.unpackedCaravanItems.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000E27A0 File Offset: 0x000E09A0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000E27A8 File Offset: 0x000E09A8
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000E27B6 File Offset: 0x000E09B6
		public IEnumerable<Thing> GetDrugs()
		{
			Pawn_InventoryTracker.<GetDrugs>d__40 <GetDrugs>d__ = new Pawn_InventoryTracker.<GetDrugs>d__40(-2);
			<GetDrugs>d__.<>4__this = this;
			return <GetDrugs>d__;
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000E27C6 File Offset: 0x000E09C6
		public IEnumerable<Thing> GetCombatEnhancingDrugs()
		{
			Pawn_InventoryTracker.<GetCombatEnhancingDrugs>d__41 <GetCombatEnhancingDrugs>d__ = new Pawn_InventoryTracker.<GetCombatEnhancingDrugs>d__41(-2);
			<GetCombatEnhancingDrugs>d__.<>4__this = this;
			return <GetCombatEnhancingDrugs>d__;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000E27D6 File Offset: 0x000E09D6
		public Thing FindCombatEnhancingDrug()
		{
			return this.GetCombatEnhancingDrugs().FirstOrDefault<Thing>();
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000E27E3 File Offset: 0x000E09E3
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_InventoryTracker.<GetGizmos>d__44 <GetGizmos>d__ = new Pawn_InventoryTracker.<GetGizmos>d__44(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x04001AE3 RID: 6883
		public Pawn pawn;

		// Token: 0x04001AE4 RID: 6884
		public ThingOwner<Thing> innerContainer;

		// Token: 0x04001AE5 RID: 6885
		private bool unloadEverything;

		// Token: 0x04001AE6 RID: 6886
		private List<Thing> itemsNotForSale = new List<Thing>();

		// Token: 0x04001AE7 RID: 6887
		private List<Thing> unpackedCaravanItems = new List<Thing>();

		// Token: 0x04001AE8 RID: 6888
		public static readonly Texture2D DrugTex = ContentFinder<Texture2D>.Get("UI/Commands/TakeDrug", true);

		// Token: 0x04001AE9 RID: 6889
		private static List<ThingDefCount> tmpItemsToKeep = new List<ThingDefCount>();

		// Token: 0x04001AEA RID: 6890
		private static readonly List<Thing> tmpThingList = new List<Thing>();

		// Token: 0x04001AEB RID: 6891
		private List<Thing> usableDrugsTmp = new List<Thing>();
	}
}
