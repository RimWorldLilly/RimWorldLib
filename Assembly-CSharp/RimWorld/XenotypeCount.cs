using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200179E RID: 6046
	public class XenotypeCount : StartingPawnCount
	{
		// Token: 0x170019A7 RID: 6567
		// (get) Token: 0x060096BD RID: 38589 RVA: 0x0036AC00 File Offset: 0x00368E00
		public override string Summary
		{
			get
			{
				return "PawnCount".Translate(this.count.Named("COUNT"), (this.description.NullOrEmpty() ? this.xenotype.label : this.description).Named("KINDLABEL"));
			}
		}

		// Token: 0x060096BE RID: 38590 RVA: 0x0036AC5B File Offset: 0x00368E5B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<XenotypeDef>(ref this.xenotype, "xenotype");
			Scribe_Values.Look<DevelopmentalStage>(ref this.allowedDevelopmentalStages, "allowedDevelopmentalStages", DevelopmentalStage.None, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
		}

		// Token: 0x060096BF RID: 38591 RVA: 0x0036AC98 File Offset: 0x00368E98
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			num ^= this.allowedDevelopmentalStages.GetHashCode();
			num ^= this.description.GetHashCodeSafe();
			if (this.xenotype != null)
			{
				num ^= this.xenotype.GetHashCode();
			}
			return num;
		}

		// Token: 0x04005A5E RID: 23134
		public XenotypeDef xenotype;

		// Token: 0x04005A5F RID: 23135
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04005A60 RID: 23136
		[MustTranslate]
		public string description;
	}
}
