using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7B RID: 7291
	public class Designator_Plan_Shrink : Designator_Plan_Remove
	{
		// Token: 0x0600BA39 RID: 47673 RVA: 0x0041A73C File Offset: 0x0041893C
		public Designator_Plan_Shrink()
		{
			this.defaultLabel = "DesignatorPlanShrinkSingular".Translate();
			this.defaultDesc = "DesignatorPlanShrinkDesc".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
