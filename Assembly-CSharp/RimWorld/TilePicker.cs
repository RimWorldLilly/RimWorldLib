using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001E24 RID: 7716
	[StaticConstructorOnStartup]
	public class TilePicker
	{
		// Token: 0x17002505 RID: 9477
		// (get) Token: 0x0600C87A RID: 51322 RVA: 0x00498481 File Offset: 0x00496681
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17002506 RID: 9478
		// (get) Token: 0x0600C87B RID: 51323 RVA: 0x00498489 File Offset: 0x00496689
		public bool AllowEscape
		{
			get
			{
				return this.allowEscape;
			}
		}

		// Token: 0x17002507 RID: 9479
		// (get) Token: 0x0600C87C RID: 51324 RVA: 0x00498491 File Offset: 0x00496691
		public bool ForGravship
		{
			get
			{
				return this.forGravship;
			}
		}

		// Token: 0x17002508 RID: 9480
		// (get) Token: 0x0600C87D RID: 51325 RVA: 0x00498499 File Offset: 0x00496699
		public PlanetTile ClosestLayerTile
		{
			get
			{
				return this.closestLayerTile;
			}
		}

		// Token: 0x0600C87E RID: 51326 RVA: 0x004984A4 File Offset: 0x004966A4
		[Obsolete]
		public void StartTargeting(Func<PlanetTile, bool> validator, Action<PlanetTile> tileChosen, Action onGuiAction = null, Action onUpdateAction = null, bool allowEscape = true, Action noTileChosen = null, string title = null, bool showRandomButton = true, bool selectTileBehindObject = false, bool hideFormCaravanGizmo = false, bool canCancel = false, string noTileChosenMessage = null)
		{
			this.StartTargeting_NewTemp(validator, tileChosen, onGuiAction, onUpdateAction, allowEscape, noTileChosen, title, showRandomButton, selectTileBehindObject, hideFormCaravanGizmo, true, canCancel, noTileChosenMessage);
		}

		// Token: 0x0600C87F RID: 51327 RVA: 0x004984D0 File Offset: 0x004966D0
		public void StartTargeting_NewTemp(Func<PlanetTile, bool> validator, Action<PlanetTile> tileChosen, Action onGuiAction = null, Action onUpdateAction = null, bool allowEscape = true, Action noTileChosen = null, string title = null, bool showRandomButton = true, bool selectTileBehindObject = false, bool hideFormCaravanGizmo = false, bool showNextButton = true, bool canCancel = false, string noTileChosenMessage = null)
		{
			this.validator = validator;
			this.allowEscape = allowEscape;
			this.noTileChosen = noTileChosen;
			this.tileChosen = tileChosen;
			this.title = title;
			this.showRandomButton = showRandomButton;
			this.onGuiAction = onGuiAction;
			this.onUpdateAction = onUpdateAction;
			this.selectTileBehindObject = selectTileBehindObject;
			this.forGravship = hideFormCaravanGizmo;
			this.showNextButton = showNextButton;
			this.canCancel = canCancel;
			this.noTileChosenMessage = (noTileChosenMessage ?? "MustSelectStartingSite".Translate());
			Find.WorldSelector.ClearSelection();
			this.active = true;
		}

		// Token: 0x0600C880 RID: 51328 RVA: 0x00498566 File Offset: 0x00496766
		public void StopTargeting()
		{
			if (this.active && this.noTileChosen != null)
			{
				this.noTileChosen();
			}
			this.StopTargetingInt();
		}

		// Token: 0x0600C881 RID: 51329 RVA: 0x00498589 File Offset: 0x00496789
		private void StopTargetingInt()
		{
			this.forGravship = false;
			this.active = false;
			this.closestLayerTile = PlanetTile.Invalid;
		}

		// Token: 0x0600C882 RID: 51330 RVA: 0x004985A4 File Offset: 0x004967A4
		public void TileSelectorOnGUI()
		{
			if (!this.title.NullOrEmpty())
			{
				Text.Font = GameFont.Medium;
				Vector2 vector = Text.CalcSize(this.title);
				Widgets.Label(new Rect((float)UI.screenWidth / 2f - vector.x / 2f, 4f, vector.x + 4f, vector.y), this.title);
				Text.Font = GameFont.Small;
			}
			Action action = this.onGuiAction;
			if (action != null)
			{
				action();
			}
			Vector2 buttonSize = TilePicker.ButtonSize;
			int num = 0;
			if (this.showNextButton)
			{
				num++;
			}
			if (this.showRandomButton)
			{
				num++;
			}
			if (this.canCancel)
			{
				num++;
			}
			int num2 = (num + 1) * 8;
			Rect rect = new Rect((float)UI.screenWidth / 2f - (float)num * buttonSize.x / 2f - (float)num2 / 2f, (float)UI.screenHeight - (buttonSize.y + 8f) + -50f, (float)num * buttonSize.x + (float)num2, buttonSize.y + 16f);
			Widgets.DrawWindowBackground(rect);
			float num3 = rect.x + 8f;
			if (this.canCancel)
			{
				if (Widgets.ButtonText(new Rect(num3, rect.y + 8f, buttonSize.x, buttonSize.y), "Cancel".Translate(), true, true, true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.StopTargeting();
				}
				num3 += buttonSize.x + 2f + 8f;
			}
			if (this.showRandomButton)
			{
				if (Widgets.ButtonText(new Rect(num3, rect.y + 8f, buttonSize.x, buttonSize.y), "SelectRandomSite".Translate(), true, true, true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
					Find.WorldCameraDriver.JumpTo(Find.WorldGrid.GetTileCenter(Find.WorldInterface.SelectedTile));
				}
				num3 += buttonSize.x + 2f + 8f;
			}
			if ((this.showNextButton && Widgets.ButtonText(new Rect(num3, rect.y + 8f, buttonSize.x, buttonSize.y), "Next".Translate(), true, true, true, null)) || KeyBindingDefOf.Accept.KeyDownEvent)
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				PlanetTile planetTile = Find.WorldInterface.SelectedTile;
				if (!planetTile.Valid)
				{
					if (this.selectTileBehindObject)
					{
						WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
						if (singleSelectedObject != null && singleSelectedObject.Tile.Valid)
						{
							planetTile = singleSelectedObject.Tile;
							if (!planetTile.Valid)
							{
								Messages.Message(this.noTileChosenMessage, MessageTypeDefOf.RejectInput, false);
							}
							else if (this.validator(planetTile))
							{
								this.StopTargetingInt();
								this.tileChosen(planetTile);
								Event.current.Use();
							}
						}
						else
						{
							Messages.Message(this.noTileChosenMessage, MessageTypeDefOf.RejectInput, false);
						}
					}
					else
					{
						Messages.Message(this.noTileChosenMessage, MessageTypeDefOf.RejectInput, false);
					}
				}
				else if (this.validator(planetTile))
				{
					this.StopTargetingInt();
					this.tileChosen(planetTile);
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.Active && !this.allowEscape)
			{
				Event.current.Use();
			}
		}

		// Token: 0x0600C883 RID: 51331 RVA: 0x00498957 File Offset: 0x00496B57
		public void TileSelectorUpdate()
		{
			Action action = this.onUpdateAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0400721B RID: 29211
		private static readonly Vector2 ButtonSize = new Vector2(150f, 38f);

		// Token: 0x0400721C RID: 29212
		private const int Padding = 8;

		// Token: 0x0400721D RID: 29213
		private const int BottomPanelYOffset = -50;

		// Token: 0x0400721E RID: 29214
		private Func<PlanetTile, bool> validator;

		// Token: 0x0400721F RID: 29215
		private bool allowEscape;

		// Token: 0x04007220 RID: 29216
		private bool active;

		// Token: 0x04007221 RID: 29217
		private Action<PlanetTile> tileChosen;

		// Token: 0x04007222 RID: 29218
		private Action noTileChosen;

		// Token: 0x04007223 RID: 29219
		private Action onGuiAction;

		// Token: 0x04007224 RID: 29220
		private Action onUpdateAction;

		// Token: 0x04007225 RID: 29221
		private string title;

		// Token: 0x04007226 RID: 29222
		private bool showRandomButton = true;

		// Token: 0x04007227 RID: 29223
		private bool selectTileBehindObject;

		// Token: 0x04007228 RID: 29224
		private bool forGravship;

		// Token: 0x04007229 RID: 29225
		private bool showNextButton = true;

		// Token: 0x0400722A RID: 29226
		private bool canCancel;

		// Token: 0x0400722B RID: 29227
		private PlanetTile closestLayerTile = PlanetTile.Invalid;

		// Token: 0x0400722C RID: 29228
		private string noTileChosenMessage;
	}
}
