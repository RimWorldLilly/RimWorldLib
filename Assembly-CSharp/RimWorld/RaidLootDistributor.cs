using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B6 RID: 4534
	internal class RaidLootDistributor
	{
		// Token: 0x0600713E RID: 28990 RVA: 0x0027A22C File Offset: 0x0027842C
		public RaidLootDistributor(IncidentParms parms, List<Pawn> allPawns, List<Thing> loot)
		{
			this.parms = parms;
			this.allPawns = allPawns;
			this.loot = loot;
			this.unusedPawns = new List<Pawn>(from x in allPawns
			where !x.RaceProps.Animal
			select x);
		}

		// Token: 0x0600713F RID: 28991 RVA: 0x0027A284 File Offset: 0x00278484
		public void DistributeLoot()
		{
			this.recipient = this.unusedPawns.MaxBy((Pawn p) => p.kindDef.combatPower);
			this.recipientMassGiven = 0f;
			foreach (IGrouping<ThingDef, Thing> grouping in from t in this.loot
			group t by t.def)
			{
				foreach (Thing item in grouping)
				{
					this.DistributeItem(item);
				}
				this.NextRecipient();
			}
		}

		// Token: 0x06007140 RID: 28992 RVA: 0x0027A364 File Offset: 0x00278564
		private void DistributeItem(Thing item)
		{
			int num = item.stackCount;
			int num2 = 0;
			while (num > 0 && num2++ < 5)
			{
				num -= this.TryGiveToRecipient(item, num, false);
				if (num > 0)
				{
					this.NextRecipient();
				}
			}
			if (num > 0)
			{
				this.NextRecipient();
				this.TryGiveToRecipient(item, num, true);
			}
		}

		// Token: 0x06007141 RID: 28993 RVA: 0x0027A3B4 File Offset: 0x002785B4
		private int TryGiveToRecipient(Thing item, int count, bool force = false)
		{
			float num = 10f * Mathf.Max(1f, this.recipient.BodySize) - this.recipientMassGiven;
			float statValue = item.GetStatValue(StatDefOf.Mass, true, -1);
			int num2 = force ? count : Mathf.RoundToInt(Mathf.Clamp(num / statValue, 0f, (float)count));
			if (num2 > 0)
			{
				int num3 = this.recipient.inventory.innerContainer.TryAdd(item, num2, true);
				this.recipientMassGiven += (float)num3 * statValue;
				return num3;
			}
			return 0;
		}

		// Token: 0x06007142 RID: 28994 RVA: 0x0027A43E File Offset: 0x0027863E
		private void NextRecipient()
		{
			this.recipientMassGiven = 0f;
			if (this.unusedPawns.Any<Pawn>())
			{
				this.recipient = this.unusedPawns.Pop<Pawn>();
				return;
			}
			this.recipient = this.allPawns.RandomElement<Pawn>();
		}

		// Token: 0x040047C6 RID: 18374
		private readonly IncidentParms parms;

		// Token: 0x040047C7 RID: 18375
		private readonly List<Pawn> allPawns;

		// Token: 0x040047C8 RID: 18376
		private readonly List<Thing> loot;

		// Token: 0x040047C9 RID: 18377
		private readonly List<Pawn> unusedPawns;

		// Token: 0x040047CA RID: 18378
		private Pawn recipient;

		// Token: 0x040047CB RID: 18379
		private float recipientMassGiven;
	}
}
