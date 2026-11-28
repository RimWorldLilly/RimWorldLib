using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C1D RID: 7197
	public abstract class Alert_Analyzable : Alert
	{
		// Token: 0x1700211F RID: 8479
		// (get) Token: 0x0600B7A3 RID: 47011
		protected abstract ThingDef Def { get; }

		// Token: 0x0600B7A4 RID: 47012 RVA: 0x0040BCB0 File Offset: 0x00409EB0
		public override AlertReport GetReport()
		{
			if (this.Def == null)
			{
				return AlertReport.Inactive;
			}
			this.targets.Clear();
			foreach (Map map in Find.Maps)
			{
				this.targets.AddRange(map.listerThings.ThingsOfDef(this.Def));
			}
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006947 RID: 26951
		protected List<Thing> targets = new List<Thing>();
	}
}
