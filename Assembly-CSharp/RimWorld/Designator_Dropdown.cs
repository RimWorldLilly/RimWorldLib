using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C44 RID: 7236
	[StaticConstructorOnStartup]
	public class Designator_Dropdown : DesignatorWithEyedropper
	{
		// Token: 0x1700214B RID: 8523
		// (get) Token: 0x0600B86F RID: 47215 RVA: 0x004113D5 File Offset: 0x0040F5D5
		public override string Label
		{
			get
			{
				return this.activeDesignator.Label + (this.activeDesignatorSet ? "" : "...");
			}
		}

		// Token: 0x1700214C RID: 8524
		// (get) Token: 0x0600B870 RID: 47216 RVA: 0x004113FB File Offset: 0x0040F5FB
		public override string Desc
		{
			get
			{
				return this.activeDesignator.Desc;
			}
		}

		// Token: 0x1700214D RID: 8525
		// (get) Token: 0x0600B871 RID: 47217 RVA: 0x00411408 File Offset: 0x0040F608
		public override Color IconDrawColor
		{
			get
			{
				return this.activeDesignator.IconDrawColor;
			}
		}

		// Token: 0x1700214E RID: 8526
		// (get) Token: 0x0600B872 RID: 47218 RVA: 0x00411418 File Offset: 0x0040F618
		public override bool Visible
		{
			get
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					if (this.elements[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700214F RID: 8527
		// (get) Token: 0x0600B873 RID: 47219 RVA: 0x00411451 File Offset: 0x0040F651
		public List<Designator> Elements
		{
			get
			{
				return this.elements;
			}
		}

		// Token: 0x17002150 RID: 8528
		// (get) Token: 0x0600B874 RID: 47220 RVA: 0x00411459 File Offset: 0x0040F659
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return this.activeDesignator.PanelReadoutTitleExtraRightMargin;
			}
		}

		// Token: 0x0600B875 RID: 47221 RVA: 0x00411466 File Offset: 0x0040F666
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			return result;
		}

		// Token: 0x0600B876 RID: 47222 RVA: 0x0041147E File Offset: 0x0040F67E
		public static void DrawExtraOptionsIcon(Vector2 topLeft, float width)
		{
			GUI.DrawTexture(new Rect(topLeft.x + width - 16f - 1f, topLeft.y + 1f, 16f, 16f), Designator_Dropdown.PlusTex);
		}

		// Token: 0x0600B877 RID: 47223 RVA: 0x004114B9 File Offset: 0x0040F6B9
		public void Add(Designator des)
		{
			this.elements.Add(des);
			if (this.activeDesignator == null)
			{
				this.SetActiveDesignator(des, false);
			}
		}

		// Token: 0x0600B878 RID: 47224 RVA: 0x004114D8 File Offset: 0x0040F6D8
		public void SetActiveDesignator(Designator des, bool explicitySet = true)
		{
			this.activeDesignator = des;
			this.icon = des.icon;
			this.iconDrawScale = des.iconDrawScale;
			this.iconProportions = des.iconProportions;
			this.iconTexCoords = des.iconTexCoords;
			this.iconAngle = des.iconAngle;
			this.iconOffset = des.iconOffset;
			if (explicitySet)
			{
				this.activeDesignatorSet = true;
			}
		}

		// Token: 0x0600B879 RID: 47225 RVA: 0x0041153E File Offset: 0x0040F73E
		public override void DrawMouseAttachments()
		{
			this.activeDesignator.DrawMouseAttachments();
		}

		// Token: 0x0600B87A RID: 47226 RVA: 0x0041154C File Offset: 0x0040F74C
		public override void ProcessInput(Event ev)
		{
			Window window = this.elements.Any(delegate(Designator x)
			{
				Designator_Place designator_Place = x as Designator_Place;
				if (designator_Place != null)
				{
					BuildableDef placingDef = designator_Place.PlacingDef;
					if (placingDef != null)
					{
						return placingDef.designatorDropdown.useGridMenu;
					}
				}
				return false;
			}) ? this.SetupGridMenu(ev) : this.SetupFloatMenu(ev);
			Find.WindowStack.Add(window);
			Find.DesignatorManager.Select(this.activeDesignator);
		}

		// Token: 0x0600B87B RID: 47227 RVA: 0x004115B4 File Offset: 0x0040F7B4
		private Window SetupGridMenu(Event ev)
		{
			List<FloatMenuGridOption> list = new List<FloatMenuGridOption>();
			if (this.elements.Any(delegate(Designator x)
			{
				Designator_Place designator_Place2 = x as Designator_Place;
				if (designator_Place2 != null)
				{
					TerrainDef terrainDef = designator_Place2.PlacingDef as TerrainDef;
					if (terrainDef != null)
					{
						return terrainDef.designatorDropdown.includeEyeDropperTool;
					}
				}
				return false;
			}))
			{
				if (this.eyedropper == null)
				{
					this.eyedropper = new Designator_Eyedropper(delegate(ColorDef newCol)
					{
						for (int j = 0; j < this.elements.Count; j++)
						{
							Designator designator2 = this.elements[j];
							Designator_Place designator_Place2 = designator2 as Designator_Place;
							if (designator_Place2 != null)
							{
								TerrainDef terrainDef = designator_Place2.PlacingDef as TerrainDef;
								if (terrainDef != null && terrainDef.colorDef == newCol)
								{
									this.GetDesignatorSelectAction(ev, designator2)();
									return;
								}
							}
						}
					}, "SelectColoredFloor".Translate(), "DesignatorEyeDropperDesc_Carpet".Translate());
				}
				List<FloatMenuGridOption> list2 = list;
				Texture2D eyeDropperTex = Designator_Eyedropper.EyeDropperTex;
				Action action = delegate()
				{
					Find.DesignatorManager.Select(this.eyedropper);
				};
				TipSignal? tooltip = new TipSignal?("DesignatorEyeDropperDesc_Carpet".Translate());
				list2.Add(new FloatMenuGridOption(eyeDropperTex, action, null, tooltip));
			}
			for (int i = 0; i < this.elements.Count; i++)
			{
				Designator designator = this.elements[i];
				if (designator.Visible)
				{
					Designator_Place designator_Place = designator as Designator_Place;
					if (designator_Place != null)
					{
						BuildableDef placingDef = designator_Place.PlacingDef;
						if (placingDef != null)
						{
							if (placingDef.designatorDropdown.iconSource == DesignatorDropdownGroupDef.IconSource.Cost)
							{
								ThingDef designatorCost = this.GetDesignatorCost(designator);
								if (designatorCost != null)
								{
									List<FloatMenuGridOption> list3 = list;
									Texture2D iconFor = Widgets.GetIconFor(designatorCost, null, null, null);
									Action designatorSelectAction = this.GetDesignatorSelectAction(ev, designator);
									TipSignal? tooltip = new TipSignal?(designator.LabelCap);
									list3.Add(new FloatMenuGridOption(iconFor, designatorSelectAction, null, tooltip));
									goto IL_1EE;
								}
							}
							if (placingDef.designatorDropdown.iconSource == DesignatorDropdownGroupDef.IconSource.Placed)
							{
								FloatMenuGridOption floatMenuGridOption = new FloatMenuGridOption((Texture2D)designator_Place.icon, this.GetDesignatorSelectAction(ev, designator), new Color?(designator_Place.IconDrawColor), new TipSignal?(designator.LabelCap));
								if (placingDef is TerrainDef)
								{
									floatMenuGridOption.iconTexCoords = Widgets.CroppedTerrainTextureRect((Texture2D)designator_Place.icon);
								}
								list.Add(floatMenuGridOption);
								goto IL_1EE;
							}
							goto IL_1EE;
						}
					}
					Log.Error("Trying to setup grid float menu with designator without icon.");
				}
				IL_1EE:;
			}
			return new FloatMenuGrid(list)
			{
				onCloseCallback = delegate()
				{
					this.activeDesignatorSet = true;
				}
			};
		}

		// Token: 0x0600B87C RID: 47228 RVA: 0x004117E0 File Offset: 0x0040F9E0
		private Window SetupFloatMenu(Event ev)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			for (int i = 0; i < this.elements.Count; i++)
			{
				Designator designator = this.elements[i];
				if (designator.Visible)
				{
					Designator_Place designator_Place = designator as Designator_Place;
					if (designator_Place != null)
					{
						BuildableDef placingDef = designator_Place.PlacingDef;
						if (placingDef != null)
						{
							if (placingDef.designatorDropdown.iconSource == DesignatorDropdownGroupDef.IconSource.Cost)
							{
								ThingDef designatorCost = this.GetDesignatorCost(designator);
								if (designatorCost != null)
								{
									list.Add(new FloatMenuOption(designator.LabelCap, this.GetDesignatorSelectAction(ev, designator), designatorCost, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
									goto IL_126;
								}
							}
							if (placingDef.designatorDropdown.iconSource == DesignatorDropdownGroupDef.IconSource.Placed)
							{
								FloatMenuOption floatMenuOption = new FloatMenuOption(designator.LabelCap, this.GetDesignatorSelectAction(ev, designator), (Texture2D)designator_Place.icon, designator_Place.IconDrawColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
								if (placingDef is TerrainDef)
								{
									floatMenuOption.iconTexCoords = Widgets.CroppedTerrainTextureRect((Texture2D)designator_Place.icon);
								}
								list.Add(floatMenuOption);
								goto IL_126;
							}
						}
					}
					list.Add(new FloatMenuOption(designator.LabelCap, this.GetDesignatorSelectAction(ev, designator), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				IL_126:;
			}
			return new FloatMenu(list)
			{
				onCloseCallback = delegate()
				{
					this.activeDesignatorSet = true;
				}
			};
		}

		// Token: 0x0600B87D RID: 47229 RVA: 0x00411940 File Offset: 0x0040FB40
		private Action GetDesignatorSelectAction(Event ev, Designator des)
		{
			return delegate()
			{
				this.<>n__0(ev);
				Find.DesignatorManager.Select(des);
				this.SetActiveDesignator(des, true);
			};
		}

		// Token: 0x0600B87E RID: 47230 RVA: 0x00411967 File Offset: 0x0040FB67
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			Designator designator = this.activeDesignator;
			if (designator == null)
			{
				return;
			}
			designator.DrawIcon(rect, buttonMat, parms);
		}

		// Token: 0x0600B87F RID: 47231 RVA: 0x0041197C File Offset: 0x0040FB7C
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.activeDesignator.CanDesignateCell(loc);
		}

		// Token: 0x0600B880 RID: 47232 RVA: 0x0041198A File Offset: 0x0040FB8A
		public override void SelectedUpdate()
		{
			this.activeDesignator.SelectedUpdate();
		}

		// Token: 0x0600B881 RID: 47233 RVA: 0x00411997 File Offset: 0x0040FB97
		public override void DrawPanelReadout(ref float curY, float width)
		{
			this.activeDesignator.DrawPanelReadout(ref curY, width);
		}

		// Token: 0x0600B882 RID: 47234 RVA: 0x004119A8 File Offset: 0x0040FBA8
		private ThingDef GetDesignatorCost(Designator des)
		{
			Designator_Place designator_Place = des as Designator_Place;
			if (designator_Place != null)
			{
				BuildableDef placingDef = designator_Place.PlacingDef;
				if (placingDef.CostList != null && placingDef.CostList.Count > 0)
				{
					return placingDef.CostList.MaxBy((ThingDefCountClass c) => c.thingDef.BaseMarketValue * (float)c.count).thingDef;
				}
			}
			return null;
		}

		// Token: 0x040069A9 RID: 27049
		private List<Designator> elements = new List<Designator>();

		// Token: 0x040069AA RID: 27050
		private Designator activeDesignator;

		// Token: 0x040069AB RID: 27051
		private bool activeDesignatorSet;

		// Token: 0x040069AC RID: 27052
		public static readonly Texture2D PlusTex = ContentFinder<Texture2D>.Get("UI/Widgets/PlusOptions", true);

		// Token: 0x040069AD RID: 27053
		private const float ButSize = 16f;

		// Token: 0x040069AE RID: 27054
		private const float ButPadding = 1f;
	}
}
