using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE0 RID: 3808
	public class CompProperties_Power : CompProperties
	{
		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x060063AD RID: 25517 RVA: 0x0023C1C8 File Offset: 0x0023A3C8
		public float PowerConsumption
		{
			get
			{
				float num = this.basePowerConsumption;
				if (Current.ProgramState == ProgramState.Entry)
				{
					return num;
				}
				int num2 = 0;
				for (;;)
				{
					int num3 = num2;
					List<CompProperties_Power.PowerUpgrade> list = this.powerUpgrades;
					int? num4 = (list != null) ? new int?(list.Count) : null;
					if (!(num3 < num4.GetValueOrDefault() & num4 != null))
					{
						break;
					}
					ResearchProjectDef researchProject = this.powerUpgrades[num2].researchProject;
					if (researchProject != null && researchProject.IsFinished)
					{
						num *= this.powerUpgrades[num2].factor;
					}
					num2++;
				}
				return num;
			}
		}

		// Token: 0x060063AE RID: 25518 RVA: 0x0023C255 File Offset: 0x0023A455
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			CompProperties_Power.<SpecialDisplayStats>d__14 <SpecialDisplayStats>d__ = new CompProperties_Power.<SpecialDisplayStats>d__14(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04003807 RID: 14343
		public bool transmitsPower;

		// Token: 0x04003808 RID: 14344
		private float basePowerConsumption;

		// Token: 0x04003809 RID: 14345
		public float idlePowerDraw = -1f;

		// Token: 0x0400380A RID: 14346
		public bool shortCircuitInRain;

		// Token: 0x0400380B RID: 14347
		public bool showPowerNeededIfOff = true;

		// Token: 0x0400380C RID: 14348
		public SoundDef soundPowerOn;

		// Token: 0x0400380D RID: 14349
		public SoundDef soundPowerOff;

		// Token: 0x0400380E RID: 14350
		public SoundDef soundAmbientPowered;

		// Token: 0x0400380F RID: 14351
		public SoundDef soundAmbientProducingPower;

		// Token: 0x04003810 RID: 14352
		public List<CompProperties_Power.PowerUpgrade> powerUpgrades;

		// Token: 0x04003811 RID: 14353
		public bool alwaysDisplayAsUsingPower;

		// Token: 0x02002F23 RID: 12067
		public class PowerUpgrade
		{
			// Token: 0x0400BF60 RID: 48992
			public ResearchProjectDef researchProject;

			// Token: 0x0400BF61 RID: 48993
			public float factor;
		}
	}
}
