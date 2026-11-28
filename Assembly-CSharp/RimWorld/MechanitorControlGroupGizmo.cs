using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200155D RID: 5469
	public class MechanitorControlGroupGizmo : Gizmo
	{
		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x060085CC RID: 34252 RVA: 0x003054D9 File Offset: 0x003036D9
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				return MechanitorControlGroupGizmo.GetWorkModeOptions(this.controlGroup);
			}
		}

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x060085CD RID: 34253 RVA: 0x003054E6 File Offset: 0x003036E6
		public override bool Visible
		{
			get
			{
				return this.controlGroup.MechsForReading.Count > 0 || Find.Selector.SelectedPawns.Count == 1;
			}
		}

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x060085CE RID: 34254 RVA: 0x0030550F File Offset: 0x0030370F
		public override float Order
		{
			get
			{
				if (this.controlGroup.MechsForReading.Count > 0)
				{
					return -89f;
				}
				return -88f;
			}
		}

		// Token: 0x060085CF RID: 34255 RVA: 0x0030552F File Offset: 0x0030372F
		public MechanitorControlGroupGizmo(MechanitorControlGroup controlGroup)
		{
			this.controlGroup = controlGroup;
			this.Order = -89f;
		}

		// Token: 0x060085D0 RID: 34256 RVA: 0x00305549 File Offset: 0x00303749
		public static IEnumerable<FloatMenuOption> GetWorkModeOptions(MechanitorControlGroup controlGroup)
		{
			MechanitorControlGroupGizmo.<GetWorkModeOptions>d__16 <GetWorkModeOptions>d__ = new MechanitorControlGroupGizmo.<GetWorkModeOptions>d__16(-2);
			<GetWorkModeOptions>d__.<>3__controlGroup = controlGroup;
			return <GetWorkModeOptions>d__;
		}

		// Token: 0x060085D1 RID: 34257 RVA: 0x00305559 File Offset: 0x00303759
		public override void GizmoUpdateOnMouseover()
		{
			base.GizmoUpdateOnMouseover();
			this.controlGroup.WorkMode.Worker.DrawControlGroupMouseOverExtra(this.controlGroup);
		}

		// Token: 0x060085D2 RID: 34258 RVA: 0x0030557C File Offset: 0x0030377C
		public override bool GroupsWith(Gizmo other)
		{
			MechanitorControlGroupGizmo mechanitorControlGroupGizmo = other as MechanitorControlGroupGizmo;
			if (mechanitorControlGroupGizmo == null)
			{
				return false;
			}
			if (mechanitorControlGroupGizmo.controlGroup == this.controlGroup)
			{
				return true;
			}
			if (this.controlGroup.Tracker == mechanitorControlGroupGizmo.controlGroup.Tracker && this.controlGroup.MechsForReading.Count == 0 && mechanitorControlGroupGizmo.controlGroup.MechsForReading.Count == 0)
			{
				return true;
			}
			if (this.mergedControlGroups.NotNullAndContains(mechanitorControlGroupGizmo.controlGroup))
			{
				this.mergedControlGroups.Remove(mechanitorControlGroupGizmo.controlGroup);
			}
			return false;
		}

		// Token: 0x060085D3 RID: 34259 RVA: 0x0030560C File Offset: 0x0030380C
		public override void MergeWith(Gizmo gizmo)
		{
			MechanitorControlGroupGizmo mechanitorControlGroupGizmo = gizmo as MechanitorControlGroupGizmo;
			if (mechanitorControlGroupGizmo == null)
			{
				Log.ErrorOnce("Tried to merge MechanitorControlGroupGizmo with unexpected type", 345234235);
				return;
			}
			if (mechanitorControlGroupGizmo.controlGroup != this.controlGroup)
			{
				if (this.mergedControlGroups == null)
				{
					this.mergedControlGroups = new List<MechanitorControlGroup>();
				}
				if (!this.mergedControlGroups.Contains(mechanitorControlGroupGizmo.controlGroup))
				{
					this.mergedControlGroups.Add(mechanitorControlGroupGizmo.controlGroup);
				}
			}
		}

		// Token: 0x060085D4 RID: 34260 RVA: 0x00305678 File Offset: 0x00303878
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!ModLister.CheckBiotech("Mechanitor control group gizmo"))
			{
				return new GizmoResult(GizmoState.Clear);
			}
			AcceptanceReport canControlMechs = this.controlGroup.Tracker.CanControlMechs;
			this.disabled = !canControlMechs;
			this.disabledReason = canControlMechs.Reason;
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			bool flag = Mouse.IsOver(rect2);
			List<Pawn> mechsForReading = this.controlGroup.MechsForReading;
			Color white = Color.white;
			Material material = (this.disabled || parms.lowLight || mechsForReading.Count <= 0) ? TexUI.GrayscaleGUI : null;
			GUI.color = (parms.lowLight ? Command.LowLightBgColor : white);
			GenUI.DrawTextureWithMaterial(rect, parms.shrunk ? Command.BGTexShrunk : Command.BGTex, material, default(Rect));
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = rect2;
			TaggedString taggedString = (!this.mergedControlGroups.NullOrEmpty<MechanitorControlGroup>()) ? "Groups".Translate() : "Group".Translate();
			taggedString += " " + this.controlGroup.Index.ToString();
			if (!this.mergedControlGroups.NullOrEmpty<MechanitorControlGroup>())
			{
				this.mergedControlGroups.SortBy((MechanitorControlGroup c) => c.Index);
				for (int i = 0; i < this.mergedControlGroups.Count; i++)
				{
					taggedString += ", " + this.mergedControlGroups[i].Index.ToString();
				}
			}
			taggedString = taggedString.Truncate(rect2.width, null);
			Vector2 vector = Text.CalcSize(taggedString);
			rect3.width = vector.x;
			rect3.height = vector.y;
			Widgets.Label(rect3, taggedString);
			if (mechsForReading.Count <= 0)
			{
				GUI.color = ColoredText.SubtleGrayColor;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect2, "(" + "NoMechs".Translate() + ")");
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return new GizmoResult(GizmoState.Clear);
			}
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				if (Widgets.ButtonInvisible(rect3, true))
				{
					Find.Selector.ClearSelection();
					for (int j = 0; j < mechsForReading.Count; j++)
					{
						Find.Selector.Select(mechsForReading[j], true, true);
					}
				}
			}
			bool flag2 = false;
			Rect rect4 = new Rect(rect.x + rect.width - 26f - 6f, rect.y + 6f, 26f, 26f);
			Widgets.DrawTextureFitted(rect4, MechanitorControlGroupGizmo.PowerIcon.Texture, 1f, 1f);
			if (!this.disabled && Mouse.IsOver(rect4))
			{
				flag2 = true;
				Widgets.DrawHighlight(rect4);
				if (Widgets.ButtonInvisible(rect4, true))
				{
					Find.WindowStack.Add(new Dialog_RechargeSettings(this.controlGroup));
				}
			}
			bool flag3 = false;
			Rect rect5 = new Rect(rect.x + rect.width - 52f - 6f, rect.y + 6f, 26f, 26f);
			Widgets.DrawTextureFitted(rect5, this.controlGroup.WorkMode.uiIcon, 1f, 1f);
			if (!this.disabled && Mouse.IsOver(rect5))
			{
				flag3 = true;
				Widgets.DrawHighlight(rect5);
			}
			Rect rect6 = new Rect(rect2.x, rect2.y + 26f + 4f, rect2.width, rect2.height - 26f - 4f);
			float num = rect6.height;
			int num2 = 0;
			int num3 = 0;
			for (float num4 = num; num4 >= 0f; num4 -= 1f)
			{
				num2 = Mathf.FloorToInt(rect6.width / num4);
				num3 = Mathf.FloorToInt(rect6.height / num4);
				if (num2 * num3 >= mechsForReading.Count)
				{
					num = num4;
					break;
				}
			}
			float num5 = (rect6.width - (float)num2 * num) / 2f;
			float num6 = (rect6.height - (float)num3 * num) / 2f;
			int num7 = 0;
			for (int k = 0; k < num2; k++)
			{
				int num8 = 0;
				while (num8 < num2 && num7 < mechsForReading.Count)
				{
					Rect rect7 = new Rect(rect6.x + (float)num8 * num + num5, rect6.y + (float)k * num + num6, num, num);
					Pawn pawn = mechsForReading[num7];
					Vector2 size = rect7.size;
					Rot4 east = Rot4.East;
					float controlGroupPortraitZoom = mechsForReading[num7].kindDef.controlGroupPortraitZoom;
					RenderTexture image = PortraitsCache.Get(pawn, size, east, default(Vector3), controlGroupPortraitZoom, true, true, true, true, null, null, false, null);
					if (!this.controlGroup.Tracker.ControlledPawns.Contains(mechsForReading[num7]))
					{
						Widgets.DrawRectFast(rect7, MechanitorControlGroupGizmo.UncontrolledMechBackgroundColor, null);
					}
					GUI.DrawTexture(rect7, image);
					if (Mouse.IsOver(rect7))
					{
						Widgets.DrawHighlight(rect7);
						MouseoverSounds.DoRegion(rect7, SoundDefOf.Mouseover_Command);
						if (Event.current.type == EventType.mouseDown)
						{
							if (Event.current.shift)
							{
								Find.Selector.Select(mechsForReading[num7], true, true);
							}
							else
							{
								CameraJumper.TryJumpAndSelect(mechsForReading[num7], CameraJumper.MovementMode.Pan);
							}
						}
						TargetHighlighter.Highlight(mechsForReading[num7], true, false, false);
					}
					if (Find.Selector.IsSelected(mechsForReading[num7]))
					{
						SelectionDrawerUtility.DrawSelectionOverlayOnGUI(mechsForReading[num7], rect7, 0.8f / (float)num2, 20f);
					}
					num7++;
					num8++;
				}
				if (num7 >= mechsForReading.Count)
				{
					break;
				}
			}
			if (Find.WindowStack.FloatMenu == null && !flag2)
			{
				TooltipHandler.TipRegion(rect, delegate()
				{
					string text = ("ControlGroup".Translate() + " #" + this.controlGroup.Index.ToString()).Colorize(ColoredText.TipSectionTitleColor) + "\n\n";
					text = string.Concat(new string[]
					{
						text,
						("CurrentMechWorkMode".Translate() + ": " + this.controlGroup.WorkMode.LabelCap).Colorize(ColoredText.TipSectionTitleColor),
						"\n",
						this.controlGroup.WorkMode.description,
						"\n\n"
					});
					IEnumerable<string> entries = from m in this.controlGroup.MechsForReading.Where(delegate(Pawn x)
					{
						Pawn_NeedsTracker needs = x.needs;
						return ((needs != null) ? needs.energy : null) != null;
					})
					select (m.LabelCap + " (" + m.needs.energy.CurLevelPercentage.ToStringPercent() + " " + "EnergyLower".Translate() + ")").Resolve();
					text = text + "AssignedMechs".Translate().Colorize(ColoredText.TipSectionTitleColor) + "\n" + entries.ToLineList(" - ", false);
					if (this.disabled && !this.disabledReason.NullOrEmpty())
					{
						text += ("\n\n" + "DisabledCommand".Translate() + ": " + this.disabledReason).Colorize(ColorLibrary.RedReadable);
					}
					return text;
				}, 2545872);
			}
			if (flag3 && Event.current.type == EventType.mouseDown)
			{
				return new GizmoResult(GizmoState.OpenedFloatMenu, Event.current);
			}
			return new GizmoResult(flag ? GizmoState.Mouseover : GizmoState.Clear);
		}

		// Token: 0x060085D5 RID: 34261 RVA: 0x00305D06 File Offset: 0x00303F06
		public override float GetWidth(float maxWidth)
		{
			return 130f;
		}

		// Token: 0x04005277 RID: 21111
		public const int InRectPadding = 6;

		// Token: 0x04005278 RID: 21112
		private const float Width = 130f;

		// Token: 0x04005279 RID: 21113
		private const int IconButtonSize = 26;

		// Token: 0x0400527A RID: 21114
		private const float BaseSelectedTexJump = 20f;

		// Token: 0x0400527B RID: 21115
		private const float BaseSelectedTextScale = 0.8f;

		// Token: 0x0400527C RID: 21116
		private static readonly CachedTexture PowerIcon = new CachedTexture("UI/Icons/MechRechargeSettings");

		// Token: 0x0400527D RID: 21117
		private static readonly Color UncontrolledMechBackgroundColor = new Color32(byte.MaxValue, 25, 25, 55);

		// Token: 0x0400527E RID: 21118
		private MechanitorControlGroup controlGroup;

		// Token: 0x0400527F RID: 21119
		private List<MechanitorControlGroup> mergedControlGroups;
	}
}
