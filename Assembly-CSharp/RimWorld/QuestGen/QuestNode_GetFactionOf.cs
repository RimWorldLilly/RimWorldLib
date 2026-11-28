using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021BF RID: 8639
	public class QuestNode_GetFactionOf : QuestNode
	{
		// Token: 0x0600D724 RID: 55076 RVA: 0x004FD73B File Offset: 0x004FB93B
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600D725 RID: 55077 RVA: 0x004FD745 File Offset: 0x004FB945
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D726 RID: 55078 RVA: 0x004FD754 File Offset: 0x004FB954
		private void DoWork(Slate slate)
		{
			Faction var = null;
			Thing value = this.thing.GetValue(slate);
			if (value != null)
			{
				var = value.Faction;
			}
			slate.Set<Faction>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04008884 RID: 34948
		public SlateRef<Thing> thing;

		// Token: 0x04008885 RID: 34949
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
