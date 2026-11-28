using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5E RID: 7262
	public static class SlaughterDesignatorUtility
	{
		// Token: 0x0600B94D RID: 47437 RVA: 0x00415830 File Offset: 0x00413A30
		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (!designated.RaceProps.IsFlesh)
			{
				return;
			}
			Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => !x.Dead);
			if (firstDirectRelationPawn != null)
			{
				Messages.Message("MessageSlaughteringBondedAnimal".Translate(designated.LabelShort, firstDirectRelationPawn.LabelShort, designated.Named("DESIGNATED"), firstDirectRelationPawn.Named("BONDED")), designated, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0600B94E RID: 47438 RVA: 0x004158CC File Offset: 0x00413ACC
		public static void CheckWarnAboutVeneratedAnimal(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (pawn.MapHeld.mapPawns.FreeColonistsSpawned.Count == 0)
			{
				return;
			}
			bool flag = true;
			bool flag2 = false;
			foreach (Pawn pawn2 in pawn.MapHeld.mapPawns.FreeColonistsSpawned)
			{
				if (!pawn2.WorkTypeIsDisabled(WorkTypeDefOf.Hunting))
				{
					flag2 = true;
					if (pawn2.Ideo == null || !pawn2.Ideo.IsVeneratedAnimal(pawn))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag2 && flag)
			{
				Messages.Message("MessageAnimalIsVeneratedForAllColonists".Translate(pawn.GetKindLabelPlural(-1).CapitalizeFirst().Named("PAWNKINDLABELPLURAL"), Faction.OfPlayer.def.pawnsPlural.Named("PAWNS")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
