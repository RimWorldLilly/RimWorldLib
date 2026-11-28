using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020005F5 RID: 1525
	[StaticConstructorOnStartup]
	public class FloatMenuOption
	{
		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x00132AD5 File Offset: 0x00130CD5
		private static float NormalVerticalMargin
		{
			get
			{
				return (float)(SteamDeck.IsSteamDeck ? 10 : 4);
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06002FE3 RID: 12259 RVA: 0x00132AE4 File Offset: 0x00130CE4
		// (set) Token: 0x06002FE4 RID: 12260 RVA: 0x00132AEC File Offset: 0x00130CEC
		public string Label
		{
			get
			{
				return this.labelInt;
			}
			set
			{
				if (value.NullOrEmpty())
				{
					value = "(missing label)";
				}
				this.labelInt = value.TrimEnd();
				this.SetSizeMode(this.sizeMode);
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06002FE5 RID: 12261 RVA: 0x00132B15 File Offset: 0x00130D15
		private float VerticalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 1f;
				}
				return FloatMenuOption.NormalVerticalMargin;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x00132B2B File Offset: 0x00130D2B
		private float HorizontalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 3f;
				}
				return 6f;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x00132B41 File Offset: 0x00130D41
		private float IconOffset
		{
			get
			{
				if (this.shownItem == null && !this.drawPlaceHolderIcon && !(this.iconTex != null) && this.iconThing == null)
				{
					return 0f;
				}
				return this.CurIconSize;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x00132B75 File Offset: 0x00130D75
		private GameFont CurrentFont
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return GameFont.Tiny;
				}
				return GameFont.Small;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x00132B83 File Offset: 0x00130D83
		private float CurIconSize
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Tiny)
				{
					return 27f;
				}
				return 16f;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06002FEA RID: 12266 RVA: 0x00132B99 File Offset: 0x00130D99
		// (set) Token: 0x06002FEB RID: 12267 RVA: 0x00132BA4 File Offset: 0x00130DA4
		public bool Disabled
		{
			get
			{
				return this.action == null;
			}
			set
			{
				if (value)
				{
					this.action = null;
				}
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06002FEC RID: 12268 RVA: 0x00132BB0 File Offset: 0x00130DB0
		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06002FED RID: 12269 RVA: 0x00132BB8 File Offset: 0x00130DB8
		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x00132BC0 File Offset: 0x00130DC0
		// (set) Token: 0x06002FEF RID: 12271 RVA: 0x00132BD2 File Offset: 0x00130DD2
		public MenuOptionPriority Priority
		{
			get
			{
				if (this.Disabled)
				{
					return MenuOptionPriority.DisabledOption;
				}
				return this.priorityInt;
			}
			set
			{
				if (this.Disabled)
				{
					Log.Error("Setting priority on disabled FloatMenuOption: " + this.Label);
				}
				this.priorityInt = value;
			}
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x00132BF8 File Offset: 0x00130DF8
		public FloatMenuOption(string label, Action action, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0)
		{
			this.Label = label;
			this.action = action;
			this.priorityInt = priority;
			this.revalidateClickTarget = revalidateClickTarget;
			this.mouseoverGuiAction = mouseoverGuiAction;
			this.extraPartWidth = extraPartWidth;
			this.extraPartOnGUI = extraPartOnGUI;
			this.revalidateWorldClickTarget = revalidateWorldClickTarget;
			this.playSelectionSound = playSelectionSound;
			this.orderInPriority = orderInPriority;
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x00132C90 File Offset: 0x00130E90
		public FloatMenuOption(string label, Action action, ThingDef shownItemForIcon, ThingStyleDef thingStyle = null, bool forceBasicStyle = false, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0, int? graphicIndexOverride = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.shownItem = shownItemForIcon;
			this.thingStyle = thingStyle;
			this.forceBasicStyle = forceBasicStyle;
			this.graphicIndexOverride = graphicIndexOverride;
			if (shownItemForIcon == null)
			{
				this.drawPlaceHolderIcon = true;
			}
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x00132CE0 File Offset: 0x00130EE0
		public FloatMenuOption(string label, Action action, ThingDef shownItemForIcon, Texture2D iconTex, ThingStyleDef thingStyle = null, bool forceBasicStyle = false, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0, int? graphicIndexOverride = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.iconTex = iconTex;
			this.shownItem = shownItemForIcon;
			this.thingStyle = thingStyle;
			this.forceBasicStyle = forceBasicStyle;
			this.graphicIndexOverride = graphicIndexOverride;
			if (shownItemForIcon == null && iconTex == null)
			{
				this.drawPlaceHolderIcon = true;
			}
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x00132D40 File Offset: 0x00130F40
		public FloatMenuOption(string label, Action action, Texture2D iconTex, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0, HorizontalJustification iconJustification = HorizontalJustification.Left, bool extraPartRightJustified = false) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.iconTex = iconTex;
			this.iconColor = iconColor;
			this.iconJustification = iconJustification;
			this.extraPartRightJustified = extraPartRightJustified;
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x00132D84 File Offset: 0x00130F84
		public FloatMenuOption(string label, Action action, Thing iconThing, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.iconThing = iconThing;
			this.iconColor = iconColor;
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x00132DB8 File Offset: 0x00130FB8
		public static FloatMenuOption CheckboxLabeled(string label, Action checkboxStateChanged, bool currentState)
		{
			return new FloatMenuOption(label, checkboxStateChanged, Widgets.GetCheckboxTexture(currentState), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Right, false);
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x00132DE8 File Offset: 0x00130FE8
		public void SetSizeMode(FloatMenuSizeMode newSizeMode)
		{
			this.sizeMode = newSizeMode;
			GameFont font = Text.Font;
			Text.Font = this.CurrentFont;
			float width = 300f - (2f * this.HorizontalMargin + 4f + this.extraPartWidth + this.IconOffset);
			this.cachedRequiredHeight = 2f * this.VerticalMargin + Text.CalcHeight(this.Label, width);
			this.cachedRequiredWidth = this.HorizontalMargin + 4f + Text.CalcSize(this.Label).x + this.extraPartWidth + this.HorizontalMargin + this.IconOffset + 4f;
			Text.Font = font;
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x00132E98 File Offset: 0x00131098
		public void Chosen(bool colonistOrdering, FloatMenu floatMenu)
		{
			if (floatMenu != null)
			{
				floatMenu.PreOptionChosen(this);
			}
			if (!this.Disabled)
			{
				if (this.action != null)
				{
					if (colonistOrdering && this.playSelectionSound)
					{
						SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
					}
					this.action();
					return;
				}
			}
			else if (this.playSelectionSound)
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x00132EF4 File Offset: 0x001310F4
		public virtual bool DoGUI(Rect rect, bool colonistOrdering, FloatMenu floatMenu)
		{
			Rect rect2 = rect;
			float height = rect2.height;
			rect2.height = height - 1f;
			bool flag = !this.Disabled && Mouse.IsOver(rect2);
			bool flag2 = false;
			Text.Font = this.CurrentFont;
			if (this.tooltip != null)
			{
				TooltipHandler.TipRegion(rect, this.tooltip.Value);
			}
			Rect rect3 = rect;
			if (this.iconJustification == HorizontalJustification.Left)
			{
				rect3.xMin += 4f;
				rect3.xMax = rect.x + this.CurIconSize;
				rect3.yMin += 4f;
				rect3.yMax = rect.y + this.CurIconSize;
				if (flag)
				{
					rect3.x += 4f;
				}
			}
			Rect rect4 = rect;
			rect4.xMin += this.HorizontalMargin;
			rect4.xMax -= this.HorizontalMargin;
			rect4.xMax -= 4f;
			rect4.xMax -= this.extraPartWidth + this.IconOffset;
			if (this.iconJustification == HorizontalJustification.Left)
			{
				rect4.x += this.IconOffset;
			}
			if (flag)
			{
				rect4.x += 4f;
			}
			float num = Mathf.Min(Text.CalcSize(this.Label).x, rect4.width - 4f);
			float num2 = rect4.xMin + num;
			if (this.iconJustification == HorizontalJustification.Right)
			{
				rect3.x = num2 + 4f;
				rect3.width = this.CurIconSize;
				rect3.yMin += 4f;
				rect3.yMax = rect.y + this.CurIconSize;
				num2 += this.CurIconSize;
			}
			Rect rect5 = default(Rect);
			if (this.extraPartWidth != 0f)
			{
				if (this.extraPartRightJustified)
				{
					num2 = rect.xMax - this.extraPartWidth;
				}
				rect5 = new Rect(num2, rect4.yMin, this.extraPartWidth, 30f);
				flag2 = Mouse.IsOver(rect5);
			}
			if (!this.Disabled)
			{
				MouseoverSounds.DoRegion(rect2);
			}
			Color color = GUI.color;
			if (this.Disabled)
			{
				GUI.color = FloatMenuOption.ColorBGDisabled * color;
			}
			else if (flag && !flag2)
			{
				GUI.color = FloatMenuOption.ColorBGActiveMouseover * color;
			}
			else
			{
				GUI.color = FloatMenuOption.ColorBGActive * color;
			}
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = ((!this.Disabled) ? FloatMenuOption.ColorTextActive : FloatMenuOption.ColorTextDisabled) * color;
			if (this.sizeMode == FloatMenuSizeMode.Tiny)
			{
				rect4.y += 1f;
			}
			Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect4, this.Label);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(this.iconColor.r, this.iconColor.g, this.iconColor.b, this.iconColor.a * GUI.color.a);
			if (this.shownItem != null || this.drawPlaceHolderIcon)
			{
				ThingStyleDef thingStyleDef;
				if ((thingStyleDef = this.thingStyle) == null)
				{
					if (this.shownItem == null || Find.World == null)
					{
						thingStyleDef = null;
					}
					else
					{
						FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
						if (ideos == null)
						{
							thingStyleDef = null;
						}
						else
						{
							Ideo primaryIdeo = ideos.PrimaryIdeo;
							thingStyleDef = ((primaryIdeo != null) ? primaryIdeo.GetStyleFor(this.shownItem) : null);
						}
					}
				}
				ThingStyleDef thingStyleDef2 = thingStyleDef;
				if (this.forceBasicStyle)
				{
					thingStyleDef2 = null;
				}
				Color value;
				if (this.forceThingColor != null)
				{
					value = this.forceThingColor.Value;
				}
				else if (this.shownItem != null)
				{
					value = (this.shownItem.MadeFromStuff ? this.shownItem.GetColorForStuff(GenStuff.DefaultStuffFor(this.shownItem)) : this.shownItem.uiIconColor);
				}
				else
				{
					value = Color.white;
				}
				value.a *= color.a;
				Widgets.DefIcon(rect3, this.shownItem, null, 1f, thingStyleDef2, this.drawPlaceHolderIcon, new Color?(value), null, this.graphicIndexOverride, 1f);
			}
			else if (this.iconTex)
			{
				Widgets.DrawTextureFitted(rect3, this.iconTex, 1f, new Vector2(1f, 1f), this.iconTexCoords, 0f, null, 1f);
			}
			else if (this.iconThing != null)
			{
				Widgets.ThingIcon(rect3, this.iconThing, color.a, null, false, 1f, false);
			}
			GUI.color = color;
			if (this.extraPartOnGUI != null)
			{
				bool flag3 = this.extraPartOnGUI(rect5);
				GUI.color = color;
				if (flag3)
				{
					return true;
				}
			}
			if (flag && this.mouseoverGuiAction != null)
			{
				this.mouseoverGuiAction(rect);
			}
			if (this.tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.tutorTag);
			}
			if (!Widgets.ButtonInvisible(rect2, true))
			{
				return false;
			}
			if (this.tutorTag != null && !TutorSystem.AllowAction(this.tutorTag))
			{
				return false;
			}
			this.Chosen(colonistOrdering, floatMenu);
			if (this.tutorTag != null)
			{
				TutorSystem.Notify_Event(this.tutorTag);
			}
			return true;
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x0013343C File Offset: 0x0013163C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"FloatMenuOption(",
				this.Label,
				", ",
				this.Disabled ? "disabled" : "enabled",
				")"
			});
		}

		// Token: 0x04001E86 RID: 7814
		private string labelInt;

		// Token: 0x04001E87 RID: 7815
		public Action action;

		// Token: 0x04001E88 RID: 7816
		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		// Token: 0x04001E89 RID: 7817
		public int orderInPriority;

		// Token: 0x04001E8A RID: 7818
		public bool autoTakeable;

		// Token: 0x04001E8B RID: 7819
		public float autoTakeablePriority;

		// Token: 0x04001E8C RID: 7820
		public Action<Rect> mouseoverGuiAction;

		// Token: 0x04001E8D RID: 7821
		public Thing revalidateClickTarget;

		// Token: 0x04001E8E RID: 7822
		public bool targetsDespawned;

		// Token: 0x04001E8F RID: 7823
		public WorldObject revalidateWorldClickTarget;

		// Token: 0x04001E90 RID: 7824
		public float extraPartWidth;

		// Token: 0x04001E91 RID: 7825
		public Func<Rect, bool> extraPartOnGUI;

		// Token: 0x04001E92 RID: 7826
		public string tutorTag;

		// Token: 0x04001E93 RID: 7827
		public ThingStyleDef thingStyle;

		// Token: 0x04001E94 RID: 7828
		public bool forceBasicStyle;

		// Token: 0x04001E95 RID: 7829
		public TipSignal? tooltip;

		// Token: 0x04001E96 RID: 7830
		public bool extraPartRightJustified;

		// Token: 0x04001E97 RID: 7831
		public int? graphicIndexOverride;

		// Token: 0x04001E98 RID: 7832
		public bool isGoto;

		// Token: 0x04001E99 RID: 7833
		private FloatMenuSizeMode sizeMode;

		// Token: 0x04001E9A RID: 7834
		private float cachedRequiredHeight;

		// Token: 0x04001E9B RID: 7835
		private float cachedRequiredWidth;

		// Token: 0x04001E9C RID: 7836
		private bool drawPlaceHolderIcon;

		// Token: 0x04001E9D RID: 7837
		private bool playSelectionSound = true;

		// Token: 0x04001E9E RID: 7838
		private ThingDef shownItem;

		// Token: 0x04001E9F RID: 7839
		public Thing iconThing;

		// Token: 0x04001EA0 RID: 7840
		private Texture2D iconTex;

		// Token: 0x04001EA1 RID: 7841
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001EA2 RID: 7842
		private HorizontalJustification iconJustification;

		// Token: 0x04001EA3 RID: 7843
		public Color iconColor = Color.white;

		// Token: 0x04001EA4 RID: 7844
		public Color? forceThingColor;

		// Token: 0x04001EA5 RID: 7845
		public const float MaxWidth = 300f;

		// Token: 0x04001EA6 RID: 7846
		private const float TinyVerticalMargin = 1f;

		// Token: 0x04001EA7 RID: 7847
		private const float NormalHorizontalMargin = 6f;

		// Token: 0x04001EA8 RID: 7848
		private const float TinyHorizontalMargin = 3f;

		// Token: 0x04001EA9 RID: 7849
		private const float MouseOverLabelShift = 4f;

		// Token: 0x04001EAA RID: 7850
		public static readonly Color ColorBGActive = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x04001EAB RID: 7851
		public static readonly Color ColorBGActiveMouseover = new ColorInt(29, 45, 50).ToColor;

		// Token: 0x04001EAC RID: 7852
		public static readonly Color ColorBGDisabled = new ColorInt(40, 40, 40).ToColor;

		// Token: 0x04001EAD RID: 7853
		public static readonly Color ColorTextActive = Color.white;

		// Token: 0x04001EAE RID: 7854
		public static readonly Color ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04001EAF RID: 7855
		public const float ExtraPartHeight = 30f;

		// Token: 0x04001EB0 RID: 7856
		private const float ItemIconSize = 27f;

		// Token: 0x04001EB1 RID: 7857
		private const float ItemIconSizeTiny = 16f;

		// Token: 0x04001EB2 RID: 7858
		private const float ItemIconMargin = 4f;
	}
}
