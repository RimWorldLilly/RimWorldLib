using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE6 RID: 3558
	public class ThoughtWorker_Precept_NonImhumanized : ThoughtWorker_Precept
	{
		// Token: 0x060060AE RID: 24750 RVA: 0x002301A4 File Offset: 0x0022E3A4
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return ThoughtState.Inactive;
			}
			return !p.Inhumanized();
		}
	}
}
