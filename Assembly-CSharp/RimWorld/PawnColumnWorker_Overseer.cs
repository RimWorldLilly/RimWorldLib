using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC5 RID: 7621
	public class PawnColumnWorker_Overseer : PawnColumnWorker_Label
	{
		// Token: 0x170024BA RID: 9402
		// (get) Token: 0x0600C608 RID: 50696 RVA: 0x00489EF0 File Offset: 0x004880F0
		protected override TextAnchor LabelAlignment
		{
			get
			{
				return TextAnchor.MiddleCenter;
			}
		}

		// Token: 0x0600C609 RID: 50697 RVA: 0x00489EF4 File Offset: 0x004880F4
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Pawn overseer = pawn.GetOverseer();
			if (overseer == null)
			{
				return;
			}
			base.DoCell(rect, overseer, table);
		}

		// Token: 0x0600C60A RID: 50698 RVA: 0x00489F18 File Offset: 0x00488118
		public override bool CanGroupWith(Pawn pawn, Pawn other)
		{
			Pawn overseer = pawn.GetOverseer();
			return overseer != null && other.GetOverseer() == overseer;
		}
	}
}
