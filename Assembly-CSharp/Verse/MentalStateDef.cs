using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000195 RID: 405
	public class MentalStateDef : Def
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0003D766 File Offset: 0x0003B966
		public MentalStateWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalStateWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0003D7A6 File Offset: 0x0003B9A6
		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0003D7B4 File Offset: 0x0003B9B4
		public bool IsExtreme
		{
			get
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].intensity == MentalBreakIntensity.Extreme && allDefsListForReading[i].mentalState == this)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0003D7F9 File Offset: 0x0003B9F9
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		// Token: 0x040009C0 RID: 2496
		public Type stateClass = typeof(MentalState);

		// Token: 0x040009C1 RID: 2497
		public Type workerClass = typeof(MentalStateWorker);

		// Token: 0x040009C2 RID: 2498
		public MentalStateCategory category;

		// Token: 0x040009C3 RID: 2499
		public bool prisonersCanDo = true;

		// Token: 0x040009C4 RID: 2500
		public bool slavesCanDo = true;

		// Token: 0x040009C5 RID: 2501
		public bool inCaravanCanDo;

		// Token: 0x040009C6 RID: 2502
		public bool colonistsOnly;

		// Token: 0x040009C7 RID: 2503
		public bool slavesOnly;

		// Token: 0x040009C8 RID: 2504
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x040009C9 RID: 2505
		public bool downedCanDo;

		// Token: 0x040009CA RID: 2506
		public bool unspawnedNotInCaravanCanDo;

		// Token: 0x040009CB RID: 2507
		public bool blockNormalThoughts;

		// Token: 0x040009CC RID: 2508
		public bool stopsJobs = true;

		// Token: 0x040009CD RID: 2509
		public List<InteractionDef> blockInteractionInitiationExcept;

		// Token: 0x040009CE RID: 2510
		public List<InteractionDef> blockInteractionRecipientExcept;

		// Token: 0x040009CF RID: 2511
		public bool blockRandomInteraction;

		// Token: 0x040009D0 RID: 2512
		public EffecterDef stateEffecter;

		// Token: 0x040009D1 RID: 2513
		public TaleDef tale;

		// Token: 0x040009D2 RID: 2514
		public bool allowBeatfire;

		// Token: 0x040009D3 RID: 2515
		public DrugCategory drugCategory = DrugCategory.Any;

		// Token: 0x040009D4 RID: 2516
		public bool ignoreDrugPolicy;

		// Token: 0x040009D5 RID: 2517
		public IntRange ticksBetweenRamblingRange;

		// Token: 0x040009D6 RID: 2518
		public float recoveryMtbDays = 1f;

		// Token: 0x040009D7 RID: 2519
		public int minTicksBeforeRecovery = 500;

		// Token: 0x040009D8 RID: 2520
		public int maxTicksBeforeRecovery = 99999999;

		// Token: 0x040009D9 RID: 2521
		public bool recoverFromSleep;

		// Token: 0x040009DA RID: 2522
		public bool recoverFromDowned = true;

		// Token: 0x040009DB RID: 2523
		public bool recoverFromCaptured = true;

		// Token: 0x040009DC RID: 2524
		public bool recoverFromCollapsingExhausted = true;

		// Token: 0x040009DD RID: 2525
		public ThoughtDef moodRecoveryThought;

		// Token: 0x040009DE RID: 2526
		public bool allowGuilty = true;

		// Token: 0x040009DF RID: 2527
		[MustTranslate]
		public string beginLetter;

		// Token: 0x040009E0 RID: 2528
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x040009E1 RID: 2529
		public LetterDef beginLetterDef;

		// Token: 0x040009E2 RID: 2530
		public Color nameColor = Color.green;

		// Token: 0x040009E3 RID: 2531
		[MustTranslate]
		public string recoveryMessage;

		// Token: 0x040009E4 RID: 2532
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x040009E5 RID: 2533
		public bool escapingPrisonersIgnore;

		// Token: 0x040009E6 RID: 2534
		public bool blocksDefendAndExpandHive;

		// Token: 0x040009E7 RID: 2535
		private MentalStateWorker workerInt;
	}
}
