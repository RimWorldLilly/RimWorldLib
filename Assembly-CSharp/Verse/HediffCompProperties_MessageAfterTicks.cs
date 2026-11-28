using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000459 RID: 1113
	public class HediffCompProperties_MessageAfterTicks : HediffCompProperties
	{
		// Token: 0x06001FB3 RID: 8115 RVA: 0x000C035C File Offset: 0x000BE55C
		public HediffCompProperties_MessageAfterTicks()
		{
			this.compClass = typeof(HediffComp_MessageAfterTicks);
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x000C0374 File Offset: 0x000BE574
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties_MessageAfterTicks.<ConfigErrors>d__7 <ConfigErrors>d__ = new HediffCompProperties_MessageAfterTicks.<ConfigErrors>d__7(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040018AD RID: 6317
		public int ticks;

		// Token: 0x040018AE RID: 6318
		public MessageTypeDef messageType;

		// Token: 0x040018AF RID: 6319
		public LetterDef letterType;

		// Token: 0x040018B0 RID: 6320
		[MustTranslate]
		public string message;

		// Token: 0x040018B1 RID: 6321
		[MustTranslate]
		public string letterLabel;

		// Token: 0x040018B2 RID: 6322
		[MustTranslate]
		public string letterText;
	}
}
