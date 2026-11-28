using System;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A6 RID: 4262
	public abstract class QuestPart_AddQuest : QuestPart
	{
		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06006AA7 RID: 27303
		public abstract QuestScriptDef QuestDef { get; }

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06006AA8 RID: 27304 RVA: 0x00257B22 File Offset: 0x00255D22
		public virtual bool CanAdd
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006AA9 RID: 27305 RVA: 0x00257B25 File Offset: 0x00255D25
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.CanAdd)
			{
				this.AddQuest();
				this.PostAdd();
			}
		}

		// Token: 0x06006AAA RID: 27306 RVA: 0x00257B58 File Offset: 0x00255D58
		private void AddQuest()
		{
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(this.QuestDef, this.GetSlate());
			quest.parent = this.parent;
			quest.Accept(this.acceptee);
			if (this.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
		}

		// Token: 0x06006AAB RID: 27307
		public abstract Slate GetSlate();

		// Token: 0x06006AAC RID: 27308 RVA: 0x00257B9E File Offset: 0x00255D9E
		public virtual void PostAdd()
		{
		}

		// Token: 0x06006AAD RID: 27309 RVA: 0x00257BA0 File Offset: 0x00255DA0
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "AddQuest " + this.QuestDef.defName, true, true, true, null))
			{
				this.AddQuest();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x06006AAE RID: 27310 RVA: 0x00257C08 File Offset: 0x00255E08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.acceptee, "acceptee", false);
			Scribe_References.Look<Quest>(ref this.parent, "parent", false);
			Scribe_Values.Look<bool>(ref this.sendAvailableLetter, "sendAvailableLetter", false, false);
		}

		// Token: 0x040043C9 RID: 17353
		public string inSignal;

		// Token: 0x040043CA RID: 17354
		public Pawn acceptee;

		// Token: 0x040043CB RID: 17355
		public Quest parent;

		// Token: 0x040043CC RID: 17356
		public bool sendAvailableLetter;
	}
}
