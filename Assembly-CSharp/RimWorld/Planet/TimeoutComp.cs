using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002369 RID: 9065
	public class TimeoutComp : WorldObjectComp
	{
		// Token: 0x17002787 RID: 10119
		// (get) Token: 0x0600E0C9 RID: 57545 RVA: 0x00538753 File Offset: 0x00536953
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		// Token: 0x17002788 RID: 10120
		// (get) Token: 0x0600E0CA RID: 57546 RVA: 0x00538761 File Offset: 0x00536961
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		// Token: 0x17002789 RID: 10121
		// (get) Token: 0x0600E0CB RID: 57547 RVA: 0x00538782 File Offset: 0x00536982
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		// Token: 0x1700278A RID: 10122
		// (get) Token: 0x0600E0CC RID: 57548 RVA: 0x00538797 File Offset: 0x00536997
		public int TicksLeft
		{
			get
			{
				if (!this.Active)
				{
					return 0;
				}
				return this.timeoutEndTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600E0CD RID: 57549 RVA: 0x005387B4 File Offset: 0x005369B4
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x0600E0CE RID: 57550 RVA: 0x005387C8 File Offset: 0x005369C8
		public void StopTimeout()
		{
			this.timeoutEndTick = -1;
		}

		// Token: 0x0600E0CF RID: 57551 RVA: 0x005387D1 File Offset: 0x005369D1
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			if (this.ShouldRemoveWorldObjectNow)
			{
				this.parent.Destroy();
			}
		}

		// Token: 0x0600E0D0 RID: 57552 RVA: 0x005387ED File Offset: 0x005369ED
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		// Token: 0x0600E0D1 RID: 57553 RVA: 0x00538807 File Offset: 0x00536A07
		public override string CompInspectStringExtra()
		{
			if (this.Active && !base.ParentHasMap)
			{
				return "WorldObjectTimeout".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			return null;
		}

		// Token: 0x0400901A RID: 36890
		private int timeoutEndTick = -1;
	}
}
