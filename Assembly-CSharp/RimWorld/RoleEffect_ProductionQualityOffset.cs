using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001755 RID: 5973
	public class RoleEffect_ProductionQualityOffset : RoleEffect
	{
		// Token: 0x060094B8 RID: 38072 RVA: 0x00363530 File Offset: 0x00361730
		public RoleEffect_ProductionQualityOffset()
		{
			this.labelKey = "RoleEffectProductionQualityOffset";
		}

		// Token: 0x060094B9 RID: 38073 RVA: 0x00363543 File Offset: 0x00361743
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.labelKey.Translate(this.offset.ToStringWithSign());
		}

		// Token: 0x040059A3 RID: 22947
		public int offset;
	}
}
