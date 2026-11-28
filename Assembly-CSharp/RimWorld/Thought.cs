using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D1 RID: 5585
	[StaticConstructorOnStartup]
	public abstract class Thought : IExposable
	{
		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x06008ABF RID: 35519
		public abstract int CurStageIndex { get; }

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x06008AC0 RID: 35520 RVA: 0x00325751 File Offset: 0x00323951
		public virtual int DurationTicks
		{
			get
			{
				return this.def.DurationTicks;
			}
		}

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x06008AC1 RID: 35521 RVA: 0x0032575E File Offset: 0x0032395E
		public ThoughtStage CurStage
		{
			get
			{
				return this.def.stages[this.CurStageIndex];
			}
		}

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x06008AC2 RID: 35522 RVA: 0x00325776 File Offset: 0x00323976
		public virtual bool VisibleInNeedsTab
		{
			get
			{
				return this.CurStage.visible;
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x06008AC3 RID: 35523 RVA: 0x00325784 File Offset: 0x00323984
		public virtual string LabelCap
		{
			get
			{
				if (this.def.Worker == null)
				{
					return this.CurStage.LabelCap.Formatted(this.pawn.Named("PAWN")).CapitalizeFirst();
				}
				return this.def.Worker.PostProcessLabel(this.pawn, this.CurStage.LabelCap).CapitalizeFirst();
			}
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x06008AC4 RID: 35524 RVA: 0x003257F2 File Offset: 0x003239F2
		protected virtual float BaseMoodOffset
		{
			get
			{
				return this.CurStage.baseMoodEffect;
			}
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x06008AC5 RID: 35525 RVA: 0x003257FF File Offset: 0x003239FF
		public virtual string LabelCapSocial
		{
			get
			{
				if (this.CurStage.labelSocial != null)
				{
					return this.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"));
				}
				return this.LabelCap;
			}
		}

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x06008AC6 RID: 35526 RVA: 0x0032583C File Offset: 0x00323A3C
		public virtual string Description
		{
			get
			{
				string text = this.CurStage.description;
				if (text == null)
				{
					text = this.def.description;
				}
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessDescription(this.pawn, text);
				}
				else
				{
					Thought_Memory thought_Memory = this as Thought_Memory;
					if (thought_Memory != null && thought_Memory.otherPawn != null)
					{
						text = text.Formatted(this.pawn.Named("PAWN"), thought_Memory.otherPawn.Named("OTHERPAWN")).Resolve();
					}
					else
					{
						ISocialThought socialThought = this as ISocialThought;
						if (socialThought != null && socialThought.OtherPawn() != null)
						{
							text = text.Formatted(this.pawn.Named("PAWN"), socialThought.OtherPawn().Named("OTHERPAWN")).Resolve();
						}
						else
						{
							text = text.Formatted(this.pawn.Named("PAWN")).Resolve();
						}
					}
				}
				if (ModsConfig.IdeologyActive && this.sourcePrecept != null && !Find.IdeoManager.classicMode)
				{
					text += this.CausedByBeliefInPrecept;
				}
				string text2 = ThoughtUtility.ThoughtNullifiedMessage(this.pawn, this.def);
				if (!string.IsNullOrEmpty(text2))
				{
					text = text + "\n\n(" + text2 + ")";
				}
				return text;
			}
		}

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x06008AC7 RID: 35527 RVA: 0x0032598C File Offset: 0x00323B8C
		public virtual string BabyTalk
		{
			get
			{
				string result;
				if ((result = this.cachedBabyText) == null)
				{
					result = (this.cachedBabyText = ThoughtUtility.GenerateBabyTalk(this.Description.Split('\n', StringSplitOptions.None)[0]));
				}
				return result;
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x06008AC8 RID: 35528 RVA: 0x003259C4 File Offset: 0x00323BC4
		public string CausedByBeliefInPrecept
		{
			get
			{
				return "\n\n" + "CausedBy".Translate() + ": " + "BeliefInIdeo".Translate() + " " + this.sourcePrecept.ideo.name.Colorize(this.sourcePrecept.ideo.TextColor) + (this.sourcePrecept.def.label.NullOrEmpty() ? string.Empty : string.Concat(new string[]
				{
					" (",
					this.sourcePrecept.def.issue.LabelCap.ToString(),
					": ",
					this.sourcePrecept.def.LabelCap.ToString(),
					")"
				}));
			}
		}

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x06008AC9 RID: 35529 RVA: 0x00325AC4 File Offset: 0x00323CC4
		public Texture2D Icon
		{
			get
			{
				if (this.def.Icon != null)
				{
					return this.def.Icon;
				}
				if (this.MoodOffset() > 0f)
				{
					return Thought.DefaultGoodIcon;
				}
				return Thought.DefaultBadIcon;
			}
		}

		// Token: 0x06008ACA RID: 35530 RVA: 0x00325B00 File Offset: 0x00323D00
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Precept precept = this.sourcePrecept;
				if (((precept != null) ? precept.ideo : null) != null && !Find.IdeoManager.IdeosListForReading.Contains(this.sourcePrecept.ideo))
				{
					this.sourcePrecept = null;
				}
			}
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Precept>(ref this.sourcePrecept, "sourcePrecept", false);
		}

		// Token: 0x06008ACB RID: 35531 RVA: 0x00325B70 File Offset: 0x00323D70
		public virtual float MoodOffset()
		{
			if (this.CurStage == null)
			{
				string str = "CurStage is null while ShouldDiscard is false on ";
				string defName = this.def.defName;
				string str2 = " for ";
				Pawn pawn = this.pawn;
				Log.Error(str + defName + str2 + ((pawn != null) ? pawn.ToString() : null));
				return 0f;
			}
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = this.BaseMoodOffset;
			if (this.def.effectMultiplyingStat != null)
			{
				if (this.def.effectMultiplyingStatCurve != null)
				{
					num *= this.def.effectMultiplyingStatCurve.Evaluate(this.pawn.GetStatValue(this.def.effectMultiplyingStat, true, -1));
				}
				else
				{
					num *= this.pawn.GetStatValue(this.def.effectMultiplyingStat, true, -1);
				}
			}
			if (this.def.Worker != null)
			{
				num *= this.def.Worker.MoodMultiplier(this.pawn);
			}
			return num;
		}

		// Token: 0x06008ACC RID: 35532 RVA: 0x00325C65 File Offset: 0x00323E65
		public virtual bool GroupsWith(Thought other)
		{
			return this.def == other.def && (this.CurStageIndex == other.CurStageIndex || this.def.stagesStack);
		}

		// Token: 0x06008ACD RID: 35533 RVA: 0x00325C92 File Offset: 0x00323E92
		public virtual void Init()
		{
		}

		// Token: 0x06008ACE RID: 35534 RVA: 0x00325C94 File Offset: 0x00323E94
		public override string ToString()
		{
			return "(" + this.def.defName + ")";
		}

		// Token: 0x04005570 RID: 21872
		public Pawn pawn;

		// Token: 0x04005571 RID: 21873
		public ThoughtDef def;

		// Token: 0x04005572 RID: 21874
		public Precept sourcePrecept;

		// Token: 0x04005573 RID: 21875
		[Unsaved(false)]
		public float cachedMoodOffsetOfGroup;

		// Token: 0x04005574 RID: 21876
		[Unsaved(false)]
		private string cachedBabyText;

		// Token: 0x04005575 RID: 21877
		private static readonly Texture2D DefaultGoodIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericGood", true);

		// Token: 0x04005576 RID: 21878
		private static readonly Texture2D DefaultBadIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericBad", true);
	}
}
