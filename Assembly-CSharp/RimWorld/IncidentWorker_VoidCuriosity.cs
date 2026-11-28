using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A4 RID: 4516
	public class IncidentWorker_VoidCuriosity : IncidentWorker
	{
		// Token: 0x060070E9 RID: 28905 RVA: 0x002781DC File Offset: 0x002763DC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (Find.Anomaly.hasPerformedVoidProvocation)
			{
				return false;
			}
			return PawnsFinder.HomeMaps_FreeColonistsSpawned.Any((Pawn x) => !x.DevelopmentalStage.Baby());
		}

		// Token: 0x060070EA RID: 28906 RVA: 0x0027822C File Offset: 0x0027642C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Pawn pawn;
			if (!PawnsFinder.HomeMaps_FreeColonistsSpawned.TryRandomElement((Pawn x) => !x.DevelopmentalStage.Baby(), out pawn))
			{
				return false;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.VoidCuriosity, null, null);
			TaggedString baseLetterText = ResearchProjectDefOf.BasicPsychicRituals.IsFinished ? "VoidCuriosityText_Researched".Translate(pawn.Named("PAWN")) : "VoidCuriosityText_NotResearched".Translate(pawn.Named("PAWN"));
			base.SendStandardLetter("VoidCuriosityLabel".Translate(), baseLetterText, LetterDefOf.NeutralEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
