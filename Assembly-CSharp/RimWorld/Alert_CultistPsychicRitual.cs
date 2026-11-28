using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BAA RID: 7082
	public class Alert_CultistPsychicRitual : Alert_Critical
	{
		// Token: 0x170020E1 RID: 8417
		// (get) Token: 0x0600B5C7 RID: 46535 RVA: 0x0040059C File Offset: 0x003FE79C
		private PsychicRitual CurPsychicRitual
		{
			get
			{
				LordToil_PsychicRitual lordToil_PsychicRitual = this.ritualLord.CurLordToil as LordToil_PsychicRitual;
				if (lordToil_PsychicRitual != null)
				{
					return lordToil_PsychicRitual.RitualData.psychicRitual;
				}
				return null;
			}
		}

		// Token: 0x170020E2 RID: 8418
		// (get) Token: 0x0600B5C8 RID: 46536 RVA: 0x004005CA File Offset: 0x003FE7CA
		protected override bool DoMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B5C9 RID: 46537 RVA: 0x004005CD File Offset: 0x003FE7CD
		public Alert_CultistPsychicRitual()
		{
			this.defaultLabel = "Alert_CultistPsychicRitual".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0600B5CA RID: 46538 RVA: 0x004005F1 File Offset: 0x003FE7F1
		public override string GetLabel()
		{
			return this.defaultLabel + ": " + this.invokeToil.TicksLeft.ToStringTicksToPeriodVerbose(true, true).CapitalizeFirst();
		}

		// Token: 0x0600B5CB RID: 46539 RVA: 0x0040061C File Offset: 0x003FE81C
		public override TaggedString GetExplanation()
		{
			Pawn pawn = this.invoker.Pawn;
			if (pawn == null)
			{
				return TaggedString.Empty;
			}
			return "Alert_CultistPsychicRitualDesc".Translate(this.CurPsychicRitual.def.Named("RITUAL"), pawn.Named("INVOKER")).CapitalizeFirst();
		}

		// Token: 0x0600B5CC RID: 46540 RVA: 0x00400670 File Offset: 0x003FE870
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Lord> lords = maps[i].lordManager.lords;
				for (int j = 0; j < lords.Count; j++)
				{
					Lord lord = lords[j];
					if (lord.faction == Faction.OfHoraxCult)
					{
						LordToil_PsychicRitual lordToil_PsychicRitual = lord.CurLordToil as LordToil_PsychicRitual;
						if (lordToil_PsychicRitual != null && !lordToil_PsychicRitual.RitualData.done)
						{
							PsychicRitualToil_InvokeHorax psychicRitualToil_InvokeHorax = lordToil_PsychicRitual.RitualData.CurPsychicRitualToil as PsychicRitualToil_InvokeHorax;
							if (psychicRitualToil_InvokeHorax != null && psychicRitualToil_InvokeHorax.invokerRole != null)
							{
								PsychicRitualRoleAssignments assignments = lordToil_PsychicRitual.RitualData.psychicRitual.assignments;
								Pawn pawn = (assignments != null) ? assignments.FirstAssignedPawn(psychicRitualToil_InvokeHorax.invokerRole) : null;
								if (pawn != null)
								{
									this.ritualLord = lord;
									this.invokeToil = psychicRitualToil_InvokeHorax;
									this.invoker = pawn;
									return AlertReport.CulpritIs(this.invoker);
								}
							}
						}
					}
				}
			}
			this.ritualLord = null;
			this.invokeToil = null;
			return false;
		}

		// Token: 0x04006889 RID: 26761
		private Lord ritualLord;

		// Token: 0x0400688A RID: 26762
		private PsychicRitualToil_InvokeHorax invokeToil;

		// Token: 0x0400688B RID: 26763
		private GlobalTargetInfo invoker;
	}
}
