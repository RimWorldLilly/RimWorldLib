using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004C6 RID: 1222
	public class Hediff_RotStinkExposure : HediffWithComps
	{
		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06002208 RID: 8712 RVA: 0x000CA025 File Offset: 0x000C8225
		public override bool Visible
		{
			get
			{
				return base.Visible && !this.pawn.health.hediffSet.HasHediff(HediffDefOf.LungRot, false);
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x000CA04F File Offset: 0x000C824F
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
