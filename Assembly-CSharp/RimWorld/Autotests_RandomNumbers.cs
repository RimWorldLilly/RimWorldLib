using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E2E RID: 7726
	public static class Autotests_RandomNumbers
	{
		// Token: 0x0600C8C0 RID: 51392 RVA: 0x0049B12F File Offset: 0x0049932F
		public static void Run()
		{
			Log.Message("Running random numbers tests.");
			Autotests_RandomNumbers.CheckSimpleFloats();
			Autotests_RandomNumbers.CheckIntsRange();
			Autotests_RandomNumbers.CheckIntsDistribution();
			Autotests_RandomNumbers.CheckSeed();
			Log.Message("Finished.");
		}

		// Token: 0x0600C8C1 RID: 51393 RVA: 0x0049B15C File Offset: 0x0049935C
		private static void CheckSimpleFloats()
		{
			List<float> list = Autotests_RandomNumbers.RandomFloats(500).ToList<float>();
			if (list.Any((float x) => x < 0f || x > 1f))
			{
				Log.Error("Float out of range.");
			}
			if (list.Any((float x) => x < 0.1f))
			{
				if (list.Any((float x) => (double)x > 0.5 && (double)x < 0.6))
				{
					if (list.Any((float x) => (double)x > 0.9))
					{
						goto IL_C0;
					}
				}
			}
			Log.Warning("Possibly uneven distribution.");
			IL_C0:
			list = Autotests_RandomNumbers.RandomFloats(1300000).ToList<float>();
			int num = list.Count((float x) => (double)x < 0.1);
			Log.Message("< 0.1 count (should be ~10%): " + ((float)num / (float)list.Count<float>() * 100f).ToString() + "%");
			num = list.Count((float x) => (double)x < 0.0001);
			Log.Message("< 0.0001 count (should be ~0.01%): " + ((float)num / (float)list.Count<float>() * 100f).ToString() + "%");
		}

		// Token: 0x0600C8C2 RID: 51394 RVA: 0x0049B2DD File Offset: 0x004994DD
		private static IEnumerable<float> RandomFloats(int count)
		{
			Autotests_RandomNumbers.<RandomFloats>d__2 <RandomFloats>d__ = new Autotests_RandomNumbers.<RandomFloats>d__2(-2);
			<RandomFloats>d__.<>3__count = count;
			return <RandomFloats>d__;
		}

		// Token: 0x0600C8C3 RID: 51395 RVA: 0x0049B2F0 File Offset: 0x004994F0
		private static void CheckIntsRange()
		{
			int num = -7;
			int num2 = 4;
			int num3 = 0;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (;;)
			{
				bool flag = true;
				for (int i = num; i <= num2; i++)
				{
					if (!dictionary.ContainsKey(i))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					goto IL_9B;
				}
				num3++;
				if (num3 == 200000)
				{
					break;
				}
				int num4 = Rand.RangeInclusive(num, num2);
				if (num4 < num || num4 > num2)
				{
					Log.Error("Value out of range.");
				}
				if (dictionary.ContainsKey(num4))
				{
					Dictionary<int, int> dictionary2 = dictionary;
					int key = num4;
					int num5 = dictionary2[key];
					dictionary2[key] = num5 + 1;
				}
				else
				{
					dictionary.Add(num4, 1);
				}
			}
			Log.Error("Failed to find all numbers in a range.");
			return;
			IL_9B:
			Log.Message(string.Concat(new string[]
			{
				"Values between ",
				num.ToString(),
				" and ",
				num2.ToString(),
				" (value: number of occurrences):"
			}));
			for (int j = num; j <= num2; j++)
			{
				Log.Message(j.ToString() + ": " + dictionary[j].ToString());
			}
		}

		// Token: 0x0600C8C4 RID: 51396 RVA: 0x0049B40C File Offset: 0x0049960C
		private static void CheckIntsDistribution()
		{
			List<int> list = new List<int>();
			for (int j = 0; j < 1000000; j++)
			{
				int num = Rand.RangeInclusive(-2, 1);
				list.Add(num + 2);
			}
			Log.Message("Ints distribution (should be even):");
			int i;
			int i2;
			for (i = 0; i < 4; i = i2 + 1)
			{
				Log.Message(i.ToString() + ": " + ((float)list.Count((int x) => x == i) / (float)list.Count<int>() * 100f).ToString() + "%");
				i2 = i;
			}
		}

		// Token: 0x0600C8C5 RID: 51397 RVA: 0x0049B4C0 File Offset: 0x004996C0
		private static void CheckSeed()
		{
			int seed = Rand.Seed = 10;
			int @int = Rand.Int;
			int int2 = Rand.Int;
			Rand.Seed = seed;
			int int3 = Rand.Int;
			int int4 = Rand.Int;
			if (@int != int3 || int2 != int4)
			{
				Log.Error("Same seed, different values.");
			}
			Autotests_RandomNumbers.TestPushSeed(15, 20);
			Autotests_RandomNumbers.TestPushSeed(-2147483645, 20);
			Autotests_RandomNumbers.TestPushSeed(6, int.MaxValue);
			Autotests_RandomNumbers.TestPushSeed(-2147483645, 2147483642);
			Autotests_RandomNumbers.TestPushSeed(-1947483645, 1147483642);
			Autotests_RandomNumbers.TestPushSeed(455, 648023);
		}

		// Token: 0x0600C8C6 RID: 51398 RVA: 0x0049B554 File Offset: 0x00499754
		private static void TestPushSeed(int seed1, int seed2)
		{
			Rand.Seed = seed1;
			int @int = Rand.Int;
			int int2 = Rand.Int;
			Rand.PushState();
			Rand.Seed = seed2;
			int int3 = Rand.Int;
			Rand.PopState();
			Rand.Seed = seed1;
			int int4 = Rand.Int;
			Rand.PushState();
			Rand.Seed = seed2;
			int int5 = Rand.Int;
			Rand.PopState();
			int int6 = Rand.Int;
			if (@int != int4 || int2 != int6 || int3 != int5)
			{
				Log.Error("PushSeed broken.");
			}
		}
	}
}
