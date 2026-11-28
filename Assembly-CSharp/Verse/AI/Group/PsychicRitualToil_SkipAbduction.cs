using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Sound;

namespace Verse.AI.Group
{
	// Token: 0x0200092F RID: 2351
	public class PsychicRitualToil_SkipAbduction : PsychicRitualToil
	{
		// Token: 0x06004805 RID: 18437 RVA: 0x001C42E7 File Offset: 0x001C24E7
		protected PsychicRitualToil_SkipAbduction()
		{
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x001C42EF File Offset: 0x001C24EF
		public PsychicRitualToil_SkipAbduction(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x001C4300 File Offset: 0x001C2500
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn);
			}
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x001C4334 File Offset: 0x001C2534
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker)
		{
			List<Pawn> freeColonistsSpawned = psychicRitual.Map.mapPawns.FreeColonistsSpawned;
			freeColonistsSpawned.AddRange(psychicRitual.Map.mapPawns.SlavesOfColonySpawned);
			freeColonistsSpawned.RemoveAll((Pawn p) => p.health.hediffSet.HasHediff(HediffDefOf.DarkPsychicShock, false));
			Pawn pawn;
			if (!freeColonistsSpawned.TryRandomElement(out pawn))
			{
				return;
			}
			psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(pawn, pawn.Map, 1f), pawn.PositionHeld, 60);
			SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(pawn.PositionHeld, pawn.Map, false));
			IntVec3 cell = psychicRitual.assignments.Target.Cell;
			if (!cell.Standable(psychicRitual.Map))
			{
				CellFinder.TryFindRandomSpawnCellForPawnNear(invoker.PositionHeld, psychicRitual.Map, out cell, 4, null);
			}
			SkipUtility.SkipTo(pawn, cell, psychicRitual.Map);
			psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(cell, psychicRitual.Map, 1f), cell, 60);
			SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(cell, psychicRitual.Map, false));
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, pawn, null);
			hediff.TryGetComp<HediffComp_Disappears>().disappearsAfterTicks = 30000;
			pawn.health.AddHediff(hediff, null, null, null);
			foreach (Pawn otherPawn in psychicRitual.assignments.AllAssignedPawns)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						ThoughtHandler thoughts = mood.thoughts;
						if (thoughts != null)
						{
							MemoryThoughtHandler memories = thoughts.memories;
							if (memories != null)
							{
								memories.TryGainMemory(ThoughtDefOf.UsedMeForPsychicRitual, otherPawn, null);
							}
						}
					}
				}
			}
			if (!psychicRitual.def.letterAICompleteLabel.NullOrEmpty() && !psychicRitual.def.letterAICompleteText.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(psychicRitual.def.letterAICompleteLabel, psychicRitual.def.letterAICompleteText.Formatted(pawn.Named("PAWN")), LetterDefOf.ThreatBig, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x001C4590 File Offset: 0x001C2790
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (Pawn pawn in psychicRitual.assignments.AllAssignedPawns)
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
			}
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x001C4614 File Offset: 0x001C2814
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x04002E8B RID: 11915
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x04002E8C RID: 11916
		private const int ComaDurationTicks = 30000;
	}
}
