using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D95 RID: 7573
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		// Token: 0x1700247B RID: 9339
		// (get) Token: 0x0600C474 RID: 50292 RVA: 0x0047E60A File Offset: 0x0047C80A
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		// Token: 0x1700247C RID: 9340
		// (get) Token: 0x0600C475 RID: 50293 RVA: 0x0047E611 File Offset: 0x0047C811
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.ColonyAnimals;
			}
		}

		// Token: 0x0600C476 RID: 50294 RVA: 0x0047E624 File Offset: 0x0047C824
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y, Mathf.Min(rect.width, 260f), 32f), "ManageAutoSlaughter".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_AutoSlaughter(Find.CurrentMap));
			}
		}
	}
}
