using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C5 RID: 8645
	public class QuestNode_GetMapOf : QuestNode
	{
		// Token: 0x0600D73E RID: 55102 RVA: 0x004FDD96 File Offset: 0x004FBF96
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600D73F RID: 55103 RVA: 0x004FDDA0 File Offset: 0x004FBFA0
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D740 RID: 55104 RVA: 0x004FDDAD File Offset: 0x004FBFAD
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), this.mapOf.GetValue(slate).MapHeld, false);
			}
		}

		// Token: 0x04008898 RID: 34968
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x04008899 RID: 34969
		public SlateRef<Thing> mapOf;
	}
}
