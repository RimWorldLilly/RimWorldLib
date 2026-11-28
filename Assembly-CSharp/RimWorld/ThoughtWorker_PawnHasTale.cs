using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E16 RID: 3606
	public class ThoughtWorker_PawnHasTale : ThoughtWorker
	{
		// Token: 0x06006117 RID: 24855 RVA: 0x00231B0E File Offset: 0x0022FD0E
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (Find.TaleManager.GetLatestTale(this.def.taleDef, p) == null)
			{
				return false;
			}
			return true;
		}
	}
}
