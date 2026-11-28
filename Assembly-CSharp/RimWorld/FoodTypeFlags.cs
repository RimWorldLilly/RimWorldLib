using System;

namespace RimWorld
{
	// Token: 0x02000EF9 RID: 3833
	[Flags]
	public enum FoodTypeFlags
	{
		// Token: 0x04003871 RID: 14449
		None = 0,
		// Token: 0x04003872 RID: 14450
		VegetableOrFruit = 1,
		// Token: 0x04003873 RID: 14451
		Meat = 2,
		// Token: 0x04003874 RID: 14452
		Fluid = 4,
		// Token: 0x04003875 RID: 14453
		Corpse = 8,
		// Token: 0x04003876 RID: 14454
		Seed = 16,
		// Token: 0x04003877 RID: 14455
		AnimalProduct = 32,
		// Token: 0x04003878 RID: 14456
		Plant = 64,
		// Token: 0x04003879 RID: 14457
		Tree = 128,
		// Token: 0x0400387A RID: 14458
		Meal = 256,
		// Token: 0x0400387B RID: 14459
		Processed = 512,
		// Token: 0x0400387C RID: 14460
		Liquor = 1024,
		// Token: 0x0400387D RID: 14461
		Kibble = 2048,
		// Token: 0x0400387E RID: 14462
		Fungus = 4097,
		// Token: 0x0400387F RID: 14463
		VegetarianAnimal = 7953,
		// Token: 0x04003880 RID: 14464
		VegetarianRoughAnimal = 8017,
		// Token: 0x04003881 RID: 14465
		CarnivoreAnimal = 2826,
		// Token: 0x04003882 RID: 14466
		CarnivoreAnimalStrict = 10,
		// Token: 0x04003883 RID: 14467
		OmnivoreAnimal = 7963,
		// Token: 0x04003884 RID: 14468
		OmnivoreRoughAnimal = 8027,
		// Token: 0x04003885 RID: 14469
		DendrovoreAnimal = 6801,
		// Token: 0x04003886 RID: 14470
		OvivoreAnimal = 2848,
		// Token: 0x04003887 RID: 14471
		OmnivoreHuman = 7999
	}
}
