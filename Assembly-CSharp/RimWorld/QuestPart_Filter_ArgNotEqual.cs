using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106F RID: 4207
	public class QuestPart_Filter_ArgNotEqual : QuestPart_Filter
	{
		// Token: 0x06006993 RID: 27027 RVA: 0x002539E8 File Offset: 0x00251BE8
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return !args.TryGetArg(this.name, out namedArgument) || !object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06006994 RID: 27028 RVA: 0x00253A1C File Offset: 0x00251C1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x06006995 RID: 27029 RVA: 0x00253A53 File Offset: 0x00251C53
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x04004322 RID: 17186
		public string name;

		// Token: 0x04004323 RID: 17187
		public object obj;

		// Token: 0x04004324 RID: 17188
		private Type objType;

		// Token: 0x04004325 RID: 17189
		private LookMode objLookMode;
	}
}
