using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A76 RID: 2678
	public class JobDriver_ShieldMech : JobDriver_CastAbility
	{
		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x060050C0 RID: 20672 RVA: 0x001E5075 File Offset: 0x001E3275
		private CompProjectileInterceptor ProjectileInterceptor
		{
			get
			{
				if (this.projectileInterceptor == null && this.mechShield != null)
				{
					this.projectileInterceptor = this.mechShield.GetComp<CompProjectileInterceptor>();
				}
				return this.projectileInterceptor;
			}
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x001E509E File Offset: 0x001E329E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ShieldMech.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_ShieldMech.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x001E50AE File Offset: 0x001E32AE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MechShield>(ref this.mechShield, "mechShield", false);
		}

		// Token: 0x0400315D RID: 12637
		private const int DurationTicks = 1800;

		// Token: 0x0400315E RID: 12638
		private MechShield mechShield;

		// Token: 0x0400315F RID: 12639
		private CompProjectileInterceptor projectileInterceptor;
	}
}
