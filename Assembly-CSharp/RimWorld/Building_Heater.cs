using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A8F RID: 6799
	public class Building_Heater : Building_TempControl
	{
		// Token: 0x0600AB98 RID: 43928 RVA: 0x003C9A64 File Offset: 0x003C7C64
		public override void TickRare()
		{
			if (!base.Spawned)
			{
				return;
			}
			if (!this.compPowerTrader.PowerOn)
			{
				return;
			}
			float ambientTemperature = base.AmbientTemperature;
			float num;
			if (ambientTemperature < 20f)
			{
				num = 1f;
			}
			else if (ambientTemperature > 120f)
			{
				num = 0f;
			}
			else
			{
				num = Mathf.InverseLerp(120f, 20f, ambientTemperature);
			}
			float energyLimit = this.compTempControl.Props.energyPerSecond * num * 4.1666665f;
			float num2 = GenTemperature.ControlTemperatureTempChange(base.Position, base.Map, energyLimit, this.compTempControl.TargetTemperature);
			bool flag = !Mathf.Approximately(num2, 0f);
			CompProperties_Power props = this.compPowerTrader.Props;
			if (flag)
			{
				this.GetRoom(RegionType.Set_All).Temperature += num2;
				this.compPowerTrader.PowerOutput = -props.PowerConsumption;
			}
			else
			{
				this.compPowerTrader.PowerOutput = -props.PowerConsumption * this.compTempControl.Props.lowPowerConsumptionFactor;
			}
			this.compTempControl.operatingAtHighPower = flag;
		}

		// Token: 0x040063E4 RID: 25572
		private const float EfficiencyFalloffSpan = 100f;
	}
}
