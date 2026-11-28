using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F1B RID: 3867
	public class WorldObjectCompProperties_DefeatAllEnemiesQuest : WorldObjectCompProperties
	{
		// Token: 0x0600643C RID: 25660 RVA: 0x0023D890 File Offset: 0x0023BA90
		public WorldObjectCompProperties_DefeatAllEnemiesQuest()
		{
			this.compClass = typeof(DefeatAllEnemiesQuestComp);
		}

		// Token: 0x0600643D RID: 25661 RVA: 0x0023D8A8 File Offset: 0x0023BAA8
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties_DefeatAllEnemiesQuest.<ConfigErrors>d__1 <ConfigErrors>d__ = new WorldObjectCompProperties_DefeatAllEnemiesQuest.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}
	}
}
