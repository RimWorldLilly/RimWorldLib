using System;

namespace RimWorld
{
	// Token: 0x020017F2 RID: 6130
	public class CompProperties_CompAnalyzableUnlockResearch : CompProperties_Analyzable
	{
		// Token: 0x060098FC RID: 39164 RVA: 0x0037651B File Offset: 0x0037471B
		public CompProperties_CompAnalyzableUnlockResearch()
		{
			this.compClass = typeof(CompAnalyzableUnlockResearch);
		}

		// Token: 0x04005B6A RID: 23402
		public int analysisID;

		// Token: 0x04005B6B RID: 23403
		public bool requiresMechanitor;
	}
}
