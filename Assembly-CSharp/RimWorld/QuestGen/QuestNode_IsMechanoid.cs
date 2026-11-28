using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F7 RID: 8695
	public class QuestNode_IsMechanoid : QuestNode_RaceProperty
	{
		// Token: 0x0600D807 RID: 55303 RVA: 0x005021D4 File Offset: 0x005003D4
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsMechanoid;
		}
	}
}
