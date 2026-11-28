using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9E RID: 7582
	public abstract class MainTabWindow_PawnTable : MainTabWindow
	{
		// Token: 0x17002497 RID: 9367
		// (get) Token: 0x0600C4C7 RID: 50375 RVA: 0x00480BE1 File Offset: 0x0047EDE1
		protected virtual float ExtraBottomSpace
		{
			get
			{
				return 53f;
			}
		}

		// Token: 0x17002498 RID: 9368
		// (get) Token: 0x0600C4C8 RID: 50376 RVA: 0x00480BE8 File Offset: 0x0047EDE8
		protected virtual float ExtraTopSpace
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002499 RID: 9369
		// (get) Token: 0x0600C4C9 RID: 50377
		protected abstract PawnTableDef PawnTableDef { get; }

		// Token: 0x1700249A RID: 9370
		// (get) Token: 0x0600C4CA RID: 50378 RVA: 0x00480BEF File Offset: 0x0047EDEF
		protected override float Margin
		{
			get
			{
				return 6f;
			}
		}

		// Token: 0x1700249B RID: 9371
		// (get) Token: 0x0600C4CB RID: 50379 RVA: 0x00480BF8 File Offset: 0x0047EDF8
		public override Vector2 RequestedTabSize
		{
			get
			{
				if (this.table == null)
				{
					return Vector2.zero;
				}
				return new Vector2(this.table.Size.x + this.Margin * 2f, this.table.Size.y + this.ExtraBottomSpace + this.ExtraTopSpace + this.Margin * 2f);
			}
		}

		// Token: 0x1700249C RID: 9372
		// (get) Token: 0x0600C4CC RID: 50380 RVA: 0x00480C60 File Offset: 0x0047EE60
		protected virtual IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.FreeColonists;
			}
		}

		// Token: 0x0600C4CD RID: 50381 RVA: 0x00480C71 File Offset: 0x0047EE71
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.table == null)
			{
				this.table = this.CreateTable();
			}
			this.SetDirty();
		}

		// Token: 0x0600C4CE RID: 50382 RVA: 0x00480C93 File Offset: 0x0047EE93
		public override void DoWindowContents(Rect rect)
		{
			this.table.PawnTableOnGUI(new Vector2(rect.x, rect.y + this.ExtraTopSpace));
		}

		// Token: 0x0600C4CF RID: 50383 RVA: 0x00480CBA File Offset: 0x0047EEBA
		public void Notify_PawnsChanged()
		{
			this.SetDirty();
		}

		// Token: 0x0600C4D0 RID: 50384 RVA: 0x00480CC2 File Offset: 0x0047EEC2
		public override void Notify_ResolutionChanged()
		{
			this.table = this.CreateTable();
			base.Notify_ResolutionChanged();
		}

		// Token: 0x0600C4D1 RID: 50385 RVA: 0x00480CD8 File Offset: 0x0047EED8
		private PawnTable CreateTable()
		{
			return (PawnTable)Activator.CreateInstance(this.PawnTableDef.workerClass, new object[]
			{
				this.PawnTableDef,
				new Func<IEnumerable<Pawn>>(() => this.Pawns),
				UI.screenWidth - (int)(this.Margin * 2f),
				(int)((float)(UI.screenHeight - 35) - this.ExtraBottomSpace - this.ExtraTopSpace - this.Margin * 2f)
			});
		}

		// Token: 0x0600C4D2 RID: 50386 RVA: 0x00480D60 File Offset: 0x0047EF60
		protected void SetDirty()
		{
			this.table.SetDirty();
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x04007068 RID: 28776
		private PawnTable table;
	}
}
