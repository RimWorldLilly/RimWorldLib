using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015DF RID: 5599
	public class Thought_MemoryRoyalTitle : Thought_Memory
	{
		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x06008B1E RID: 35614 RVA: 0x00326C24 File Offset: 0x00324E24
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x06008B1F RID: 35615 RVA: 0x00326C74 File Offset: 0x00324E74
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x06008B20 RID: 35616 RVA: 0x00326CC1 File Offset: 0x00324EC1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.titleDef, "titleDef");
		}

		// Token: 0x06008B21 RID: 35617 RVA: 0x00326CD9 File Offset: 0x00324ED9
		public override void CopyFrom(Thought_Memory m)
		{
			this.titleDef = ((Thought_MemoryRoyalTitle)m).titleDef;
			base.CopyFrom(m);
		}

		// Token: 0x0400558E RID: 21902
		public RoyalTitleDef titleDef;
	}
}
