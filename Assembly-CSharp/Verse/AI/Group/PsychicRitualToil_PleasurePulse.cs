using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x0200092D RID: 2349
	public class PsychicRitualToil_PleasurePulse : PsychicRitualToil
	{
		// Token: 0x060047FB RID: 18427 RVA: 0x001C3CCF File Offset: 0x001C1ECF
		protected PsychicRitualToil_PleasurePulse()
		{
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x001C3CD7 File Offset: 0x001C1ED7
		public PsychicRitualToil_PleasurePulse(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x001C3CE8 File Offset: 0x001C1EE8
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float duration = ((PsychicRitualDef_PleasurePulse)psychicRitual.def).durationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, duration);
			}
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x001C3D40 File Offset: 0x001C1F40
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float duration)
		{
			foreach (Pawn pawn in invoker.Map.mapPawns.AllHumanlikeSpawned)
			{
				if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > 0f)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.NeurosisPulse, false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.PleasurePulse, pawn, null);
					HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
					if (hediffComp_Disappears != null)
					{
						hediffComp_Disappears.ticksToDisappear = Mathf.RoundToInt(duration * 60000f);
					}
					pawn.health.AddHediff(hediff, null, null, null);
				}
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "PleasurePulseCompleteText".Translate(invoker, Mathf.FloorToInt(duration * 60000f).ToStringTicksToDays("F1"), psychicRitual.def.Named("RITUAL")), LetterDefOf.NeutralEvent, null, 0, true);
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x001C3E84 File Offset: 0x001C2084
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x04002E86 RID: 11910
		private PsychicRitualRoleDef invokerRole;
	}
}
