using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015B6 RID: 5558
	public class DrugTakeRecord : IExposable
	{
		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x060089B7 RID: 35255 RVA: 0x0031DD46 File Offset: 0x0031BF46
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x060089B8 RID: 35256 RVA: 0x0031DD53 File Offset: 0x0031BF53
		// (set) Token: 0x060089B9 RID: 35257 RVA: 0x0031DD6A File Offset: 0x0031BF6A
		public int TimesTakenThisDay
		{
			get
			{
				if (this.thisDay != GenDate.DaysPassed)
				{
					return 0;
				}
				return this.timesTakenThisDayInt;
			}
			set
			{
				this.timesTakenThisDayInt = value;
				this.thisDay = GenDate.DaysPassed;
			}
		}

		// Token: 0x060089BA RID: 35258 RVA: 0x0031DD80 File Offset: 0x0031BF80
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}

		// Token: 0x060089BB RID: 35259 RVA: 0x0031DDD3 File Offset: 0x0031BFD3
		public void Notify_LeftSuspension(int suspendedTicks)
		{
			this.lastTakenTicks += suspendedTicks;
			if (GenDate.DaysPassedAt(Find.TickManager.TicksGame - suspendedTicks) == this.thisDay)
			{
				this.thisDay = GenDate.DaysPassed;
			}
		}

		// Token: 0x040054D2 RID: 21714
		public ThingDef drug;

		// Token: 0x040054D3 RID: 21715
		public int lastTakenTicks;

		// Token: 0x040054D4 RID: 21716
		private int timesTakenThisDayInt;

		// Token: 0x040054D5 RID: 21717
		private int thisDay;
	}
}
