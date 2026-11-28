using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130B RID: 4875
	public class TileMutatorWorker_AbandonedColonyTribal : TileMutatorWorker_AbandonedColony
	{
		// Token: 0x0600791A RID: 31002 RVA: 0x002B03F8 File Offset: 0x002AE5F8
		public TileMutatorWorker_AbandonedColonyTribal(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600791B RID: 31003 RVA: 0x002B0404 File Offset: 0x002AE604
		protected override Faction GetFaction()
		{
			Faction result;
			if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, false, true, TechLevel.Animal, TechLevel.Neolithic, false, false))
			{
				return result;
			}
			return null;
		}
	}
}
