using System;

namespace RimWorld
{
	// Token: 0x02001138 RID: 4408
	public class GameCondition_ColdSnap : GameCondition
	{
		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x06006EAC RID: 28332 RVA: 0x0026D077 File Offset: 0x0026B277
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x06006EAD RID: 28333 RVA: 0x0026D07E File Offset: 0x0026B27E
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, -20f);
		}

		// Token: 0x040046CE RID: 18126
		private const float MaxTempOffset = -20f;
	}
}
