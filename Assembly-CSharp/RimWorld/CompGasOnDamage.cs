using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189A RID: 6298
	public class CompGasOnDamage : ThingComp
	{
		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x06009DC7 RID: 40391 RVA: 0x0038AF45 File Offset: 0x00389145
		private CompProperties_GasOnDamage Props
		{
			get
			{
				return (CompProperties_GasOnDamage)this.props;
			}
		}

		// Token: 0x06009DC8 RID: 40392 RVA: 0x0038AF54 File Offset: 0x00389154
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(ref dinfo, out absorbed);
			if (dinfo.Def.ExternalViolenceFor(this.parent))
			{
				float num = Mathf.Min(dinfo.Amount, (float)this.parent.HitPoints) * this.Props.damageFactor;
				if (this.Props.useStackCountAsFactor)
				{
					num *= (float)this.parent.stackCount;
				}
				GasUtility.AddGas(this.parent.PositionHeld, this.parent.MapHeld, this.Props.type, (int)num);
			}
		}
	}
}
