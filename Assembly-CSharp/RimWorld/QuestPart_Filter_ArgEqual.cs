using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106E RID: 4206
	public class QuestPart_Filter_ArgEqual : QuestPart_Filter
	{
		// Token: 0x0600698F RID: 27023 RVA: 0x00253958 File Offset: 0x00251B58
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return args.TryGetArg(this.name, out namedArgument) && object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06006990 RID: 27024 RVA: 0x00253989 File Offset: 0x00251B89
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x06006991 RID: 27025 RVA: 0x002539C0 File Offset: 0x00251BC0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x0400431E RID: 17182
		public string name;

		// Token: 0x0400431F RID: 17183
		public object obj;

		// Token: 0x04004320 RID: 17184
		public LookMode objLookMode;

		// Token: 0x04004321 RID: 17185
		private Type objType;
	}
}
