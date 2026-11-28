using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CE3 RID: 7395
	public abstract class Dialog_ManagePolicies<T> : Window where T : Policy
	{
		// Token: 0x1700229F RID: 8863
		// (get) Token: 0x0600BE23 RID: 48675 RVA: 0x004452C3 File Offset: 0x004434C3
		// (set) Token: 0x0600BE24 RID: 48676 RVA: 0x004452CB File Offset: 0x004434CB
		protected T SelectedPolicy
		{
			get
			{
				return this.policyInt;
			}
			set
			{
				this.ValidateName();
				this.policyInt = value;
			}
		}

		// Token: 0x170022A0 RID: 8864
		// (get) Token: 0x0600BE25 RID: 48677
		protected abstract string TitleKey { get; }

		// Token: 0x170022A1 RID: 8865
		// (get) Token: 0x0600BE26 RID: 48678
		protected abstract string TipKey { get; }

		// Token: 0x170022A2 RID: 8866
		// (get) Token: 0x0600BE27 RID: 48679 RVA: 0x004452DA File Offset: 0x004434DA
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x170022A3 RID: 8867
		// (get) Token: 0x0600BE28 RID: 48680 RVA: 0x004452EB File Offset: 0x004434EB
		protected virtual float OffsetHeaderY
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600BE29 RID: 48681 RVA: 0x004452F4 File Offset: 0x004434F4
		private void ValidateName()
		{
			if (this.SelectedPolicy != null && this.SelectedPolicy.label.NullOrEmpty())
			{
				this.SelectedPolicy.label = "UnnamedPolicy".Translate();
			}
		}

		// Token: 0x0600BE2A RID: 48682 RVA: 0x00445344 File Offset: 0x00443544
		protected Dialog_ManagePolicies(T policy) : base(null)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.scroll = Vector2.zero;
			this.quickSearch.Reset();
			this.SelectedPolicy = policy;
		}

		// Token: 0x0600BE2B RID: 48683 RVA: 0x004453AE File Offset: 0x004435AE
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x0600BE2C RID: 48684 RVA: 0x004453C8 File Offset: 0x004435C8
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect;
			rect.height = 32f;
			Rect rect2 = rect;
			rect2.y = rect.yMax;
			Rect rect3 = rect2;
			rect3.y = rect2.yMax + 10f;
			rect3.height = 32f;
			rect3.width = 200f;
			Rect rect4 = rect3;
			rect4.x = rect3.xMax + 10f;
			rect4.xMax = inRect.xMax;
			Rect rect5 = rect4;
			rect5.xMax -= 100f;
			Rect rect6 = rect4;
			rect6.x = rect4.xMax - rect4.height;
			rect6.width = rect4.height;
			Rect rect7 = rect6;
			rect7.x = rect6.x - rect4.height - 10f;
			Rect rect8 = rect7;
			rect8.x = rect7.x - rect4.height - 10f;
			Rect rect9 = rect8;
			rect9.x = rect8.x - rect4.height - 10f;
			Rect leftRect = rect3;
			leftRect.y = rect3.yMax + this.OffsetHeaderY + 4f;
			leftRect.yMax = inRect.yMax - Window.CloseButSize.y - 10f;
			Rect rect10 = rect4;
			rect10.y = leftRect.y;
			rect10.yMax = leftRect.yMax;
			Rect rect11 = rect4;
			rect11.yMax = rect10.yMax;
			using (new TextBlock(GameFont.Medium))
			{
				Widgets.Label(rect, this.TitleKey.Translate());
			}
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, this.TipKey.Translate());
			using (new TextBlock(GameFont.Medium))
			{
				Widgets.Label(rect3, "AvailablePolicies".Translate());
			}
			this.DoPolicyListing(leftRect);
			if (this.SelectedPolicy != null)
			{
				this.DoContentsRect(rect10);
				bool flag = this.GetDefaultPolicy() == this.SelectedPolicy;
				string text = this.SelectedPolicy.label;
				if (flag)
				{
					string str = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("default".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(")");
					text = str + defaultInterpolatedStringHandler.ToStringAndClear().Colorize(Color.gray);
				}
				using (new TextBlock(GameFont.Medium))
				{
					Widgets.LabelEllipses(rect5, text);
				}
				TooltipHandler.TipRegionByKey(rect6, "DeletePolicyTip");
				TooltipHandler.TipRegionByKey(rect7, "DuplicatePolicyTip");
				TooltipHandler.TipRegionByKey(rect8, "RenamePolicyTip");
				if (!flag)
				{
					TooltipHandler.TipRegionByKey(rect9, "DefaultPolicyTip");
				}
				if (Widgets.ButtonImage(rect6, TexUI.DismissTex, true, null))
				{
					if (Input.GetKey(KeyCode.LeftControl))
					{
						this.DeletePolicy();
					}
					else
					{
						TaggedString taggedString = "DeletePolicyConfirm".Translate(this.SelectedPolicy.label);
						TaggedString taggedString2 = "DeletePolicyConfirmButton".Translate();
						Find.WindowStack.Add(new Dialog_Confirm(taggedString, taggedString2, new Action(this.DeletePolicy)));
					}
				}
				if (Widgets.ButtonImage(rect7, TexUI.CopyTex, true, null))
				{
					T t = this.CreateNewPolicy();
					t.CopyFrom(this.SelectedPolicy);
					this.SelectedPolicy = t;
				}
				if (Widgets.ButtonImage(rect8, TexUI.RenameTex, true, null))
				{
					Find.WindowStack.Add(new Dialog_RenamePolicy(this.SelectedPolicy));
				}
				if (!flag && Widgets.ButtonImage(rect9, TexUI.MakeDefault, true, null))
				{
					this.SetDefaultPolicy(this.SelectedPolicy);
					return;
				}
			}
			else
			{
				using (new TextBlock(new GameFont?(GameFont.Medium), new TextAnchor?(TextAnchor.MiddleCenter), new Color?(Color.gray)))
				{
					Widgets.Label(rect11, "NoPolicySelected".Translate());
				}
			}
		}

		// Token: 0x0600BE2D RID: 48685 RVA: 0x00445818 File Offset: 0x00443A18
		private void DeletePolicy()
		{
			AcceptanceReport acceptanceReport = this.TryDeletePolicy(this.SelectedPolicy);
			if (!acceptanceReport.Accepted)
			{
				Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
				return;
			}
			this.SelectedPolicy = default(T);
		}

		// Token: 0x0600BE2E RID: 48686 RVA: 0x00445860 File Offset: 0x00443A60
		private void DoPolicyListing(Rect leftRect)
		{
			Rect rect = leftRect;
			rect.y = leftRect.yMax - 24f;
			rect.height = 24f;
			Rect rect2 = leftRect;
			rect2.yMax = rect.y - 10f;
			Rect rect3 = rect2;
			rect3.xMin += 10f;
			rect3.xMax -= 10f;
			rect3.y = rect2.yMax - Window.CloseButSize.y - 10f;
			rect3.height = Window.CloseButSize.y;
			Rect outRect = rect2;
			outRect.yMax = rect3.y - 10f;
			this.quickSearch.OnGUI(rect, null, null);
			Widgets.DrawMenuSection(rect2);
			if (Widgets.ButtonText(rect3, "NewPolicy".Translate(), true, true, true, null))
			{
				this.SelectedPolicy = this.CreateNewPolicy();
			}
			int num = 0;
			foreach (T t in this.GetPolicies())
			{
				if (this.quickSearch.filter.Matches(t.label))
				{
					num++;
				}
			}
			Rect viewRect = new Rect(0f, 0f, outRect.width, (float)num * 32f);
			Widgets.AdjustRectsForScrollView(rect2, ref outRect, ref viewRect);
			Widgets.BeginScrollView(outRect, ref this.scroll, viewRect, true);
			float num2 = 0f;
			int num3 = 0;
			T defaultPolicy = this.GetDefaultPolicy();
			IEnumerable<T> policies = this.GetPolicies();
			Func<T, bool> <>9__0;
			Func<T, bool> keySelector;
			if ((keySelector = <>9__0) == null)
			{
				keySelector = (<>9__0 = ((T x) => defaultPolicy != x));
			}
			foreach (T t2 in policies.OrderBy(keySelector).ThenBy((T x) => x.label))
			{
				if (this.quickSearch.filter.Matches(t2.label))
				{
					Rect rect4 = new Rect(0f, num2, outRect.width, 32f);
					Rect rect5 = rect4;
					rect5.x += 10f;
					num2 += 32f;
					if (this.SelectedPolicy == t2)
					{
						Widgets.DrawHighlightSelected(rect4);
					}
					else if (Mouse.IsOver(rect4))
					{
						Widgets.DrawHighlight(rect4);
					}
					else if (num3 % 2 == 1)
					{
						Widgets.DrawLightHighlight(rect4);
					}
					num3++;
					string text = t2.label;
					if (defaultPolicy == t2)
					{
						text += "*".Colorize(Color.gray);
					}
					using (new TextBlock(TextAnchor.MiddleLeft))
					{
						Widgets.Label(rect5, text);
					}
					if (Widgets.ButtonInvisible(rect4, true))
					{
						this.SelectedPolicy = t2;
					}
				}
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600BE2F RID: 48687
		protected abstract T CreateNewPolicy();

		// Token: 0x0600BE30 RID: 48688
		protected abstract T GetDefaultPolicy();

		// Token: 0x0600BE31 RID: 48689
		protected abstract void SetDefaultPolicy(T policy);

		// Token: 0x0600BE32 RID: 48690
		protected abstract AcceptanceReport TryDeletePolicy(T policy);

		// Token: 0x0600BE33 RID: 48691
		protected abstract List<T> GetPolicies();

		// Token: 0x0600BE34 RID: 48692
		protected abstract void DoContentsRect(Rect rect);

		// Token: 0x0600BE35 RID: 48693 RVA: 0x00445BE4 File Offset: 0x00443DE4
		public override void PreClose()
		{
			base.PreClose();
			this.ValidateName();
		}

		// Token: 0x04006CF0 RID: 27888
		private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x04006CF1 RID: 27889
		private readonly QuickSearchWidget quickSearch = new QuickSearchWidget();

		// Token: 0x04006CF2 RID: 27890
		private Vector2 scroll;

		// Token: 0x04006CF3 RID: 27891
		private T policyInt;

		// Token: 0x04006CF4 RID: 27892
		private const float LeftPanelWidth = 200f;

		// Token: 0x04006CF5 RID: 27893
		private const float PolicyHeight = 32f;
	}
}
