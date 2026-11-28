using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1D RID: 3613
	public class ThoughtWorker_Hot : ThoughtWorker
	{
		// Token: 0x06006126 RID: 24870 RVA: 0x00231E68 File Offset: 0x00230068
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMax, true, 10);
			float num = p.AmbientTemperature - statValue;
			if (num <= 0f)
			{
				return ThoughtState.Inactive;
			}
			int num2;
			if (num < 10f)
			{
				num2 = 0;
			}
			else if (num < 20f)
			{
				num2 = 1;
			}
			else if (num < 30f)
			{
				num2 = 2;
			}
			else
			{
				num2 = 3;
			}
			if (ModsConfig.IdeologyActive)
			{
				Ideo ideo = p.Ideo;
				if (ideo != null && ideo.HasPrecept(PreceptDefOf.Temperature_Tough))
				{
					num2 -= 2;
				}
			}
			if (num2 >= 0)
			{
				return ThoughtState.ActiveAtStage(num2);
			}
			return ThoughtState.Inactive;
		}
	}
}
