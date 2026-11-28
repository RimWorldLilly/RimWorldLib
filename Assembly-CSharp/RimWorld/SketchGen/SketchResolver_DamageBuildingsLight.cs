using System;

namespace RimWorld.SketchGen
{
	// Token: 0x020020CC RID: 8396
	public class SketchResolver_DamageBuildingsLight : SketchResolver_DamageBuildings
	{
		// Token: 0x170025E4 RID: 9700
		// (get) Token: 0x0600D35B RID: 54107 RVA: 0x004DE109 File Offset: 0x004DC309
		protected override float MaxPctOfTotalDestroyed
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x170025E5 RID: 9701
		// (get) Token: 0x0600D35C RID: 54108 RVA: 0x004DE110 File Offset: 0x004DC310
		protected override float HpRandomFactor
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x170025E6 RID: 9702
		// (get) Token: 0x0600D35D RID: 54109 RVA: 0x004DE117 File Offset: 0x004DC317
		protected override float DestroyChanceExp
		{
			get
			{
				return 2f;
			}
		}
	}
}
