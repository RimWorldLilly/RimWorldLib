using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015C2 RID: 5570
	public class SkillRecord : IExposable, IComparable<SkillRecord>
	{
		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x06008A07 RID: 35335 RVA: 0x0031F21D File Offset: 0x0031D41D
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x06008A08 RID: 35336 RVA: 0x0031F225 File Offset: 0x0031D425
		// (set) Token: 0x06008A09 RID: 35337 RVA: 0x0031F22E File Offset: 0x0031D42E
		public int Level
		{
			get
			{
				return this.GetLevel(true);
			}
			set
			{
				this.levelInt = Mathf.Clamp(value, 0, 20);
			}
		}

		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x06008A0A RID: 35338 RVA: 0x0031F23F File Offset: 0x0031D43F
		public float XpRequiredForLevelUp
		{
			get
			{
				return SkillRecord.XpRequiredToLevelUpFrom(this.levelInt);
			}
		}

		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x06008A0B RID: 35339 RVA: 0x0031F24C File Offset: 0x0031D44C
		public float XpProgressPercent
		{
			get
			{
				return this.xpSinceLastLevel / this.XpRequiredForLevelUp;
			}
		}

		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x06008A0C RID: 35340 RVA: 0x0031F25C File Offset: 0x0031D45C
		public float XpTotalEarned
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.levelInt; i++)
				{
					num += SkillRecord.XpRequiredToLevelUpFrom(i);
				}
				return num;
			}
		}

		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x06008A0D RID: 35341 RVA: 0x0031F28A File Offset: 0x0031D48A
		public bool TotallyDisabled
		{
			get
			{
				if (this.cachedTotallyDisabled == BoolUnknown.Unknown)
				{
					this.cachedTotallyDisabled = (this.CalculateTotallyDisabled() ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.PermanentlyDisabled || this.cachedTotallyDisabled == BoolUnknown.True;
			}
		}

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x06008A0E RID: 35342 RVA: 0x0031F2BA File Offset: 0x0031D4BA
		public bool PermanentlyDisabled
		{
			get
			{
				if (this.cachedPermanentlyDisabled == BoolUnknown.Unknown)
				{
					this.cachedPermanentlyDisabled = (this.CalculatePermanentlyDisabled() ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.cachedPermanentlyDisabled == BoolUnknown.True;
			}
		}

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x06008A0F RID: 35343 RVA: 0x0031F2E0 File Offset: 0x0031D4E0
		public string LevelDescriptor
		{
			get
			{
				switch (this.GetLevelForUI(true))
				{
				case 0:
					return "Skill0".Translate();
				case 1:
					return "Skill1".Translate();
				case 2:
					return "Skill2".Translate();
				case 3:
					return "Skill3".Translate();
				case 4:
					return "Skill4".Translate();
				case 5:
					return "Skill5".Translate();
				case 6:
					return "Skill6".Translate();
				case 7:
					return "Skill7".Translate();
				case 8:
					return "Skill8".Translate();
				case 9:
					return "Skill9".Translate();
				case 10:
					return "Skill10".Translate();
				case 11:
					return "Skill11".Translate();
				case 12:
					return "Skill12".Translate();
				case 13:
					return "Skill13".Translate();
				case 14:
					return "Skill14".Translate();
				case 15:
					return "Skill15".Translate();
				case 16:
					return "Skill16".Translate();
				case 17:
					return "Skill17".Translate();
				case 18:
					return "Skill18".Translate();
				case 19:
					return "Skill19".Translate();
				case 20:
					return "Skill20".Translate();
				default:
					return "Unknown";
				}
			}
		}

		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x06008A10 RID: 35344 RVA: 0x0031F4A9 File Offset: 0x0031D6A9
		public bool LearningSaturatedToday
		{
			get
			{
				return this.xpSinceMidnight > 4000f;
			}
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x06008A11 RID: 35345 RVA: 0x0031F4B8 File Offset: 0x0031D6B8
		public int Aptitude
		{
			get
			{
				if (this.aptitudeCached == null)
				{
					this.aptitudeCached = new int?(0);
					if (ModsConfig.BiotechActive && this.pawn.genes != null)
					{
						foreach (Gene gene in this.pawn.genes.GenesListForReading)
						{
							if (gene.Active)
							{
								this.aptitudeCached += gene.def.AptitudeFor(this.def);
							}
						}
					}
					if (ModsConfig.AnomalyActive)
					{
						Pawn_StoryTracker story = this.pawn.story;
						if (((story != null) ? story.traits : null) != null)
						{
							foreach (Trait trait in this.pawn.story.traits.TraitsSorted)
							{
								if (!trait.Suppressed)
								{
									this.aptitudeCached += trait.def.DataAtDegree(trait.Degree).AptitudeFor(this.def);
								}
							}
						}
						Pawn_HealthTracker health = this.pawn.health;
						if (((health != null) ? health.hediffSet : null) != null)
						{
							foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
							{
								this.aptitudeCached += hediff.def.AptitudeFor(this.def);
							}
						}
					}
				}
				return this.aptitudeCached.Value;
			}
		}

		// Token: 0x06008A12 RID: 35346 RVA: 0x0031F710 File Offset: 0x0031D910
		public SkillRecord()
		{
		}

		// Token: 0x06008A13 RID: 35347 RVA: 0x0031F726 File Offset: 0x0031D926
		public SkillRecord(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008A14 RID: 35348 RVA: 0x0031F743 File Offset: 0x0031D943
		public SkillRecord(Pawn pawn, SkillDef def)
		{
			this.pawn = pawn;
			this.def = def;
		}

		// Token: 0x06008A15 RID: 35349 RVA: 0x0031F768 File Offset: 0x0031D968
		public void ExposeData()
		{
			Scribe_Defs.Look<SkillDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.levelInt, "level", 0, false);
			Scribe_Values.Look<float>(ref this.xpSinceLastLevel, "xpSinceLastLevel", 0f, false);
			Scribe_Values.Look<Passion>(ref this.passion, "passion", Passion.None, false);
			Scribe_Values.Look<float>(ref this.xpSinceMidnight, "xpSinceMidnight", 0f, false);
		}

		// Token: 0x06008A16 RID: 35350 RVA: 0x0031F7D8 File Offset: 0x0031D9D8
		public void Interval()
		{
			if (ModsConfig.AnomalyActive && this.pawn.story.traits.HasTrait(TraitDefOf.PerfectMemory))
			{
				return;
			}
			float num = this.pawn.story.traits.HasTrait(TraitDefOf.GreatMemory) ? 0.5f : 1f;
			switch (this.levelInt)
			{
			case 10:
				this.Learn(-0.1f * num, false, false);
				return;
			case 11:
				this.Learn(-0.2f * num, false, false);
				return;
			case 12:
				this.Learn(-0.4f * num, false, false);
				return;
			case 13:
				this.Learn(-0.6f * num, false, false);
				return;
			case 14:
				this.Learn(-1f * num, false, false);
				return;
			case 15:
				this.Learn(-1.8f * num, false, false);
				return;
			case 16:
				this.Learn(-2.8f * num, false, false);
				return;
			case 17:
				this.Learn(-4f * num, false, false);
				return;
			case 18:
				this.Learn(-6f * num, false, false);
				return;
			case 19:
				this.Learn(-8f * num, false, false);
				return;
			case 20:
				this.Learn(-12f * num, false, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06008A17 RID: 35351 RVA: 0x0031F91E File Offset: 0x0031DB1E
		public static float XpRequiredToLevelUpFrom(int startingLevel)
		{
			return SkillRecord.XpForLevelUpCurve.Evaluate((float)startingLevel);
		}

		// Token: 0x06008A18 RID: 35352 RVA: 0x0031F92C File Offset: 0x0031DB2C
		public void Learn(float xp, bool direct = false, bool ignoreLearnRate = false)
		{
			if (this.TotallyDisabled)
			{
				return;
			}
			if (xp < 0f && this.levelInt == 0)
			{
				return;
			}
			bool flag = false;
			if (xp > 0f && !ignoreLearnRate)
			{
				xp *= this.LearnRateFactor(direct);
			}
			this.xpSinceLastLevel += xp;
			if (!direct)
			{
				this.xpSinceMidnight += xp;
			}
			if (this.levelInt == 20 && this.xpSinceLastLevel > this.XpRequiredForLevelUp - 1f)
			{
				this.xpSinceLastLevel = this.XpRequiredForLevelUp - 1f;
			}
			while (this.xpSinceLastLevel >= this.XpRequiredForLevelUp)
			{
				this.xpSinceLastLevel -= this.XpRequiredForLevelUp;
				this.levelInt++;
				flag = true;
				if (this.levelInt == 14)
				{
					if (this.passion == Passion.None)
					{
						TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithoutPassion, new object[]
						{
							this.pawn,
							this.def
						});
					}
					else
					{
						TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithPassion, new object[]
						{
							this.pawn,
							this.def
						});
					}
				}
				if (this.levelInt >= 20)
				{
					this.levelInt = 20;
					this.xpSinceLastLevel = Mathf.Clamp(this.xpSinceLastLevel, 0f, this.XpRequiredForLevelUp - 1f);
					IL_190:
					while (this.xpSinceLastLevel <= -1000f)
					{
						this.levelInt--;
						this.xpSinceLastLevel += this.XpRequiredForLevelUp;
						if (this.levelInt <= 0)
						{
							this.levelInt = 0;
							this.xpSinceLastLevel = 0f;
							break;
						}
					}
					if (flag && this.pawn.IsColonist && this.pawn.SpawnedOrAnyParentSpawned)
					{
						MoteMaker.ThrowText(this.pawn.DrawPosHeld ?? this.pawn.PositionHeld.ToVector3Shifted(), this.pawn.MapHeld, this.def.LabelCap + "\n" + "TextMote_SkillUp".Translate(this.Level), -1f);
					}
					return;
				}
			}
			goto IL_190;
		}

		// Token: 0x06008A19 RID: 35353 RVA: 0x0031FB74 File Offset: 0x0031DD74
		public int CompareTo(SkillRecord other)
		{
			int result = this.Level - other.Level;
			if (this.Level != other.Level)
			{
				return result;
			}
			if (this.xpSinceLastLevel == other.xpSinceLastLevel)
			{
				return 0;
			}
			if (this.xpSinceLastLevel <= other.xpSinceLastLevel)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06008A1A RID: 35354 RVA: 0x0031FBC0 File Offset: 0x0031DDC0
		public int GetLevel(bool includeAptitudes = true)
		{
			if (this.TotallyDisabled)
			{
				return 0;
			}
			int num = this.levelInt;
			if (includeAptitudes)
			{
				num += this.Aptitude;
			}
			return Mathf.Clamp(num, 0, 20);
		}

		// Token: 0x06008A1B RID: 35355 RVA: 0x0031FBF4 File Offset: 0x0031DDF4
		public int GetLevelForUI(bool includeAptitudes = true)
		{
			if (this.PermanentlyDisabled)
			{
				return 0;
			}
			int num = this.levelInt;
			if (includeAptitudes)
			{
				num += this.Aptitude;
			}
			return Mathf.Clamp(num, 0, 20);
		}

		// Token: 0x06008A1C RID: 35356 RVA: 0x0031FC27 File Offset: 0x0031DE27
		public int GetUnclampedLevel()
		{
			if (this.PermanentlyDisabled)
			{
				return 0;
			}
			return this.levelInt + this.Aptitude;
		}

		// Token: 0x06008A1D RID: 35357 RVA: 0x0031FC40 File Offset: 0x0031DE40
		public float LearnRateFactor(bool direct = false)
		{
			if (DebugSettings.fastLearning)
			{
				return 200f;
			}
			float num;
			switch (this.passion)
			{
			case Passion.None:
				num = 0.35f;
				break;
			case Passion.Minor:
				num = 1f;
				break;
			case Passion.Major:
				num = 1.5f;
				break;
			default:
				throw new NotImplementedException("Passion level " + this.passion.ToString());
			}
			if (!direct)
			{
				num *= this.pawn.GetStatValue(StatDefOf.GlobalLearningFactor, true, -1);
				if (this.def == SkillDefOf.Animals)
				{
					num *= this.pawn.GetStatValue(StatDefOf.AnimalsLearningFactor, true, -1);
				}
				if (this.LearningSaturatedToday)
				{
					num *= 0.2f;
				}
			}
			return num;
		}

		// Token: 0x06008A1E RID: 35358 RVA: 0x0031FCFC File Offset: 0x0031DEFC
		public void EnsureMinLevelWithMargin(int minLevel)
		{
			if (this.TotallyDisabled)
			{
				return;
			}
			if (this.levelInt < minLevel || (this.levelInt == minLevel && this.xpSinceLastLevel < this.XpRequiredForLevelUp / 2f))
			{
				this.levelInt = minLevel;
				this.xpSinceLastLevel = this.XpRequiredForLevelUp / 2f;
			}
		}

		// Token: 0x06008A1F RID: 35359 RVA: 0x0031FD51 File Offset: 0x0031DF51
		public void Notify_SkillDisablesChanged()
		{
			this.cachedTotallyDisabled = BoolUnknown.Unknown;
			this.cachedPermanentlyDisabled = BoolUnknown.Unknown;
		}

		// Token: 0x06008A20 RID: 35360 RVA: 0x0031FD61 File Offset: 0x0031DF61
		public void DirtyAptitudes()
		{
			this.aptitudeCached = null;
		}

		// Token: 0x06008A21 RID: 35361 RVA: 0x0031FD6F File Offset: 0x0031DF6F
		private bool CalculateTotallyDisabled()
		{
			return this.def.IsDisabled(this.pawn.CombinedDisabledWorkTags, this.pawn.GetDisabledWorkTypes(false));
		}

		// Token: 0x06008A22 RID: 35362 RVA: 0x0031FD93 File Offset: 0x0031DF93
		private bool CalculatePermanentlyDisabled()
		{
			return this.def.IsDisabled(this.pawn.CombinedDisabledWorkTags, this.pawn.GetDisabledWorkTypes(true));
		}

		// Token: 0x06008A23 RID: 35363 RVA: 0x0031FDB8 File Offset: 0x0031DFB8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.def.defName,
				": ",
				this.levelInt.ToString(),
				" (",
				this.xpSinceLastLevel.ToString(),
				"xp)"
			});
		}

		// Token: 0x040054F2 RID: 21746
		private Pawn pawn;

		// Token: 0x040054F3 RID: 21747
		public SkillDef def;

		// Token: 0x040054F4 RID: 21748
		public int levelInt;

		// Token: 0x040054F5 RID: 21749
		public Passion passion;

		// Token: 0x040054F6 RID: 21750
		public float xpSinceLastLevel;

		// Token: 0x040054F7 RID: 21751
		public float xpSinceMidnight;

		// Token: 0x040054F8 RID: 21752
		private BoolUnknown cachedTotallyDisabled = BoolUnknown.Unknown;

		// Token: 0x040054F9 RID: 21753
		private BoolUnknown cachedPermanentlyDisabled = BoolUnknown.Unknown;

		// Token: 0x040054FA RID: 21754
		private int? aptitudeCached;

		// Token: 0x040054FB RID: 21755
		public const int IntervalTicks = 200;

		// Token: 0x040054FC RID: 21756
		public const int MinLevel = 0;

		// Token: 0x040054FD RID: 21757
		public const int MaxLevel = 20;

		// Token: 0x040054FE RID: 21758
		public const int MaxFullRateXpPerDay = 4000;

		// Token: 0x040054FF RID: 21759
		public const int MasterSkillThreshold = 14;

		// Token: 0x04005500 RID: 21760
		public const float SaturatedLearningFactor = 0.2f;

		// Token: 0x04005501 RID: 21761
		public const float LearnFactorPassionNone = 0.35f;

		// Token: 0x04005502 RID: 21762
		public const float LearnFactorPassionMinor = 1f;

		// Token: 0x04005503 RID: 21763
		public const float LearnFactorPassionMajor = 1.5f;

		// Token: 0x04005504 RID: 21764
		public const float MinXPAmount = -1000f;

		// Token: 0x04005505 RID: 21765
		private static readonly SimpleCurve XpForLevelUpCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1000f),
				true
			},
			{
				new CurvePoint(9f, 10000f),
				true
			},
			{
				new CurvePoint(19f, 30000f),
				true
			}
		};
	}
}
