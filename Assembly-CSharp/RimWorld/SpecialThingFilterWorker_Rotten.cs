using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F4F RID: 8015
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		// Token: 0x0600CC72 RID: 52338 RVA: 0x004B5B94 File Offset: 0x004B3D94
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && !compRottable.PropsRot.rotDestroys && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x0600CC73 RID: 52339 RVA: 0x004B5BC4 File Offset: 0x004B3DC4
		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
