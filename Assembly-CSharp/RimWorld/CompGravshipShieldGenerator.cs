using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AA RID: 6314
	public class CompGravshipShieldGenerator : CompProjectileInterceptor
	{
		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x06009E1F RID: 40479 RVA: 0x0038C50C File Offset: 0x0038A70C
		private CompPowerTrader Power
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.power) == null)
				{
					result = (this.power = this.parent.TryGetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x06009E20 RID: 40480 RVA: 0x0038C537 File Offset: 0x0038A737
		private CompGravshipFacility Facility
		{
			get
			{
				return this.facility ?? this.parent.TryGetComp<CompGravshipFacility>();
			}
		}

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x06009E21 RID: 40481 RVA: 0x0038C54E File Offset: 0x0038A74E
		protected override int NumInactiveDots
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x06009E22 RID: 40482 RVA: 0x0038C551 File Offset: 0x0038A751
		private bool ShouldCharge
		{
			get
			{
				return this.Power.PowerOn && this.Facility.LinkedBuildings.Count > 0;
			}
		}

		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x06009E23 RID: 40483 RVA: 0x0038C575 File Offset: 0x0038A775
		protected override int HitPointsPerInterval
		{
			get
			{
				if (base.Active)
				{
					return 0;
				}
				if (this.ShouldCharge)
				{
					return 2;
				}
				return -1;
			}
		}

		// Token: 0x04005D88 RID: 23944
		private const int HitPointsPerIntervalPowered = 2;

		// Token: 0x04005D89 RID: 23945
		private const int HitPointsPerIntervalUnpowered = -1;

		// Token: 0x04005D8A RID: 23946
		private CompPowerTrader power;

		// Token: 0x04005D8B RID: 23947
		private CompGravshipFacility facility;
	}
}
