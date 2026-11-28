using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000428 RID: 1064
	public class HediffCompProperties_Disorientation : HediffCompProperties
	{
		// Token: 0x06001F11 RID: 7953 RVA: 0x000BDA11 File Offset: 0x000BBC11
		public HediffCompProperties_Disorientation()
		{
			this.compClass = typeof(HediffComp_Disorientation);
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x000BDA3B File Offset: 0x000BBC3B
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties_Disorientation.<ConfigErrors>d__4 <ConfigErrors>d__ = new HediffCompProperties_Disorientation.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400184B RID: 6219
		public float wanderMtbHours = -1f;

		// Token: 0x0400184C RID: 6220
		public float wanderRadius;

		// Token: 0x0400184D RID: 6221
		public int singleWanderDurationTicks = -1;
	}
}
