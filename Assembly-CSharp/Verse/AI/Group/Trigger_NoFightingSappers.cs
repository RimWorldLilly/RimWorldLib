using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000971 RID: 2417
	public class Trigger_NoFightingSappers : Trigger
	{
		// Token: 0x06004989 RID: 18825 RVA: 0x001CA978 File Offset: 0x001C8B78
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn p = lord.ownedPawns[i];
					if (this.IsFightingSapper(p))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x001CA9BF File Offset: 0x001C8BBF
		private bool IsFightingSapper(Pawn p)
		{
			return !p.Downed && !p.InMentalState && (SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p));
		}
	}
}
