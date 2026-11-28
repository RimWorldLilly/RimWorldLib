using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000489 RID: 1161
	public class HediffComp_VatGrowing : HediffComp
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06002050 RID: 8272 RVA: 0x000C2555 File Offset: 0x000C0755
		public override bool CompShouldRemove
		{
			get
			{
				return base.Pawn.Spawned || !(base.Pawn.ParentHolder is Building_GrowthVat);
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06002051 RID: 8273 RVA: 0x000C257C File Offset: 0x000C077C
		public override string CompTipStringExtra
		{
			get
			{
				return "AgingSpeed".Translate() + ": x" + 20.ToString();
			}
		}
	}
}
