using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020006A5 RID: 1701
	public static class LookTargetsUtility
	{
		// Token: 0x060035E0 RID: 13792 RVA: 0x0015C21E File Offset: 0x0015A41E
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x0015C22B File Offset: 0x0015A42B
		public static GlobalTargetInfo TryGetPrimaryTarget(this LookTargets lookTargets)
		{
			if (lookTargets == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			return lookTargets.PrimaryTarget;
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x0015C23C File Offset: 0x0015A43C
		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets == null)
			{
				return;
			}
			lookTargets.Highlight(arrow, colonistBar, circleOverlay);
		}
	}
}
