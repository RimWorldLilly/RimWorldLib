using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC1 RID: 7617
	public class PawnColumnWorker_AutoRepair : PawnColumnWorker
	{
		// Token: 0x0600C5F3 RID: 50675 RVA: 0x00489B04 File Offset: 0x00487D04
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction == Faction.OfPlayer && pawn.RaceProps.IsMechanoid && pawn.GetOverseer() == null)
			{
				return;
			}
			CompMechRepairable comp = pawn.GetComp<CompMechRepairable>();
			if (comp == null)
			{
				return;
			}
			rect.xMin += (rect.width - 24f) / 2f;
			rect.yMin += (rect.height - 24f) / 2f;
			Widgets.Checkbox(rect.position, ref comp.autoRepair, 24f, false, this.def.paintable, null, null);
		}

		// Token: 0x0600C5F4 RID: 50676 RVA: 0x00489BA6 File Offset: 0x00487DA6
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x0600C5F5 RID: 50677 RVA: 0x00489BB6 File Offset: 0x00487DB6
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600C5F6 RID: 50678 RVA: 0x00489BCB File Offset: 0x00487DCB
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}
	}
}
