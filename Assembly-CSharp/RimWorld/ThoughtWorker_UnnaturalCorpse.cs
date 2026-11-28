using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8E RID: 3726
	public class ThoughtWorker_UnnaturalCorpse : ThoughtWorker
	{
		// Token: 0x06006267 RID: 25191 RVA: 0x00235D88 File Offset: 0x00233F88
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!Find.Anomaly.TryGetUnnaturalCorpseTrackerForHaunted(p, out unnaturalCorpseTracker))
			{
				return false;
			}
			return ThoughtState.ActiveDefault;
		}
	}
}
