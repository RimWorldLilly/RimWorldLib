using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119C RID: 4508
	public class IncidentWorker_SightstealerArrival : IncidentWorker
	{
		// Token: 0x060070CC RID: 28876 RVA: 0x00277928 File Offset: 0x00275B28
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Slate slate = new Slate();
			Map map = QuestGen_Get.GetMap(false, null, false);
			slate.Set<Map>("map", map, false);
			slate.Set<float>("points", parms.points * IncidentWorker_SightstealerArrival.SightstealerPointsFactorCurve.Evaluate(parms.points), false);
			if (!QuestScriptDefOf.SightstealerArrival.CanRun(slate, map))
			{
				return false;
			}
			QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.SightstealerArrival, slate);
			return true;
		}

		// Token: 0x040047BD RID: 18365
		public static readonly SimpleCurve SightstealerPointsFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(4000f, 1f),
				true
			},
			{
				new CurvePoint(10000f, 0.6f),
				true
			}
		};
	}
}
