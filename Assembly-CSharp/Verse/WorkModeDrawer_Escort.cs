using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200018F RID: 399
	public class WorkModeDrawer_Escort : WorkModeDrawer
	{
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x0003D5C2 File Offset: 0x0003B7C2
		protected override bool DrawIconAtTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0003D5C5 File Offset: 0x0003B7C5
		public override GlobalTargetInfo GetTargetForLine(MechanitorControlGroup group)
		{
			return group.Tracker.Pawn;
		}
	}
}
