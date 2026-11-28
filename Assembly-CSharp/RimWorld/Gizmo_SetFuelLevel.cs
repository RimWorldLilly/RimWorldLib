using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001952 RID: 6482
	public class Gizmo_SetFuelLevel : Gizmo_Slider
	{
		// Token: 0x17001C2D RID: 7213
		// (get) Token: 0x0600A2B9 RID: 41657 RVA: 0x003A225A File Offset: 0x003A045A
		// (set) Token: 0x0600A2BA RID: 41658 RVA: 0x003A2278 File Offset: 0x003A0478
		protected override float Target
		{
			get
			{
				return this.refuelable.TargetFuelLevel / this.refuelable.Props.fuelCapacity;
			}
			set
			{
				this.refuelable.TargetFuelLevel = value * this.refuelable.Props.fuelCapacity;
			}
		}

		// Token: 0x17001C2E RID: 7214
		// (get) Token: 0x0600A2BB RID: 41659 RVA: 0x003A2297 File Offset: 0x003A0497
		protected override float ValuePercent
		{
			get
			{
				return this.refuelable.FuelPercentOfMax;
			}
		}

		// Token: 0x17001C2F RID: 7215
		// (get) Token: 0x0600A2BC RID: 41660 RVA: 0x003A22A4 File Offset: 0x003A04A4
		protected override string Title
		{
			get
			{
				return this.refuelable.Props.FuelGizmoLabel;
			}
		}

		// Token: 0x17001C30 RID: 7216
		// (get) Token: 0x0600A2BD RID: 41661 RVA: 0x003A22B6 File Offset: 0x003A04B6
		protected override bool IsDraggable
		{
			get
			{
				return this.refuelable.Props.targetFuelLevelConfigurable;
			}
		}

		// Token: 0x17001C31 RID: 7217
		// (get) Token: 0x0600A2BE RID: 41662 RVA: 0x003A22C8 File Offset: 0x003A04C8
		protected override string BarLabel
		{
			get
			{
				return this.refuelable.Fuel.ToStringDecimalIfSmall() + " / " + this.refuelable.Props.fuelCapacity.ToStringDecimalIfSmall();
			}
		}

		// Token: 0x17001C32 RID: 7218
		// (get) Token: 0x0600A2BF RID: 41663 RVA: 0x003A22F9 File Offset: 0x003A04F9
		// (set) Token: 0x0600A2C0 RID: 41664 RVA: 0x003A2300 File Offset: 0x003A0500
		protected override bool DraggingBar
		{
			get
			{
				return Gizmo_SetFuelLevel.draggingBar;
			}
			set
			{
				Gizmo_SetFuelLevel.draggingBar = value;
			}
		}

		// Token: 0x0600A2C1 RID: 41665 RVA: 0x003A2308 File Offset: 0x003A0508
		public Gizmo_SetFuelLevel(CompRefuelable refuelable)
		{
			this.refuelable = refuelable;
		}

		// Token: 0x0600A2C2 RID: 41666 RVA: 0x003A2318 File Offset: 0x003A0518
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!this.refuelable.Props.showAllowAutoRefuelToggle)
			{
				return base.GizmoOnGUI(topLeft, maxWidth, parms);
			}
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				return base.GizmoOnGUI(topLeft, maxWidth, parms);
			}
			KeyCode keyCode = (KeyBindingDefOf.Command_ItemForbid == null) ? KeyCode.None : KeyBindingDefOf.Command_ItemForbid.MainKey;
			if (keyCode != KeyCode.None && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode) && KeyBindingDefOf.Command_ItemForbid.KeyDownEvent)
			{
				this.ToggleAutoRefuel();
				Event.current.Use();
			}
			return base.GizmoOnGUI(topLeft, maxWidth, parms);
		}

		// Token: 0x0600A2C3 RID: 41667 RVA: 0x003A239C File Offset: 0x003A059C
		protected override void DrawHeader(Rect headerRect, ref bool mouseOverElement)
		{
			if (this.refuelable.Props.showAllowAutoRefuelToggle)
			{
				headerRect.xMax -= 24f;
				Rect rect = new Rect(headerRect.xMax, headerRect.y, 24f, 24f);
				GUI.DrawTexture(rect, this.refuelable.Props.FuelIcon);
				GUI.DrawTexture(new Rect(rect.center.x, rect.y, rect.width / 2f, rect.height / 2f), this.refuelable.allowAutoRefuel ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
				if (Widgets.ButtonInvisible(rect, true))
				{
					this.ToggleAutoRefuel();
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
					TooltipHandler.TipRegion(rect, new Func<string>(this.RefuelTip), 828267373);
					mouseOverElement = true;
				}
			}
			base.DrawHeader(headerRect, ref mouseOverElement);
		}

		// Token: 0x0600A2C4 RID: 41668 RVA: 0x003A2495 File Offset: 0x003A0695
		private void ToggleAutoRefuel()
		{
			this.refuelable.allowAutoRefuel = !this.refuelable.allowAutoRefuel;
			if (this.refuelable.allowAutoRefuel)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				return;
			}
			SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
		}

		// Token: 0x0600A2C5 RID: 41669 RVA: 0x003A24D4 File Offset: 0x003A06D4
		private string RefuelTip()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CommandToggleAllowAutoRefuel".Translate());
			string str = defaultInterpolatedStringHandler.ToStringAndClear() + "\n\n";
			string str2 = this.refuelable.allowAutoRefuel ? "On".Translate() : "Off".Translate();
			string value = this.refuelable.TargetFuelLevel.ToString("F0").Colorize(ColoredText.TipSectionTitleColor);
			string t = str + "CommandToggleAllowAutoRefuelDesc".Translate(value, str2.UncapitalizeFirst().Named("ONOFF")).Resolve() + "\n\n";
			string t2 = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.Command_ItemForbid, KeyPrefs.BindingSlot.A).ToStringReadable();
			return t + ("HotKeyTip".Translate() + ": " + t2);
		}

		// Token: 0x0600A2C6 RID: 41670 RVA: 0x003A25CB File Offset: 0x003A07CB
		protected override string GetTooltip()
		{
			return "";
		}

		// Token: 0x0400602E RID: 24622
		private CompRefuelable refuelable;

		// Token: 0x0400602F RID: 24623
		private static bool draggingBar;
	}
}
