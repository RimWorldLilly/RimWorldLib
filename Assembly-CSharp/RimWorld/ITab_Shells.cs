using System;

namespace RimWorld
{
	// Token: 0x02001D81 RID: 7553
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x17002445 RID: 9285
		// (get) Token: 0x0600C3BC RID: 50108 RVA: 0x00479D58 File Offset: 0x00477F58
		protected override IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				IStoreSettingsParent selStoreSettingsParent = base.SelStoreSettingsParent;
				if (selStoreSettingsParent != null)
				{
					return selStoreSettingsParent;
				}
				Building_TurretGun building_TurretGun = base.SelObject as Building_TurretGun;
				if (building_TurretGun != null)
				{
					return base.GetThingOrThingCompStoreSettingsParent(building_TurretGun.gun);
				}
				return null;
			}
		}

		// Token: 0x17002446 RID: 9286
		// (get) Token: 0x0600C3BD RID: 50109 RVA: 0x00479D8E File Offset: 0x00477F8E
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C3BE RID: 50110 RVA: 0x00479D91 File Offset: 0x00477F91
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}
	}
}
