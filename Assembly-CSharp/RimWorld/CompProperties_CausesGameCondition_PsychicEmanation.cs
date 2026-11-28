using System;

namespace RimWorld
{
	// Token: 0x020017FB RID: 6139
	public class CompProperties_CausesGameCondition_PsychicEmanation : CompProperties_CausesGameCondition
	{
		// Token: 0x06009937 RID: 39223 RVA: 0x003771AF File Offset: 0x003753AF
		public CompProperties_CausesGameCondition_PsychicEmanation()
		{
			this.compClass = typeof(CompCauseGameCondition_PsychicEmanation);
		}

		// Token: 0x04005B7F RID: 23423
		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x04005B80 RID: 23424
		public int droneLevelIncreaseInterval = int.MinValue;
	}
}
