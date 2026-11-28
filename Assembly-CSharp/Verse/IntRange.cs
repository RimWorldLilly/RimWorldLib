using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000066 RID: 102
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0001723B File Offset: 0x0001543B
		public static IntRange Zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00017244 File Offset: 0x00015444
		[Obsolete]
		public static IntRange zero
		{
			get
			{
				return IntRange.Zero;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0001724B File Offset: 0x0001544B
		public static IntRange One
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00017254 File Offset: 0x00015454
		[Obsolete]
		public static IntRange one
		{
			get
			{
				return IntRange.One;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0001725B File Offset: 0x0001545B
		public static IntRange Invalid
		{
			get
			{
				return new IntRange(-1, -1);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00017264 File Offset: 0x00015464
		[Obsolete]
		public static IntRange invalid
		{
			get
			{
				return IntRange.Invalid;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0001726B File Offset: 0x0001546B
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0001727E File Offset: 0x0001547E
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00017291 File Offset: 0x00015491
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600036A RID: 874 RVA: 0x000172A8 File Offset: 0x000154A8
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600036B RID: 875 RVA: 0x000172BB File Offset: 0x000154BB
		public bool IsValid
		{
			get
			{
				return this != IntRange.Invalid;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000172CD File Offset: 0x000154CD
		public bool IsInvalid
		{
			get
			{
				return this == IntRange.Invalid;
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x000172DF File Offset: 0x000154DF
		public static IntRange Between(int min, int max)
		{
			return new IntRange(min, max);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000172E8 File Offset: 0x000154E8
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000172F8 File Offset: 0x000154F8
		public IntRange(int val)
		{
			this.min = val;
			this.max = val;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00017308 File Offset: 0x00015508
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00017328 File Offset: 0x00015528
		public static IntRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split('~', StringSplitOptions.None);
			if (array.Length == 1)
			{
				int num = Convert.ToInt32(array[0], invariantCulture);
				return new IntRange(num, num);
			}
			int num2 = array[0].NullOrEmpty() ? int.MinValue : Convert.ToInt32(array[0], invariantCulture);
			int num3 = array[1].NullOrEmpty() ? int.MaxValue : Convert.ToInt32(array[1], invariantCulture);
			return new IntRange(num2, num3);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00017396 File Offset: 0x00015596
		public override string ToString()
		{
			if (this.min == this.max)
			{
				return this.min.ToString();
			}
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000173D2 File Offset: 0x000155D2
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000173E8 File Offset: 0x000155E8
		public override bool Equals(object obj)
		{
			if (obj is IntRange)
			{
				IntRange other = (IntRange)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001740F File Offset: 0x0001560F
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0001742F File Offset: 0x0001562F
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00017439 File Offset: 0x00015639
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00017445 File Offset: 0x00015645
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}

		// Token: 0x04000178 RID: 376
		public int min;

		// Token: 0x04000179 RID: 377
		public int max;
	}
}
