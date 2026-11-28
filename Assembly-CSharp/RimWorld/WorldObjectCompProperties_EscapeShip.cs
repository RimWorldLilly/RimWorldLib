using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F1D RID: 3869
	public class WorldObjectCompProperties_EscapeShip : WorldObjectCompProperties
	{
		// Token: 0x06006440 RID: 25664 RVA: 0x0023D8E0 File Offset: 0x0023BAE0
		public WorldObjectCompProperties_EscapeShip()
		{
			this.compClass = typeof(EscapeShipComp);
		}

		// Token: 0x06006441 RID: 25665 RVA: 0x0023D8F8 File Offset: 0x0023BAF8
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties_EscapeShip.<ConfigErrors>d__1 <ConfigErrors>d__ = new WorldObjectCompProperties_EscapeShip.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}
	}
}
