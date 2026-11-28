using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E50 RID: 3664
	public class ThoughtWorker_ListeningToPiano : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x060061A4 RID: 24996 RVA: 0x00233913 File Offset: 0x00231B13
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Piano;
			}
		}
	}
}
