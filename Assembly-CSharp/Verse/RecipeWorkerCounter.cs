using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000153 RID: 339
	public class RecipeWorkerCounter
	{
		// Token: 0x06000A61 RID: 2657 RVA: 0x0003988A File Offset: 0x00037A8A
		public virtual bool CanCountProducts(Bill_Production bill)
		{
			return this.recipe.specialProducts == null && this.recipe.products != null && this.recipe.products.Count == 1;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000398BC File Offset: 0x00037ABC
		public virtual int CountProducts(Bill_Production bill)
		{
			ThingDefCountClass thingDefCountClass = this.recipe.products[0];
			ThingDef thingDef = thingDefCountClass.thingDef;
			if (thingDefCountClass.thingDef.CountAsResource && !bill.includeEquipped && (bill.includeTainted || !thingDefCountClass.thingDef.IsApparel || !thingDefCountClass.thingDef.apparel.careIfWornByCorpse) && bill.GetIncludeSlotGroup() == null && bill.hpRange.min == 0f && bill.hpRange.max == 1f && bill.qualityRange.min == QualityCategory.Awful && bill.qualityRange.max == QualityCategory.Legendary && !bill.limitToAllowedStuff)
			{
				return bill.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) + this.GetCarriedCount(bill, thingDef);
			}
			int num = 0;
			ISlotGroup includeSlotGroup = bill.GetIncludeSlotGroup();
			if (includeSlotGroup == null)
			{
				num = this.CountValidThings(bill.Map.listerThings.ThingsOfDef(thingDefCountClass.thingDef), bill, thingDef);
				if (thingDefCountClass.thingDef.Minifiable)
				{
					List<Thing> list = bill.Map.listerThings.ThingsInGroup(ThingRequestGroup.MinifiedThing);
					for (int i = 0; i < list.Count; i++)
					{
						MinifiedThing minifiedThing = (MinifiedThing)list[i];
						if (this.CountValidThing(minifiedThing.InnerThing, bill, thingDef))
						{
							num += minifiedThing.stackCount * minifiedThing.InnerThing.stackCount;
						}
					}
				}
				num += this.GetCarriedCount(bill, thingDef);
				using (List<IHaulSource>.Enumerator enumerator = bill.Map.haulDestinationManager.AllHaulSourcesListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IHaulSource haulSource = enumerator.Current;
						num += this.CountValidThings(haulSource.GetDirectlyHeldThings(), bill, thingDef);
					}
					goto IL_209;
				}
			}
			foreach (Thing outerThing in includeSlotGroup.HeldThings)
			{
				Thing innerIfMinified = outerThing.GetInnerIfMinified();
				if (this.CountValidThing(innerIfMinified, bill, thingDef))
				{
					num += innerIfMinified.stackCount;
				}
			}
			IL_209:
			if (bill.includeEquipped)
			{
				foreach (Pawn pawn in bill.Map.mapPawns.FreeColonistsSpawned)
				{
					List<ThingWithComps> allEquipmentListForReading = pawn.equipment.AllEquipmentListForReading;
					for (int j = 0; j < allEquipmentListForReading.Count; j++)
					{
						if (this.CountValidThing(allEquipmentListForReading[j], bill, thingDef))
						{
							num += allEquipmentListForReading[j].stackCount;
						}
					}
					List<Apparel> wornApparel = pawn.apparel.WornApparel;
					for (int k = 0; k < wornApparel.Count; k++)
					{
						if (this.CountValidThing(wornApparel[k], bill, thingDef))
						{
							num += wornApparel[k].stackCount;
						}
					}
					ThingOwner directlyHeldThings = pawn.inventory.GetDirectlyHeldThings();
					for (int l = 0; l < directlyHeldThings.Count; l++)
					{
						if (this.CountValidThing(directlyHeldThings[l], bill, thingDef))
						{
							num += directlyHeldThings[l].stackCount;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00039C20 File Offset: 0x00037E20
		private int CountValidThings(ThingOwner thingOwner, Bill_Production bill, ThingDef def)
		{
			int num = 0;
			for (int i = 0; i < thingOwner.Count; i++)
			{
				if (this.CountValidThing(thingOwner[i], bill, def))
				{
					num += thingOwner[i].stackCount;
				}
			}
			return num;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00039C64 File Offset: 0x00037E64
		public int CountValidThings(List<Thing> things, Bill_Production bill, ThingDef def)
		{
			int num = 0;
			for (int i = 0; i < things.Count; i++)
			{
				if (this.CountValidThing(things[i], bill, def))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00039C9C File Offset: 0x00037E9C
		public bool CountValidThing(Thing thing, Bill_Production bill, ThingDef def)
		{
			ThingDef def2 = thing.def;
			if (def2 != def)
			{
				return false;
			}
			if (!bill.includeTainted && def2.IsApparel && ((Apparel)thing).WornByCorpse)
			{
				return false;
			}
			if (thing.def.useHitPoints && !bill.hpRange.IncludesEpsilon((float)thing.HitPoints / (float)thing.MaxHitPoints))
			{
				return false;
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			return (compQuality == null || bill.qualityRange.Includes(compQuality.Quality)) && (!bill.limitToAllowedStuff || bill.ingredientFilter.Allows(thing.Stuff)) && (!thing.SpawnedOrAnyParentSpawned || !thing.PositionHeld.Fogged(thing.MapHeld));
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00039D5A File Offset: 0x00037F5A
		public virtual string ProductsDescription(Bill_Production bill)
		{
			return null;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00039D5D File Offset: 0x00037F5D
		public virtual bool CanPossiblyStore(Bill_Production bill, ISlotGroup slotGroup)
		{
			return !this.CanCountProducts(bill) || slotGroup.Settings.AllowedToAccept(this.recipe.products[0].thingDef);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00039D8C File Offset: 0x00037F8C
		private int GetCarriedCount(Bill_Production bill, ThingDef prodDef)
		{
			int num = 0;
			foreach (Pawn pawn in bill.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				Pawn_CarryTracker carryTracker = pawn.carryTracker;
				Thing thing = (carryTracker != null) ? carryTracker.CarriedThing : null;
				if (thing != null)
				{
					int stackCount = thing.stackCount;
					thing = thing.GetInnerIfMinified();
					if (this.CountValidThing(thing, bill, prodDef))
					{
						num += stackCount;
					}
				}
			}
			return num;
		}

		// Token: 0x04000718 RID: 1816
		public RecipeDef recipe;
	}
}
