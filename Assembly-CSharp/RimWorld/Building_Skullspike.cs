using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC8 RID: 6856
	public class Building_Skullspike : Building, IObservedThoughtGiver
	{
		// Token: 0x0600AE79 RID: 44665 RVA: 0x003D71C4 File Offset: 0x003D53C4
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			if ((observer.Ideo != null && observer.Ideo.IdeoApprovesOfSlavery()) || !observer.IsSlave)
			{
				return null;
			}
			Thought_MemoryObservation thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedSkullspike);
			thought_MemoryObservation.Target = this;
			return thought_MemoryObservation;
		}

		// Token: 0x0600AE7A RID: 44666 RVA: 0x003D71FB File Offset: 0x003D53FB
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			return null;
		}
	}
}
