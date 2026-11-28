using System;
using RimWorld.QuestGen;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001094 RID: 4244
	public class QuestPart_AssaultColony : QuestPart_MakeLord
	{
		// Token: 0x06006A52 RID: 27218 RVA: 0x00256508 File Offset: 0x00254708
		protected override Lord MakeLord()
		{
			Faction assaulterFaction = this.faction ?? this.pawns[0].Faction;
			bool flag = this.canTimeoutOrFlee;
			LordJob_AssaultColony lordJob = new LordJob_AssaultColony(assaulterFaction, this.canKidnap, flag, false, false, this.canSteal, false, false);
			Lord lord = LordMaker.MakeNewLord(this.faction ?? this.pawns[0].Faction, lordJob, this.mapParent.Map, null);
			QuestUtility.AddQuestTag(lord, QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.questTag));
			return lord;
		}

		// Token: 0x06006A53 RID: 27219 RVA: 0x0025658C File Offset: 0x0025478C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
		}

		// Token: 0x04004383 RID: 17283
		public bool canTimeoutOrFlee = true;

		// Token: 0x04004384 RID: 17284
		public bool canKidnap = true;

		// Token: 0x04004385 RID: 17285
		public bool canSteal = true;

		// Token: 0x04004386 RID: 17286
		public string questTag;
	}
}
