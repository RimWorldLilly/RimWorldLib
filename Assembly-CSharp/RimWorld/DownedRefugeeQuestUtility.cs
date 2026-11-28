using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103F RID: 4159
	public static class DownedRefugeeQuestUtility
	{
		// Token: 0x0600689F RID: 26783 RVA: 0x0024F794 File Offset: 0x0024D994
		public static Pawn GenerateRefugee(PlanetTile tile, PawnKindDef pawnKind = null, float chanceForFaction = 0.6f)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind ?? PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(chanceForFaction), PawnGenerationContext.NonPlayer, new PlanetTile?(tile), false, false, false, true, false, 20f, true, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, new float?(0.2f), null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			HealthUtility.DamageUntilDowned(pawn, false, null, null, null);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			return pawn;
		}

		// Token: 0x060068A0 RID: 26784 RVA: 0x0024F84C File Offset: 0x0024DA4C
		public static Faction GetRandomFactionForRefugee(float chanceForFaction = 0.6f)
		{
			Faction result;
			if (Rand.Chance(chanceForFaction) && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, true, false, TechLevel.Undefined, TechLevel.Undefined, false, false))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0400429D RID: 17053
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x0400429E RID: 17054
		private const float ChanceToRedressWorldPawn = 0.2f;
	}
}
