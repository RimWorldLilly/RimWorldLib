using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015DC RID: 5596
	public class Thought_MemoryObservation : Thought_Memory
	{
		// Token: 0x17001810 RID: 6160
		// (set) Token: 0x06008B10 RID: 35600 RVA: 0x00326A2E File Offset: 0x00324C2E
		public virtual Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
			}
		}

		// Token: 0x06008B11 RID: 35601 RVA: 0x00326A3C File Offset: 0x00324C3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetThingID, "targetThingID", 0, false);
		}

		// Token: 0x06008B12 RID: 35602 RVA: 0x00326A58 File Offset: 0x00324C58
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			Thought_MemoryObservation thought_MemoryObservation = null;
			List<Thought_Memory> memories = thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				Thought_MemoryObservation thought_MemoryObservation2 = memories[i] as Thought_MemoryObservation;
				if (thought_MemoryObservation2 != null && thought_MemoryObservation2.def == this.def && thought_MemoryObservation2.targetThingID == this.targetThingID && (thought_MemoryObservation == null || thought_MemoryObservation2.age > thought_MemoryObservation.age))
				{
					thought_MemoryObservation = thought_MemoryObservation2;
				}
			}
			if (thought_MemoryObservation != null)
			{
				showBubble = (thought_MemoryObservation.age > thought_MemoryObservation.DurationTicks / 2);
				thought_MemoryObservation.Renew();
				return true;
			}
			showBubble = true;
			return false;
		}

		// Token: 0x0400558A RID: 21898
		protected int targetThingID;
	}
}
