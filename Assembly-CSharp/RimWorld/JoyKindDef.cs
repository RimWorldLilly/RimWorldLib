using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F81 RID: 3969
	public class JoyKindDef : Def
	{
		// Token: 0x06006597 RID: 26007 RVA: 0x00242DFC File Offset: 0x00240FFC
		public bool PawnCanDo(Pawn pawn)
		{
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (royalTitle.conceited && royalTitle.def.JoyKindDisabled(this))
					{
						return false;
					}
				}
				if (this.titleRequiredAny == null)
				{
					return true;
				}
				bool flag = false;
				foreach (RoyalTitle royalTitle2 in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (this.titleRequiredAny.Contains(royalTitle2.def))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x04003C4E RID: 15438
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x04003C4F RID: 15439
		public bool needsThing = true;
	}
}
