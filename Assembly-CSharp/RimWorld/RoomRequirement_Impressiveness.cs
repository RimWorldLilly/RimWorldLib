using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002008 RID: 8200
	public class RoomRequirement_Impressiveness : RoomRequirement
	{
		// Token: 0x0600CFC6 RID: 53190 RVA: 0x004C6F80 File Offset: 0x004C5180
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementImpressiveness").Translate() + " " + ((r != null) ? (Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)).ToString() + "/") : "") + this.impressiveness.ToString();
		}

		// Token: 0x0600CFC7 RID: 53191 RVA: 0x004C6FFC File Offset: 0x004C51FC
		public override bool Met(Room r, Pawn p = null)
		{
			return Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)) >= (float)this.impressiveness;
		}

		// Token: 0x0600CFC8 RID: 53192 RVA: 0x004C701A File Offset: 0x004C521A
		public override IEnumerable<string> ConfigErrors()
		{
			RoomRequirement_Impressiveness.<ConfigErrors>d__3 <ConfigErrors>d__ = new RoomRequirement_Impressiveness.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CFC9 RID: 53193 RVA: 0x004C702A File Offset: 0x004C522A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.impressiveness, "impressiveness", 0, false);
		}

		// Token: 0x04008422 RID: 33826
		public int impressiveness;
	}
}
