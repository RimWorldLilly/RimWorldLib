using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D0D RID: 7437
	public class Screen_Credits : Window
	{
		// Token: 0x170022EC RID: 8940
		// (get) Token: 0x0600C048 RID: 49224 RVA: 0x00462462 File Offset: 0x00460662
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x170022ED RID: 8941
		// (get) Token: 0x0600C049 RID: 49225 RVA: 0x00462475 File Offset: 0x00460675
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170022EE RID: 8942
		// (get) Token: 0x0600C04A RID: 49226 RVA: 0x0046247C File Offset: 0x0046067C
		private float ViewWidth
		{
			get
			{
				return 800f;
			}
		}

		// Token: 0x170022EF RID: 8943
		// (get) Token: 0x0600C04B RID: 49227 RVA: 0x00462484 File Offset: 0x00460684
		private float ViewHeight
		{
			get
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = this.creds.Sum((CreditsEntry c) => c.DrawHeight(this.ViewWidth)) + 20f;
				Text.Font = font;
				return result;
			}
		}

		// Token: 0x170022F0 RID: 8944
		// (get) Token: 0x0600C04C RID: 49228 RVA: 0x004624C0 File Offset: 0x004606C0
		private float MaxScrollPosition
		{
			get
			{
				return Mathf.Max(this.ViewHeight - (float)UI.screenHeight / 2f, 0f);
			}
		}

		// Token: 0x170022F1 RID: 8945
		// (get) Token: 0x0600C04D RID: 49229 RVA: 0x004624E0 File Offset: 0x004606E0
		private float AutoScrollRate
		{
			get
			{
				if (!this.wonGame)
				{
					return 30f;
				}
				if (this.scrollPosition < this.victoryTextHeight - 200f)
				{
					return 20f;
				}
				float num = 130f;
				if (this.EndCreditsSong != null)
				{
					num = this.EndCreditsSong.clip.length + this.songStartDelay - 10f - this.victoryTextHeight / 20f;
				}
				float t = (this.scrollPosition - this.victoryTextHeight) / 200f;
				return Mathf.Lerp(20f, this.MaxScrollPosition / num, t);
			}
		}

		// Token: 0x170022F2 RID: 8946
		// (get) Token: 0x0600C04E RID: 49230 RVA: 0x00462575 File Offset: 0x00460775
		private SongDef EndCreditsSong
		{
			get
			{
				return this.endCreditsSong;
			}
		}

		// Token: 0x0600C04F RID: 49231 RVA: 0x0046257D File Offset: 0x0046077D
		public Screen_Credits() : this("", 50)
		{
		}

		// Token: 0x0600C050 RID: 49232 RVA: 0x0046258C File Offset: 0x0046078C
		public Screen_Credits(string preCreditsMessage, int preCreditsSpace = 50) : base(null)
		{
			this.doWindowBackground = false;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.forcePause = true;
			this.closeOnCancel = false;
			this.creds = CreditsAssembler.AllCredits().ToList<CreditsEntry>();
			this.creds.Insert(0, new CreditRecord_Space(100f));
			if (!preCreditsMessage.NullOrEmpty())
			{
				this.creds.Insert(1, new CreditRecord_Space(200f));
				this.creds.Insert(2, new CreditRecord_Text(preCreditsMessage, TextAnchor.UpperLeft));
				this.creds.Insert(3, new CreditRecord_Space((float)preCreditsSpace));
				Text.Font = GameFont.Medium;
				this.victoryTextHeight = this.creds.Take(4).Sum((CreditsEntry c) => c.DrawHeight(this.ViewWidth));
			}
			this.creds.Add(new CreditRecord_Space(300f));
			this.creds.Add(new CreditRecord_Text("ThanksForPlaying".Translate(), TextAnchor.UpperCenter));
			string text = string.Empty;
			foreach (CreditsEntry creditsEntry in this.creds)
			{
				CreditRecord_Role creditRecord_Role = creditsEntry as CreditRecord_Role;
				if (creditRecord_Role == null)
				{
					text = string.Empty;
				}
				else
				{
					creditRecord_Role.displayKey = (text.NullOrEmpty() || creditRecord_Role.roleKey != text);
					text = creditRecord_Role.roleKey;
				}
			}
		}

		// Token: 0x0600C051 RID: 49233 RVA: 0x0046271C File Offset: 0x0046091C
		public override void PreOpen()
		{
			base.PreOpen();
			Screen_Credits.creationRealtime = Time.realtimeSinceStartup;
			Screen_Credits.creditsShowing = true;
			if (this.wonGame)
			{
				this.timeUntilAutoScroll = 10f;
				return;
			}
			this.timeUntilAutoScroll = 1f;
		}

		// Token: 0x0600C052 RID: 49234 RVA: 0x00462753 File Offset: 0x00460953
		public override void PostClose()
		{
			base.PostOpen();
			Screen_Credits.creditsShowing = false;
			if (this.exitToMainMenu)
			{
				GenScene.GoToMainMenu();
			}
		}

		// Token: 0x0600C053 RID: 49235 RVA: 0x00462770 File Offset: 0x00460970
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (this.timeUntilAutoScroll > 0f)
			{
				this.timeUntilAutoScroll -= Time.deltaTime;
			}
			else
			{
				this.scrollPosition += this.AutoScrollRate * Time.deltaTime;
			}
			if (this.wonGame && this.EndCreditsSong != null && !this.playedMusic && Time.realtimeSinceStartup > Screen_Credits.creationRealtime + this.songStartDelay)
			{
				Find.MusicManagerPlay.ForcePlaySong(this.EndCreditsSong, false);
				this.playedMusic = true;
			}
			if (this.closeDelay > 0f)
			{
				this.closeDelay -= Time.deltaTime;
				if (this.closeDelay <= 0f)
				{
					ScreenFader.StartFade(Color.clear, 3f);
					this.Close(true);
					if (Current.ProgramState == ProgramState.Playing)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
					}
				}
			}
		}

		// Token: 0x0600C054 RID: 49236 RVA: 0x00462858 File Offset: 0x00460A58
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			GUI.DrawTexture(rect, BaseContent.BlackTex);
			Rect rect2 = new Rect(rect);
			rect2.yMin += 30f;
			rect2.yMax -= 30f;
			rect2.xMin = rect.center.x - 400f;
			rect2.width = 800f;
			float viewWidth = this.ViewWidth;
			float viewHeight = this.ViewHeight;
			this.scrollPosition = Mathf.Clamp(this.scrollPosition, 0f, this.MaxScrollPosition);
			Widgets.BeginGroup(rect2);
			Rect rect3 = new Rect(0f, 0f, viewWidth, viewHeight);
			rect3.y -= this.scrollPosition;
			Widgets.BeginGroup(rect3);
			Text.Font = GameFont.Medium;
			float num = 0f;
			foreach (CreditsEntry creditsEntry in this.creds)
			{
				float num2 = creditsEntry.DrawHeight(rect3.width);
				Rect rect4 = new Rect(0f, num, rect3.width, num2);
				creditsEntry.Draw(rect4);
				num += num2;
			}
			Widgets.EndGroup();
			Widgets.EndGroup();
			if (this.closeDelay < 0f && this.scrollPosition > 0f && Widgets.ButtonText(new Rect(rect.xMax - 200f, rect.yMax - 100f, 150f, 50f), "SkipCredits".Translate(), true, true, true, null))
			{
				this.OnCancelKeyPressed();
			}
			if (Event.current.type == EventType.scrollWheel)
			{
				this.Scroll(Event.current.delta.y * 25f);
				Event.current.Use();
			}
			if (Event.current.type == EventType.keyDown)
			{
				if (Event.current.keyCode == KeyCode.DownArrow)
				{
					this.Scroll(250f);
					Event.current.Use();
				}
				if (Event.current.keyCode == KeyCode.UpArrow)
				{
					this.Scroll(-250f);
					Event.current.Use();
				}
			}
		}

		// Token: 0x0600C055 RID: 49237 RVA: 0x00462AC0 File Offset: 0x00460CC0
		private void Scroll(float offset)
		{
			this.scrollPosition += offset;
			this.timeUntilAutoScroll = 3f;
		}

		// Token: 0x0600C056 RID: 49238 RVA: 0x00462ADB File Offset: 0x00460CDB
		public override void OnCancelKeyPressed()
		{
			Event.current.Use();
			if (this.closeDelay > 0f)
			{
				return;
			}
			this.closeDelay = 3f;
			ScreenFader.StartFade(Color.black, 3f);
		}

		// Token: 0x04006EB2 RID: 28338
		private List<CreditsEntry> creds;

		// Token: 0x04006EB3 RID: 28339
		public bool wonGame;

		// Token: 0x04006EB4 RID: 28340
		public SongDef endCreditsSong;

		// Token: 0x04006EB5 RID: 28341
		public bool exitToMainMenu;

		// Token: 0x04006EB6 RID: 28342
		public float songStartDelay = 5f;

		// Token: 0x04006EB7 RID: 28343
		private float timeUntilAutoScroll;

		// Token: 0x04006EB8 RID: 28344
		private float scrollPosition;

		// Token: 0x04006EB9 RID: 28345
		private bool playedMusic;

		// Token: 0x04006EBA RID: 28346
		private float victoryTextHeight;

		// Token: 0x04006EBB RID: 28347
		private float closeDelay = -1f;

		// Token: 0x04006EBC RID: 28348
		public static bool creditsShowing;

		// Token: 0x04006EBD RID: 28349
		public static float creationRealtime = -1f;

		// Token: 0x04006EBE RID: 28350
		private const int ColumnWidth = 800;

		// Token: 0x04006EBF RID: 28351
		private const float InitialAutoScrollDelay = 1f;

		// Token: 0x04006EC0 RID: 28352
		private const float InitialAutoScrollDelayWonGame = 10f;

		// Token: 0x04006EC1 RID: 28353
		private const float AutoScrollDelayAfterManualScroll = 3f;

		// Token: 0x04006EC2 RID: 28354
		private const float VictoryTextScrollSpeed = 20f;

		// Token: 0x04006EC3 RID: 28355
		private const float ScrollSpeedLerpHeight = 200f;

		// Token: 0x04006EC4 RID: 28356
		private const GameFont Font = GameFont.Medium;

		// Token: 0x04006EC5 RID: 28357
		public const float DefaultSongStartDelay = 5f;

		// Token: 0x04006EC6 RID: 28358
		private const int CloseTransitionSeconds = 3;
	}
}
