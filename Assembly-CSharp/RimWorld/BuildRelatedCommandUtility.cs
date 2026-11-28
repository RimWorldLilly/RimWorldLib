using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E42 RID: 7746
	public static class BuildRelatedCommandUtility
	{
		// Token: 0x0600C8E5 RID: 51429 RVA: 0x0049BB55 File Offset: 0x00499D55
		public static IEnumerable<Command> RelatedBuildCommands(BuildableDef building)
		{
			BuildRelatedCommandUtility.<RelatedBuildCommands>d__0 <RelatedBuildCommands>d__ = new BuildRelatedCommandUtility.<RelatedBuildCommands>d__0(-2);
			<RelatedBuildCommands>d__.<>3__building = building;
			return <RelatedBuildCommands>d__;
		}

		// Token: 0x0600C8E6 RID: 51430 RVA: 0x0049BB68 File Offset: 0x00499D68
		[CompilerGenerated]
		internal static bool <RelatedBuildCommands>g__IdeoHasBuilding|0_0(Ideo ideo, ThingDef td)
		{
			return ideo.HasPreceptForBuilding(td) || ideo.PreceptsListForReading.Any(delegate(Precept p)
			{
				Precept_RitualSeat precept_RitualSeat = p as Precept_RitualSeat;
				return precept_RitualSeat != null && precept_RitualSeat.ThingDef == td;
			});
		}
	}
}
