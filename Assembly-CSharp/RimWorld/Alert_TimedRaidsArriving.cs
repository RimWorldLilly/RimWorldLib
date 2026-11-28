using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C03 RID: 7171
	public class Alert_TimedRaidsArriving : Alert
	{
		// Token: 0x0600B731 RID: 46897 RVA: 0x00408F60 File Offset: 0x00407160
		private void CalculateTargets()
		{
			this.timedRaidsArrivingSoon.Clear();
			this.targets.Clear();
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
			{
				foreach (WorldObjectComp worldObjectComp in worldObject.AllComps)
				{
					TimedDetectionRaids timedDetectionRaids = worldObjectComp as TimedDetectionRaids;
					if (timedDetectionRaids != null && this.ShouldAlertTimedRaids(timedDetectionRaids))
					{
						this.timedRaidsArrivingSoon.Add(timedDetectionRaids);
						if (!this.targets.Contains(worldObject))
						{
							this.targets.Add(worldObject);
						}
					}
				}
			}
		}

		// Token: 0x0600B732 RID: 46898 RVA: 0x00409048 File Offset: 0x00407248
		private bool ShouldAlertTimedRaids(TimedDetectionRaids timedDetectionRaids)
		{
			return timedDetectionRaids.alertRaidsArrivingIn && timedDetectionRaids.DetectionCountdownStarted && timedDetectionRaids.RaidsSentCount == 0;
		}

		// Token: 0x0600B733 RID: 46899 RVA: 0x00409068 File Offset: 0x00407268
		public override string GetLabel()
		{
			return "AlertTimedRaidsArrivingIn".Translate(this.timedRaidsArrivingSoon.MinBy((TimedDetectionRaids t) => t.TicksLeftToSendRaids).TicksLeftToSendRaids.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x0600B734 RID: 46900 RVA: 0x004090C0 File Offset: 0x004072C0
		public override TaggedString GetExplanation()
		{
			return "AlertTimedRaidsArrivingInDesc".Translate((from t in this.timedRaidsArrivingSoon
			select "AlertTimedRaidsArrivingAt".Translate(t.parent.Label, t.TicksLeftToSendRaids.ToStringTicksToPeriodVerbose(true, true)).Resolve()).ToLineList("  - ", false));
		}

		// Token: 0x0600B735 RID: 46901 RVA: 0x00409111 File Offset: 0x00407311
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006917 RID: 26903
		private List<TimedDetectionRaids> timedRaidsArrivingSoon = new List<TimedDetectionRaids>();

		// Token: 0x04006918 RID: 26904
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
