using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BEB RID: 3051
	public static class ActivitySuppressionUtility
	{
		// Token: 0x0600576B RID: 22379 RVA: 0x002067AC File Offset: 0x002049AC
		public static bool CanBeSuppressed(Thing thing, bool considerMinActivity = true, bool playerForced = false)
		{
			CompActivity compActivity;
			if (!thing.TryGetComp(out compActivity))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (pawn.Downed)
				{
					return false;
				}
				if (compActivity.Props.requiresHoldingPlatform && !pawn.IsOnHoldingPlatform)
				{
					return false;
				}
			}
			return !compActivity.IsActive && compActivity.CanBeSuppressed && (playerForced || compActivity.suppressionEnabled) && (!considerMinActivity || compActivity.ActivityLevel >= 0.05f);
		}

		// Token: 0x04003332 RID: 13106
		private const float MinActivityToSuppress = 0.05f;
	}
}
