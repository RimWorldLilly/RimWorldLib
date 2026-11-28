using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E41 RID: 7745
	public static class BuildFacilityCommandUtility
	{
		// Token: 0x0600C8E4 RID: 51428 RVA: 0x0049BB45 File Offset: 0x00499D45
		public static IEnumerable<Command> BuildFacilityCommands(BuildableDef building)
		{
			BuildFacilityCommandUtility.<BuildFacilityCommands>d__0 <BuildFacilityCommands>d__ = new BuildFacilityCommandUtility.<BuildFacilityCommands>d__0(-2);
			<BuildFacilityCommands>d__.<>3__building = building;
			return <BuildFacilityCommands>d__;
		}
	}
}
