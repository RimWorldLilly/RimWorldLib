using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B7E RID: 2942
	public class JoyGiver_Read : JoyGiver
	{
		// Token: 0x06005574 RID: 21876 RVA: 0x001FC6AF File Offset: 0x001FA8AF
		public override bool CanBeGivenTo(Pawn pawn)
		{
			return BookUtility.CanReadNow(pawn) && !PawnUtility.WillSoonHaveBasicNeed(pawn, 0.05f) && base.CanBeGivenTo(pawn);
		}

		// Token: 0x06005575 RID: 21877 RVA: 0x001FC6D4 File Offset: 0x001FA8D4
		public override Job TryGiveJob(Pawn pawn)
		{
			Book t;
			if (BookUtility.TryGetRandomBookToRead(pawn, out t))
			{
				return JobMaker.MakeJob(this.def.jobDef, t);
			}
			return null;
		}
	}
}
