using System;

namespace RimWorld
{
	// Token: 0x02000E57 RID: 3671
	public class Thought_WeaponTraitNotEquipped : Thought_WeaponTrait
	{
		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x060061BF RID: 25023 RVA: 0x00233E67 File Offset: 0x00232067
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.pawn.equipment.Primary == this.weapon;
			}
		}
	}
}
