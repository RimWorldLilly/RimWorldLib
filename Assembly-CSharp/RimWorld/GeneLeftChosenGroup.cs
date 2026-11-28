using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CDE RID: 7390
	public class GeneLeftChosenGroup
	{
		// Token: 0x0600BDE5 RID: 48613 RVA: 0x004438DB File Offset: 0x00441ADB
		public GeneLeftChosenGroup(GeneDef left)
		{
			this.leftChosen = left;
		}

		// Token: 0x04006CBB RID: 27835
		public GeneDef leftChosen;

		// Token: 0x04006CBC RID: 27836
		public List<GeneDef> overriddenGenes = new List<GeneDef>();
	}
}
