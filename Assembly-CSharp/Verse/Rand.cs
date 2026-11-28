using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x020000A8 RID: 168
	public static class Rand
	{
		// Token: 0x170000FF RID: 255
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x00027DF4 File Offset: 0x00025FF4
		public static int Seed
		{
			set
			{
				if (Rand.stateStack.Count == 0)
				{
					Log.ErrorOnce("Modifying the initial rand seed. Call PushState() first. The initial rand seed should always be based on the startup time and set only once.", 825343540);
				}
				Rand.seed = (uint)value;
				Rand.iterations = 0U;
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00027E1D File Offset: 0x0002601D
		public static RandBlock Block(int blockSeed)
		{
			return new RandBlock(blockSeed);
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x00027E25 File Offset: 0x00026025
		public static float Value
		{
			get
			{
				return (float)(((double)MurmurHash.GetInt(Rand.seed, Rand.iterations++) - -2147483648.0) / 4294967295.0);
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00027E54 File Offset: 0x00026054
		public static float ValueAsync(int seed)
		{
			return (float)(((double)MurmurHash.GetInt((uint)seed, 0U) - -2147483648.0) / 4294967295.0);
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x00027E73 File Offset: 0x00026073
		public static bool Bool
		{
			get
			{
				return Rand.Value < 0.5f;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x00027E81 File Offset: 0x00026081
		public static int Sign
		{
			get
			{
				if (!Rand.Bool)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00027E8D File Offset: 0x0002608D
		public static int Int
		{
			get
			{
				return MurmurHash.GetInt(Rand.seed, Rand.iterations++);
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00027EA8 File Offset: 0x000260A8
		public static Vector3 UnitVector3
		{
			get
			{
				return new Vector3(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x00027EF0 File Offset: 0x000260F0
		public static Vector2 UnitVector2
		{
			get
			{
				return new Vector2(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00027F28 File Offset: 0x00026128
		public static Vector2 InsideUnitCircle
		{
			get
			{
				Vector2 result;
				do
				{
					result = new Vector2(Rand.Value - 0.5f, Rand.Value - 0.5f) * 2f;
				}
				while (result.sqrMagnitude > 1f);
				return result;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00027F6C File Offset: 0x0002616C
		public static Vector3 InsideUnitCircleVec3
		{
			get
			{
				Vector2 insideUnitCircle = Rand.InsideUnitCircle;
				return new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x00027F98 File Offset: 0x00026198
		public static Vector3 InsideUnitSphere
		{
			get
			{
				Vector3 result;
				do
				{
					result = new Vector3(Rand.Value - 0.5f, Rand.Value - 0.5f, Rand.Value - 0.5f) * 2f;
				}
				while (result.sqrMagnitude > 1f);
				return result;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00027FE5 File Offset: 0x000261E5
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x00027FF7 File Offset: 0x000261F7
		private static ulong StateCompressed
		{
			get
			{
				return (ulong)Rand.seed | (ulong)Rand.iterations << 32;
			}
			set
			{
				Rand.seed = (uint)(value & (ulong)-1);
				Rand.iterations = (uint)(value >> 32 & (ulong)-1);
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00028010 File Offset: 0x00026210
		static Rand()
		{
			Rand.seed = (uint)DateTime.Now.GetHashCode();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00028043 File Offset: 0x00026243
		public static void EnsureStateStackEmpty()
		{
			if (Rand.stateStack.Count <= 0)
			{
				return;
			}
			Log.Warning("Random state stack is not empty. There were more calls to PushState than PopState. Fixing.");
			while (Rand.stateStack.Any<ulong>())
			{
				Rand.PopState();
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00028070 File Offset: 0x00026270
		public static Vector2 InsideAnnulus(float innerRadius, float outerRadius)
		{
			float f = 6.2831855f * Rand.Value;
			Vector2 a = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
			innerRadius *= innerRadius;
			outerRadius *= outerRadius;
			return Mathf.Sqrt(Mathf.Lerp(innerRadius, outerRadius, Rand.Value)) * a;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000280C0 File Offset: 0x000262C0
		public static Vector3 InsideAnnulusVector3(float innerRadius, float outerRadius)
		{
			Vector2 vector = Rand.InsideAnnulus(innerRadius, outerRadius);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x000280EC File Offset: 0x000262EC
		public static float Gaussian(float centerX = 0f, float widthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			return Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.2831855f * value2) * widthFactor + centerX;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00028128 File Offset: 0x00026328
		public static float GaussianAsymmetric(float centerX = 0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			float num = Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.2831855f * value2);
			if (num <= 0f)
			{
				return num * lowerWidthFactor + centerX;
			}
			return num * upperWidthFactor + centerX;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00028174 File Offset: 0x00026374
		public static T EnumValue<T>()
		{
			Array values = Enum.GetValues(typeof(T));
			return (T)((object)values.GetValue(Rand.Range(0, values.Length)));
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000281A8 File Offset: 0x000263A8
		public static int Range(int minInclusive, int maxExclusive)
		{
			if (maxExclusive <= minInclusive)
			{
				return minInclusive;
			}
			return minInclusive + Mathf.Abs(Rand.Int % (maxExclusive - minInclusive));
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000281C0 File Offset: 0x000263C0
		public static int RangeInclusive(int minInclusive, int maxInclusive)
		{
			if (maxInclusive <= minInclusive)
			{
				return minInclusive;
			}
			return Rand.Range(minInclusive, maxInclusive + 1);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000281D1 File Offset: 0x000263D1
		public static float Range(float minInclusive, float maxInclusive)
		{
			if (maxInclusive <= minInclusive)
			{
				return minInclusive;
			}
			return Rand.Value * (maxInclusive - minInclusive) + minInclusive;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000281E4 File Offset: 0x000263E4
		public static bool Chance(float chance)
		{
			return chance > 0f && (chance >= 1f || Rand.Value < chance);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00028202 File Offset: 0x00026402
		public static bool DynamicChance(int amountPlaced, int amountToPlace, int positionsRemaining)
		{
			return amountPlaced < amountToPlace && positionsRemaining > 0 && Rand.Chance((float)(amountToPlace - amountPlaced) / (float)positionsRemaining);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0002821A File Offset: 0x0002641A
		public static bool ChanceSeeded(float chance, int specialSeed)
		{
			Rand.PushState(specialSeed);
			bool result = Rand.Chance(chance);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0002822D File Offset: 0x0002642D
		public static float ValueSeeded(int specialSeed)
		{
			Rand.PushState(specialSeed);
			float value = Rand.Value;
			Rand.PopState();
			return value;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002823F File Offset: 0x0002643F
		public static float RangeSeeded(float minInclusive, float maxInclusive, int specialSeed)
		{
			Rand.PushState(specialSeed);
			float result = Rand.Range(minInclusive, maxInclusive);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00028253 File Offset: 0x00026453
		public static int RangeSeeded(int minInclusive, int maxExclusive, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.Range(minInclusive, maxExclusive);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00028267 File Offset: 0x00026467
		public static int RangeInclusiveSeeded(int minInclusive, int maxInclusive, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.RangeInclusive(minInclusive, maxInclusive);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0002827C File Offset: 0x0002647C
		public static T Element<T>(params T[] list)
		{
			float value = Rand.Value;
			return list[Rand.Range(0, Mathf.RoundToInt(value * (float)list.Length))];
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x000282A6 File Offset: 0x000264A6
		public static T Element<T>(T a, T b)
		{
			if (!Rand.Bool)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x000282B4 File Offset: 0x000264B4
		public static T Element<T>(T a, T b, T c)
		{
			float value = Rand.Value;
			if (value < 0.33333f)
			{
				return a;
			}
			if (value < 0.66666f)
			{
				return b;
			}
			return c;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000282DC File Offset: 0x000264DC
		public static T Element<T>(T a, T b, T c, T d)
		{
			float value = Rand.Value;
			if (value < 0.25f)
			{
				return a;
			}
			if (value < 0.5f)
			{
				return b;
			}
			if (value < 0.75f)
			{
				return c;
			}
			return d;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00028310 File Offset: 0x00026510
		public static T Element<T>(T a, T b, T c, T d, T e)
		{
			float value = Rand.Value;
			if (value < 0.2f)
			{
				return a;
			}
			if (value < 0.4f)
			{
				return b;
			}
			if (value < 0.6f)
			{
				return c;
			}
			if (value < 0.8f)
			{
				return d;
			}
			return e;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00028350 File Offset: 0x00026550
		public static T Element<T>(T a, T b, T c, T d, T e, T f)
		{
			float value = Rand.Value;
			if (value < 0.16666f)
			{
				return a;
			}
			if (value < 0.33333f)
			{
				return b;
			}
			if (value < 0.5f)
			{
				return c;
			}
			if (value < 0.66666f)
			{
				return d;
			}
			if (value < 0.83333f)
			{
				return e;
			}
			return f;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00028398 File Offset: 0x00026598
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB)
		{
			float num = weightA + weightB;
			if (Rand.Value < weightA / num)
			{
				return a;
			}
			return b;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x000283B8 File Offset: 0x000265B8
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC)
		{
			float num = weightA + weightB + weightC;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			return c;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x000283E8 File Offset: 0x000265E8
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD)
		{
			float num = weightA + weightB + weightC + weightD;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			return d;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00028428 File Offset: 0x00026628
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD, T e, float weightE)
		{
			float num = weightA + weightB + weightC + weightD + weightE;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			if (value < (weightA + weightB + weightC + weightD) / num)
			{
				return d;
			}
			return e;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0002847C File Offset: 0x0002667C
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD, T e, float weightE, T f, float weightF)
		{
			float num = weightA + weightB + weightC + weightD + weightE + weightF;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			if (value < (weightA + weightB + weightC + weightD) / num)
			{
				return d;
			}
			if (value < (weightA + weightB + weightC + weightD + weightE) / num)
			{
				return e;
			}
			return f;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x000284E6 File Offset: 0x000266E6
		public static void PushState()
		{
			Rand.stateStack.Push(Rand.StateCompressed);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000284F7 File Offset: 0x000266F7
		public static void PushState(int replacementSeed)
		{
			Rand.PushState();
			Rand.Seed = replacementSeed;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00028504 File Offset: 0x00026704
		public static void PopState()
		{
			Rand.StateCompressed = Rand.stateStack.Pop();
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00028518 File Offset: 0x00026718
		public static float ByCurve(SimpleCurve curve)
		{
			if (curve.PointsCount < 3)
			{
				throw new ArgumentException("curve has < 3 points");
			}
			if (curve[0].y != 0f || curve[curve.PointsCount - 1].y != 0f)
			{
				throw new ArgumentException("curve has start/end point with y != 0");
			}
			float num = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				if (curve[i].y < 0f)
				{
					throw new ArgumentException("curve has point with y < 0");
				}
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
			}
			float num2 = Rand.Range(0f, num);
			for (int j = 0; j < curve.PointsCount - 1; j++)
			{
				float num3 = (curve[j + 1].x - curve[j].x) * (curve[j].y + curve[j + 1].y);
				if (num3 >= num2)
				{
					float num4 = curve[j + 1].x - curve[j].x;
					float y = curve[j].y;
					float y2 = curve[j + 1].y;
					float num5 = num2 / (y + y2);
					if (Rand.Range(0f, (y + y2) / 2f) > Mathf.Lerp(y, y2, num5 / num4))
					{
						num5 = num4 - num5;
					}
					return num5 + curve[j].x;
				}
				num2 -= num3;
			}
			throw new Exception("Reached end of Rand.ByCurve without choosing a point.");
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00028718 File Offset: 0x00026918
		public static float ByCurveAverage(SimpleCurve curve)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
				num2 += (curve[i + 1].x - curve[i].x) * (curve[i].x * (2f * curve[i].y + curve[i + 1].y) + curve[i + 1].x * (curve[i].y + 2f * curve[i + 1].y));
			}
			return num2 / num / 3f;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00028830 File Offset: 0x00026A30
		public static bool MTBEventOccurs(float mtb, float mtbUnit, float ticksSinceLastCheck)
		{
			if (float.IsPositiveInfinity(mtb))
			{
				return false;
			}
			if (mtb == 0f)
			{
				return true;
			}
			if (mtb < 0f)
			{
				Log.Error("MTBEventOccurs with nevative mtb=" + mtb.ToString());
				return true;
			}
			if (mtbUnit <= 0f)
			{
				Log.Error("MTBEventOccurs with mtbUnit=" + mtbUnit.ToString());
				return false;
			}
			if (ticksSinceLastCheck <= 0f)
			{
				Log.Error("MTBEventOccurs with checkDuration=" + ticksSinceLastCheck.ToString());
				return false;
			}
			double num = (double)ticksSinceLastCheck / ((double)mtb * (double)mtbUnit);
			if (num <= 0.0)
			{
				Log.Error(string.Concat(new string[]
				{
					"chancePerCheck is ",
					num.ToString(),
					". mtb=",
					mtb.ToString(),
					", mtbUnit=",
					mtbUnit.ToString(),
					", checkDuration=",
					ticksSinceLastCheck.ToString()
				}));
				return false;
			}
			double num2 = 1.0;
			if (num < 0.0001)
			{
				while (num < 0.0001)
				{
					num *= 8.0;
					num2 /= 8.0;
				}
				if ((double)Rand.Value > num2)
				{
					return false;
				}
			}
			return (double)Rand.Value < num;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00028970 File Offset: 0x00026B70
		public static float CombineMTBs(List<float> mtbs)
		{
			if (mtbs.Count == 0)
			{
				return 0f;
			}
			if (mtbs.Count == 1)
			{
				return mtbs[0];
			}
			foreach (float num in mtbs)
			{
				if (num < 0f || float.IsNaN(num))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unexpected MTB value found while combining MTBs: ");
					defaultInterpolatedStringHandler.AppendFormatted<float>(num);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					return 0f;
				}
			}
			return (float)(1.0 / mtbs.Aggregate(0.0, (double acc, float mtb) => acc + 1.0 / (double)mtb));
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00028A54 File Offset: 0x00026C54
		public static float CombineMTBs(float mtb1, float mtb2)
		{
			return (float)(1.0 / (1.0 / (double)mtb1 + 1.0 / (double)mtb2));
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00028A7C File Offset: 0x00026C7C
		public static void SplitRandomly(float valueToSplit, int splitIntoCount, List<float> outValues, List<float> zeroIfFractionBelow = null, List<float> minFractions = null)
		{
			outValues.Clear();
			if (splitIntoCount > 0)
			{
				if (minFractions != null)
				{
					float num = 0f;
					for (int i = 0; i < minFractions.Count; i++)
					{
						if (minFractions[i] > 1f)
						{
							throw new ArgumentException("minFractions[i] > 1");
						}
						num += minFractions[i];
					}
					if (num > 1f)
					{
						throw new ArgumentException("minFractions sum > 1");
					}
				}
				float num2 = 0f;
				int num3 = 0;
				for (;;)
				{
					num3++;
					if (num3 > 10000)
					{
						break;
					}
					outValues.Clear();
					for (int j = 0; j < splitIntoCount; j++)
					{
						float num4 = Rand.Range(0f, 1f);
						num2 += num4;
						outValues.Add(num4);
					}
					bool flag = true;
					if (minFractions != null)
					{
						for (int k = 0; k < minFractions.Count; k++)
						{
							if (outValues[k] / num2 < minFractions[k])
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						goto IL_151;
					}
				}
				Log.Error(string.Concat(new string[]
				{
					"Could not meet SplitRandomly requirements. valueToSplit=",
					valueToSplit.ToString(),
					", splitIntoCount=",
					splitIntoCount.ToString(),
					", zeroIfFractionsBelow=",
					zeroIfFractionBelow.ToStringSafeEnumerable(),
					", minFractions=",
					minFractions.ToStringSafeEnumerable()
				}));
				IL_151:
				if (zeroIfFractionBelow != null)
				{
					for (int l = 0; l < zeroIfFractionBelow.Count; l++)
					{
						if (outValues[l] / num2 < zeroIfFractionBelow[l])
						{
							outValues[l] = 0f;
							num2 = 0f;
							for (int m = 0; m < outValues.Count; m++)
							{
								num2 += outValues[m];
							}
						}
					}
				}
				if (num2 != 0f)
				{
					for (int n = 0; n < outValues.Count; n++)
					{
						outValues[n] = outValues[n] / num2 * valueToSplit;
					}
				}
				return;
			}
			if (valueToSplit == 0f)
			{
				return;
			}
			throw new ArgumentException("splitIntoCount <= 0");
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00028C6C File Offset: 0x00026E6C
		[DebugOutput("System", false)]
		internal static void RandTests()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Random generation tests");
			stringBuilder.Append("To see texture outputs, turn on 'draw recorded noise' and run this again.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Performance test with " + 10000000.ToString() + " values");
			Stopwatch stopwatch = new Stopwatch();
			float num = 0f;
			Rand.PushState();
			stopwatch.Start();
			for (int i = 0; i < 10000000; i++)
			{
				num += Rand.Value;
			}
			stopwatch.Stop();
			Rand.PopState();
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Time: ",
				stopwatch.ElapsedMilliseconds.ToString(),
				"ms (for sum ",
				num.ToString(),
				")"
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Distribution test with " + 100000.ToString() + " values");
			DebugHistogram debugHistogram = new DebugHistogram(new float[]
			{
				0f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				1f
			});
			DebugHistogram debugHistogram2 = new DebugHistogram(new float[]
			{
				0f,
				0.01f,
				0.02f,
				0.03f,
				0.04f,
				0.05f,
				0.06f,
				0.07f,
				0.08f,
				0.09f,
				0.1f,
				1f
			});
			Rand.PushState();
			for (int j = 0; j < 100000; j++)
			{
				debugHistogram.Add(Rand.Value);
				debugHistogram2.Add(Rand.Value);
			}
			Rand.PopState();
			stringBuilder.AppendLine("Gross histogram:");
			debugHistogram.Display(stringBuilder);
			stringBuilder.AppendLine("Fine histogram:");
			debugHistogram2.Display(stringBuilder);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Long-term tests");
			for (int k = 0; k < 3; k++)
			{
				int num2 = 0;
				for (int l = 0; l < 5000000; l++)
				{
					if (Rand.MTBEventOccurs(250f, 60000f, 60f))
					{
						num2++;
					}
				}
				string value = string.Concat(new string[]
				{
					"MTB=",
					250.ToString(),
					" days, MTBUnit=",
					60000.ToString(),
					", check duration=",
					60.ToString(),
					" Simulated ",
					5000.ToString(),
					" days (",
					5000000.ToString(),
					" tests). Got ",
					num2.ToString(),
					" events."
				});
				stringBuilder.AppendLine(value);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Short-term tests");
			for (int m = 0; m < 5; m++)
			{
				int num3 = 0;
				for (int n = 0; n < 10000; n++)
				{
					if (Rand.MTBEventOccurs(1f, 24000f, 12000f))
					{
						num3++;
					}
				}
				string value2 = string.Concat(new string[]
				{
					"MTB=",
					1f.ToString(),
					" days, MTBUnit=",
					24000f.ToString(),
					", check duration=",
					12000f.ToString(),
					", ",
					10000.ToString(),
					" tests got ",
					num3.ToString(),
					" events."
				});
				stringBuilder.AppendLine(value2);
			}
			for (int num4 = 0; num4 < 5; num4++)
			{
				int num5 = 0;
				for (int num6 = 0; num6 < 10000; num6++)
				{
					if (Rand.MTBEventOccurs(2f, 24000f, 6000f))
					{
						num5++;
					}
				}
				string value3 = string.Concat(new string[]
				{
					"MTB=",
					2f.ToString(),
					" days, MTBUnit=",
					24000f.ToString(),
					", check duration=",
					6000f.ToString(),
					", ",
					10000.ToString(),
					" tests got ",
					num5.ToString(),
					" events."
				});
				stringBuilder.AppendLine(value3);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Near seed tests");
			DebugHistogram debugHistogram3 = new DebugHistogram(new float[]
			{
				0f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				1f
			});
			Rand.PushState();
			for (int num7 = 0; num7 < 1000; num7++)
			{
				Rand.Seed = num7;
				debugHistogram3.Add(Rand.Value);
			}
			Rand.PopState();
			debugHistogram3.Display(stringBuilder);
			int @int = Rand.Int;
			stringBuilder.AppendLine("Repeating single ValueSeeded with seed " + @int.ToString() + ". This should give the same result:");
			for (int num8 = 0; num8 < 4; num8++)
			{
				stringBuilder.AppendLine("   " + Rand.ValueSeeded(@int).ToString());
			}
			Log.Message(stringBuilder.ToString());
			if (DebugViewSettings.drawRecordedNoise)
			{
				int[] array = new int[65536];
				Rand.PushState();
				for (int num9 = 0; num9 < 65536; num9++)
				{
					array[num9] = (int)(Rand.Value * 1000f);
				}
				Rand.PopState();
				Rand.DebugStoreBucketsAsTexture("One rand output per pixel", array, 1000f, 256);
				int[] array2 = new int[65536];
				Rand.PushState();
				for (int num10 = 0; num10 < 65536; num10++)
				{
					Rand.Seed = num10;
					array2[num10] = (int)(Rand.Value * 1000f);
				}
				Rand.PopState();
				Rand.DebugStoreBucketsAsTexture("One seed per pixel", array2, 1000f, 256);
				int[] array3 = new int[65536];
				Rand.PushState();
				for (int num11 = 0; num11 < 300000; num11++)
				{
					int num12 = (int)(Rand.Value * 65536f);
					array3[num12]++;
				}
				Rand.PopState();
				float whiteValue = 9.155273f;
				Rand.DebugStoreBucketsAsTexture("Brighten random pixel index", array3, whiteValue, 256);
				int[] array4 = new int[65536];
				Rand.PushState();
				for (int num13 = 0; num13 < 300000; num13++)
				{
					int num14 = (int)(Rand.Value * 256f);
					int num15 = (int)(Rand.Value * 256f);
					int num16 = num14 + 256 * num15;
					array4[num16]++;
				}
				Rand.PopState();
				float whiteValue2 = 9.155273f;
				Rand.DebugStoreBucketsAsTexture("Brighten random pixel coords", array4, whiteValue2, 256);
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00029328 File Offset: 0x00027528
		private static void DebugStoreBucketsAsTexture(string name, int[] buckets, float whiteValue, int texSize)
		{
			Texture2D texture2D = new Texture2D(texSize, texSize);
			for (int i = 0; i < texSize; i++)
			{
				for (int j = 0; j < texSize; j++)
				{
					int num = i + j * texSize;
					float num2 = (float)buckets[num] / whiteValue;
					num2 = Mathf.Clamp01(num2);
					texture2D.SetPixel(i, j, new Color(num2, num2, num2));
				}
			}
			texture2D.Apply();
			NoiseDebugUI.StoreTexture(texture2D, name);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0002938C File Offset: 0x0002758C
		public static int RandSeedForHour(this Thing t, int salt)
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(t.HashOffset(), Find.TickManager.TicksAbs / 2500), salt);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x000293B0 File Offset: 0x000275B0
		public static bool TryRangeInclusiveWhere(int from, int to, Predicate<int> predicate, out int value)
		{
			int num = to - from + 1;
			if (num <= 0)
			{
				value = 0;
				return false;
			}
			int num2 = Mathf.Max(Mathf.RoundToInt(Mathf.Sqrt((float)num)), 5);
			for (int i = 0; i < num2; i++)
			{
				int num3 = Rand.RangeInclusive(from, to);
				if (predicate(num3))
				{
					value = num3;
					return true;
				}
			}
			Rand.tmpRange.Clear();
			for (int j = from; j <= to; j++)
			{
				Rand.tmpRange.Add(j);
			}
			Rand.tmpRange.Shuffle<int>();
			int k = 0;
			int count = Rand.tmpRange.Count;
			while (k < count)
			{
				if (predicate(Rand.tmpRange[k]))
				{
					value = Rand.tmpRange[k];
					return true;
				}
				k++;
			}
			value = 0;
			return false;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00029478 File Offset: 0x00027678
		public static Vector3 PointOnSphereCap(Vector3 center, float angle)
		{
			if (angle <= 0f)
			{
				return center;
			}
			if (angle >= 180f)
			{
				return Rand.UnitVector3;
			}
			float num = Rand.Range(Mathf.Cos(angle * 0.017453292f), 1f);
			float f = Rand.Range(0f, 6.2831855f);
			Vector3 point = new Vector3(Mathf.Sqrt(1f - num * num) * Mathf.Cos(f), Mathf.Sqrt(1f - num * num) * Mathf.Sin(f), num);
			return Quaternion.FromToRotation(Vector3.forward, center) * point;
		}

		// Token: 0x04000271 RID: 625
		private static uint seed;

		// Token: 0x04000272 RID: 626
		private static uint iterations;

		// Token: 0x04000273 RID: 627
		private static readonly Stack<ulong> stateStack = new Stack<ulong>();

		// Token: 0x04000274 RID: 628
		private static List<int> tmpRange = new List<int>();
	}
}
