using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B93 RID: 2963
	public class LearningGiver_Reading : LearningGiver
	{
		// Token: 0x060055E5 RID: 21989 RVA: 0x001FF654 File Offset: 0x001FD854
		public override bool CanDo(Pawn pawn)
		{
			Book book;
			return base.CanDo(pawn) && BookUtility.CanReadNow(pawn) && !PawnUtility.WillSoonHaveBasicNeed(pawn, 0.05f) && BookUtility.TryGetRandomBookToRead(pawn, out book);
		}

		// Token: 0x060055E6 RID: 21990 RVA: 0x001FF68C File Offset: 0x001FD88C
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
