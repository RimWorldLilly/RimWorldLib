using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD8 RID: 7128
	public class Alert_ActivatorCountdown : Alert
	{
		// Token: 0x0600B687 RID: 46727 RVA: 0x00404D90 File Offset: 0x00402F90
		public Alert_ActivatorCountdown()
		{
			this.defaultPriority = AlertPriority.High;
			this.requireRoyalty = true;
		}

		// Token: 0x170020FB RID: 8443
		// (get) Token: 0x0600B688 RID: 46728 RVA: 0x00404DB4 File Offset: 0x00402FB4
		private List<Thing> ActivatorCountdowns
		{
			get
			{
				this.activatorCountdownsResult.Clear();
				foreach (Map map in Find.Maps)
				{
					if (map.mapPawns.AnyColonistSpawned)
					{
						foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForDef(ThingDefOf.ActivatorCountdown)))
						{
							CompSendSignalOnCountdown compSendSignalOnCountdown = thing.TryGetComp<CompSendSignalOnCountdown>();
							if (compSendSignalOnCountdown != null && compSendSignalOnCountdown.ticksLeft > 0)
							{
								this.activatorCountdownsResult.Add(thing);
							}
						}
					}
				}
				return this.activatorCountdownsResult;
			}
		}

		// Token: 0x0600B689 RID: 46729 RVA: 0x00404E8C File Offset: 0x0040308C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ActivatorCountdowns);
		}

		// Token: 0x0600B68A RID: 46730 RVA: 0x00404E9C File Offset: 0x0040309C
		public override string GetLabel()
		{
			int count = this.activatorCountdownsResult.Count;
			if (count > 1)
			{
				return "ActivatorCountdownMultiple".Translate(count);
			}
			if (count == 0)
			{
				return string.Empty;
			}
			CompSendSignalOnCountdown compSendSignalOnCountdown = this.activatorCountdownsResult[0].TryGetComp<CompSendSignalOnCountdown>();
			return "ActivatorCountdown".Translate(compSendSignalOnCountdown.ticksLeft.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x0600B68B RID: 46731 RVA: 0x00404F0C File Offset: 0x0040310C
		public override TaggedString GetExplanation()
		{
			int count = this.activatorCountdownsResult.Count;
			if (count > 1)
			{
				return "ActivatorCountdownDescMultiple".Translate(count);
			}
			if (count == 0)
			{
				return string.Empty;
			}
			return "ActivatorCountdownDesc".Translate();
		}

		// Token: 0x040068D4 RID: 26836
		private List<Thing> activatorCountdownsResult = new List<Thing>();
	}
}
