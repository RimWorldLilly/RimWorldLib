using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000570 RID: 1392
	public class Graphic_MealVariants : Graphic_StackCount
	{
		// Token: 0x06002944 RID: 10564 RVA: 0x000FD787 File Offset: 0x000FB987
		public override Graphic SubGraphicFor(Thing thing)
		{
			return this.subGraphics[this.SubGraphicTypeIndex(thing) + this.SubGraphicIndexOffset(thing)];
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x000FD7A0 File Offset: 0x000FB9A0
		public int SubGraphicTypeIndex(Thing thing)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return 0;
			}
			FoodKind foodKind = FoodUtility.GetFoodKind(thing);
			if (foodKind == FoodKind.Meat)
			{
				return this.subGraphics.Length / 3;
			}
			if (foodKind != FoodKind.NonMeat)
			{
				return 0;
			}
			return this.subGraphics.Length / 3 * 2;
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000FD7E0 File Offset: 0x000FB9E0
		public int SubGraphicIndexOffset(Thing thing)
		{
			if (thing == null)
			{
				return 0;
			}
			switch (this.subGraphics.Length / 3)
			{
			case 1:
				return 0;
			case 2:
				if (thing.stackCount == 1)
				{
					return 0;
				}
				return 1;
			case 3:
				if (thing.stackCount == 1)
				{
					return 0;
				}
				if (thing.stackCount == thing.def.stackLimit)
				{
					return 2;
				}
				return 1;
			default:
				throw new NotImplementedException("More than 3 different stack size meal graphics per meal type not supported");
			}
		}
	}
}
