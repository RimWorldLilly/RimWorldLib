using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C1 RID: 4289
	public static class ExtraFactionTypeExt
	{
		// Token: 0x06006B39 RID: 27449 RVA: 0x0025A1AA File Offset: 0x002583AA
		public static string GetLabel(this ExtraFactionType factionType)
		{
			return ("ExtraFactionType_" + factionType.ToString()).Translate();
		}
	}
}
