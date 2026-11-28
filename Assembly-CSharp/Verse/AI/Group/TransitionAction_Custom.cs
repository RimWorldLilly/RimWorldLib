using System;

namespace Verse.AI.Group
{
	// Token: 0x02000954 RID: 2388
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x06004947 RID: 18759 RVA: 0x001C9E76 File Offset: 0x001C8076
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x001C9E85 File Offset: 0x001C8085
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x001C9E94 File Offset: 0x001C8094
		public override void DoAction(Transition trans)
		{
			if (this.actionWithArg != null)
			{
				this.actionWithArg(trans);
			}
			if (this.action != null)
			{
				this.action();
			}
		}

		// Token: 0x04002F01 RID: 12033
		public Action action;

		// Token: 0x04002F02 RID: 12034
		public Action<Transition> actionWithArg;
	}
}
