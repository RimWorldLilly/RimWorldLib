using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000931 RID: 2353
	public class PsychicRitualToil_SummonAnimals : PsychicRitualToil
	{
		// Token: 0x06004810 RID: 18448 RVA: 0x001C4AC3 File Offset: 0x001C2CC3
		protected PsychicRitualToil_SummonAnimals()
		{
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x001C4ACB File Offset: 0x001C2CCB
		public PsychicRitualToil_SummonAnimals(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x001C4ADC File Offset: 0x001C2CDC
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float manhunterChance = ((PsychicRitualDef_SummonAnimals)psychicRitual.def).manhunterSpawnChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, manhunterChance);
			}
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x001C4B34 File Offset: 0x001C2D34
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float manhunterChance)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = invoker.Map;
			incidentParms.forced = true;
			LetterDef textLetterDef;
			TaggedString text;
			if (Rand.Chance(manhunterChance))
			{
				textLetterDef = LetterDefOf.ThreatBig;
				text = "SummonAnimalsFailureText".Translate(invoker, psychicRitual.def.Named("RITUAL"));
				incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(invoker.Map);
				Find.Storyteller.incidentQueue.Add(IncidentDefOf.FrenziedAnimals, Find.TickManager.TicksGame + PsychicRitualToil_SummonAnimals.AnimalsDelayTicks.RandomInRange, incidentParms, 0);
			}
			else
			{
				textLetterDef = LetterDefOf.NeutralEvent;
				text = "SummonAnimalsSuccessText".Translate(invoker, psychicRitual.def.Named("RITUAL"));
				Find.Storyteller.incidentQueue.Add(IncidentDefOf.HerdMigration, Find.TickManager.TicksGame + PsychicRitualToil_SummonAnimals.AnimalsDelayTicks.RandomInRange, incidentParms, 0);
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), text, textLetterDef, null, 0, true);
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x001C4C4A File Offset: 0x001C2E4A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x04002E90 RID: 11920
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x04002E91 RID: 11921
		private static readonly IntRange AnimalsDelayTicks = new IntRange(2500, 7500);
	}
}
