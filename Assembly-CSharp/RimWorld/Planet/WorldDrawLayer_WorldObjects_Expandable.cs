using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002329 RID: 9001
	public class WorldDrawLayer_WorldObjects_Expandable : WorldDrawLayer_WorldObjects
	{
		// Token: 0x170026FD RID: 9981
		// (get) Token: 0x0600DE4D RID: 56909 RVA: 0x0052A986 File Offset: 0x00528B86
		protected override float Alpha
		{
			get
			{
				if (!Find.PlaySettings.showBasesExpandingIcons || !Find.PlaySettings.showImportantExpandingIcons)
				{
					return 1f;
				}
				return 1f - ExpandableWorldObjectsUtility.RawTransitionPct;
			}
		}

		// Token: 0x0600DE4E RID: 56910 RVA: 0x0052A9B1 File Offset: 0x00528BB1
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
