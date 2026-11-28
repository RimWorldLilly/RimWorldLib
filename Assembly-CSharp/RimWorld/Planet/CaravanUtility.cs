using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002393 RID: 9107
	public static class CaravanUtility
	{
		// Token: 0x0600E283 RID: 57987 RVA: 0x00541D00 File Offset: 0x0053FF00
		public static bool IsOwner(Pawn pawn, Faction caravanFaction)
		{
			if (caravanFaction == null)
			{
				Log.Warning("Called IsOwner with null faction.");
				return false;
			}
			if (!pawn.NonHumanlikeOrWildMan() && pawn.Faction == caravanFaction && pawn.HostFaction == null && !pawn.IsSlave)
			{
				Pawn_MutantTracker mutant = pawn.mutant;
				return mutant == null || mutant.Def.canOwnCaravan;
			}
			return false;
		}

		// Token: 0x0600E284 RID: 57988 RVA: 0x00541D55 File Offset: 0x0053FF55
		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		// Token: 0x0600E285 RID: 57989 RVA: 0x00541D60 File Offset: 0x0053FF60
		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		// Token: 0x0600E286 RID: 57990 RVA: 0x00541D80 File Offset: 0x0053FF80
		public static PlanetTile BestGotoDestNear(PlanetTile tile, Caravan c)
		{
			CaravanUtility.<>c__DisplayClass3_0 CS$<>8__locals1 = new CaravanUtility.<>c__DisplayClass3_0();
			CS$<>8__locals1.c = c;
			if (CS$<>8__locals1.<BestGotoDestNear>g__IsGoodDest|0(tile))
			{
				return tile;
			}
			PlanetTile result;
			GenWorldClosest.TryFindClosestTile(tile, new Predicate<PlanetTile>(CS$<>8__locals1.<BestGotoDestNear>g__IsGoodDest|0), out result, 50, true);
			return result;
		}

		// Token: 0x0600E287 RID: 57991 RVA: 0x00541DC0 File Offset: 0x0053FFC0
		public static bool PlayerHasAnyCaravan()
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].IsPlayerControlled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E288 RID: 57992 RVA: 0x00541DFC File Offset: 0x0053FFFC
		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		// Token: 0x0600E289 RID: 57993 RVA: 0x00541E37 File Offset: 0x00540037
		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}

		// Token: 0x0600E28A RID: 57994 RVA: 0x00541E70 File Offset: 0x00540070
		public static PlanetTile GetTileCurrentlyOver(this Caravan caravan)
		{
			if (caravan.pather.Moving && caravan.pather.IsNextTilePassable() && 1f - caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal > 0.5f)
			{
				return caravan.pather.nextTile;
			}
			return caravan.Tile;
		}

		// Token: 0x0600E28B RID: 57995 RVA: 0x00541ED0 File Offset: 0x005400D0
		public static Caravan GetCaravan(this Thing thing)
		{
			Caravan caravan = thing.ParentHolder as Caravan;
			if (caravan != null)
			{
				return caravan;
			}
			Pawn_InventoryTracker pawn_InventoryTracker = thing.ParentHolder as Pawn_InventoryTracker;
			if (pawn_InventoryTracker != null)
			{
				return pawn_InventoryTracker.pawn.GetCaravan();
			}
			Thing thing2 = thing.ParentHolder as Thing;
			if (thing2 != null)
			{
				return thing2.GetCaravan();
			}
			return null;
		}

		// Token: 0x0600E28C RID: 57996 RVA: 0x00541F20 File Offset: 0x00540120
		public static bool IsInCaravan(this Thing thing)
		{
			return thing.GetCaravan() != null;
		}
	}
}
