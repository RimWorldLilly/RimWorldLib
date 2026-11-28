using System;

namespace Verse.Grammar
{
	// Token: 0x020007B2 RID: 1970
	public class Rule_Number : Rule
	{
		// Token: 0x06003F82 RID: 16258 RVA: 0x00196ADC File Offset: 0x00194CDC
		public override Rule DeepCopy()
		{
			Rule_Number rule_Number = (Rule_Number)base.DeepCopy();
			rule_Number.range = this.range;
			rule_Number.selectionWeight = this.selectionWeight;
			return rule_Number;
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06003F83 RID: 16259 RVA: 0x00196B01 File Offset: 0x00194D01
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x00196B0C File Offset: 0x00194D0C
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x00196B2C File Offset: 0x00194D2C
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}

		// Token: 0x04002A00 RID: 10752
		private IntRange range = IntRange.Zero;

		// Token: 0x04002A01 RID: 10753
		public int selectionWeight = 1;
	}
}
