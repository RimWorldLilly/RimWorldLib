using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.Utility
{
	// Token: 0x020020B1 RID: 8369
	public static class ReloadableUtility
	{
		// Token: 0x0600D2EB RID: 53995 RVA: 0x004D8E18 File Offset: 0x004D7018
		public static IReloadableComp FindSomeReloadableComponent(Pawn pawn, bool allowForcedReload)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (((equipment != null) ? equipment.PrimaryEq : null) != null)
			{
				IReloadableComp reloadableComp = pawn.equipment.PrimaryEq as IReloadableComp;
				if (reloadableComp != null && reloadableComp.NeedsReload(allowForcedReload))
				{
					return reloadableComp;
				}
			}
			if (pawn.apparel != null)
			{
				foreach (Apparel thing in pawn.apparel.WornApparel)
				{
					CompApparelReloadable compApparelReloadable = thing.TryGetComp<CompApparelReloadable>();
					if (compApparelReloadable != null && compApparelReloadable.NeedsReload(allowForcedReload))
					{
						return compApparelReloadable;
					}
				}
			}
			return null;
		}

		// Token: 0x0600D2EC RID: 53996 RVA: 0x004D8EBC File Offset: 0x004D70BC
		public static List<Thing> FindEnoughAmmo(Pawn pawn, IntVec3 rootCell, IReloadableComp reloadable, bool forceReload)
		{
			if (reloadable == null)
			{
				return null;
			}
			IntRange desiredQuantity = new IntRange(reloadable.MinAmmoNeeded(forceReload), reloadable.MaxAmmoNeeded(forceReload));
			return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, rootCell, desiredQuantity, (Thing t) => t.def == reloadable.AmmoDef);
		}

		// Token: 0x0600D2ED RID: 53997 RVA: 0x004D8F14 File Offset: 0x004D7114
		public static Pawn OwnerOf(IReloadableComp reloadable)
		{
			IThingHolder parentHolder = reloadable.ReloadableThing.ParentHolder;
			Pawn_ApparelTracker pawn_ApparelTracker = parentHolder as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				return pawn_ApparelTracker.pawn;
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = parentHolder as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker == null)
			{
				return null;
			}
			return pawn_EquipmentTracker.pawn;
		}

		// Token: 0x0600D2EE RID: 53998 RVA: 0x004D8F54 File Offset: 0x004D7154
		public static int TotalChargesFromQueuedJobs(Pawn pawn, ThingWithComps gear)
		{
			ICompWithCharges compWithCharges = gear.TryGetComp<CompApparelVerbOwner_Charged>();
			ICompWithCharges compWithCharges2 = compWithCharges ?? gear.TryGetComp<CompEquippableAbilityReloadable>();
			int num = 0;
			if (compWithCharges2 != null && pawn != null)
			{
				foreach (Job job in pawn.jobs.AllJobs())
				{
					Verb verbToUse = job.verbToUse;
					if (verbToUse != null && (compWithCharges2 == verbToUse.ReloadableCompSource || compWithCharges2 == verbToUse.DirectOwner))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600D2EF RID: 53999 RVA: 0x004D8FE0 File Offset: 0x004D71E0
		public static bool CanUseConsideringQueuedJobs(Pawn pawn, ThingWithComps gear, bool showMessage = true)
		{
			ICompWithCharges compWithCharges = gear.TryGetComp<CompApparelVerbOwner_Charged>();
			ICompWithCharges compWithCharges2 = compWithCharges ?? gear.TryGetComp<CompEquippableAbilityReloadable>();
			if (compWithCharges2 == null)
			{
				return true;
			}
			string text = null;
			if (!Event.current.shift)
			{
				string text2;
				if (!compWithCharges2.CanBeUsed(out text2))
				{
					text = text2;
				}
			}
			else if (ReloadableUtility.TotalChargesFromQueuedJobs(pawn, gear) + 1 > compWithCharges2.RemainingCharges)
			{
				IReloadableComp reloadableComp = compWithCharges2 as IReloadableComp;
				if (reloadableComp != null)
				{
					text = reloadableComp.DisabledReason(reloadableComp.MaxAmmoAmount(), reloadableComp.MaxAmmoAmount());
				}
				else
				{
					text = "CommandNoUsesLeft".Translate();
				}
			}
			if (text != null)
			{
				if (showMessage)
				{
					Messages.Message(text, pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
