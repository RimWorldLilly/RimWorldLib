using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFD RID: 4093
	public class ThingSetMakerDef : Def
	{
		// Token: 0x0600673B RID: 26427 RVA: 0x00248E8D File Offset: 0x0024708D
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}

		// Token: 0x0600673C RID: 26428 RVA: 0x00248EA0 File Offset: 0x002470A0
		public override IEnumerable<string> ConfigErrors()
		{
			ThingSetMakerDef.<ConfigErrors>d__3 <ConfigErrors>d__ = new ThingSetMakerDef.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040040FF RID: 16639
		public ThingSetMaker root;

		// Token: 0x04004100 RID: 16640
		public ThingSetMakerParams debugParams;
	}
}
