using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200175D RID: 5981
	public class Landmark : IExposable
	{
		// Token: 0x060094D2 RID: 38098 RVA: 0x003638C0 File Offset: 0x00361AC0
		public Landmark()
		{
		}

		// Token: 0x060094D3 RID: 38099 RVA: 0x003638C8 File Offset: 0x00361AC8
		public Landmark(LandmarkDef def)
		{
			this.def = def;
		}

		// Token: 0x060094D4 RID: 38100 RVA: 0x003638D7 File Offset: 0x00361AD7
		public void ExposeData()
		{
			Scribe_Defs.Look<LandmarkDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<bool>(ref this.isComboLandmark, "isComboLandmark", false, false);
		}

		// Token: 0x040059AA RID: 22954
		public LandmarkDef def;

		// Token: 0x040059AB RID: 22955
		public string name;

		// Token: 0x040059AC RID: 22956
		public bool isComboLandmark;
	}
}
