using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023FA RID: 9210
	public class WorldInspectPane : Window, IInspectPane
	{
		// Token: 0x170028A6 RID: 10406
		// (get) Token: 0x0600E634 RID: 58932 RVA: 0x0055A25D File Offset: 0x0055845D
		// (set) Token: 0x0600E635 RID: 58933 RVA: 0x0055A265 File Offset: 0x00558465
		public Type OpenTabType
		{
			get
			{
				return this.openTabType;
			}
			set
			{
				this.openTabType = value;
			}
		}

		// Token: 0x170028A7 RID: 10407
		// (get) Token: 0x0600E636 RID: 58934 RVA: 0x0055A26E File Offset: 0x0055846E
		// (set) Token: 0x0600E637 RID: 58935 RVA: 0x0055A276 File Offset: 0x00558476
		public float RecentHeight { get; set; }

		// Token: 0x170028A8 RID: 10408
		// (get) Token: 0x0600E638 RID: 58936 RVA: 0x0055A27F File Offset: 0x0055847F
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170028A9 RID: 10409
		// (get) Token: 0x0600E639 RID: 58937 RVA: 0x0055A286 File Offset: 0x00558486
		public override Vector2 InitialSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x170028AA RID: 10410
		// (get) Token: 0x0600E63A RID: 58938 RVA: 0x0055A28E File Offset: 0x0055848E
		private List<WorldObject> Selected
		{
			get
			{
				return Find.WorldSelector.SelectedObjects;
			}
		}

		// Token: 0x170028AB RID: 10411
		// (get) Token: 0x0600E63B RID: 58939 RVA: 0x0055A29A File Offset: 0x0055849A
		private int NumSelectedObjects
		{
			get
			{
				return Find.WorldSelector.NumSelectedObjects;
			}
		}

		// Token: 0x170028AC RID: 10412
		// (get) Token: 0x0600E63C RID: 58940 RVA: 0x0055A2A8 File Offset: 0x005584A8
		public float PaneTopY
		{
			get
			{
				float num = (float)UI.screenHeight - 165f;
				if (Current.ProgramState == ProgramState.Playing)
				{
					num -= 35f;
				}
				return num;
			}
		}

		// Token: 0x170028AD RID: 10413
		// (get) Token: 0x0600E63D RID: 58941 RVA: 0x0055A2D3 File Offset: 0x005584D3
		public bool AnythingSelected
		{
			get
			{
				return Find.WorldSelector.AnyObjectOrTileSelected;
			}
		}

		// Token: 0x170028AE RID: 10414
		// (get) Token: 0x0600E63E RID: 58942 RVA: 0x0055A2DF File Offset: 0x005584DF
		private PlanetTile SelectedTile
		{
			get
			{
				return Find.WorldSelector.SelectedTile;
			}
		}

		// Token: 0x170028AF RID: 10415
		// (get) Token: 0x0600E63F RID: 58943 RVA: 0x0055A2EC File Offset: 0x005584EC
		private bool SelectedSingleObjectOrTile
		{
			get
			{
				return this.NumSelectedObjects == 1 || (this.NumSelectedObjects == 0 && this.SelectedTile.Valid);
			}
		}

		// Token: 0x170028B0 RID: 10416
		// (get) Token: 0x0600E640 RID: 58944 RVA: 0x0055A31C File Offset: 0x0055851C
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x170028B1 RID: 10417
		// (get) Token: 0x0600E641 RID: 58945 RVA: 0x0055A324 File Offset: 0x00558524
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x170028B2 RID: 10418
		// (get) Token: 0x0600E642 RID: 58946 RVA: 0x0055A32C File Offset: 0x0055852C
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (this.NumSelectedObjects == 1)
				{
					return Find.WorldSelector.SingleSelectedObject.GetInspectTabs();
				}
				if (this.NumSelectedObjects == 0 && this.SelectedTile.Valid)
				{
					return PlanetLayer.Selected.Def.Tabs;
				}
				return null;
			}
		}

		// Token: 0x170028B3 RID: 10419
		// (get) Token: 0x0600E643 RID: 58947 RVA: 0x0055A37C File Offset: 0x0055857C
		private string TileInspectString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				Vector2 vector = Find.WorldGrid.LongLatOf(this.SelectedTile);
				stringBuilder.Append(vector.y.ToStringLatitude());
				stringBuilder.Append(" ");
				stringBuilder.Append(vector.x.ToStringLongitude());
				Tile tile = Find.WorldGrid[this.SelectedTile];
				if (!tile.PrimaryBiome.impassable && tile.hilliness != Hilliness.Undefined)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(tile.hilliness.GetLabelCap());
				}
				SurfaceTile surfaceTile = tile as SurfaceTile;
				if (surfaceTile != null)
				{
					if (surfaceTile.Roads != null)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append((from rl in surfaceTile.Roads
						select rl.road).MaxBy((RoadDef road) => road.priority).LabelCap);
					}
					if (!Find.World.Impassable(this.SelectedTile))
					{
						string value = (WorldPathGrid.CalculatedMovementDifficultyAt(this.SelectedTile, false, null, null) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(this.SelectedTile, PlanetTile.Invalid, null)).ToString("0.#");
						stringBuilder.AppendLine();
						StringBuilder stringBuilder2 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("MovementDifficulty".Translate());
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted(value);
						stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				stringBuilder.AppendLine();
				StringBuilder stringBuilder3 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("AvgTemp".Translate());
				defaultInterpolatedStringHandler2.AppendLiteral(": ");
				defaultInterpolatedStringHandler2.AppendFormatted(GenTemperature.GetAverageTemperatureLabel(this.SelectedTile));
				stringBuilder3.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
				if (ModsConfig.BiotechActive && tile.pollution > 0f && tile.OnSurface)
				{
					stringBuilder.AppendLine();
					string text = GenWorld.GetPollutionDescription(tile.pollution);
					text = text + " (" + tile.pollution.ToStringPercent() + ")";
					StringBuilder stringBuilder4 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("TilePollution".Translate());
					defaultInterpolatedStringHandler3.AppendLiteral(": ");
					defaultInterpolatedStringHandler3.AppendFormatted(text);
					stringBuilder4.Append(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				if (ModsConfig.OdysseyActive && tile.Mutators.Any<TileMutatorDef>())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("TileMutators".Translate() + ": " + (from m in tile.Mutators
					orderby -m.displayPriority
					select m.Label(tile.tile)).ToCommaList(false, false).CapitalizeFirst());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600E644 RID: 58948 RVA: 0x0055A6CC File Offset: 0x005588CC
		public WorldInspectPane() : base(null)
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x0600E645 RID: 58949 RVA: 0x0055A706 File Offset: 0x00558906
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			this.windowRect.x = 0f;
			this.windowRect.y = this.PaneTopY;
		}

		// Token: 0x0600E646 RID: 58950 RVA: 0x0055A730 File Offset: 0x00558930
		public string GetLabel(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				return WorldInspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
			}
			if (!this.SelectedTile.Valid)
			{
				return "error";
			}
			if (ModsConfig.OdysseyActive && Find.World.landmarks[this.SelectedTile] != null)
			{
				return Find.World.landmarks[this.SelectedTile].name;
			}
			return Find.WorldGrid[this.SelectedTile].PrimaryBiome.LabelCap;
		}

		// Token: 0x0600E647 RID: 58951 RVA: 0x0055A7C1 File Offset: 0x005589C1
		public void SelectNextInCell()
		{
			if (!this.AnythingSelected)
			{
				return;
			}
			if (this.NumSelectedObjects > 0)
			{
				Find.WorldSelector.SelectFirstOrNextAt(this.Selected[0].Tile);
				return;
			}
			Find.WorldSelector.SelectFirstOrNextAt(this.SelectedTile);
		}

		// Token: 0x0600E648 RID: 58952 RVA: 0x0055A804 File Offset: 0x00558A04
		public void DoPaneContents(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				InspectPaneFiller.DoPaneContentsFor(Find.WorldSelector.FirstSelectedObject, rect);
				return;
			}
			if (this.SelectedTile.Valid)
			{
				InspectPaneFiller.DrawInspectString(this.TileInspectString, rect);
			}
		}

		// Token: 0x0600E649 RID: 58953 RVA: 0x0055A848 File Offset: 0x00558A48
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
			if (singleSelectedObject != null || this.SelectedTile.Valid)
			{
				float x = rect.width - 48f;
				if (singleSelectedObject != null)
				{
					Widgets.InfoCardButton(x, 0f, singleSelectedObject);
				}
				else if (Find.WorldGrid[this.SelectedTile].PrimaryBiome != null)
				{
					Widgets.InfoCardButton(x, 0f, Find.WorldGrid[this.SelectedTile].PrimaryBiome);
				}
				lineEndWidth += 24f;
			}
		}

		// Token: 0x0600E64A RID: 58954 RVA: 0x0055A8D5 File Offset: 0x00558AD5
		public override void DoWindowContents(Rect rect)
		{
			InspectPaneUtility.InspectPaneOnGUI(rect, this);
		}

		// Token: 0x0600E64B RID: 58955 RVA: 0x0055A8DE File Offset: 0x00558ADE
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
		}

		// Token: 0x0600E64C RID: 58956 RVA: 0x0055A8EC File Offset: 0x00558AEC
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
		}

		// Token: 0x0600E64D RID: 58957 RVA: 0x0055A8FA File Offset: 0x00558AFA
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x0600E64E RID: 58958 RVA: 0x0055A903 File Offset: 0x00558B03
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x04009259 RID: 37465
		private Type openTabType;
	}
}
