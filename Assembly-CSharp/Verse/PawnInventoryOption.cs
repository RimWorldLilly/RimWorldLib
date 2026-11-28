using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001A1 RID: 417
	public class PawnInventoryOption
	{
		// Token: 0x06000B8D RID: 2957 RVA: 0x0003E108 File Offset: 0x0003C308
		public IEnumerable<Thing> GenerateThings()
		{
			PawnInventoryOption.<GenerateThings>d__6 <GenerateThings>d__ = new PawnInventoryOption.<GenerateThings>d__6(-2);
			<GenerateThings>d__.<>4__this = this;
			return <GenerateThings>d__;
		}

		// Token: 0x04000A9E RID: 2718
		public ThingDef thingDef;

		// Token: 0x04000A9F RID: 2719
		public IntRange countRange = IntRange.One;

		// Token: 0x04000AA0 RID: 2720
		public float choiceChance = 1f;

		// Token: 0x04000AA1 RID: 2721
		public float skipChance;

		// Token: 0x04000AA2 RID: 2722
		public List<PawnInventoryOption> subOptionsTakeAll;

		// Token: 0x04000AA3 RID: 2723
		public List<PawnInventoryOption> subOptionsChooseOne;
	}
}
