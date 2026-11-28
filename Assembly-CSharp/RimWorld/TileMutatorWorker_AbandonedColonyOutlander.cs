using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130A RID: 4874
	public class TileMutatorWorker_AbandonedColonyOutlander : TileMutatorWorker_AbandonedColony
	{
		// Token: 0x06007918 RID: 31000 RVA: 0x002B03C8 File Offset: 0x002AE5C8
		public TileMutatorWorker_AbandonedColonyOutlander(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007919 RID: 31001 RVA: 0x002B03D4 File Offset: 0x002AE5D4
		protected override Faction GetFaction()
		{
			Faction result;
			if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, false, true, TechLevel.Medieval, TechLevel.Spacer, false, false))
			{
				return result;
			}
			return null;
		}
	}
}
