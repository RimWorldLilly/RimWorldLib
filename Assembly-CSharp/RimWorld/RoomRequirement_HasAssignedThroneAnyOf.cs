using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200200D RID: 8205
	public class RoomRequirement_HasAssignedThroneAnyOf : RoomRequirement_ThingAnyOf
	{
		// Token: 0x0600CFDB RID: 53211 RVA: 0x004C7350 File Offset: 0x004C5550
		public override bool Met(Room r, Pawn p = null)
		{
			if (p == null)
			{
				return false;
			}
			if (!r.ProperRoom)
			{
				return false;
			}
			foreach (Thing thing in r.ContainedAndAdjacentThings)
			{
				if (this.things.Contains(thing.def) && p.ownership.AssignedThrone == thing)
				{
					return true;
				}
			}
			return false;
		}
	}
}
