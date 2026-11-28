using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000EB2 RID: 3762
	[StaticConstructorOnStartup]
	public class BookOutcomeDoer_GiveQuest : BookOutcomeDoer
	{
		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x06006309 RID: 25353 RVA: 0x0023A229 File Offset: 0x00238429
		public new BookOutcomeProperties_GiveQuest Props
		{
			get
			{
				return (BookOutcomeProperties_GiveQuest)this.props;
			}
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x0600630A RID: 25354 RVA: 0x0023A236 File Offset: 0x00238436
		private bool QuestGiven
		{
			get
			{
				return this.quest != null;
			}
		}

		// Token: 0x0600630B RID: 25355 RVA: 0x0023A241 File Offset: 0x00238441
		public override bool BenefitDetailsCanChange(Pawn reader = null)
		{
			return !this.QuestGiven && this.hasQuest;
		}

		// Token: 0x0600630C RID: 25356 RVA: 0x0023A254 File Offset: 0x00238454
		public override string GetBenefitsString(Pawn reader = null)
		{
			if (!this.hasQuest)
			{
				return null;
			}
			TaggedString taggedString = " - " + "BookHasQuest".Translate();
			if (this.QuestGiven)
			{
				TaggedString t = taggedString;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("DiscoveredQuest".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				taggedString = t + defaultInterpolatedStringHandler.ToStringAndClear();
				taggedString = taggedString.Colorize(Color.gray);
			}
			return taggedString.Resolve();
		}

		// Token: 0x0600630D RID: 25357 RVA: 0x0023A2E0 File Offset: 0x002384E0
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			return this.hasQuest && !this.QuestGiven;
		}

		// Token: 0x0600630E RID: 25358 RVA: 0x0023A2F8 File Offset: 0x002384F8
		public override void OnBookGenerated(Pawn author = null)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.hasQuest = Rand.Chance(this.Props.questChance);
			if (this.hasQuest)
			{
				this.questDef = this.GetQuestDef();
			}
			if (this.questDef == null)
			{
				this.hasQuest = false;
			}
		}

		// Token: 0x0600630F RID: 25359 RVA: 0x0023A348 File Offset: 0x00238548
		private QuestScriptDef GetQuestDef()
		{
			IEnumerable<QuestScriptDef> giverQuests = QuestUtility.GetGiverQuests(QuestGiverTag.Reading);
			if (giverQuests.EnumerableNullOrEmpty<QuestScriptDef>())
			{
				return null;
			}
			return giverQuests.RandomElementByWeight((QuestScriptDef q) => q.rootSelectionWeight);
		}

		// Token: 0x06006310 RID: 25360 RVA: 0x0023A38B File Offset: 0x0023858B
		public override void OnReadingTick(Pawn reader, float factor)
		{
			if (!this.hasQuest || this.QuestGiven)
			{
				return;
			}
			if (Rand.MTBEventOccurs(12500f, 1f, 1f) || this.giveNext)
			{
				this.GenerateQuest(reader);
			}
		}

		// Token: 0x06006311 RID: 25361 RVA: 0x0023A3C4 File Offset: 0x002385C4
		private void GenerateQuest(Pawn reader)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(reader.Map), false);
			slate.Set<TaggedString>("discoveryMethod", "QuestDiscoveredFromBook".Translate(base.Book.Named("BOOK"), reader.Named("READER")), false);
			if (this.questDef == null)
			{
				this.questDef = this.GetQuestDef();
			}
			if (this.questDef == null)
			{
				return;
			}
			this.quest = QuestUtility.GenerateQuestAndMakeAvailable(this.questDef, slate);
			Messages.Message("MessageBookGaveQuest".Translate(this.quest.name, base.Book.Named("BOOK"), reader.Named("READER")), MessageTypeDefOf.PositiveEvent, true);
			if (!this.quest.hidden && this.quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(this.quest, slate.Get<string>("discoveryMethod", null, false));
			}
		}

		// Token: 0x06006312 RID: 25362 RVA: 0x0023A4CA File Offset: 0x002386CA
		public override IEnumerable<Gizmo> GetGizmos()
		{
			BookOutcomeDoer_GiveQuest.<GetGizmos>d__17 <GetGizmos>d__ = new BookOutcomeDoer_GiveQuest.<GetGizmos>d__17(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06006313 RID: 25363 RVA: 0x0023A4DA File Offset: 0x002386DA
		public override IEnumerable<RulePack> GetTopicRulePacks()
		{
			BookOutcomeDoer_GiveQuest.<GetTopicRulePacks>d__18 <GetTopicRulePacks>d__ = new BookOutcomeDoer_GiveQuest.<GetTopicRulePacks>d__18(-2);
			<GetTopicRulePacks>d__.<>4__this = this;
			return <GetTopicRulePacks>d__;
		}

		// Token: 0x06006314 RID: 25364 RVA: 0x0023A4EA File Offset: 0x002386EA
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.hasQuest, "hasQuest", false, false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.questDef, "questDef");
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x0400369A RID: 13978
		private bool hasQuest;

		// Token: 0x0400369B RID: 13979
		private QuestScriptDef questDef;

		// Token: 0x0400369C RID: 13980
		private Quest quest;

		// Token: 0x0400369D RID: 13981
		private bool giveNext;

		// Token: 0x0400369E RID: 13982
		private const int ReceiveQuestMTBTicks = 12500;

		// Token: 0x0400369F RID: 13983
		private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);
	}
}
