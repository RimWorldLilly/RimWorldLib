using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000955 RID: 2389
	public class TransitionAction_Message : TransitionAction
	{
		// Token: 0x0600494A RID: 18762 RVA: 0x001C9EBD File Offset: 0x001C80BD
		public TransitionAction_Message(string message, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f) : this(message, MessageTypeDefOf.NeutralEvent, repeatAvoiderTag, repeatAvoiderSeconds, null)
		{
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x001C9ECE File Offset: 0x001C80CE
		public TransitionAction_Message(string message, MessageTypeDef messageType, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f, Func<bool> canSendMessage = null)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
			this.canSendMessage = canSendMessage;
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x001C9F06 File Offset: 0x001C8106
		public TransitionAction_Message(string message, MessageTypeDef messageType, TargetInfo lookTarget, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTarget = lookTarget;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x001C9F3E File Offset: 0x001C813E
		public TransitionAction_Message(string message, MessageTypeDef messageType, Func<TargetInfo> lookTargetGetter, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTargetGetter = lookTargetGetter;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x001C9F78 File Offset: 0x001C8178
		public override void DoAction(Transition trans)
		{
			if (!this.repeatAvoiderTag.NullOrEmpty() && !MessagesRepeatAvoider.MessageShowAllowed(this.repeatAvoiderTag, this.repeatAvoiderSeconds))
			{
				return;
			}
			if (this.canSendMessage != null && !this.canSendMessage())
			{
				return;
			}
			TargetInfo target = (this.lookTargetGetter != null) ? this.lookTargetGetter() : this.lookTarget;
			if (!target.IsValid)
			{
				target = trans.target.lord.ownedPawns.FirstOrDefault<Pawn>();
			}
			Messages.Message(this.message, target, this.type, true);
		}

		// Token: 0x04002F03 RID: 12035
		public string message;

		// Token: 0x04002F04 RID: 12036
		public MessageTypeDef type;

		// Token: 0x04002F05 RID: 12037
		public TargetInfo lookTarget;

		// Token: 0x04002F06 RID: 12038
		public Func<TargetInfo> lookTargetGetter;

		// Token: 0x04002F07 RID: 12039
		public string repeatAvoiderTag;

		// Token: 0x04002F08 RID: 12040
		public float repeatAvoiderSeconds;

		// Token: 0x04002F09 RID: 12041
		private Func<bool> canSendMessage;
	}
}
