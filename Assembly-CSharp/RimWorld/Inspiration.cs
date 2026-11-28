using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A1 RID: 2465
	public class Inspiration : IExposable
	{
		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06004AA3 RID: 19107 RVA: 0x001D1414 File Offset: 0x001CF614
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06004AA4 RID: 19108 RVA: 0x001D141C File Offset: 0x001CF61C
		public float AgeDays
		{
			get
			{
				return (float)this.age / 60000f;
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06004AA5 RID: 19109 RVA: 0x001D142C File Offset: 0x001CF62C
		public virtual string InspectLine
		{
			get
			{
				int numTicks = (int)((this.def.baseDurationDays - this.AgeDays) * 60000f);
				return this.def.baseInspectLine + " (" + "ExpiresIn".Translate() + ": " + numTicks.ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06004AA6 RID: 19110 RVA: 0x001D14A0 File Offset: 0x001CF6A0
		public virtual string LetterText
		{
			get
			{
				string text = this.def.beginLetter.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).Resolve();
				if (!string.IsNullOrWhiteSpace(this.reason))
				{
					text = this.reason.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).Resolve() + "\n\n" + text;
				}
				return text;
			}
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x001D155B File Offset: 0x001CF75B
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<InspirationDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x001D1591 File Offset: 0x001CF791
		public virtual void InspirationTick(int delta)
		{
			this.age += delta;
			if (this.AgeDays >= this.def.baseDurationDays)
			{
				this.End();
			}
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x001D15BA File Offset: 0x001CF7BA
		public virtual void PostStart(bool sendLetter = true)
		{
			if (sendLetter)
			{
				this.SendBeginLetter();
			}
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x001D15C5 File Offset: 0x001CF7C5
		public virtual void PostEnd()
		{
			this.AddEndMessage();
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x001D15CD File Offset: 0x001CF7CD
		protected void End()
		{
			this.pawn.mindState.inspirationHandler.EndInspiration(this);
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x001D15E8 File Offset: 0x001CF7E8
		protected virtual void SendBeginLetter()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return;
			}
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				return;
			}
			string str = (this.def.beginLetterLabel ?? this.def.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
			Find.LetterStack.ReceiveLetter(str, this.LetterText, this.def.beginLetterDef, this.pawn, null, null, null, null, 0, true);
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x001D1688 File Offset: 0x001CF888
		protected virtual void AddEndMessage()
		{
			if (this.def.endMessage.NullOrEmpty())
			{
				return;
			}
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				return;
			}
			Messages.Message(this.def.endMessage.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x04002F98 RID: 12184
		public Pawn pawn;

		// Token: 0x04002F99 RID: 12185
		public InspirationDef def;

		// Token: 0x04002F9A RID: 12186
		private int age;

		// Token: 0x04002F9B RID: 12187
		public string reason;
	}
}
