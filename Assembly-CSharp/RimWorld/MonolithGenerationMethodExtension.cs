using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017AC RID: 6060
	public static class MonolithGenerationMethodExtension
	{
		// Token: 0x06009708 RID: 38664 RVA: 0x0036C93E File Offset: 0x0036AB3E
		public static string ToStringHuman(this MonolithGenerationMethod method)
		{
			if (method == MonolithGenerationMethod.Disabled)
			{
				return "MonolithGenerationMethod_Disabled".Translate();
			}
			if (method != MonolithGenerationMethod.NearColonists)
			{
				throw new NotImplementedException();
			}
			return "MonolithGenerationMethod_NearColonists".Translate();
		}
	}
}
