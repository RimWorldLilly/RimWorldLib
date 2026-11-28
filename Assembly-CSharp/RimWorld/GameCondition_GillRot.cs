using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001142 RID: 4418
	public class GameCondition_GillRot : GameCondition
	{
		// Token: 0x06006EF6 RID: 28406 RVA: 0x0026E11C File Offset: 0x0026C31C
		public override void Init()
		{
			base.Init();
			this.fishPopulationOffsetFactorPerDay = this.def.fishPopulationOffsetPerDay;
		}

		// Token: 0x06006EF7 RID: 28407 RVA: 0x0026E135 File Offset: 0x0026C335
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.fishPopulationOffsetFactorPerDay, "fishPopulationOffsetFactorPerDay", 0f, false);
		}

		// Token: 0x040046FC RID: 18172
		public float fishPopulationOffsetFactorPerDay;
	}
}
