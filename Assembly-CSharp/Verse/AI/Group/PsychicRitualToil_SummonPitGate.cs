using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000934 RID: 2356
	public class PsychicRitualToil_SummonPitGate : PsychicRitualToil
	{
		// Token: 0x06004826 RID: 18470 RVA: 0x001C53BC File Offset: 0x001C35BC
		protected PsychicRitualToil_SummonPitGate()
		{
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x001C53C4 File Offset: 0x001C35C4
		public PsychicRitualToil_SummonPitGate(PsychicRitualRoleDef invokerRole, FloatRange combatPointMultiplierFromQualityRange)
		{
			this.invokerRole = invokerRole;
			this.combatPointMultiplierFromQualityRange = combatPointMultiplierFromQualityRange;
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x001C53DC File Offset: 0x001C35DC
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float combatPointMultiplier = this.combatPointMultiplierFromQualityRange.LerpThroughRange(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, combatPointMultiplier);
			}
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x001C5428 File Offset: 0x001C3628
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float combatPointMultiplier)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = invoker.Map;
			incidentParms.pointMultiplier = combatPointMultiplier;
			incidentParms.forced = true;
			Find.Storyteller.incidentQueue.Add(IncidentDefOf.PitGate, Find.TickManager.TicksGame, incidentParms, 0);
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x001C5478 File Offset: 0x001C3678
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Values.Look<FloatRange>(ref this.combatPointMultiplierFromQualityRange, "combatPointMultiplierFromQualityRange", default(FloatRange), false);
		}

		// Token: 0x04002E9F RID: 11935
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x04002EA0 RID: 11936
		private FloatRange combatPointMultiplierFromQualityRange;
	}
}
