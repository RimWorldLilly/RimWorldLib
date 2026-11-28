using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001A74 RID: 6772
	public class Building_TurretRocket : Building_TurretGun
	{
		// Token: 0x17001E0B RID: 7691
		// (get) Token: 0x0600AA64 RID: 43620 RVA: 0x003C5735 File Offset: 0x003C3935
		protected override bool CanSetForcedTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E0C RID: 7692
		// (get) Token: 0x0600AA65 RID: 43621 RVA: 0x003C5738 File Offset: 0x003C3938
		protected override bool HideForceTargetGizmo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E0D RID: 7693
		// (get) Token: 0x0600AA66 RID: 43622 RVA: 0x003C573B File Offset: 0x003C393B
		public override Material TurretTopMaterial
		{
			get
			{
				if (this.refuelableComp.IsFull)
				{
					return this.def.building.turretGunDef.building.turretTopLoadedMat;
				}
				return this.def.building.turretTopMat;
			}
		}
	}
}
