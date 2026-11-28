using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001314 RID: 4884
	public class TileMutatorWorker_AncientToxVent : TileMutatorWorker_AncientVent
	{
		// Token: 0x06007933 RID: 31027 RVA: 0x002B12AF File Offset: 0x002AF4AF
		public TileMutatorWorker_AncientToxVent(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x170014AA RID: 5290
		// (get) Token: 0x06007934 RID: 31028 RVA: 0x002B12B8 File Offset: 0x002AF4B8
		protected override ThingDef AncientVentDef
		{
			get
			{
				return ThingDefOf.AncientToxVent;
			}
		}
	}
}
