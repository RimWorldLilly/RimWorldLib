using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD0 RID: 8144
	public class StatWorker_Terror : StatWorker
	{
		// Token: 0x0600CEB0 RID: 52912 RVA: 0x004C17D8 File Offset: 0x004BF9D8
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsSlave;
		}

		// Token: 0x0600CEB1 RID: 52913 RVA: 0x004C1808 File Offset: 0x004BFA08
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return ((Pawn)req.Thing).GetTerrorLevel();
		}
	}
}
