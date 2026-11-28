using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001805 RID: 6149
	public class CompProperties_AncientSecurityTerminalHackable : CompProperties_Hackable
	{
		// Token: 0x06009995 RID: 39317 RVA: 0x00378BE1 File Offset: 0x00376DE1
		public CompProperties_AncientSecurityTerminalHackable()
		{
			this.compClass = typeof(CompAncientSecurityTerminal);
		}

		// Token: 0x04005B9B RID: 23451
		[MustTranslate]
		public string letterTurretDisabledLabel;

		// Token: 0x04005B9C RID: 23452
		[MustTranslate]
		public string letterTurretDisabledText;

		// Token: 0x04005B9D RID: 23453
		[MustTranslate]
		public string letterTurretHackedLabel;

		// Token: 0x04005B9E RID: 23454
		[MustTranslate]
		public string letterTurretHackedText;

		// Token: 0x04005B9F RID: 23455
		[MustTranslate]
		public string letterDoorOpenedLabel;

		// Token: 0x04005BA0 RID: 23456
		[MustTranslate]
		public string letterDoorOpenedText;

		// Token: 0x04005BA1 RID: 23457
		[MustTranslate]
		public string messageNoValidTarget;
	}
}
