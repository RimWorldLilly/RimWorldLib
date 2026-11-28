using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002237 RID: 8759
	public class QuestNode_SetChildCount : QuestNode
	{
		// Token: 0x0600D8D7 RID: 55511 RVA: 0x0050577C File Offset: 0x0050397C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int num = 0;
			using (IEnumerator<Pawn> enumerator = this.pawns.GetValue(slate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.DevelopmentalStage.Juvenile())
					{
						num++;
					}
				}
			}
			slate.Set<int>("childCount", num, false);
		}

		// Token: 0x0600D8D8 RID: 55512 RVA: 0x005057EC File Offset: 0x005039EC
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008A7C RID: 35452
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
