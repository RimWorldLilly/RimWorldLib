using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1C RID: 3612
	public class ThoughtWorker_Cold : ThoughtWorker
	{
		// Token: 0x06006124 RID: 24868 RVA: 0x00231DD0 File Offset: 0x0022FFD0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMin, true, 10);
			float ambientTemperature = p.AmbientTemperature;
			float num = statValue - ambientTemperature;
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
