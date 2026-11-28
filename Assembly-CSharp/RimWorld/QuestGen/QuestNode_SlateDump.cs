using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002198 RID: 8600
	public class QuestNode_SlateDump : QuestNode
	{
		// Token: 0x0600D69B RID: 54939 RVA: 0x004FB42A File Offset: 0x004F962A
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D69C RID: 54940 RVA: 0x004FB42D File Offset: 0x004F962D
		protected override void RunInt()
		{
			Log.Message(QuestGen.slate.ToString());
		}
	}
}
