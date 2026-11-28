using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F1E RID: 3870
	public class WorldObjectCompProperties_FormCaravan : WorldObjectCompProperties
	{
		// Token: 0x06006443 RID: 25667 RVA: 0x0023D918 File Offset: 0x0023BB18
		public WorldObjectCompProperties_FormCaravan()
		{
			this.compClass = typeof(FormCaravanComp);
		}

		// Token: 0x06006444 RID: 25668 RVA: 0x0023D930 File Offset: 0x0023BB30
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties_FormCaravan.<ConfigErrors>d__1 <ConfigErrors>d__ = new WorldObjectCompProperties_FormCaravan.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}
	}
}
