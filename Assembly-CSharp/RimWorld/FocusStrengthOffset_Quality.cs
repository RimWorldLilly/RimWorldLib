using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FB RID: 6395
	public class FocusStrengthOffset_Quality : FocusStrengthOffset_Curve
	{
		// Token: 0x17001BA3 RID: 7075
		// (get) Token: 0x0600A058 RID: 41048 RVA: 0x00396637 File Offset: 0x00394837
		public override bool NeedsToBeSpawned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600A059 RID: 41049 RVA: 0x0039663C File Offset: 0x0039483C
		protected override float SourceValue(Thing parent)
		{
			QualityCategory qualityCategory;
			parent.TryGetQuality(out qualityCategory);
			return (float)qualityCategory;
		}

		// Token: 0x0600A05A RID: 41050 RVA: 0x00396654 File Offset: 0x00394854
		public override float MaxOffset(Thing parent = null)
		{
			if (parent != null)
			{
				return 0f;
			}
			return base.MaxOffset(null);
		}

		// Token: 0x0600A05B RID: 41051 RVA: 0x00396668 File Offset: 0x00394868
		public override float MinOffset(Thing parent = null)
		{
			if (parent != null)
			{
				return this.GetOffset(parent, null);
			}
			return this.curve[0].y;
		}

		// Token: 0x17001BA4 RID: 7076
		// (get) Token: 0x0600A05C RID: 41052 RVA: 0x00396695 File Offset: 0x00394895
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_FromQuality";
			}
		}
	}
}
