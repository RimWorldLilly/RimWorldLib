using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B47 RID: 6983
	public class Projectile_Liquid : Projectile
	{
		// Token: 0x17002070 RID: 8304
		// (get) Token: 0x0600B351 RID: 45905 RVA: 0x003F3171 File Offset: 0x003F1371
		public override Material DrawMat
		{
			get
			{
				return this.materialResolved;
			}
		}

		// Token: 0x0600B352 RID: 45906 RVA: 0x003F317C File Offset: 0x003F137C
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			this.DoImpact(hitThing, base.Position);
			if (!blockedByShield && !this.def.projectile.soundImpact.NullOrUndefined())
			{
				this.def.projectile.soundImpact.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
			}
			for (int i = 0; i < this.def.projectile.numExtraHitCells; i++)
			{
				IntVec3 intVec = base.Position + GenAdj.AdjacentCellsAndInside[i];
				if (intVec.InBounds(base.Map))
				{
					this.DoImpact(hitThing, intVec);
				}
			}
			base.Impact(hitThing, blockedByShield);
		}

		// Token: 0x0600B353 RID: 45907 RVA: 0x003F3224 File Offset: 0x003F1424
		private void DoImpact(Thing hitThing, IntVec3 cell)
		{
			if (this.def.projectile.filth != null && this.def.projectile.filthCount.TrueMax > 0 && Rand.Chance(this.def.projectile.filthChance) && !cell.Filled(base.Map))
			{
				FilthMaker.TryMakeFilth(cell, base.Map, this.def.projectile.filth, this.def.projectile.filthCount.RandomInRange, FilthSourceFlags.None, true);
			}
			TerrainDef terrain = cell.GetTerrain(base.Map);
			Frame firstThing = cell.GetFirstThing(base.Map);
			Plant plant = cell.GetPlant(base.Map);
			if (this.def.projectile.spawnTerrain != null && Rand.Chance(this.def.projectile.terrainChance) && (this.def.projectile.terrainReplacesFloors || terrain.natural || !terrain.IsFloor) && (this.def.projectile.terrainReplacesFloors || !(((firstThing != null) ? firstThing.def.entityDefToBuild : null) is TerrainDef)) && (plant == null || plant.def.plant.treeCategory != TreeCategory.Super) && GenConstruct.CanBuildOnTerrain(this.def.projectile.spawnTerrain, cell, base.Map, Rot4.North, null, null))
			{
				if (firstThing != null)
				{
					firstThing.Destroy(DestroyMode.Vanish);
				}
				base.Map.terrainGrid.SetTerrain(cell, this.def.projectile.spawnTerrain);
			}
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (!(thing is Mote) && !(thing is Filth) && thing != hitThing)
				{
					if (this.preventFriendlyFire)
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null && !pawn.HostileTo(this.launcher))
						{
							goto IL_274;
						}
					}
					Find.BattleLog.Add(new BattleLogEntry_RangedImpact(this.launcher, thing, thing, this.equipmentDef, this.def, this.targetCoverDef));
					DamageInfo dinfo = new DamageInfo(base.DamageDef, (float)this.def.projectile.GetDamageAmount(null, null), this.def.projectile.GetArmorPenetration(null, null), -1f, this.launcher, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
					thing.TakeDamage(dinfo);
				}
				IL_274:;
			}
		}

		// Token: 0x0600B354 RID: 45908 RVA: 0x003F34B8 File Offset: 0x003F16B8
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.materialResolved == null)
			{
				this.materialResolved = this.def.DrawMatSingle;
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x04006775 RID: 26485
		private Material materialResolved;
	}
}
