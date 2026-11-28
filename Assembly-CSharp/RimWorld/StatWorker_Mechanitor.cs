using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC2 RID: 8130
	public class StatWorker_Mechanitor : StatWorker
	{
		// Token: 0x0600CE74 RID: 52852 RVA: 0x004BFE08 File Offset: 0x004BE008
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			if (req.Thing != null)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return MechanitorUtility.IsMechanitor(pawn);
				}
			}
			return false;
		}
	}
}
