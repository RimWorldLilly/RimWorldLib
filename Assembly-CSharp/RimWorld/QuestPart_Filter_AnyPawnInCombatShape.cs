using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106A RID: 4202
	public class QuestPart_Filter_AnyPawnInCombatShape : QuestPart_Filter_AnyPawn
	{
		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06006981 RID: 27009 RVA: 0x002534E4 File Offset: 0x002516E4
		protected override int PawnsCount
		{
			get
			{
				if (this.pawns.NullOrEmpty<Pawn>())
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Destroyed && !this.pawns[i].Downed)
					{
						num++;
					}
				}
				return num;
			}
		}
	}
}
