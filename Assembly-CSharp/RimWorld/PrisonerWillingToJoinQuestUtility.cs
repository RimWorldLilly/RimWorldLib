using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001041 RID: 4161
	public static class PrisonerWillingToJoinQuestUtility
	{
		// Token: 0x060068A7 RID: 26791 RVA: 0x0024FB54 File Offset: 0x0024DD54
		public static Pawn GeneratePrisoner(PlanetTile tile, Faction hostFaction)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Slave, hostFaction, PawnGenerationContext.NonPlayer, new PlanetTile?(tile), false, false, false, true, false, 75f, true, true, false, true, true, false, false, true, true, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				request.AllowedDevelopmentalStages |= DevelopmentalStage.Child;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.guest.SetGuestStatus(hostFaction, GuestStatus.Prisoner);
			return pawn;
		}

		// Token: 0x0400429F RID: 17055
		private const float RelationWithColonistWeight = 75f;
	}
}
