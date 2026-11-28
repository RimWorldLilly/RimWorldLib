using System;

namespace RimWorld
{
	// Token: 0x0200187B RID: 6267
	public class CompFacilityInactiveWhenElectricityDisabled : CompFacility
	{
		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x06009CFB RID: 40187 RVA: 0x00387A92 File Offset: 0x00385C92
		public override bool CanBeActive
		{
			get
			{
				return base.CanBeActive && !this.parent.Map.gameConditionManager.ElectricityDisabled(this.parent.Map);
			}
		}
	}
}
