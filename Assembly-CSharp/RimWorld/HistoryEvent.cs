using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001031 RID: 4145
	public struct HistoryEvent
	{
		// Token: 0x06006875 RID: 26741 RVA: 0x0024EAD1 File Offset: 0x0024CCD1
		public HistoryEvent(HistoryEventDef def)
		{
			this.def = def;
			this.args = default(SignalArgs);
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x0024EAE6 File Offset: 0x0024CCE6
		public HistoryEvent(HistoryEventDef def, SignalArgs args)
		{
			this.def = def;
			this.args = args;
		}

		// Token: 0x06006877 RID: 26743 RVA: 0x0024EAF6 File Offset: 0x0024CCF6
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1)
		{
			this.def = def;
			this.args = new SignalArgs(arg1);
		}

		// Token: 0x06006878 RID: 26744 RVA: 0x0024EB0B File Offset: 0x0024CD0B
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1, NamedArgument arg2)
		{
			this.def = def;
			this.args = new SignalArgs(arg1, arg2);
		}

		// Token: 0x06006879 RID: 26745 RVA: 0x0024EB21 File Offset: 0x0024CD21
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.def = def;
			this.args = new SignalArgs(arg1, arg2, arg3);
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x0024EB39 File Offset: 0x0024CD39
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.def = def;
			this.args = new SignalArgs(arg1, arg2, arg3, arg4);
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x0024EB53 File Offset: 0x0024CD53
		public HistoryEvent(HistoryEventDef def, params NamedArgument[] args)
		{
			this.def = def;
			this.args = new SignalArgs(args);
		}

		// Token: 0x04004287 RID: 17031
		public HistoryEventDef def;

		// Token: 0x04004288 RID: 17032
		public SignalArgs args;
	}
}
