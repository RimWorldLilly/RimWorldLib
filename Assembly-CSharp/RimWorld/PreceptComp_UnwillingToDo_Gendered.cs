using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200163C RID: 5692
	public class PreceptComp_UnwillingToDo_Gendered : PreceptComp_UnwillingToDo
	{
		// Token: 0x06008E9D RID: 36509 RVA: 0x00346C40 File Offset: 0x00344E40
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			Pawn pawn;
			return !ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn) || pawn.gender != this.gender || base.MemberWillingToDo(ev);
		}

		// Token: 0x06008E9E RID: 36510 RVA: 0x00346C79 File Offset: 0x00344E79
		public override string GetProhibitionText()
		{
			if (!this.displayDescription)
			{
				return null;
			}
			return base.GetProhibitionText() + " (" + this.gender.GetLabel(false) + ")";
		}

		// Token: 0x04005768 RID: 22376
		public Gender gender;

		// Token: 0x04005769 RID: 22377
		public bool displayDescription = true;
	}
}
