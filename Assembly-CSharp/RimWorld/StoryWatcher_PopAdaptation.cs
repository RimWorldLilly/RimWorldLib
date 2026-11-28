using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011FD RID: 4605
	public class StoryWatcher_PopAdaptation : IExposable
	{
		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x0600726F RID: 29295 RVA: 0x00281151 File Offset: 0x0027F351
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x06007270 RID: 29296 RVA: 0x0028115C File Offset: 0x0027F35C
		public void Notify_PawnEvent(Pawn p, PopAdaptationEvent ev)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			if (DebugViewSettings.writeStoryteller)
			{
				Log.Message("PopAdaptation event: " + ev.ToString() + " - " + ((p != null) ? p.ToString() : null));
			}
			if (ev == PopAdaptationEvent.GainedColonist)
			{
				this.adaptDays = 0f;
			}
		}

		// Token: 0x06007271 RID: 29297 RVA: 0x002811BC File Offset: 0x0027F3BC
		public void PopAdaptationWatcherTick()
		{
			if (Find.TickManager.TicksGame % 30000 == 171)
			{
				float num = 0.5f;
				this.adaptDays += num;
			}
		}

		// Token: 0x06007272 RID: 29298 RVA: 0x002811F4 File Offset: 0x0027F3F4
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x06007273 RID: 29299 RVA: 0x0028120C File Offset: 0x0027F40C
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x06007274 RID: 29300 RVA: 0x0028121C File Offset: 0x0027F41C
		public void ResetAdaptDays()
		{
			this.adaptDays = 0f;
		}

		// Token: 0x0400486E RID: 18542
		private float adaptDays;

		// Token: 0x0400486F RID: 18543
		private const int UpdateInterval = 30000;
	}
}
