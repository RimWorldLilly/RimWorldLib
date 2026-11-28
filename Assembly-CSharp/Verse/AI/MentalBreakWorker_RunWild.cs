using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200085C RID: 2140
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		// Token: 0x0600432D RID: 17197 RVA: 0x001A8120 File Offset: 0x001A6320
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled && !pawn.Downed && pawn.Spawned && !pawn.IsQuestLodger() && (pawn.guest == null || pawn.guest.Recruitable) && base.BreakCanOccur(pawn) && !pawn.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && (!ModsConfig.BiotechActive || !pawn.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.NoxiousHaze)) && pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def, 7f);
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x001A81C4 File Offset: 0x001A63C4
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			base.TrySendLetter(pawn, "LetterRunWildMentalBreak", reason);
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "RanWild", pawn.Named("SUBJECT"));
			pawn.ChangeKind(PawnKindDefOf.WildMan);
			if (pawn.Faction != null)
			{
				pawn.SetFaction(null, null);
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Catharsis, null, null);
			if (pawn.Spawned && !pawn.Downed)
			{
				pawn.jobs.StopAll(false, true);
			}
			return true;
		}
	}
}
