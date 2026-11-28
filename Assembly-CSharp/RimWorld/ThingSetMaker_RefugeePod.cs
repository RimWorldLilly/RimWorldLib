using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6E RID: 7022
	public class ThingSetMaker_RefugeePod : ThingSetMaker
	{
		// Token: 0x0600B441 RID: 46145 RVA: 0x003F8970 File Offset: 0x003F6B70
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(0.6f), PawnGenerationContext.NonPlayer, parms.tile, false, false, false, true, false, 20f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			int num = 0;
			Pawn pawn = null;
			while (num < 10 && (pawn == null || !pawn.Downed))
			{
				num++;
				if (pawn != null)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				pawn = PawnGenerator.GeneratePawn(request);
				HealthUtility.DamageUntilDowned(pawn, true, null, null, null);
			}
			outThings.Add(pawn);
		}

		// Token: 0x0600B442 RID: 46146 RVA: 0x003F8A55 File Offset: 0x003F6C55
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return new ThingSetMaker_RefugeePod.<AllGeneratableThingsDebugSub>d__2(-2);
		}

		// Token: 0x040067D5 RID: 26581
		private const float RelationWithColonistWeight = 20f;
	}
}
