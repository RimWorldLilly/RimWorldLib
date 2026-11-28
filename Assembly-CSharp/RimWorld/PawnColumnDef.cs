using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB6 RID: 4022
	public class PawnColumnDef : Def
	{
		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06006626 RID: 26150 RVA: 0x00244FF4 File Offset: 0x002431F4
		public PawnColumnWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnColumnWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x06006627 RID: 26151 RVA: 0x00245026 File Offset: 0x00243226
		public Texture2D HeaderIcon
		{
			get
			{
				if (this.headerIconTex == null && !this.headerIcon.NullOrEmpty())
				{
					this.headerIconTex = ContentFinder<Texture2D>.Get(this.headerIcon, true);
				}
				return this.headerIconTex;
			}
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x06006628 RID: 26152 RVA: 0x0024505C File Offset: 0x0024325C
		public Vector2 HeaderIconSize
		{
			get
			{
				if (this.headerIconSize != default(Vector2))
				{
					return this.headerIconSize;
				}
				if (this.HeaderIcon != null)
				{
					return PawnColumnDef.IconSize;
				}
				return Vector2.zero;
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x06006629 RID: 26153 RVA: 0x0024509F File Offset: 0x0024329F
		public bool HeaderInteractable
		{
			get
			{
				return this.sortable || !this.headerTip.NullOrEmpty() || this.headerAlwaysInteractable;
			}
		}

		// Token: 0x04003E82 RID: 16002
		public Type workerClass = typeof(PawnColumnWorker);

		// Token: 0x04003E83 RID: 16003
		public bool sortable;

		// Token: 0x04003E84 RID: 16004
		public bool ignoreWhenCalculatingOptimalTableSize;

		// Token: 0x04003E85 RID: 16005
		[NoTranslate]
		public string headerIcon;

		// Token: 0x04003E86 RID: 16006
		public Vector2 headerIconSize;

		// Token: 0x04003E87 RID: 16007
		[MustTranslate]
		public string headerTip;

		// Token: 0x04003E88 RID: 16008
		public bool headerAlwaysInteractable;

		// Token: 0x04003E89 RID: 16009
		public bool paintable;

		// Token: 0x04003E8A RID: 16010
		public bool groupable;

		// Token: 0x04003E8B RID: 16011
		public TrainableDef trainable;

		// Token: 0x04003E8C RID: 16012
		public int gap;

		// Token: 0x04003E8D RID: 16013
		public WorkTypeDef workType;

		// Token: 0x04003E8E RID: 16014
		public bool moveWorkTypeLabelDown;

		// Token: 0x04003E8F RID: 16015
		public bool showIcon;

		// Token: 0x04003E90 RID: 16016
		public bool useLabelShort;

		// Token: 0x04003E91 RID: 16017
		public int widthPriority;

		// Token: 0x04003E92 RID: 16018
		public int width = -1;

		// Token: 0x04003E93 RID: 16019
		[Unsaved(false)]
		private PawnColumnWorker workerInt;

		// Token: 0x04003E94 RID: 16020
		[Unsaved(false)]
		private Texture2D headerIconTex;

		// Token: 0x04003E95 RID: 16021
		private const int IconWidth = 26;

		// Token: 0x04003E96 RID: 16022
		private static readonly Vector2 IconSize = new Vector2(26f, 26f);
	}
}
