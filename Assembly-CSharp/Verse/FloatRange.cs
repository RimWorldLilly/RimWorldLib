using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000065 RID: 101
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00016F5A File Offset: 0x0001515A
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00016F6B File Offset: 0x0001516B
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00016F7C File Offset: 0x0001517C
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00016F8D File Offset: 0x0001518D
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00016FA2 File Offset: 0x000151A2
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00016FB5 File Offset: 0x000151B5
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00016FC8 File Offset: 0x000151C8
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00016FDB File Offset: 0x000151DB
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00016FEA File Offset: 0x000151EA
		public bool IsZeros
		{
			get
			{
				return this.min == 0f && this.max == 0f;
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00017008 File Offset: 0x00015208
		public FloatRange(float value)
		{
			this.min = value;
			this.max = value;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00017018 File Offset: 0x00015218
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00017028 File Offset: 0x00015228
		public float ClampToRange(float value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001703C File Offset: 0x0001523C
		public float RandomInRangeSeeded(int seed)
		{
			return Rand.RangeSeeded(this.min, this.max, seed);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00017050 File Offset: 0x00015250
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00017064 File Offset: 0x00015264
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00017078 File Offset: 0x00015278
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00017091 File Offset: 0x00015291
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000170B6 File Offset: 0x000152B6
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000170CD File Offset: 0x000152CD
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000170ED File Offset: 0x000152ED
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00017110 File Offset: 0x00015310
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00017127 File Offset: 0x00015327
		public static FloatRange operator *(float val, FloatRange r)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00017140 File Offset: 0x00015340
		public static FloatRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split('~', StringSplitOptions.None);
			if (array.Length == 1)
			{
				float num = Convert.ToSingle(array[0], invariantCulture);
				return new FloatRange(num, num);
			}
			return new FloatRange(Convert.ToSingle(array[0], invariantCulture), Convert.ToSingle(array[1], invariantCulture));
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0001718A File Offset: 0x0001538A
		public override string ToString()
		{
			return this.min.ToString("G9") + "~" + this.max.ToString("G9");
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000171B6 File Offset: 0x000153B6
		public string ToString(string format)
		{
			return this.min.ToString(format) + "~" + this.max.ToString(format);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000171DA File Offset: 0x000153DA
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000171F4 File Offset: 0x000153F4
		public override bool Equals(object obj)
		{
			if (obj is FloatRange)
			{
				FloatRange other = (FloatRange)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001721B File Offset: 0x0001541B
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x04000176 RID: 374
		public float min;

		// Token: 0x04000177 RID: 375
		public float max;
	}
}
