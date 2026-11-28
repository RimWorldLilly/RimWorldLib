using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F3 RID: 6387
	public class CompProperties_MechRelay : CompProperties
	{
		// Token: 0x0600A024 RID: 40996 RVA: 0x00395D74 File Offset: 0x00393F74
		public CompProperties_MechRelay()
		{
			this.compClass = typeof(CompMechRelay);
		}

		// Token: 0x04005EB8 RID: 24248
		[MustTranslate]
		public string activeInspectString;

		// Token: 0x04005EB9 RID: 24249
		[MustTranslate]
		public string destabilizationInspectString;

		// Token: 0x04005EBA RID: 24250
		[MustTranslate]
		public string destabilizationMessage;

		// Token: 0x04005EBB RID: 24251
		[MustTranslate]
		public string crashedLetterLabel;

		// Token: 0x04005EBC RID: 24252
		[MustTranslate]
		public string crashedLetterText;

		// Token: 0x04005EBD RID: 24253
		public ThingDef crashedThingDef;
	}
}
