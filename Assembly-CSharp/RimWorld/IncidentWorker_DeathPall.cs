using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116E RID: 4462
	public class IncidentWorker_DeathPall : IncidentWorker_MakeGameCondition
	{
		// Token: 0x0600700E RID: 28686 RVA: 0x002738D5 File Offset: 0x00271AD5
		public override GameConditionDef GetGameConditionDef(IncidentParms parms)
		{
			return GameConditionDefOf.DeathPall;
		}
	}
}
