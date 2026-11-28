using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200200A RID: 8202
	public class RoomRequirement_AllThingsAreGlowing : RoomRequirement
	{
		// Token: 0x0600CFD1 RID: 53201 RVA: 0x004C70E8 File Offset: 0x004C52E8
		public override bool Met(Room r, Pawn p = null)
		{
			using (IEnumerator<Thing> enumerator = r.ContainedThings(this.thingDef).GetEnumerator())
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

		// Token: 0x0600CFD2 RID: 53202 RVA: 0x004C7148 File Offset: 0x004C5348
		public override IEnumerable<string> ConfigErrors()
		{
			RoomRequirement_AllThingsAreGlowing.<ConfigErrors>d__2 <ConfigErrors>d__ = new RoomRequirement_AllThingsAreGlowing.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CFD3 RID: 53203 RVA: 0x004C7158 File Offset: 0x004C5358
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x04008423 RID: 33827
		public ThingDef thingDef;
	}
}
