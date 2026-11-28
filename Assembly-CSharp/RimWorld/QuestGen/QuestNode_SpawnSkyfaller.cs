using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002240 RID: 8768
	public class QuestNode_SpawnSkyfaller : QuestNode
	{
		// Token: 0x0600D8F5 RID: 55541 RVA: 0x00505F17 File Offset: 0x00504117
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8F6 RID: 55542 RVA: 0x00505F1C File Offset: 0x0050411C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			Skyfaller thing = SkyfallerMaker.MakeSkyfaller(this.skyfallerDef.GetValue(slate), this.innerThings.GetValue(slate));
			QuestPart_SpawnThing questPart_SpawnThing = new QuestPart_SpawnThing();
			questPart_SpawnThing.thing = thing;
			questPart_SpawnThing.mapParent = map.Parent;
			if (this.factionOfForSafeSpot.GetValue(slate) != null)
			{
				questPart_SpawnThing.factionForFindingSpot = this.factionOfForSafeSpot.GetValue(slate).Faction;
			}
			if (this.cell.GetValue(slate) != null)
			{
				questPart_SpawnThing.cell = this.cell.GetValue(slate).Value;
			}
			questPart_SpawnThing.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SpawnThing.lookForSafeSpot = this.lookForSafeSpot.GetValue(slate);
			questPart_SpawnThing.tryLandInShipLandingZone = this.tryLandInShipLandingZone.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SpawnThing);
		}

		// Token: 0x04008A9B RID: 35483
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A9C RID: 35484
		public SlateRef<ThingDef> skyfallerDef;

		// Token: 0x04008A9D RID: 35485
		public SlateRef<IEnumerable<Thing>> innerThings;

		// Token: 0x04008A9E RID: 35486
		public SlateRef<IntVec3?> cell;

		// Token: 0x04008A9F RID: 35487
		public SlateRef<Pawn> factionOfForSafeSpot;

		// Token: 0x04008AA0 RID: 35488
		public SlateRef<bool> lookForSafeSpot;

		// Token: 0x04008AA1 RID: 35489
		public SlateRef<bool> tryLandInShipLandingZone;
	}
}
