using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B60 RID: 7008
	public class ThingSetMaker_Conditional_ResearchFinished : ThingSetMaker_Conditional
	{
		// Token: 0x0600B3E9 RID: 46057 RVA: 0x003F697F File Offset: 0x003F4B7F
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return this.researchProject.IsFinished;
		}

		// Token: 0x040067BE RID: 26558
		public ResearchProjectDef researchProject;
	}
}
