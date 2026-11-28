using System;

namespace Verse
{
	// Token: 0x02000275 RID: 629
	public static class PsychGlowUtility
	{
		// Token: 0x06001246 RID: 4678 RVA: 0x00068AA0 File Offset: 0x00066CA0
		public static string GetLabel(this PsychGlow gl)
		{
			switch (gl)
			{
			case PsychGlow.Dark:
				return "Dark".Translate();
			case PsychGlow.Lit:
				return "Lit".Translate();
			case PsychGlow.Overlit:
				return "LitBrightly".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}
