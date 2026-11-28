using System;

namespace RimWorld
{
	// Token: 0x02001A06 RID: 6662
	public class CompProperties_Targetable_SingleAnimal : CompProperties_Targetable
	{
		// Token: 0x0600A7C0 RID: 42944 RVA: 0x003BB816 File Offset: 0x003B9A16
		public CompProperties_Targetable_SingleAnimal()
		{
			this.compClass = typeof(CompTargetable_SingleAnimal);
		}

		// Token: 0x040062B2 RID: 25266
		public bool allowWildMan = true;
	}
}
