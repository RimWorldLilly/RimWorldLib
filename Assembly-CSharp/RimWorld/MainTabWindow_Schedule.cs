using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA2 RID: 7586
	public class MainTabWindow_Schedule : MainTabWindow_PawnTable
	{
		// Token: 0x170024A3 RID: 9379
		// (get) Token: 0x0600C53D RID: 50493 RVA: 0x00487D98 File Offset: 0x00485F98
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x170024A4 RID: 9380
		// (get) Token: 0x0600C53E RID: 50494 RVA: 0x00487D9F File Offset: 0x00485F9F
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from pawn in base.Pawns.Concat(Find.CurrentMap.mapPawns.ColonySubhumansControllable)
				where !pawn.DevelopmentalStage.Baby()
				select pawn;
			}
		}

		// Token: 0x0600C53F RID: 50495 RVA: 0x00487DDF File Offset: 0x00485FDF
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(new Rect(0f, 0f, 191f, 65f));
		}

		// Token: 0x040070CD RID: 28877
		private const int TimeAssignmentSelectorWidth = 191;

		// Token: 0x040070CE RID: 28878
		private const int TimeAssignmentSelectorHeight = 65;
	}
}
