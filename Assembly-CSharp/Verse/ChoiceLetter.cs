using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000619 RID: 1561
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060030CD RID: 12493 RVA: 0x00138996 File Offset: 0x00136B96
		// (set) Token: 0x060030CE RID: 12494 RVA: 0x0013899E File Offset: 0x00136B9E
		public TaggedString Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value.CapitalizeFirst();
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060030CF RID: 12495
		public abstract IEnumerable<DiaOption> Choices { get; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060030D0 RID: 12496 RVA: 0x001389AD File Offset: 0x00136BAD
		protected DiaOption Option_Close
		{
			get
			{
				return new DiaOption("Close".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060030D1 RID: 12497 RVA: 0x001389DC File Offset: 0x00136BDC
		protected DiaOption Option_JumpToLocation
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target, CameraJumper.MovementMode.Pan);
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060030D2 RID: 12498 RVA: 0x00138A4C File Offset: 0x00136C4C
		protected DiaOption Option_JumpToLocationAndPostpone
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target, CameraJumper.MovementMode.Pan);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060030D3 RID: 12499 RVA: 0x00138AB3 File Offset: 0x00136CB3
		protected DiaOption Option_Reject
		{
			get
			{
				return new DiaOption("RejectLetter".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060030D4 RID: 12500 RVA: 0x00138AE4 File Offset: 0x00136CE4
		protected DiaOption Option_Postpone
		{
			get
			{
				DiaOption diaOption = new DiaOption("PostponeLetter".Translate());
				diaOption.resolveTree = true;
				if (this.LastTickBeforeTimeout)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x00138B20 File Offset: 0x00136D20
		protected DiaOption Option_ViewInQuestsTab(string labelKey = "ViewRelatedQuest", bool postpone = false)
		{
			TaggedString taggedString = labelKey.Translate();
			if (this.title != this.quest.name && !this.quest.hidden)
			{
				taggedString += ": " + this.quest.name;
			}
			DiaOption diaOption = new DiaOption(taggedString);
			diaOption.action = delegate()
			{
				if (this.quest != null)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					if (!postpone)
					{
						Find.LetterStack.RemoveLetter(this);
					}
				}
			};
			diaOption.resolveTree = true;
			if (this.quest == null || this.quest.hidden)
			{
				diaOption.Disable(null);
			}
			return diaOption;
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x00138BCC File Offset: 0x00136DCC
		protected DiaOption Option_ViewInfoCard(int index)
		{
			int num = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
			if (index >= num)
			{
				return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkHediffDefs[index - num], -1));
			}
			return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkThingDefs[index], -1));
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x00138C28 File Offset: 0x00136E28
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<TaggedString>(ref this.text, "text", default(TaggedString), false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Collections.Look<ThingDef>(ref this.hyperlinkThingDefs, "hyperlinkThingDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<HediffDef>(ref this.hyperlinkHediffDefs, "hyperlinkHediffDefs", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x00138CB6 File Offset: 0x00136EB6
		protected override string GetMouseoverText()
		{
			return this.text.Resolve();
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x00138CC4 File Offset: 0x00136EC4
		public override void OpenLetter()
		{
			DiaNode diaNode = new DiaNode(this.text);
			diaNode.options.AddRange(this.Choices);
			Dialog_NodeTreeWithFactionInfo window = new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title);
			Find.WindowStack.Add(window);
		}

		// Token: 0x04001F6C RID: 8044
		public string title;

		// Token: 0x04001F6D RID: 8045
		private TaggedString text;

		// Token: 0x04001F6E RID: 8046
		public bool radioMode;

		// Token: 0x04001F6F RID: 8047
		public Quest quest;

		// Token: 0x04001F70 RID: 8048
		public List<ThingDef> hyperlinkThingDefs;

		// Token: 0x04001F71 RID: 8049
		public List<HediffDef> hyperlinkHediffDefs;
	}
}
