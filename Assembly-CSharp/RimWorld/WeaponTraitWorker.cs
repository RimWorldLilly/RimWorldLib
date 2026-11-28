using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001012 RID: 4114
	public class WeaponTraitWorker
	{
		// Token: 0x060067AA RID: 26538 RVA: 0x0024A308 File Offset: 0x00248508
		public virtual void Notify_Bonded(Pawn pawn)
		{
			if (!this.def.bondedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.bondedHediffs.Count; i++)
				{
					pawn.health.AddHediff(this.def.bondedHediffs[i], pawn.health.hediffSet.GetBrain(), null, null);
				}
			}
		}

		// Token: 0x060067AB RID: 26539 RVA: 0x0024A37C File Offset: 0x0024857C
		public virtual void Notify_Equipped(Pawn pawn)
		{
			if (!this.def.equippedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.equippedHediffs.Count; i++)
				{
					pawn.health.AddHediff(this.def.equippedHediffs[i], pawn.health.hediffSet.GetBrain(), null, null);
				}
			}
		}

		// Token: 0x060067AC RID: 26540 RVA: 0x0024A3F0 File Offset: 0x002485F0
		public virtual void Notify_EquipmentLost(Pawn pawn)
		{
			if (!this.def.equippedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.equippedHediffs.Count; i++)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.def.equippedHediffs[i], false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
				}
			}
		}

		// Token: 0x060067AD RID: 26541 RVA: 0x0024A45C File Offset: 0x0024865C
		public virtual void Notify_KilledPawn(Pawn pawn)
		{
			if (this.def.killThought != null && pawn.needs.mood != null)
			{
				Thought_WeaponTrait thought_WeaponTrait = (Thought_WeaponTrait)ThoughtMaker.MakeThought(this.def.killThought);
				thought_WeaponTrait.weapon = pawn.equipment.Primary;
				pawn.needs.mood.thoughts.memories.TryGainMemory(thought_WeaponTrait, null);
			}
		}

		// Token: 0x060067AE RID: 26542 RVA: 0x0024A4C8 File Offset: 0x002486C8
		public virtual void Notify_Unbonded(Pawn pawn)
		{
			if (!this.def.bondedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.bondedHediffs.Count; i++)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.def.bondedHediffs[i], false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
				}
			}
		}

		// Token: 0x060067AF RID: 26543 RVA: 0x0024A534 File Offset: 0x00248734
		public virtual void Notify_OtherWeaponWielded(CompBladelinkWeapon weapon)
		{
		}

		// Token: 0x040041F8 RID: 16888
		public WeaponTraitDef def;
	}
}
