using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA4 RID: 7588
	public class MainTabWindow_Work : MainTabWindow_PawnTable
	{
		// Token: 0x170024A7 RID: 9383
		// (get) Token: 0x0600C544 RID: 50500 RVA: 0x00487E52 File Offset: 0x00486052
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Work;
			}
		}

		// Token: 0x170024A8 RID: 9384
		// (get) Token: 0x0600C545 RID: 50501 RVA: 0x00487E59 File Offset: 0x00486059
		protected override float ExtraTopSpace
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x170024A9 RID: 9385
		// (get) Token: 0x0600C546 RID: 50502 RVA: 0x00487E60 File Offset: 0x00486060
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from pawn in base.Pawns
				where !pawn.DevelopmentalStage.Baby()
				select pawn;
			}
		}

		// Token: 0x0600C547 RID: 50503 RVA: 0x00487E8C File Offset: 0x0048608C
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			if (Event.current.type == EventType.layout)
			{
				return;
			}
			this.DoManualPrioritiesCheckbox();
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Tiny;
			Widgets.Label(new Rect(370f, rect.y + 5f, 160f, 30f), "<= " + "HigherPriority".Translate());
			Widgets.Label(new Rect(630f, rect.y + 5f, 160f, 30f), "LowerPriority".Translate() + " =>");
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600C548 RID: 50504 RVA: 0x00487F68 File Offset: 0x00486168
		private void DoManualPrioritiesCheckbox()
		{
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect = new Rect(5f, 5f, 140f, 30f);
			bool useWorkPriorities = Current.Game.playSettings.useWorkPriorities;
			Widgets.CheckboxLabeled(rect, "ManualPriorities".Translate(), ref Current.Game.playSettings.useWorkPriorities, false, null, null, false, false);
			if (useWorkPriorities != Current.Game.playSettings.useWorkPriorities)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive)
				{
					if (pawn.Faction == Faction.OfPlayer && pawn.workSettings != null)
					{
						pawn.workSettings.Notify_UseWorkPrioritiesChanged();
					}
				}
			}
			if (Current.Game.playSettings.useWorkPriorities)
			{
				using (new TextBlock(new Color(1f, 1f, 1f, 0.5f)))
				{
					Widgets.Label(new Rect(rect.x, rect.yMax - 6f, rect.width, 60f), "PriorityOneDoneFirst".Translate());
				}
			}
			if (!Current.Game.playSettings.useWorkPriorities)
			{
				UIHighlighter.HighlightOpportunity(rect, "ManualPriorities-Off");
			}
		}

		// Token: 0x040070CF RID: 28879
		private const int SpaceBetweenPriorityArrowsAndWorkLabels = 40;
	}
}
