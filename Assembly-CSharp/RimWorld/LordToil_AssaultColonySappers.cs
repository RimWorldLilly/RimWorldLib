using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC6 RID: 3270
	public class LordToil_AssaultColonySappers : LordToil
	{
		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x06005CC3 RID: 23747 RVA: 0x00220DD1 File Offset: 0x0021EFD1
		private LordToilData_AssaultColonySappers Data
		{
			get
			{
				return (LordToilData_AssaultColonySappers)this.data;
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x06005CC4 RID: 23748 RVA: 0x00220DDE File Offset: 0x0021EFDE
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06005CC5 RID: 23749 RVA: 0x00220DE1 File Offset: 0x0021EFE1
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005CC6 RID: 23750 RVA: 0x00220DE4 File Offset: 0x0021EFE4
		public LordToil_AssaultColonySappers()
		{
			this.data = new LordToilData_AssaultColonySappers();
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x00220DF7 File Offset: 0x0021EFF7
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x00220E0C File Offset: 0x0021F00C
		public override void UpdateAllDuties()
		{
			if (!this.Data.sapperDest.IsValid && this.lord.ownedPawns.Any<Pawn>())
			{
				this.Data.sapperDest = GenAI.RandomRaidDest(this.lord.ownedPawns[0].PositionHeld, base.Map);
			}
			List<Pawn> list = null;
			if (this.Data.sapperDest.IsValid)
			{
				list = new List<Pawn>();
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (SappersUtility.IsGoodSapper(pawn))
					{
						list.Add(pawn);
					}
				}
				if (list.Count == 0 && this.lord.ownedPawns.Count >= 2)
				{
					Pawn pawn2 = null;
					int num = 0;
					for (int j = 0; j < this.lord.ownedPawns.Count; j++)
					{
						if (SappersUtility.IsGoodBackupSapper(this.lord.ownedPawns[j]))
						{
							int level = this.lord.ownedPawns[j].skills.GetSkill(SkillDefOf.Mining).Level;
							if (pawn2 == null || level > num)
							{
								pawn2 = this.lord.ownedPawns[j];
								num = level;
							}
						}
					}
					if (pawn2 != null)
					{
						list.Add(pawn2);
					}
				}
			}
			for (int k = 0; k < this.lord.ownedPawns.Count; k++)
			{
				Pawn pawn3 = this.lord.ownedPawns[k];
				if (list != null && list.Contains(pawn3))
				{
					pawn3.mindState.duty = new PawnDuty(DutyDefOf.Sapper, this.Data.sapperDest, -1f);
				}
				else if (!list.NullOrEmpty<Pawn>())
				{
					float randomInRange;
					if (pawn3.equipment != null && pawn3.equipment.Primary != null && pawn3.equipment.Primary.def.IsRangedWeapon)
					{
						randomInRange = LordToil_AssaultColonySappers.EscortRadiusRanged.RandomInRange;
					}
					else
					{
						randomInRange = LordToil_AssaultColonySappers.EscortRadiusMelee.RandomInRange;
					}
					pawn3.mindState.duty = new PawnDuty(DutyDefOf.Escort, list.RandomElement<Pawn>(), randomInRange);
				}
				else
				{
					pawn3.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
			}
		}

		// Token: 0x06005CC9 RID: 23753 RVA: 0x0022107C File Offset: 0x0021F27C
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.Data.sapperDest = IntVec3.Invalid;
			this.UpdateAllDuties();
		}

		// Token: 0x040034E2 RID: 13538
		private static readonly FloatRange EscortRadiusRanged = new FloatRange(15f, 19f);

		// Token: 0x040034E3 RID: 13539
		private static readonly FloatRange EscortRadiusMelee = new FloatRange(23f, 26f);
	}
}
