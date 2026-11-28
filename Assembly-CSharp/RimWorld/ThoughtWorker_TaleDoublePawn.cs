using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E17 RID: 3607
	public class ThoughtWorker_TaleDoublePawn : ThoughtWorker
	{
		// Token: 0x06006119 RID: 24857 RVA: 0x00231B54 File Offset: 0x0022FD54
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			Tale_DoublePawn tale_DoublePawn = Find.TaleManager.GetLatestTale(this.def.taleDef, other) as Tale_DoublePawn;
			if (tale_DoublePawn == null)
			{
				return false;
			}
			return (tale_DoublePawn.firstPawnData.pawn == p && tale_DoublePawn.secondPawnData.pawn == other) || (tale_DoublePawn.firstPawnData.pawn == other && tale_DoublePawn.secondPawnData.pawn == p);
		}
	}
}
