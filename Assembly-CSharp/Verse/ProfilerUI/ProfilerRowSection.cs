using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.ProfilerUI
{
	// Token: 0x0200079C RID: 1948
	public abstract class ProfilerRowSection
	{
		// Token: 0x06003E18 RID: 15896 RVA: 0x0018BB58 File Offset: 0x00189D58
		protected ProfilerRowSection(ProfilerSession session)
		{
			this.session = session;
			this.View = new ProfilerRowListView(session)
			{
				ToggleExpansion = new Action<int>(this.ToggleExpansionAt),
				ContextMenu = new Action(this.OpenContextMenu)
			};
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06003E19 RID: 15897
		protected abstract string CopyHeader { get; }

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06003E1A RID: 15898 RVA: 0x0018BB98 File Offset: 0x00189D98
		protected virtual bool SupportsSubtreeCopy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003E1B RID: 15899
		public abstract void Refresh();

		// Token: 0x06003E1C RID: 15900
		public abstract void Invalidate();

		// Token: 0x06003E1D RID: 15901
		public abstract void Notify_SearchChanged();

		// Token: 0x06003E1E RID: 15902
		public abstract void ExpandAll();

		// Token: 0x06003E1F RID: 15903
		public abstract void CollapseAll();

		// Token: 0x06003E20 RID: 15904
		protected abstract void ToggleExpansionAt(int index);

		// Token: 0x06003E21 RID: 15905 RVA: 0x0018BB9B File Offset: 0x00189D9B
		public virtual void CollapseSelected()
		{
			this.View.CollapseSelected(true);
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x0018BBA9 File Offset: 0x00189DA9
		public virtual void Clear()
		{
			this.View.SelectQuietly(-1);
			this.View.Resize(0);
			this.Invalidate();
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x0018BBC9 File Offset: 0x00189DC9
		protected virtual void AppendCopyContext(StringBuilder builder)
		{
		}

		// Token: 0x06003E24 RID: 15908
		protected abstract bool BuildFullSubtree(int index, List<ProfilerRowData> into);

		// Token: 0x06003E25 RID: 15909 RVA: 0x0018BBCC File Offset: 0x00189DCC
		private void CopySelectedSubtree(bool full)
		{
			int selectedIndex = this.View.SelectedIndex;
			if (selectedIndex < 0 || selectedIndex >= this.View.Rows.Count)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.CopyHeader);
			List<ProfilerRowData> list = new List<ProfilerRowData>();
			if (full && this.BuildFullSubtree(selectedIndex, list))
			{
				ProfilerClipboard.AppendSubtree(stringBuilder, list, 0);
			}
			else
			{
				ProfilerClipboard.AppendSubtree(stringBuilder, this.View.Rows, selectedIndex);
			}
			ProfilerClipboard.Put(stringBuilder);
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x0018BC45 File Offset: 0x00189E45
		public void AppendCopyText(StringBuilder builder)
		{
			this.AppendCopyContext(builder);
			builder.AppendLine(this.CopyHeader);
			ProfilerClipboard.AppendRows(builder, this.View.Rows);
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x0018BC6C File Offset: 0x00189E6C
		public virtual void Draw(Rect rect)
		{
			GUI.BeginGroup(rect);
			try
			{
				float footerHeight = this.FooterHeight;
				this.View.DrawHeader(new Rect(0f, 0f, rect.width, 22f));
				this.View.DrawRows(new Rect(0f, 22f, rect.width, rect.height - 22f - 24f - footerHeight));
				if (footerHeight > 0f)
				{
					this.DrawFooter(new Rect(0f, rect.height - 24f - footerHeight, rect.width, footerHeight));
				}
				this.View.DrawBreadcrumbs(new Rect(0f, rect.height - 24f, rect.width, 24f));
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06003E28 RID: 15912 RVA: 0x0018BD58 File Offset: 0x00189F58
		protected virtual float FooterHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x0018BD5F File Offset: 0x00189F5F
		protected virtual void DrawFooter(Rect rect)
		{
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x0018BD64 File Offset: 0x00189F64
		private void OpenContextMenu()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>
			{
				new FloatMenuOption("Copy row", delegate()
				{
					this.View.CopySelectedRow();
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			};
			if (this.SupportsSubtreeCopy)
			{
				list.Add(new FloatMenuOption("Copy subtree", delegate()
				{
					this.CopySelectedSubtree(true);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list.Add(new FloatMenuOption("Copy visible subtree", delegate()
				{
					this.CopySelectedSubtree(false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			list.Add(new FloatMenuOption("Copy view", delegate()
			{
				Action copyViewRequested = this.CopyViewRequested;
				if (copyViewRequested == null)
				{
					return;
				}
				copyViewRequested();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x040028F7 RID: 10487
		protected readonly ProfilerSession session;

		// Token: 0x040028F8 RID: 10488
		public readonly ProfilerRowListView View;

		// Token: 0x040028F9 RID: 10489
		public Action CopyViewRequested;
	}
}
