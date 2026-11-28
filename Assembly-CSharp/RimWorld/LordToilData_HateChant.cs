using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CDD RID: 3293
	public class LordToilData_HateChant : LordToilData
	{
		// Token: 0x06005D26 RID: 23846 RVA: 0x00222AFF File Offset: 0x00220CFF
		public override void ExposeData()
		{
			Scribe_References.Look<GameCondition_HateChantDrone>(ref this.condition, "condition", false);
			Scribe_Values.Look<int>(ref this.lastDroneUpdate, "lastDroneUpdate", 0, false);
			Scribe_Collections.Look<PsychicRitualParticipant>(ref this.chanters, "chanters", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x040034FC RID: 13564
		public List<PsychicRitualParticipant> chanters;

		// Token: 0x040034FD RID: 13565
		public GameCondition_HateChantDrone condition;

		// Token: 0x040034FE RID: 13566
		public int lastDroneUpdate;
	}
}
