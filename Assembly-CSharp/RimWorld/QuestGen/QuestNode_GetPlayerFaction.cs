using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D2 RID: 8658
	public class QuestNode_GetPlayerFaction : QuestNode
	{
		// Token: 0x0600D77A RID: 55162 RVA: 0x004FFCF4 File Offset: 0x004FDEF4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<Faction>(this.storeAs.GetValue(slate), Faction.OfPlayer, false);
		}

		// Token: 0x0600D77B RID: 55163 RVA: 0x004FFD1F File Offset: 0x004FDF1F
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<Faction>(this.storeAs.GetValue(slate), Faction.OfPlayer, false);
			return true;
		}

		// Token: 0x040088E5 RID: 35045
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
