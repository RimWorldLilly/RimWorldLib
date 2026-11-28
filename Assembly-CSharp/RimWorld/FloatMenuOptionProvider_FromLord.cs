using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D2B RID: 7467
	public class FloatMenuOptionProvider_FromLord : FloatMenuOptionProvider
	{
		// Token: 0x17002365 RID: 9061
		// (get) Token: 0x0600C140 RID: 49472 RVA: 0x004671E1 File Offset: 0x004653E1
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002366 RID: 9062
		// (get) Token: 0x0600C141 RID: 49473 RVA: 0x004671E4 File Offset: 0x004653E4
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002367 RID: 9063
		// (get) Token: 0x0600C142 RID: 49474 RVA: 0x004671E7 File Offset: 0x004653E7
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C143 RID: 49475 RVA: 0x004671EC File Offset: 0x004653EC
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			Lord lord = clickedPawn.GetLord();
			if (lord == null)
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
			return lord.CurLordToil.ExtraFloatMenuOptions(clickedPawn, context.FirstSelectedPawn);
		}
	}
}
