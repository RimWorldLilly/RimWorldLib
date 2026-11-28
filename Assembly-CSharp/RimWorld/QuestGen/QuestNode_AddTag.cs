using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200217F RID: 8575
	public class QuestNode_AddTag : QuestNode
	{
		// Token: 0x0600D64C RID: 54860 RVA: 0x004F99D6 File Offset: 0x004F7BD6
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D64D RID: 54861 RVA: 0x004F99DC File Offset: 0x004F7BDC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.targets.GetValue(slate) == null)
			{
				return;
			}
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			foreach (object obj in this.targets.GetValue(slate))
			{
				Thing thing = obj as Thing;
				if (thing != null)
				{
					QuestUtility.AddQuestTag(ref thing.questTags, questTagToAdd);
				}
				else
				{
					WorldObject worldObject = obj as WorldObject;
					if (worldObject != null)
					{
						QuestUtility.AddQuestTag(ref worldObject.questTags, questTagToAdd);
					}
					else
					{
						TransportShip transportShip = obj as TransportShip;
						if (transportShip != null)
						{
							QuestUtility.AddQuestTag(ref transportShip.questTags, questTagToAdd);
						}
					}
				}
			}
		}

		// Token: 0x0400878B RID: 34699
		[NoTranslate]
		public SlateRef<IEnumerable<object>> targets;

		// Token: 0x0400878C RID: 34700
		[NoTranslate]
		public SlateRef<string> tag;
	}
}
