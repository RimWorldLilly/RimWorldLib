using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183F RID: 6207
	public class BreakdownManager : MapComponent
	{
		// Token: 0x06009B6E RID: 39790 RVA: 0x00380743 File Offset: 0x0037E943
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x06009B6F RID: 39791 RVA: 0x00380762 File Offset: 0x0037E962
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x06009B70 RID: 39792 RVA: 0x0038078A File Offset: 0x0037E98A
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x06009B71 RID: 39793 RVA: 0x003807AC File Offset: 0x0037E9AC
		public override void MapComponentTick()
		{
			if (Find.TickManager.TicksGame % 1041 == 0)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CheckForBreakdown();
				}
			}
		}

		// Token: 0x06009B72 RID: 39794 RVA: 0x003807F2 File Offset: 0x0037E9F2
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x06009B73 RID: 39795 RVA: 0x00380801 File Offset: 0x0037EA01
		public void Notify_Repaired(Thing thing)
		{
			this.brokenDownThings.Remove(thing);
		}

		// Token: 0x04005C5E RID: 23646
		private List<CompBreakdownable> comps = new List<CompBreakdownable>();

		// Token: 0x04005C5F RID: 23647
		public HashSet<Thing> brokenDownThings = new HashSet<Thing>();

		// Token: 0x04005C60 RID: 23648
		public const int CheckIntervalTicks = 1041;
	}
}
