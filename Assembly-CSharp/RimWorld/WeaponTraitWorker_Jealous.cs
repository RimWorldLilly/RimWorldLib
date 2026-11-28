using System;

namespace RimWorld
{
	// Token: 0x02001014 RID: 4116
	public class WeaponTraitWorker_Jealous : WeaponTraitWorker
	{
		// Token: 0x060067B3 RID: 26547 RVA: 0x0024A564 File Offset: 0x00248764
		public override void Notify_OtherWeaponWielded(CompBladelinkWeapon weapon)
		{
			if (weapon.CodedPawn == null || weapon.CodedPawn.Dead || weapon.CodedPawn.needs.mood == null)
			{
				return;
			}
			Thought_WeaponTrait thought_WeaponTrait = (Thought_WeaponTrait)ThoughtMaker.MakeThought(ThoughtDefOf.JealousRage);
			thought_WeaponTrait.weapon = weapon.parent;
			weapon.CodedPawn.needs.mood.thoughts.memories.TryGainMemory(thought_WeaponTrait, null);
		}
	}
}
