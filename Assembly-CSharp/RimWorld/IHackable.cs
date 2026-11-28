using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B1 RID: 6321
	public interface IHackable
	{
		// Token: 0x06009E59 RID: 40537
		void OnLockedOut(Pawn pawn = null);

		// Token: 0x06009E5A RID: 40538
		void OnHacked(Pawn pawn = null);
	}
}
