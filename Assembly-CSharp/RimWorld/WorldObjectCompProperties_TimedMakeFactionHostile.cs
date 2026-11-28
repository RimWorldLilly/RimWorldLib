using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F23 RID: 3875
	public class WorldObjectCompProperties_TimedMakeFactionHostile : WorldObjectCompProperties
	{
		// Token: 0x0600644E RID: 25678 RVA: 0x0023D9F0 File Offset: 0x0023BBF0
		public WorldObjectCompProperties_TimedMakeFactionHostile()
		{
			this.compClass = typeof(TimedMakeFactionHostile);
		}

		// Token: 0x0600644F RID: 25679 RVA: 0x0023DA08 File Offset: 0x0023BC08
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties_TimedMakeFactionHostile.<ConfigErrors>d__1 <ConfigErrors>d__ = new WorldObjectCompProperties_TimedMakeFactionHostile.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}
	}
}
