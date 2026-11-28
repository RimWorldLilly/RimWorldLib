using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001098 RID: 4248
	public class QuestPart_Bossgroup : QuestPart_MakeLord
	{
		// Token: 0x06006A61 RID: 27233 RVA: 0x00256A8C File Offset: 0x00254C8C
		protected override Lord MakeLord()
		{
			LordJob_BossgroupAssaultColony lordJob = new LordJob_BossgroupAssaultColony(this.faction, this.stageLocation, this.bosses);
			return LordMaker.MakeNewLord(this.faction, lordJob, this.mapParent.Map, null);
		}

		// Token: 0x06006A62 RID: 27234 RVA: 0x00256ACC File Offset: 0x00254CCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.stageLocation, "stageLocation", default(IntVec3), false);
			Scribe_Collections.Look<Pawn>(ref this.bosses, "bosses", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.bosses.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04004391 RID: 17297
		public List<Pawn> bosses = new List<Pawn>();

		// Token: 0x04004392 RID: 17298
		public IntVec3 stageLocation;
	}
}
