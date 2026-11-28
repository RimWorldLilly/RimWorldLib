using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001143 RID: 4419
	public class GameCondition_GrayPall : GameCondition_ForceWeather
	{
		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x06006EF9 RID: 28409 RVA: 0x0026E15B File Offset: 0x0026C35B
		public override int TransitionTicks
		{
			get
			{
				return 180;
			}
		}

		// Token: 0x06006EFA RID: 28410 RVA: 0x0026E162 File Offset: 0x0026C362
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Grey pall"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x06006EFB RID: 28411 RVA: 0x0026E17D File Offset: 0x0026C37D
		public override void End()
		{
			base.End();
			base.SingleMap.weatherDecider.StartNextWeather();
		}
	}
}
