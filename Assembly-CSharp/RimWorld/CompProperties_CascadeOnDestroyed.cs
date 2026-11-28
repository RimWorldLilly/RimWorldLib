using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001846 RID: 6214
	public class CompProperties_CascadeOnDestroyed : CompProperties
	{
		// Token: 0x06009B97 RID: 39831 RVA: 0x003815ED File Offset: 0x0037F7ED
		public CompProperties_CascadeOnDestroyed()
		{
			this.compClass = typeof(CompCascadeOnDestroyed);
		}

		// Token: 0x04005C84 RID: 23684
		public IntRange cascadeCountRange = new IntRange(4, 8);

		// Token: 0x04005C85 RID: 23685
		public List<ThingDef> cascadeThingDefs;
	}
}
