using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E7E RID: 7806
	[DefOf]
	public static class HediffDefOf
	{
		// Token: 0x0600CA39 RID: 51769 RVA: 0x004A5C8B File Offset: 0x004A3E8B
		static HediffDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
		}

		// Token: 0x0400797F RID: 31103
		public static HediffDef Cut;

		// Token: 0x04007980 RID: 31104
		public static HediffDef SurgicalCut;

		// Token: 0x04007981 RID: 31105
		public static HediffDef ExecutionCut;

		// Token: 0x04007982 RID: 31106
		public static HediffDef Bite;

		// Token: 0x04007983 RID: 31107
		public static HediffDef MissingBodyPart;

		// Token: 0x04007984 RID: 31108
		public static HediffDef Misc;

		// Token: 0x04007985 RID: 31109
		[MayRequireBiotech]
		public static HediffDef BloodfeederMark;

		// Token: 0x04007986 RID: 31110
		[MayRequireAnomaly]
		public static HediffDef Decayed;

		// Token: 0x04007987 RID: 31111
		[MayRequireAnomaly]
		public static HediffDef Digested;

		// Token: 0x04007988 RID: 31112
		[MayRequireOdyssey]
		public static HediffDef PorcupineQuill;

		// Token: 0x04007989 RID: 31113
		public static HediffDef BloodLoss;

		// Token: 0x0400798A RID: 31114
		public static HediffDef Hypothermia;

		// Token: 0x0400798B RID: 31115
		public static HediffDef Heatstroke;

		// Token: 0x0400798C RID: 31116
		public static HediffDef Malnutrition;

		// Token: 0x0400798D RID: 31117
		public static HediffDef ToxicBuildup;

		// Token: 0x0400798E RID: 31118
		public static HediffDef PsychicShock;

		// Token: 0x0400798F RID: 31119
		public static HediffDef ResurrectionSickness;

		// Token: 0x04007990 RID: 31120
		public static HediffDef ResurrectionPsychosis;

		// Token: 0x04007991 RID: 31121
		[MayRequireBiotech]
		public static HediffDef PollutionStimulus;

		// Token: 0x04007992 RID: 31122
		[MayRequireBiotech]
		public static HediffDef ToxGasExposure;

		// Token: 0x04007993 RID: 31123
		[MayRequireAnomaly]
		public static HediffDef Shambler;

		// Token: 0x04007994 RID: 31124
		[MayRequireAnomaly]
		public static HediffDef ShamblerCorpse;

		// Token: 0x04007995 RID: 31125
		[MayRequireAnomaly]
		public static HediffDef Rising;

		// Token: 0x04007996 RID: 31126
		[MayRequireAnomaly]
		public static HediffDef DeathRefusal;

		// Token: 0x04007997 RID: 31127
		[MayRequireAnomaly]
		public static HediffDef DeathRefusalSickness;

		// Token: 0x04007998 RID: 31128
		[MayRequireAnomaly]
		public static HediffDef MetalhorrorImplant;

		// Token: 0x04007999 RID: 31129
		[MayRequireAnomaly]
		public static HediffDef MetalhorrorSpeedBoost;

		// Token: 0x0400799A RID: 31130
		[MayRequireAnomaly]
		public static HediffDef PsychicTrance;

		// Token: 0x0400799B RID: 31131
		[MayRequireAnomaly]
		public static HediffDef DuplicateSickness;

		// Token: 0x0400799C RID: 31132
		[MayRequireAnomaly]
		public static HediffDef VoidTouched;

		// Token: 0x0400799D RID: 31133
		[MayRequireAnomaly]
		public static HediffDef MeatHunger;

		// Token: 0x0400799E RID: 31134
		[MayRequireAnomaly]
		public static HediffDef ShardHolder;

		// Token: 0x0400799F RID: 31135
		[MayRequireAnomaly]
		public static HediffDef Inhumanized;

		// Token: 0x040079A0 RID: 31136
		[MayRequireAnomaly]
		public static HediffDef DarknessExposure;

		// Token: 0x040079A1 RID: 31137
		[MayRequireAnomaly]
		public static HediffDef LightExposure;

		// Token: 0x040079A2 RID: 31138
		[MayRequireAnomaly]
		public static HediffDef DisruptorFlash;

		// Token: 0x040079A3 RID: 31139
		[MayRequireOdyssey]
		public static HediffDef VacuumExposure;

		// Token: 0x040079A4 RID: 31140
		[MayRequireOdyssey]
		public static HediffDef PsilocapHigh;

		// Token: 0x040079A5 RID: 31141
		[MayRequireOdyssey]
		public static HediffDef GravNausea;

		// Token: 0x040079A6 RID: 31142
		[MayRequireOdyssey]
		public static HediffDef VolcanicAsh;

		// Token: 0x040079A7 RID: 31143
		public static HediffDef Anesthetic;

		// Token: 0x040079A8 RID: 31144
		public static HediffDef CryptosleepSickness;

		// Token: 0x040079A9 RID: 31145
		public static HediffDef FoodPoisoning;

		// Token: 0x040079AA RID: 31146
		public static HediffDef Pregnant;

		// Token: 0x040079AB RID: 31147
		[MayRequireBiotech]
		public static HediffDef PregnantHuman;

		// Token: 0x040079AC RID: 31148
		[MayRequireBiotech]
		public static HediffDef MorningSickness;

		// Token: 0x040079AD RID: 31149
		[MayRequireBiotech]
		public static HediffDef PregnancyMood;

		// Token: 0x040079AE RID: 31150
		[MayRequireBiotech]
		public static HediffDef PregnancyLabor;

		// Token: 0x040079AF RID: 31151
		[MayRequireBiotech]
		public static HediffDef PregnancyLaborPushing;

		// Token: 0x040079B0 RID: 31152
		[MayRequireBiotech]
		public static HediffDef OvumExtracted;

		// Token: 0x040079B1 RID: 31153
		[MayRequireBiotech]
		public static HediffDef VatLearning;

		// Token: 0x040079B2 RID: 31154
		[MayRequireBiotech]
		public static HediffDef VatGrowing;

		// Token: 0x040079B3 RID: 31155
		public static HediffDef CatatonicBreakdown;

		// Token: 0x040079B4 RID: 31156
		public static HediffDef Scaria;

		// Token: 0x040079B5 RID: 31157
		[MayRequireIdeology]
		public static HediffDef Scarification;

		// Token: 0x040079B6 RID: 31158
		[MayRequireIdeology]
		public static HediffDef NeuralSupercharge;

		// Token: 0x040079B7 RID: 31159
		[MayRequireIdeology]
		public static HediffDef BiosculptingSickness;

		// Token: 0x040079B8 RID: 31160
		[MayRequireRoyalty]
		public static HediffDef PsychicEntropy;

		// Token: 0x040079B9 RID: 31161
		[MayRequireRoyalty]
		public static HediffDef PsychicHangover;

		// Token: 0x040079BA RID: 31162
		[MayRequireRoyalty]
		public static HediffDef PsychicSuppression;

		// Token: 0x040079BB RID: 31163
		[MayRequireRoyalty]
		public static HediffDef Abasia;

		// Token: 0x040079BC RID: 31164
		[MayRequireRoyalty]
		public static HediffDef PsychicLove;

		// Token: 0x040079BD RID: 31165
		public static HediffDef Sterilized;

		// Token: 0x040079BE RID: 31166
		[MayRequireBiotech]
		public static HediffDef Vasectomy;

		// Token: 0x040079BF RID: 31167
		[MayRequireBiotech]
		public static HediffDef ImplantedIUD;

		// Token: 0x040079C0 RID: 31168
		[MayRequireIdeology]
		public static HediffDef WorkFocus;

		// Token: 0x040079C1 RID: 31169
		[MayRequireBiotech]
		public static HediffDef ScanningSickness;

		// Token: 0x040079C2 RID: 31170
		[MayRequireBiotech]
		public static HediffDef PsychicBond;

		// Token: 0x040079C3 RID: 31171
		[MayRequireBiotech]
		public static HediffDef PsychicBondTorn;

		// Token: 0x040079C4 RID: 31172
		public static HediffDef LungRot;

		// Token: 0x040079C5 RID: 31173
		public static HediffDef LungRotExposure;

		// Token: 0x040079C6 RID: 31174
		public static HediffDef CoveredInFirefoam;

		// Token: 0x040079C7 RID: 31175
		[MayRequireAnomaly]
		public static HediffDef BioferriteExtracted;

		// Token: 0x040079C8 RID: 31176
		public static HediffDef Plague;

		// Token: 0x040079C9 RID: 31177
		public static HediffDef WoundInfection;

		// Token: 0x040079CA RID: 31178
		public static HediffDef ScariaInfection;

		// Token: 0x040079CB RID: 31179
		public static HediffDef AlcoholHigh;

		// Token: 0x040079CC RID: 31180
		public static HediffDef Hangover;

		// Token: 0x040079CD RID: 31181
		public static HediffDef DrugOverdose;

		// Token: 0x040079CE RID: 31182
		public static HediffDef WakeUpTolerance;

		// Token: 0x040079CF RID: 31183
		public static HediffDef GoJuiceTolerance;

		// Token: 0x040079D0 RID: 31184
		public static HediffDef Blindness;

		// Token: 0x040079D1 RID: 31185
		public static HediffDef Carcinoma;

		// Token: 0x040079D2 RID: 31186
		public static HediffDef Dementia;

		// Token: 0x040079D3 RID: 31187
		public static HediffDef OrganDecay;

		// Token: 0x040079D4 RID: 31188
		[MayRequireBiotech]
		public static HediffDef DetoxifierLung;

		// Token: 0x040079D5 RID: 31189
		[MayRequireRoyalty]
		public static HediffDef LoveEnhancer;

		// Token: 0x040079D6 RID: 31190
		[MayRequireRoyalty]
		public static HediffDef PsychicAmplifier;

		// Token: 0x040079D7 RID: 31191
		[MayRequireRoyalty]
		public static HediffDef PsychicHarmonizer;

		// Token: 0x040079D8 RID: 31192
		[MayRequireBiotech]
		public static HediffDef MechlinkImplant;

		// Token: 0x040079D9 RID: 31193
		[MayRequireBiotech]
		public static HediffDef SelfShutdown;

		// Token: 0x040079DA RID: 31194
		[MayRequireBiotech]
		public static HediffDef InterruptedDeathrest;

		// Token: 0x040079DB RID: 31195
		[MayRequireBiotech]
		public static HediffDef InfantIllness;

		// Token: 0x040079DC RID: 31196
		[MayRequireBiotech]
		public static HediffDef Stillborn;

		// Token: 0x040079DD RID: 31197
		[MayRequireBiotech]
		public static HediffDef PostpartumExhaustion;

		// Token: 0x040079DE RID: 31198
		[MayRequireBiotech]
		public static HediffDef Lactating;

		// Token: 0x040079DF RID: 31199
		[MayRequireBiotech]
		public static HediffDef RegenerationComa;

		// Token: 0x040079E0 RID: 31200
		[MayRequireBiotech]
		public static HediffDef XenogermLossShock;

		// Token: 0x040079E1 RID: 31201
		[MayRequireBiotech]
		public static HediffDef XenogermReplicating;

		// Token: 0x040079E2 RID: 31202
		[MayRequireBiotech]
		public static HediffDef GeneticDrugNeed;

		// Token: 0x040079E3 RID: 31203
		[MayRequireBiotech]
		public static HediffDef BioStarvation;

		// Token: 0x040079E4 RID: 31204
		[MayRequireBiotech]
		public static HediffDef XenogerminationComa;

		// Token: 0x040079E5 RID: 31205
		[MayRequireBiotech]
		public static HediffDef Deathrest;

		// Token: 0x040079E6 RID: 31206
		[MayRequireBiotech]
		public static HediffDef DeathrestExhaustion;

		// Token: 0x040079E7 RID: 31207
		[MayRequireBiotech]
		public static HediffDef BandNode;

		// Token: 0x040079E8 RID: 31208
		[MayRequireBiotech]
		public static HediffDef HemogenCraving;

		// Token: 0x040079E9 RID: 31209
		[MayRequireAnomaly]
		public static HediffDef HoraxianInvisibility;

		// Token: 0x040079EA RID: 31210
		[MayRequireAnomaly]
		public static HediffDef RevenantHypnosis;

		// Token: 0x040079EB RID: 31211
		[MayRequireAnomaly]
		public static HediffDef AwokenHypnosis;

		// Token: 0x040079EC RID: 31212
		[MayRequireAnomaly]
		public static HediffDef DarkPsychicShock;

		// Token: 0x040079ED RID: 31213
		[MayRequireAnomaly]
		public static HediffDef Tentacle;

		// Token: 0x040079EE RID: 31214
		[MayRequireAnomaly]
		public static HediffDef FleshWhip;

		// Token: 0x040079EF RID: 31215
		[MayRequireAnomaly]
		public static HediffDef OrganDecayUndiagnosedDuplicaton;

		// Token: 0x040079F0 RID: 31216
		[MayRequireAnomaly]
		public static HediffDef CrumblingMindUndiagnosedDuplication;

		// Token: 0x040079F1 RID: 31217
		[MayRequireAnomaly]
		public static HediffDef OrganDecayCreepjoiner;

		// Token: 0x040079F2 RID: 31218
		[MayRequireAnomaly]
		public static HediffDef CrumblingMind;

		// Token: 0x040079F3 RID: 31219
		[MayRequireAnomaly]
		public static HediffDef CrumbledMind;

		// Token: 0x040079F4 RID: 31220
		[MayRequireAnomaly]
		public static HediffDef BlissLobotomy;

		// Token: 0x040079F5 RID: 31221
		[MayRequireAnomaly]
		public static HediffDef PleasurePulse;

		// Token: 0x040079F6 RID: 31222
		[MayRequireAnomaly]
		public static HediffDef NeurosisPulse;

		// Token: 0x040079F7 RID: 31223
		[MayRequireAnomaly]
		public static HediffDef BloodRage;

		// Token: 0x040079F8 RID: 31224
		[MayRequireAnomaly]
		public static HediffDef FrenzyField;

		// Token: 0x040079F9 RID: 31225
		[MayRequireAnomaly]
		public static HediffDef AgonyPulse;

		// Token: 0x040079FA RID: 31226
		[MayRequireAnomaly]
		public static HediffDef CubeInterest;

		// Token: 0x040079FB RID: 31227
		[MayRequireAnomaly]
		public static HediffDef CubeWithdrawal;

		// Token: 0x040079FC RID: 31228
		[MayRequireAnomaly]
		public static HediffDef CubeRage;

		// Token: 0x040079FD RID: 31229
		[MayRequireAnomaly]
		public static HediffDef CubeComa;

		// Token: 0x040079FE RID: 31230
		[MayRequireAnomaly]
		public static HediffDef BrainwipeComa;

		// Token: 0x040079FF RID: 31231
		[MayRequireAnomaly]
		public static HediffDef CorpseTorment;

		// Token: 0x04007A00 RID: 31232
		[MayRequireAnomaly]
		public static HediffDef PsychicallyDead;

		// Token: 0x04007A01 RID: 31233
		[MayRequireAnomaly]
		public static HediffDef Psychophage;

		// Token: 0x04007A02 RID: 31234
		[MayRequireAnomaly]
		public static HediffDef SleepSuppression;

		// Token: 0x04007A03 RID: 31235
		[MayRequireAnomaly]
		public static HediffDef RageSpeed;

		// Token: 0x04007A04 RID: 31236
		[MayRequireAnomaly]
		public static HediffDef AwokenCorpse;

		// Token: 0x04007A05 RID: 31237
		[MayRequireAnomaly]
		public static HediffDef RapidRegeneration;

		// Token: 0x04007A06 RID: 31238
		[MayRequireAnomaly]
		public static HediffDef GhoulBarbs;

		// Token: 0x04007A07 RID: 31239
		[MayRequireAnomaly]
		public static HediffDef GhoulPlating;

		// Token: 0x04007A08 RID: 31240
		[MayRequireAnomaly]
		public static HediffDef AdrenalHeart;

		// Token: 0x04007A09 RID: 31241
		[MayRequireAnomaly]
		public static HediffDef FleshmassStomach;

		// Token: 0x04007A0A RID: 31242
		[MayRequireAnomaly]
		public static HediffDef FleshmassLung;

		// Token: 0x04007A0B RID: 31243
		[MayRequireAnomaly]
		public static HediffDef Metalblood;

		// Token: 0x04007A0C RID: 31244
		[MayRequireOdyssey]
		public static HediffDef SentienceCatalyst;
	}
}
