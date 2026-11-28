using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F6 RID: 8694
	public class QuestNode_IsHumanlike : QuestNode_RaceProperty
	{
		// Token: 0x0600D805 RID: 55301 RVA: 0x005021C4 File Offset: 0x005003C4
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Humanlike;
		}
	}
}
