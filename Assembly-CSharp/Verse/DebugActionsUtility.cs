using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005BE RID: 1470
	public static class DebugActionsUtility
	{
		// Token: 0x06002D21 RID: 11553 RVA: 0x00112D08 File Offset: 0x00110F08
		public static void DustPuffFrom(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.Drawer.Notify_DebugAffected();
			}
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x00112D2A File Offset: 0x00110F2A
		public static IEnumerable<float> PointsOptions(bool extended)
		{
			DebugActionsUtility.<PointsOptions>d__1 <PointsOptions>d__ = new DebugActionsUtility.<PointsOptions>d__1(-2);
			<PointsOptions>d__.<>3__extended = extended;
			return <PointsOptions>d__;
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x00112D3A File Offset: 0x00110F3A
		public static IEnumerable<int> PopulationOptions()
		{
			return new DebugActionsUtility.<PopulationOptions>d__2(-2);
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x00112D43 File Offset: 0x00110F43
		public static IEnumerable<int> RadiusOptions()
		{
			return new DebugActionsUtility.<RadiusOptions>d__3(-2);
		}
	}
}
