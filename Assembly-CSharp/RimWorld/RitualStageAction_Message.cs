using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200171D RID: 5917
	public class RitualStageAction_Message : RitualStageAction
	{
		// Token: 0x060093B6 RID: 37814 RVA: 0x0035E478 File Offset: 0x0035C678
		public override void Apply(LordJob_Ritual ritual)
		{
			Messages.Message(this.text.Formatted(ritual.Ritual.Label).CapitalizeFirst(), ritual.selectedTarget, this.messageTypeDef, false);
		}

		// Token: 0x060093B7 RID: 37815 RVA: 0x0035E4C4 File Offset: 0x0035C6C4
		public override void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageTypeDef, "messageTypeDef");
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
		}

		// Token: 0x04005949 RID: 22857
		[MustTranslate]
		public string text;

		// Token: 0x0400594A RID: 22858
		public MessageTypeDef messageTypeDef;
	}
}
