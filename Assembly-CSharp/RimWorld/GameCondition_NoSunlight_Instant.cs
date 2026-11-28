using System;

namespace RimWorld
{
	// Token: 0x0200114A RID: 4426
	public class GameCondition_NoSunlight_Instant : GameCondition_NoSunlight
	{
		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x06006F22 RID: 28450 RVA: 0x0026E8AC File Offset: 0x0026CAAC
		public override int TransitionTicks
		{
			get
			{
				return 0;
			}
		}
	}
}
