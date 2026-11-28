using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020018B3 RID: 6323
	[StaticConstructorOnStartup]
	public class CompHackable : ThingComp, IThingGlower
	{
		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x06009E5C RID: 40540 RVA: 0x0038D4BA File Offset: 0x0038B6BA
		public CompProperties_Hackable Props
		{
			get
			{
				return (CompProperties_Hackable)this.props;
			}
		}

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x06009E5D RID: 40541 RVA: 0x0038D4C7 File Offset: 0x0038B6C7
		public float ProgressPercent
		{
			get
			{
				return this.progress / this.defence;
			}
		}

		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x06009E5E RID: 40542 RVA: 0x0038D4D6 File Offset: 0x0038B6D6
		public bool Autohack
		{
			get
			{
				return this.autohack;
			}
		}

		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x06009E5F RID: 40543 RVA: 0x0038D4DE File Offset: 0x0038B6DE
		public bool IsHacked
		{
			get
			{
				return this.hacked;
			}
		}

		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x06009E60 RID: 40544 RVA: 0x0038D4E6 File Offset: 0x0038B6E6
		public bool LockedOut
		{
			get
			{
				return this.lockedOutPermanently || this.lockedOutUntilTick > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06009E61 RID: 40545 RVA: 0x0038D504 File Offset: 0x0038B704
		public bool ShouldBeLitNow()
		{
			return !this.IsHacked || this.Props.glowIfHacked;
		}

		// Token: 0x06009E62 RID: 40546 RVA: 0x0038D51B File Offset: 0x0038B71B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.defence = this.Props.defence;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06009E63 RID: 40547 RVA: 0x0038D545 File Offset: 0x0038B745
		public void HackNow()
		{
			this.progress = this.defence + 1f;
			this.ProcessHacked(null, true);
		}

		// Token: 0x06009E64 RID: 40548 RVA: 0x0038D564 File Offset: 0x0038B764
		public void Hack(float amount, Pawn hacker = null, bool suppressMessages = false)
		{
			bool isHacked = this.IsHacked;
			this.progress += amount;
			this.progress = Mathf.Clamp(this.progress, 0f, this.defence);
			if (!isHacked && this.progress >= this.defence)
			{
				this.ProcessHacked(hacker, suppressMessages);
			}
			if (this.lastHackTick < 0)
			{
				if (!this.hackingStartedSignal.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.hackingStartedSignal, this.parent.Named("SUBJECT")));
				}
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "HackingStarted", this.parent.Named("SUBJECT"));
			}
			this.lastUserSpeed = amount;
			this.lastHackTick = Find.TickManager.TicksGame;
			this.lastUser = hacker;
			if (hacker != null && (this.Props.lockoutDurationHoursRange.TrueMax > 0 || this.Props.lockoutPermanently) && !this.IsHacked && this.progress - this.progressLastLockout > 3000f && Rand.MTBEventOccurs(hacker.GetStatValue(StatDefOf.HackingStealth, true, -1), amount * 60f, 1f))
			{
				this.LockOut(hacker);
			}
		}

		// Token: 0x06009E65 RID: 40549 RVA: 0x0038D6A0 File Offset: 0x0038B8A0
		private void ProcessHacked(Pawn hacker, bool suppressMessages)
		{
			if (this.hacked)
			{
				return;
			}
			this.hacked = true;
			if (!this.hackingCompletedSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(this.hackingCompletedSignal, this.parent.Named("SUBJECT")));
			}
			Pawn arg = hacker ?? ((Current.ProgramState == ProgramState.Playing && this.parent.Map.mapPawns.ColonistCount > 0) ? this.parent.Map.mapPawns.FreeColonists.RandomElement<Pawn>() : null);
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "Hacked", this.parent.Named("SUBJECT"), arg.Named("ACTIVATOR"));
			this.parent.BroadcastCompSignal("Hacked");
			if (this.Props.completedQuest != null)
			{
				Slate slate = new Slate();
				slate.Set<Map>("map", this.parent.Map, false);
				if (this.Props.completedQuest.CanRun(slate, this.parent.Map))
				{
					QuestUtility.GenerateQuestAndMakeAvailable(this.Props.completedQuest, slate);
				}
			}
			this.OnHacked(hacker, suppressMessages);
		}

		// Token: 0x06009E66 RID: 40550 RVA: 0x0038D7D4 File Offset: 0x0038B9D4
		public override IEnumerable<ThingDefCountClass> GetAdditionalLeavings(Map map, DestroyMode mode)
		{
			CompHackable.<GetAdditionalLeavings>d__32 <GetAdditionalLeavings>d__ = new CompHackable.<GetAdditionalLeavings>d__32(-2);
			<GetAdditionalLeavings>d__.<>4__this = this;
			return <GetAdditionalLeavings>d__;
		}

		// Token: 0x06009E67 RID: 40551 RVA: 0x0038D7E4 File Offset: 0x0038B9E4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (!this.sentLetter && !this.Props.destroyedLetterLabel.NullOrEmpty() && !this.Props.destroyedLetterText.NullOrEmpty())
			{
				this.sentLetter = true;
				Find.LetterStack.ReceiveLetter(this.Props.destroyedLetterLabel, this.Props.destroyedLetterText, this.Props.destroyedLetterDef, null, 0, true);
			}
		}

		// Token: 0x06009E68 RID: 40552 RVA: 0x0038D85C File Offset: 0x0038BA5C
		public override bool CompPreventClaimingBy(Faction faction)
		{
			return faction.IsPlayer && !this.IsHacked;
		}

		// Token: 0x06009E69 RID: 40553 RVA: 0x0038D874 File Offset: 0x0038BA74
		protected virtual void OnHacked(Pawn hacker = null, bool suppressMessages = false)
		{
			IHackable hackable = this.parent as IHackable;
			if (hackable != null)
			{
				hackable.OnHacked(hacker);
			}
			foreach (ThingComp thingComp in this.parent.AllComps)
			{
				thingComp.Notify_Hacked(hacker);
			}
			GenLeaving.DropThingsNear(this.parent, this.Props.dropOnHacked, CompHackable.tmpSpawnedThings, true, null);
			if (!this.Props.hackedMessage.NullOrEmpty() && hacker != null && !suppressMessages)
			{
				Messages.Message(this.Props.hackedMessage.Formatted(hacker.Named("HACKER"), this.parent.Named("SUBJECT")), this.parent, MessageTypeDefOf.PositiveEvent, false);
			}
			if (!this.sentLetter && !this.Props.hackedLetterLabel.NullOrEmpty() && !this.Props.hackedLetterText.NullOrEmpty() && !suppressMessages)
			{
				this.sentLetter = true;
				Find.LetterStack.ReceiveLetter(this.Props.hackedLetterLabel, this.Props.hackedLetterText, this.Props.hackedLetterDef, CompHackable.tmpSpawnedThings, null, null, null, null, 0, true);
			}
			CompHackable.tmpSpawnedThings.Clear();
			if (this.parent.Spawned)
			{
				this.parent.DirtyMapMesh(this.parent.Map);
			}
		}

		// Token: 0x06009E6A RID: 40554 RVA: 0x0038DA04 File Offset: 0x0038BC04
		public override void PostDraw()
		{
			if (this.Props.graphicDrawerType == DrawerType.MapMeshOnly)
			{
				return;
			}
			if (this.IsHacked && this.Props.hackedGraphicData != null)
			{
				this.Props.hackedGraphicData.Graphic.Draw(this.parent.DrawPos + this.Props.hackedGraphicOffset, this.parent.Rotation, this.parent, 0f);
				return;
			}
			if (!this.IsHacked && this.Props.unhackedGraphicData != null)
			{
				this.Props.unhackedGraphicData.Graphic.Draw(this.parent.DrawPos + this.Props.unhackedGraphicOffset, this.parent.Rotation, this.parent, 0f);
			}
		}

		// Token: 0x06009E6B RID: 40555 RVA: 0x0038DAD8 File Offset: 0x0038BCD8
		public override void PostPrintOnto(SectionLayer layer)
		{
			if (this.Props.graphicDrawerType == DrawerType.RealtimeOnly)
			{
				return;
			}
			if (this.IsHacked && this.Props.hackedGraphicData != null)
			{
				this.Props.hackedGraphicData.Graphic.Print(layer, this.parent, 0f);
				return;
			}
			if (!this.IsHacked && this.Props.unhackedGraphicData != null)
			{
				this.Props.unhackedGraphicData.Graphic.Print(layer, this.parent, 0f);
			}
		}

		// Token: 0x06009E6C RID: 40556 RVA: 0x0038DB61 File Offset: 0x0038BD61
		public override bool DontDrawParent()
		{
			return (this.IsHacked && this.Props.hackedGraphicData != null) || (!this.IsHacked && this.Props.unhackedGraphicData != null);
		}

		// Token: 0x06009E6D RID: 40557 RVA: 0x0038DB92 File Offset: 0x0038BD92
		public override void CompTick()
		{
			if (this.lockedOutUntilTick >= 0 && this.lockedOutUntilTick <= Find.TickManager.TicksGame && !this.Props.lockoutPermanently)
			{
				this.EndLockout();
			}
		}

		// Token: 0x06009E6E RID: 40558 RVA: 0x0038DBC4 File Offset: 0x0038BDC4
		public void LockOut(Pawn hacker)
		{
			if (this.Props.lockoutPermanently)
			{
				this.lockedOutPermanently = true;
				Messages.Message("MessageHackerLockedOutPermanently".Translate(hacker.Named("HACKER")), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
			else
			{
				int num = this.Props.lockoutDurationHoursRange.RandomInRange * 2500;
				this.progressLastLockout = this.progress;
				this.lockedOutUntilTick = Find.TickManager.TicksGame + num;
				Messages.Message("MessageHackerLockedOut".Translate(hacker.Named("HACKER"), num.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
			this.OnLockedOut(hacker);
		}

		// Token: 0x06009E6F RID: 40559 RVA: 0x0038DC98 File Offset: 0x0038BE98
		protected virtual void OnLockedOut(Pawn hacker = null)
		{
			Pawn arg = hacker ?? ((this.parent.Map.mapPawns.ColonistCount > 0) ? this.parent.Map.mapPawns.FreeColonists.RandomElement<Pawn>() : null);
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "LockedOut", this.parent.Named("SUBJECT"), arg.Named("ACTIVATOR"));
			IHackable hackable = this.parent as IHackable;
			if (hackable != null)
			{
				hackable.OnLockedOut(hacker);
			}
		}

		// Token: 0x06009E70 RID: 40560 RVA: 0x0038DD26 File Offset: 0x0038BF26
		private void EndLockout()
		{
			this.lockedOutUntilTick = -1;
			this.lockedOutPermanently = false;
			Messages.Message("MessageHackableUnlocked".Translate(this.parent), this.parent, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x06009E71 RID: 40561 RVA: 0x0038DD68 File Offset: 0x0038BF68
		private AcceptanceReport CanHackNow(bool reportAlreadyHacked)
		{
			if (this.IsHacked && reportAlreadyHacked)
			{
				return "AlreadyHacked".Translate();
			}
			if (this.IsHacked)
			{
				return false;
			}
			if (this.LockedOut)
			{
				return "LockedOut".Translate();
			}
			return true;
		}

		// Token: 0x06009E72 RID: 40562 RVA: 0x0038DDBC File Offset: 0x0038BFBC
		public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			CompHackable.<CompMultiSelectFloatMenuOptions>d__46 <CompMultiSelectFloatMenuOptions>d__ = new CompHackable.<CompMultiSelectFloatMenuOptions>d__46(-2);
			<CompMultiSelectFloatMenuOptions>d__.<>4__this = this;
			<CompMultiSelectFloatMenuOptions>d__.<>3__selPawns = selPawns;
			return <CompMultiSelectFloatMenuOptions>d__;
		}

		// Token: 0x06009E73 RID: 40563 RVA: 0x0038DDD4 File Offset: 0x0038BFD4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
			Scribe_Values.Look<float>(ref this.lastUserSpeed, "lastUserSpeed", 0f, false);
			Scribe_Values.Look<int>(ref this.lastHackTick, "lastHackTick", 0, false);
			Scribe_Values.Look<float>(ref this.defence, "defence", 0f, false);
			Scribe_References.Look<Pawn>(ref this.lastUser, "lasterUser", false);
			Scribe_Values.Look<string>(ref this.hackingStartedSignal, "hackingStartedSignal", null, false);
			Scribe_Values.Look<string>(ref this.hackingCompletedSignal, "hackingCompletedSignal", null, false);
			Scribe_Values.Look<int>(ref this.lockedOutUntilTick, "lockedOutUntilTick", -1, false);
			Scribe_Values.Look<float>(ref this.progressLastLockout, "progressLastLockout", 0f, false);
			Scribe_Values.Look<bool>(ref this.lockedOutPermanently, "lockedOutPermanently", false, false);
			Scribe_Values.Look<bool>(ref this.sentLetter, "sentLetter", false, false);
			Scribe_Values.Look<bool>(ref this.autohack, "autohack", false, false);
			Scribe_Values.Look<bool>(ref this.hacked, "hacked", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.parent.Spawned)
			{
				this.hacked = (this.hacked || this.progress >= this.defence);
			}
		}

		// Token: 0x06009E74 RID: 40564 RVA: 0x0038DF18 File Offset: 0x0038C118
		public override string CompInspectStringExtra()
		{
			TaggedString t = TaggedString.Empty;
			bool isHacked = this.IsHacked;
			if (!isHacked && !this.Props.notHackedInspectString.NullOrEmpty())
			{
				t += this.Props.notHackedInspectString;
			}
			if ((!isHacked || this.Props.showProgressAfterHackCompletion) && !this.Props.onlyRemotelyHackable)
			{
				if (t.Length > 0)
				{
					t += "\n";
				}
				t += "HackProgress".Translate() + ": " + this.progress.ToStringWorkAmount() + " / " + this.defence.ToStringWorkAmount();
				if (isHacked)
				{
					t += " (" + "Hacked".Translate() + ")";
				}
			}
			if (isHacked)
			{
				if (!this.Props.hackedInspectString.NullOrEmpty())
				{
					if (t.Length > 0)
					{
						t += "\n";
					}
					t += this.Props.hackedInspectString;
				}
			}
			else
			{
				if (this.LockedOut)
				{
					if (t.Length > 0)
					{
						t += "\n";
					}
					t += ("LockedOut".Translate() + ": " + (this.lockedOutUntilTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).Colorize(ColorLibrary.RedReadable);
				}
				if (this.lastHackTick > Find.TickManager.TicksGame - 30)
				{
					string str;
					if (this.lastUser != null)
					{
						str = "HackingLastUser".Translate(this.lastUser) + " " + StatDefOf.HackingSpeed.label;
					}
					else
					{
						str = StatDefOf.HackingSpeed.LabelCap;
					}
					if (t.Length > 0)
					{
						t += "\n";
					}
					t += str + ": " + StatDefOf.HackingSpeed.ValueToString(this.lastUserSpeed, ToStringNumberSense.Absolute, true);
				}
			}
			if (!this.IsHacked && this.Props.intellectualSkillPrerequisite > 0)
			{
				if (t.Length > 0)
				{
					t += "\n";
				}
				t += "IntellectualSkillPrerequisite".Translate() + ": " + this.Props.intellectualSkillPrerequisite.ToString();
			}
			return t.Resolve();
		}

		// Token: 0x06009E75 RID: 40565 RVA: 0x0038E1AF File Offset: 0x0038C3AF
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompHackable.<SpecialDisplayStats>d__49 <SpecialDisplayStats>d__ = new CompHackable.<SpecialDisplayStats>d__49(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06009E76 RID: 40566 RVA: 0x0038E1C0 File Offset: 0x0038C3C0
		private AcceptanceReport ValidateHacker(LocalTargetInfo target)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!pawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if (!HackUtility.IsCapableOfHacking(pawn))
			{
				return "IncapableOfHacking".Translate();
			}
			if (this.Props.intellectualSkillPrerequisite > 0)
			{
				Pawn_SkillTracker skills = pawn.skills;
				int? num = (skills != null) ? new int?(skills.GetSkill(SkillDefOf.Intellectual).Level) : null;
				int intellectualSkillPrerequisite = this.Props.intellectualSkillPrerequisite;
				if (num.GetValueOrDefault() < intellectualSkillPrerequisite & num != null)
				{
					return "SkillTooLow".Translate(SkillDefOf.Intellectual.label, pawn.skills.GetSkill(SkillDefOf.Intellectual).Level, this.Props.intellectualSkillPrerequisite);
				}
			}
			return true;
		}

		// Token: 0x06009E77 RID: 40567 RVA: 0x0038E2CE File Offset: 0x0038C4CE
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompHackable.<CompGetGizmosExtra>d__51 <CompGetGizmosExtra>d__ = new CompHackable.<CompGetGizmosExtra>d__51(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009E78 RID: 40568 RVA: 0x0038E2E0 File Offset: 0x0038C4E0
		public AcceptanceReport CanHackNow(Pawn pawn)
		{
			if (this.Props.onlyRemotelyHackable || this.IsHacked)
			{
				return false;
			}
			if (this.LockedOut)
			{
				return "LockedOut".Translate();
			}
			if (!HackUtility.IsCapableOfHacking(pawn))
			{
				return "IncapableOfHacking".Translate();
			}
			if (!pawn.CanReach(this.parent, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate().CapitalizeFirst();
			}
			if (this.Props.intellectualSkillPrerequisite > 0 && pawn.skills.GetSkill(SkillDefOf.Intellectual).Level < this.Props.intellectualSkillPrerequisite)
			{
				return "SkillTooLow".Translate(SkillDefOf.Intellectual.label, pawn.skills.GetSkill(SkillDefOf.Intellectual).Level, this.Props.intellectualSkillPrerequisite);
			}
			return true;
		}

		// Token: 0x04005DC9 RID: 24009
		public string hackingStartedSignal;

		// Token: 0x04005DCA RID: 24010
		public string hackingCompletedSignal;

		// Token: 0x04005DCB RID: 24011
		private float progress;

		// Token: 0x04005DCC RID: 24012
		public float defence;

		// Token: 0x04005DCD RID: 24013
		private float lastUserSpeed = 1f;

		// Token: 0x04005DCE RID: 24014
		private int lastHackTick = -1;

		// Token: 0x04005DCF RID: 24015
		private Pawn lastUser;

		// Token: 0x04005DD0 RID: 24016
		private float progressLastLockout;

		// Token: 0x04005DD1 RID: 24017
		private int lockedOutUntilTick = -1;

		// Token: 0x04005DD2 RID: 24018
		private bool lockedOutPermanently;

		// Token: 0x04005DD3 RID: 24019
		private bool sentLetter;

		// Token: 0x04005DD4 RID: 24020
		private bool autohack;

		// Token: 0x04005DD5 RID: 24021
		private bool hacked;

		// Token: 0x04005DD6 RID: 24022
		public const string HackedSignal = "Hacked";

		// Token: 0x04005DD7 RID: 24023
		private const float MinHackProgressBeforeLockout = 3000f;

		// Token: 0x04005DD8 RID: 24024
		private static readonly Texture2D HackTexture = ContentFinder<Texture2D>.Get("UI/Commands/Hack", true);

		// Token: 0x04005DD9 RID: 24025
		private static readonly Texture2D AutohackTexture = ContentFinder<Texture2D>.Get("UI/Commands/Autohack", true);

		// Token: 0x04005DDA RID: 24026
		private static readonly List<Thing> tmpSpawnedThings = new List<Thing>();

		// Token: 0x04005DDB RID: 24027
		private static readonly List<Pawn> tmpAllowedPawns = new List<Pawn>();
	}
}
