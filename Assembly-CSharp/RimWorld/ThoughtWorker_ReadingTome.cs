using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E90 RID: 3728
	public class ThoughtWorker_ReadingTome : ThoughtWorker
	{
		// Token: 0x0600626C RID: 25196 RVA: 0x00235E4C File Offset: 0x0023404C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (p.CurJobDef != JobDefOf.Reading)
			{
				return false;
			}
			return p.CurJob.targetA.Thing.def == ThingDefOf.Tome;
		}
	}
}
