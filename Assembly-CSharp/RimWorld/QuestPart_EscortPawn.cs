using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200109A RID: 4250
	public class QuestPart_EscortPawn : QuestPart_MakeLord
	{
		// Token: 0x06006A68 RID: 27240 RVA: 0x00256C2C File Offset: 0x00254E2C
		protected override Lord MakeLord()
		{
			LordJob_EscortPawn lordJob_EscortPawn = new LordJob_EscortPawn(this.escortee, this.shuttle);
			lordJob_EscortPawn.leavingDangerMessage = this.leavingDangerMessage;
			Lord lord = LordMaker.MakeNewLord(this.faction, lordJob_EscortPawn, base.Map, null);
			QuestUtility.AddQuestTag(ref lord.questTags, this.questTag);
			return lord;
		}

		// Token: 0x06006A69 RID: 27241 RVA: 0x00256C7C File Offset: 0x00254E7C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.escortee, "escortee", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
			Scribe_Values.Look<string>(ref this.leavingDangerMessage, "leavingDangerMessage", null, false);
		}

		// Token: 0x04004398 RID: 17304
		public Pawn escortee;

		// Token: 0x04004399 RID: 17305
		public Thing shuttle;

		// Token: 0x0400439A RID: 17306
		public string questTag;

		// Token: 0x0400439B RID: 17307
		public string leavingDangerMessage;
	}
}
