using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB5 RID: 4021
	public class NegativeFishingOutcomeDef : Def
	{
		// Token: 0x04003E7A RID: 15994
		public ThingDef fishType;

		// Token: 0x04003E7B RID: 15995
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04003E7C RID: 15996
		[MustTranslate]
		public string letterText;

		// Token: 0x04003E7D RID: 15997
		public LetterDef letterDef;

		// Token: 0x04003E7E RID: 15998
		public DamageDef damageDef;

		// Token: 0x04003E7F RID: 15999
		public IntRange damageAmountRange;

		// Token: 0x04003E80 RID: 16000
		public HediffDef addsHediff;

		// Token: 0x04003E81 RID: 16001
		public float hediffSeverity;
	}
}
