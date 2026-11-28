using System;

namespace Verse
{
	// Token: 0x0200065F RID: 1631
	public struct LabelValue
	{
		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x060033B2 RID: 13234 RVA: 0x0014CB63 File Offset: 0x0014AD63
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x060033B3 RID: 13235 RVA: 0x0014CB6B File Offset: 0x0014AD6B
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x0014CB73 File Offset: 0x0014AD73
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x0014CB8A File Offset: 0x0014AD8A
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0400218F RID: 8591
		private string label;

		// Token: 0x04002190 RID: 8592
		private string value;
	}
}
