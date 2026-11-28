using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB2 RID: 7090
	public class Alert_CustomCritical : Alert_Critical
	{
		// Token: 0x0600B5EE RID: 46574 RVA: 0x00401246 File Offset: 0x003FF446
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x0600B5EF RID: 46575 RVA: 0x0040124E File Offset: 0x003FF44E
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x0600B5F0 RID: 46576 RVA: 0x0040125B File Offset: 0x003FF45B
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x04006897 RID: 26775
		public string label;

		// Token: 0x04006898 RID: 26776
		public string explanation;

		// Token: 0x04006899 RID: 26777
		public AlertReport report;
	}
}
