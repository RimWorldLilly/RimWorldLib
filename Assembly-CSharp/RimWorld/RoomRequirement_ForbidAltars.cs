using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200200B RID: 8203
	public class RoomRequirement_ForbidAltars : RoomRequirement
	{
		// Token: 0x0600CFD5 RID: 53205 RVA: 0x004C7178 File Offset: 0x004C5378
		public override bool Met(Room r, Pawn p = null)
		{
			using (List<Thing>.Enumerator enumerator = r.ContainedAndAdjacentThings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.isAltar)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
