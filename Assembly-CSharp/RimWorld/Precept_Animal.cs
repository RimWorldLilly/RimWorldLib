using System;

namespace RimWorld
{
	// Token: 0x02001646 RID: 5702
	public class Precept_Animal : Precept_ThingDef
	{
		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x06008F12 RID: 36626 RVA: 0x00349A28 File Offset: 0x00347C28
		public override string UIInfoFirstLine
		{
			get
			{
				if (base.ThingDef == null)
				{
					return base.UIInfoFirstLine;
				}
				return base.ThingDef.LabelCap;
			}
		}

		// Token: 0x170018B4 RID: 6324
		// (get) Token: 0x06008F13 RID: 36627 RVA: 0x00349A49 File Offset: 0x00347C49
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + base.ThingDef.LabelCap;
			}
		}
	}
}
