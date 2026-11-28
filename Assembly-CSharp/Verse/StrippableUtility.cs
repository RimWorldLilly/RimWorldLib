using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004DB RID: 1243
	public static class StrippableUtility
	{
		// Token: 0x06002456 RID: 9302 RVA: 0x000DB818 File Offset: 0x000D9A18
		public static bool CanBeStrippedByColony(Thing th)
		{
			IStrippable strippable = th as IStrippable;
			if (strippable == null)
			{
				return false;
			}
			if (!strippable.AnythingToStrip())
			{
				return false;
			}
			Pawn pawn = th as Pawn;
			if (pawn == null)
			{
				return true;
			}
			if (!pawn.kindDef.canStrip)
			{
				return false;
			}
			if (pawn.CarriedBy != null)
			{
				return false;
			}
			if (pawn.Downed)
			{
				return true;
			}
			if (pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure)
			{
				return true;
			}
			pawn.IsQuestLodger();
			return false;
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000DB88C File Offset: 0x000D9A8C
		public static void CheckSendStrippingImpactsGoodwillMessage(Thing th)
		{
			Pawn pawn = th as Pawn;
			if (pawn != null && !pawn.Dead && pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.Faction.Hidden)
			{
				Messages.Message("MessageStrippingWillAngerFaction".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
