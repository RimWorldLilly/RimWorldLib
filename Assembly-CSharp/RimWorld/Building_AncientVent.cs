using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A98 RID: 6808
	public class Building_AncientVent : Building
	{
		// Token: 0x0600ABB7 RID: 43959 RVA: 0x003CA0FC File Offset: 0x003C82FC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			CompFleckEmitterLongTerm comp = base.GetComp<CompFleckEmitterLongTerm>();
			if (comp != null)
			{
				GameConditionDef conditionToCause = base.GetComp<CompAncientVent>().Props.conditionToCause;
				bool enabled = map.GameConditionManager.ConditionIsActive(conditionToCause);
				comp.Enabled = enabled;
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}
	}
}
