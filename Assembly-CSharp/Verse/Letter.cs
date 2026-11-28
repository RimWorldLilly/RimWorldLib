using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200061B RID: 1563
	public abstract class Letter : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060030E1 RID: 12513 RVA: 0x00138F14 File Offset: 0x00137114
		public virtual bool CanShowInLetterStack
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060030E2 RID: 12514 RVA: 0x00138F17 File Offset: 0x00137117
		public virtual bool CanDismissWithRightClick
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060030E3 RID: 12515 RVA: 0x00138F1A File Offset: 0x0013711A
		public bool ArchivedOnly
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060030E4 RID: 12516 RVA: 0x00138F2F File Offset: 0x0013712F
		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060030E5 RID: 12517 RVA: 0x00138F36 File Offset: 0x00137136
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return this.def.Icon;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x00138F43 File Offset: 0x00137143
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return this.def.color;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060030E7 RID: 12519 RVA: 0x00138F50 File Offset: 0x00137150
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060030E8 RID: 12520 RVA: 0x00138F5D File Offset: 0x0013715D
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.GetMouseoverText();
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x00138F65 File Offset: 0x00137165
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.arrivalTick;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x00138F6D File Offset: 0x0013716D
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060030EB RID: 12523 RVA: 0x00138F82 File Offset: 0x00137182
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x00138F8A File Offset: 0x0013718A
		public virtual bool ShouldAutomaticallyOpenLetter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x060030ED RID: 12525 RVA: 0x00138F8D File Offset: 0x0013718D
		// (set) Token: 0x060030EE RID: 12526 RVA: 0x00138F95 File Offset: 0x00137195
		public TaggedString Label
		{
			get
			{
				return this.label;
			}
			set
			{
				this.label = value.CapitalizeFirst();
			}
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x00138FA4 File Offset: 0x001371A4
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<TaggedString>(ref this.label, "label", default(TaggedString), false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.arrivalTick, "arrivalTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.relatedFaction != null && !Find.FactionManager.AllFactionsListForReading.Contains(this.relatedFaction))
			{
				this.relatedFaction = null;
			}
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x00139054 File Offset: 0x00137254
		public virtual void DrawButtonAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			Rect rect = new Rect(num, topY, 38f, 30f);
			Rect rect2 = new Rect(rect);
			float num2 = Time.time - this.arrivalTime;
			Color color = this.def.color;
			if (num2 < 1f)
			{
				rect2.y -= (1f - num2) * 200f;
				color.a = num2 / 1f;
			}
			if (!Mouse.IsOver(rect) && this.def.bounce && num2 > 15f && num2 % 5f < 1f)
			{
				float num3 = (float)UI.screenWidth * 0.06f;
				float num4 = 2f * (num2 % 1f) - 1f;
				float num5 = num3 * (1f - num4 * num4);
				rect2.x -= num5;
			}
			if (Event.current.type == EventType.repaint)
			{
				if (this.def.flashInterval > 0f)
				{
					float num6 = Time.time - (this.arrivalTime + 1f);
					if (num6 > 0f && num6 % this.def.flashInterval < 1f)
					{
						GenUI.DrawFlash(num, topY, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num6) * 0.55f, this.def.flashColor);
					}
				}
				GUI.color = color;
				Widgets.DrawShadowAround(rect2);
				GUI.DrawTexture(rect2, this.def.Icon);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperRight;
				string text = this.PostProcessedLabel();
				Vector2 vector = Text.CalcSize(text);
				float x = vector.x;
				float y = vector.y;
				Vector2 vector2 = new Vector2(rect2.x + rect2.width / 2f, rect2.center.y - y / 2f + 4f);
				float num7 = vector2.x + x / 2f - (float)(UI.screenWidth - 2);
				if (num7 > 0f)
				{
					vector2.x -= num7;
				}
				GUI.DrawTexture(new Rect(vector2.x - x / 2f - 6f - 1f, vector2.y, x + 12f, 16f), TexUI.GrayTextBG);
				GUI.color = new Color(1f, 1f, 1f, 0.75f);
				Rect rect3 = new Rect(vector2.x - x / 2f, vector2.y - 3f, x, 999f);
				Text.WordWrap = false;
				Widgets.Label(rect3, text);
				Text.WordWrap = true;
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (this.CanDismissWithRightClick && Event.current.type == EventType.mouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.LetterStack.RemoveLetter(this);
				Event.current.Use();
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.OpenLetter();
				Event.current.Use();
			}
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x0013938C File Offset: 0x0013758C
		public virtual void CheckForMouseOverTextAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			if (Mouse.IsOver(new Rect(num, topY, 38f, 30f)))
			{
				Find.LetterStack.Notify_LetterMouseover(this);
				TaggedString mouseoverText = this.GetMouseoverText();
				if (!mouseoverText.Resolve().NullOrEmpty())
				{
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					float num2 = Text.CalcHeight(mouseoverText, 310f);
					num2 += 20f;
					float x = num - 330f - 10f;
					float y = Mathf.Max(topY - num2 / 2f, 0f);
					Rect infoRect = new Rect(x, y, 330f, num2);
					Find.WindowStack.ImmediateWindow(2768333, infoRect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Small;
						Rect rect = infoRect.AtZero().ContractedBy(10f);
						Widgets.BeginGroup(rect);
						Widgets.Label(new Rect(0f, 0f, rect.width, rect.height), mouseoverText.Resolve());
						Widgets.EndGroup();
					}, true, false, 1f, null, false);
				}
			}
		}

		// Token: 0x060030F2 RID: 12530
		protected abstract string GetMouseoverText();

		// Token: 0x060030F3 RID: 12531
		public abstract void OpenLetter();

		// Token: 0x060030F4 RID: 12532 RVA: 0x0013948A File Offset: 0x0013768A
		public virtual void Received()
		{
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x0013948C File Offset: 0x0013768C
		public virtual void Removed()
		{
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x0013948E File Offset: 0x0013768E
		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x0013949B File Offset: 0x0013769B
		void IArchivable.OpenArchived()
		{
			this.OpenLetter();
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x001394A3 File Offset: 0x001376A3
		public string GetUniqueLoadID()
		{
			return "Letter_" + this.ID.ToString();
		}

		// Token: 0x04001F72 RID: 8050
		public int ID;

		// Token: 0x04001F73 RID: 8051
		public LetterDef def;

		// Token: 0x04001F74 RID: 8052
		private TaggedString label;

		// Token: 0x04001F75 RID: 8053
		public LookTargets lookTargets;

		// Token: 0x04001F76 RID: 8054
		public Faction relatedFaction;

		// Token: 0x04001F77 RID: 8055
		public int arrivalTick;

		// Token: 0x04001F78 RID: 8056
		public float arrivalTime;

		// Token: 0x04001F79 RID: 8057
		public string debugInfo;

		// Token: 0x04001F7A RID: 8058
		public const float DrawWidth = 38f;

		// Token: 0x04001F7B RID: 8059
		public const float DrawHeight = 30f;

		// Token: 0x04001F7C RID: 8060
		private const float FallTime = 1f;

		// Token: 0x04001F7D RID: 8061
		private const float FallDistance = 200f;
	}
}
