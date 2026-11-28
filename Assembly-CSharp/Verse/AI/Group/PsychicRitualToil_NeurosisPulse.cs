using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x0200092B RID: 2347
	public class PsychicRitualToil_NeurosisPulse : PsychicRitualToil
	{
		// Token: 0x060047F1 RID: 18417 RVA: 0x001C3597 File Offset: 0x001C1797
		protected PsychicRitualToil_NeurosisPulse()
		{
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x001C359F File Offset: 0x001C179F
		public PsychicRitualToil_NeurosisPulse(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x001C35B0 File Offset: 0x001C17B0
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float duration = ((PsychicRitualDef_NeurosisPulse)psychicRitual.def).durationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, duration);
			}
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x001C3608 File Offset: 0x001C1808
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float duration)
		{
			foreach (Pawn pawn in invoker.Map.mapPawns.AllHumanlikeSpawned)
			{
				if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > 0f)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PleasurePulse, false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.NeurosisPulse, pawn, null);
					HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
					if (hediffComp_Disappears != null)
					{
						hediffComp_Disappears.ticksToDisappear = Mathf.RoundToInt(duration * 60000f);
					}
					pawn.health.AddHediff(hediff, null, null, null);
				}
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "NeurosisPulseCompleteText".Translate(invoker, Mathf.FloorToInt(duration * 60000f).ToStringTicksToDays("F1"), psychicRitual.def.Named("RITUAL")), LetterDefOf.NeutralEvent, null, 0, true);
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x001C374C File Offset: 0x001C194C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x04002E81 RID: 11905
		private PsychicRitualRoleDef invokerRole;
	}
}
