using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001705 RID: 5893
	public interface ILordJobRole
	{
		// Token: 0x17001947 RID: 6471
		// (get) Token: 0x06009355 RID: 37717
		int MaxCount { get; }

		// Token: 0x17001948 RID: 6472
		// (get) Token: 0x06009356 RID: 37718
		int MinCount { get; }

		// Token: 0x17001949 RID: 6473
		// (get) Token: 0x06009357 RID: 37719
		TaggedString Label { get; }

		// Token: 0x1700194A RID: 6474
		// (get) Token: 0x06009358 RID: 37720
		TaggedString LabelCap { get; }

		// Token: 0x1700194B RID: 6475
		// (get) Token: 0x06009359 RID: 37721
		TaggedString CategoryLabel { get; }

		// Token: 0x1700194C RID: 6476
		// (get) Token: 0x0600935A RID: 37722
		TaggedString CategoryLabelCap { get; }
	}
}
