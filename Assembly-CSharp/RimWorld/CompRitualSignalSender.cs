using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195A RID: 6490
	public class CompRitualSignalSender : ThingComp, IThingGlower
	{
		// Token: 0x0600A2ED RID: 41709 RVA: 0x003A34DA File Offset: 0x003A16DA
		public bool ShouldBeLitNow()
		{
			return this.ritualTarget;
		}

		// Token: 0x0600A2EE RID: 41710 RVA: 0x003A34E2 File Offset: 0x003A16E2
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(30))
			{
				bool flag = this.ritualTarget;
				this.ritualTarget = this.parent.IsRitualTarget();
				if (flag != this.ritualTarget)
				{
					this.parent.BroadcastCompSignal("RitualTargetChanged");
				}
			}
		}

		// Token: 0x0600A2EF RID: 41711 RVA: 0x003A3522 File Offset: 0x003A1722
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.ritualTarget, "ritualTarget", false, false);
		}

		// Token: 0x0400604B RID: 24651
		public bool ritualTarget;

		// Token: 0x0400604C RID: 24652
		private const int CheckInterval = 30;

		// Token: 0x0400604D RID: 24653
		public const string RitualTargetChangedSignal = "RitualTargetChanged";
	}
}
