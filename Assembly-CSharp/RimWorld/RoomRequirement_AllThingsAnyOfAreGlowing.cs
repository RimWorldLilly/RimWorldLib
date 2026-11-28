using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002009 RID: 8201
	public class RoomRequirement_AllThingsAnyOfAreGlowing : RoomRequirement_ThingAnyOf
	{
		// Token: 0x0600CFCC RID: 53196 RVA: 0x004C7054 File Offset: 0x004C5254
		public override string Label(Room r = null)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x0600CFCD RID: 53197 RVA: 0x004C7068 File Offset: 0x004C5268
		public override bool Met(Room r, Pawn p = null)
		{
			using (IEnumerator<Thing> enumerator = r.ContainedThingsList(this.things).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.TryGetComp<CompGlower>().Glows)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600CFCE RID: 53198 RVA: 0x004C70C8 File Offset: 0x004C52C8
		public override IEnumerable<string> ConfigErrors()
		{
			RoomRequirement_AllThingsAnyOfAreGlowing.<ConfigErrors>d__2 <ConfigErrors>d__ = new RoomRequirement_AllThingsAnyOfAreGlowing.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}
	}
}
