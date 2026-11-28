using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001691 RID: 5777
	public class RitualObligation : IExposable, ILoadReferenceable
	{
		// Token: 0x170018FE RID: 6398
		// (get) Token: 0x060090C9 RID: 37065 RVA: 0x00351BF7 File Offset: 0x0034FDF7
		public bool StillValid
		{
			get
			{
				return !this.expires || this.TicksUntilExpiration >= 0;
			}
		}

		// Token: 0x170018FF RID: 6399
		// (get) Token: 0x060090CA RID: 37066 RVA: 0x00351C0F File Offset: 0x0034FE0F
		public static int DaysToExpire
		{
			get
			{
				return RitualObligation.StageDays[RitualObligation.StageDays.Length - 1];
			}
		}

		// Token: 0x17001900 RID: 6400
		// (get) Token: 0x060090CB RID: 37067 RVA: 0x00351C20 File Offset: 0x0034FE20
		public int ActiveForTicks
		{
			get
			{
				return Find.TickManager.TicksGame - this.triggeredTick;
			}
		}

		// Token: 0x17001901 RID: 6401
		// (get) Token: 0x060090CC RID: 37068 RVA: 0x00351C33 File Offset: 0x0034FE33
		public int TicksUntilExpiration
		{
			get
			{
				return this.triggeredTick + RitualObligation.DaysToExpire * 60000 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x060090CD RID: 37069 RVA: 0x00351C54 File Offset: 0x0034FE54
		public TargetInfo FirstValidTarget
		{
			get
			{
				if (this.targetA.IsValid)
				{
					return this.targetA;
				}
				if (this.targetB.IsValid)
				{
					return this.targetB;
				}
				if (this.targetC.IsValid)
				{
					return this.targetC;
				}
				return TargetInfo.Invalid;
			}
		}

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x060090CE RID: 37070 RVA: 0x00351CA4 File Offset: 0x0034FEA4
		public string LetterLabel
		{
			get
			{
				string text = this.precept.obligationTargetFilter.LabelExtraPart(this);
				string result;
				if (text.NullOrEmpty())
				{
					result = "RitualOpportunity".Translate(this.precept.LabelCap);
				}
				else
				{
					result = "RitualOpportunityFor".Translate(this.precept.LabelCap, text);
				}
				return result;
			}
		}

		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x060090CF RID: 37071 RVA: 0x00351D14 File Offset: 0x0034FF14
		public TaggedString LetterText
		{
			get
			{
				Precept_Ritual precept_Ritual = this.precept;
				this.sbTemp.Clear();
				Pawn arg = this.targetA.Thing as Pawn;
				Corpse corpse = this.targetA.Thing as Corpse;
				if (corpse != null)
				{
					arg = corpse.InnerPawn;
				}
				RitualObligationTrigger_Date ritualObligationTrigger_Date = precept_Ritual.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
				TaggedString taggedString = ((precept_Ritual.ideo.adjective.NullOrEmpty() && !precept_Ritual.ritualExpectedDescNoAdjective.NullOrEmpty()) ? precept_Ritual.ritualExpectedDescNoAdjective : precept_Ritual.ritualExpectedDesc).Formatted(precept_Ritual.ideo.Named("IDEOLIGION"), precept_Ritual.Named("RITUAL"), arg.Named("PAWN"), precept_Ritual.ideo.memberName.Colorize(precept_Ritual.ideo.TextColor).Named("MEMBER"), precept_Ritual.ideo.MemberNamePlural.Colorize(precept_Ritual.ideo.TextColor).Named("MEMBERS"), (precept_Ritual.ideo.classicMode ? Faction.OfPlayer.Name : precept_Ritual.ideo.name).Colorize(precept_Ritual.ideo.TextColor).Named("IDEO"), (ritualObligationTrigger_Date != null) ? ritualObligationTrigger_Date.DateString.Named("DATE") : "");
				this.sbTemp.AppendLineTagged(taggedString);
				this.sbTemp.AppendLine();
				IEnumerable<string> targetInfos = precept_Ritual.obligationTargetFilter.GetTargetInfos(this);
				if (targetInfos.Count<string>() > 1)
				{
					if ("RitualMultiTargetDesc".CanTranslate())
					{
						this.sbTemp.AppendLine("RitualMultiTargetDesc".Translate(precept_Ritual.Named("RITUAL")) + ":\n" + targetInfos.ToLineList("  - ", true));
					}
					else
					{
						this.sbTemp.AppendLine("RitualTargetsExplanation".Translate(precept_Ritual.Named("RITUAL"), targetInfos.ToLineList("  - ", true)).CapitalizeFirst());
					}
				}
				else if ("RitualSingleTargetDesc".CanTranslate())
				{
					this.sbTemp.AppendLine("RitualSingleTargetDesc".Translate(precept_Ritual.Named("RITUAL"), targetInfos.First<string>().UncapitalizeFirst().Named("FOCUS")));
				}
				else
				{
					this.sbTemp.AppendLine("RitualTargetsExplanation".Translate(precept_Ritual.Named("RITUAL"), targetInfos.ToLineList("  - ", true)).CapitalizeFirst());
				}
				if (this.expires)
				{
					Map map = (Current.ProgramState == ProgramState.Playing) ? Find.AnyPlayerHomeMap : null;
					string value = GenDate.QuadrumDateStringAt((long)(Find.TickManager.TicksAbs + RitualObligation.StageDays[RitualObligation.StageDays.Length - 1] * 60000), (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).x : 0f);
					this.sbTemp.AppendLine();
					this.sbTemp.AppendLineTagged("RitualExpiresIn".Translate((RitualObligation.StageDays[RitualObligation.StageDays.Length - 1] * 60000).ToStringTicksToPeriodVague(true, true), value));
				}
				this.unfilledRolesTemp.Clear();
				foreach (RitualRole ritualRole in precept_Ritual.behavior.def.RequiredRoles())
				{
					Precept_Role precept_Role = ritualRole.FindInstance(precept_Ritual.ideo);
					if (!ritualRole.substitutable && precept_Role != null && precept_Role.Active && precept_Role.ChosenPawnSingle() == null)
					{
						this.unfilledRolesTemp.Add(precept_Role.LabelCap);
					}
				}
				if (this.unfilledRolesTemp.Any<string>())
				{
					this.sbTemp.AppendLine();
					this.sbTemp.AppendLine();
					this.sbTemp.AppendLine("RitualUnfilledRoles".Translate(precept_Ritual.LabelCap) + ":");
					this.sbTemp.Append(this.unfilledRolesTemp.ToLineList("  - "));
				}
				return this.sbTemp.ToString().TrimEndNewlines();
			}
		}

		// Token: 0x060090D0 RID: 37072 RVA: 0x00352190 File Offset: 0x00350390
		public RitualObligation()
		{
		}

		// Token: 0x060090D1 RID: 37073 RVA: 0x003521BC File Offset: 0x003503BC
		public RitualObligation(Precept_Ritual precept, TargetInfo targetA, TargetInfo targetB, TargetInfo targetC)
		{
			this.precept = precept;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.triggeredTick = Find.TickManager.TicksGame;
			this.ID = Find.UniqueIDsManager.GetNextRitualObligationID();
		}

		// Token: 0x060090D2 RID: 37074 RVA: 0x00352230 File Offset: 0x00350430
		public RitualObligation(Precept_Ritual precept, TargetInfo targetA, TargetInfo targetB, bool expires = true) : this(precept, targetA, targetB, TargetInfo.Invalid)
		{
			this.expires = expires;
		}

		// Token: 0x060090D3 RID: 37075 RVA: 0x00352248 File Offset: 0x00350448
		public RitualObligation(Precept_Ritual precept, TargetInfo targetA, bool expires = true) : this(precept, targetA, TargetInfo.Invalid, TargetInfo.Invalid)
		{
			this.expires = expires;
		}

		// Token: 0x060090D4 RID: 37076 RVA: 0x00352263 File Offset: 0x00350463
		public RitualObligation(Precept_Ritual precept, bool expires = true) : this(precept, TargetInfo.Invalid, TargetInfo.Invalid, TargetInfo.Invalid)
		{
			this.expires = expires;
		}

		// Token: 0x060090D5 RID: 37077 RVA: 0x00352282 File Offset: 0x00350482
		public void DebugOffsetTriggeredTick(int ticks)
		{
			this.triggeredTick += ticks;
		}

		// Token: 0x060090D6 RID: 37078 RVA: 0x00352294 File Offset: 0x00350494
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.triggeredTick, "triggeredTick", 0, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
			Scribe_Values.Look<bool>(ref this.expires, "expires", false, false);
			Scribe_TargetInfo.Look(ref this.targetA, true, "targetA");
			Scribe_TargetInfo.Look(ref this.targetB, true, "targetB");
			Scribe_TargetInfo.Look(ref this.targetC, true, "targetC");
			Scribe_Collections.Look<Pawn>(ref this.onlyForPawns, "onlyForPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
		}

		// Token: 0x060090D7 RID: 37079 RVA: 0x00352334 File Offset: 0x00350534
		public virtual void CopyTo(RitualObligation other)
		{
			other.ID = this.ID;
			other.precept = this.precept;
			other.triggeredTick = this.triggeredTick;
			other.sendLetter = this.sendLetter;
			other.expires = this.expires;
			other.targetA = this.targetA;
			other.targetB = this.targetB;
			other.targetC = this.targetC;
			other.onlyForPawns = this.onlyForPawns;
		}

		// Token: 0x060090D8 RID: 37080 RVA: 0x003523AD File Offset: 0x003505AD
		public string GetUniqueLoadID()
		{
			return "RitualObligation_" + this.ID.ToString();
		}

		// Token: 0x04005842 RID: 22594
		public Precept_Ritual precept;

		// Token: 0x04005843 RID: 22595
		private int triggeredTick;

		// Token: 0x04005844 RID: 22596
		public bool sendLetter = true;

		// Token: 0x04005845 RID: 22597
		public bool expires = true;

		// Token: 0x04005846 RID: 22598
		public TargetInfo targetA;

		// Token: 0x04005847 RID: 22599
		public TargetInfo targetB;

		// Token: 0x04005848 RID: 22600
		public TargetInfo targetC;

		// Token: 0x04005849 RID: 22601
		public List<Pawn> onlyForPawns;

		// Token: 0x0400584A RID: 22602
		public int ID;

		// Token: 0x0400584B RID: 22603
		public static readonly int[] StageDays = new int[]
		{
			3,
			9
		};

		// Token: 0x0400584C RID: 22604
		private StringBuilder sbTemp = new StringBuilder();

		// Token: 0x0400584D RID: 22605
		private List<string> unfilledRolesTemp = new List<string>();
	}
}
