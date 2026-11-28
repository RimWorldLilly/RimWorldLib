using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C9 RID: 6089
	public class HorrorMonolithAdvancedTransition : MusicTransition
	{
		// Token: 0x0600980A RID: 38922 RVA: 0x0037218F File Offset: 0x0037038F
		public override bool IsTransitionSatisfied()
		{
			return ModsConfig.AnomalyActive && base.IsTransitionSatisfied() && Find.Anomaly.Level > 0 && Find.Anomaly.TicksSinceLastLevelChange <= 3600;
		}

		// Token: 0x04005AF8 RID: 23288
		private const int TransitionTicksDuration = 3600;
	}
}
