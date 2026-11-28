using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E6C RID: 7788
	[DefOf]
	public static class SoundDefOf
	{
		// Token: 0x0600CA28 RID: 51752 RVA: 0x004A5B6A File Offset: 0x004A3D6A
		static SoundDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SoundDefOf));
		}

		// Token: 0x04007543 RID: 30019
		public static SoundDef Tick_High;

		// Token: 0x04007544 RID: 30020
		public static SoundDef Tick_Low;

		// Token: 0x04007545 RID: 30021
		public static SoundDef Tick_Tiny;

		// Token: 0x04007546 RID: 30022
		public static SoundDef DragElement;

		// Token: 0x04007547 RID: 30023
		public static SoundDef DropElement;

		// Token: 0x04007548 RID: 30024
		public static SoundDef Crunch;

		// Token: 0x04007549 RID: 30025
		public static SoundDef Click;

		// Token: 0x0400754A RID: 30026
		public static SoundDef ClickReject;

		// Token: 0x0400754B RID: 30027
		public static SoundDef CancelMode;

		// Token: 0x0400754C RID: 30028
		public static SoundDef TabClose;

		// Token: 0x0400754D RID: 30029
		public static SoundDef TabOpen;

		// Token: 0x0400754E RID: 30030
		public static SoundDef Checkbox_TurnedOff;

		// Token: 0x0400754F RID: 30031
		public static SoundDef Checkbox_TurnedOn;

		// Token: 0x04007550 RID: 30032
		public static SoundDef RowTabSelect;

		// Token: 0x04007551 RID: 30033
		public static SoundDef ArchitectCategorySelect;

		// Token: 0x04007552 RID: 30034
		public static SoundDef ExecuteTrade;

		// Token: 0x04007553 RID: 30035
		public static SoundDef FloatMenu_Open;

		// Token: 0x04007554 RID: 30036
		public static SoundDef FloatMenu_Cancel;

		// Token: 0x04007555 RID: 30037
		public static SoundDef DialogBoxAppear;

		// Token: 0x04007556 RID: 30038
		public static SoundDef TutorMessageAppear;

		// Token: 0x04007557 RID: 30039
		public static SoundDef TinyBell;

		// Token: 0x04007558 RID: 30040
		public static SoundDef PageChange;

		// Token: 0x04007559 RID: 30041
		public static SoundDef DragSlider;

		// Token: 0x0400755A RID: 30042
		public static SoundDef DragGoto;

		// Token: 0x0400755B RID: 30043
		public static SoundDef Lesson_Activated;

		// Token: 0x0400755C RID: 30044
		public static SoundDef Lesson_Deactivated;

		// Token: 0x0400755D RID: 30045
		public static SoundDef DraftOn;

		// Token: 0x0400755E RID: 30046
		public static SoundDef DraftOff;

		// Token: 0x0400755F RID: 30047
		public static SoundDef Interact_RemovePaint;

		// Token: 0x04007560 RID: 30048
		public static SoundDef CommsWindow_Open;

		// Token: 0x04007561 RID: 30049
		public static SoundDef CommsWindow_Close;

		// Token: 0x04007562 RID: 30050
		public static SoundDef RadioComms_Ambience;

		// Token: 0x04007563 RID: 30051
		public static SoundDef InfoCard_Open;

		// Token: 0x04007564 RID: 30052
		public static SoundDef InfoCard_Close;

		// Token: 0x04007565 RID: 30053
		public static SoundDef Clock_Stop;

		// Token: 0x04007566 RID: 30054
		public static SoundDef Clock_Normal;

		// Token: 0x04007567 RID: 30055
		public static SoundDef Clock_Fast;

		// Token: 0x04007568 RID: 30056
		public static SoundDef Clock_Superfast;

		// Token: 0x04007569 RID: 30057
		public static SoundDef Quest_Accepted;

		// Token: 0x0400756A RID: 30058
		public static SoundDef Quest_Succeded;

		// Token: 0x0400756B RID: 30059
		public static SoundDef Quest_Concluded;

		// Token: 0x0400756C RID: 30060
		public static SoundDef Quest_Failed;

		// Token: 0x0400756D RID: 30061
		[MayRequireIdeology]
		public static SoundDef DislikedWorkTypeActivated;

		// Token: 0x0400756E RID: 30062
		[MayRequireIdeology]
		public static SoundDef ArchonexusNewColonyAccept;

		// Token: 0x0400756F RID: 30063
		[MayRequireIdeology]
		public static SoundDef GameStartSting_FirstArchonexusCycle;

		// Token: 0x04007570 RID: 30064
		[MayRequireIdeology]
		public static SoundDef GameStartSting_SecondArchonexusCycle;

		// Token: 0x04007571 RID: 30065
		[MayRequireBiotech]
		public static SoundDef StartRecombining;

		// Token: 0x04007572 RID: 30066
		public static SoundDef Mouseover_Standard;

		// Token: 0x04007573 RID: 30067
		public static SoundDef Mouseover_Category;

		// Token: 0x04007574 RID: 30068
		public static SoundDef Mouseover_Command;

		// Token: 0x04007575 RID: 30069
		public static SoundDef Mouseover_ButtonToggle;

		// Token: 0x04007576 RID: 30070
		public static SoundDef Mouseover_Tab;

		// Token: 0x04007577 RID: 30071
		public static SoundDef ThingSelected;

		// Token: 0x04007578 RID: 30072
		public static SoundDef MapSelected;

		// Token: 0x04007579 RID: 30073
		public static SoundDef ColonistSelected;

		// Token: 0x0400757A RID: 30074
		public static SoundDef ColonistOrdered;

		// Token: 0x0400757B RID: 30075
		public static SoundDef LetterArrive;

		// Token: 0x0400757C RID: 30076
		public static SoundDef Designate_DragStandard;

		// Token: 0x0400757D RID: 30077
		public static SoundDef Designate_DragStandard_Changed;

		// Token: 0x0400757E RID: 30078
		public static SoundDef Designate_DragStandard_Changed_NoCam;

		// Token: 0x0400757F RID: 30079
		public static SoundDef Designate_DragZone_Changed;

		// Token: 0x04007580 RID: 30080
		public static SoundDef Designate_DragBuilding;

		// Token: 0x04007581 RID: 30081
		public static SoundDef Designate_DragAreaAdd;

		// Token: 0x04007582 RID: 30082
		public static SoundDef Designate_DragAreaDelete;

		// Token: 0x04007583 RID: 30083
		public static SoundDef Designate_Failed;

		// Token: 0x04007584 RID: 30084
		public static SoundDef Designate_ZoneAdd;

		// Token: 0x04007585 RID: 30085
		public static SoundDef Designate_ZoneAdd_Stockpile;

		// Token: 0x04007586 RID: 30086
		public static SoundDef Designate_ZoneAdd_Dumping;

		// Token: 0x04007587 RID: 30087
		public static SoundDef Designate_ZoneAdd_Growing;

		// Token: 0x04007588 RID: 30088
		public static SoundDef Designate_ZoneAdd_Roof;

		// Token: 0x04007589 RID: 30089
		public static SoundDef Designate_ZoneAdd_AllowedArea;

		// Token: 0x0400758A RID: 30090
		public static SoundDef Designate_ZoneAdd_RemoveSnow;

		// Token: 0x0400758B RID: 30091
		[MayRequireBiotech]
		public static SoundDef Designate_ZoneAdd_RemovePollution;

		// Token: 0x0400758C RID: 30092
		public static SoundDef Designate_ZoneDelete;

		// Token: 0x0400758D RID: 30093
		public static SoundDef Designate_Cancel;

		// Token: 0x0400758E RID: 30094
		public static SoundDef Designate_Haul;

		// Token: 0x0400758F RID: 30095
		public static SoundDef Designate_Mine;

		// Token: 0x04007590 RID: 30096
		public static SoundDef Designate_SmoothSurface;

		// Token: 0x04007591 RID: 30097
		public static SoundDef Designate_PlanRemove;

		// Token: 0x04007592 RID: 30098
		public static SoundDef Designate_PlanAdd;

		// Token: 0x04007593 RID: 30099
		public static SoundDef Designate_Claim;

		// Token: 0x04007594 RID: 30100
		public static SoundDef Designate_Paint;

		// Token: 0x04007595 RID: 30101
		public static SoundDef Designate_RemovePaint;

		// Token: 0x04007596 RID: 30102
		public static SoundDef Designate_Deconstruct;

		// Token: 0x04007597 RID: 30103
		public static SoundDef Designate_Hunt;

		// Token: 0x04007598 RID: 30104
		public static SoundDef Designate_Slaughter;

		// Token: 0x04007599 RID: 30105
		public static SoundDef Designate_PlaceBuilding;

		// Token: 0x0400759A RID: 30106
		public static SoundDef Designate_CutPlants;

		// Token: 0x0400759B RID: 30107
		public static SoundDef Designate_HarvestWood;

		// Token: 0x0400759C RID: 30108
		public static SoundDef Designate_HarvestPlants;

		// Token: 0x0400759D RID: 30109
		public static SoundDef Designate_ReleaseToWild;

		// Token: 0x0400759E RID: 30110
		public static SoundDef Designate_ExtractTree;

		// Token: 0x0400759F RID: 30111
		public static SoundDef Designate_RemoveFloor;

		// Token: 0x040075A0 RID: 30112
		public static SoundDef Designate_RemoveBridge;

		// Token: 0x040075A1 RID: 30113
		public static SoundDef Designate_Tame;

		// Token: 0x040075A2 RID: 30114
		public static SoundDef Designate_ExtractSkull;

		// Token: 0x040075A3 RID: 30115
		[MayRequireBiotech]
		public static SoundDef Designate_Adopt;

		// Token: 0x040075A4 RID: 30116
		public static SoundDef Standard_Drop;

		// Token: 0x040075A5 RID: 30117
		public static SoundDef Standard_Pickup;

		// Token: 0x040075A6 RID: 30118
		public static SoundDef BulletImpact_Ground;

		// Token: 0x040075A7 RID: 30119
		public static SoundDef Pawn_Melee_Punch_HitBuilding_Generic;

		// Token: 0x040075A8 RID: 30120
		public static SoundDef Ambient_AltitudeWind;

		// Token: 0x040075A9 RID: 30121
		public static SoundDef Ambient_Space;

		// Token: 0x040075AA RID: 30122
		public static SoundDef Power_OnSmall;

		// Token: 0x040075AB RID: 30123
		public static SoundDef Power_OffSmall;

		// Token: 0x040075AC RID: 30124
		public static SoundDef Thunder_OnMap;

		// Token: 0x040075AD RID: 30125
		public static SoundDef Thunder_OffMap;

		// Token: 0x040075AE RID: 30126
		public static SoundDef Interact_CleanFilth;

		// Token: 0x040075AF RID: 30127
		public static SoundDef Interact_Sow;

		// Token: 0x040075B0 RID: 30128
		public static SoundDef Interact_ConstructDirt;

		// Token: 0x040075B1 RID: 30129
		public static SoundDef Interact_Tend;

		// Token: 0x040075B2 RID: 30130
		public static SoundDef Interact_BeatFire;

		// Token: 0x040075B3 RID: 30131
		public static SoundDef Interact_Ignite;

		// Token: 0x040075B4 RID: 30132
		public static SoundDef Roof_Start;

		// Token: 0x040075B5 RID: 30133
		public static SoundDef Roof_Finish;

		// Token: 0x040075B6 RID: 30134
		public static SoundDef Finish_Wood;

		// Token: 0x040075B7 RID: 30135
		public static SoundDef Roof_Collapse;

		// Token: 0x040075B8 RID: 30136
		public static SoundDef PsychicPulseGlobal;

		// Token: 0x040075B9 RID: 30137
		public static SoundDef PsychicSootheGlobal;

		// Token: 0x040075BA RID: 30138
		public static SoundDef GeyserSpray;

		// Token: 0x040075BB RID: 30139
		public static SoundDef TurretAcquireTarget;

		// Token: 0x040075BC RID: 30140
		public static SoundDef FlickSwitch;

		// Token: 0x040075BD RID: 30141
		public static SoundDef PlayBilliards;

		// Token: 0x040075BE RID: 30142
		public static SoundDef Building_Complete;

		// Token: 0x040075BF RID: 30143
		public static SoundDef Replant_Complete;

		// Token: 0x040075C0 RID: 30144
		public static SoundDef RawMeat_Eat;

		// Token: 0x040075C1 RID: 30145
		public static SoundDef HissSmall;

		// Token: 0x040075C2 RID: 30146
		public static SoundDef HissJet;

		// Token: 0x040075C3 RID: 30147
		public static SoundDef MetalHitImportant;

		// Token: 0x040075C4 RID: 30148
		public static SoundDef Door_OpenPowered;

		// Token: 0x040075C5 RID: 30149
		public static SoundDef Door_ClosePowered;

		// Token: 0x040075C6 RID: 30150
		public static SoundDef Door_OpenManual;

		// Token: 0x040075C7 RID: 30151
		public static SoundDef Door_CloseManual;

		// Token: 0x040075C8 RID: 30152
		public static SoundDef EnergyShield_AbsorbDamage;

		// Token: 0x040075C9 RID: 30153
		public static SoundDef EnergyShield_Reset;

		// Token: 0x040075CA RID: 30154
		public static SoundDef Pawn_Melee_Punch_HitPawn;

		// Token: 0x040075CB RID: 30155
		public static SoundDef Pawn_Melee_Punch_Miss;

		// Token: 0x040075CC RID: 30156
		public static SoundDef MeleeHit_Unarmed;

		// Token: 0x040075CD RID: 30157
		public static SoundDef Artillery_ShellLoaded;

		// Token: 0x040075CE RID: 30158
		public static SoundDef TechMedicineUsed;

		// Token: 0x040075CF RID: 30159
		public static SoundDef Building_Deconstructed;

		// Token: 0x040075D0 RID: 30160
		public static SoundDef CryptosleepCasket_Accept;

		// Token: 0x040075D1 RID: 30161
		public static SoundDef CryptosleepCasket_Eject;

		// Token: 0x040075D2 RID: 30162
		public static SoundDef TrapSpring;

		// Token: 0x040075D3 RID: 30163
		public static SoundDef TrapArm;

		// Token: 0x040075D4 RID: 30164
		public static SoundDef FireBurning;

		// Token: 0x040075D5 RID: 30165
		public static SoundDef Vomit;

		// Token: 0x040075D6 RID: 30166
		public static SoundDef Roping;

		// Token: 0x040075D7 RID: 30167
		public static SoundDef ResearchStart;

		// Token: 0x040075D8 RID: 30168
		public static SoundDef ThingUninstalled;

		// Token: 0x040075D9 RID: 30169
		public static SoundDef ShipTakeoff;

		// Token: 0x040075DA RID: 30170
		public static SoundDef Corpse_Drop;

		// Token: 0x040075DB RID: 30171
		public static SoundDef Tornado;

		// Token: 0x040075DC RID: 30172
		public static SoundDef Tunnel;

		// Token: 0x040075DD RID: 30173
		public static SoundDef MechanoidsWakeUp;

		// Token: 0x040075DE RID: 30174
		public static SoundDef FlashstormAmbience;

		// Token: 0x040075DF RID: 30175
		public static SoundDef Recipe_Surgery;

		// Token: 0x040075E0 RID: 30176
		public static SoundDef Execute_Cut;

		// Token: 0x040075E1 RID: 30177
		public static SoundDef Recipe_ButcherCorpseMechanoid;

		// Token: 0x040075E2 RID: 30178
		public static SoundDef Explosion_FirefoamPopper;

		// Token: 0x040075E3 RID: 30179
		public static SoundDef Psycast_Skip_Entry;

		// Token: 0x040075E4 RID: 30180
		public static SoundDef EmergeFromWater;

		// Token: 0x040075E5 RID: 30181
		public static SoundDef CocoonDestroyed;

		// Token: 0x040075E6 RID: 30182
		public static SoundDef Crater_FilledIn;

		// Token: 0x040075E7 RID: 30183
		public static SoundDef MechSerumUsed;

		// Token: 0x040075E8 RID: 30184
		public static SoundDef ShipReactor_Startup;

		// Token: 0x040075E9 RID: 30185
		public static SoundDef WindTurbine_Ambience;

		// Token: 0x040075EA RID: 30186
		public static SoundDef Bombardment_PreImpact;

		// Token: 0x040075EB RID: 30187
		[MayRequireIdeology]
		public static SoundDef Archotech_Invoked;

		// Token: 0x040075EC RID: 30188
		[MayRequireIdeology]
		public static SoundDef InsectsWakeUp;

		// Token: 0x040075ED RID: 30189
		[MayRequireRoyalty]
		public static SoundDef PsycastPsychicEffect;

		// Token: 0x040075EE RID: 30190
		[MayRequireRoyalty]
		public static SoundDef PsycastPsychicPulse;

		// Token: 0x040075EF RID: 30191
		[MayRequireRoyalty]
		public static SoundDef Psycast_Skip_Pulse;

		// Token: 0x040075F0 RID: 30192
		[MayRequireRoyalty]
		public static SoundDef Psycast_Skip_Exit;

		// Token: 0x040075F1 RID: 30193
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyOverloaded;

		// Token: 0x040075F2 RID: 30194
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyHyperloaded;

		// Token: 0x040075F3 RID: 30195
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyBrainCharring;

		// Token: 0x040075F4 RID: 30196
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyBrainRoasting;

		// Token: 0x040075F5 RID: 30197
		[MayRequireRoyalty]
		public static SoundDef MeditationGainPsyfocus;

		// Token: 0x040075F6 RID: 30198
		[MayRequireRoyalty]
		public static SoundDef MechClusterDefeated;

		// Token: 0x040075F7 RID: 30199
		[MayRequireRoyalty]
		public static SoundDef TechprintApplied;

		// Token: 0x040075F8 RID: 30200
		[MayRequireRoyalty]
		public static SoundDef Bestowing_Start;

		// Token: 0x040075F9 RID: 30201
		[MayRequireRoyalty]
		public static SoundDef Bestowing_Warmup;

		// Token: 0x040075FA RID: 30202
		[MayRequireRoyalty]
		public static SoundDef Bestowing_Finished;

		// Token: 0x040075FB RID: 30203
		[MayRequireRoyalty]
		public static SoundDef OrbitalStrike_Ordered;

		// Token: 0x040075FC RID: 30204
		[MayRequireRoyalty]
		public static SoundDef Broadshield_Startup;

		// Token: 0x040075FD RID: 30205
		[MayRequireIdeology]
		public static SoundDef Hacking_Started;

		// Token: 0x040075FE RID: 30206
		[MayRequireIdeology]
		public static SoundDef Hacking_InProgress;

		// Token: 0x040075FF RID: 30207
		[MayRequireIdeology]
		public static SoundDef Hacking_Completed;

		// Token: 0x04007600 RID: 30208
		[MayRequireIdeology]
		public static SoundDef Hacking_Suspended;

		// Token: 0x04007601 RID: 30209
		[MayRequireIdeology]
		public static SoundDef Speech_Leader_Male;

		// Token: 0x04007602 RID: 30210
		[MayRequireIdeology]
		public static SoundDef Speech_Leader_Female;

		// Token: 0x04007603 RID: 30211
		[MayRequireIdeology]
		public static SoundDef Relic_Installed;

		// Token: 0x04007604 RID: 30212
		[MayRequireIdeology]
		public static SoundDef Interact_Prune;

		// Token: 0x04007605 RID: 30213
		[MayRequireIdeology]
		public static SoundDef HairCutting;

		// Token: 0x04007606 RID: 30214
		[MayRequireIdeology]
		public static SoundDef Pawn_Dryad_Spawn;

		// Token: 0x04007607 RID: 30215
		[MayRequireIdeology]
		public static SoundDef GauranlenProductionModeSet;

		// Token: 0x04007608 RID: 30216
		[MayRequireIdeology]
		public static SoundDef GauranlenConnectionTorn;

		// Token: 0x04007609 RID: 30217
		[MayRequireIdeology]
		public static SoundDef Pawn_EnterDryadPod;

		// Token: 0x0400760A RID: 30218
		[MayRequireIdeology]
		public static SoundDef AncientRelicRoomReveal;

		// Token: 0x0400760B RID: 30219
		[MayRequireIdeology]
		public static SoundDef AncientRelicTakenAlarm;

		// Token: 0x0400760C RID: 30220
		[MayRequireIdeology]
		public static SoundDef ArchonexusThreatsAwakened_Alarm;

		// Token: 0x0400760D RID: 30221
		[MayRequireIdeology]
		public static SoundDef DuelMusic;

		// Token: 0x0400760E RID: 30222
		[MayRequireIdeology]
		public static SoundDef DanceParty_NoMusic;

		// Token: 0x0400760F RID: 30223
		[MayRequireIdeology]
		public static SoundDef DryadCasteSet;

		// Token: 0x04007610 RID: 30224
		[MayRequireIdeology]
		public static SoundDef Interact_ReleaseSkylantern;

		// Token: 0x04007611 RID: 30225
		[MayRequireIdeology]
		public static SoundDef Interact_RecolorApparel;

		// Token: 0x04007612 RID: 30226
		[MayRequireIdeology]
		public static SoundDef RitualSustainer_Theist;

		// Token: 0x04007613 RID: 30227
		[MayRequireBiotech]
		public static SoundDef DisconnectedMech;

		// Token: 0x04007614 RID: 30228
		[MayRequireBiotech]
		public static SoundDef MechChargerCharging;

		// Token: 0x04007615 RID: 30229
		[MayRequireBiotech]
		public static SoundDef GrowthVat_Open;

		// Token: 0x04007616 RID: 30230
		[MayRequireBiotech]
		public static SoundDef GrowthVat_Close;

		// Token: 0x04007617 RID: 30231
		[MayRequireBiotech]
		public static SoundDef GrowthVat_Working;

		// Token: 0x04007618 RID: 30232
		[MayRequireBiotech]
		public static SoundDef GeneExtractor_Working;

		// Token: 0x04007619 RID: 30233
		[MayRequireBiotech]
		public static SoundDef GeneAssembler_Complete;

		// Token: 0x0400761A RID: 30234
		[MayRequireBiotech]
		public static SoundDef ShieldMech;

		// Token: 0x0400761B RID: 30235
		[MayRequireBiotech]
		public static SoundDef RepairMech_Touch;

		// Token: 0x0400761C RID: 30236
		[MayRequireBiotech]
		public static SoundDef RepairMech_Remote;

		// Token: 0x0400761D RID: 30237
		[MayRequireBiotech]
		public static SoundDef MechChargerStart;

		// Token: 0x0400761E RID: 30238
		[MayRequireBiotech]
		public static SoundDef MechSelfShutdown;

		// Token: 0x0400761F RID: 30239
		[MayRequireBiotech]
		public static SoundDef Toxifier_Pollute;

		// Token: 0x04007620 RID: 30240
		[MayRequireBiotech]
		public static SoundDef MechGestator_Ambience;

		// Token: 0x04007621 RID: 30241
		[MayRequireBiotech]
		public static SoundDef MechGestatorCycle_Started;

		// Token: 0x04007622 RID: 30242
		[MayRequireBiotech]
		public static SoundDef MechGestatorBill_Completed;

		// Token: 0x04007623 RID: 30243
		[MayRequireBiotech]
		public static SoundDef MechGestator_MaterialInserted;

		// Token: 0x04007624 RID: 30244
		[MayRequireBiotech]
		public static SoundDef ControlMech_Complete;

		// Token: 0x04007625 RID: 30245
		[MayRequireBiotech]
		public static SoundDef WastepackDissolution;

		// Token: 0x04007626 RID: 30246
		[MayRequireBiotech]
		public static SoundDef Mechlink_Removed;

		// Token: 0x04007627 RID: 30247
		[MayRequireBiotech]
		public static SoundDef MechlinkCorpseReveal;

		// Token: 0x04007628 RID: 30248
		[MayRequireBiotech]
		public static SoundDef Bloodfeed_Cast;

		// Token: 0x04007629 RID: 30249
		[MayRequireAnomaly]
		public static SoundDef PitGateOpen;

		// Token: 0x0400762A RID: 30250
		[MayRequireAnomaly]
		public static SoundDef Pawn_Shambler_Alert;

		// Token: 0x0400762B RID: 30251
		[MayRequireAnomaly]
		public static SoundDef Pawn_Shambler_Rise;

		// Token: 0x0400762C RID: 30252
		[MayRequireAnomaly]
		public static SoundDef PitGateCollapsing;

		// Token: 0x0400762D RID: 30253
		[MayRequireAnomaly]
		public static SoundDef PitGateCollapsing_End;

		// Token: 0x0400762E RID: 30254
		[MayRequireAnomaly]
		public static SoundDef PitGateGas;

		// Token: 0x0400762F RID: 30255
		[MayRequireAnomaly]
		public static SoundDef UndercaveRumble;

		// Token: 0x04007630 RID: 30256
		[MayRequireAnomaly]
		public static SoundDef UndercaveCollapsingStage1;

		// Token: 0x04007631 RID: 30257
		[MayRequireAnomaly]
		public static SoundDef UndercaveCollapsingStage2;

		// Token: 0x04007632 RID: 30258
		[MayRequireAnomaly]
		public static SoundDef UndercaveCollapsing_End;

		// Token: 0x04007633 RID: 30259
		[MayRequireAnomaly]
		public static SoundDef Pawn_Fleshbeast_EmergeFromPitGate;

		// Token: 0x04007634 RID: 30260
		[MayRequireAnomaly]
		public static SoundDef Pawn_Sightstealer_Howl;

		// Token: 0x04007635 RID: 30261
		[MayRequireAnomaly]
		public static SoundDef Sightstealer_DistantHowl;

		// Token: 0x04007636 RID: 30262
		[MayRequireAnomaly]
		public static SoundDef Sightstealer_DistantHowlLarge;

		// Token: 0x04007637 RID: 30263
		[MayRequireAnomaly]
		public static SoundDef Sightstealer_SummonedHowl;

		// Token: 0x04007638 RID: 30264
		[MayRequireAnomaly]
		public static SoundDef Pawn_Revenant_Revealed;

		// Token: 0x04007639 RID: 30265
		[MayRequireAnomaly]
		public static SoundDef Pawn_Revenant_Stealth;

		// Token: 0x0400763A RID: 30266
		[MayRequireAnomaly]
		public static SoundDef Pawn_Revenant_Hypnotize;

		// Token: 0x0400763B RID: 30267
		[MayRequireAnomaly]
		public static SoundDef Pawn_Revenant_StartledScream;

		// Token: 0x0400763C RID: 30268
		[MayRequireAnomaly]
		public static SoundDef Pawn_SelfResurrection;

		// Token: 0x0400763D RID: 30269
		[MayRequireAnomaly]
		public static SoundDef PsychicBanshee;

		// Token: 0x0400763E RID: 30270
		[MayRequireAnomaly]
		public static SoundDef HateChant_Sighing;

		// Token: 0x0400763F RID: 30271
		[MayRequireAnomaly]
		public static SoundDef Pawn_HateChanter_Call;

		// Token: 0x04007640 RID: 30272
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Ongoing;

		// Token: 0x04007641 RID: 30273
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Ongoing_Human;

		// Token: 0x04007642 RID: 30274
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Complete;

		// Token: 0x04007643 RID: 30275
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Interrupted;

		// Token: 0x04007644 RID: 30276
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Interrupted_Human;

		// Token: 0x04007645 RID: 30277
		[MayRequireAnomaly]
		public static SoundDef ChainToPlatform;

		// Token: 0x04007646 RID: 30278
		[MayRequireAnomaly]
		public static SoundDef ReleaseFromPlatform;

		// Token: 0x04007647 RID: 30279
		[MayRequireAnomaly]
		public static SoundDef ObeliskAmbientStageOne;

		// Token: 0x04007648 RID: 30280
		[MayRequireAnomaly]
		public static SoundDef ObeliskAmbientStageTwo;

		// Token: 0x04007649 RID: 30281
		[MayRequireAnomaly]
		public static SoundDef ObeliskAmbientStageThree;

		// Token: 0x0400764A RID: 30282
		[MayRequireAnomaly]
		public static SoundDef FleshmassHeart_Throb;

		// Token: 0x0400764B RID: 30283
		[MayRequireAnomaly]
		public static SoundDef FleshbeastDigging;

		// Token: 0x0400764C RID: 30284
		[MayRequireAnomaly]
		public static SoundDef Pawn_Chimera_SpeedupRoar;

		// Token: 0x0400764D RID: 30285
		[MayRequireAnomaly]
		public static SoundDef Noctolith_Destroyed;

		// Token: 0x0400764E RID: 30286
		[MayRequireAnomaly]
		public static SoundDef FleshmassAmbience;

		// Token: 0x0400764F RID: 30287
		[MayRequireAnomaly]
		public static SoundDef FleshmassBirth;

		// Token: 0x04007650 RID: 30288
		[MayRequireAnomaly]
		public static SoundDef DarknessDamage;

		// Token: 0x04007651 RID: 30289
		[MayRequireAnomaly]
		public static SoundDef TraversePitGate;

		// Token: 0x04007652 RID: 30290
		[MayRequireAnomaly]
		public static SoundDef Pawn_Awoken_Hypnotize;

		// Token: 0x04007653 RID: 30291
		[MayRequireAnomaly]
		public static SoundDef Pawn_Devourer_Digesting;

		// Token: 0x04007654 RID: 30292
		[MayRequireAnomaly]
		public static SoundDef Pawn_Nociosphere_Arc;

		// Token: 0x04007655 RID: 30293
		[MayRequireAnomaly]
		public static SoundDef Pawn_Nociosphere_Departing;

		// Token: 0x04007656 RID: 30294
		[MayRequireAnomaly]
		public static SoundDef VoidMonolith_InspectLoop;

		// Token: 0x04007657 RID: 30295
		[MayRequireAnomaly]
		public static SoundDef PitBurrow_Collapse;

		// Token: 0x04007658 RID: 30296
		[MayRequireAnomaly]
		public static SoundDef VoidStructure_AmbientPreActivate;

		// Token: 0x04007659 RID: 30297
		[MayRequireAnomaly]
		public static SoundDef VoidStructure_AmbientPostActivate;

		// Token: 0x0400765A RID: 30298
		[MayRequireAnomaly]
		public static SoundDef VoidNode_Ambient;

		// Token: 0x0400765B RID: 30299
		[MayRequireAnomaly]
		public static SoundDef VoidNode_Explode;

		// Token: 0x0400765C RID: 30300
		[MayRequireAnomaly]
		public static SoundDef BioferriteHarvester_Ambient;

		// Token: 0x0400765D RID: 30301
		[MayRequireAnomaly]
		public static SoundDef EntityChainHigh;

		// Token: 0x0400765E RID: 30302
		[MayRequireAnomaly]
		public static SoundDef EntityChainLow;

		// Token: 0x0400765F RID: 30303
		[MayRequireOdyssey]
		public static SoundDef CerebrexCore_Pain;

		// Token: 0x04007660 RID: 30304
		[MayRequireOdyssey]
		public static SoundDef DroneTrapSpring;

		// Token: 0x04007661 RID: 30305
		[MayRequireOdyssey]
		public static SoundDef Interact_CatchFish;

		// Token: 0x04007662 RID: 30306
		[MayRequireOdyssey]
		public static SoundDef Gravship_Launch;

		// Token: 0x04007663 RID: 30307
		[MayRequireOdyssey]
		public static SoundDef Gravship_Engine_Start;

		// Token: 0x04007664 RID: 30308
		[MayRequireOdyssey]
		public static SoundDef Gravship_Engine_Loop;

		// Token: 0x04007665 RID: 30309
		[MayRequireOdyssey]
		public static SoundDef Gravship_Antigrav_Loop;

		// Token: 0x04007666 RID: 30310
		[MayRequireOdyssey]
		public static SoundDef Gravship_Land;

		// Token: 0x04007667 RID: 30311
		[MayRequireOdyssey]
		public static SoundDef Gravship_RetroThruster_Loop;

		// Token: 0x04007668 RID: 30312
		[MayRequireOdyssey]
		public static SoundDef Gravship_RetroThruster_Oneshot_01;

		// Token: 0x04007669 RID: 30313
		[MayRequireOdyssey]
		public static SoundDef Gravship_RetroThruster_Oneshot_02;

		// Token: 0x0400766A RID: 30314
		[MayRequireOdyssey]
		public static SoundDef Gravship_RetroThruster_Oneshot_03;

		// Token: 0x0400766B RID: 30315
		[MayRequireOdyssey]
		public static SoundDef Gravship_LandingServos;

		// Token: 0x0400766C RID: 30316
		[MayRequireOdyssey]
		public static SoundDef Gravship_Touchdown;

		// Token: 0x0400766D RID: 30317
		[MayRequireOdyssey]
		public static SoundDef Gravship_Engine_Stop;

		// Token: 0x0400766E RID: 30318
		public static SoundDef GameStartSting;

		// Token: 0x0400766F RID: 30319
		public static SoundDef PlanetkillerImpact;
	}
}
