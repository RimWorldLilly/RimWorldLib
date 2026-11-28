using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F19 RID: 3865
	public class WorldObjectCompProperties_Abandon : WorldObjectCompProperties
	{
		// Token: 0x06006438 RID: 25656 RVA: 0x0023D840 File Offset: 0x0023BA40
		public WorldObjectCompProperties_Abandon()
		{
			this.compClass = typeof(AbandonComp);
		}

		// Token: 0x06006439 RID: 25657 RVA: 0x0023D858 File Offset: 0x0023BA58
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties_Abandon.<ConfigErrors>d__1 <ConfigErrors>d__ = new WorldObjectCompProperties_Abandon.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}
	}
}
