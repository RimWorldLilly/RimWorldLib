using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001A94 RID: 6804
	public class Building_AncientCryptosleepCasket : Building_CryptosleepCasket
	{
		// Token: 0x0600ABAB RID: 43947 RVA: 0x003C9E37 File Offset: 0x003C8037
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
		}

		// Token: 0x0600ABAC RID: 43948 RVA: 0x003C9E54 File Offset: 0x003C8054
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (!this.contentsKnown && this.innerContainer.Count > 0 && dinfo.Def.harmsHealth && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				bool flag = false;
				using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)this.innerContainer).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Pawn)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					this.EjectContents();
				}
			}
			absorbed = false;
		}

		// Token: 0x0600ABAD RID: 43949 RVA: 0x003C9EF8 File Offset: 0x003C80F8
		public override void EjectContents()
		{
			bool contentsKnown = this.contentsKnown;
			List<Thing> list = null;
			if (!contentsKnown)
			{
				list = new List<Thing>();
				list.AddRange(this.innerContainer);
				list.AddRange(this.UnopenedCasketsInGroup().SelectMany((Building_AncientCryptosleepCasket c) => c.innerContainer));
				list.RemoveDuplicates(null);
			}
			base.EjectContents();
			if (this.ClaimableBy(Faction.OfPlayer))
			{
				this.SetFaction(null, null);
			}
			if (!contentsKnown)
			{
				ThingDef filth_Slime = ThingDefOf.Filth_Slime;
				FilthMaker.TryMakeFilth(base.Position, base.Map, filth_Slime, Rand.Range(8, 12), FilthSourceFlags.None, true);
				foreach (Building_AncientCryptosleepCasket building_AncientCryptosleepCasket in this.UnopenedCasketsInGroup())
				{
					building_AncientCryptosleepCasket.contentsKnown = true;
					building_AncientCryptosleepCasket.EjectContents();
				}
				IEnumerable<Pawn> enumerable = from p in list.OfType<Pawn>().ToList<Pawn>()
				where p.RaceProps.Humanlike && p.GetLord() == null && p.Faction == Faction.OfAncientsHostile
				select p;
				if (enumerable.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_AssaultColony(Faction.OfAncientsHostile, false, true, false, false, false, false, false), base.Map, enumerable);
				}
			}
		}

		// Token: 0x0600ABAE RID: 43950 RVA: 0x003CA040 File Offset: 0x003C8240
		private IEnumerable<Building_AncientCryptosleepCasket> UnopenedCasketsInGroup()
		{
			Building_AncientCryptosleepCasket.<UnopenedCasketsInGroup>d__4 <UnopenedCasketsInGroup>d__ = new Building_AncientCryptosleepCasket.<UnopenedCasketsInGroup>d__4(-2);
			<UnopenedCasketsInGroup>d__.<>4__this = this;
			return <UnopenedCasketsInGroup>d__;
		}

		// Token: 0x040063EF RID: 25583
		public int groupID = -1;
	}
}
