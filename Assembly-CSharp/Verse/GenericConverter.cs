using System;
using System.Linq.Expressions;

namespace Verse
{
	// Token: 0x02000093 RID: 147
	public static class GenericConverter
	{
		// Token: 0x0600067D RID: 1661 RVA: 0x00025D13 File Offset: 0x00023F13
		public static OutType Convert<InType, OutType>(InType value)
		{
			return GenericConverter.StaticGenericCache<InType, OutType>.cachedFunc(value);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00025D20 File Offset: 0x00023F20
		private static Func<InType, OutType> GenerateFunc<InType, OutType>()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(InType));
			return Expression.Lambda<Func<InType, OutType>>(Expression.Convert(parameterExpression, typeof(OutType)), new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x020024D1 RID: 9425
		private static class StaticGenericCache<InType, OutType>
		{
			// Token: 0x040096B5 RID: 38581
			public static Func<InType, OutType> cachedFunc = GenericConverter.GenerateFunc<InType, OutType>();
		}
	}
}
