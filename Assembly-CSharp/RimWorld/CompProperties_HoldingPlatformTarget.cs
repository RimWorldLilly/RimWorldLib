using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C0 RID: 6336
	public class CompProperties_HoldingPlatformTarget : CompProperties
	{
		// Token: 0x06009ECC RID: 40652 RVA: 0x0038FB0D File Offset: 0x0038DD0D
		public CompProperties_HoldingPlatformTarget()
		{
			this.compClass = typeof(CompHoldingPlatformTarget);
		}

		// Token: 0x04005DF6 RID: 24054
		public PawnKindDef heldPawnKind;

		// Token: 0x04005DF7 RID: 24055
		[MustTranslate]
		public string capturedLetterLabel;

		// Token: 0x04005DF8 RID: 24056
		[MustTranslate]
		public string capturedLetterText;

		// Token: 0x04005DF9 RID: 24057
		public float baseEscapeIntervalMtbDays = 60f;

		// Token: 0x04005DFA RID: 24058
		public bool lookForTargetOnEscape = true;

		// Token: 0x04005DFB RID: 24059
		public bool canBeExecuted = true;

		// Token: 0x04005DFC RID: 24060
		public bool getsColdContainmentBonus;

		// Token: 0x04005DFD RID: 24061
		public bool hasAnimation = true;
	}
}
