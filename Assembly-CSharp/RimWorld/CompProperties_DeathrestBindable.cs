using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD3 RID: 6867
	public class CompProperties_DeathrestBindable : CompProperties
	{
		// Token: 0x0600AF01 RID: 44801 RVA: 0x003D9F77 File Offset: 0x003D8177
		public CompProperties_DeathrestBindable()
		{
			this.compClass = typeof(CompDeathrestBindable);
		}

		// Token: 0x0600AF02 RID: 44802 RVA: 0x003D9FA8 File Offset: 0x003D81A8
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			CompProperties_DeathrestBindable.<SpecialDisplayStats>d__11 <SpecialDisplayStats>d__ = new CompProperties_DeathrestBindable.<SpecialDisplayStats>d__11(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04006524 RID: 25892
		public int stackLimit;

		// Token: 0x04006525 RID: 25893
		public bool countsTowardsBuildingLimit = true;

		// Token: 0x04006526 RID: 25894
		public bool displayTimeActive = true;

		// Token: 0x04006527 RID: 25895
		public float deathrestEffectivenessFactor = 1f;

		// Token: 0x04006528 RID: 25896
		public bool mustBeLayingInToBind;

		// Token: 0x04006529 RID: 25897
		public float hemogenLimitOffset;

		// Token: 0x0400652A RID: 25898
		public HediffDef hediffToApply;

		// Token: 0x0400652B RID: 25899
		public SoundDef soundWorking;

		// Token: 0x0400652C RID: 25900
		public SoundDef soundStart;

		// Token: 0x0400652D RID: 25901
		public SoundDef soundEnd;
	}
}
