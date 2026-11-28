using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F3 RID: 6643
	public struct QualityRange : IEquatable<QualityRange>
	{
		// Token: 0x17001D4B RID: 7499
		// (get) Token: 0x0600A742 RID: 42818 RVA: 0x003B92E9 File Offset: 0x003B74E9
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		// Token: 0x0600A743 RID: 42819 RVA: 0x003B92F2 File Offset: 0x003B74F2
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0600A744 RID: 42820 RVA: 0x003B9302 File Offset: 0x003B7502
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		// Token: 0x0600A745 RID: 42821 RVA: 0x003B931B File Offset: 0x003B751B
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x0600A746 RID: 42822 RVA: 0x003B933B File Offset: 0x003B753B
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		// Token: 0x0600A747 RID: 42823 RVA: 0x003B9348 File Offset: 0x003B7548
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split('~', StringSplitOptions.None);
			return new QualityRange(ParseHelper.FromString<QualityCategory>(array[0]), ParseHelper.FromString<QualityCategory>(array[1]));
		}

		// Token: 0x0600A748 RID: 42824 RVA: 0x003B9374 File Offset: 0x003B7574
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x0600A749 RID: 42825 RVA: 0x003B93A2 File Offset: 0x003B75A2
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x0600A74A RID: 42826 RVA: 0x003B93C0 File Offset: 0x003B75C0
		public override bool Equals(object obj)
		{
			if (!(obj is QualityRange))
			{
				return false;
			}
			QualityRange qualityRange = (QualityRange)obj;
			return qualityRange.min == this.min && qualityRange.max == this.max;
		}

		// Token: 0x0600A74B RID: 42827 RVA: 0x003B93FC File Offset: 0x003B75FC
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x0400627E RID: 25214
		public QualityCategory min;

		// Token: 0x0400627F RID: 25215
		public QualityCategory max;
	}
}
