using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

// Token: 0x0200001A RID: 26
public class Screen_ArchonexusSettlementCinematics : Window
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060000CF RID: 207 RVA: 0x000082EB File Offset: 0x000064EB
	public override Vector2 InitialSize
	{
		get
		{
			return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060000D0 RID: 208 RVA: 0x000082FE File Offset: 0x000064FE
	protected override float Margin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000D1 RID: 209 RVA: 0x00008305 File Offset: 0x00006505
	private float FadeToBlackEndTime
	{
		get
		{
			return this.ScreenStartTime + 2f + 0.2f;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060000D2 RID: 210 RVA: 0x00008319 File Offset: 0x00006519
	private float MessageDisplayEndTime
	{
		get
		{
			return this.FadeToBlackEndTime + 7f;
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00008327 File Offset: 0x00006527
	public Screen_ArchonexusSettlementCinematics(Action cameraJumpAction, Action nextStepAction) : base(null)
	{
		this.doWindowBackground = false;
		this.doCloseButton = false;
		this.doCloseX = false;
		this.forcePause = true;
		this.cameraJumpAction = cameraJumpAction;
		this.nextStepAction = nextStepAction;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0000835C File Offset: 0x0000655C
	public override void PreOpen()
	{
		base.PreOpen();
		Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(11.2f, 1.5f, false);
		SoundDefOf.ArchonexusNewColonyAccept.PlayOneShotOnCamera(null);
		ScreenFader.StartFade(Color.black, 2f);
		this.ScreenStartTime = Time.realtimeSinceStartup;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x000083A9 File Offset: 0x000065A9
	public override void PostClose()
	{
		base.PostOpen();
		ScreenFader.SetColor(Color.black);
		this.nextStepAction();
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x000083C8 File Offset: 0x000065C8
	public override void DoWindowContents(Rect inRect)
	{
		if (!this.IsFinishedFadingIn())
		{
			return;
		}
		if (!this.FadeInLatch)
		{
			this.FadeInLatch = true;
			this.cameraJumpAction();
			ScreenFader.SetColor(Color.clear);
		}
		if (this.IsFinishedDisplayMessage())
		{
			this.Close(false);
			return;
		}
		Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
		GUI.DrawTexture(rect, BaseContent.BlackTex);
		Rect rect2 = new Rect(rect);
		rect2.xMin = rect.center.x - 400f;
		rect2.width = 800f;
		rect2.yMin = rect.center.y;
		GameFont font = Text.Font;
		TextAnchor anchor = Text.Anchor;
		Color color = GUI.color;
		Text.Font = GameFont.Medium;
		GUI.color = Color.white;
		Text.Anchor = TextAnchor.MiddleCenter;
		Widgets.Label(new Rect(inRect), "SoldColonyDescription".Translate());
		Text.Font = font;
		GUI.color = color;
		Text.Anchor = anchor;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x000084C5 File Offset: 0x000066C5
	public bool IsFinishedFadingIn()
	{
		return Time.realtimeSinceStartup > this.FadeToBlackEndTime;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000084D4 File Offset: 0x000066D4
	public bool IsFinishedDisplayMessage()
	{
		return Time.realtimeSinceStartup > this.MessageDisplayEndTime;
	}

	// Token: 0x04000032 RID: 50
	private Action cameraJumpAction;

	// Token: 0x04000033 RID: 51
	private Action nextStepAction;

	// Token: 0x04000034 RID: 52
	private bool FadeInLatch;

	// Token: 0x04000035 RID: 53
	private float ScreenStartTime;

	// Token: 0x04000036 RID: 54
	public const float FadeSecs = 2f;

	// Token: 0x04000037 RID: 55
	private const float MessageDisplaySecs = 7f;

	// Token: 0x04000038 RID: 56
	private const float FadeBuffer = 0.2f;

	// Token: 0x04000039 RID: 57
	private const int MessageWidth = 800;
}
