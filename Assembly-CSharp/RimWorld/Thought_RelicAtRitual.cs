using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBD RID: 3517
	public class Thought_RelicAtRitual : Thought_Memory
	{
		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x06006038 RID: 24632 RVA: 0x0022EB20 File Offset: 0x0022CD20
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.relicName.Named("RELICNAME")).CapitalizeFirst();
			}
		}

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x06006039 RID: 24633 RVA: 0x0022EB5C File Offset: 0x0022CD5C
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.relicName.Named("RELICNAME")).CapitalizeFirst();
			}
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0022EB96 File Offset: 0x0022CD96
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.relicName, "relicName", null, false);
		}

		// Token: 0x040035EF RID: 13807
		public string relicName;
	}
}
