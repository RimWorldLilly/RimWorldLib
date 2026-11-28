using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002196 RID: 8598
	public class QuestNode_Log : QuestNode
	{
		// Token: 0x0600D695 RID: 54933 RVA: 0x004FB38F File Offset: 0x004F958F
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D696 RID: 54934 RVA: 0x004FB392 File Offset: 0x004F9592
		protected override void RunInt()
		{
			Log.Message("QuestNode_Log: " + this.message.ToString(QuestGen.slate));
		}

		// Token: 0x040087E8 RID: 34792
		[NoTranslate]
		public SlateRef<object> message;
	}
}
