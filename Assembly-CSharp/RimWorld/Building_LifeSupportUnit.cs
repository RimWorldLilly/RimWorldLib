using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A90 RID: 6800
	public class Building_LifeSupportUnit : Building
	{
		// Token: 0x17001E87 RID: 7815
		// (get) Token: 0x0600AB9A RID: 43930 RVA: 0x003C9B7C File Offset: 0x003C7D7C
		private Room Room
		{
			get
			{
				Room result;
				if ((result = this.cachedRoom) == null)
				{
					result = (this.cachedRoom = this.GetRoom(RegionType.Set_All));
				}
				return result;
			}
		}

		// Token: 0x0600AB9B RID: 43931 RVA: 0x003C9BA4 File Offset: 0x003C7DA4
		public override void TickRare()
		{
			if (!base.Spawned)
			{
				return;
			}
			this.ComputeTempChange();
			this.ComputeVacuum();
			this.cachedRoom = null;
		}

		// Token: 0x0600AB9C RID: 43932 RVA: 0x003C9BC4 File Offset: 0x003C7DC4
		private void ComputeTempChange()
		{
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
			float energyLimit = 30f * num * 4.1666665f;
			float num2 = GenTemperature.ControlTemperatureTempChange(base.Position, base.Map, energyLimit, 20f);
			if (!Mathf.Approximately(num2, 0f))
			{
				this.Room.Temperature += num2;
			}
		}

		// Token: 0x0600AB9D RID: 43933 RVA: 0x003C9C50 File Offset: 0x003C7E50
		private void ComputeVacuum()
		{
			if (!base.Map.Biome.inVacuum || this.Room.ExposedToSpace)
			{
				return;
			}
			float num = 100f / (float)this.Room.CellCount * 0.05f * 4.1666665f;
			this.Room.Vacuum -= num;
		}

		// Token: 0x040063E5 RID: 25573
		private Room cachedRoom;

		// Token: 0x040063E6 RID: 25574
		private const float EfficiencyFalloffSpan = 100f;

		// Token: 0x040063E7 RID: 25575
		private const float TargetTemperature = 20f;

		// Token: 0x040063E8 RID: 25576
		private const float EnergyPerSecond = 30f;

		// Token: 0x040063E9 RID: 25577
		private const float AirPerSecondPerHundredCells = 0.05f;

		// Token: 0x040063EA RID: 25578
		private const float IntervalToPerSecond = 4.1666665f;
	}
}
