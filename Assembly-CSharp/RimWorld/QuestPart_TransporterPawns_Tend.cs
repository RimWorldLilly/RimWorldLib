using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001107 RID: 4359
	public class QuestPart_TransporterPawns_Tend : QuestPart_TransporterPawns
	{
		// Token: 0x06006CC8 RID: 27848 RVA: 0x002624D4 File Offset: 0x002606D4
		public override void Process(Pawn pawn)
		{
			int num = 0;
			while (pawn.health.HasHediffsNeedingTend(false))
			{
				num++;
				if (num > 10000)
				{
					Log.Error("Too many iterations.");
					return;
				}
				this.DoTend(pawn);
			}
		}

		// Token: 0x06006CC9 RID: 27849 RVA: 0x00262511 File Offset: 0x00260711
		protected virtual void DoTend(Pawn pawn)
		{
			TendUtility.DoTend(null, pawn, null);
		}
	}
}
