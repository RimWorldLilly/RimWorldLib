using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.Planet
{
	// Token: 0x02002362 RID: 9058
	[StaticConstructorOnStartup]
	public class FormCaravanComp : WorldObjectComp
	{
		// Token: 0x17002774 RID: 10100
		// (get) Token: 0x0600E083 RID: 57475 RVA: 0x005379E2 File Offset: 0x00535BE2
		public WorldObjectCompProperties_FormCaravan Props
		{
			get
			{
				return (WorldObjectCompProperties_FormCaravan)this.props;
			}
		}

		// Token: 0x17002775 RID: 10101
		// (get) Token: 0x0600E084 RID: 57476 RVA: 0x005379EF File Offset: 0x00535BEF
		private MapParent MapParent
		{
			get
			{
				return (MapParent)this.parent;
			}
		}

		// Token: 0x17002776 RID: 10102
		// (get) Token: 0x0600E085 RID: 57477 RVA: 0x005379FC File Offset: 0x00535BFC
		public bool Reform
		{
			get
			{
				return !this.MapParent.HasMap || !this.MapParent.Map.IsPlayerHome;
			}
		}

		// Token: 0x17002777 RID: 10103
		// (get) Token: 0x0600E086 RID: 57478 RVA: 0x00537A20 File Offset: 0x00535C20
		public bool CanFormOrReformCaravanNow
		{
			get
			{
				MapParent mapParent = this.MapParent;
				if (!mapParent.HasMap)
				{
					return false;
				}
				if (this.Reform)
				{
					if (this.AnyActiveThreatNow)
					{
						return false;
					}
					List<Pawn> freeColonistsSpawned = mapParent.Map.mapPawns.FreeColonistsSpawned;
					for (int i = 0; i < freeColonistsSpawned.Count; i++)
					{
						MentalState mentalState = freeColonistsSpawned[i].MentalState;
						if (mentalState != null && mentalState.def.category == MentalStateCategory.Aggro)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17002778 RID: 10104
		// (get) Token: 0x0600E087 RID: 57479 RVA: 0x00537A97 File Offset: 0x00535C97
		public bool AnyActiveThreatNow
		{
			get
			{
				return this.MapParent.HasMap && GenHostility.AnyHostileActiveThreatToPlayer(this.MapParent.Map, true, !this.parent.CanReformFoggedEnemies);
			}
		}

		// Token: 0x17002779 RID: 10105
		// (get) Token: 0x0600E088 RID: 57480 RVA: 0x00537AC8 File Offset: 0x00535CC8
		public bool AnyUnexploredFoggedRooms
		{
			get
			{
				if (this.foggedRoomsCheckRect == null)
				{
					return false;
				}
				MapParent mapParent = this.MapParent;
				if (!mapParent.HasMap)
				{
					return false;
				}
				IReadOnlyList<Room> allRooms = mapParent.Map.regionGrid.AllRooms;
				CellRect value = this.foggedRoomsCheckRect.Value;
				for (int i = 0; i < allRooms.Count; i++)
				{
					if (allRooms[i].Fogged && allRooms[i].ProperRoom)
					{
						foreach (IntVec3 c in allRooms[i].Cells)
						{
							if (value.Contains(c))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600E089 RID: 57481 RVA: 0x00537B98 File Offset: 0x00535D98
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			bool anyActiveThreatNow = this.AnyActiveThreatNow;
			if (!anyActiveThreatNow && this.anyActiveThreatLastTick && this.Reform && this.CanFormOrReformCaravanNow)
			{
				if (this.AnyUnexploredFoggedRooms)
				{
					Messages.Message("MessageCanReformCaravanNowNoMoreEnemiesButUnexploredAreas".Translate(), new LookTargets(this.parent), MessageTypeDefOf.SituationResolved, false);
				}
				else
				{
					Messages.Message("MessageCanReformCaravanNowNoMoreEnemies".Translate(), new LookTargets(this.parent), MessageTypeDefOf.NeutralEvent, false);
				}
			}
			this.anyActiveThreatLastTick = anyActiveThreatNow;
		}

		// Token: 0x0600E08A RID: 57482 RVA: 0x00537C2C File Offset: 0x00535E2C
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.anyActiveThreatLastTick, "anyActiveThreatLastTick", false, false);
			Scribe_Values.Look<CellRect?>(ref this.foggedRoomsCheckRect, "foggedRoomsCheckRect", null, false);
		}

		// Token: 0x0600E08B RID: 57483 RVA: 0x00537C68 File Offset: 0x00535E68
		public bool CanReformNow()
		{
			MapParent mapParent = (MapParent)this.parent;
			return this.MapParent.HasMap && this.Reform && this.CanFormOrReformCaravanNow && mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0;
		}

		// Token: 0x0600E08C RID: 57484 RVA: 0x00537CB3 File Offset: 0x00535EB3
		public override IEnumerable<Gizmo> GetGizmos()
		{
			FormCaravanComp.<GetGizmos>d__18 <GetGizmos>d__ = new FormCaravanComp.<GetGizmos>d__18(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x04009007 RID: 36871
		public CellRect? foggedRoomsCheckRect;

		// Token: 0x04009008 RID: 36872
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x04009009 RID: 36873
		private bool anyActiveThreatLastTick;
	}
}
