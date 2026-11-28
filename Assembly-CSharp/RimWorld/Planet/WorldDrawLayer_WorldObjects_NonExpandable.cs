using System;

namespace RimWorld.Planet
{
	// Token: 0x0200232B RID: 9003
	public class WorldDrawLayer_WorldObjects_NonExpandable : WorldDrawLayer_WorldObjects
	{
		// Token: 0x0600DE5E RID: 56926 RVA: 0x0052B037 File Offset: 0x00529237
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
