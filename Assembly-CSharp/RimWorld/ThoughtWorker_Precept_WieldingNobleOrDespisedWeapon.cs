using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC3 RID: 3523
	public class ThoughtWorker_Precept_WieldingNobleOrDespisedWeapon : ThoughtWorker_Precept
	{
		// Token: 0x06006054 RID: 24660 RVA: 0x0022F154 File Offset: 0x0022D354
		public override string PostProcessLabel(Pawn p, string label)
		{
			Pawn_EquipmentTracker equipment = p.equipment;
			ThingWithComps thingWithComps = (equipment != null) ? equipment.Primary : null;
			if (thingWithComps == null)
			{
				return string.Empty;
			}
			return label.Formatted(thingWithComps.Named("WEAPON"));
		}

		// Token: 0x06006055 RID: 24661 RVA: 0x0022F194 File Offset: 0x0022D394
		public override string PostProcessDescription(Pawn p, string description)
		{
			Pawn_EquipmentTracker equipment = p.equipment;
			ThingWithComps thingWithComps = (equipment != null) ? equipment.Primary : null;
			if (thingWithComps == null)
			{
				return string.Empty;
			}
			return description.Formatted(thingWithComps.Named("WEAPON"));
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x0022F1D4 File Offset: 0x0022D3D4
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Ideo != null)
			{
				Pawn_EquipmentTracker equipment = p.equipment;
				if (((equipment != null) ? equipment.Primary : null) != null)
				{
					IdeoWeaponDisposition dispositionForWeapon = p.Ideo.GetDispositionForWeapon(p.equipment.Primary.def);
					if (dispositionForWeapon != IdeoWeaponDisposition.None)
					{
						if (dispositionForWeapon == IdeoWeaponDisposition.Noble)
						{
							return ThoughtState.ActiveAtStage(0);
						}
						if (dispositionForWeapon == IdeoWeaponDisposition.Despised)
						{
							return ThoughtState.ActiveAtStage(1);
						}
					}
					return ThoughtState.Inactive;
				}
			}
			return false;
		}
	}
}
