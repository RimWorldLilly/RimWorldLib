using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ACF RID: 6863
	public class Building_Throne : Building
	{
		// Token: 0x17001F5E RID: 8030
		// (get) Token: 0x0600AEB6 RID: 44726 RVA: 0x003D86FC File Offset: 0x003D68FC
		public static IEnumerable<RoyalTitleDef> AllTitlesForThroneStature
		{
			get
			{
				return from title in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where !title.throneRoomRequirements.NullOrEmpty<RoomRequirement>()
				orderby title.seniority
				select title;
			}
		}

		// Token: 0x17001F5F RID: 8031
		// (get) Token: 0x0600AEB7 RID: 44727 RVA: 0x003D8756 File Offset: 0x003D6956
		public Pawn AssignedPawn
		{
			get
			{
				if (!ModLister.CheckRoyalty("Throne"))
				{
					return null;
				}
				if (this.CompAssignableToPawn == null || !this.CompAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
				{
					return null;
				}
				return this.CompAssignableToPawn.AssignedPawnsForReading[0];
			}
		}

		// Token: 0x17001F60 RID: 8032
		// (get) Token: 0x0600AEB8 RID: 44728 RVA: 0x003D8793 File Offset: 0x003D6993
		public CompAssignableToPawn_Throne CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Throne>();
			}
		}

		// Token: 0x17001F61 RID: 8033
		// (get) Token: 0x0600AEB9 RID: 44729 RVA: 0x003D879C File Offset: 0x003D699C
		public RoyalTitleDef TitleStature
		{
			get
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room == null || room.OutdoorsForWork)
				{
					return null;
				}
				RoyalTitleDef result = null;
				foreach (RoyalTitleDef royalTitleDef in Building_Throne.AllTitlesForThroneStature)
				{
					bool flag = true;
					for (int i = 0; i < royalTitleDef.throneRoomRequirements.Count; i++)
					{
						if (!(royalTitleDef.throneRoomRequirements[i] is RoomRequirement_HasAssignedThroneAnyOf) && !royalTitleDef.throneRoomRequirements[i].MetOrDisabled(room, null))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					result = royalTitleDef;
				}
				return result;
			}
		}

		// Token: 0x0600AEBA RID: 44730 RVA: 0x003D884C File Offset: 0x003D6A4C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			Room room = this.GetRoom(RegionType.Set_All);
			Pawn p = (this.CompAssignableToPawn.AssignedPawnsForReading.Count == 1) ? this.CompAssignableToPawn.AssignedPawnsForReading[0] : null;
			RoyalTitleDef titleStature = this.TitleStature;
			text += "\n" + "ThroneMaxSatisfiedTitle".Translate() + ": " + ((titleStature == null) ? "None".Translate() : titleStature.GetLabelCapFor(p));
			string text2 = RoomRoleWorker_ThroneRoom.Validate(room);
			if (text2 != null)
			{
				return text + "\n" + text2;
			}
			return text;
		}

		// Token: 0x0600AEBB RID: 44731 RVA: 0x003D88F8 File Offset: 0x003D6AF8
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Throne.<GetGizmos>d__9 <GetGizmos>d__ = new Building_Throne.<GetGizmos>d__9(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}
	}
}
