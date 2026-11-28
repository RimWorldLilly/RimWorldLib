using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020015A1 RID: 5537
	public class Pawn_PsychicEntropyTracker : IExposable
	{
		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x0600888B RID: 34955 RVA: 0x003163D1 File Offset: 0x003145D1
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x0600888C RID: 34956 RVA: 0x003163D9 File Offset: 0x003145D9
		public float MaxEntropy
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true, -1);
			}
		}

		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x0600888D RID: 34957 RVA: 0x003163ED File Offset: 0x003145ED
		public float MaxPotentialEntropy
		{
			get
			{
				return Mathf.Max(this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true, -1), this.MaxEntropy);
			}
		}

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x0600888E RID: 34958 RVA: 0x0031640C File Offset: 0x0031460C
		public float RecoveryRate
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyRecoveryRate, true, -1);
			}
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x0600888F RID: 34959 RVA: 0x00316420 File Offset: 0x00314620
		public float EntropyValue
		{
			get
			{
				return this.currentEntropy;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x06008890 RID: 34960 RVA: 0x00316428 File Offset: 0x00314628
		public float CurrentPsyfocus
		{
			get
			{
				return this.currentPsyfocus;
			}
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x06008891 RID: 34961 RVA: 0x00316430 File Offset: 0x00314630
		public float TargetPsyfocus
		{
			get
			{
				return this.targetPsyfocus;
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x06008892 RID: 34962 RVA: 0x00316438 File Offset: 0x00314638
		public int MaxAbilityLevel
		{
			get
			{
				return Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[this.PsyfocusBand];
			}
		}

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x06008893 RID: 34963 RVA: 0x0031644A File Offset: 0x0031464A
		public bool IsCurrentlyMeditating
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastMeditationTick + 10;
			}
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x06008894 RID: 34964 RVA: 0x00316464 File Offset: 0x00314664
		public float PsychicSensitivity
		{
			get
			{
				if (this.psychicSensitivityCachedTick != Find.TickManager.TicksGame)
				{
					this.psychicSensitivityCached = this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
					this.psychicSensitivityCachedTick = Find.TickManager.TicksGame;
				}
				return this.psychicSensitivityCached;
			}
		}

		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x06008895 RID: 34965 RVA: 0x003164B1 File Offset: 0x003146B1
		public bool IsPsychicallySensitive
		{
			get
			{
				return this.PsychicSensitivity > float.Epsilon;
			}
		}

		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x06008896 RID: 34966 RVA: 0x003164C0 File Offset: 0x003146C0
		public float EntropyRelativeValue
		{
			get
			{
				return this.EntropyToRelativeValue(this.currentEntropy);
			}
		}

		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x06008897 RID: 34967 RVA: 0x003164D0 File Offset: 0x003146D0
		public PsychicEntropySeverity Severity
		{
			get
			{
				PsychicEntropySeverity result = PsychicEntropySeverity.Safe;
				foreach (PsychicEntropySeverity psychicEntropySeverity in Pawn_PsychicEntropyTracker.EntropyThresholds.Keys)
				{
					if (Pawn_PsychicEntropyTracker.EntropyThresholds[psychicEntropySeverity] >= this.EntropyRelativeValue)
					{
						break;
					}
					result = psychicEntropySeverity;
				}
				return result;
			}
		}

		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x06008898 RID: 34968 RVA: 0x00316538 File Offset: 0x00314738
		public int PsyfocusBand
		{
			get
			{
				if (this.currentPsyfocus < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[1])
				{
					return 0;
				}
				if (this.currentPsyfocus < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[2])
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x06008899 RID: 34969 RVA: 0x00316565 File Offset: 0x00314765
		public Hediff_Psylink Psylink
		{
			get
			{
				if (this.psylinkCachedForTick != Find.TickManager.TicksGame)
				{
					this.psylinkCached = this.pawn.GetMainPsylinkSource();
					this.psylinkCachedForTick = Find.TickManager.TicksGame;
				}
				return this.psylinkCached;
			}
		}

		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x0600889A RID: 34970 RVA: 0x003165A0 File Offset: 0x003147A0
		public bool NeedsPsyfocus
		{
			get
			{
				return this.Psylink != null && !this.pawn.Suspended && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x0600889B RID: 34971 RVA: 0x003165D8 File Offset: 0x003147D8
		private float PsyfocusFallPerDay
		{
			get
			{
				if (this.pawn.GetPsylinkLevel() == 0)
				{
					return 0f;
				}
				return Pawn_PsychicEntropyTracker.FallRatePerPsyfocusBand[this.PsyfocusBand];
			}
		}

		// Token: 0x0600889C RID: 34972 RVA: 0x00316600 File Offset: 0x00314800
		public Pawn_PsychicEntropyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600889D RID: 34973 RVA: 0x0031664C File Offset: 0x0031484C
		public static void ResetStaticData()
		{
			Pawn_PsychicEntropyTracker.EntropyThresholdSounds = new Dictionary<PsychicEntropySeverity, SoundDef>
			{
				{
					PsychicEntropySeverity.Overloaded,
					SoundDefOf.PsychicEntropyOverloaded
				},
				{
					PsychicEntropySeverity.VeryOverloaded,
					SoundDefOf.PsychicEntropyHyperloaded
				},
				{
					PsychicEntropySeverity.Extreme,
					SoundDefOf.PsychicEntropyBrainCharring
				},
				{
					PsychicEntropySeverity.Overwhelming,
					SoundDefOf.PsychicEntropyBrainRoasting
				}
			};
		}

		// Token: 0x0600889E RID: 34974 RVA: 0x00316688 File Offset: 0x00314888
		public void PsychicEntropyTrackerTickInterval(int delta)
		{
			if (this.currentEntropy > 1E-45f)
			{
				this.currentEntropy = Mathf.Max(this.currentEntropy - 1.TicksToSeconds() * this.RecoveryRate * (float)delta, 0f);
			}
			if (this.currentEntropy > 1E-45f && !this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicEntropy, false))
			{
				this.pawn.health.AddHediff(HediffDefOf.PsychicEntropy, null, null, null);
			}
			if (this.currentEntropy > 1E-45f)
			{
				if (this.ticksSinceLastMote >= Pawn_PsychicEntropyTracker.TicksBetweenMotes[(int)this.Severity])
				{
					if (this.pawn.Spawned)
					{
						FleckMaker.AttachedOverlay(this.pawn, FleckDefOf.EntropyPulse, Vector3.zero, 1f, -1f);
					}
					this.ticksSinceLastMote = 0;
				}
				else
				{
					this.ticksSinceLastMote += delta;
				}
			}
			else
			{
				this.ticksSinceLastMote = 0;
			}
			if (this.NeedsPsyfocus && this.pawn.IsHashIntervalTick(150, delta))
			{
				float num = 400f;
				if (!this.IsCurrentlyMeditating)
				{
					this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus - this.PsyfocusFallPerDay / num, 0f, 1f);
				}
				MeditationUtility.CheckMeditationScheduleTeachOpportunity(this.pawn);
			}
		}

		// Token: 0x0600889F RID: 34975 RVA: 0x003167D9 File Offset: 0x003149D9
		public bool WouldOverflowEntropy(float value)
		{
			return this.limitEntropyAmount && this.currentEntropy + value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true, -1) > this.MaxEntropy;
		}

		// Token: 0x060088A0 RID: 34976 RVA: 0x00316808 File Offset: 0x00314A08
		public bool TryAddEntropy(float value, Thing source = null, bool scale = true, bool overLimit = false)
		{
			PsychicEntropySeverity severity = this.Severity;
			float num = scale ? (value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true, -1)) : value;
			if (!this.WouldOverflowEntropy(num) || overLimit)
			{
				this.currentEntropy = Mathf.Max(this.currentEntropy + num, 0f);
				foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
				{
					hediff.Notify_EntropyGained(value, num, source);
				}
				if (severity != this.Severity && num > 0f && this.Severity != PsychicEntropySeverity.Safe)
				{
					Pawn_PsychicEntropyTracker.EntropyThresholdSounds[this.Severity].PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
					if (severity < PsychicEntropySeverity.Overloaded && this.Severity >= PsychicEntropySeverity.Overloaded)
					{
						Messages.Message("MessageWentOverPsychicEntropyLimit".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060088A1 RID: 34977 RVA: 0x00316950 File Offset: 0x00314B50
		public void RemoveAllEntropy()
		{
			this.currentEntropy = 0f;
		}

		// Token: 0x060088A2 RID: 34978 RVA: 0x0031695D File Offset: 0x00314B5D
		[Obsolete]
		public void GainPsyfocus(Thing focus = null)
		{
			this.GainPsyfocus_NewTemp(1, focus);
		}

		// Token: 0x060088A3 RID: 34979 RVA: 0x00316968 File Offset: 0x00314B68
		public void GainPsyfocus_NewTemp(int delta, Thing focus = null)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + MeditationUtility.PsyfocusGainPerTick(this.pawn, focus) * (float)delta, 0f, 1f);
			if (focus != null && !focus.Destroyed)
			{
				CompMeditationFocus compMeditationFocus = focus.TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus == null)
				{
					return;
				}
				compMeditationFocus.Used(this.pawn);
			}
		}

		// Token: 0x060088A4 RID: 34980 RVA: 0x003169C1 File Offset: 0x00314BC1
		public void Notify_Meditated()
		{
			this.lastMeditationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060088A5 RID: 34981 RVA: 0x003169D3 File Offset: 0x00314BD3
		public void OffsetPsyfocusDirectly(float offset)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + offset, 0f, 1f);
		}

		// Token: 0x060088A6 RID: 34982 RVA: 0x003169F2 File Offset: 0x00314BF2
		public void RechargePsyfocus()
		{
			this.currentPsyfocus = 1f;
		}

		// Token: 0x060088A7 RID: 34983 RVA: 0x003169FF File Offset: 0x00314BFF
		public void SetInitialPsyfocusLevel()
		{
			if (this.pawn.IsColonist && !this.pawn.IsQuestLodger())
			{
				this.currentPsyfocus = 0.75f;
				return;
			}
			this.currentPsyfocus = Rand.Range(0.5f, 0.7f);
		}

		// Token: 0x060088A8 RID: 34984 RVA: 0x00316A3C File Offset: 0x00314C3C
		public void SetPsyfocusTarget(float val)
		{
			this.targetPsyfocus = Mathf.Clamp(val, 0f, 1f);
		}

		// Token: 0x060088A9 RID: 34985 RVA: 0x00316A54 File Offset: 0x00314C54
		public float EntropyToRelativeValue(float val)
		{
			if (val < 1E-45f)
			{
				return 0f;
			}
			if (val < this.MaxEntropy)
			{
				if (this.MaxEntropy <= 1E-45f)
				{
					return 0f;
				}
				return val / this.MaxEntropy;
			}
			else
			{
				if (this.MaxPotentialEntropy <= 1E-45f)
				{
					return 0f;
				}
				return 1f + (val - this.MaxEntropy) / this.MaxPotentialEntropy;
			}
		}

		// Token: 0x060088AA RID: 34986 RVA: 0x00316ABC File Offset: 0x00314CBC
		public void Notify_GainedPsylink()
		{
			this.currentPsyfocus = Mathf.Max(this.currentPsyfocus, 0.75f);
		}

		// Token: 0x060088AB RID: 34987 RVA: 0x00316AD4 File Offset: 0x00314CD4
		public void Notify_PawnDied()
		{
			this.currentEntropy = 0f;
			this.currentPsyfocus = 0f;
		}

		// Token: 0x060088AC RID: 34988 RVA: 0x00316AEC File Offset: 0x00314CEC
		public bool NeedToShowGizmo()
		{
			return ModsConfig.RoyaltyActive && this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > float.Epsilon && (this.EntropyValue > float.Epsilon || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false));
		}

		// Token: 0x060088AD RID: 34989 RVA: 0x00316B47 File Offset: 0x00314D47
		public Gizmo GetGizmo()
		{
			if (this.gizmo == null)
			{
				this.gizmo = new PsychicEntropyGizmo(this);
			}
			return this.gizmo;
		}

		// Token: 0x060088AE RID: 34990 RVA: 0x00316B64 File Offset: 0x00314D64
		public string PsyfocusTipString(float psyfocusTargetOverride = -1f)
		{
			if (Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached == null)
			{
				for (int i = 0; i < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; i++)
				{
					Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "PsyfocusLevelInfoRange".Translate((Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i] * 100f).ToStringDecimalIfSmall(), (Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i + 1] * 100f).ToStringDecimalIfSmall()) + ": " + "PsyfocusLevelInfoPsycasts".Translate(Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[i]) + "\n";
				}
				Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "\n";
				for (int j = 0; j < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; j++)
				{
					Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "PsyfocusLevelInfoRange".Translate((Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[j] * 100f).ToStringDecimalIfSmall(), (Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[j + 1] * 100f).ToStringDecimalIfSmall()) + ": " + "PsyfocusLevelInfoFallRate".Translate(Pawn_PsychicEntropyTracker.FallRatePerPsyfocusBand[j].ToStringPercent()) + "\n";
				}
			}
			return (("Psyfocus".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor) + this.currentPsyfocus.ToStringPercent("0.#") + "\n" + "DesiredPsyfocus".Translate() + ": " + ((psyfocusTargetOverride >= 0f) ? psyfocusTargetOverride : this.targetPsyfocus).ToStringPercent("0.#") + "\n\n" + "DesiredPsyfocusDesc".Translate(this.pawn.Named("PAWN")) + "\n\n" + "PsyfocusDesc".Translate() + ":\n\n" + Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached).Resolve();
		}

		// Token: 0x060088AF RID: 34991 RVA: 0x00316DB0 File Offset: 0x00314FB0
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.currentEntropy, "currentEntropy", 0f, false);
			Scribe_Values.Look<float>(ref this.currentPsyfocus, "currentPsyfocus", -1f, false);
			Scribe_Values.Look<float>(ref this.targetPsyfocus, "targetPsyfocus", 0.5f, false);
			Scribe_Values.Look<int>(ref this.lastMeditationTick, "lastMeditationTick", -1, false);
			Scribe_Values.Look<bool>(ref this.limitEntropyAmount, "limitEntropyAmount", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.currentPsyfocus < 0f)
			{
				this.SetInitialPsyfocusLevel();
			}
		}

		// Token: 0x0400542A RID: 21546
		private Pawn pawn;

		// Token: 0x0400542B RID: 21547
		private float currentEntropy;

		// Token: 0x0400542C RID: 21548
		private int ticksSinceLastMote;

		// Token: 0x0400542D RID: 21549
		public bool limitEntropyAmount = true;

		// Token: 0x0400542E RID: 21550
		private float currentPsyfocus = -1f;

		// Token: 0x0400542F RID: 21551
		private float targetPsyfocus = 0.5f;

		// Token: 0x04005430 RID: 21552
		private int lastMeditationTick = -1;

		// Token: 0x04005431 RID: 21553
		private Gizmo gizmo;

		// Token: 0x04005432 RID: 21554
		private Hediff_Psylink psylinkCached;

		// Token: 0x04005433 RID: 21555
		private int psylinkCachedForTick = -1;

		// Token: 0x04005434 RID: 21556
		private static readonly int[] TicksBetweenMotes = new int[]
		{
			300,
			200,
			100,
			75,
			50
		};

		// Token: 0x04005435 RID: 21557
		public const float PercentageAfterGainingPsylink = 0.75f;

		// Token: 0x04005436 RID: 21558
		public const int PsyfocusUpdateInterval = 150;

		// Token: 0x04005437 RID: 21559
		public const float PsyfocusCostTolerance = 0.0005f;

		// Token: 0x04005438 RID: 21560
		public static readonly Dictionary<PsychicEntropySeverity, float> EntropyThresholds = new Dictionary<PsychicEntropySeverity, float>
		{
			{
				PsychicEntropySeverity.Safe,
				0f
			},
			{
				PsychicEntropySeverity.Overloaded,
				1f
			},
			{
				PsychicEntropySeverity.VeryOverloaded,
				1.33f
			},
			{
				PsychicEntropySeverity.Extreme,
				1.66f
			},
			{
				PsychicEntropySeverity.Overwhelming,
				2f
			}
		};

		// Token: 0x04005439 RID: 21561
		public static readonly List<float> PsyfocusBandPercentages = new List<float>
		{
			0f,
			0.25f,
			0.5f,
			1f
		};

		// Token: 0x0400543A RID: 21562
		public static readonly List<float> FallRatePerPsyfocusBand = new List<float>
		{
			0.035f,
			0.055f,
			0.075f
		};

		// Token: 0x0400543B RID: 21563
		public static readonly List<int> MaxAbilityLevelPerPsyfocusBand = new List<int>
		{
			2,
			4,
			6
		};

		// Token: 0x0400543C RID: 21564
		public static Dictionary<PsychicEntropySeverity, SoundDef> EntropyThresholdSounds;

		// Token: 0x0400543D RID: 21565
		private float psychicSensitivityCached;

		// Token: 0x0400543E RID: 21566
		private int psychicSensitivityCachedTick = -1;

		// Token: 0x0400543F RID: 21567
		public static string psyfocusLevelInfoCached = null;
	}
}
