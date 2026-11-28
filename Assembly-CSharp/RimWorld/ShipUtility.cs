using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019FF RID: 6655
	public static class ShipUtility
	{
		// Token: 0x0600A790 RID: 42896 RVA: 0x003BAE50 File Offset: 0x003B9050
		public static Dictionary<ThingDef, int> RequiredParts()
		{
			if (ShipUtility.requiredParts == null)
			{
				ShipUtility.requiredParts = new Dictionary<ThingDef, int>();
				ShipUtility.requiredParts[ThingDefOf.Ship_CryptosleepCasket] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_ComputerCore] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_Reactor] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_Engine] = 3;
				ShipUtility.requiredParts[ThingDefOf.Ship_Beam] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_SensorCluster] = 1;
			}
			return ShipUtility.requiredParts;
		}

		// Token: 0x0600A791 RID: 42897 RVA: 0x003BAED3 File Offset: 0x003B90D3
		public static IEnumerable<string> LaunchFailReasons(Building rootBuilding)
		{
			ShipUtility.<LaunchFailReasons>d__2 <LaunchFailReasons>d__ = new ShipUtility.<LaunchFailReasons>d__2(-2);
			<LaunchFailReasons>d__.<>3__rootBuilding = rootBuilding;
			return <LaunchFailReasons>d__;
		}

		// Token: 0x0600A792 RID: 42898 RVA: 0x003BAEE4 File Offset: 0x003B90E4
		public static bool HasHibernatingParts(Building rootBuilding)
		{
			foreach (Building thing in ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A793 RID: 42899 RVA: 0x003BAF54 File Offset: 0x003B9154
		public static void StartupHibernatingParts(Building rootBuilding)
		{
			foreach (Building thing in ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					compHibernatable.Startup();
				}
			}
			SoundDefOf.ShipReactor_Startup.PlayOneShot(SoundInfo.InMap(rootBuilding, MaintenanceType.None));
		}

		// Token: 0x0600A794 RID: 42900 RVA: 0x003BAFD8 File Offset: 0x003B91D8
		public static List<Building> ShipBuildingsAttachedTo(Building root)
		{
			ShipUtility.closedSet.Clear();
			if (root == null || root.Destroyed)
			{
				return ShipUtility.closedSet;
			}
			ShipUtility.openSet.Clear();
			ShipUtility.openSet.Add(root);
			while (ShipUtility.openSet.Count > 0)
			{
				Building building = ShipUtility.openSet[ShipUtility.openSet.Count - 1];
				ShipUtility.openSet.Remove(building);
				ShipUtility.closedSet.Add(building);
				foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(building))
				{
					Building edifice = c.GetEdifice(building.Map);
					if (edifice != null && edifice.def.building.shipPart && !ShipUtility.closedSet.Contains(edifice) && !ShipUtility.openSet.Contains(edifice))
					{
						ShipUtility.openSet.Add(edifice);
					}
				}
			}
			return ShipUtility.closedSet;
		}

		// Token: 0x0600A795 RID: 42901 RVA: 0x003BB0DC File Offset: 0x003B92DC
		public static IEnumerable<Gizmo> ShipStartupGizmos(Building building)
		{
			ShipUtility.<ShipStartupGizmos>d__8 <ShipStartupGizmos>d__ = new ShipUtility.<ShipStartupGizmos>d__8(-2);
			<ShipStartupGizmos>d__.<>3__building = building;
			return <ShipStartupGizmos>d__;
		}

		// Token: 0x04006298 RID: 25240
		private static Dictionary<ThingDef, int> requiredParts;

		// Token: 0x04006299 RID: 25241
		private static List<Building> closedSet = new List<Building>();

		// Token: 0x0400629A RID: 25242
		private static List<Building> openSet = new List<Building>();
	}
}
