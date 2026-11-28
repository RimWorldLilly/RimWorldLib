using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C7 RID: 6087
	public class CorpseAttackTransition : MusicTransition
	{
		// Token: 0x06009803 RID: 38915 RVA: 0x00372003 File Offset: 0x00370203
		public override bool IsTransitionSatisfied()
		{
			return ModsConfig.AnomalyActive && base.IsTransitionSatisfied() && Find.Anomaly.HasActiveAwokenCorpse();
		}
	}
}
