using System;
using RimWorld.Planet;

namespace RimWorld.QuestGen
{
	// Token: 0x02002245 RID: 8773
	public class QuestNode_WorldObjectTimeout : QuestNode_Delay
	{
		// Token: 0x0600D904 RID: 55556 RVA: 0x005063B8 File Offset: 0x005045B8
		protected override QuestPart_Delay MakeDelayQuestPart()
		{
			QuestPart_WorldObjectTimeout questPart_WorldObjectTimeout = new QuestPart_WorldObjectTimeout();
			questPart_WorldObjectTimeout.worldObject = this.worldObject.GetValue(QuestGen.slate);
			this.destroyOnCleanup.TryGetValue(QuestGen.slate, out questPart_WorldObjectTimeout.destroyOnCleanup);
			return questPart_WorldObjectTimeout;
		}

		// Token: 0x04008AB3 RID: 35507
		public SlateRef<bool> destroyOnCleanup;

		// Token: 0x04008AB4 RID: 35508
		public SlateRef<WorldObject> worldObject;
	}
}
