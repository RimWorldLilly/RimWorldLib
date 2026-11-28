using System;

namespace Verse
{
	// Token: 0x02000242 RID: 578
	public class AutoSlaughterConfig : IExposable
	{
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x0006184C File Offset: 0x0005FA4C
		public bool AnyLimit
		{
			get
			{
				return this.maxTotal != -1 || this.maxMales != -1 || this.maxFemales != -1 || this.maxMalesYoung != -1 || this.maxFemalesYoung != -1 || !this.allowSlaughterPregnant || !this.allowSlaughterBonded;
			}
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0006189C File Offset: 0x0005FA9C
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.animal, "animal");
			Scribe_Values.Look<int>(ref this.maxTotal, "maxTotal", -1, false);
			Scribe_Values.Look<int>(ref this.maxMales, "maxMales", -1, false);
			Scribe_Values.Look<int>(ref this.maxMalesYoung, "maxMalesYoung", -1, false);
			Scribe_Values.Look<int>(ref this.maxFemales, "maxFemales", -1, false);
			Scribe_Values.Look<int>(ref this.maxFemalesYoung, "maxFemalesYoung", -1, false);
			Scribe_Values.Look<bool>(ref this.allowSlaughterPregnant, "allowSlaughterPregnant", false, false);
			Scribe_Values.Look<bool>(ref this.allowSlaughterBonded, "allowSlaughterBonded", false, false);
		}

		// Token: 0x04000FA2 RID: 4002
		public ThingDef animal;

		// Token: 0x04000FA3 RID: 4003
		public int maxTotal = -1;

		// Token: 0x04000FA4 RID: 4004
		public int maxMales = -1;

		// Token: 0x04000FA5 RID: 4005
		public int maxMalesYoung = -1;

		// Token: 0x04000FA6 RID: 4006
		public int maxFemales = -1;

		// Token: 0x04000FA7 RID: 4007
		public int maxFemalesYoung = -1;

		// Token: 0x04000FA8 RID: 4008
		public bool allowSlaughterPregnant;

		// Token: 0x04000FA9 RID: 4009
		public bool allowSlaughterBonded;

		// Token: 0x04000FAA RID: 4010
		public string uiMaxTotalBuffer;

		// Token: 0x04000FAB RID: 4011
		public string uiMaxMalesBuffer;

		// Token: 0x04000FAC RID: 4012
		public string uiMaxMalesYoungBuffer;

		// Token: 0x04000FAD RID: 4013
		public string uiMaxFemalesBuffer;

		// Token: 0x04000FAE RID: 4014
		public string uiMaxFemalesYoungBuffer;

		// Token: 0x04000FAF RID: 4015
		public const int NoLimit = -1;
	}
}
