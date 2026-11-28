using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001366 RID: 4966
	public class CompSchedule : ThingComp
	{
		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x06007B18 RID: 31512 RVA: 0x002BD14D File Offset: 0x002BB34D
		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)this.props;
			}
		}

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x06007B19 RID: 31513 RVA: 0x002BD15A File Offset: 0x002BB35A
		// (set) Token: 0x06007B1A RID: 31514 RVA: 0x002BD162 File Offset: 0x002BB362
		public bool Allowed
		{
			get
			{
				return this.intAllowed;
			}
			set
			{
				if (this.intAllowed == value)
				{
					return;
				}
				this.intAllowed = value;
				this.parent.BroadcastCompSignal(this.intAllowed ? "ScheduledOn" : "ScheduledOff");
			}
		}

		// Token: 0x06007B1B RID: 31515 RVA: 0x002BD194 File Offset: 0x002BB394
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		// Token: 0x06007B1C RID: 31516 RVA: 0x002BD1A3 File Offset: 0x002BB3A3
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		// Token: 0x06007B1D RID: 31517 RVA: 0x002BD1B4 File Offset: 0x002BB3B4
		public void RecalculateAllowed()
		{
			float num = GenLocalDate.DayPercent(this.parent);
			if (this.Props.startTime <= this.Props.endTime)
			{
				this.Allowed = (num > this.Props.startTime && num < this.Props.endTime);
				return;
			}
			this.Allowed = (num < this.Props.endTime || num > this.Props.startTime);
		}

		// Token: 0x06007B1E RID: 31518 RVA: 0x002BD230 File Offset: 0x002BB430
		public override string CompInspectStringExtra()
		{
			if (!this.Allowed)
			{
				return this.Props.offMessage;
			}
			return null;
		}

		// Token: 0x04004DB7 RID: 19895
		public const string ScheduledOnSignal = "ScheduledOn";

		// Token: 0x04004DB8 RID: 19896
		public const string ScheduledOffSignal = "ScheduledOff";

		// Token: 0x04004DB9 RID: 19897
		private bool intAllowed;
	}
}
