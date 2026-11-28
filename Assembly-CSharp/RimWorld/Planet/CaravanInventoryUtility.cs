using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200238A RID: 9098
	public static class CaravanInventoryUtility
	{
		// Token: 0x0600E241 RID: 57921 RVA: 0x0053FBC0 File Offset: 0x0053DDC0
		public static List<Thing> AllInventoryItems(Caravan caravan)
		{
			CaravanInventoryUtility.inventoryItems.Clear();
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				for (int j = pawn.inventory.innerContainer.Count - 1; j >= 0; j--)
				{
					Thing item = pawn.inventory.innerContainer[j];
					CaravanInventoryUtility.inventoryItems.Add(item);
				}
			}
			return CaravanInventoryUtility.inventoryItems;
		}

		// Token: 0x0600E242 RID: 57922 RVA: 0x0053FC38 File Offset: 0x0053DE38
		public static Building_PassengerShuttle FindShuttle(Caravan caravan)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Building_PassengerShuttle building_PassengerShuttle = list[i] as Building_PassengerShuttle;
				if (building_PassengerShuttle != null)
				{
					return building_PassengerShuttle;
				}
			}
			return null;
		}

		// Token: 0x0600E243 RID: 57923 RVA: 0x0053FC70 File Offset: 0x0053DE70
		public static void CaravanInventoryUtilityStaticUpdate()
		{
			CaravanInventoryUtility.inventoryItems.Clear();
		}

		// Token: 0x0600E244 RID: 57924 RVA: 0x0053FC7C File Offset: 0x0053DE7C
		public static Pawn GetOwnerOf(Caravan caravan, Thing item)
		{
			IThingHolder parentHolder = item.ParentHolder;
			if (parentHolder is Pawn_InventoryTracker)
			{
				Pawn pawn = (Pawn)parentHolder.ParentHolder;
				if (caravan.ContainsPawn(pawn))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x0600E245 RID: 57925 RVA: 0x0053FCB0 File Offset: 0x0053DEB0
		public static bool TryGetBestFood(Caravan caravan, Pawn forPawn, out Thing food, out Pawn owner)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Thing thing = null;
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (CaravanPawnsNeedsUtility.CanEatForNutritionNow(thing2, forPawn))
				{
					float foodScore = CaravanPawnsNeedsUtility.GetFoodScore(thing2, forPawn);
					if (thing == null || foodScore > num)
					{
						thing = thing2;
						num = foodScore;
					}
				}
			}
			if (thing != null)
			{
				food = thing;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
				return true;
			}
			food = null;
			owner = null;
			return false;
		}

		// Token: 0x0600E246 RID: 57926 RVA: 0x0053FD20 File Offset: 0x0053DF20
		public static bool TryGetDrugToSatisfyChemicalNeed(Caravan caravan, Pawn forPawn, Hediff hediff, out Thing drug, out Pawn owner)
		{
			if (hediff == null)
			{
				drug = null;
				owner = null;
				return false;
			}
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Thing thing = null;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2.IngestibleNow && thing2.def.IsDrug)
				{
					CompDrug compDrug = thing2.TryGetComp<CompDrug>();
					if (compDrug != null && compDrug.Props.chemical != null)
					{
						Hediff_ChemicalDependency hediff_ChemicalDependency = hediff as Hediff_ChemicalDependency;
						if (hediff_ChemicalDependency == null || compDrug.Props.chemical == hediff_ChemicalDependency.chemical)
						{
							Hediff_Addiction hediff_Addiction = hediff as Hediff_Addiction;
							if (hediff_Addiction == null || compDrug.Props.chemical.addictionHediff == hediff_Addiction.def)
							{
								Pawn_DrugPolicyTracker drugs = forPawn.drugs;
								DrugPolicy drugPolicy = (drugs != null) ? drugs.CurrentPolicy : null;
								if (drugPolicy == null || drugPolicy[thing2.def].allowedForAddiction || forPawn.story == null || forPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0)
								{
									thing = thing2;
									break;
								}
							}
						}
					}
				}
			}
			if (thing != null)
			{
				drug = thing;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
				return true;
			}
			drug = null;
			owner = null;
			return false;
		}

		// Token: 0x0600E247 RID: 57927 RVA: 0x0053FE50 File Offset: 0x0053E050
		public static bool TryGetBestMedicine(Caravan caravan, Pawn patient, out Medicine medicine, out Pawn owner)
		{
			if (patient.playerSettings == null || patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
			{
				medicine = null;
				owner = null;
				return false;
			}
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Medicine medicine2 = null;
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.IsMedicine && patient.playerSettings.medCare.AllowsMedicine(thing.def))
				{
					float statValue = thing.GetStatValue(StatDefOf.MedicalPotency, true, -1);
					if (statValue > num || medicine2 == null)
					{
						num = statValue;
						medicine2 = (Medicine)thing;
					}
				}
			}
			if (medicine2 != null)
			{
				medicine = medicine2;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, medicine2);
				return true;
			}
			medicine = null;
			owner = null;
			return false;
		}

		// Token: 0x0600E248 RID: 57928 RVA: 0x0053FF08 File Offset: 0x0053E108
		public static bool TryGetThingOfDef(Caravan caravan, ThingDef thingDef, out Thing thing, out Pawn owner)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2.def == thingDef)
				{
					thing = thing2;
					owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing2);
					return true;
				}
			}
			thing = null;
			owner = null;
			return false;
		}

		// Token: 0x0600E249 RID: 57929 RVA: 0x0053FF54 File Offset: 0x0053E154
		public static void MoveAllInventoryToSomeoneElse(Pawn from, List<Pawn> candidates, List<Pawn> ignoreCandidates = null)
		{
			CaravanInventoryUtility.inventoryToMove.Clear();
			CaravanInventoryUtility.inventoryToMove.AddRange(from.inventory.innerContainer);
			for (int i = 0; i < CaravanInventoryUtility.inventoryToMove.Count; i++)
			{
				CaravanInventoryUtility.MoveInventoryToSomeoneElse(from, CaravanInventoryUtility.inventoryToMove[i], candidates, ignoreCandidates, CaravanInventoryUtility.inventoryToMove[i].stackCount);
			}
			CaravanInventoryUtility.inventoryToMove.Clear();
		}

		// Token: 0x0600E24A RID: 57930 RVA: 0x0053FFC4 File Offset: 0x0053E1C4
		public static void MoveInventoryToSomeoneElse(Pawn itemOwner, Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, int numToMove)
		{
			if (numToMove < 0 || numToMove > item.stackCount)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to move item ",
					(item != null) ? item.ToString() : null,
					" with numToMove=",
					numToMove.ToString(),
					" (item stack count = ",
					item.stackCount.ToString(),
					")"
				}));
				return;
			}
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, candidates, ignoreCandidates, itemOwner);
			if (pawn == null)
			{
				return;
			}
			itemOwner.inventory.innerContainer.TryTransferToContainer(item, pawn.inventory.innerContainer, numToMove, true);
		}

		// Token: 0x0600E24B RID: 57931 RVA: 0x00540068 File Offset: 0x0053E268
		public static Pawn FindPawnToMoveInventoryTo(Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, Pawn currentItemOwner = null)
		{
			if (item is Pawn)
			{
				Log.Error("Called FindPawnToMoveInventoryTo but the item is a pawn.");
				return null;
			}
			Pawn result;
			if ((from x in candidates
			where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner && !MassUtility.IsOverEncumbered(x)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			if ((from x in candidates
			where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
			select x).TryRandomElement(out result))
			{
				return result;
			}
			if ((from x in candidates
			where (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600E24C RID: 57932 RVA: 0x005400F8 File Offset: 0x0053E2F8
		public static void MoveAllApparelToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates, bool moveLocked = true)
		{
			if (moveFrom.apparel == null)
			{
				return;
			}
			CaravanInventoryUtility.tmpApparel.Clear();
			if (moveLocked)
			{
				CaravanInventoryUtility.tmpApparel.AddRange(moveFrom.apparel.WornApparel);
			}
			else
			{
				for (int i = 0; i < moveFrom.apparel.WornApparel.Count; i++)
				{
					Apparel apparel = moveFrom.apparel.WornApparel[i];
					if (!moveFrom.apparel.IsLocked(apparel))
					{
						CaravanInventoryUtility.tmpApparel.Add(apparel);
					}
				}
			}
			for (int j = 0; j < CaravanInventoryUtility.tmpApparel.Count; j++)
			{
				moveFrom.apparel.Remove(CaravanInventoryUtility.tmpApparel[j]);
				Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(CaravanInventoryUtility.tmpApparel[j], candidates, null, moveFrom);
				if (pawn != null)
				{
					pawn.inventory.innerContainer.TryAdd(CaravanInventoryUtility.tmpApparel[j], true);
				}
			}
			CaravanInventoryUtility.tmpApparel.Clear();
		}

		// Token: 0x0600E24D RID: 57933 RVA: 0x005401E4 File Offset: 0x0053E3E4
		public static void MoveAllEquipmentToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates)
		{
			if (moveFrom.equipment == null)
			{
				return;
			}
			CaravanInventoryUtility.tmpEquipment.Clear();
			CaravanInventoryUtility.tmpEquipment.AddRange(moveFrom.equipment.AllEquipmentListForReading);
			for (int i = 0; i < CaravanInventoryUtility.tmpEquipment.Count; i++)
			{
				moveFrom.equipment.Remove(CaravanInventoryUtility.tmpEquipment[i]);
				Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(CaravanInventoryUtility.tmpEquipment[i], candidates, null, moveFrom);
				if (pawn != null)
				{
					pawn.inventory.innerContainer.TryAdd(CaravanInventoryUtility.tmpEquipment[i], true);
				}
			}
			CaravanInventoryUtility.tmpEquipment.Clear();
		}

		// Token: 0x0600E24E RID: 57934 RVA: 0x00540282 File Offset: 0x0053E482
		private static bool CanMoveInventoryTo(Pawn pawn)
		{
			return MassUtility.CanEverCarryAnything(pawn);
		}

		// Token: 0x0600E24F RID: 57935 RVA: 0x0054028C File Offset: 0x0053E48C
		public static List<Thing> TakeThings(Caravan caravan, Func<Thing, int> takeQuantity)
		{
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan).ToList<Thing>())
			{
				int num = takeQuantity(thing);
				if (num > 0)
				{
					list.Add(thing.holdingOwner.Take(thing, num));
				}
			}
			return list;
		}

		// Token: 0x0600E250 RID: 57936 RVA: 0x00540304 File Offset: 0x0053E504
		public static void GiveThing(Caravan caravan, Thing thing)
		{
			if (CaravanInventoryUtility.AllInventoryItems(caravan).Contains(thing))
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to give the same item twice (",
					(thing != null) ? thing.ToString() : null,
					") to a caravan (",
					(caravan != null) ? caravan.ToString() : null,
					")."
				}));
				return;
			}
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn == null)
			{
				Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan));
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan));
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
		}

		// Token: 0x0600E251 RID: 57937 RVA: 0x005403C0 File Offset: 0x0053E5C0
		public static bool HasThings(Caravan caravan, ThingDef thingDef, int count, Func<Thing, bool> validator = null)
		{
			int num = 0;
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def == thingDef && (validator == null || validator(thing)))
				{
					num += thing.stackCount;
				}
			}
			return num >= count;
		}

		// Token: 0x0600E252 RID: 57938 RVA: 0x00540414 File Offset: 0x0053E614
		public static IEnumerable<Thing> GetAllDissolvingThings(Caravan caravan)
		{
			CaravanInventoryUtility.<GetAllDissolvingThings>d__21 <GetAllDissolvingThings>d__ = new CaravanInventoryUtility.<GetAllDissolvingThings>d__21(-2);
			<GetAllDissolvingThings>d__.<>3__caravan = caravan;
			return <GetAllDissolvingThings>d__;
		}

		// Token: 0x0400907E RID: 36990
		private static List<Thing> inventoryItems = new List<Thing>();

		// Token: 0x0400907F RID: 36991
		private static List<Thing> inventoryToMove = new List<Thing>();

		// Token: 0x04009080 RID: 36992
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x04009081 RID: 36993
		private static List<ThingWithComps> tmpEquipment = new List<ThingWithComps>();
	}
}
