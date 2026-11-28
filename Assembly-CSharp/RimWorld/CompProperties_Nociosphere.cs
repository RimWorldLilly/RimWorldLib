using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001914 RID: 6420
	public class CompProperties_Nociosphere : CompProperties_Interactable
	{
		// Token: 0x0600A0E2 RID: 41186 RVA: 0x0039856A File Offset: 0x0039676A
		public CompProperties_Nociosphere()
		{
			this.compClass = typeof(CompNociosphere);
		}

		// Token: 0x04005EFD RID: 24317
		public List<CompProperties_Nociosphere.AttackDetails> attacks = new List<CompProperties_Nociosphere.AttackDetails>();

		// Token: 0x04005EFE RID: 24318
		public FloatRange sentOnslaughtDurationSeconds;

		// Token: 0x04005EFF RID: 24319
		public int minOnslaughtTicks;

		// Token: 0x04005F00 RID: 24320
		public float activityOnRoofCollapsed = 0.3f;

		// Token: 0x04005F01 RID: 24321
		[MustTranslate]
		public string onslaughtInspectText;

		// Token: 0x04005F02 RID: 24322
		[MustTranslate]
		public string becomingUnstableInspectText;

		// Token: 0x04005F03 RID: 24323
		[MustTranslate]
		public string unstableInspectText;

		// Token: 0x04005F04 RID: 24324
		[MustTranslate]
		public string unstableWarning;

		// Token: 0x04005F05 RID: 24325
		[MustTranslate]
		public string leftLetterLabel;

		// Token: 0x04005F06 RID: 24326
		[MustTranslate]
		public string leftLetterText;

		// Token: 0x04005F07 RID: 24327
		[MustTranslate]
		public string onslaughtEndedMessage;

		// Token: 0x04005F08 RID: 24328
		[MustTranslate]
		public string departingMessage;

		// Token: 0x0200367E RID: 13950
		public class AttackDetails
		{
			// Token: 0x0400D9BE RID: 55742
			public AbilityDef ability;

			// Token: 0x0400D9BF RID: 55743
			public bool requiresPawn = true;

			// Token: 0x0400D9C0 RID: 55744
			public bool aiPreferArtificial;

			// Token: 0x0400D9C1 RID: 55745
			public float maxCooldownTicks = 500f;
		}
	}
}
