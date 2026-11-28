using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8F RID: 3727
	public class ThoughtWorker_CubeSculptures : ThoughtWorker
	{
		// Token: 0x06006269 RID: 25193 RVA: 0x00235DC8 File Offset: 0x00233FC8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Hediff_CubeInterest hediff_CubeInterest;
			if (!p.health.hediffSet.TryGetHediff<Hediff_CubeInterest>(out hediff_CubeInterest))
			{
				return false;
			}
			return hediff_CubeInterest.Sculptures > 0;
		}

		// Token: 0x0600626A RID: 25194 RVA: 0x00235E0C File Offset: 0x0023400C
		public override float MoodMultiplier(Pawn p)
		{
			if (!p.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false))
			{
				return 1f;
			}
			return (float)p.health.hediffSet.GetFirstHediff<Hediff_CubeInterest>().Sculptures;
		}
	}
}
