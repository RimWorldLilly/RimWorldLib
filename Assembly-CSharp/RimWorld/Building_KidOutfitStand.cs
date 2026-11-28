using System;

namespace RimWorld
{
	// Token: 0x02001A7A RID: 6778
	public class Building_KidOutfitStand : Building_OutfitStand
	{
		// Token: 0x17001E39 RID: 7737
		// (get) Token: 0x0600AAEC RID: 43756 RVA: 0x003C825D File Offset: 0x003C645D
		protected override BodyTypeDef BodyTypeDefForRendering { get; } = BodyTypeDefOf.Child;

		// Token: 0x17001E3A RID: 7738
		// (get) Token: 0x0600AAED RID: 43757 RVA: 0x003C8265 File Offset: 0x003C6465
		protected override float WeaponDrawDistanceFactor
		{
			get
			{
				return 0.55f;
			}
		}
	}
}
