using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Utility
{
	// Token: 0x02000785 RID: 1925
	public static class EnumUtility
	{
		// Token: 0x06003C9E RID: 15518 RVA: 0x0017E3F8 File Offset: 0x0017C5F8
		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x0017E40E File Offset: 0x0017C60E
		public static IEnumerable<T> GetValuesReverse<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>().Reverse<T>();
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x0017E429 File Offset: 0x0017C629
		public static IEnumerable<Enum> GetBitFlags(this Enum input)
		{
			EnumUtility.<GetBitFlags>d__2 <GetBitFlags>d__ = new EnumUtility.<GetBitFlags>d__2(-2);
			<GetBitFlags>d__.<>3__input = input;
			return <GetBitFlags>d__;
		}
	}
}
