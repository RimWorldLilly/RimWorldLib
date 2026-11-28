using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001879 RID: 6265
	public class CompExplosivePowered : CompExplosive
	{
		// Token: 0x17001AD5 RID: 6869
		// (get) Token: 0x06009CDD RID: 40157 RVA: 0x0038714C File Offset: 0x0038534C
		protected override bool CanEverExplodeFromDamage
		{
			get
			{
				CompMechPowerCell compMechPowerCell = this.parent.TryGetComp<CompMechPowerCell>();
				return (compMechPowerCell == null || !compMechPowerCell.depleted) && base.CanEverExplodeFromDamage;
			}
		}
	}
}
