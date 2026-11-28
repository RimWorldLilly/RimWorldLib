using System;

namespace Verse
{
	// Token: 0x020003FF RID: 1023
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x06001DCA RID: 7626 RVA: 0x000B7A29 File Offset: 0x000B5C29
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		// Token: 0x040017D3 RID: 6099
		private float chance = 1f;
	}
}
