using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A4E RID: 6734
	public static class InstallationDesignatorDatabase
	{
		// Token: 0x0600A8BB RID: 43195 RVA: 0x003BEA94 File Offset: 0x003BCC94
		public static Designator_Install DesignatorFor(ThingDef artDef)
		{
			Designator_Install designator_Install;
			if (InstallationDesignatorDatabase.designators.TryGetValue(artDef, out designator_Install))
			{
				return designator_Install;
			}
			designator_Install = InstallationDesignatorDatabase.NewDesignatorFor(artDef);
			InstallationDesignatorDatabase.designators.Add(artDef, designator_Install);
			return designator_Install;
		}

		// Token: 0x0600A8BC RID: 43196 RVA: 0x003BEAC8 File Offset: 0x003BCCC8
		private static Designator_Install NewDesignatorFor(ThingDef artDef)
		{
			Designator_Install designator_Install;
			if (artDef.thingClass == typeof(MinifiedTree))
			{
				designator_Install = new Designator_Replant();
			}
			else
			{
				designator_Install = new Designator_Install();
			}
			designator_Install.hotKey = KeyBindingDefOf.Misc1;
			return designator_Install;
		}

		// Token: 0x04006308 RID: 25352
		private static Dictionary<ThingDef, Designator_Install> designators = new Dictionary<ThingDef, Designator_Install>();
	}
}
