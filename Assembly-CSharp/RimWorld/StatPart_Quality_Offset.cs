using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F9E RID: 8094
	public class StatPart_Quality_Offset : StatPart
	{
		// Token: 0x0600CDA6 RID: 52646 RVA: 0x004B93CD File Offset: 0x004B75CD
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ApplyTo(req))
			{
				val += this.QualityOffset(req.QualityCategory);
			}
		}

		// Token: 0x0600CDA7 RID: 52647 RVA: 0x004B93EC File Offset: 0x004B75EC
		public override string ExplanationPart(StatRequest req)
		{
			if (!this.ApplyTo(req))
			{
				return null;
			}
			return "StatsReport_QualityOffset".Translate() + ": " + this.QualityOffset(req.QualityCategory).ToString();
		}

		// Token: 0x0600CDA8 RID: 52648 RVA: 0x004B9438 File Offset: 0x004B7638
		private bool ApplyTo(StatRequest req)
		{
			return this.thingDefs == null || (req.Def != null && this.thingDefs.Contains(req.Def)) || (req.Thing != null && this.thingDefs.Contains(req.Thing.def));
		}

		// Token: 0x0600CDA9 RID: 52649 RVA: 0x004B9490 File Offset: 0x004B7690
		private float QualityOffset(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.offsetAwful;
			case QualityCategory.Poor:
				return this.offsetPoor;
			case QualityCategory.Normal:
				return this.offsetNormal;
			case QualityCategory.Good:
				return this.offsetGood;
			case QualityCategory.Excellent:
				return this.offsetExcellent;
			case QualityCategory.Masterwork:
				return this.offsetMasterwork;
			case QualityCategory.Legendary:
				return this.offsetLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04008325 RID: 33573
		public float offsetAwful;

		// Token: 0x04008326 RID: 33574
		public float offsetPoor;

		// Token: 0x04008327 RID: 33575
		public float offsetNormal;

		// Token: 0x04008328 RID: 33576
		public float offsetGood;

		// Token: 0x04008329 RID: 33577
		public float offsetExcellent;

		// Token: 0x0400832A RID: 33578
		public float offsetMasterwork;

		// Token: 0x0400832B RID: 33579
		public float offsetLegendary;

		// Token: 0x0400832C RID: 33580
		public List<ThingDef> thingDefs;
	}
}
