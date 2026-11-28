using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200160F RID: 5647
	public class GoodwillSituationWorker_SameIdeo : GoodwillSituationWorker
	{
		// Token: 0x06008CBA RID: 36026 RVA: 0x00332262 File Offset: 0x00330462
		public override string GetPostProcessedLabel(Faction other)
		{
			return this.def.label.Formatted(Faction.OfPlayer.ideos.PrimaryIdeo);
		}

		// Token: 0x06008CBB RID: 36027 RVA: 0x00332290 File Offset: 0x00330490
		public override int GetNaturalGoodwillOffset(Faction other)
		{
			if (Find.IdeoManager.classicMode)
			{
				return 0;
			}
			Ideo primaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
			if (primaryIdeo == null || primaryIdeo != other.ideos.PrimaryIdeo)
			{
				return 0;
			}
			return this.def.naturalGoodwillOffset;
		}
	}
}
