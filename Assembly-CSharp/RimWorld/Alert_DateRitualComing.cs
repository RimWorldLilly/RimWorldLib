using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C07 RID: 7175
	public class Alert_DateRitualComing : Alert
	{
		// Token: 0x0600B746 RID: 46918 RVA: 0x0040991F File Offset: 0x00407B1F
		public Alert_DateRitualComing()
		{
			this.defaultLabel = "AlertRitualComing".Translate();
			this.requireIdeology = true;
		}

		// Token: 0x0600B747 RID: 46919 RVA: 0x00409950 File Offset: 0x00407B50
		private void UpcomingRituals()
		{
			this.ritualEntries.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept_Ritual precept_Ritual in ideo.PreceptsListForReading.OfType<Precept_Ritual>())
				{
					if (!precept_Ritual.isAnytime)
					{
						RitualObligationTrigger ritualObligationTrigger = precept_Ritual.obligationTriggers.FirstOrDefault((RitualObligationTrigger o) => o is RitualObligationTrigger_Date);
						if (ritualObligationTrigger != null)
						{
							RitualObligationTrigger_Date ritualObligationTrigger_Date = (RitualObligationTrigger_Date)ritualObligationTrigger;
							int num = ritualObligationTrigger_Date.OccursOnTick();
							int num2 = ritualObligationTrigger_Date.CurrentTickRelative();
							if ((float)(num - num2) < 180000f && num2 < num)
							{
								this.ritualEntries.Add("AlertRitualComingEntry".Translate(precept_Ritual.LabelCap, ritualObligationTrigger_Date.DateString, (num - num2).ToStringTicksToPeriodVerbose(true, true)));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B748 RID: 46920 RVA: 0x00409A98 File Offset: 0x00407C98
		public override TaggedString GetExplanation()
		{
			return "AlertRitualComingDesc".Translate() + ":\n" + this.ritualEntries.ToLineList("  - ") + "\n\n" + "AlertRitualComingExtra".Translate(RitualObligation.DaysToExpire).Resolve();
		}

		// Token: 0x0600B749 RID: 46921 RVA: 0x00409AF9 File Offset: 0x00407CF9
		public override AlertReport GetReport()
		{
			this.UpcomingRituals();
			return this.ritualEntries.Count > 0;
		}

		// Token: 0x04006922 RID: 26914
		private const float TicksBeforeWarning = 180000f;

		// Token: 0x04006923 RID: 26915
		private List<string> ritualEntries = new List<string>();
	}
}
