using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002004 RID: 8196
	public class RoomRequirement_Thing : RoomRequirement
	{
		// Token: 0x0600CFA7 RID: 53159 RVA: 0x004C6A93 File Offset: 0x004C4C93
		public override bool Met(Room r, Pawn p = null)
		{
			return r.ContainsThing(this.thingDef);
		}

		// Token: 0x0600CFA8 RID: 53160 RVA: 0x004C6AA4 File Offset: 0x004C4CA4
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_Thing roomRequirement_Thing = (RoomRequirement_Thing)other;
			return this.thingDef == roomRequirement_Thing.thingDef;
		}

		// Token: 0x0600CFA9 RID: 53161 RVA: 0x004C6AD1 File Offset: 0x004C4CD1
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.thingDef.label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x0600CFAA RID: 53162 RVA: 0x004C6B11 File Offset: 0x004C4D11
		public override IEnumerable<string> ConfigErrors()
		{
			RoomRequirement_Thing.<ConfigErrors>d__4 <ConfigErrors>d__ = new RoomRequirement_Thing.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CFAB RID: 53163 RVA: 0x004C6B21 File Offset: 0x004C4D21
		public override bool PlayerCanBuildNow()
		{
			return this.thingDef.IsResearchFinished;
		}

		// Token: 0x0600CFAC RID: 53164 RVA: 0x004C6B2E File Offset: 0x004C4D2E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x0400841E RID: 33822
		public ThingDef thingDef;
	}
}
