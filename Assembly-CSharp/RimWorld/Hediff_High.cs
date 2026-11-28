using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001498 RID: 5272
	public class Hediff_High : HediffWithComps
	{
		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x0600813D RID: 33085 RVA: 0x002E2298 File Offset: 0x002E0498
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity <= 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent("F0");
			}
		}
	}
}
