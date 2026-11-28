using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F50 RID: 8016
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		// Token: 0x0600CC75 RID: 52341 RVA: 0x004B5BF0 File Offset: 0x004B3DF0
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			if (compRottable == null)
			{
				return t.def.IsIngestible && !t.def.IsDrug;
			}
			return compRottable.Stage == RotStage.Fresh;
		}

		// Token: 0x0600CC76 RID: 52342 RVA: 0x004B5C2E File Offset: 0x004B3E2E
		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || (def.IsIngestible && !def.IsDrug);
		}

		// Token: 0x0600CC77 RID: 52343 RVA: 0x004B5C50 File Offset: 0x004B3E50
		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible && !def.IsDrug);
		}
	}
}
