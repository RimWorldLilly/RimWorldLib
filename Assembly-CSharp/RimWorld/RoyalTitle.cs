using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A5 RID: 5541
	public class RoyalTitle : IExposable
	{
		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x060088E5 RID: 35045 RVA: 0x0031868C File Offset: 0x0031688C
		public string Label
		{
			get
			{
				return this.def.GetLabelFor(this.pawn);
			}
		}

		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x060088E6 RID: 35046 RVA: 0x0031869F File Offset: 0x0031689F
		private int RoomRequirementsGracePeriodTicksLeft
		{
			get
			{
				int[] array = new int[3];
				array[0] = 180000 - (GenTicks.TicksGame - this.receivedTick);
				array[1] = MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft;
				return Mathf.Max(array);
			}
		}

		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x060088E7 RID: 35047 RVA: 0x003186C9 File Offset: 0x003168C9
		public float RoomRequirementGracePeriodDaysLeft
		{
			get
			{
				return this.RoomRequirementsGracePeriodTicksLeft.TicksToDays();
			}
		}

		// Token: 0x060088E8 RID: 35048 RVA: 0x003186D6 File Offset: 0x003168D6
		public bool RoomRequirementGracePeriodActive(Pawn pawn)
		{
			return this.RoomRequirementsGracePeriodTicksLeft > 0 && !pawn.IsQuestLodger();
		}

		// Token: 0x060088E9 RID: 35049 RVA: 0x003186EC File Offset: 0x003168EC
		public RoyalTitle()
		{
		}

		// Token: 0x060088EA RID: 35050 RVA: 0x003186FB File Offset: 0x003168FB
		public RoyalTitle(RoyalTitle other)
		{
			this.faction = other.faction;
			this.def = other.def;
			this.receivedTick = other.receivedTick;
			this.pawn = other.pawn;
		}

		// Token: 0x060088EB RID: 35051 RVA: 0x0031873C File Offset: 0x0031693C
		public void RoyalTitleTick(int delta)
		{
			if (this.pawn.IsHashIntervalTick(833, delta) && this.conceited && this.pawn.Spawned && this.pawn.IsFreeColonist && (!this.pawn.IsQuestLodger() || this.pawn.LodgerAllowedDecrees()) && this.def.decreeMtbDays > 0f && this.pawn.Awake() && Rand.MTBEventOccurs(this.def.decreeMtbDays, 60000f, 833f) && (float)(Find.TickManager.TicksGame - this.pawn.royalty.lastDecreeTicks) >= this.def.decreeMinIntervalDays * 60000f)
			{
				this.pawn.royalty.IssueDecree(false, null);
			}
		}

		// Token: 0x060088EC RID: 35052 RVA: 0x00318820 File Offset: 0x00316A20
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.receivedTick, "receivedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.wasInherited, "wasInherited", false, false);
			Scribe_Values.Look<bool>(ref this.conceited, "conceited", false, false);
		}

		// Token: 0x04005463 RID: 21603
		public Faction faction;

		// Token: 0x04005464 RID: 21604
		public RoyalTitleDef def;

		// Token: 0x04005465 RID: 21605
		public Pawn pawn;

		// Token: 0x04005466 RID: 21606
		public int receivedTick = -1;

		// Token: 0x04005467 RID: 21607
		public bool wasInherited;

		// Token: 0x04005468 RID: 21608
		public bool conceited;

		// Token: 0x04005469 RID: 21609
		private const int DecreeCheckInterval = 833;

		// Token: 0x0400546A RID: 21610
		private const int RoomRequirementsGracePeriodTicks = 180000;
	}
}
