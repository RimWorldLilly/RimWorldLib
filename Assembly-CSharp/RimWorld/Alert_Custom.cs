using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB1 RID: 7089
	public class Alert_Custom : Alert
	{
		// Token: 0x0600B5EA RID: 46570 RVA: 0x00401221 File Offset: 0x003FF421
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x0600B5EB RID: 46571 RVA: 0x00401229 File Offset: 0x003FF429
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x0600B5EC RID: 46572 RVA: 0x00401236 File Offset: 0x003FF436
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x04006894 RID: 26772
		public string label;

		// Token: 0x04006895 RID: 26773
		public string explanation;

		// Token: 0x04006896 RID: 26774
		public AlertReport report;
	}
}
