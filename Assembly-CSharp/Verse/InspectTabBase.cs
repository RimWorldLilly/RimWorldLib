using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D4 RID: 212
	public abstract class InspectTabBase
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000827 RID: 2087
		protected abstract float PaneTopY { get; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000828 RID: 2088
		protected abstract bool StillValid { get; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0002F726 File Offset: 0x0002D926
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x0002F729 File Offset: 0x0002D929
		public virtual bool Hidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0002F72C File Offset: 0x0002D92C
		public virtual bool VisibleInBlueprintMode { get; } = 1;

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x0002F734 File Offset: 0x0002D934
		public string TutorHighlightTagClosed
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorHighlightTagClosed == null)
				{
					this.cachedTutorHighlightTagClosed = "ITab-" + this.tutorTag + "-Closed";
				}
				return this.cachedTutorHighlightTagClosed;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0002F76C File Offset: 0x0002D96C
		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0002F7BC File Offset: 0x0002D9BC
		public void DoTabGUI()
		{
			Rect rect = this.TabRect;
			Find.WindowStack.ImmediateWindow(235086, rect, WindowLayer.GameUI, delegate
			{
				if (!this.StillValid || !this.IsVisible)
				{
					return;
				}
				if (Widgets.CloseButtonFor(rect.AtZero()))
				{
					this.CloseTab();
				}
				try
				{
					this.FillTab();
				}
				catch (Exception ex)
				{
					string str = "Exception filling tab ";
					Type type = this.GetType();
					string str2 = (type != null) ? type.ToString() : null;
					string str3 = ": ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), 49827);
				}
			}, true, false, 1f, new Action(this.Notify_ClickOutsideWindow), false);
			this.ExtraOnGUI();
		}

		// Token: 0x0600082F RID: 2095
		protected abstract void CloseTab();

		// Token: 0x06000830 RID: 2096
		protected abstract void FillTab();

		// Token: 0x06000831 RID: 2097 RVA: 0x0002F81F File Offset: 0x0002DA1F
		protected virtual void ExtraOnGUI()
		{
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0002F821 File Offset: 0x0002DA21
		protected virtual void UpdateSize()
		{
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0002F823 File Offset: 0x0002DA23
		public virtual void OnOpen()
		{
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0002F825 File Offset: 0x0002DA25
		public virtual void TabTick()
		{
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0002F827 File Offset: 0x0002DA27
		public virtual void TabUpdate()
		{
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0002F829 File Offset: 0x0002DA29
		public virtual void Notify_ClearingAllMapsMemory()
		{
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0002F82B File Offset: 0x0002DA2B
		public virtual void Notify_ClickOutsideWindow()
		{
		}

		// Token: 0x040003B1 RID: 945
		public string labelKey;

		// Token: 0x040003B2 RID: 946
		protected Vector2 size;

		// Token: 0x040003B3 RID: 947
		public string tutorTag;

		// Token: 0x040003B4 RID: 948
		private string cachedTutorHighlightTagClosed;
	}
}
