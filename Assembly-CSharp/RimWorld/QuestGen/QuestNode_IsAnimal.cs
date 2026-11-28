using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F4 RID: 8692
	public class QuestNode_IsAnimal : QuestNode_RaceProperty
	{
		// Token: 0x0600D801 RID: 55297 RVA: 0x005021A4 File Offset: 0x005003A4
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Animal;
		}
	}
}
