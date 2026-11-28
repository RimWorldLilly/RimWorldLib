using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200179C RID: 6044
	public class PawnKindCount : StartingPawnCount
	{
		// Token: 0x170019A3 RID: 6563
		// (get) Token: 0x060096AD RID: 38573 RVA: 0x0036A4B8 File Offset: 0x003686B8
		public override string Summary
		{
			get
			{
				return "PawnCount".Translate(this.count.Named("COUNT"), ((this.count > 1) ? this.kindDef.GetLabelPlural(this.count) : this.kindDef.label).Named("KINDLABEL"));
			}
		}

		// Token: 0x060096AE RID: 38574 RVA: 0x0036A51A File Offset: 0x0036871A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
		}

		// Token: 0x060096AF RID: 38575 RVA: 0x0036A534 File Offset: 0x00368734
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			num ^= this.count;
			num ^= ((this.requiredAtStart > false) ? 1 : 0);
			if (this.kindDef != null)
			{
				num ^= this.kindDef.GetHashCode();
			}
			return num;
		}

		// Token: 0x04005A5B RID: 23131
		public PawnKindDef kindDef;
	}
}
