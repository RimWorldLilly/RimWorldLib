using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000443 RID: 1091
	public class HediffCompProperties_GiveRandomSituationalThought : HediffCompProperties
	{
		// Token: 0x06001F58 RID: 8024 RVA: 0x000BEC00 File Offset: 0x000BCE00
		public HediffCompProperties_GiveRandomSituationalThought()
		{
			this.compClass = typeof(HediffComp_GiveRandomSituationalThought);
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x000BEC18 File Offset: 0x000BCE18
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties_GiveRandomSituationalThought.<ConfigErrors>d__2 <ConfigErrors>d__ = new HediffCompProperties_GiveRandomSituationalThought.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04001879 RID: 6265
		public List<ThoughtDef> thoughtDefs;
	}
}
