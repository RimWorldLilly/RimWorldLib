using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4E RID: 3662
	public class ThoughtWorker_ListeningToHarp : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x060061A0 RID: 24992 RVA: 0x002338F5 File Offset: 0x00231AF5
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harp;
			}
		}
	}
}
