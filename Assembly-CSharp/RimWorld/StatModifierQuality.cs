using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F17 RID: 3863
	public class StatModifierQuality
	{
		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x06006431 RID: 25649 RVA: 0x0023D71C File Offset: 0x0023B91C
		public string ToStringAsOffsetRange
		{
			get
			{
				string str = this.stat.Worker.ValueToString(this.GetValue(QualityCategory.Awful), false, ToStringNumberSense.Offset);
				string str2 = this.stat.Worker.ValueToString(this.GetValue(QualityCategory.Legendary), false, ToStringNumberSense.Offset);
				return str + " ~ " + str2;
			}
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x06006432 RID: 25650 RVA: 0x0023D768 File Offset: 0x0023B968
		public string ToStringAsFactorRange
		{
			get
			{
				string str = this.GetValue(QualityCategory.Awful).ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				string str2 = this.GetValue(QualityCategory.Legendary).ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				return str + " ~ " + str2;
			}
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x0023D7A0 File Offset: 0x0023B9A0
		public float GetValue(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.awful;
			case QualityCategory.Poor:
				return this.poor;
			case QualityCategory.Normal:
				return this.normal;
			case QualityCategory.Good:
				return this.good;
			case QualityCategory.Excellent:
				return this.excellent;
			case QualityCategory.Masterwork:
				return this.masterwork;
			case QualityCategory.Legendary:
				return this.legendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04003941 RID: 14657
		public StatDef stat;

		// Token: 0x04003942 RID: 14658
		private float awful;

		// Token: 0x04003943 RID: 14659
		private float poor;

		// Token: 0x04003944 RID: 14660
		private float normal;

		// Token: 0x04003945 RID: 14661
		private float good;

		// Token: 0x04003946 RID: 14662
		private float excellent;

		// Token: 0x04003947 RID: 14663
		private float masterwork;

		// Token: 0x04003948 RID: 14664
		private float legendary;
	}
}
