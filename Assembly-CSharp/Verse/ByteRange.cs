using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000060 RID: 96
	public struct ByteRange : IEquatable<ByteRange>
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00013FAF File Offset: 0x000121AF
		public static ByteRange Zero
		{
			get
			{
				return new ByteRange(0, 0);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00013FB8 File Offset: 0x000121B8
		public static ByteRange One
		{
			get
			{
				return new ByteRange(1, 1);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00013FC1 File Offset: 0x000121C1
		public byte TrueMin
		{
			get
			{
				return Math.Min(this.min, this.max);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00013FD4 File Offset: 0x000121D4
		public byte TrueMax
		{
			get
			{
				return Math.Max(this.min, this.max);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x00013FE7 File Offset: 0x000121E7
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00013FFE File Offset: 0x000121FE
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive((int)this.min, (int)this.max);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00014011 File Offset: 0x00012211
		public ByteRange(byte min, byte max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00014021 File Offset: 0x00012221
		public int Lerped(float lerpFactor)
		{
			return (int)this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00014040 File Offset: 0x00012240
		public static ByteRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split('~', StringSplitOptions.None);
			if (array.Length == 1)
			{
				byte b = Convert.ToByte(array[0], invariantCulture);
				return new ByteRange(b, b);
			}
			byte b2 = array[0].NullOrEmpty() ? 0 : Convert.ToByte(array[0], invariantCulture);
			byte b3 = array[1].NullOrEmpty() ? byte.MaxValue : Convert.ToByte(array[1], invariantCulture);
			return new ByteRange(b2, b3);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000140AA File Offset: 0x000122AA
		public override string ToString()
		{
			if (this.min == this.max)
			{
				return this.min.ToString();
			}
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000140E6 File Offset: 0x000122E6
		public override int GetHashCode()
		{
			return Gen.HashCombineInt((int)this.min, (int)this.max);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000140FC File Offset: 0x000122FC
		public override bool Equals(object obj)
		{
			if (obj is ByteRange)
			{
				ByteRange other = (ByteRange)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00014123 File Offset: 0x00012323
		public bool Equals(ByteRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00014143 File Offset: 0x00012343
		public static bool operator ==(ByteRange lhs, ByteRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0001414D File Offset: 0x0001234D
		public static bool operator !=(ByteRange lhs, ByteRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00014159 File Offset: 0x00012359
		internal bool Includes(int val)
		{
			return val >= (int)this.min && val <= (int)this.max;
		}

		// Token: 0x0400016A RID: 362
		public byte min;

		// Token: 0x0400016B RID: 363
		public byte max;
	}
}
