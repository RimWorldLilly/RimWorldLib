using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F18 RID: 3864
	public class WorldObjectCompProperties
	{
		// Token: 0x06006435 RID: 25653 RVA: 0x0023D80F File Offset: 0x0023BA0F
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			WorldObjectCompProperties.<ConfigErrors>d__1 <ConfigErrors>d__ = new WorldObjectCompProperties.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x06006436 RID: 25654 RVA: 0x0023D826 File Offset: 0x0023BA26
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}

		// Token: 0x04003949 RID: 14665
		[TranslationHandle]
		public Type compClass = typeof(WorldObjectComp);
	}
}
