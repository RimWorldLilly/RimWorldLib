using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002006 RID: 8198
	public class RoomRequirement_ThingAnyOf : RoomRequirement
	{
		// Token: 0x0600CFB5 RID: 53173 RVA: 0x004C6C58 File Offset: 0x004C4E58
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.things[0].label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x0600CFB6 RID: 53174 RVA: 0x004C6CAC File Offset: 0x004C4EAC
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (ThingDef def in this.things)
			{
				if (r.ContainsThing(def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CFB7 RID: 53175 RVA: 0x004C6D08 File Offset: 0x004C4F08
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_ThingAnyOf roomRequirement_ThingAnyOf = (RoomRequirement_ThingAnyOf)other;
			foreach (ThingDef item in this.things)
			{
				if (!roomRequirement_ThingAnyOf.things.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600CFB8 RID: 53176 RVA: 0x004C6D7C File Offset: 0x004C4F7C
		public override IEnumerable<string> ConfigErrors()
		{
			RoomRequirement_ThingAnyOf.<ConfigErrors>d__4 <ConfigErrors>d__ = new RoomRequirement_ThingAnyOf.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CFB9 RID: 53177 RVA: 0x004C6D8C File Offset: 0x004C4F8C
		public override bool PlayerCanBuildNow()
		{
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].IsResearchFinished)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CFBA RID: 53178 RVA: 0x004C6DC5 File Offset: 0x004C4FC5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.things, "things", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x04008420 RID: 33824
		public List<ThingDef> things;
	}
}
