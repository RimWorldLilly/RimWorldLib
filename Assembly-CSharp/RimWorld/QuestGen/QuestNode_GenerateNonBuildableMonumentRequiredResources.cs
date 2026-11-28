using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021AD RID: 8621
	public class QuestNode_GenerateNonBuildableMonumentRequiredResources : QuestNode
	{
		// Token: 0x0600D6E1 RID: 55009 RVA: 0x004FC2DA File Offset: 0x004FA4DA
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D6E2 RID: 55010 RVA: 0x004FC2E0 File Offset: 0x004FA4E0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker value = this.monumentMarker.GetValue(slate);
			for (int i = 0; i < value.sketch.Things.Count; i++)
			{
				ThingDef def = value.sketch.Things[i].def;
				ThingDef stuff = value.sketch.Things[i].stuff;
				if (def.category == ThingCategory.Building && !def.BuildableByPlayer)
				{
					MinifiedThing obj = ThingMaker.MakeThing(def, stuff).MakeMinified(DestroyMode.Vanish);
					QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.addToList.GetValue(slate), obj);
				}
			}
		}

		// Token: 0x04008820 RID: 34848
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x04008821 RID: 34849
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
