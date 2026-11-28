using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001069 RID: 4201
	public class QuestPart_Filter_AnyPawnAlive : QuestPart_Filter_AnyPawn
	{
		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x0600697F RID: 27007 RVA: 0x00253490 File Offset: 0x00251690
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
					if (!this.pawns[i].Destroyed)
					{
						num++;
					}
				}
				return num;
			}
		}
	}
}
