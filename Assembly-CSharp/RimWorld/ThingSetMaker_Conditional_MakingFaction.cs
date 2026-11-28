using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B5E RID: 7006
	public class ThingSetMaker_Conditional_MakingFaction : ThingSetMaker_Conditional
	{
		// Token: 0x0600B3E5 RID: 46053 RVA: 0x003F68C8 File Offset: 0x003F4AC8
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return (!this.requireNonNull || parms.makingFaction != null) && (this.makingFaction == null || (parms.makingFaction != null && parms.makingFaction.def == this.makingFaction)) && (this.makingFactionCategories.NullOrEmpty<string>() || (parms.makingFaction != null && this.makingFactionCategories.Contains(parms.makingFaction.def.categoryTag)));
		}

		// Token: 0x040067BA RID: 26554
		public FactionDef makingFaction;

		// Token: 0x040067BB RID: 26555
		public List<string> makingFactionCategories;

		// Token: 0x040067BC RID: 26556
		public bool requireNonNull;
	}
}
