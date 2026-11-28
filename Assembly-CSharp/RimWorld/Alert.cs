using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B9C RID: 7068
	[StaticConstructorOnStartup]
	public abstract class Alert
	{
		// Token: 0x170020C7 RID: 8391
		// (get) Token: 0x0600B56F RID: 46447 RVA: 0x003FE32E File Offset: 0x003FC52E
		public virtual AlertPriority Priority
		{
			get
			{
				return this.defaultPriority;
			}
		}

		// Token: 0x170020C8 RID: 8392
		// (get) Token: 0x0600B570 RID: 46448 RVA: 0x003FE336 File Offset: 0x003FC536
		protected virtual Color BGColor
		{
			get
			{
				return Color.clear;
			}
		}

		// Token: 0x170020C9 RID: 8393
		// (get) Token: 0x0600B571 RID: 46449 RVA: 0x003FE33D File Offset: 0x003FC53D
		public bool Active
		{
			get
			{
				return this.cachedActive;
			}
		}

		// Token: 0x170020CA RID: 8394
		// (get) Token: 0x0600B572 RID: 46450 RVA: 0x003FE345 File Offset: 0x003FC545
		public string Label
		{
			get
			{
				if (!this.Active)
				{
					return string.Empty;
				}
				return this.cachedLabel;
			}
		}

		// Token: 0x170020CB RID: 8395
		// (get) Token: 0x0600B573 RID: 46451 RVA: 0x003FE35B File Offset: 0x003FC55B
		public float Height
		{
			get
			{
				Text.Font = GameFont.Small;
				return Text.CalcHeight(this.Label, 148f);
			}
		}

		// Token: 0x170020CC RID: 8396
		// (get) Token: 0x0600B574 RID: 46452 RVA: 0x003FE374 File Offset: 0x003FC574
		public bool EnabledWithActiveExpansions
		{
			get
			{
				return (!this.requireRoyalty || ModsConfig.RoyaltyActive) && (!this.requireIdeology || ModsConfig.IdeologyActive) && (!this.requireBiotech || ModsConfig.BiotechActive) && (!this.requireAnomaly || ModsConfig.AnomalyActive);
			}
		}

		// Token: 0x0600B575 RID: 46453
		public abstract AlertReport GetReport();

		// Token: 0x0600B576 RID: 46454 RVA: 0x003FE3C6 File Offset: 0x003FC5C6
		public virtual TaggedString GetExplanation()
		{
			return this.defaultExplanation;
		}

		// Token: 0x0600B577 RID: 46455 RVA: 0x003FE3D3 File Offset: 0x003FC5D3
		public virtual string GetLabel()
		{
			return this.defaultLabel;
		}

		// Token: 0x170020CD RID: 8397
		// (get) Token: 0x0600B578 RID: 46456 RVA: 0x003FE3DB File Offset: 0x003FC5DB
		public virtual string GetJumpToTargetsText
		{
			get
			{
				return "ClickToJumpToProblem".Translate();
			}
		}

		// Token: 0x0600B579 RID: 46457 RVA: 0x003FE3EC File Offset: 0x003FC5EC
		public void Notify_Started()
		{
			if (this.Priority >= AlertPriority.High)
			{
				if (this.alertBounce == null)
				{
					this.alertBounce = new AlertBounce();
				}
				this.alertBounce.DoAlertStartEffect();
				if (Time.timeSinceLevelLoad > 1f && Time.realtimeSinceStartup > this.lastBellTime + 0.5f)
				{
					SoundDefOf.TinyBell.PlayOneShotOnCamera(null);
					this.lastBellTime = Time.realtimeSinceStartup;
				}
			}
		}

		// Token: 0x0600B57A RID: 46458 RVA: 0x003FE458 File Offset: 0x003FC658
		public void Recalculate()
		{
			AlertReport report = this.GetReport();
			this.cachedActive = report.active;
			if (report.active)
			{
				this.cachedLabel = this.GetLabel();
			}
		}

		// Token: 0x0600B57B RID: 46459 RVA: 0x003FE48C File Offset: 0x003FC68C
		public virtual void AlertActiveUpdate()
		{
		}

		// Token: 0x0600B57C RID: 46460 RVA: 0x003FE490 File Offset: 0x003FC690
		public virtual Rect DrawAt(float topY, bool minimized)
		{
			Rect rect = new Rect((float)UI.screenWidth - 154f, topY, 154f, this.Height);
			if (this.alertBounce != null)
			{
				rect.x -= this.alertBounce.CalculateHorizontalOffset();
			}
			GUI.color = this.BGColor;
			GUI.DrawTexture(rect, Alert.AlertBGTex);
			GUI.color = Color.white;
			Widgets.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(new Rect(0f, 0f, 148f, this.Height), this.Label);
			Widgets.EndGroup();
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, Alert.AlertBGTexHighlight);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.OnClick();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return rect;
		}

		// Token: 0x0600B57D RID: 46461 RVA: 0x003FE55C File Offset: 0x003FC75C
		protected virtual void OnClick()
		{
			IEnumerable<GlobalTargetInfo> allCulprits = this.GetReport().AllCulprits;
			if (allCulprits != null)
			{
				Alert.tmpTargets.Clear();
				foreach (GlobalTargetInfo item in allCulprits)
				{
					if (item.IsValid)
					{
						Alert.tmpTargets.Add(item);
					}
				}
				if (Alert.tmpTargets.Any<GlobalTargetInfo>())
				{
					if (Event.current.button == 1)
					{
						this.jumpToTargetCycleIndex--;
					}
					else
					{
						this.jumpToTargetCycleIndex++;
					}
					CameraJumper.TryJumpAndSelect(Alert.tmpTargets[GenMath.PositiveMod(this.jumpToTargetCycleIndex, Alert.tmpTargets.Count)], CameraJumper.MovementMode.Pan);
					Alert.tmpTargets.Clear();
				}
			}
		}

		// Token: 0x0600B57E RID: 46462 RVA: 0x003FE638 File Offset: 0x003FC838
		public void DrawInfoPane()
		{
			Alert.<>c__DisplayClass40_0 CS$<>8__locals1 = new Alert.<>c__DisplayClass40_0();
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			this.Recalculate();
			if (!this.Active)
			{
				return;
			}
			CS$<>8__locals1.expString = this.GetExplanation();
			if (CS$<>8__locals1.expString.NullOrEmpty())
			{
				return;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.GetReport().AnyCulpritValid)
			{
				CS$<>8__locals1.expString += "\n\n(" + this.GetJumpToTargetsText + ")";
			}
			float num = Text.CalcHeight(CS$<>8__locals1.expString, 310f);
			num += 20f;
			CS$<>8__locals1.infoRect = new Rect((float)UI.screenWidth - 154f - 330f - 8f, Mathf.Max(Mathf.Min(Event.current.mousePosition.y, (float)UI.screenHeight - num), 0f), 330f, num);
			if (CS$<>8__locals1.infoRect.yMax > (float)UI.screenHeight)
			{
				Alert.<>c__DisplayClass40_0 CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals2.infoRect.y = CS$<>8__locals2.infoRect.y - ((float)UI.screenHeight - CS$<>8__locals1.infoRect.yMax);
			}
			if (CS$<>8__locals1.infoRect.y < 0f)
			{
				CS$<>8__locals1.infoRect.y = 0f;
			}
			Find.WindowStack.ImmediateWindow(138956, CS$<>8__locals1.infoRect, WindowLayer.Super, delegate
			{
				Text.Font = GameFont.Small;
				Rect rect = CS$<>8__locals1.infoRect.AtZero();
				Widgets.DrawWindowBackground(rect);
				Rect rect2 = rect.ContractedBy(10f);
				Widgets.BeginGroup(rect2);
				Widgets.Label(new Rect(0f, 0f, rect2.width, rect2.height), CS$<>8__locals1.expString);
				Widgets.EndGroup();
			}, false, false, 1f, null, false);
		}

		// Token: 0x0400684C RID: 26700
		protected AlertPriority defaultPriority;

		// Token: 0x0400684D RID: 26701
		protected string defaultLabel;

		// Token: 0x0400684E RID: 26702
		protected string defaultExplanation;

		// Token: 0x0400684F RID: 26703
		protected bool requireRoyalty;

		// Token: 0x04006850 RID: 26704
		protected bool requireIdeology;

		// Token: 0x04006851 RID: 26705
		protected bool requireBiotech;

		// Token: 0x04006852 RID: 26706
		protected bool requireAnomaly;

		// Token: 0x04006853 RID: 26707
		protected float lastBellTime = -1000f;

		// Token: 0x04006854 RID: 26708
		private int jumpToTargetCycleIndex = -1;

		// Token: 0x04006855 RID: 26709
		private bool cachedActive;

		// Token: 0x04006856 RID: 26710
		private string cachedLabel;

		// Token: 0x04006857 RID: 26711
		private AlertBounce alertBounce;

		// Token: 0x04006858 RID: 26712
		public const float Width = 154f;

		// Token: 0x04006859 RID: 26713
		private const float TextWidth = 148f;

		// Token: 0x0400685A RID: 26714
		public const float InfoRectWidth = 330f;

		// Token: 0x0400685B RID: 26715
		private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x0400685C RID: 26716
		private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;

		// Token: 0x0400685D RID: 26717
		private static List<GlobalTargetInfo> tmpTargets = new List<GlobalTargetInfo>();
	}
}
