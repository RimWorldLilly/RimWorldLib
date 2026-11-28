using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000850 RID: 2128
	public class Job : IExposable, ILoadReferenceable
	{
		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x060042D5 RID: 17109 RVA: 0x001A54B7 File Offset: 0x001A36B7
		public RecipeDef RecipeDef
		{
			get
			{
				Bill bill = this.bill;
				if (bill == null)
				{
					return null;
				}
				return bill.recipe;
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x060042D6 RID: 17110 RVA: 0x001A54CA File Offset: 0x001A36CA
		public JobDriver GetCachedDriverDirect
		{
			get
			{
				return this.cachedDriver;
			}
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x001A54D4 File Offset: 0x001A36D4
		public void Clear()
		{
			this.def = null;
			this.targetA = LocalTargetInfo.Invalid;
			this.targetB = LocalTargetInfo.Invalid;
			this.targetC = LocalTargetInfo.Invalid;
			this.targetQueueA = null;
			this.targetQueueB = null;
			this.count = -1;
			this.countQueue = null;
			this.loadID = -1;
			this.startTick = -1;
			this.expiryInterval = -1;
			this.checkOverrideOnExpire = false;
			this.playerForced = false;
			this.playerInterruptedForced = false;
			this.placedThings = null;
			this.maxNumMeleeAttacks = int.MaxValue;
			this.maxNumStaticAttacks = int.MaxValue;
			this.locomotionUrgency = LocomotionUrgency.Jog;
			this.haulMode = HaulMode.Undefined;
			this.bill = null;
			this.commTarget = null;
			this.plantDefToSow = null;
			this.verbToUse = null;
			this.haulOpportunisticDuplicates = false;
			this.exitMapOnArrival = false;
			this.failIfCantJoinOrCreateCaravan = false;
			this.killIncappedTarget = false;
			this.ignoreForbidden = false;
			this.ignoreDesignations = false;
			this.canBashDoors = false;
			this.canBashFences = false;
			this.canUseRangedWeapon = true;
			this.haulDroppedApparel = false;
			this.restUntilHealed = false;
			this.ignoreJoyTimeAssignment = false;
			this.doUntilGatheringEnded = false;
			this.overeat = false;
			this.ingestTotalCount = false;
			this.attackDoorIfTargetLost = false;
			this.takeExtraIngestibles = 0;
			this.expireRequiresEnemiesNearby = false;
			this.lord = null;
			this.collideWithPawns = false;
			this.forceSleep = false;
			this.interaction = null;
			this.endIfCantShootTargetFromCurPos = false;
			this.endIfCantShootInMelee = false;
			this.checkEncumbrance = false;
			this.followRadius = 0f;
			this.endAfterTendedOnce = false;
			this.quest = null;
			this.mote = null;
			this.reactingToMeleeThreat = false;
			this.wasOnMeditationTimeAssignment = false;
			this.psyfocusTargetLast = -1f;
			this.preventFriendlyFire = false;
			this.ropingPriority = RopingPriority.Closest;
			this.ropeToUnenclosedPens = false;
			this.thingDefToCarry = null;
			this.dutyTag = null;
			this.ritualTag = null;
			this.controlGroupTag = null;
			this.lookDirection = Direction8Way.Invalid;
			this.overrideFacing = Rot4.Invalid;
			this.forceMaintainFacing = false;
			this.takeInventoryDelay = 0;
			this.draftedTend = false;
			this.showSpeechBubbles = true;
			this.speechSoundFemale = null;
			this.speechSoundMale = null;
			this.speechFaceSpectatorsIfPossible = false;
			this.biosculpterCycleKey = null;
			this.startInvoluntarySleep = false;
			this.reportStringOverride = null;
			this.crawlingReportStringOverride = null;
			this.isLearningDesire = false;
			this.flying = false;
			this.swimming = false;
			this.intervalScalingTarget = TargetIndex.None;
			this.ensureReachable = false;
			this.jobGiverThinkTree = null;
			this.jobGiver = null;
			this.workGiverDef = null;
			this.ability = null;
			this.source = null;
			if (this.cachedDriver != null)
			{
				this.cachedDriver.job = null;
			}
			this.cachedDriver = null;
			if (this.lastJobDriverMade != null)
			{
				this.lastJobDriverMade.job = null;
			}
			this.lastJobDriverMade = null;
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x001A578C File Offset: 0x001A398C
		public Job()
		{
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x001A5844 File Offset: 0x001A3A44
		public Job(JobDef def) : this(def, null)
		{
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x001A5853 File Offset: 0x001A3A53
		public Job(JobDef def, LocalTargetInfo targetA) : this(def, targetA, null)
		{
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x001A5864 File Offset: 0x001A3A64
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x001A5944 File Offset: 0x001A3B44
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x001A5A2C File Offset: 0x001A3C2C
		public Job(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.targetA = targetA;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x001A5B14 File Offset: 0x001A3D14
		public Job(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x001A5BF4 File Offset: 0x001A3DF4
		public Job Clone()
		{
			return new Job
			{
				def = this.def,
				targetA = this.targetA,
				targetB = this.targetB,
				targetC = this.targetC,
				targetQueueA = this.targetQueueA,
				targetQueueB = this.targetQueueB,
				globalTarget = this.globalTarget,
				count = this.count,
				countQueue = this.countQueue,
				loadID = this.loadID,
				expiryInterval = this.expiryInterval,
				checkOverrideOnExpire = this.checkOverrideOnExpire,
				playerForced = this.playerForced,
				showCarryingInspectLine = this.showCarryingInspectLine,
				placedThings = this.placedThings,
				maxNumMeleeAttacks = this.maxNumMeleeAttacks,
				maxNumStaticAttacks = this.maxNumStaticAttacks,
				locomotionUrgency = this.locomotionUrgency,
				haulMode = this.haulMode,
				bill = this.bill,
				commTarget = this.commTarget,
				plantDefToSow = this.plantDefToSow,
				thingDefToCarry = this.thingDefToCarry,
				verbToUse = this.verbToUse,
				haulOpportunisticDuplicates = this.haulOpportunisticDuplicates,
				exitMapOnArrival = this.exitMapOnArrival,
				failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan,
				killIncappedTarget = this.killIncappedTarget,
				ignoreForbidden = this.ignoreForbidden,
				ignoreDesignations = this.ignoreDesignations,
				canBashDoors = this.canBashDoors,
				canBashFences = this.canBashFences,
				canUseRangedWeapon = this.canUseRangedWeapon,
				haulDroppedApparel = this.haulDroppedApparel,
				restUntilHealed = this.restUntilHealed,
				ignoreJoyTimeAssignment = this.ignoreJoyTimeAssignment,
				doUntilGatheringEnded = this.doUntilGatheringEnded,
				overeat = this.overeat,
				ingestTotalCount = this.ingestTotalCount,
				attackDoorIfTargetLost = this.attackDoorIfTargetLost,
				takeExtraIngestibles = this.takeExtraIngestibles,
				expireRequiresEnemiesNearby = this.expireRequiresEnemiesNearby,
				expireOnEnemiesNearby = this.expireOnEnemiesNearby,
				intervalScalingTarget = this.intervalScalingTarget,
				lord = this.lord,
				collideWithPawns = this.collideWithPawns,
				forceSleep = this.forceSleep,
				interaction = this.interaction,
				endIfCantShootTargetFromCurPos = this.endIfCantShootTargetFromCurPos,
				endIfCantShootInMelee = this.endIfCantShootInMelee,
				checkEncumbrance = this.checkEncumbrance,
				followRadius = this.followRadius,
				endAfterTendedOnce = this.endAfterTendedOnce,
				quest = this.quest,
				mote = this.mote,
				psyfocusTargetLast = this.psyfocusTargetLast,
				wasOnMeditationTimeAssignment = this.wasOnMeditationTimeAssignment,
				reactingToMeleeThreat = this.reactingToMeleeThreat,
				preventFriendlyFire = this.preventFriendlyFire,
				ropingPriority = this.ropingPriority,
				ropeToUnenclosedPens = this.ropeToUnenclosedPens,
				showSpeechBubbles = this.showSpeechBubbles,
				lookDirection = this.lookDirection,
				overrideFacing = this.overrideFacing,
				forceMaintainFacing = this.forceMaintainFacing,
				dutyTag = this.dutyTag,
				ritualTag = this.ritualTag,
				controlGroupTag = this.controlGroupTag,
				takeInventoryDelay = this.takeInventoryDelay,
				draftedTend = this.draftedTend,
				speechFaceSpectatorsIfPossible = this.speechFaceSpectatorsIfPossible,
				speechSoundMale = this.speechSoundMale,
				speechSoundFemale = this.speechSoundFemale,
				biosculpterCycleKey = this.biosculpterCycleKey,
				reportStringOverride = this.reportStringOverride,
				crawlingReportStringOverride = this.crawlingReportStringOverride,
				startInvoluntarySleep = this.startInvoluntarySleep,
				isLearningDesire = this.isLearningDesire,
				flying = this.flying,
				swimming = this.swimming,
				ensureReachable = this.ensureReachable,
				jobGiverThinkTree = this.jobGiverThinkTree,
				jobGiver = this.jobGiver,
				workGiverDef = this.workGiverDef,
				ability = this.ability,
				source = this.source
			};
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x001A600E File Offset: 0x001A420E
		public LocalTargetInfo GetTarget(TargetIndex ind)
		{
			switch (ind)
			{
			case TargetIndex.A:
				return this.targetA;
			case TargetIndex.B:
				return this.targetB;
			case TargetIndex.C:
				return this.targetC;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x001A6040 File Offset: 0x001A4240
		public List<LocalTargetInfo> GetTargetQueue(TargetIndex ind)
		{
			if (ind == TargetIndex.A)
			{
				if (this.targetQueueA == null)
				{
					this.targetQueueA = new List<LocalTargetInfo>();
				}
				return this.targetQueueA;
			}
			if (ind != TargetIndex.B)
			{
				throw new ArgumentException();
			}
			if (this.targetQueueB == null)
			{
				this.targetQueueB = new List<LocalTargetInfo>();
			}
			return this.targetQueueB;
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x001A6090 File Offset: 0x001A4290
		public void SetTarget(TargetIndex ind, LocalTargetInfo pack)
		{
			switch (ind)
			{
			case TargetIndex.A:
				this.targetA = pack;
				return;
			case TargetIndex.B:
				this.targetB = pack;
				return;
			case TargetIndex.C:
				this.targetC = pack;
				return;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x001A60C5 File Offset: 0x001A42C5
		public void AddQueuedTarget(TargetIndex ind, LocalTargetInfo target)
		{
			this.GetTargetQueue(ind).Add(target);
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x001A60D4 File Offset: 0x001A42D4
		public void ExposeData()
		{
			ILoadReferenceable loadReferenceable = (ILoadReferenceable)this.commTarget;
			Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, "commTarget", false);
			this.commTarget = (ICommunicable)loadReferenceable;
			Scribe_References.Look<Verb>(ref this.verbToUse, "verbToUse", false);
			Scribe_References.Look<Bill>(ref this.bill, "bill", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Defs.Look<JobDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_TargetInfo.Look(ref this.targetA, "targetA");
			Scribe_TargetInfo.Look(ref this.targetB, "targetB");
			Scribe_TargetInfo.Look(ref this.targetC, "targetC");
			Scribe_TargetInfo.Look(ref this.globalTarget, "globalTarget");
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueA, "targetQueueA", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueB, "targetQueueB", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.count, "count", -1, false);
			Scribe_Collections.Look<int>(ref this.countQueue, "countQueue", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.ignoreForbidden, "ignoreForbidden", false, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", -1, false);
			Scribe_Values.Look<int>(ref this.expiryInterval, "expiryInterval", -1, false);
			Scribe_Values.Look<bool>(ref this.checkOverrideOnExpire, "checkOverrideOnExpire", false, false);
			Scribe_Values.Look<bool>(ref this.playerForced, "playerForced", false, false);
			Scribe_Values.Look<bool>(ref this.playerInterruptedForced, "playerInterruptedForced", false, false);
			Scribe_Values.Look<TargetIndex>(ref this.intervalScalingTarget, "intervalScalingTarget", TargetIndex.None, false);
			Scribe_Collections.Look<ThingCountClass>(ref this.placedThings, "placedThings", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.maxNumMeleeAttacks, "maxNumMeleeAttacks", int.MaxValue, false);
			Scribe_Values.Look<int>(ref this.maxNumStaticAttacks, "maxNumStaticAttacks", int.MaxValue, false);
			Scribe_Values.Look<bool>(ref this.exitMapOnArrival, "exitMapOnArrival", false, false);
			Scribe_Values.Look<bool>(ref this.failIfCantJoinOrCreateCaravan, "failIfCantJoinOrCreateCaravan", false, false);
			Scribe_Values.Look<bool>(ref this.killIncappedTarget, "killIncappedTarget", false, false);
			Scribe_Values.Look<bool>(ref this.haulOpportunisticDuplicates, "haulOpportunisticDuplicates", false, false);
			Scribe_Values.Look<HaulMode>(ref this.haulMode, "haulMode", HaulMode.Undefined, false);
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToSow, "plantDefToSow");
			Scribe_Defs.Look<ThingDef>(ref this.thingDefToCarry, "thingDefToCarry");
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotionUrgency, "locomotionUrgency", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.ignoreDesignations, "ignoreDesignations", false, false);
			Scribe_Values.Look<bool>(ref this.canBashDoors, "canBash", false, false);
			Scribe_Values.Look<bool>(ref this.canBashFences, "canBashFences", false, false);
			Scribe_Values.Look<bool>(ref this.canUseRangedWeapon, "canUseRangedWeapon", true, false);
			Scribe_Values.Look<bool>(ref this.haulDroppedApparel, "haulDroppedApparel", false, false);
			Scribe_Values.Look<bool>(ref this.restUntilHealed, "restUntilHealed", false, false);
			Scribe_Values.Look<bool>(ref this.ignoreJoyTimeAssignment, "ignoreJoyTimeAssignment", false, false);
			Scribe_Values.Look<bool>(ref this.overeat, "overeat", false, false);
			Scribe_Values.Look<bool>(ref this.ingestTotalCount, "ingestTotalCount", false, false);
			Scribe_Values.Look<bool>(ref this.attackDoorIfTargetLost, "attackDoorIfTargetLost", false, false);
			Scribe_Values.Look<int>(ref this.takeExtraIngestibles, "takeExtraIngestibles", 0, false);
			Scribe_Values.Look<bool>(ref this.expireRequiresEnemiesNearby, "expireRequiresEnemiesNearby", false, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_Values.Look<bool>(ref this.forceSleep, "forceSleep", false, false);
			Scribe_Defs.Look<InteractionDef>(ref this.interaction, "interaction");
			Scribe_Values.Look<bool>(ref this.endIfCantShootTargetFromCurPos, "endIfCantShootTargetFromCurPos", false, false);
			Scribe_Values.Look<bool>(ref this.endIfCantShootInMelee, "endIfCantShootInMelee", false, false);
			Scribe_Values.Look<bool>(ref this.checkEncumbrance, "checkEncumbrance", false, false);
			Scribe_Values.Look<float>(ref this.followRadius, "followRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.endAfterTendedOnce, "endAfterTendedOnce", false, false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.workGiverDef, "workGiverDef");
			Scribe_Defs.Look<ThinkTreeDef>(ref this.jobGiverThinkTree, "jobGiverThinkTree");
			Scribe_Values.Look<bool>(ref this.doUntilGatheringEnded, "doUntilGatheringEnded", false, false);
			Scribe_Values.Look<float>(ref this.psyfocusTargetLast, "psyfocusTargetLast", 0f, false);
			Scribe_Values.Look<bool>(ref this.wasOnMeditationTimeAssignment, "wasOnMeditationTimeAssignment", false, false);
			Scribe_Values.Look<bool>(ref this.reactingToMeleeThreat, "reactingToMeleeThreat", false, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<RopingPriority>(ref this.ropingPriority, "ropingPriority", RopingPriority.Closest, false);
			Scribe_Values.Look<bool>(ref this.ropeToUnenclosedPens, "ropeToUnenclosedPens", false, false);
			Scribe_Values.Look<Direction8Way>(ref this.lookDirection, "lookDirection", Direction8Way.Invalid, false);
			Scribe_Values.Look<string>(ref this.dutyTag, "dutyTag", null, false);
			Scribe_Values.Look<string>(ref this.ritualTag, "ritualTag", null, false);
			Scribe_Values.Look<string>(ref this.controlGroupTag, "controlGroupTag", null, false);
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
			Scribe_References.Look<ILoadReferenceable>(ref this.source, "source", false);
			Scribe_Values.Look<int>(ref this.takeInventoryDelay, "takeInventoryDelay", 0, false);
			Scribe_Values.Look<bool>(ref this.draftedTend, "draftedTend", false, false);
			Scribe_Values.Look<bool>(ref this.showSpeechBubbles, "showSpeechBubbles", true, false);
			Scribe_Values.Look<Rot4>(ref this.overrideFacing, "overrideFacing", Rot4.Invalid, false);
			Scribe_Values.Look<bool>(ref this.forceMaintainFacing, "forceMaintainFacing", false, false);
			Scribe_Values.Look<bool>(ref this.speechFaceSpectatorsIfPossible, "speechFaceSpectatorsIfPossible", false, false);
			Scribe_Defs.Look<SoundDef>(ref this.speechSoundMale, "speechSoundMale");
			Scribe_Defs.Look<SoundDef>(ref this.speechSoundFemale, "speechSoundFemale");
			Scribe_Values.Look<string>(ref this.biosculpterCycleKey, "biosculpterCycleKey", null, false);
			Scribe_Values.Look<bool>(ref this.startInvoluntarySleep, "startInvoluntarySleep", false, false);
			Scribe_Values.Look<string>(ref this.reportStringOverride, "reportStringOverride", null, false);
			Scribe_Values.Look<string>(ref this.crawlingReportStringOverride, "crawlingReportStringOverride", null, false);
			Scribe_Values.Look<bool>(ref this.isLearningDesire, "isLearning", false, false);
			Scribe_Values.Look<int>(ref this.interactableIndex, "interactableIndex", 0, false);
			Scribe_Values.Look<bool>(ref this.ensureReachable, "ensureReachable", false, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				ThinkNode thinkNode = this.jobGiver;
				this.jobGiverKey = ((thinkNode != null) ? thinkNode.UniqueSaveKey : -1);
			}
			Scribe_Values.Look<int>(ref this.jobGiverKey, "lastJobGiverKey", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.jobGiverKey != -1 && !this.jobGiverThinkTree.TryGetThinkNodeWithSaveKey(this.jobGiverKey, out this.jobGiver))
				{
					Log.Warning("Could not find think node with key " + this.jobGiverKey.ToString());
				}
				if (this.verbToUse != null && this.verbToUse.BuggedAfterLoading)
				{
					this.verbToUse = null;
					Type type = base.GetType();
					Log.Warning(((type != null) ? type.ToString() : null) + " had a bugged verbToUse after loading.");
				}
			}
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x001A6784 File Offset: 0x001A4984
		public JobDriver MakeDriver(Pawn driverPawn)
		{
			JobDriver jobDriver = (JobDriver)Activator.CreateInstance(this.def.driverClass);
			jobDriver.pawn = driverPawn;
			jobDriver.job = this;
			this.lastJobDriverMade = jobDriver;
			return jobDriver;
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x001A67C0 File Offset: 0x001A49C0
		public JobDriver GetCachedDriver(Pawn driverPawn)
		{
			if (this.cachedDriver == null)
			{
				this.cachedDriver = this.MakeDriver(driverPawn);
			}
			if (this.cachedDriver.pawn != driverPawn)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to use the same driver for 2 pawns: ",
					this.cachedDriver.ToStringSafe<JobDriver>(),
					", first pawn= ",
					this.cachedDriver.pawn.ToStringSafe<Pawn>(),
					", second pawn=",
					driverPawn.ToStringSafe<Pawn>()
				}));
			}
			return this.cachedDriver;
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x001A6848 File Offset: 0x001A4A48
		public bool TryMakePreToilReservations(Pawn driverPawn, bool errorOnFailed)
		{
			return this.GetCachedDriver(driverPawn).TryMakePreToilReservations(errorOnFailed);
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x001A6857 File Offset: 0x001A4A57
		public string GetReport(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).GetReport();
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x001A6865 File Offset: 0x001A4A65
		public LocalTargetInfo GetDestination(Pawn driverPawn)
		{
			return this.targetA;
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x001A686D File Offset: 0x001A4A6D
		public bool CanBeginNow(Pawn pawn, bool whileLyingDown = false)
		{
			if (pawn.Downed)
			{
				whileLyingDown = true;
			}
			return !whileLyingDown || this.GetCachedDriver(pawn).CanBeginNowWhileLyingDown();
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x001A688C File Offset: 0x001A4A8C
		public bool JobIsSameAs(Pawn pawn, Job other)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (this.def != other.def || this.verbToUse != other.verbToUse || this.bill != other.bill)
			{
				return false;
			}
			bool? flag = this.GetCachedDriver(pawn).IsSameJobAs(other);
			if (flag != null)
			{
				return flag.Value;
			}
			return !(this.targetA != other.targetA) && !(this.targetB != other.targetB) && !(this.targetC != other.targetC) && this.commTarget == other.commTarget;
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x001A693C File Offset: 0x001A4B3C
		public bool AnyTargetIs(LocalTargetInfo target)
		{
			return target.IsValid && (this.targetA == target || this.targetB == target || this.targetC == target || (this.targetQueueA != null && this.targetQueueA.Contains(target)) || (this.targetQueueB != null && this.targetQueueB.Contains(target)));
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x001A69AC File Offset: 0x001A4BAC
		public bool AnyTargetOutsideArea(Area zone)
		{
			if (Job.IsTargetOutsideArea(this.targetA, zone) || Job.IsTargetOutsideArea(this.targetB, zone) || Job.IsTargetOutsideArea(this.targetC, zone))
			{
				return true;
			}
			if (this.targetQueueA != null)
			{
				using (List<LocalTargetInfo>.Enumerator enumerator = this.targetQueueA.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (Job.IsTargetOutsideArea(enumerator.Current, zone))
						{
							return true;
						}
					}
				}
			}
			if (this.targetQueueB != null)
			{
				using (List<LocalTargetInfo>.Enumerator enumerator = this.targetQueueB.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (Job.IsTargetOutsideArea(enumerator.Current, zone))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x001A6A88 File Offset: 0x001A4C88
		private static bool IsTargetOutsideArea(LocalTargetInfo target, Area zone)
		{
			IntVec3 cell = target.Cell;
			return cell.IsValid && !zone[cell];
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x001A6AB4 File Offset: 0x001A4CB4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted<JobDef>(this.def);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(this.GetUniqueLoadID());
			defaultInterpolatedStringHandler.AppendLiteral(")");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			if (this.targetA.IsValid)
			{
				string str = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(5, 1);
				defaultInterpolatedStringHandler2.AppendLiteral(" A = ");
				defaultInterpolatedStringHandler2.AppendFormatted<LocalTargetInfo>(this.targetA);
				text = str + defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			if (this.targetB.IsValid)
			{
				string str2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(5, 1);
				defaultInterpolatedStringHandler3.AppendLiteral(" B = ");
				defaultInterpolatedStringHandler3.AppendFormatted<LocalTargetInfo>(this.targetB);
				text = str2 + defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			if (this.targetC.IsValid)
			{
				string str3 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(5, 1);
				defaultInterpolatedStringHandler4.AppendLiteral(" C = ");
				defaultInterpolatedStringHandler4.AppendFormatted<LocalTargetInfo>(this.targetC);
				text = str3 + defaultInterpolatedStringHandler4.ToStringAndClear();
			}
			if (this.jobGiver != null)
			{
				string str4 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(26, 2);
				defaultInterpolatedStringHandler5.AppendLiteral(" Giver = ");
				defaultInterpolatedStringHandler5.AppendFormatted(this.jobGiver.GetType().Name);
				defaultInterpolatedStringHandler5.AppendLiteral(" [workGiverDef: ");
				defaultInterpolatedStringHandler5.AppendFormatted((this.workGiverDef == null) ? "null" : this.workGiverDef.ToString());
				defaultInterpolatedStringHandler5.AppendLiteral("]");
				text = str4 + defaultInterpolatedStringHandler5.ToStringAndClear();
			}
			return text;
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x001A6C38 File Offset: 0x001A4E38
		public string GetUniqueLoadID()
		{
			return "Job_" + this.loadID.ToString();
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x001A6C50 File Offset: 0x001A4E50
		public void LogDetails(Pawn pawn)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<Job>(this);
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			JobDriver curDriver = pawn.jobs.curDriver;
			if (curDriver != null)
			{
				string str = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("\ndriver details - current toil [");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(curDriver.CurToilIndex);
				defaultInterpolatedStringHandler2.AppendLiteral("]: ");
				defaultInterpolatedStringHandler2.AppendFormatted(curDriver.CurToilString);
				text = str + defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			else
			{
				text += "\nno driver.";
			}
			Log.Message(text);
		}

		// Token: 0x04002B7D RID: 11133
		public JobDef def;

		// Token: 0x04002B7E RID: 11134
		public LocalTargetInfo targetA = LocalTargetInfo.Invalid;

		// Token: 0x04002B7F RID: 11135
		public LocalTargetInfo targetB = LocalTargetInfo.Invalid;

		// Token: 0x04002B80 RID: 11136
		public LocalTargetInfo targetC = LocalTargetInfo.Invalid;

		// Token: 0x04002B81 RID: 11137
		public List<LocalTargetInfo> targetQueueA;

		// Token: 0x04002B82 RID: 11138
		public List<LocalTargetInfo> targetQueueB;

		// Token: 0x04002B83 RID: 11139
		public GlobalTargetInfo globalTarget = GlobalTargetInfo.Invalid;

		// Token: 0x04002B84 RID: 11140
		public int count = -1;

		// Token: 0x04002B85 RID: 11141
		public List<int> countQueue;

		// Token: 0x04002B86 RID: 11142
		public int loadID = -1;

		// Token: 0x04002B87 RID: 11143
		public int startTick = -1;

		// Token: 0x04002B88 RID: 11144
		public int expiryInterval = -1;

		// Token: 0x04002B89 RID: 11145
		public bool checkOverrideOnExpire;

		// Token: 0x04002B8A RID: 11146
		public bool playerForced;

		// Token: 0x04002B8B RID: 11147
		public bool playerInterruptedForced;

		// Token: 0x04002B8C RID: 11148
		public bool showCarryingInspectLine = true;

		// Token: 0x04002B8D RID: 11149
		public bool flying;

		// Token: 0x04002B8E RID: 11150
		public bool swimming;

		// Token: 0x04002B8F RID: 11151
		public TargetIndex intervalScalingTarget;

		// Token: 0x04002B90 RID: 11152
		public List<ThingCountClass> placedThings;

		// Token: 0x04002B91 RID: 11153
		public int maxNumMeleeAttacks = int.MaxValue;

		// Token: 0x04002B92 RID: 11154
		public int maxNumStaticAttacks = int.MaxValue;

		// Token: 0x04002B93 RID: 11155
		public LocomotionUrgency locomotionUrgency = LocomotionUrgency.Jog;

		// Token: 0x04002B94 RID: 11156
		public HaulMode haulMode;

		// Token: 0x04002B95 RID: 11157
		public Bill bill;

		// Token: 0x04002B96 RID: 11158
		public ICommunicable commTarget;

		// Token: 0x04002B97 RID: 11159
		public ThingDef plantDefToSow;

		// Token: 0x04002B98 RID: 11160
		public ThingDef thingDefToCarry;

		// Token: 0x04002B99 RID: 11161
		public Verb verbToUse;

		// Token: 0x04002B9A RID: 11162
		public bool haulOpportunisticDuplicates;

		// Token: 0x04002B9B RID: 11163
		public bool exitMapOnArrival;

		// Token: 0x04002B9C RID: 11164
		public bool failIfCantJoinOrCreateCaravan;

		// Token: 0x04002B9D RID: 11165
		public bool killIncappedTarget;

		// Token: 0x04002B9E RID: 11166
		public bool ignoreForbidden;

		// Token: 0x04002B9F RID: 11167
		public bool ignoreDesignations;

		// Token: 0x04002BA0 RID: 11168
		public bool canBashDoors;

		// Token: 0x04002BA1 RID: 11169
		public bool canBashFences;

		// Token: 0x04002BA2 RID: 11170
		public bool canUseRangedWeapon = true;

		// Token: 0x04002BA3 RID: 11171
		public bool haulDroppedApparel;

		// Token: 0x04002BA4 RID: 11172
		public bool restUntilHealed;

		// Token: 0x04002BA5 RID: 11173
		public bool ignoreJoyTimeAssignment;

		// Token: 0x04002BA6 RID: 11174
		public bool doUntilGatheringEnded;

		// Token: 0x04002BA7 RID: 11175
		public bool overeat;

		// Token: 0x04002BA8 RID: 11176
		public bool ingestTotalCount;

		// Token: 0x04002BA9 RID: 11177
		public bool attackDoorIfTargetLost;

		// Token: 0x04002BAA RID: 11178
		public int takeExtraIngestibles;

		// Token: 0x04002BAB RID: 11179
		public bool expireRequiresEnemiesNearby;

		// Token: 0x04002BAC RID: 11180
		public bool ensureReachable;

		// Token: 0x04002BAD RID: 11181
		public bool expireOnEnemiesNearby;

		// Token: 0x04002BAE RID: 11182
		public Lord lord;

		// Token: 0x04002BAF RID: 11183
		public bool collideWithPawns;

		// Token: 0x04002BB0 RID: 11184
		public bool forceSleep;

		// Token: 0x04002BB1 RID: 11185
		public InteractionDef interaction;

		// Token: 0x04002BB2 RID: 11186
		public bool endIfCantShootTargetFromCurPos;

		// Token: 0x04002BB3 RID: 11187
		public bool endIfCantShootInMelee;

		// Token: 0x04002BB4 RID: 11188
		public bool checkEncumbrance;

		// Token: 0x04002BB5 RID: 11189
		public float followRadius;

		// Token: 0x04002BB6 RID: 11190
		public bool endAfterTendedOnce;

		// Token: 0x04002BB7 RID: 11191
		public Quest quest;

		// Token: 0x04002BB8 RID: 11192
		public Mote mote;

		// Token: 0x04002BB9 RID: 11193
		public float psyfocusTargetLast = -1f;

		// Token: 0x04002BBA RID: 11194
		public bool wasOnMeditationTimeAssignment;

		// Token: 0x04002BBB RID: 11195
		public bool reactingToMeleeThreat;

		// Token: 0x04002BBC RID: 11196
		public bool preventFriendlyFire;

		// Token: 0x04002BBD RID: 11197
		public RopingPriority ropingPriority;

		// Token: 0x04002BBE RID: 11198
		public bool ropeToUnenclosedPens;

		// Token: 0x04002BBF RID: 11199
		public bool showSpeechBubbles = true;

		// Token: 0x04002BC0 RID: 11200
		public Direction8Way lookDirection = Direction8Way.Invalid;

		// Token: 0x04002BC1 RID: 11201
		public Rot4 overrideFacing = Rot4.Invalid;

		// Token: 0x04002BC2 RID: 11202
		public bool forceMaintainFacing;

		// Token: 0x04002BC3 RID: 11203
		public string dutyTag;

		// Token: 0x04002BC4 RID: 11204
		public string ritualTag;

		// Token: 0x04002BC5 RID: 11205
		public string controlGroupTag;

		// Token: 0x04002BC6 RID: 11206
		public int takeInventoryDelay;

		// Token: 0x04002BC7 RID: 11207
		public bool draftedTend;

		// Token: 0x04002BC8 RID: 11208
		public bool speechFaceSpectatorsIfPossible;

		// Token: 0x04002BC9 RID: 11209
		public SoundDef speechSoundMale;

		// Token: 0x04002BCA RID: 11210
		public SoundDef speechSoundFemale;

		// Token: 0x04002BCB RID: 11211
		public string biosculpterCycleKey;

		// Token: 0x04002BCC RID: 11212
		[MustTranslate]
		public string reportStringOverride;

		// Token: 0x04002BCD RID: 11213
		[MustTranslate]
		public string crawlingReportStringOverride;

		// Token: 0x04002BCE RID: 11214
		public bool startInvoluntarySleep;

		// Token: 0x04002BCF RID: 11215
		public bool isLearningDesire;

		// Token: 0x04002BD0 RID: 11216
		public int interactableIndex = -1;

		// Token: 0x04002BD1 RID: 11217
		public ThinkTreeDef jobGiverThinkTree;

		// Token: 0x04002BD2 RID: 11218
		public ThinkNode jobGiver;

		// Token: 0x04002BD3 RID: 11219
		public WorkGiverDef workGiverDef;

		// Token: 0x04002BD4 RID: 11220
		public Ability ability;

		// Token: 0x04002BD5 RID: 11221
		public ILoadReferenceable source;

		// Token: 0x04002BD6 RID: 11222
		private JobDriver cachedDriver;

		// Token: 0x04002BD7 RID: 11223
		private JobDriver lastJobDriverMade;

		// Token: 0x04002BD8 RID: 11224
		private int jobGiverKey = -1;
	}
}
