using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C58 RID: 7256
	public static class ReleaseAnimalToWildUtility
	{
		// Token: 0x0600B929 RID: 47401 RVA: 0x00414D70 File Offset: 0x00412F70
		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (!designated.RaceProps.IsFlesh)
			{
				return;
			}
			Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => !x.Dead);
			if (firstDirectRelationPawn != null)
			{
				Messages.Message("MessageReleaseBondedAnimal".Translate(designated.LabelShort, firstDirectRelationPawn.LabelShort, designated.Named("DESIGNATED"), firstDirectRelationPawn.Named("BONDED")), designated, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0600B92A RID: 47402 RVA: 0x00414E0C File Offset: 0x0041300C
		public static void DoReleaseAnimal(Pawn animal, Pawn releasedBy)
		{
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(animal, null, PawnDiedOrDownedThoughtsKind.ReleasedToWild);
			releasedBy.interactions.TryInteractWith(animal, InteractionDefOf.ReleaseToWild);
			animal.Map.designationManager.RemoveDesignation(animal.Map.designationManager.DesignationOn(animal, DesignationDefOf.ReleaseAnimalToWild));
			animal.SetFaction(null, null);
			Pawn_Ownership ownership = animal.ownership;
			if (ownership != null)
			{
				ownership.UnclaimAll();
			}
			Messages.Message("MessageAnimalReturnedWildReleased".Translate(animal.LabelShort, animal), releasedBy, MessageTypeDefOf.NeutralEvent, true);
		}
	}
}
