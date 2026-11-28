using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F5 RID: 8693
	public class QuestNode_IsFlesh : QuestNode_RaceProperty
	{
		// Token: 0x0600D803 RID: 55299 RVA: 0x005021B4 File Offset: 0x005003B4
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsFlesh;
		}
	}
}
