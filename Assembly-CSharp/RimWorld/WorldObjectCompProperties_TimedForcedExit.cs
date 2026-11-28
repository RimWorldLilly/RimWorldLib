using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F22 RID: 3874
	[Obsolete]
	public class WorldObjectCompProperties_TimedForcedExit : WorldObjectCompProperties
	{
		// Token: 0x0600644B RID: 25675 RVA: 0x0023D9B8 File Offset: 0x0023BBB8
		public WorldObjectCompProperties_TimedForcedExit()
		{
			this.compClass = typeof(TimedForcedExit);
		}

		// Token: 0x0600644C RID: 25676 RVA: 0x0023D9D0 File Offset: 0x0023BBD0
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties_TimedForcedExit.<ConfigErrors>d__1 <ConfigErrors>d__ = new WorldObjectCompProperties_TimedForcedExit.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}
	}
}
