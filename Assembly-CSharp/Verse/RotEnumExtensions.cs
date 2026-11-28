using System;
using System.Collections.Generic;
using Verse.Utility;

namespace Verse
{
	// Token: 0x02000070 RID: 112
	public static class RotEnumExtensions
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x00018A5C File Offset: 0x00016C5C
		public static Rot4 Random(this RotEnum r)
		{
			RotEnumExtensions.options.Clear();
			foreach (Enum @enum in r.GetBitFlags())
			{
				RotEnum rot = (RotEnum)@enum;
				RotEnumExtensions.options.Add(rot);
			}
			if (RotEnumExtensions.options.Count == 0)
			{
				return Rot4.North;
			}
			return RotEnumExtensions.options[Rand.Range(0, RotEnumExtensions.options.Count)];
		}

		// Token: 0x04000197 RID: 407
		private static readonly List<Rot4> options = new List<Rot4>();
	}
}
