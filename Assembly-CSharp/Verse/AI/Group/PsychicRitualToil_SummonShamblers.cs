using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000935 RID: 2357
	public class PsychicRitualToil_SummonShamblers : PsychicRitualToil
	{
		// Token: 0x0600482B RID: 18475 RVA: 0x001C54B5 File Offset: 0x001C36B5
		protected PsychicRitualToil_SummonShamblers()
		{
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x001C54BD File Offset: 0x001C36BD
		public PsychicRitualToil_SummonShamblers(PsychicRitualRoleDef invokerRole, FloatRange combatPointsFromQualityRange)
		{
			this.invokerRole = invokerRole;
			this.combatPointsFromQualityRange = combatPointsFromQualityRange;
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x001C54D4 File Offset: 0x001C36D4
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float combatPoints = this.combatPointsFromQualityRange.LerpThroughRange(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, combatPoints);
			}
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x001C5520 File Offset: 0x001C3720
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float combatPoints)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = invoker.Map;
			incidentParms.points = combatPoints;
			incidentParms.forced = true;
			Find.Storyteller.incidentQueue.Add(IncidentDefOf.ShamblerSwarm, Find.TickManager.TicksGame + PsychicRitualToil_SummonShamblers.ShamblersDelayTicks.RandomInRange, incidentParms, 0);
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "SummonShamblersCompleteText".Translate(invoker, psychicRitual.def.Named("RITUAL")), LetterDefOf.ThreatBig, null, 0, true);
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x001C55C8 File Offset: 0x001C37C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Values.Look<FloatRange>(ref this.combatPointsFromQualityRange, "combatPointsFromQualityRange", default(FloatRange), false);
		}

		// Token: 0x04002EA1 RID: 11937
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x04002EA2 RID: 11938
		private FloatRange combatPointsFromQualityRange;

		// Token: 0x04002EA3 RID: 11939
		private static readonly IntRange ShamblersDelayTicks = new IntRange(2500, 7500);
	}
}
