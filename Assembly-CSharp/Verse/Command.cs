using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000601 RID: 1537
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x001353E7 File Offset: 0x001335E7
		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x001353EF File Offset: 0x001335EF
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x001353FC File Offset: 0x001335FC
		public virtual string TopRightLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003033 RID: 12339 RVA: 0x001353FF File Offset: 0x001335FF
		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003034 RID: 12340 RVA: 0x00135407 File Offset: 0x00133607
		public virtual string DescPostfix
		{
			get
			{
				return this.defaultDescPostfix;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003035 RID: 12341 RVA: 0x0013540F File Offset: 0x0013360F
		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003036 RID: 12342 RVA: 0x00135417 File Offset: 0x00133617
		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003037 RID: 12343 RVA: 0x0013541F File Offset: 0x0013361F
		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003038 RID: 12344 RVA: 0x00135422 File Offset: 0x00133622
		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x0013542A File Offset: 0x0013362A
		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x0600303A RID: 12346 RVA: 0x00135432 File Offset: 0x00133632
		public virtual Texture2D BGTexture
		{
			get
			{
				return Command.BGTex;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x00135439 File Offset: 0x00133639
		public virtual Texture2D BGTextureShrunk
		{
			get
			{
				return Command.BGTexShrunk;
			}
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x00135440 File Offset: 0x00133640
		public override float GetWidth(float maxWidth)
		{
			return 75f;
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x00135447 File Offset: 0x00133647
		public float GetShrunkSize
		{
			get
			{
				return 36f;
			}
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x0013544E File Offset: 0x0013364E
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f), parms);
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x00135474 File Offset: 0x00133674
		public virtual GizmoResult GizmoOnGUIShrunk(Vector2 topLeft, float size, GizmoRenderParms parms)
		{
			parms.shrunk = true;
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, size, size), parms);
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x00135498 File Offset: 0x00133698
		protected virtual GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			Text.Font = GameFont.Tiny;
			Color color = Color.white;
			bool flag = false;
			if (Mouse.IsOver(butRect))
			{
				flag = true;
				if (!this.disabled)
				{
					color = GenUI.MouseoverColor;
				}
			}
			MouseoverSounds.DoRegion(butRect, SoundDefOf.Mouseover_Command);
			if (parms.highLight)
			{
				Widgets.DrawStrongHighlight(butRect.ExpandedBy(4f), null);
			}
			if (this.disabled)
			{
				parms.lowLight = true;
			}
			Material material = parms.lowLight ? TexUI.GrayscaleGUI : null;
			GUI.color = (parms.lowLight ? Command.LowLightBgColor : color);
			GenUI.DrawTextureWithMaterial(butRect, parms.shrunk ? this.BGTextureShrunk : this.BGTexture, material, default(Rect));
			GUI.color = color;
			this.DrawIcon(butRect, material, parms);
			bool flag2 = false;
			GUI.color = Color.white;
			if (parms.lowLight)
			{
				GUI.color = Command.LowLightLabelColor;
			}
			Vector2 vector = parms.shrunk ? new Vector2(3f, 0f) : new Vector2(5f, 3f);
			Rect rect = new Rect(butRect.x + vector.x, butRect.y + vector.y, butRect.width - 10f, Text.LineHeight);
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				if (parms.isFirst)
				{
					GUI.DrawTexture(new Rect(rect.x, rect.y, 21f, 21f), TexUI.SteamDeck_ButtonA);
					if (KeyBindingDefOf.Accept.KeyDownEvent)
					{
						flag2 = true;
						Event.current.Use();
					}
				}
			}
			else
			{
				KeyCode keyCode = (this.hotKey == null) ? KeyCode.None : this.hotKey.MainKey;
				if (keyCode != KeyCode.None && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode))
				{
					Widgets.Label(rect, keyCode.ToStringReadable());
					GizmoGridDrawer.drawnHotKeys.Add(keyCode);
					if (this.hotKey.KeyDownEvent)
					{
						flag2 = true;
						Event.current.Use();
					}
				}
			}
			if (GizmoGridDrawer.customActivator != null && GizmoGridDrawer.customActivator(this))
			{
				flag2 = true;
			}
			if (Widgets.ButtonInvisible(butRect, true))
			{
				flag2 = true;
			}
			if (!parms.shrunk)
			{
				string topRightLabel = this.TopRightLabel;
				if (!topRightLabel.NullOrEmpty())
				{
					Vector2 vector2 = Text.CalcSize(topRightLabel);
					Rect position;
					Rect rect2 = position = new Rect(butRect.xMax - vector2.x - 2f, butRect.y + 3f, vector2.x, vector2.y);
					position.x -= 2f;
					position.width += 3f;
					Text.Anchor = TextAnchor.UpperRight;
					GUI.DrawTexture(position, TexUI.GrayTextBG);
					Widgets.Label(rect2, topRightLabel);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				string labelCap = this.LabelCap;
				if (!labelCap.NullOrEmpty())
				{
					float num = Text.CalcHeight(labelCap, butRect.width + 0.1f);
					Rect rect3 = new Rect(butRect.x, butRect.yMax - num + 12f, butRect.width, num);
					GUI.DrawTexture(rect3, TexUI.GrayTextBG);
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect3, labelCap);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				GUI.color = Color.white;
			}
			if (Mouse.IsOver(butRect) && this.DoTooltip)
			{
				TipSignal tip = this.Desc;
				if (this.disabled && !this.disabledReason.NullOrEmpty())
				{
					tip.text += ("\n\n" + "DisabledCommand".Translate() + ": " + this.disabledReason).Colorize(ColorLibrary.RedReadable);
				}
				tip.text += this.DescPostfix;
				TooltipHandler.TipRegion(butRect, tip);
			}
			if (!this.HighlightTag.NullOrEmpty() && (Find.WindowStack.FloatMenu == null || !Find.WindowStack.FloatMenu.windowRect.Overlaps(butRect)))
			{
				UIHighlighter.HighlightOpportunity(butRect, this.HighlightTag);
			}
			Text.Font = GameFont.Small;
			if (flag2)
			{
				if (this.disabled)
				{
					if (!this.disabledReason.NullOrEmpty())
					{
						Messages.Message("DisabledCommand".Translate() + ": " + this.disabledReason, MessageTypeDefOf.RejectInput, false);
					}
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				GizmoResult result;
				if (Event.current.button == 1)
				{
					result = new GizmoResult(GizmoState.OpenedFloatMenu, Event.current);
				}
				else
				{
					if (!TutorSystem.AllowAction(this.TutorTagSelect))
					{
						return new GizmoResult(GizmoState.Mouseover, null);
					}
					result = new GizmoResult(GizmoState.Interacted, Event.current);
					TutorSystem.Notify_Event(this.TutorTagSelect);
				}
				return result;
			}
			else
			{
				if (flag)
				{
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				return new GizmoResult(GizmoState.Clear, null);
			}
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x00135964 File Offset: 0x00133B64
		public virtual void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			Texture badTex = this.icon;
			if (badTex == null)
			{
				badTex = BaseContent.BadTex;
			}
			rect.position += new Vector2(this.iconOffset.x * rect.size.x, this.iconOffset.y * rect.size.y);
			if (!this.disabled || parms.lowLight)
			{
				GUI.color = this.IconDrawColor;
			}
			else
			{
				GUI.color = this.IconDrawColor.SaturationChanged(0f);
			}
			if (parms.lowLight)
			{
				GUI.color = GUI.color.ToTransparent(0.6f);
			}
			Widgets.DrawTextureFitted(rect, badTex, this.iconDrawScale * 0.85f, this.iconProportions, this.iconTexCoords, this.iconAngle, this.overrideMaterial ?? buttonMat, 1f);
			GUI.color = Color.white;
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x00135A5C File Offset: 0x00133C5C
		public override bool GroupsWith(Gizmo other)
		{
			if (!this.groupable)
			{
				return false;
			}
			Command command = other as Command;
			return command != null && command.groupable && ((this.hotKey == command.hotKey && this.Label == command.Label && this.icon == command.icon && this.groupKey == command.groupKey) || (this.groupKeyIgnoreContent != -1 && command.groupKeyIgnoreContent != -1 && this.groupKeyIgnoreContent == command.groupKeyIgnoreContent));
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x00135AF0 File Offset: 0x00133CF0
		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x00135B06 File Offset: 0x00133D06
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Command(label=",
				this.defaultLabel,
				", defaultDesc=",
				this.defaultDesc,
				")"
			});
		}

		// Token: 0x04001ED9 RID: 7897
		public string defaultLabel;

		// Token: 0x04001EDA RID: 7898
		public string defaultDesc;

		// Token: 0x04001EDB RID: 7899
		public string defaultDescPostfix;

		// Token: 0x04001EDC RID: 7900
		public string mouseText;

		// Token: 0x04001EDD RID: 7901
		public Texture icon;

		// Token: 0x04001EDE RID: 7902
		public float iconAngle;

		// Token: 0x04001EDF RID: 7903
		public Vector2 iconProportions = Vector2.one;

		// Token: 0x04001EE0 RID: 7904
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001EE1 RID: 7905
		public float iconDrawScale = 1f;

		// Token: 0x04001EE2 RID: 7906
		public Vector2 iconOffset;

		// Token: 0x04001EE3 RID: 7907
		public Color defaultIconColor = Color.white;

		// Token: 0x04001EE4 RID: 7908
		public KeyBindingDef hotKey;

		// Token: 0x04001EE5 RID: 7909
		public SoundDef activateSound;

		// Token: 0x04001EE6 RID: 7910
		public int groupKey = -1;

		// Token: 0x04001EE7 RID: 7911
		public int groupKeyIgnoreContent = -1;

		// Token: 0x04001EE8 RID: 7912
		public string tutorTag = "TutorTagNotSet";

		// Token: 0x04001EE9 RID: 7913
		public bool shrinkable;

		// Token: 0x04001EEA RID: 7914
		public bool groupable = true;

		// Token: 0x04001EEB RID: 7915
		public bool hideMouseIcon;

		// Token: 0x04001EEC RID: 7916
		public Material overrideMaterial;

		// Token: 0x04001EED RID: 7917
		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x04001EEE RID: 7918
		public static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x04001EEF RID: 7919
		public static readonly Color LowLightBgColor = new Color(0.8f, 0.8f, 0.7f, 0.5f);

		// Token: 0x04001EF0 RID: 7920
		public static readonly Color LowLightIconColor = new Color(0.8f, 0.8f, 0.7f, 0.6f);

		// Token: 0x04001EF1 RID: 7921
		public static readonly Color LowLightLabelColor = Color.white;

		// Token: 0x04001EF2 RID: 7922
		public const float LowLightIconAlpha = 0.6f;

		// Token: 0x04001EF3 RID: 7923
		protected const float InnerIconDrawScale = 0.85f;
	}
}
