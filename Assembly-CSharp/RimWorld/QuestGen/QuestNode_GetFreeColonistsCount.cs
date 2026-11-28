using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021BA RID: 8634
	public class QuestNode_GetFreeColonistsCount : QuestNode
	{
		// Token: 0x0600D70E RID: 55054 RVA: 0x004FD0A1 File Offset: 0x004FB2A1
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D70F RID: 55055 RVA: 0x004FD0AB File Offset: 0x004FB2AB
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D710 RID: 55056 RVA: 0x004FD0B8 File Offset: 0x004FB2B8
		private void SetVars(Slate slate)
		{
			int var;
			if (this.onlyThisMap.GetValue(slate) != null)
			{
				var = this.onlyThisMap.GetValue(slate).mapPawns.FreeColonistsCount;
			}
			else
			{
				var = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04008869 RID: 34921
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400886A RID: 34922
		public SlateRef<Map> onlyThisMap;
	}
}
