using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001127 RID: 4391
	public struct Signal
	{
		// Token: 0x06006E09 RID: 28169 RVA: 0x00269455 File Offset: 0x00267655
		public Signal(string tag, bool global = false)
		{
			this.tag = tag;
			this.args = default(SignalArgs);
			this.global = global;
		}

		// Token: 0x06006E0A RID: 28170 RVA: 0x00269471 File Offset: 0x00267671
		public Signal(string tag, SignalArgs args, bool global = false)
		{
			this.tag = tag;
			this.args = args;
			this.global = global;
		}

		// Token: 0x06006E0B RID: 28171 RVA: 0x00269488 File Offset: 0x00267688
		public Signal(string tag, NamedArgument arg1)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1);
			this.global = false;
		}

		// Token: 0x06006E0C RID: 28172 RVA: 0x002694A4 File Offset: 0x002676A4
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2);
			this.global = false;
		}

		// Token: 0x06006E0D RID: 28173 RVA: 0x002694C1 File Offset: 0x002676C1
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3);
			this.global = false;
		}

		// Token: 0x06006E0E RID: 28174 RVA: 0x002694E0 File Offset: 0x002676E0
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3, arg4);
			this.global = false;
		}

		// Token: 0x06006E0F RID: 28175 RVA: 0x00269501 File Offset: 0x00267701
		public Signal(string tag, params NamedArgument[] args)
		{
			this.tag = tag;
			this.args = new SignalArgs(args);
			this.global = false;
		}

		// Token: 0x06006E10 RID: 28176 RVA: 0x0026951D File Offset: 0x0026771D
		public override string ToString()
		{
			return this.tag;
		}

		// Token: 0x04004653 RID: 18003
		public string tag;

		// Token: 0x04004654 RID: 18004
		public SignalArgs args;

		// Token: 0x04004655 RID: 18005
		public bool global;
	}
}
