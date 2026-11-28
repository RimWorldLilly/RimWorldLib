using System;

namespace RimWorld
{
	// Token: 0x0200114F RID: 4431
	public class GameCondition_SmokeSpewer : GameCondition_VolcanicWinter
	{
		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x06006F40 RID: 28480 RVA: 0x0026F027 File Offset: 0x0026D227
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}
	}
}
