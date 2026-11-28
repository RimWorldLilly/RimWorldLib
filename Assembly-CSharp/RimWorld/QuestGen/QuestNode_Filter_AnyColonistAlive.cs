using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B4 RID: 8884
	public class QuestNode_Filter_AnyColonistAlive : QuestNode_Filter
	{
		// Token: 0x0600DB5B RID: 56155 RVA: 0x0051BB94 File Offset: 0x00519D94
		protected override QuestPart_Filter MakeFilterQuestPart()
		{
			Slate slate = QuestGen.slate;
			Map map = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
			return new QuestPart_Filter_AnyColonistAlive
			{
				mapParent = map.Parent
			};
		}

		// Token: 0x04008C67 RID: 35943
		public SlateRef<Map> map;
	}
}
