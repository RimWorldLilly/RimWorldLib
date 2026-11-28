using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B37 RID: 2871
	public class JobGiver_DropUnusedInventory : ThinkNode_JobGiver
	{
		// Token: 0x0600545D RID: 21597 RVA: 0x001F5B54 File Offset: 0x001F3D54
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.inventory == null)
			{
				return null;
			}
			if (!pawn.Map.areaManager.Home[pawn.Position])
			{
				return null;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				return null;
			}
			if (Find.TickManager.TicksGame > pawn.mindState.lastInventoryRawFoodUseTick + 150000)
			{
				for (int i = pawn.inventory.innerContainer.Count - 1; i >= 0; i--)
				{
					Thing thing = pawn.inventory.innerContainer[i];
					if (thing.def.IsIngestible && !thing.def.IsDrug && thing.def.ingestible.preferability <= FoodPreferability.RawTasty)
					{
						this.Drop(pawn, thing);
					}
				}
			}
			for (int j = pawn.inventory.innerContainer.Count - 1; j >= 0; j--)
			{
				Thing thing2 = pawn.inventory.innerContainer[j];
				if (!JobGiver_DropUnusedInventory.ShouldKeepDrugInInventory(pawn, thing2))
				{
					this.Drop(pawn, thing2);
				}
			}
			return null;
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x001F5C5C File Offset: 0x001F3E5C
		public static bool ShouldKeepDrugInInventory(Pawn pawn, Thing drug)
		{
			return !drug.def.IsDrug || pawn.drugs == null || pawn.drugs.AllowedToTakeScheduledEver(drug.def) || (pawn.drugs.CurrentPolicy != null && pawn.drugs.CurrentPolicy[drug.def].takeToInventory != 0) || AddictionUtility.IsAddicted(pawn, drug) || AddictionUtility.HasChemicalDependency(pawn, drug);
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x001F5CD0 File Offset: 0x001F3ED0
		private void Drop(Pawn pawn, Thing thing)
		{
			Thing thing2;
			pawn.inventory.innerContainer.TryDrop(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near, out thing2, null, null);
		}

		// Token: 0x0400326F RID: 12911
		private const int RawFoodDropDelay = 150000;
	}
}
