using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005DB RID: 1499
	public class Dialog_PawnTableTest : Window
	{
		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002F96 RID: 12182 RVA: 0x001315ED File Offset: 0x0012F7ED
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002F97 RID: 12183 RVA: 0x00131600 File Offset: 0x0012F800
		private List<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x00131616 File Offset: 0x0012F816
		public Dialog_PawnTableTest(PawnColumnDef singleColumn) : base(null)
		{
			this.singleColumn = singleColumn;
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x00131628 File Offset: 0x0012F828
		public override void DoWindowContents(Rect inRect)
		{
			int num = ((int)inRect.height - 90) / 3;
			PawnTableDef pawnTableDef = new PawnTableDef();
			pawnTableDef.columns = new List<PawnColumnDef>
			{
				this.singleColumn
			};
			pawnTableDef.minWidth = 0;
			if (this.pawnTableMin == null)
			{
				this.pawnTableMin = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableMin.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetMinWidth(this.pawnTableMin) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetMinWidth(this.pawnTableMin) + 16, (int)inRect.width), 0, num);
			}
			if (this.pawnTableOptimal == null)
			{
				this.pawnTableOptimal = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableOptimal.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetOptimalWidth(this.pawnTableOptimal) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetOptimalWidth(this.pawnTableOptimal) + 16, (int)inRect.width), 0, num);
			}
			if (this.pawnTableMax == null)
			{
				this.pawnTableMax = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableMax.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetMaxWidth(this.pawnTableMax) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetMaxWidth(this.pawnTableMax) + 16, (int)inRect.width), 0, num);
			}
			int num2 = 0;
			Text.Font = GameFont.Small;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Min size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableMin.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Optimal size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableOptimal.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Max size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableMax.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x001318E4 File Offset: 0x0012FAE4
		[DebugOutput("UI", false)]
		private static void PawnColumnTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<PawnColumnDef> allDefsListForReading = DefDatabase<PawnColumnDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				PawnColumnDef localDef = allDefsListForReading[i];
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_PawnTableTest(localDef));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x04001E4A RID: 7754
		private PawnColumnDef singleColumn;

		// Token: 0x04001E4B RID: 7755
		private PawnTable pawnTableMin;

		// Token: 0x04001E4C RID: 7756
		private PawnTable pawnTableOptimal;

		// Token: 0x04001E4D RID: 7757
		private PawnTable pawnTableMax;

		// Token: 0x04001E4E RID: 7758
		private const int TableTitleHeight = 30;
	}
}
