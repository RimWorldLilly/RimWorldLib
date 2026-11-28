using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001140 RID: 4416
	public class GameCondition_DroughtInitial : GameCondition_Drought
	{
		// Token: 0x06006EE9 RID: 28393 RVA: 0x0026DC78 File Offset: 0x0026BE78
		public override void GameConditionTick()
		{
			if (base.HandleEndDueToRain())
			{
				return;
			}
			if (Find.TickManager.TicksGame > this.startTick + 240000)
			{
				IncidentDef drought = IncidentDefOf.Drought;
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(drought.category, base.SingleMap);
				this.End();
				drought.Worker.TryExecute(parms);
			}
		}

		// Token: 0x040046EF RID: 18159
		private const int DurationUntilFullDrought = 240000;
	}
}
