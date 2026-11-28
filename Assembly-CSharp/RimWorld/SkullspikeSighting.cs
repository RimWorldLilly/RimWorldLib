using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF7 RID: 8183
	public struct SkullspikeSighting : IExposable
	{
		// Token: 0x17002558 RID: 9560
		// (get) Token: 0x0600CF54 RID: 53076 RVA: 0x004C5A26 File Offset: 0x004C3C26
		public int TicksSinceSighting
		{
			get
			{
				return Find.TickManager.TicksGame - this.tickSighted;
			}
		}

		// Token: 0x0600CF55 RID: 53077 RVA: 0x004C5A39 File Offset: 0x004C3C39
		public SkullspikeSighting(Thing skullspike, int tickSighted)
		{
			this.skullspike = skullspike;
			this.tickSighted = tickSighted;
		}

		// Token: 0x0600CF56 RID: 53078 RVA: 0x004C5A49 File Offset: 0x004C3C49
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.skullspike, "skullspike", false);
			Scribe_Values.Look<int>(ref this.tickSighted, "tickSighted", 0, false);
		}

		// Token: 0x040083DC RID: 33756
		public Thing skullspike;

		// Token: 0x040083DD RID: 33757
		public int tickSighted;

		// Token: 0x040083DE RID: 33758
		public const int MaxSightingAge = 1800;

		// Token: 0x040083DF RID: 33759
		public const int CheckRadius = 10;

		// Token: 0x040083E0 RID: 33760
		public const int CheckIntervalTicks = 60;
	}
}
