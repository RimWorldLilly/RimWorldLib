using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B9 RID: 6329
	public class CompProperties_HasPawnSources : CompProperties
	{
		// Token: 0x06009EAB RID: 40619 RVA: 0x0038F08C File Offset: 0x0038D28C
		public CompProperties_HasPawnSources()
		{
			this.compClass = typeof(CompHasPawnSources);
		}

		// Token: 0x04005DE6 RID: 24038
		public bool affectLabel = true;
	}
}
