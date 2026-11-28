using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FA RID: 6138
	public static class PsychicDroneLevelUtility
	{
		// Token: 0x06009935 RID: 39221 RVA: 0x00377110 File Offset: 0x00375310
		public static string GetLabel(this PsychicDroneLevel level)
		{
			switch (level)
			{
			case PsychicDroneLevel.None:
				return "PsychicDroneLevel_None".Translate();
			case PsychicDroneLevel.GoodMedium:
				return "PsychicDroneLevel_GoodMedium".Translate();
			case PsychicDroneLevel.BadLow:
				return "PsychicDroneLevel_BadLow".Translate();
			case PsychicDroneLevel.BadMedium:
				return "PsychicDroneLevel_BadMedium".Translate();
			case PsychicDroneLevel.BadHigh:
				return "PsychicDroneLevel_BadHigh".Translate();
			case PsychicDroneLevel.BadExtreme:
				return "PsychicDroneLevel_BadExtreme".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06009936 RID: 39222 RVA: 0x003771A2 File Offset: 0x003753A2
		public static string GetLabelCap(this PsychicDroneLevel level)
		{
			return level.GetLabel().CapitalizeFirst();
		}
	}
}
