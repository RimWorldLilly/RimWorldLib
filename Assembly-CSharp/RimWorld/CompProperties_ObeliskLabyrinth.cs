using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001921 RID: 6433
	public class CompProperties_ObeliskLabyrinth : CompProperties_Interactable
	{
		// Token: 0x0600A165 RID: 41317 RVA: 0x0039B056 File Offset: 0x00399256
		public CompProperties_ObeliskLabyrinth()
		{
			this.compClass = typeof(CompObelisk_Labyrinth);
		}

		// Token: 0x04005F5C RID: 24412
		[MustTranslate]
		public string messageActivating;
	}
}
