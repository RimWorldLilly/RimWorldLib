using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002241 RID: 8769
	public class QuestNode_SpawnWorldObjects : QuestNode
	{
		// Token: 0x0600D8F8 RID: 55544 RVA: 0x00506032 File Offset: 0x00504232
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8F9 RID: 55545 RVA: 0x00506038 File Offset: 0x00504238
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.worldObjects.GetValue(slate) == null)
			{
				return;
			}
			string text = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false);
			foreach (WorldObject worldObject in this.worldObjects.GetValue(slate))
			{
				QuestPart_SpawnWorldObject questPart_SpawnWorldObject = new QuestPart_SpawnWorldObject();
				questPart_SpawnWorldObject.worldObject = worldObject;
				questPart_SpawnWorldObject.inSignal = text;
				questPart_SpawnWorldObject.defsToExcludeFromHyperlinks = this.defsToExcludeFromHyperlinks.GetValue(slate);
				if (this.tile.GetValue(slate) != null)
				{
					worldObject.Tile = (this.tile.GetValue(slate) ?? PlanetTile.Invalid);
				}
				QuestGen.quest.AddPart(questPart_SpawnWorldObject);
			}
		}

		// Token: 0x04008AA2 RID: 35490
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008AA3 RID: 35491
		public SlateRef<IEnumerable<WorldObject>> worldObjects;

		// Token: 0x04008AA4 RID: 35492
		public SlateRef<PlanetTile?> tile;

		// Token: 0x04008AA5 RID: 35493
		public SlateRef<List<ThingDef>> defsToExcludeFromHyperlinks;
	}
}
