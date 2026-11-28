using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200158A RID: 5514
	public interface IObservedThoughtGiver
	{
		// Token: 0x0600871B RID: 34587
		Thought_Memory GiveObservedThought(Pawn observer);

		// Token: 0x0600871C RID: 34588
		HistoryEventDef GiveObservedHistoryEvent(Pawn observer);
	}
}
