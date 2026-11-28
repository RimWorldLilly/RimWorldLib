using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001313 RID: 4883
	public class TileMutatorWorker_AncientHeatVent : TileMutatorWorker_AncientVent
	{
		// Token: 0x06007931 RID: 31025 RVA: 0x002B129F File Offset: 0x002AF49F
		public TileMutatorWorker_AncientHeatVent(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x170014A9 RID: 5289
		// (get) Token: 0x06007932 RID: 31026 RVA: 0x002B12A8 File Offset: 0x002AF4A8
		protected override ThingDef AncientVentDef
		{
			get
			{
				return ThingDefOf.AncientHeatVent;
			}
		}
	}
}
