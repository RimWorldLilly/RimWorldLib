using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4F RID: 3663
	public class ThoughtWorker_ListeningToHarpsichord : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x060061A2 RID: 24994 RVA: 0x00233904 File Offset: 0x00231B04
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harpsichord;
			}
		}
	}
}
