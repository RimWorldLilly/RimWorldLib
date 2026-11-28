using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3F RID: 6975
	public class Plant_Psilocap : Plant
	{
		// Token: 0x0600B337 RID: 45879 RVA: 0x003F2884 File Offset: 0x003F0A84
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (base.Destroyed)
			{
				return;
			}
			for (int i = 0; i < base.Map.mapPawns.AllPawnsSpawned.Count; i++)
			{
				Pawn pawn = base.Map.mapPawns.AllPawnsSpawned[i];
				if (pawn.Position.DistanceTo(base.Position) <= 2f)
				{
					RaceProperties raceProps = pawn.RaceProps;
					if ((raceProps.Humanlike || raceProps.Animal) && raceProps.IsFlesh)
					{
						Hediff orAddHediff = pawn.health.GetOrAddHediff(HediffDefOf.PsilocapHigh, null, null, null);
						if (orAddHediff.Severity <= 0.2f)
						{
							orAddHediff.Severity += 0.00033333333f * (float)delta;
						}
					}
				}
			}
		}

		// Token: 0x04006766 RID: 26470
		private const float HediffSeverityIncreasePerSecond = 0.02f;

		// Token: 0x04006767 RID: 26471
		private const float HediffSeverityIncreasePerTick = 0.00033333333f;

		// Token: 0x04006768 RID: 26472
		private const float HediffSeverityMaxFromAmbient = 0.2f;
	}
}
