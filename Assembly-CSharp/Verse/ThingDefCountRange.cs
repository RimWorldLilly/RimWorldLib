using System;

namespace Verse
{
	// Token: 0x020006B3 RID: 1715
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x0600367A RID: 13946 RVA: 0x0015E5B1 File Offset: 0x0015C7B1
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x0600367B RID: 13947 RVA: 0x0015E5B9 File Offset: 0x0015C7B9
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x0015E5C1 File Offset: 0x0015C7C1
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x0600367D RID: 13949 RVA: 0x0015E5CE File Offset: 0x0015C7CE
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x0600367E RID: 13950 RVA: 0x0015E5DB File Offset: 0x0015C7DB
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x0600367F RID: 13951 RVA: 0x0015E5E8 File Offset: 0x0015C7E8
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x0015E5F5 File Offset: 0x0015C7F5
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x0015E605 File Offset: 0x0015C805
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x0015E618 File Offset: 0x0015C818
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x0015E64F File Offset: 0x0015C84F
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x0015E65D File Offset: 0x0015C85D
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x0015E66C File Offset: 0x0015C86C
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x0015E684 File Offset: 0x0015C884
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x0015E692 File Offset: 0x0015C892
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x0015E6B5 File Offset: 0x0015C8B5
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x0015E6C1 File Offset: 0x0015C8C1
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x0015E6E0 File Offset: 0x0015C8E0
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			IntRange intRange = this.countRange;
			array[num] = intRange.ToString();
			array[2] = "x ";
			array[3] = ((this.thingDef != null) ? this.thingDef.defName : "null");
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x0015E744 File Offset: 0x0015C944
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x0015E757 File Offset: 0x0015C957
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x0015E773 File Offset: 0x0015C973
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}

		// Token: 0x0400233F RID: 9023
		private ThingDef thingDef;

		// Token: 0x04002340 RID: 9024
		private IntRange countRange;
	}
}
