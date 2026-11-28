using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A77 RID: 2679
	public static class RemoteShieldUtility
	{
		// Token: 0x060050C9 RID: 20681 RVA: 0x001E5208 File Offset: 0x001E3408
		private static MechShield GetActiveMechShield(Pawn mech)
		{
			if (!mech.Spawned)
			{
				return null;
			}
			List<Thing> thingList = mech.Position.GetThingList(mech.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				MechShield mechShield = thingList[i] as MechShield;
				if (mechShield != null && mechShield.IsTargeting(mech))
				{
					return mechShield;
				}
			}
			return null;
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x001E5260 File Offset: 0x001E3460
		public static Gizmo GetActiveMechShieldGizmo(Pawn mech)
		{
			if (!Find.Selector.IsSelected(mech))
			{
				return null;
			}
			MechShield activeMechShield = RemoteShieldUtility.GetActiveMechShield(mech);
			if (activeMechShield != null)
			{
				return new Gizmo_ProjectileInterceptorHitPoints
				{
					interceptor = activeMechShield.TryGetComp<CompProjectileInterceptor>()
				};
			}
			return null;
		}
	}
}
