using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A55 RID: 6741
	public static class InstallBlueprintUtility
	{
		// Token: 0x0600A911 RID: 43281 RVA: 0x003BFE08 File Offset: 0x003BE008
		public static void CancelBlueprintsFor(Thing th)
		{
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(th);
			if (blueprint_Install != null)
			{
				blueprint_Install.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x0600A912 RID: 43282 RVA: 0x003BFE28 File Offset: 0x003BE028
		public static Blueprint_Install ExistingBlueprintFor(Thing th)
		{
			List<Map> maps = Find.Maps;
			Thing innerIfMinified = th.GetInnerIfMinified();
			for (int i = 0; i < maps.Count; i++)
			{
				Blueprint_Install result;
				if (maps[i].listerBuildings.TryGetReinstallBlueprint(innerIfMinified, out result))
				{
					return result;
				}
			}
			return null;
		}
	}
}
