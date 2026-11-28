using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001181 RID: 4481
	public class IncidentWorker_Infestation : IncidentWorker
	{
		// Token: 0x06007064 RID: 28772 RVA: 0x00275900 File Offset: 0x00273B00
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && Faction.OfInsects != null && HiveUtility.TotalSpawnedHivesCount(map, false) < 30 && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x06007065 RID: 28773 RVA: 0x00275940 File Offset: 0x00273B40
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			parms.points *= IncidentWorker_Infestation.PointsFactorCurve.Evaluate(parms.points);
			Thing t = InfestationUtility.SpawnTunnels(Mathf.Max(GenMath.RoundRandom(parms.points / 220f), 1), map, false, parms.infestationLocOverride != null, null, parms.infestationLocOverride, null);
			base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}

		// Token: 0x040047AC RID: 18348
		public const float HivePoints = 220f;

		// Token: 0x040047AD RID: 18349
		public static readonly SimpleCurve PointsFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.7f),
				true
			},
			{
				new CurvePoint(5000f, 0.45f),
				true
			}
		};
	}
}
