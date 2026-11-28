using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000937 RID: 2359
	public class PsychicRitualToil_VoidProvocation : PsychicRitualToil
	{
		// Token: 0x06004837 RID: 18487 RVA: 0x001C5875 File Offset: 0x001C3A75
		protected PsychicRitualToil_VoidProvocation()
		{
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x001C587D File Offset: 0x001C3A7D
		public PsychicRitualToil_VoidProvocation(PsychicRitualRoleDef invokerRole, SimpleCurve psychicShockChanceFromQualityCurve)
		{
			this.invokerRole = invokerRole;
			this.psychicShockChanceFromQualityCurve = psychicShockChanceFromQualityCurve;
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x001C5894 File Offset: 0x001C3A94
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn);
			}
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x001C58C0 File Offset: 0x001C3AC0
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker)
		{
			PsychicRitualDef_VoidProvocation psychicRitualDef_VoidProvocation = (PsychicRitualDef_VoidProvocation)psychicRitual.def;
			Map map = psychicRitual.Map;
			List<IncidentDef> list = new List<IncidentDef>();
			bool flag = false;
			foreach (EntityCategoryDef entityCategoryDef in from x in DefDatabase<EntityCategoryDef>.AllDefs
			orderby x.listOrder
			select x)
			{
				foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
				{
					if (entityCodexEntryDef.category == entityCategoryDef && !entityCodexEntryDef.provocationIncidents.NullOrEmpty<IncidentDef>() && !entityCodexEntryDef.Discovered)
					{
						foreach (IncidentDef incidentDef in entityCodexEntryDef.provocationIncidents)
						{
							IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incidentDef.category, map);
							incidentParms.bypassStorytellerSettings = true;
							if (incidentDef.Worker.CanFireNow(incidentParms))
							{
								list.Add(incidentDef);
								flag = true;
							}
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!list.Any<IncidentDef>())
			{
				foreach (EntityCodexEntryDef entityCodexEntryDef2 in DefDatabase<EntityCodexEntryDef>.AllDefs)
				{
					if (!entityCodexEntryDef2.provocationIncidents.NullOrEmpty<IncidentDef>())
					{
						foreach (IncidentDef incidentDef2 in entityCodexEntryDef2.provocationIncidents)
						{
							IncidentParms incidentParms2 = StorytellerUtility.DefaultParmsNow(incidentDef2.category, map);
							incidentParms2.bypassStorytellerSettings = true;
							if (incidentDef2.Worker.CanFireNow(incidentParms2))
							{
								list.Add(incidentDef2);
							}
						}
					}
				}
			}
			IncidentDef incidentDef3;
			bool flag2;
			if (list.TryRandomElement(out incidentDef3))
			{
				flag2 = true;
				IncidentParms incidentParms3 = StorytellerUtility.DefaultParmsNow(incidentDef3.category, map);
				incidentParms3.bypassStorytellerSettings = true;
				Find.Storyteller.incidentQueue.Add(incidentDef3, Find.TickManager.TicksGame + Mathf.RoundToInt(psychicRitualDef_VoidProvocation.incidentDelayHoursRange.RandomInRange * 2500f), incidentParms3, 0);
			}
			else
			{
				flag2 = false;
			}
			TaggedString taggedString = "VoidProvocationCompletedText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL")) + "\n\n" + (flag2 ? "VoidProvocationSucceeded" : "VoidProvocationFailed").Translate();
			if (flag2 && Rand.Chance(psychicRitualDef_VoidProvocation.psychicShockChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent)))
			{
				taggedString += "\n\n" + "VoidProvocationDarkPsychicShock".Translate(invoker.Named("INVOKER"));
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, invoker, null);
				int duration = Mathf.RoundToInt(psychicRitualDef_VoidProvocation.darkPsychicShockDurarionHoursRange.RandomInRange * 2500f);
				HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears != null)
				{
					hediffComp_Disappears.SetDuration(duration);
				}
				invoker.health.AddHediff(hediff, null, null, null);
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), taggedString, LetterDefOf.NeutralEvent, null, 0, true);
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (pawn.needs.mood.thoughts.memories.NumMemoriesOfDef(ThoughtDefOf.VoidCuriosity) > 0)
				{
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.VoidCuriosity);
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.VoidCuriositySatisfied, null, null);
				}
			}
			Find.Anomaly.hasPerformedVoidProvocation = true;
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x001C5D10 File Offset: 0x001C3F10
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Deep.Look<SimpleCurve>(ref this.psychicShockChanceFromQualityCurve, "psychicShockChanceFromQualityCurve", Array.Empty<object>());
		}

		// Token: 0x04002EA9 RID: 11945
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x04002EAA RID: 11946
		private SimpleCurve psychicShockChanceFromQualityCurve;
	}
}
