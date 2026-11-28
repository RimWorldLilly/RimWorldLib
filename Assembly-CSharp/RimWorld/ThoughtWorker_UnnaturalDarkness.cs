using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E73 RID: 3699
	public class ThoughtWorker_UnnaturalDarkness : ThoughtWorker_GameCondition
	{
		// Token: 0x06006209 RID: 25097 RVA: 0x00234B10 File Offset: 0x00232D10
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive || !base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			if (p.MapHeld.weatherManager.curWeather != WeatherDefOf.UnnaturalDarkness_Stage2)
			{
				return ThoughtState.Inactive;
			}
			if (p.Inhumanized())
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
