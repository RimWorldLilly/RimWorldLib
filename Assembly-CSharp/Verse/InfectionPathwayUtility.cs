using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020006FB RID: 1787
	public static class InfectionPathwayUtility
	{
		// Token: 0x060038CA RID: 14538 RVA: 0x0016C9FB File Offset: 0x0016ABFB
		public static void AddInfectionPathway(InfectionPathwayDef def, Pawn to, Pawn source = null)
		{
			if (to.infectionVectors == null)
			{
				return;
			}
			to.infectionVectors.AddInfectionVector(def, source);
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x0016CA14 File Offset: 0x0016AC14
		public static void AddInfectionPathways(List<InfectionPathwayDef> defs, Pawn to, Pawn source = null)
		{
			if (to.infectionVectors == null)
			{
				return;
			}
			foreach (InfectionPathwayDef def in defs)
			{
				to.infectionVectors.AddInfectionVector(def, source);
			}
		}
	}
}
