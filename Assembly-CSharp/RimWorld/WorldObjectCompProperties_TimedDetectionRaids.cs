using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F21 RID: 3873
	public class WorldObjectCompProperties_TimedDetectionRaids : WorldObjectCompProperties
	{
		// Token: 0x06006448 RID: 25672 RVA: 0x0023D980 File Offset: 0x0023BB80
		public WorldObjectCompProperties_TimedDetectionRaids()
		{
			this.compClass = typeof(TimedDetectionRaids);
		}

		// Token: 0x06006449 RID: 25673 RVA: 0x0023D998 File Offset: 0x0023BB98
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties_TimedDetectionRaids.<ConfigErrors>d__1 <ConfigErrors>d__ = new WorldObjectCompProperties_TimedDetectionRaids.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}
	}
}
