using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200178F RID: 6031
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		// Token: 0x06009642 RID: 38466 RVA: 0x003693DF File Offset: 0x003675DF
		public override void Randomize()
		{
			this.radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
			this.damage = this.PossibleDamageDefs().RandomElement<DamageDef>();
		}

		// Token: 0x06009643 RID: 38467 RVA: 0x00369406 File Offset: 0x00367606
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		// Token: 0x06009644 RID: 38468 RVA: 0x00369434 File Offset: 0x00367634
		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(this.damage.label, this.radius.ToString());
		}

		// Token: 0x06009645 RID: 38469 RVA: 0x00369468 File Offset: 0x00367668
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect.TopHalf(), "radius".Translate(), ref this.radius, ref this.radiusBuf, 0f, 1E+09f);
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.damage.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<DamageDef>(this.PossibleDamageDefs(), (DamageDef d) => d.LabelCap, (DamageDef d) => delegate()
				{
					this.damage = d;
				});
			}
		}

		// Token: 0x06009646 RID: 38470 RVA: 0x00369518 File Offset: 0x00367718
		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			}
		}

		// Token: 0x06009647 RID: 38471 RVA: 0x003695AA File Offset: 0x003677AA
		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			return new ScenPart_OnPawnDeathExplode.<PossibleDamageDefs>d__8(-2);
		}

		// Token: 0x06009648 RID: 38472 RVA: 0x003695B3 File Offset: 0x003677B3
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.damage != null) ? this.damage.GetHashCode() : 0) ^ this.radius.GetHashCode();
		}

		// Token: 0x04005A3E RID: 23102
		private float radius = 5.9f;

		// Token: 0x04005A3F RID: 23103
		private DamageDef damage;

		// Token: 0x04005A40 RID: 23104
		private string radiusBuf;
	}
}
