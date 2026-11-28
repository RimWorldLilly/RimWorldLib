using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002002 RID: 8194
	public class RoomRequirement_Area : RoomRequirement
	{
		// Token: 0x0600CF9A RID: 53146 RVA: 0x004C6824 File Offset: 0x004C4A24
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementArea").Translate(((r != null) ? (r.CellCount.ToString() + "/") : "") + this.area.ToString());
		}

		// Token: 0x0600CF9B RID: 53147 RVA: 0x004C688C File Offset: 0x004C4A8C
		public override bool Met(Room r, Pawn p = null)
		{
			return r.CellCount >= this.area;
		}

		// Token: 0x0600CF9C RID: 53148 RVA: 0x004C689F File Offset: 0x004C4A9F
		public override IEnumerable<string> ConfigErrors()
		{
			RoomRequirement_Area.<ConfigErrors>d__3 <ConfigErrors>d__ = new RoomRequirement_Area.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CF9D RID: 53149 RVA: 0x004C68AF File Offset: 0x004C4AAF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.area, "area", 0, false);
		}

		// Token: 0x0400841B RID: 33819
		public int area;
	}
}
