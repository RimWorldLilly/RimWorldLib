using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200062E RID: 1582
	public class Message : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06003199 RID: 12697 RVA: 0x0013D2C5 File Offset: 0x0013B4C5
		protected float Age
		{
			get
			{
				return RealTime.LastRealTime - this.startingTime;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x0600319A RID: 12698 RVA: 0x0013D2D3 File Offset: 0x0013B4D3
		protected float TimeLeft
		{
			get
			{
				return 13f - this.Age;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x0600319B RID: 12699 RVA: 0x0013D2E1 File Offset: 0x0013B4E1
		public bool Expired
		{
			get
			{
				return this.TimeLeft <= 0f;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x0600319C RID: 12700 RVA: 0x0013D2F3 File Offset: 0x0013B4F3
		public float Alpha
		{
			get
			{
				if (this.TimeLeft < 0.6f)
				{
					return this.TimeLeft / 0.6f;
				}
				return 1f;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x0600319D RID: 12701 RVA: 0x0013D314 File Offset: 0x0013B514
		private static bool ShouldDrawBackground
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return true;
				}
				WindowStack windowStack = Find.WindowStack;
				for (int i = 0; i < windowStack.Count; i++)
				{
					if (windowStack[i].CausesMessageBackground())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x0600319E RID: 12702 RVA: 0x0013D353 File Offset: 0x0013B553
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x0600319F RID: 12703 RVA: 0x0013D356 File Offset: 0x0013B556
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x060031A0 RID: 12704 RVA: 0x0013D35D File Offset: 0x0013B55D
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x060031A1 RID: 12705 RVA: 0x0013D36A File Offset: 0x0013B56A
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x060031A2 RID: 12706 RVA: 0x0013D372 File Offset: 0x0013B572
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.startingTick;
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x060031A3 RID: 12707 RVA: 0x0013D37A File Offset: 0x0013B57A
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Messages.IsLive(this);
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x0013D385 File Offset: 0x0013B585
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x0013D38D File Offset: 0x0013B58D
		public Message()
		{
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x0013D3AC File Offset: 0x0013B5AC
		public Message(string text, MessageTypeDef def)
		{
			this.text = text;
			this.def = def;
			this.ResetTimer();
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextMessageID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x0013D40B File Offset: 0x0013B60B
		public Message(string text, MessageTypeDef def, LookTargets lookTargets) : this(text, def)
		{
			this.lookTargets = lookTargets;
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x0013D41C File Offset: 0x0013B61C
		public Message(string text, MessageTypeDef def, LookTargets lookTargets, Quest quest) : this(text, def, lookTargets)
		{
			this.quest = quest;
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x0013D430 File Offset: 0x0013B630
		public void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<float>(ref this.startingTime, "startingTime", 0f, false);
			Scribe_Values.Look<int>(ref this.startingFrame, "startingFrame", 0, false);
			Scribe_Values.Look<int>(ref this.startingTick, "startingTick", 0, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x0013D4D4 File Offset: 0x0013B6D4
		public Rect CalculateRect(float x, float y)
		{
			Text.Font = GameFont.Small;
			if (this.cachedSize.x < 0f)
			{
				this.cachedSize = Text.CalcSize(this.text);
			}
			this.lastDrawRect = new Rect(x, y, this.cachedSize.x, this.cachedSize.y);
			this.lastDrawRect = this.lastDrawRect.ExpandedBy(6f, 2f);
			return this.lastDrawRect;
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x0013D550 File Offset: 0x0013B750
		public void Draw(int xOffset, int yOffset)
		{
			Rect rect = this.CalculateRect((float)xOffset, (float)yOffset);
			Find.WindowStack.ImmediateWindow(Gen.HashCombineInt(this.ID, 45574281), rect, WindowLayer.Super, delegate
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect = rect.AtZero();
				float alpha = this.Alpha;
				GUI.color = new Color(1f, 1f, 1f, alpha);
				if (Message.ShouldDrawBackground)
				{
					GUI.color = new Color(0.15f, 0.15f, 0.15f, 0.8f * alpha);
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()) || (this.quest != null && !this.quest.hidden))
				{
					UIHighlighter.HighlightOpportunity(rect, "Messages");
					Widgets.DrawHighlightIfMouseover(rect);
				}
				if (this.flashTime + 0.6f > RealTime.LastRealTime)
				{
					float transparency = 1f - (RealTime.LastRealTime - this.flashTime) / 0.6f;
					Widgets.DrawTextHighlight(rect, 0f, new Color?(ColorLibrary.Yellow.ToTransparent(transparency)));
				}
				Widgets.Label(new Rect(6f, 2f, rect.width - 6f, rect.height - 2f), this.text);
				if (Current.ProgramState == ProgramState.Playing && Widgets.ButtonInvisible(rect, true))
				{
					if (Event.current.button == 1)
					{
						this.startingTime = -99999f;
					}
					else if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()))
					{
						CameraJumper.TryJumpAndSelect(this.lookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ClickingMessages, KnowledgeAmount.Total);
					}
					else if (this.quest != null && !this.quest.hidden)
					{
						if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Quests)
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
						else
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						}
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					Messages.Notify_Mouseover(this);
				}
			}, false, false, 0f, null, false);
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x0013D5B0 File Offset: 0x0013B7B0
		void IArchivable.OpenArchived()
		{
			Find.WindowStack.Add(new Dialog_MessageBox(this.text, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x0013D5E0 File Offset: 0x0013B7E0
		public void ResetTimer()
		{
			this.startingFrame = RealTime.frameCount;
			this.startingTime = RealTime.LastRealTime;
			this.startingTick = GenTicks.TicksGame;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x0013D603 File Offset: 0x0013B803
		public void Flash()
		{
			this.flashTime = RealTime.LastRealTime;
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x0013D610 File Offset: 0x0013B810
		public string GetUniqueLoadID()
		{
			return "Message_" + this.ID.ToString();
		}

		// Token: 0x04001FC5 RID: 8133
		public MessageTypeDef def;

		// Token: 0x04001FC6 RID: 8134
		private int ID;

		// Token: 0x04001FC7 RID: 8135
		public string text;

		// Token: 0x04001FC8 RID: 8136
		private float startingTime;

		// Token: 0x04001FC9 RID: 8137
		public int startingFrame;

		// Token: 0x04001FCA RID: 8138
		public int startingTick;

		// Token: 0x04001FCB RID: 8139
		private float flashTime;

		// Token: 0x04001FCC RID: 8140
		public LookTargets lookTargets;

		// Token: 0x04001FCD RID: 8141
		public Quest quest;

		// Token: 0x04001FCE RID: 8142
		private Vector2 cachedSize = new Vector2(-1f, -1f);

		// Token: 0x04001FCF RID: 8143
		public Rect lastDrawRect;

		// Token: 0x04001FD0 RID: 8144
		private const float DefaultMessageLifespan = 13f;

		// Token: 0x04001FD1 RID: 8145
		private const float FadeoutDuration = 0.6f;

		// Token: 0x04001FD2 RID: 8146
		private const float FlashDuration = 0.6f;
	}
}
