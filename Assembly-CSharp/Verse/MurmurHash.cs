using System;

namespace Verse
{
	// Token: 0x020000A6 RID: 166
	public static class MurmurHash
	{
		// Token: 0x060006F5 RID: 1781 RVA: 0x00027D6C File Offset: 0x00025F6C
		public static int GetInt(uint seed, uint input)
		{
			uint num = input * 3432918353U;
			num = (num << 15 | num >> 17);
			num *= 461845907U;
			uint num2 = seed ^ num;
			num2 = (num2 << 13 | num2 >> 19);
			num2 = num2 * 5U + 3864292196U;
			num2 ^= 2834544218U;
			num2 ^= num2 >> 16;
			num2 *= 2246822507U;
			num2 ^= num2 >> 13;
			num2 *= 3266489909U;
			return (int)(num2 ^ num2 >> 16);
		}

		// Token: 0x0400026B RID: 619
		private const uint Const1 = 3432918353U;

		// Token: 0x0400026C RID: 620
		private const uint Const2 = 461845907U;

		// Token: 0x0400026D RID: 621
		private const uint Const3 = 3864292196U;

		// Token: 0x0400026E RID: 622
		private const uint Const4Mix = 2246822507U;

		// Token: 0x0400026F RID: 623
		private const uint Const5Mix = 3266489909U;

		// Token: 0x04000270 RID: 624
		private const uint Const6StreamPosition = 2834544218U;
	}
}
