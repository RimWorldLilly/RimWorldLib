using System;

namespace Verse.AI.Group
{
	// Token: 0x02000956 RID: 2390
	public class TransitionAction_Letter : TransitionAction
	{
		// Token: 0x0600494F RID: 18767 RVA: 0x001CA014 File Offset: 0x001C8214
		public TransitionAction_Letter(string title, string message, LetterDef type)
		{
			this.title = title;
			this.message = message;
			this.type = type;
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x001CA034 File Offset: 0x001C8234
		public override void DoAction(Transition trans)
		{
			Find.LetterStack.ReceiveLetter(this.title, this.message, this.type, trans.target.lord.ownedPawns, null, null, null, null, 0, true);
		}

		// Token: 0x04002F0A RID: 12042
		public string title;

		// Token: 0x04002F0B RID: 12043
		public string message;

		// Token: 0x04002F0C RID: 12044
		public LetterDef type;
	}
}
