using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAF RID: 7343
	public abstract class Dialog_GiveName : Window
	{
		// Token: 0x1700223F RID: 8767
		// (get) Token: 0x0600BC61 RID: 48225 RVA: 0x00435176 File Offset: 0x00433376
		private float Height
		{
			get
			{
				if (!this.useSecondName)
				{
					return 200f;
				}
				return 300f;
			}
		}

		// Token: 0x17002240 RID: 8768
		// (get) Token: 0x0600BC62 RID: 48226 RVA: 0x0043518B File Offset: 0x0043338B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, this.Height);
			}
		}

		// Token: 0x17002241 RID: 8769
		// (get) Token: 0x0600BC63 RID: 48227 RVA: 0x0043519D File Offset: 0x0043339D
		protected virtual int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x17002242 RID: 8770
		// (get) Token: 0x0600BC64 RID: 48228 RVA: 0x004351A1 File Offset: 0x004333A1
		protected virtual int SecondCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x0600BC65 RID: 48229 RVA: 0x004351A8 File Offset: 0x004333A8
		public Dialog_GiveName() : base(null)
		{
			if (Find.AnyPlayerHomeMap != null && Find.AnyPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
			{
				if (Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
				}
				else
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonists.RandomElement<Pawn>();
				}
			}
			else
			{
				this.suggestingPawn = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended.RandomElement<Pawn>();
			}
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BC66 RID: 48230 RVA: 0x0043524C File Offset: 0x0043344C
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.keyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				flag = true;
				Event.current.Use();
			}
			Rect rect2;
			if (!this.useSecondName)
			{
				Widgets.Label(new Rect(0f, 0f, rect.width, rect.height), this.nameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn).CapitalizeFirst());
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, 80f, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true, null))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, 80f, rect.width / 2f + 70f, 35f), this.curName, this.FirstCharLimit, null);
				rect2 = new Rect(rect.width / 2f + 90f, rect.height - 35f, rect.width / 2f - 90f, 35f);
			}
			else
			{
				float num = 0f;
				string text = this.nameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn).CapitalizeFirst();
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, num, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true, null))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, num, rect.width / 2f + 70f, 35f), this.curName, this.FirstCharLimit, null);
				num += 60f;
				text = this.secondNameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn);
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.secondNameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, num, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true, null))
				{
					this.curSecondName = this.secondNameGenerator();
				}
				this.curSecondName = Widgets.TextField(new Rect(0f, num, rect.width / 2f + 70f, 35f), this.curSecondName, this.SecondCharLimit, null);
				num += 45f;
				float num2 = rect.width / 2f - 90f;
				rect2 = new Rect(rect.width / 2f - num2 / 2f, rect.height - 35f, num2, 35f);
			}
			if (Widgets.ButtonText(rect2, "OK".Translate(), true, true, true, null) || flag)
			{
				string text2 = this.curName;
				string text3 = (text2 != null) ? text2.Trim() : null;
				string text4 = this.curSecondName;
				string text5 = (text4 != null) ? text4.Trim() : null;
				if (this.IsValidName(text3) && (!this.useSecondName || this.IsValidSecondName(text5)))
				{
					if (this.useSecondName)
					{
						this.Named(text3);
						this.NamedSecond(text5);
						Messages.Message(this.gainedNameMessageKey.Translate(text3, text5), MessageTypeDefOf.TaskCompletion, false);
					}
					else
					{
						this.Named(text3);
						Messages.Message(this.gainedNameMessageKey.Translate(text3), MessageTypeDefOf.TaskCompletion, false);
					}
					Find.WindowStack.TryRemove(this, true);
				}
				else
				{
					Messages.Message(this.invalidNameMessageKey.Translate(), MessageTypeDefOf.RejectInput, false);
				}
				Event.current.Use();
			}
		}

		// Token: 0x0600BC67 RID: 48231
		protected abstract bool IsValidName(string s);

		// Token: 0x0600BC68 RID: 48232
		protected abstract void Named(string s);

		// Token: 0x0600BC69 RID: 48233 RVA: 0x00435780 File Offset: 0x00433980
		protected virtual bool IsValidSecondName(string s)
		{
			return true;
		}

		// Token: 0x0600BC6A RID: 48234 RVA: 0x00435783 File Offset: 0x00433983
		protected virtual void NamedSecond(string s)
		{
		}

		// Token: 0x04006BA6 RID: 27558
		protected Pawn suggestingPawn;

		// Token: 0x04006BA7 RID: 27559
		protected string curName;

		// Token: 0x04006BA8 RID: 27560
		protected Func<string> nameGenerator;

		// Token: 0x04006BA9 RID: 27561
		protected string nameMessageKey;

		// Token: 0x04006BAA RID: 27562
		protected string gainedNameMessageKey;

		// Token: 0x04006BAB RID: 27563
		protected string invalidNameMessageKey;

		// Token: 0x04006BAC RID: 27564
		protected bool useSecondName;

		// Token: 0x04006BAD RID: 27565
		protected string curSecondName;

		// Token: 0x04006BAE RID: 27566
		protected Func<string> secondNameGenerator;

		// Token: 0x04006BAF RID: 27567
		protected string secondNameMessageKey;

		// Token: 0x04006BB0 RID: 27568
		protected string invalidSecondNameMessageKey;
	}
}
