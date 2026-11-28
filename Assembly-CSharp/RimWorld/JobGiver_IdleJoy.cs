using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B76 RID: 2934
	public class JobGiver_IdleJoy : JobGiver_GetJoy
	{
		// Token: 0x06005550 RID: 21840 RVA: 0x001FB731 File Offset: 0x001F9931
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return null;
			}
			if (Find.TickManager.TicksGame < 60000)
			{
				return null;
			}
			if (JoyUtility.LordPreventsGettingJoy(pawn) || JoyUtility.TimetablePreventsGettingJoy(pawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x040032CB RID: 13003
		private const int GameStartNoIdleJoyTicks = 60000;
	}
}
