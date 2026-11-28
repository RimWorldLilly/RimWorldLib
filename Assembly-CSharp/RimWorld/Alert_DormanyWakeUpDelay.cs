using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB9 RID: 7097
	public class Alert_DormanyWakeUpDelay : Alert_ActionDelay
	{
		// Token: 0x0600B608 RID: 46600 RVA: 0x00401A5A File Offset: 0x003FFC5A
		public Alert_DormanyWakeUpDelay()
		{
		}

		// Token: 0x0600B609 RID: 46601 RVA: 0x00401A62 File Offset: 0x003FFC62
		public Alert_DormanyWakeUpDelay(SignalAction_DormancyWakeUp wakeUpDelay)
		{
			this.wakeUpDelay = wakeUpDelay;
		}

		// Token: 0x0600B60A RID: 46602 RVA: 0x00401A74 File Offset: 0x003FFC74
		public override AlertReport GetReport()
		{
			if (this.wakeUpDelay == null)
			{
				return AlertReport.Inactive;
			}
			SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = this.wakeUpDelay;
			if (((signalAction_DormancyWakeUp != null) ? signalAction_DormancyWakeUp.lord : null) != null && !this.wakeUpDelay.lord.ownedPawns.NullOrEmpty<Pawn>())
			{
				return AlertReport.CulpritsAre(this.wakeUpDelay.lord.ownedPawns);
			}
			return AlertReport.Active;
		}

		// Token: 0x0600B60B RID: 46603 RVA: 0x00401AD8 File Offset: 0x003FFCD8
		public override string GetLabel()
		{
			return "AlertHostilesWakingUp".Translate(this.wakeUpDelay.lord.faction, this.wakeUpDelay.delayTicks.ToStringTicksToPeriodVerbose(true, true)).CapitalizeFirst();
		}

		// Token: 0x0600B60C RID: 46604 RVA: 0x00401B28 File Offset: 0x003FFD28
		public override TaggedString GetExplanation()
		{
			return "AlertHostilesWakingUpDesc".Translate(this.wakeUpDelay.lord.faction).CapitalizeFirst();
		}

		// Token: 0x040068A2 RID: 26786
		private SignalAction_DormancyWakeUp wakeUpDelay;
	}
}
