using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FDB RID: 8155
	public static class ThreatsGenerator
	{
		// Token: 0x0600CEF1 RID: 52977 RVA: 0x004C3AE2 File Offset: 0x004C1CE2
		public static IEnumerable<FiringIncident> MakeIntervalIncidents(ThreatsGeneratorParams parms, IIncidentTarget target, int startTick)
		{
			ThreatsGenerator.<MakeIntervalIncidents>d__1 <MakeIntervalIncidents>d__ = new ThreatsGenerator.<MakeIntervalIncidents>d__1(-2);
			<MakeIntervalIncidents>d__.<>3__parms = parms;
			<MakeIntervalIncidents>d__.<>3__target = target;
			<MakeIntervalIncidents>d__.<>3__startTick = startTick;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x0600CEF2 RID: 52978 RVA: 0x004C3B00 File Offset: 0x004C1D00
		private static FiringIncident MakeThreat(ThreatsGeneratorParams parms, IIncidentTarget target)
		{
			IncidentParms incParms = ThreatsGenerator.GetIncidentParms(parms, target);
			IncidentDef def;
			if (!(from x in ThreatsGenerator.GetPossibleIncidents(parms.allowedThreats)
			where x.Worker.CanFireNow(incParms)
			select x).TryRandomElementByWeight((IncidentDef x) => x.Worker.BaseChanceThisGame, out def))
			{
				return null;
			}
			return new FiringIncident
			{
				def = def,
				parms = incParms
			};
		}

		// Token: 0x0600CEF3 RID: 52979 RVA: 0x004C3B80 File Offset: 0x004C1D80
		public static bool AnyIncidentPossible(ThreatsGeneratorParams parms, IIncidentTarget target)
		{
			IncidentParms incParms = ThreatsGenerator.GetIncidentParms(parms, target);
			return ThreatsGenerator.GetPossibleIncidents(parms.allowedThreats).Any((IncidentDef x) => x.Worker.BaseChanceThisGame > 0f && x.Worker.CanFireNow(incParms));
		}

		// Token: 0x0600CEF4 RID: 52980 RVA: 0x004C3BBC File Offset: 0x004C1DBC
		private static IncidentParms GetIncidentParms(ThreatsGeneratorParams parms, IIncidentTarget target)
		{
			IncidentParms incidentParms = new IncidentParms
			{
				target = target,
				points = (parms.threatPoints ?? (StorytellerUtility.DefaultThreatPointsNow(target) * parms.currentThreatPointsFactor))
			};
			if (parms.minThreatPoints != null)
			{
				incidentParms.points = Mathf.Max(incidentParms.points, parms.minThreatPoints.Value);
			}
			incidentParms.faction = parms.faction;
			incidentParms.forced = true;
			return incidentParms;
		}

		// Token: 0x0600CEF5 RID: 52981 RVA: 0x004C3C3F File Offset: 0x004C1E3F
		private static IEnumerable<IncidentDef> GetPossibleIncidents(AllowedThreatsGeneratorThreats allowedThreats)
		{
			ThreatsGenerator.<GetPossibleIncidents>d__5 <GetPossibleIncidents>d__ = new ThreatsGenerator.<GetPossibleIncidents>d__5(-2);
			<GetPossibleIncidents>d__.<>3__allowedThreats = allowedThreats;
			return <GetPossibleIncidents>d__;
		}

		// Token: 0x040083AB RID: 33707
		private static readonly SimpleCurve ThreatScaleToCountFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.1f),
				true
			},
			{
				new CurvePoint(0.3f, 0.5f),
				true
			},
			{
				new CurvePoint(0.6f, 0.8f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.55f, 1.1f),
				true
			},
			{
				new CurvePoint(2.2f, 1.2f),
				true
			},
			{
				new CurvePoint(10f, 2f),
				true
			}
		};
	}
}
