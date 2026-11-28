using System;

namespace RimWorld
{
	// Token: 0x02001186 RID: 4486
	public class IncidentWorker_MakeInitialDrought : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06007072 RID: 28786 RVA: 0x00275D34 File Offset: 0x00273F34
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.GetGameConditionDef(parms));
			gameConditionManager.RegisterCondition(cond);
			return true;
		}
	}
}
