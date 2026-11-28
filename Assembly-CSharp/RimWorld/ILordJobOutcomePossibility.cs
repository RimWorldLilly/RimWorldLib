using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C94 RID: 7316
	public interface ILordJobOutcomePossibility
	{
		// Token: 0x170021E4 RID: 8676
		// (get) Token: 0x0600BAC1 RID: 47809
		TaggedString Label { get; }

		// Token: 0x170021E5 RID: 8677
		// (get) Token: 0x0600BAC2 RID: 47810
		TaggedString ToolTip { get; }

		// Token: 0x0600BAC3 RID: 47811
		float Weight(FloatRange qualityRange);
	}
}
