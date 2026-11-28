using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB3 RID: 8115
	public class StatPart_WorkTableRoomRole : StatPart
	{
		// Token: 0x0600CDFA RID: 52730 RVA: 0x004BA73C File Offset: 0x004B893C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && req.Thing.def.building != null && StatPart_WorkTableRoomRole.Applies(req.Thing))
			{
				val *= req.Thing.def.building.workTableNotInRoomRoleFactor;
			}
		}

		// Token: 0x0600CDFB RID: 52731 RVA: 0x004BA790 File Offset: 0x004B8990
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing.def.building != null && StatPart_WorkTableRoomRole.Applies(req.Thing))
			{
				return "NotInRoomRole".Translate(req.Thing.def.building.workTableRoomRole.label).CapitalizeFirst() + ": x" + req.Thing.def.building.workTableNotInRoomRoleFactor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CDFC RID: 52732 RVA: 0x004BA830 File Offset: 0x004B8A30
		public static bool Applies(Thing parent)
		{
			ThingDef def = parent.def;
			bool flag;
			if (def == null)
			{
				flag = (null != null);
			}
			else
			{
				BuildingProperties building = def.building;
				flag = (((building != null) ? building.workTableRoomRole : null) != null);
			}
			if (!flag)
			{
				return false;
			}
			Room room = parent.GetRoom(RegionType.Set_All);
			return room != null && !room.PsychologicallyOutdoors && room.Role != parent.def.building.workTableRoomRole;
		}

		// Token: 0x0600CDFD RID: 52733 RVA: 0x004BA890 File Offset: 0x004B8A90
		public static bool WouldApplyToBuildingIfPlaced(ThingDef def, Map map, IntVec3 cell)
		{
			bool flag;
			if (def == null)
			{
				flag = (null != null);
			}
			else
			{
				BuildingProperties building = def.building;
				flag = (((building != null) ? building.workTableRoomRole : null) != null);
			}
			if (!flag || Mathf.Approximately(def.building.workTableNotInRoomRoleFactor, 1f))
			{
				return false;
			}
			Room room = cell.GetRoom(map);
			return room != null && !room.PsychologicallyOutdoors && room.Role != def.building.workTableRoomRole && room.GetRoomRoleIfBuildingPlaced(def) != def.building.workTableRoomRole;
		}
	}
}
