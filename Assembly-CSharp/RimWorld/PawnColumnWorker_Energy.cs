using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC4 RID: 7620
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Energy : PawnColumnWorker
	{
		// Token: 0x0600C603 RID: 50691 RVA: 0x00489DF8 File Offset: 0x00487FF8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.IsGestating())
			{
				return;
			}
			Widgets.FillableBar(rect.ContractedBy(4f), pawn.needs.energy.CurLevelPercentage, PawnColumnWorker_Energy.EnergyBarTex, BaseContent.ClearTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, Mathf.RoundToInt(pawn.needs.energy.CurLevel).ToString() + " / " + Mathf.RoundToInt(pawn.needs.energy.MaxLevel).ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		// Token: 0x0600C604 RID: 50692 RVA: 0x00489E9C File Offset: 0x0048809C
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 120);
		}

		// Token: 0x0600C605 RID: 50693 RVA: 0x00489EAC File Offset: 0x004880AC
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x040070E8 RID: 28904
		private const int Width = 120;

		// Token: 0x040070E9 RID: 28905
		private const int BarPadding = 4;

		// Token: 0x040070EA RID: 28906
		public static readonly Texture2D EnergyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(252, byte.MaxValue, byte.MaxValue, 65));
	}
}
