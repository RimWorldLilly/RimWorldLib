using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC3 RID: 7619
	public class PawnColumnWorker_DraftMech : PawnColumnWorker
	{
		// Token: 0x0600C5FE RID: 50686 RVA: 0x00489CC8 File Offset: 0x00487EC8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			if (!pawn.IsColonyMechPlayerControlled || !pawn.Spawned)
			{
				return;
			}
			AcceptanceReport report = MechanitorUtility.CanDraftMech(pawn);
			if (!report && report.Reason.NullOrEmpty())
			{
				return;
			}
			rect.xMin += (rect.width - 24f) / 2f;
			rect.yMin += (rect.height - 24f) / 2f;
			bool drafted = pawn.Drafted;
			Vector2 position = rect.position;
			float size = 24f;
			bool paintable = this.def.paintable;
			Widgets.Checkbox(position, ref drafted, size, !report, paintable, null, null);
			if (!report.Reason.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, report.Reason.CapitalizeFirst());
			}
			if (drafted != pawn.Drafted)
			{
				pawn.drafter.Drafted = drafted;
			}
		}

		// Token: 0x0600C5FF RID: 50687 RVA: 0x00489DBA File Offset: 0x00487FBA
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 44);
		}

		// Token: 0x0600C600 RID: 50688 RVA: 0x00489DCA File Offset: 0x00487FCA
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600C601 RID: 50689 RVA: 0x00489DDF File Offset: 0x00487FDF
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}
	}
}
