using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C6 RID: 5318
	public struct ThoughtToAddToAll
	{
		// Token: 0x06008208 RID: 33288 RVA: 0x002E69D2 File Offset: 0x002E4BD2
		public ThoughtToAddToAll(ThoughtDef thoughtDef, Pawn otherPawn = null)
		{
			if (thoughtDef == null)
			{
				throw new NullReferenceException("Thought def cant be null!");
			}
			this.thoughtDef = thoughtDef;
			this.otherPawn = otherPawn;
		}

		// Token: 0x06008209 RID: 33289 RVA: 0x002E69F0 File Offset: 0x002E4BF0
		public void Add(Pawn to)
		{
			if (to.needs == null || to.needs.mood == null)
			{
				return;
			}
			if (this.otherPawn == null)
			{
				to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, null, null);
				return;
			}
			to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, this.otherPawn, null);
		}

		// Token: 0x04005088 RID: 20616
		public ThoughtDef thoughtDef;

		// Token: 0x04005089 RID: 20617
		public Pawn otherPawn;
	}
}
