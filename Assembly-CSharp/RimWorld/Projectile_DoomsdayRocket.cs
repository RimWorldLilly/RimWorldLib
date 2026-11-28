using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B46 RID: 6982
	public class Projectile_DoomsdayRocket : Projectile
	{
		// Token: 0x1700206F RID: 8303
		// (get) Token: 0x0600B34D RID: 45901 RVA: 0x003F2FC8 File Offset: 0x003F11C8
		public override bool AnimalsFleeImpact
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B34E RID: 45902 RVA: 0x003F2FCC File Offset: 0x003F11CC
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Map map = base.Map;
			base.Impact(hitThing, blockedByShield);
			IntVec3 position = base.Position;
			Map map2 = map;
			float explosionRadius = this.def.projectile.explosionRadius;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing launcher = this.launcher;
			int damageAmount = this.DamageAmount;
			float armorPenetration = this.ArmorPenetration;
			SoundDef explosionSound = null;
			ThingDef equipmentDef = this.equipmentDef;
			ThingDef def = this.def;
			ThingDef filth_Fuel = ThingDefOf.Filth_Fuel;
			GenExplosion.DoExplosion(position, map2, explosionRadius, bomb, launcher, damageAmount, armorPenetration, explosionSound, equipmentDef, def, this.intendedTarget.Thing, filth_Fuel, 0.2f, 1, null, null, 255, false, null, 0f, 1, 0.4f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			CellRect cellRect = CellRect.CenteredOn(base.Position, 5);
			cellRect.ClipInsideMap(map);
			for (int i = 0; i < 3; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				this.DoFireExplosion(randomCell, map, 3.9f);
			}
		}

		// Token: 0x0600B34F RID: 45903 RVA: 0x003F30D0 File Offset: 0x003F12D0
		protected void DoFireExplosion(IntVec3 pos, Map map, float radius)
		{
			GenExplosion.DoExplosion(pos, map, radius, DamageDefOf.Flame, this.launcher, this.DamageAmount, this.ArmorPenetration, null, this.equipmentDef, this.def, this.intendedTarget.Thing, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x04006773 RID: 26483
		private const int ExtraExplosionCount = 3;

		// Token: 0x04006774 RID: 26484
		private const int ExtraExplosionRadius = 5;
	}
}
