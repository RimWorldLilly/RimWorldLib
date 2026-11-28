using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001315 RID: 4885
	public class TileMutatorWorker_AncientSmokeVent : TileMutatorWorker_AncientVent
	{
		// Token: 0x06007935 RID: 31029 RVA: 0x002B12BF File Offset: 0x002AF4BF
		public TileMutatorWorker_AncientSmokeVent(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x170014AB RID: 5291
		// (get) Token: 0x06007936 RID: 31030 RVA: 0x002B12C8 File Offset: 0x002AF4C8
		protected override ThingDef AncientVentDef
		{
			get
			{
				return ThingDefOf.AncientSmokeVent;
			}
		}
	}
}
