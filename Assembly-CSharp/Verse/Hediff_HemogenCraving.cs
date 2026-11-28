using System;

namespace Verse
{
	// Token: 0x0200047D RID: 1149
	public class Hediff_HemogenCraving : HediffWithComps
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002015 RID: 8213 RVA: 0x000C1770 File Offset: 0x000BF970
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent();
			}
		}
	}
}
