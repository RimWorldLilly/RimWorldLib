using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001134 RID: 4404
	public class GameCondition_TemperatureOffset : GameCondition
	{
		// Token: 0x06006E9B RID: 28315 RVA: 0x0026CCF2 File Offset: 0x0026AEF2
		public override void Init()
		{
			base.Init();
			this.tempOffset = this.def.temperatureOffset;
		}

		// Token: 0x06006E9C RID: 28316 RVA: 0x0026CD0B File Offset: 0x0026AF0B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
		}

		// Token: 0x06006E9D RID: 28317 RVA: 0x0026CD29 File Offset: 0x0026AF29
		public override float TemperatureOffset()
		{
			return this.tempOffset;
		}

		// Token: 0x040046CB RID: 18123
		public float tempOffset;
	}
}
