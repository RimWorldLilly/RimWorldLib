using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D10 RID: 7440
	public class FloatMenuContext
	{
		// Token: 0x170022F4 RID: 8948
		// (get) Token: 0x0600C064 RID: 49252 RVA: 0x00462E41 File Offset: 0x00461041
		public IntVec3 ClickedCell
		{
			get
			{
				return this.cachedClickedCell;
			}
		}

		// Token: 0x170022F5 RID: 8949
		// (get) Token: 0x0600C065 RID: 49253 RVA: 0x00462E49 File Offset: 0x00461049
		public Room ClickedRoom
		{
			get
			{
				return this.cachedClickedRoom;
			}
		}

		// Token: 0x170022F6 RID: 8950
		// (get) Token: 0x0600C066 RID: 49254 RVA: 0x00462E51 File Offset: 0x00461051
		public Zone ClickedZone
		{
			get
			{
				return this.cachedClickedZone;
			}
		}

		// Token: 0x170022F7 RID: 8951
		// (get) Token: 0x0600C067 RID: 49255 RVA: 0x00462E59 File Offset: 0x00461059
		public List<Thing> ClickedThings
		{
			get
			{
				return this.cachedClickedThings;
			}
		}

		// Token: 0x170022F8 RID: 8952
		// (get) Token: 0x0600C068 RID: 49256 RVA: 0x00462E61 File Offset: 0x00461061
		public List<Pawn> ClickedPawns
		{
			get
			{
				return this.cachedClickedPawns;
			}
		}

		// Token: 0x170022F9 RID: 8953
		// (get) Token: 0x0600C069 RID: 49257 RVA: 0x00462E69 File Offset: 0x00461069
		public bool IsMultiselect
		{
			get
			{
				return this.allSelectedPawns.Count > 1;
			}
		}

		// Token: 0x170022FA RID: 8954
		// (get) Token: 0x0600C06A RID: 49258 RVA: 0x00462E7C File Offset: 0x0046107C
		public Pawn FirstSelectedPawn
		{
			get
			{
				foreach (Pawn pawn in this.allSelectedPawns)
				{
					if (FloatMenuMakerMap.currentProvider == null || FloatMenuMakerMap.currentProvider.SelectedPawnValid(pawn, this))
					{
						return pawn;
					}
				}
				return null;
			}
		}

		// Token: 0x170022FB RID: 8955
		// (get) Token: 0x0600C06B RID: 49259 RVA: 0x00462EE4 File Offset: 0x004610E4
		public IEnumerable<Pawn> ValidSelectedPawns
		{
			get
			{
				FloatMenuContext.<get_ValidSelectedPawns>d__23 <get_ValidSelectedPawns>d__ = new FloatMenuContext.<get_ValidSelectedPawns>d__23(-2);
				<get_ValidSelectedPawns>d__.<>4__this = this;
				return <get_ValidSelectedPawns>d__;
			}
		}

		// Token: 0x0600C06C RID: 49260 RVA: 0x00462EF4 File Offset: 0x004610F4
		public FloatMenuContext(List<Pawn> selectedPawns, Vector3 clickPosition, Map map)
		{
			this.allSelectedPawns = selectedPawns;
			this.clickPosition = clickPosition;
			this.map = map;
			this.cachedClickedCell = IntVec3.FromVector3(clickPosition);
			this.cachedClickedRoom = this.cachedClickedCell.GetRoom(map);
			this.cachedClickedZone = this.cachedClickedCell.GetZone(map);
			this.cachedClickedThings = GenUI.ThingsUnderMouse(clickPosition, 0.8f, TargetingParameters.ForThing(), null);
			this.cachedClickedPawns = GenUI.ThingsUnderMouse(clickPosition, 0.8f, TargetingParameters.ForPawns(), null).OfType<Pawn>().ToList<Pawn>();
			selectedPawns.RemoveAll((Pawn pawn) => !pawn.CanTakeOrder);
		}

		// Token: 0x04006ECB RID: 28363
		public List<Pawn> allSelectedPawns;

		// Token: 0x04006ECC RID: 28364
		public Vector3 clickPosition;

		// Token: 0x04006ECD RID: 28365
		public Map map;

		// Token: 0x04006ECE RID: 28366
		private IntVec3 cachedClickedCell;

		// Token: 0x04006ECF RID: 28367
		private List<Thing> cachedClickedThings;

		// Token: 0x04006ED0 RID: 28368
		private List<Pawn> cachedClickedPawns;

		// Token: 0x04006ED1 RID: 28369
		private Room cachedClickedRoom;

		// Token: 0x04006ED2 RID: 28370
		private Zone cachedClickedZone;
	}
}
