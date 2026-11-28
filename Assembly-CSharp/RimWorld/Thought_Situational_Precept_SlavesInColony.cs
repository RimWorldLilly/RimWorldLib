using System;

namespace RimWorld
{
	// Token: 0x02000DB4 RID: 3508
	public class Thought_Situational_Precept_SlavesInColony : Thought_Situational
	{
		// Token: 0x0600601F RID: 24607 RVA: 0x0022E650 File Offset: 0x0022C850
		public override float MoodOffset()
		{
			return this.BaseMoodOffset * (float)FactionUtility.GetSlavesInFactionCount(this.pawn.Faction);
		}
	}
}
