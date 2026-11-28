using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CA RID: 6346
	public class CompInspectStringEmergence : CompInspectString
	{
		// Token: 0x06009F0B RID: 40715 RVA: 0x0039101C File Offset: 0x0038F21C
		public override string CompInspectStringExtra()
		{
			if (this.sourcePawn != null)
			{
				return base.Props.inspectString.Formatted(this.sourcePawn.Named("SOURCEPAWN")).Resolve();
			}
			return null;
		}

		// Token: 0x06009F0C RID: 40716 RVA: 0x0039105B File Offset: 0x0038F25B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Pawn>(ref this.sourcePawn, "sourcePawn", false);
		}

		// Token: 0x04005E19 RID: 24089
		public Pawn sourcePawn;
	}
}
