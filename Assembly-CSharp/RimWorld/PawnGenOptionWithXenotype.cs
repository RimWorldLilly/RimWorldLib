using System;

namespace RimWorld
{
	// Token: 0x02000F55 RID: 3925
	public struct PawnGenOptionWithXenotype
	{
		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x060064FF RID: 25855 RVA: 0x00240701 File Offset: 0x0023E901
		public PawnGenOption Option
		{
			get
			{
				return this.option;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x06006500 RID: 25856 RVA: 0x00240709 File Offset: 0x0023E909
		public XenotypeDef Xenotype
		{
			get
			{
				return this.xenotype;
			}
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x06006501 RID: 25857 RVA: 0x00240711 File Offset: 0x0023E911
		public float SelectionWeight
		{
			get
			{
				return this.selectionWeight;
			}
		}

		// Token: 0x06006502 RID: 25858 RVA: 0x00240719 File Offset: 0x0023E919
		public PawnGenOptionWithXenotype(PawnGenOption option, XenotypeDef xenotype, float selectionWeight)
		{
			this.option = option;
			this.xenotype = xenotype;
			this.selectionWeight = selectionWeight;
		}

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x06006503 RID: 25859 RVA: 0x00240730 File Offset: 0x0023E930
		public float Cost
		{
			get
			{
				if (this.xenotype == null)
				{
					return this.option.Cost;
				}
				return this.option.Cost * this.xenotype.combatPowerFactor;
			}
		}

		// Token: 0x04003AE6 RID: 15078
		private PawnGenOption option;

		// Token: 0x04003AE7 RID: 15079
		private XenotypeDef xenotype;

		// Token: 0x04003AE8 RID: 15080
		private float selectionWeight;
	}
}
