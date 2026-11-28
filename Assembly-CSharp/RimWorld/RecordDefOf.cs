using System;

namespace RimWorld
{
	// Token: 0x02001E9F RID: 7839
	[DefOf]
	public static class RecordDefOf
	{
		// Token: 0x0600CA5A RID: 51802 RVA: 0x004A5EBC File Offset: 0x004A40BC
		static RecordDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecordDefOf));
		}

		// Token: 0x04007CEE RID: 31982
		public static RecordDef Kills;

		// Token: 0x04007CEF RID: 31983
		public static RecordDef KillsHumanlikes;

		// Token: 0x04007CF0 RID: 31984
		public static RecordDef KillsAnimals;

		// Token: 0x04007CF1 RID: 31985
		public static RecordDef KillsMechanoids;

		// Token: 0x04007CF2 RID: 31986
		[MayRequireAnomaly]
		public static RecordDef KillsEntities;

		// Token: 0x04007CF3 RID: 31987
		public static RecordDef PawnsDowned;

		// Token: 0x04007CF4 RID: 31988
		public static RecordDef PawnsDownedHumanlikes;

		// Token: 0x04007CF5 RID: 31989
		[MayRequireAnomaly]
		public static RecordDef PawnsDownedEntities;

		// Token: 0x04007CF6 RID: 31990
		public static RecordDef PawnsDownedAnimals;

		// Token: 0x04007CF7 RID: 31991
		public static RecordDef PawnsDownedMechanoids;

		// Token: 0x04007CF8 RID: 31992
		public static RecordDef ShotsFired;

		// Token: 0x04007CF9 RID: 31993
		public static RecordDef Headshots;

		// Token: 0x04007CFA RID: 31994
		public static RecordDef DamageDealt;

		// Token: 0x04007CFB RID: 31995
		public static RecordDef DamageTaken;

		// Token: 0x04007CFC RID: 31996
		public static RecordDef TimesInMentalState;

		// Token: 0x04007CFD RID: 31997
		public static RecordDef TimesOnFire;

		// Token: 0x04007CFE RID: 31998
		public static RecordDef FiresExtinguished;

		// Token: 0x04007CFF RID: 31999
		public static RecordDef OperationsReceived;

		// Token: 0x04007D00 RID: 32000
		public static RecordDef OperationsPerformed;

		// Token: 0x04007D01 RID: 32001
		public static RecordDef TimesTendedTo;

		// Token: 0x04007D02 RID: 32002
		public static RecordDef TimesTendedOther;

		// Token: 0x04007D03 RID: 32003
		public static RecordDef PeopleCaptured;

		// Token: 0x04007D04 RID: 32004
		public static RecordDef PrisonersRecruited;

		// Token: 0x04007D05 RID: 32005
		public static RecordDef PrisonersChatted;

		// Token: 0x04007D06 RID: 32006
		public static RecordDef AnimalsTamed;

		// Token: 0x04007D07 RID: 32007
		public static RecordDef AnimalsSlaughtered;

		// Token: 0x04007D08 RID: 32008
		public static RecordDef MealsCooked;

		// Token: 0x04007D09 RID: 32009
		public static RecordDef ThingsConstructed;

		// Token: 0x04007D0A RID: 32010
		public static RecordDef ThingsInstalled;

		// Token: 0x04007D0B RID: 32011
		public static RecordDef ThingsRepaired;

		// Token: 0x04007D0C RID: 32012
		public static RecordDef ThingsCrafted;

		// Token: 0x04007D0D RID: 32013
		public static RecordDef ThingsHauled;

		// Token: 0x04007D0E RID: 32014
		public static RecordDef PlantsSown;

		// Token: 0x04007D0F RID: 32015
		public static RecordDef PlantsHarvested;

		// Token: 0x04007D10 RID: 32016
		public static RecordDef CellsMined;

		// Token: 0x04007D11 RID: 32017
		public static RecordDef MessesCleaned;

		// Token: 0x04007D12 RID: 32018
		public static RecordDef ResearchPointsResearched;

		// Token: 0x04007D13 RID: 32019
		public static RecordDef CorpsesBuried;

		// Token: 0x04007D14 RID: 32020
		public static RecordDef NutritionEaten;

		// Token: 0x04007D15 RID: 32021
		public static RecordDef BodiesStripped;

		// Token: 0x04007D16 RID: 32022
		public static RecordDef ThingsUninstalled;

		// Token: 0x04007D17 RID: 32023
		public static RecordDef ThingsDeconstructed;

		// Token: 0x04007D18 RID: 32024
		public static RecordDef ArtifactsActivated;

		// Token: 0x04007D19 RID: 32025
		public static RecordDef ContainersOpened;

		// Token: 0x04007D1A RID: 32026
		public static RecordDef SwitchesFlicked;

		// Token: 0x04007D1B RID: 32027
		public static RecordDef TimeAsColonistOrColonyAnimal;

		// Token: 0x04007D1C RID: 32028
		public static RecordDef TimeAsPrisoner;

		// Token: 0x04007D1D RID: 32029
		public static RecordDef TimeAsQuestLodger;

		// Token: 0x04007D1E RID: 32030
		[MayRequireBiotech]
		public static RecordDef TimeAsChildInColony;
	}
}
