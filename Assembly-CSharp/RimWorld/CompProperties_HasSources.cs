using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B7 RID: 6327
	public class CompProperties_HasSources : CompProperties
	{
		// Token: 0x06009EA1 RID: 40609 RVA: 0x0038EDBF File Offset: 0x0038CFBF
		public CompProperties_HasSources()
		{
			this.compClass = typeof(CompHasSources);
		}

		// Token: 0x04005DE1 RID: 24033
		public bool affectLabel = true;

		// Token: 0x04005DE2 RID: 24034
		[MustTranslate]
		public string inspectStringLabel;
	}
}
