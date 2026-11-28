using System;

namespace Verse
{
	// Token: 0x02000294 RID: 660
	public enum ThingRequestGroup : byte
	{
		// Token: 0x04001127 RID: 4391
		Undefined,
		// Token: 0x04001128 RID: 4392
		Nothing,
		// Token: 0x04001129 RID: 4393
		Everything,
		// Token: 0x0400112A RID: 4394
		HaulableEver,
		// Token: 0x0400112B RID: 4395
		HaulableAlways,
		// Token: 0x0400112C RID: 4396
		FoodSource,
		// Token: 0x0400112D RID: 4397
		MechCharger,
		// Token: 0x0400112E RID: 4398
		FoodSourceNotPlantOrTree,
		// Token: 0x0400112F RID: 4399
		Corpse,
		// Token: 0x04001130 RID: 4400
		Blueprint,
		// Token: 0x04001131 RID: 4401
		BuildingArtificial,
		// Token: 0x04001132 RID: 4402
		BuildingFrame,
		// Token: 0x04001133 RID: 4403
		Pawn,
		// Token: 0x04001134 RID: 4404
		PotentialBillGiver,
		// Token: 0x04001135 RID: 4405
		Medicine,
		// Token: 0x04001136 RID: 4406
		Filth,
		// Token: 0x04001137 RID: 4407
		AttackTarget,
		// Token: 0x04001138 RID: 4408
		Weapon,
		// Token: 0x04001139 RID: 4409
		Apparel,
		// Token: 0x0400113A RID: 4410
		Refuelable,
		// Token: 0x0400113B RID: 4411
		Styleable,
		// Token: 0x0400113C RID: 4412
		HaulableEverOrMinifiable,
		// Token: 0x0400113D RID: 4413
		Drug,
		// Token: 0x0400113E RID: 4414
		Shell,
		// Token: 0x0400113F RID: 4415
		HarvestablePlant,
		// Token: 0x04001140 RID: 4416
		Fire,
		// Token: 0x04001141 RID: 4417
		Bed,
		// Token: 0x04001142 RID: 4418
		Plant,
		// Token: 0x04001143 RID: 4419
		NonStumpPlant,
		// Token: 0x04001144 RID: 4420
		WasteProducer,
		// Token: 0x04001145 RID: 4421
		Book,
		// Token: 0x04001146 RID: 4422
		HoldingPlatformTarget,
		// Token: 0x04001147 RID: 4423
		PsychicRitualSpot,
		// Token: 0x04001148 RID: 4424
		EntityHolder,
		// Token: 0x04001149 RID: 4425
		SubstructureFootprint,
		// Token: 0x0400114A RID: 4426
		PassengerShuttle,
		// Token: 0x0400114B RID: 4427
		Fish,
		// Token: 0x0400114C RID: 4428
		Construction,
		// Token: 0x0400114D RID: 4429
		HasGUIOverlay,
		// Token: 0x0400114E RID: 4430
		MinifiedThing,
		// Token: 0x0400114F RID: 4431
		Grave,
		// Token: 0x04001150 RID: 4432
		Art,
		// Token: 0x04001151 RID: 4433
		ThingHolder,
		// Token: 0x04001152 RID: 4434
		ActiveTransporter,
		// Token: 0x04001153 RID: 4435
		Transporter,
		// Token: 0x04001154 RID: 4436
		LongRangeMineralScanner,
		// Token: 0x04001155 RID: 4437
		AffectsSky,
		// Token: 0x04001156 RID: 4438
		WindSource,
		// Token: 0x04001157 RID: 4439
		AlwaysFlee,
		// Token: 0x04001158 RID: 4440
		ResearchBench,
		// Token: 0x04001159 RID: 4441
		Facility,
		// Token: 0x0400115A RID: 4442
		AffectedByFacilities,
		// Token: 0x0400115B RID: 4443
		CreatesInfestations,
		// Token: 0x0400115C RID: 4444
		WithCustomRectForSelector,
		// Token: 0x0400115D RID: 4445
		ProjectileInterceptor,
		// Token: 0x0400115E RID: 4446
		ConditionCauser,
		// Token: 0x0400115F RID: 4447
		MusicalInstrument,
		// Token: 0x04001160 RID: 4448
		MusicSource,
		// Token: 0x04001161 RID: 4449
		Throne,
		// Token: 0x04001162 RID: 4450
		FoodDispenser,
		// Token: 0x04001163 RID: 4451
		Projectile,
		// Token: 0x04001164 RID: 4452
		Chunk,
		// Token: 0x04001165 RID: 4453
		MeditationFocus,
		// Token: 0x04001166 RID: 4454
		Seed,
		// Token: 0x04001167 RID: 4455
		DryadSpawner,
		// Token: 0x04001168 RID: 4456
		Studiable,
		// Token: 0x04001169 RID: 4457
		Suppressable,
		// Token: 0x0400116A RID: 4458
		ActionDelay,
		// Token: 0x0400116B RID: 4459
		MechGestator,
		// Token: 0x0400116C RID: 4460
		GenepackHolder,
		// Token: 0x0400116D RID: 4461
		Dissolving,
		// Token: 0x0400116E RID: 4462
		SubcoreScanner,
		// Token: 0x0400116F RID: 4463
		BossgroupCaller,
		// Token: 0x04001170 RID: 4464
		WakeUpDormant,
		// Token: 0x04001171 RID: 4465
		Atomizer,
		// Token: 0x04001172 RID: 4466
		PowerTrader,
		// Token: 0x04001173 RID: 4467
		MapPortal,
		// Token: 0x04001174 RID: 4468
		BuildingGroundSpawner,
		// Token: 0x04001175 RID: 4469
		Hackable,
		// Token: 0x04001176 RID: 4470
		Door,
		// Token: 0x04001177 RID: 4471
		CostProvider,
		// Token: 0x04001178 RID: 4472
		ApparelSource
	}
}
