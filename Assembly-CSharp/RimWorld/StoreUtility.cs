using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BB1 RID: 2993
	public static class StoreUtility
	{
		// Token: 0x06005644 RID: 22084 RVA: 0x00201980 File Offset: 0x001FFB80
		public static IHaulDestination CurrentHaulDestinationOf(Thing t)
		{
			if (t.Spawned)
			{
				return t.Map.haulDestinationManager.SlotGroupParentAt(t.Position);
			}
			return t.ParentHolder as IHaulDestination;
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x002019AC File Offset: 0x001FFBAC
		public static StoragePriority CurrentStoragePriorityOf(Thing t, bool forced = false)
		{
			return StoreUtility.StoragePriorityAtFor(StoreUtility.CurrentHaulDestinationOf(t), t, forced);
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x002019BB File Offset: 0x001FFBBB
		public static StoragePriority StoragePriorityAtFor(IntVec3 c, Thing t, bool forced = false)
		{
			return StoreUtility.StoragePriorityAtFor(t.Map.haulDestinationManager.SlotGroupParentAt(c), t, forced);
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x002019D8 File Offset: 0x001FFBD8
		public static StoragePriority StoragePriorityAtFor(IHaulDestination at, Thing t, bool forced = false)
		{
			if (at != null && at.Accepts(t))
			{
				if (forced)
				{
					Building building = at as Building;
					if (building != null && building.Faction != Faction.OfPlayer)
					{
						return StoragePriority.Unstored;
					}
				}
				return at.GetStoreSettings().Priority;
			}
			return StoragePriority.Unstored;
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x00201A18 File Offset: 0x001FFC18
		public static bool IsInAnyStorage(this Thing t)
		{
			return StoreUtility.CurrentHaulDestinationOf(t) != null;
		}

		// Token: 0x06005649 RID: 22089 RVA: 0x00201A24 File Offset: 0x001FFC24
		public static bool IsInValidStorage(this Thing t)
		{
			IHaulDestination haulDestination = StoreUtility.CurrentHaulDestinationOf(t);
			return haulDestination != null && haulDestination.Accepts(t);
		}

		// Token: 0x0600564A RID: 22090 RVA: 0x00201A44 File Offset: 0x001FFC44
		public static bool TryGetValidStoragePriority(this Thing t, out StoragePriority priority)
		{
			IHaulDestination haulDestination = StoreUtility.CurrentHaulDestinationOf(t);
			if (haulDestination.Accepts(t))
			{
				priority = haulDestination.GetStoreSettings().Priority;
				return true;
			}
			priority = StoragePriority.Unstored;
			return false;
		}

		// Token: 0x0600564B RID: 22091 RVA: 0x00201A74 File Offset: 0x001FFC74
		public static bool IsInValidBestStorage(this Thing t)
		{
			IHaulDestination haulDestination = StoreUtility.CurrentHaulDestinationOf(t);
			if (haulDestination != null && haulDestination.Accepts(t))
			{
				Building building = haulDestination as Building;
				if (building == null || building.Faction == Faction.OfPlayer)
				{
					IntVec3 intVec;
					IHaulDestination haulDestination2;
					return !StoreUtility.TryFindBestBetterStorageFor(t, null, t.MapHeld, haulDestination.GetStoreSettings().Priority, Faction.OfPlayer, out intVec, out haulDestination2, false);
				}
			}
			return false;
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x00201AD4 File Offset: 0x001FFCD4
		public static Thing StoringThing(this Thing t)
		{
			return StoreUtility.CurrentHaulDestinationOf(t) as Thing;
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x00201AE1 File Offset: 0x001FFCE1
		public static SlotGroup GetSlotGroup(this Thing thing)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return thing.Position.GetSlotGroup(thing.Map);
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x00201AFE File Offset: 0x001FFCFE
		public static SlotGroup GetSlotGroup(this IntVec3 c, Map map)
		{
			if (map.haulDestinationManager == null)
			{
				return null;
			}
			return map.haulDestinationManager.SlotGroupAt(c);
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x00201B16 File Offset: 0x001FFD16
		public static bool TryGetSlotGroup(this IntVec3 c, Map map, out SlotGroup group)
		{
			group = c.GetSlotGroup(map);
			return group != null;
		}

		// Token: 0x06005650 RID: 22096 RVA: 0x00201B28 File Offset: 0x001FFD28
		public static bool IsValidStorageFor(this IntVec3 c, Map map, Thing storable)
		{
			if (!StoreUtility.NoStorageBlockersIn(c, map, storable))
			{
				return false;
			}
			SlotGroup slotGroup = c.GetSlotGroup(map);
			return slotGroup != null && slotGroup.parent.Accepts(storable);
		}

		// Token: 0x06005651 RID: 22097 RVA: 0x00201B60 File Offset: 0x001FFD60
		private static bool NoStorageBlockersIn(IntVec3 c, Map map, Thing thing)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (!flag && thing2.def.EverStorable(false) && thing2.CanStackWith(thing) && thing2.stackCount < thing2.def.stackLimit)
				{
					flag = true;
				}
				if (thing2.def.entityDefToBuild != null && thing2.def.entityDefToBuild.passability != Traversability.Standable)
				{
					return false;
				}
				if (thing2.def.surfaceType == SurfaceType.None && thing2.def.passability != Traversability.Standable && (c.GetMaxItemsAllowedInCell(map) <= 1 || thing2.def.category != ThingCategory.Item))
				{
					return false;
				}
			}
			return flag || c.GetItemCount(map) < c.GetMaxItemsAllowedInCell(map);
		}

		// Token: 0x06005652 RID: 22098 RVA: 0x00201C38 File Offset: 0x001FFE38
		public static bool TryFindBestBetterStorageFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IntVec3 foundCell, out IHaulDestination haulDestination, bool needAccurateResult = true)
		{
			IntVec3 invalid = IntVec3.Invalid;
			StoragePriority storagePriority = StoragePriority.Unstored;
			if (StoreUtility.TryFindBestBetterStoreCellFor(t, carrier, map, currentPriority, faction, out invalid, needAccurateResult))
			{
				storagePriority = invalid.GetSlotGroup(map).Settings.Priority;
			}
			IHaulDestination haulDestination2;
			if (!StoreUtility.TryFindBestBetterNonSlotGroupStorageFor(t, carrier, map, currentPriority, faction, out haulDestination2, false, true))
			{
				haulDestination2 = null;
			}
			if (storagePriority == StoragePriority.Unstored && haulDestination2 == null)
			{
				foundCell = IntVec3.Invalid;
				haulDestination = null;
				return false;
			}
			if (haulDestination2 != null && (storagePriority == StoragePriority.Unstored || haulDestination2.GetStoreSettings().Priority > storagePriority))
			{
				foundCell = IntVec3.Invalid;
				haulDestination = haulDestination2;
				return true;
			}
			foundCell = invalid;
			haulDestination = invalid.GetSlotGroup(map).parent;
			return true;
		}

		// Token: 0x06005653 RID: 22099 RVA: 0x00201CDC File Offset: 0x001FFEDC
		public static bool TryFindBestBetterStoreCellFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IntVec3 foundCell, bool needAccurateResult = true)
		{
			List<SlotGroup> allGroupsListInPriorityOrder = map.haulDestinationManager.AllGroupsListInPriorityOrder;
			if (allGroupsListInPriorityOrder.Count == 0)
			{
				foundCell = IntVec3.Invalid;
				return false;
			}
			StoragePriority storagePriority = currentPriority;
			float num = 2.1474836E+09f;
			IntVec3 invalid = IntVec3.Invalid;
			int count = allGroupsListInPriorityOrder.Count;
			for (int i = 0; i < count; i++)
			{
				SlotGroup slotGroup = allGroupsListInPriorityOrder[i];
				StoragePriority priority = slotGroup.Settings.Priority;
				if (priority < storagePriority || priority <= currentPriority)
				{
					break;
				}
				Thing thing = slotGroup.parent as Thing;
				if ((thing == null || thing.Faction == faction) && slotGroup.parent.HaulDestinationEnabled)
				{
					StoreUtility.TryFindBestBetterStoreCellForWorker(t, carrier, map, faction, slotGroup, needAccurateResult, ref invalid, ref num, ref storagePriority);
				}
			}
			if (!invalid.IsValid)
			{
				foundCell = IntVec3.Invalid;
				return false;
			}
			foundCell = invalid;
			return true;
		}

		// Token: 0x06005654 RID: 22100 RVA: 0x00201DB4 File Offset: 0x001FFFB4
		public static bool TryFindBestBetterStoreCellForIn(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, ISlotGroup slotGroup, out IntVec3 foundCell, bool needAccurateResult = true)
		{
			foundCell = IntVec3.Invalid;
			float num = 2.1474836E+09f;
			StoreUtility.TryFindBestBetterStoreCellForWorker(t, carrier, map, faction, slotGroup, needAccurateResult, ref foundCell, ref num, ref currentPriority);
			return foundCell.IsValid;
		}

		// Token: 0x06005655 RID: 22101 RVA: 0x00201DF0 File Offset: 0x001FFFF0
		private static void TryFindBestBetterStoreCellForWorker(Thing t, Pawn carrier, Map map, Faction faction, ISlotGroup slotGroup, bool needAccurateResult, ref IntVec3 closestSlot, ref float closestDistSquared, ref StoragePriority foundPriority)
		{
			if (slotGroup == null)
			{
				return;
			}
			if (!slotGroup.Settings.AllowedToAccept(t))
			{
				return;
			}
			IntVec3 a = t.SpawnedOrAnyParentSpawned ? t.PositionHeld : carrier.PositionHeld;
			List<IntVec3> cellsList = slotGroup.CellsList;
			int count = cellsList.Count;
			int num;
			if (needAccurateResult)
			{
				num = Mathf.FloorToInt((float)count * Rand.Range(0.005f, 0.018f));
			}
			else
			{
				num = 0;
			}
			for (int i = 0; i < count; i++)
			{
				IntVec3 intVec = cellsList[i];
				float num2 = (float)(a - intVec).LengthHorizontalSquared;
				if (num2 <= closestDistSquared && StoreUtility.IsGoodStoreCell(intVec, map, t, carrier, faction))
				{
					closestSlot = intVec;
					closestDistSquared = num2;
					foundPriority = slotGroup.Settings.Priority;
					if (i >= num)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06005656 RID: 22102 RVA: 0x00201EBC File Offset: 0x002000BC
		public static bool TryFindBestBetterNonSlotGroupStorageFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IHaulDestination haulDestination, bool acceptSamePriority = false, bool requiresDestReservation = true)
		{
			List<IHaulDestination> allHaulDestinationsListInPriorityOrder = map.haulDestinationManager.AllHaulDestinationsListInPriorityOrder;
			IntVec3 intVec = t.SpawnedOrAnyParentSpawned ? t.PositionHeld : carrier.PositionHeld;
			float num = float.MaxValue;
			StoragePriority storagePriority = StoragePriority.Unstored;
			haulDestination = null;
			for (int i = 0; i < allHaulDestinationsListInPriorityOrder.Count; i++)
			{
				IHaulDestination haulDestination2 = allHaulDestinationsListInPriorityOrder[i];
				if (!(haulDestination2 is ISlotGroupParent) && (!(haulDestination2 is Building_Grave) || t.CanBeBuried()) && haulDestination2.HaulDestinationEnabled)
				{
					StoragePriority priority = haulDestination2.GetStoreSettings().Priority;
					if (priority < storagePriority || (acceptSamePriority && priority < currentPriority) || (!acceptSamePriority && priority <= currentPriority))
					{
						break;
					}
					float num2 = (float)intVec.DistanceToSquared(haulDestination2.Position);
					if (num2 <= num && haulDestination2.Accepts(t))
					{
						Thing thing = haulDestination2 as Thing;
						if (thing == null || thing.Faction == faction)
						{
							if (thing != null)
							{
								if (carrier != null)
								{
									if (thing.IsForbidden(carrier))
									{
										goto IL_1E5;
									}
								}
								else if (faction != null && thing.IsForbidden(faction))
								{
									goto IL_1E5;
								}
							}
							if (thing != null && requiresDestReservation)
							{
								IHaulEnroute haulEnroute = thing as IHaulEnroute;
								if (haulEnroute != null)
								{
									if (!map.reservationManager.OnlyReservationsForJobDef(thing, JobDefOf.HaulToContainer, false))
									{
										goto IL_1E5;
									}
									if (haulEnroute.GetSpaceRemainingWithEnroute(t.def, null) <= 0)
									{
										goto IL_1E5;
									}
								}
								else if (carrier != null)
								{
									if (!carrier.CanReserveNew(thing))
									{
										goto IL_1E5;
									}
								}
								else if (faction != null && map.reservationManager.IsReservedByAnyoneOf(thing, faction))
								{
									goto IL_1E5;
								}
							}
							if (carrier != null)
							{
								if (thing != null)
								{
									if (!carrier.Map.reachability.CanReach(intVec, thing, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true)))
									{
										goto IL_1E5;
									}
								}
								else if (!carrier.Map.reachability.CanReach(intVec, haulDestination2.Position, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true)))
								{
									goto IL_1E5;
								}
							}
							num = num2;
							storagePriority = priority;
							haulDestination = haulDestination2;
						}
					}
				}
				IL_1E5:;
			}
			return haulDestination != null;
		}

		// Token: 0x06005657 RID: 22103 RVA: 0x002020C8 File Offset: 0x002002C8
		public static bool IsGoodStoreCell(IntVec3 c, Map map, Thing t, Pawn carrier, Faction faction)
		{
			if (carrier != null && c.IsForbidden(carrier))
			{
				return false;
			}
			if (!StoreUtility.NoStorageBlockersIn(c, map, t))
			{
				return false;
			}
			if (carrier != null)
			{
				if (!carrier.CanReserveNew(c))
				{
					return false;
				}
			}
			else if (faction != null && map.reservationManager.IsReservedByAnyoneOf(c, faction))
			{
				return false;
			}
			if (c.ContainsStaticFire(map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is IConstructible && GenConstruct.BlocksConstruction(thingList[i], t))
				{
					return false;
				}
			}
			if (carrier != null)
			{
				Thing spawnedParentOrMe = t.SpawnedParentOrMe;
				IntVec3 start;
				if (spawnedParentOrMe != null)
				{
					if (spawnedParentOrMe != t && spawnedParentOrMe.def.hasInteractionCell)
					{
						start = spawnedParentOrMe.InteractionCell;
					}
					else
					{
						start = spawnedParentOrMe.Position;
					}
				}
				else
				{
					start = carrier.PositionHeld;
				}
				if (!carrier.Map.reachability.CanReach(start, c, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x002021C0 File Offset: 0x002003C0
		public static bool TryFindStoreCellNearColonyDesperate(Thing item, Pawn carrier, out IntVec3 storeCell)
		{
			if (StoreUtility.TryFindBestBetterStoreCellFor(item, carrier, carrier.Map, StoragePriority.Unstored, carrier.Faction, out storeCell, true))
			{
				return true;
			}
			for (int i = -4; i < 20; i++)
			{
				int num = (i < 0) ? Rand.RangeInclusive(0, 4) : i;
				IntVec3 intVec = carrier.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(carrier.Map) && carrier.Map.areaManager.Home[intVec] && carrier.CanReach(intVec, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn) && intVec.GetSlotGroup(carrier.Map) == null && StoreUtility.IsGoodStoreCell(intVec, carrier.Map, item, carrier, carrier.Faction))
				{
					storeCell = intVec;
					return true;
				}
			}
			if (RCellFinder.TryFindRandomSpotJustOutsideColony(carrier.Position, carrier.Map, carrier, out storeCell, (IntVec3 x) => x.GetSlotGroup(carrier.Map) == null && StoreUtility.IsGoodStoreCell(x, carrier.Map, item, carrier, carrier.Faction)))
			{
				return true;
			}
			storeCell = IntVec3.Invalid;
			return false;
		}
	}
}
