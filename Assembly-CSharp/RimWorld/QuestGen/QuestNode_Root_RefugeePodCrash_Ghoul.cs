using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200228E RID: 8846
	public class QuestNode_Root_RefugeePodCrash_Ghoul : QuestNode_Root_WandererJoin
	{
		// Token: 0x0600DA70 RID: 55920 RVA: 0x00512D39 File Offset: 0x00510F39
		public override Pawn GeneratePawn()
		{
			return this.GeneratePawn_NewTemp(null);
		}

		// Token: 0x0600DA71 RID: 55921 RVA: 0x00512D44 File Offset: 0x00510F44
		public override Pawn GeneratePawn_NewTemp(Map map)
		{
			Pawn pawn;
			if (Find.World.worldPawns.GetPawnsBySituationCount(WorldPawnSituation.StartingPawnLeftBehind) > 0)
			{
				pawn = Find.World.worldPawns.GetPawnsBySituation(WorldPawnSituation.StartingPawnLeftBehind).RandomElement<Pawn>();
				MutantUtility.SetFreshPawnAsMutant(pawn, MutantDefOf.Ghoul);
				pawn.SetFaction(Faction.OfEntities, null);
				this.wasStartingPawn = true;
			}
			else
			{
				pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Ghoul, Faction.OfEntities, (map != null) ? new PlanetTile?(map.Tile) : null);
			}
			pawn.health.AddHediff(HediffDefOf.DarkPsychicShock, null, null, null).TryGetComp<HediffComp_Disappears>().ticksToDisappear = QuestNode_Root_RefugeePodCrash_Ghoul.ShockTicksRange.RandomInRange;
			return pawn;
		}

		// Token: 0x0600DA72 RID: 55922 RVA: 0x00512DF8 File Offset: 0x00510FF8
		[Obsolete]
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			this.SendLetter_NewTemp(quest, pawn, Find.AnyPlayerHomeMap);
		}

		// Token: 0x0600DA73 RID: 55923 RVA: 0x00512E08 File Offset: 0x00511008
		public override void SendLetter_NewTemp(Quest quest, Pawn pawn, Map map)
		{
			TaggedString label = "GhoulPodCrashLabel".Translate();
			TaggedString text = this.wasStartingPawn ? "GhoulPodCrashStartingPawnText".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true) : "GhoulPodCrashText".Translate();
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatSmall, new TargetInfo(pawn), null, null, null, null, 0, true);
		}

		// Token: 0x04008B8E RID: 35726
		private static readonly IntRange ShockTicksRange = new IntRange(10000, 15000);

		// Token: 0x04008B8F RID: 35727
		private bool wasStartingPawn;
	}
}
