using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200007F RID: 127
	public static class GenMath
	{
		// Token: 0x06000541 RID: 1345 RVA: 0x0001E9A0 File Offset: 0x0001CBA0
		public static float RoundedHundredth(float f)
		{
			return Mathf.Round(f * 100f) / 100f;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001E9B4 File Offset: 0x0001CBB4
		public static int RoundTo(int value, int roundToNearest)
		{
			return (int)Math.Round((double)((float)value / (float)roundToNearest)) * roundToNearest;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001E9C4 File Offset: 0x0001CBC4
		public static float RoundTo(float value, float roundToNearest)
		{
			return (float)((int)Math.Round((double)(value / roundToNearest))) * roundToNearest;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001E9D4 File Offset: 0x0001CBD4
		public static float ChanceEitherHappens(float chanceA, float chanceB)
		{
			return chanceA + (1f - chanceA) * chanceB;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001E9E1 File Offset: 0x0001CBE1
		public static float SmootherStep(float edge0, float edge1, float x)
		{
			x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
			return x * x * x * (x * (x * 6f - 15f) + 10f);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001EA0C File Offset: 0x0001CC0C
		public static int RoundRandom(float f)
		{
			return (int)f + ((Rand.Value < f % 1f) ? 1 : 0);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001EA1F File Offset: 0x0001CC1F
		public static float WeightedAverage(float A, float weightA, float B, float weightB)
		{
			return (A * weightA + B * weightB) / (weightA + weightB);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001EA2C File Offset: 0x0001CC2C
		public static float Median<T>(IList<T> list, Func<T, float> orderBy, float noneValue = 0f, float center = 0.5f)
		{
			if (list.NullOrEmpty<T>())
			{
				return noneValue;
			}
			GenMath.tmpElements.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				GenMath.tmpElements.Add(orderBy(list[i]));
			}
			GenMath.tmpElements.Sort();
			return GenMath.tmpElements[Mathf.Min(Mathf.FloorToInt((float)GenMath.tmpElements.Count * center), GenMath.tmpElements.Count - 1)];
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001EAAC File Offset: 0x0001CCAC
		public static float WeightedMedian(IList<Pair<float, float>> list, float noneValue = 0f, float center = 0.5f)
		{
			GenMath.tmpPairs.Clear();
			GenMath.tmpPairs.AddRange(list);
			float num = 0f;
			for (int i = 0; i < GenMath.tmpPairs.Count; i++)
			{
				float second = GenMath.tmpPairs[i].Second;
				if (second < 0f)
				{
					Log.ErrorOnce("Negative weight in WeightedMedian: " + second.ToString(), GenMath.tmpPairs.GetHashCode());
				}
				else
				{
					num += second;
				}
			}
			if (num <= 0f)
			{
				return noneValue;
			}
			GenMath.tmpPairs.SortBy((Pair<float, float> x) => x.First);
			float num2 = 0f;
			for (int j = 0; j < GenMath.tmpPairs.Count; j++)
			{
				float first = GenMath.tmpPairs[j].First;
				float second2 = GenMath.tmpPairs[j].Second;
				num2 += second2 / num;
				if (num2 >= center)
				{
					return first;
				}
			}
			return GenMath.tmpPairs.Last<Pair<float, float>>().First;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001EBCD File Offset: 0x0001CDCD
		public static float Sqrt(float f)
		{
			return (float)Math.Sqrt((double)f);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001EBD8 File Offset: 0x0001CDD8
		public static float LerpDouble(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			float num = (x - inFrom) / (inTo - inFrom);
			return outFrom + (outTo - outFrom) * num;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001EBF5 File Offset: 0x0001CDF5
		public static float LerpDoubleClamped(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			return GenMath.LerpDouble(inFrom, inTo, outFrom, outTo, Mathf.Clamp(x, Mathf.Min(inFrom, inTo), Mathf.Max(inFrom, inTo)));
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001EC15 File Offset: 0x0001CE15
		public static float Reflection(float value, float mirror)
		{
			return mirror - (value - mirror);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001EC1C File Offset: 0x0001CE1C
		public static Quaternion ToQuat(this float ang)
		{
			return Quaternion.AngleAxis(ang, Vector3.up);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001EC2C File Offset: 0x0001CE2C
		public static float GetFactorInInterval(float min, float mid, float max, float power, float x)
		{
			if (min > max)
			{
				return 0f;
			}
			if (x <= min || x >= max)
			{
				return 0f;
			}
			if (x == mid)
			{
				return 1f;
			}
			float f;
			if (x < mid)
			{
				f = 1f - (mid - x) / (mid - min);
			}
			else
			{
				f = 1f - (x - mid) / (max - mid);
			}
			return Mathf.Pow(f, power);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001EC90 File Offset: 0x0001CE90
		public static float FlatHill(float min, float lower, float upper, float max, float x)
		{
			if (x < min)
			{
				return 0f;
			}
			if (x < lower)
			{
				return Mathf.InverseLerp(min, lower, x);
			}
			if (x < upper)
			{
				return 1f;
			}
			if (x < max)
			{
				return Mathf.InverseLerp(max, upper, x);
			}
			return 0f;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001ECCC File Offset: 0x0001CECC
		public static float FlatHill(float minY, float min, float lower, float upper, float max, float maxY, float x)
		{
			if (x < min)
			{
				return minY;
			}
			if (x < lower)
			{
				return GenMath.LerpDouble(min, lower, minY, 1f, x);
			}
			if (x < upper)
			{
				return 1f;
			}
			if (x < max)
			{
				return GenMath.LerpDouble(upper, max, 1f, maxY, x);
			}
			return maxY;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001ED1A File Offset: 0x0001CF1A
		public static int OctileDistance(int dx, int dz, int cardinal, int diagonal)
		{
			return cardinal * (dx + dz) + (diagonal - 2 * cardinal) * Mathf.Min(dx, dz);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001ED2F File Offset: 0x0001CF2F
		public static float UnboundedValueToFactor(float val)
		{
			if (val > 0f)
			{
				return 1f + val;
			}
			return 1f / (1f - val);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001ED50 File Offset: 0x0001CF50
		[DebugOutput("System", false)]
		public static void TestMathPerf()
		{
			IntVec3 intVec = new IntVec3(72, 0, 65);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Math perf tests (" + 10000000f.ToString() + " tests each)");
			float num = 0f;
			Stopwatch stopwatch = Stopwatch.StartNew();
			int num2 = 0;
			while ((float)num2 < 10000000f)
			{
				num += (float)Math.Sqrt(101.20999908447266);
				num2++;
			}
			stringBuilder.AppendLine("(float)System.Math.Sqrt(" + 101.21f.ToString() + "): " + stopwatch.ElapsedTicks.ToString());
			Stopwatch stopwatch2 = Stopwatch.StartNew();
			int num3 = 0;
			while ((float)num3 < 10000000f)
			{
				num += Mathf.Sqrt(101.21f);
				num3++;
			}
			stringBuilder.AppendLine("UnityEngine.Mathf.Sqrt(" + 101.21f.ToString() + "): " + stopwatch2.ElapsedTicks.ToString());
			Stopwatch stopwatch3 = Stopwatch.StartNew();
			int num4 = 0;
			while ((float)num4 < 10000000f)
			{
				num += GenMath.Sqrt(101.21f);
				num4++;
			}
			stringBuilder.AppendLine("Verse.GenMath.Sqrt(" + 101.21f.ToString() + "): " + stopwatch3.ElapsedTicks.ToString());
			Stopwatch stopwatch4 = Stopwatch.StartNew();
			int num5 = 0;
			while ((float)num5 < 10000000f)
			{
				num += (float)intVec.LengthManhattan;
				num5++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthManhattan: " + stopwatch4.ElapsedTicks.ToString());
			Stopwatch stopwatch5 = Stopwatch.StartNew();
			int num6 = 0;
			while ((float)num6 < 10000000f)
			{
				num += intVec.LengthHorizontal;
				num6++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontal: " + stopwatch5.ElapsedTicks.ToString());
			Stopwatch stopwatch6 = Stopwatch.StartNew();
			int num7 = 0;
			while ((float)num7 < 10000000f)
			{
				num += (float)intVec.LengthHorizontalSquared;
				num7++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontalSquared: " + stopwatch6.ElapsedTicks.ToString());
			stringBuilder.AppendLine("total: " + num.ToString());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001EFAB File Offset: 0x0001D1AB
		public static float Min(float a, float b, float c)
		{
			if (a < b)
			{
				if (a < c)
				{
					return a;
				}
				return c;
			}
			else
			{
				if (b < c)
				{
					return b;
				}
				return c;
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001EFC0 File Offset: 0x0001D1C0
		public static int Max(int a, int b, int c)
		{
			if (a > b)
			{
				if (a > c)
				{
					return a;
				}
				return c;
			}
			else
			{
				if (b > c)
				{
					return b;
				}
				return c;
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001EFD5 File Offset: 0x0001D1D5
		public static float SphericalDistance(Vector3 normalizedA, Vector3 normalizedB)
		{
			if (normalizedA == normalizedB)
			{
				return 0f;
			}
			return Mathf.Acos(Vector3.Dot(normalizedA, normalizedB));
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001EFF4 File Offset: 0x0001D1F4
		public static void DHondtDistribution(List<int> candidates, Func<int, float> scoreGetter, int numToDistribute)
		{
			GenMath.tmpScores.Clear();
			GenMath.tmpCalcList.Clear();
			for (int i = 0; i < candidates.Count; i++)
			{
				float item = scoreGetter(i);
				candidates[i] = 0;
				GenMath.tmpScores.Add(item);
				GenMath.tmpCalcList.Add(item);
			}
			for (int j = 0; j < numToDistribute; j++)
			{
				int num = GenMath.tmpCalcList.IndexOf(GenMath.tmpCalcList.Max());
				int index = num;
				int num2 = candidates[index];
				candidates[index] = num2 + 1;
				GenMath.tmpCalcList[num] = GenMath.tmpScores[num] / ((float)candidates[num] + 1f);
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001F0AB File Offset: 0x0001D2AB
		public static int PositiveMod(int x, int m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001F0B4 File Offset: 0x0001D2B4
		public static long PositiveMod(long x, long m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001F0BD File Offset: 0x0001D2BD
		public static float PositiveMod(float x, float m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001F0C6 File Offset: 0x0001D2C6
		public static int PositiveModRemap(long x, int d, int m)
		{
			if (x < 0L)
			{
				x -= (long)(d - 1);
			}
			return (int)((x / (long)d % (long)m + (long)m) % (long)m);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001F0E3 File Offset: 0x0001D2E3
		public static Vector3 BezierCubicEvaluate(float t, GenMath.BezierCubicControls bcc)
		{
			return GenMath.BezierCubicEvaluate(t, bcc.w0, bcc.w1, bcc.w2, bcc.w3);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001F104 File Offset: 0x0001D304
		public static Vector3 BezierCubicEvaluate(float t, Vector3 w0, Vector3 w1, Vector3 w2, Vector3 w3)
		{
			float d = t * t;
			float num = 1f - t;
			float d2 = num * num;
			return w0 * d2 * num + 3f * w1 * d2 * t + 3f * w2 * num * d + w3 * d * t;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001F17C File Offset: 0x0001D37C
		public static float CirclesOverlapArea(float x1, float y1, float r1, float x2, float y2, float r2)
		{
			float num = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
			float num2 = Mathf.Sqrt(num);
			float num3 = r1 * r1;
			float num4 = r2 * r2;
			float num5 = Mathf.Abs(r1 - r2);
			if (num2 >= r1 + r2)
			{
				return 0f;
			}
			if (num2 <= num5 && r1 >= r2)
			{
				return 3.1415927f * num4;
			}
			if (num2 <= num5 && r2 >= r1)
			{
				return 3.1415927f * num3;
			}
			float num6 = Mathf.Acos((num3 - num4 + num) / (2f * r1 * num2)) * 2f;
			float num7 = Mathf.Acos((num4 - num3 + num) / (2f * r2 * num2)) * 2f;
			float num8 = (num7 * num4 - num4 * Mathf.Sin(num7)) * 0.5f;
			float num9 = (num6 * num3 - num3 * Mathf.Sin(num6)) * 0.5f;
			return num8 + num9;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001F24E File Offset: 0x0001D44E
		public static bool AnyIntegerInRange(float min, float max)
		{
			return Mathf.Ceil(min) <= max;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001F25C File Offset: 0x0001D45C
		public static void NormalizeToSum1(ref float a, ref float b, ref float c)
		{
			float num = a + b + c;
			if (num == 0f)
			{
				a = 1f;
				b = 0f;
				c = 0f;
				return;
			}
			a /= num;
			b /= num;
			c /= num;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001F2A2 File Offset: 0x0001D4A2
		public static float InverseLerp(float a, float b, float value)
		{
			if (a != b)
			{
				return Mathf.InverseLerp(a, b, value);
			}
			if (value >= a)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001F2C0 File Offset: 0x0001D4C0
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			if (by1 >= by2 && by1 >= by3)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3)
			{
				return elem2;
			}
			return elem3;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001F2DA File Offset: 0x0001D4DA
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4)
			{
				return elem3;
			}
			return elem4;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001F314 File Offset: 0x0001D514
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5)
			{
				return elem4;
			}
			return elem5;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001F380 File Offset: 0x0001D580
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6)
			{
				return elem5;
			}
			return elem6;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001F420 File Offset: 0x0001D620
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7)
			{
				return elem5;
			}
			if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7)
			{
				return elem6;
			}
			return elem7;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001F504 File Offset: 0x0001D704
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7 && by1 >= by8)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7 && by2 >= by8)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7 && by3 >= by8)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7 && by4 >= by8)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7 && by5 >= by8)
			{
				return elem5;
			}
			if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7 && by6 >= by8)
			{
				return elem6;
			}
			if (by7 >= by1 && by7 >= by2 && by7 >= by3 && by7 >= by4 && by7 >= by5 && by7 >= by6 && by7 >= by8)
			{
				return elem7;
			}
			return elem8;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001F632 File Offset: 0x0001D832
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001F644 File Offset: 0x0001D844
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001F65C File Offset: 0x0001D85C
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001F684 File Offset: 0x0001D884
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001F6B0 File Offset: 0x0001D8B0
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001F6E4 File Offset: 0x0001D8E4
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7, elem8, -by8);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001F71C File Offset: 0x0001D91C
		public static T MaxByRandomIfEqual<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8, float eps = 0.0001f)
		{
			return GenMath.MaxBy<T>(elem1, by1 + Rand.Range(0f, eps), elem2, by2 + Rand.Range(0f, eps), elem3, by3 + Rand.Range(0f, eps), elem4, by4 + Rand.Range(0f, eps), elem5, by5 + Rand.Range(0f, eps), elem6, by6 + Rand.Range(0f, eps), elem7, by7 + Rand.Range(0f, eps), elem8, by8 + Rand.Range(0f, eps));
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001F7B4 File Offset: 0x0001D9B4
		public static float Stddev(IEnumerable<float> data)
		{
			int num = 0;
			double num2 = 0.0;
			double num3 = 0.0;
			foreach (float num4 in data)
			{
				num++;
				num2 += (double)num4;
				num3 += (double)(num4 * num4);
			}
			double num5 = num2 / (double)num;
			return Mathf.Sqrt((float)(num3 / (double)num - num5 * num5));
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001F838 File Offset: 0x0001DA38
		public static float InverseParabola(float x)
		{
			x = Mathf.Clamp01(x);
			return -4f * x * (x - 1f);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001F854 File Offset: 0x0001DA54
		public static float ExponentialWarpInterpolation(float min, float max, float fraction, Vector2 setPoint)
		{
			float p = Mathf.Log(Mathf.InverseLerp(min, max, setPoint.y), setPoint.x);
			float t = Mathf.Pow(fraction, p);
			return Mathf.Lerp(min, max, t);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001F88C File Offset: 0x0001DA8C
		public static float InverseExponentialWarpInterpolation(float min, float max, float value, Vector2 setPoint)
		{
			float num = Mathf.Log(Mathf.InverseLerp(min, max, setPoint.y), setPoint.x);
			return Mathf.Exp(Mathf.Log(Mathf.InverseLerp(min, max, value)) / num);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
		public static float Sign(float val)
		{
			if (val >= 0f)
			{
				return 1f;
			}
			return -1f;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001F8DB File Offset: 0x0001DADB
		public static float Remap(this float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001F8EB File Offset: 0x0001DAEB
		public static float RemapClamped(this float value, float from1, float to1, float from2, float to2)
		{
			return (Mathf.Clamp(value, from1, to1) - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001F902 File Offset: 0x0001DB02
		public static Vector3 Position(this Matrix4x4 matrix)
		{
			return matrix.GetColumn(3);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001F914 File Offset: 0x0001DB14
		public static float MeanAngle(List<float> angles)
		{
			float x = angles.Sum((float a) => Mathf.Cos(a * 3.1415927f / 180f)) / (float)angles.Count;
			return Mathf.Atan2(angles.Sum((float a) => Mathf.Sin(a * 3.1415927f / 180f)) / (float)angles.Count, x) * 180f / 3.1415927f;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001F990 File Offset: 0x0001DB90
		public static float SmoothMin(float a, float b, float k)
		{
			if (k == 0f)
			{
				return Mathf.Min(a, b);
			}
			float num = Mathf.Max(0f, Mathf.Min(1f, (b - a) / k + 0.5f));
			return a * num + b * (1f - num) - k * num * (1f - num);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001F9E8 File Offset: 0x0001DBE8
		public static double SmoothMin(double a, double b, double k)
		{
			if (k == 0.0)
			{
				return Math.Min(a, b);
			}
			double num = Math.Max(0.0, Math.Min(1.0, (b - a) / k + 0.5));
			return a * num + b * (1.0 - num) - k * num * (1.0 - num);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001FA56 File Offset: 0x0001DC56
		public static Vector3 RotateScale(this Vector3 vec, RotationDirection rotation)
		{
			if (rotation == RotationDirection.Clockwise || rotation == RotationDirection.Counterclockwise)
			{
				return new Vector3(vec.z, vec.y, vec.x);
			}
			return vec;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001FA79 File Offset: 0x0001DC79
		public static Vector3 RotateScale(this Vector3 vec, Rot4 orientation)
		{
			return vec.RotateScale((RotationDirection)orientation.AsInt);
		}

		// Token: 0x040001C7 RID: 455
		public const float BigEpsilon = 1E-07f;

		// Token: 0x040001C8 RID: 456
		public const float Sqrt2 = 1.4142135f;

		// Token: 0x040001C9 RID: 457
		private static List<float> tmpElements = new List<float>();

		// Token: 0x040001CA RID: 458
		private static List<Pair<float, float>> tmpPairs = new List<Pair<float, float>>();

		// Token: 0x040001CB RID: 459
		private static List<float> tmpScores = new List<float>();

		// Token: 0x040001CC RID: 460
		private static List<float> tmpCalcList = new List<float>();

		// Token: 0x020024B9 RID: 9401
		public struct BezierCubicControls
		{
			// Token: 0x0400964D RID: 38477
			public Vector3 w0;

			// Token: 0x0400964E RID: 38478
			public Vector3 w1;

			// Token: 0x0400964F RID: 38479
			public Vector3 w2;

			// Token: 0x04009650 RID: 38480
			public Vector3 w3;
		}
	}
}
